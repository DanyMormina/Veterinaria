using Microsoft.EntityFrameworkCore;
using Veterinaria.Domain.Comunes;
using Veterinaria.Domain.Dtos;
using Veterinaria.Domain.Entidades;
using Veterinaria.Infrastructure;

namespace Veterinaria.Controllers.Controladores;

public class TratamientoControlador(ContextoVeterinaria context)
{
    public async Task<Resultado<IEnumerable<TratamientoRespuestaDto>>> ObtenerTodosAsync()
    {
        try
        {
            var tratamientos = await ConsultaBase().ToListAsync();
            return Resultado<IEnumerable<TratamientoRespuestaDto>>.Exito(tratamientos);
        }
        catch (Exception ex)
        {
            return Resultado<IEnumerable<TratamientoRespuestaDto>>.Falla($"Error interno al obtener tratamientos: {ex.Message}");
        }
    }

    public async Task<Resultado<TratamientoRespuestaDto>> ObtenerPorIdAsync(long id)
    {
        try
        {
            if (id <= 0)
                return Resultado<TratamientoRespuestaDto>.Falla("El identificador del tratamiento debe ser mayor a cero.");

            var tratamiento = await ConsultaBase().FirstOrDefaultAsync(t => t.Id == id);
            if (tratamiento is null)
                return Resultado<TratamientoRespuestaDto>.Falla($"No se encontró el tratamiento con ID {id}.");

            return Resultado<TratamientoRespuestaDto>.Exito(tratamiento);
        }
        catch (Exception ex)
        {
            return Resultado<TratamientoRespuestaDto>.Falla($"Error interno al obtener el tratamiento: {ex.Message}");
        }
    }

    public async Task<Resultado<long>> CrearAsync(TratamientoSolicitudDto solicitud)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(solicitud.Descripcion))
                return Resultado<long>.Falla("La descripción del tratamiento es obligatoria.");

            var errorDatos = ValidarDatosTratamiento(solicitud);
            if (errorDatos is not null)
                return Resultado<long>.Falla(errorDatos);

            var entidad = new Tratamiento
            {
                TipoTratamiento = string.IsNullOrWhiteSpace(solicitud.TipoTratamiento) ? "General" : solicitud.TipoTratamiento.Trim(),
                Descripcion = solicitud.Descripcion.Trim(),
                Dosis = string.IsNullOrWhiteSpace(solicitud.Dosis) ? null : solicitud.Dosis.Trim(),
                Precio = solicitud.Precio,
                UsaStock = solicitud.UsaStock,
                Stock = solicitud.UsaStock ? Math.Max(0, solicitud.Stock) : 0,
                Activo = true
            };

            context.Tratamientos.Add(entidad);
            await context.SaveChangesAsync();
            return Resultado<long>.Exito(entidad.Id, "Tratamiento registrado exitosamente.");
        }
        catch (Exception ex)
        {
            return Resultado<long>.Falla($"Error interno al registrar el tratamiento: {ex.Message}");
        }
    }
    // Registra la aplicación de un tratamiento a una consulta médica veterinaria (DetalleConsulta).
    public async Task<Resultado<long>> CrearAsync(DetalleConsultaSolicitudDto solicitud)
    {
        try
        {
            // 1. Validaciones previas de integridad del detalle aplicado
            if (solicitud.IdConsulta <= 0)
                return Resultado<long>.Falla("Debe seleccionar una consulta clínica válida.");

            if (solicitud.IdTratamiento <= 0)
                return Resultado<long>.Falla("Debe seleccionar un tratamiento válido del catálogo.");

            if (solicitud.Cantidad <= 0)
                return Resultado<long>.Falla("La cantidad debe ser mayor a cero.");

            var consultaExiste = await context.Consultas.AnyAsync(c => c.Id == solicitud.IdConsulta);
            if (!consultaExiste)
                return Resultado<long>.Falla($"No existe la consulta con ID {solicitud.IdConsulta}.");

            var tratamiento = await context.Tratamientos.AsNoTracking().FirstOrDefaultAsync(t => t.Id == solicitud.IdTratamiento);
            if (tratamiento is null)
                return Resultado<long>.Falla($"No existe el tratamiento con ID {solicitud.IdTratamiento}.");

            if (tratamiento.UsaStock && tratamiento.Stock < solicitud.Cantidad)
                return Resultado<long>.Falla($"No hay stock suficiente de este tratamiento. Stock actual: {tratamiento.Stock}.");

            // 2. Cálculo de importes unitario y subtotal
            var precioUnitario = solicitud.PrecioUnitario > 0 ? solicitud.PrecioUnitario : tratamiento.Precio;
            var subtotal = solicitud.Subtotal > 0 ? solicitud.Subtotal : (precioUnitario * solicitud.Cantidad);

            // 3. Creación y persistencia de la entidad DetalleConsulta
            var entidad = new DetalleConsulta
            {
                IdConsulta = solicitud.IdConsulta,
                IdTratamiento = solicitud.IdTratamiento,
                Cantidad = solicitud.Cantidad,
                PrecioUnitario = precioUnitario,
                Subtotal = subtotal,
                Indicaciones = string.IsNullOrWhiteSpace(solicitud.Indicaciones) ? null : solicitud.Indicaciones.Trim(),
                Activo = true
            };

            var strategy = context.Database.CreateExecutionStrategy();
            return await strategy.ExecuteAsync(async () =>
            {
                using var transaction = await context.Database.BeginTransactionAsync();
                try
                {
                    if (tratamiento.UsaStock)
                    {
                        // Descuento atómico condicional en el motor de base de datos
                        var filasAfectadas = await context.Tratamientos
                            .Where(t => t.Id == solicitud.IdTratamiento && t.Stock >= solicitud.Cantidad)
                            .ExecuteUpdateAsync(s => s.SetProperty(t => t.Stock, t => t.Stock - solicitud.Cantidad));

                        if (filasAfectadas == 0)
                        {
                            await transaction.RollbackAsync();
                            return Resultado<long>.Falla("No hay stock suficiente de este tratamiento (puede haber sido consumido simultáneamente por otro usuario).");
                        }
                    }

                    context.DetalleConsultas.Add(entidad);
                    await context.SaveChangesAsync();
                    await transaction.CommitAsync();

                    return Resultado<long>.Exito(entidad.Id, "Tratamiento aplicado exitosamente a la consulta.");
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    return Resultado<long>.Falla($"Error interno al aplicar el tratamiento: {ex.Message}");
                }
            });
        }
        catch (Exception ex)
        {
            return Resultado<long>.Falla($"Error interno al aplicar el tratamiento: {ex.Message}");
        }
    }
    // Obtiene todos los tratamientos aplicados a una consulta médica específica.
    public async Task<Resultado<IEnumerable<DetalleConsultaRespuestaDto>>> ObtenerTratamientosAplicadosPorConsultaAsync(long idConsulta)
    {
        try
        {
            if (idConsulta <= 0)
                return Resultado<IEnumerable<DetalleConsultaRespuestaDto>>.Falla("El identificador de la consulta debe ser mayor a cero.");

            var detalles = await context.DetalleConsultas
                .AsNoTracking()
                .Where(d => d.IdConsulta == idConsulta && d.Activo)
                .Select(d => new DetalleConsultaRespuestaDto
                {
                    Id = d.Id,
                    IdConsulta = d.IdConsulta,
                    IdTratamiento = d.IdTratamiento,
                    TipoTratamiento = d.Tratamiento != null ? d.Tratamiento.TipoTratamiento : string.Empty,
                    DescripcionTratamiento = d.Tratamiento != null ? d.Tratamiento.Descripcion : string.Empty,
                    Cantidad = d.Cantidad,
                    PrecioUnitario = d.PrecioUnitario,
                    Subtotal = d.Subtotal,
                    Indicaciones = d.Indicaciones,
                    Activo = d.Activo
                })
                .ToListAsync();

            return Resultado<IEnumerable<DetalleConsultaRespuestaDto>>.Exito(detalles);
        }
        catch (Exception ex)
        {
            return Resultado<IEnumerable<DetalleConsultaRespuestaDto>>.Falla($"Error al obtener tratamientos aplicados: {ex.Message}");
        }
    }
    // Obtiene todos los tratamientos aplicados activos (listado general del módulo).
    public async Task<Resultado<IEnumerable<DetalleConsultaRespuestaDto>>> ObtenerTodosAplicadosAsync()
    {
        try
        {
            var detalles = await context.DetalleConsultas
                .AsNoTracking()
                .Where(d => d.Activo)
                .OrderByDescending(d => d.Id)
                .Select(d => new DetalleConsultaRespuestaDto
                {
                    Id = d.Id,
                    IdConsulta = d.IdConsulta,
                    IdTratamiento = d.IdTratamiento,
                    TipoTratamiento = d.Tratamiento != null ? d.Tratamiento.TipoTratamiento : string.Empty,
                    DescripcionTratamiento = d.Tratamiento != null ? d.Tratamiento.Descripcion : string.Empty,
                    Cantidad = d.Cantidad,
                    PrecioUnitario = d.PrecioUnitario,
                    Subtotal = d.Subtotal,
                    Indicaciones = d.Indicaciones,
                    Activo = d.Activo
                })
                .ToListAsync();

            return Resultado<IEnumerable<DetalleConsultaRespuestaDto>>.Exito(detalles);
        }
        catch (Exception ex)
        {
            return Resultado<IEnumerable<DetalleConsultaRespuestaDto>>.Falla($"Error al obtener tratamientos aplicados: {ex.Message}");
        }
    }

    public async Task<Resultado> ActualizarAsync(long id, TratamientoSolicitudDto solicitud)
    {
        try
        {
            if (id <= 0)
                return Resultado.Falla("El identificador del tratamiento debe ser mayor a cero.");

            if (string.IsNullOrWhiteSpace(solicitud.Descripcion))
                return Resultado.Falla("La descripción del tratamiento es obligatoria.");

            var errorDatos = ValidarDatosTratamiento(solicitud);
            if (errorDatos is not null)
                return Resultado.Falla(errorDatos);

            var entidad = await context.Tratamientos.FirstOrDefaultAsync(t => t.Id == id);
            if (entidad is null)
                return Resultado.Falla($"No se encontró el tratamiento con ID {id}.");

            // Inmutabilidad de producto inventariable: si ya controlaba stock, no puede convertirse en servicio sin stock.
            if (entidad.UsaStock && !solicitud.UsaStock)
                return Resultado.Falla("Un tratamiento que controla stock no puede transformarse en un servicio sin stock. Para discontinuarlo debe darse de baja.");

            // El stock solo se puede aumentar si el tratamiento ya controlaba stock previamente.
            // Si antes no usaba stock (ej. era un servicio), se está fijando el inventario inicial por primera vez.
            if (entidad.UsaStock && solicitud.UsaStock && solicitud.Stock < entidad.Stock)
                return Resultado.Falla($"El stock solo se puede aumentar (stock actual: {entidad.Stock}).");

            entidad.TipoTratamiento = string.IsNullOrWhiteSpace(solicitud.TipoTratamiento) ? "General" : solicitud.TipoTratamiento.Trim();
            entidad.Descripcion = solicitud.Descripcion.Trim();
            entidad.Dosis = string.IsNullOrWhiteSpace(solicitud.Dosis) ? null : solicitud.Dosis.Trim();
            entidad.Precio = solicitud.Precio;
            entidad.UsaStock = solicitud.UsaStock;
            entidad.Stock = solicitud.UsaStock ? Math.Max(0, solicitud.Stock) : 0;

            if (!context.ChangeTracker.HasChanges())
                return Resultado.Falla("No se detectaron cambios para guardar.");

            await context.SaveChangesAsync();
            return Resultado.Exito("Tratamiento actualizado exitosamente.");
        }
        catch (Exception ex)
        {
            return Resultado.Falla($"Error interno al actualizar el tratamiento: {ex.Message}");
        }
    }

    public async Task<Resultado> EliminarAsync(long id)
    {
        try
        {
            if (id <= 0)
                return Resultado.Falla("El identificador del tratamiento debe ser mayor a cero.");

            var entidad = await context.Tratamientos.FirstOrDefaultAsync(t => t.Id == id);
            if (entidad is null)
                return Resultado.Falla($"No se encontró el tratamiento con ID {id}.");

            entidad.Activo = false;
            await context.SaveChangesAsync();
            return Resultado.Exito("Tratamiento eliminado exitosamente.");
        }
        catch (Exception ex)
        {
            return Resultado.Falla($"Error interno al eliminar el tratamiento: {ex.Message}");
        }
    }
    // Elimina (baja lógica) un tratamiento aplicado a una consulta clínica.
    public async Task<Resultado> EliminarDetalleAplicadoAsync(long idDetalle)
    {
        try
        {
            if (idDetalle <= 0)
                return Resultado.Falla("Debe seleccionar un tratamiento aplicado válido.");

            var entidad = await context.DetalleConsultas.FirstOrDefaultAsync(d => d.Id == idDetalle);
            if (entidad is null)
                return Resultado.Falla($"No se encontró el tratamiento aplicado con ID {idDetalle}.");

            var strategy = context.Database.CreateExecutionStrategy();
            return await strategy.ExecuteAsync(async () =>
            {
                using var transaction = await context.Database.BeginTransactionAsync();
                try
                {
                    if (entidad.Activo)
                    {
                        await context.Tratamientos
                            .Where(t => t.Id == entidad.IdTratamiento && t.UsaStock)
                            .ExecuteUpdateAsync(s => s.SetProperty(t => t.Stock, t => t.Stock + entidad.Cantidad));
                    }

                    entidad.Activo = false;
                    await context.SaveChangesAsync();
                    await transaction.CommitAsync();

                    return Resultado.Exito("Tratamiento eliminado de la consulta.");
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    return Resultado.Falla($"Error al eliminar el tratamiento aplicado: {ex.Message}");
                }
            });
        }
        catch (Exception ex)
        {
            return Resultado.Falla($"Error al eliminar el tratamiento aplicado: {ex.Message}");
        }
    }

    private static string? ValidarDatosTratamiento(TratamientoSolicitudDto solicitud)
    {
        if (solicitud.Precio < 0)
            return "El precio del tratamiento no puede ser negativo.";

        if (solicitud.UsaStock && solicitud.Stock < 0)
            return "El stock no puede ser negativo.";

        if (solicitud.Dosis?.Trim().Length > 100)
            return "La dosis no puede superar los 100 caracteres.";

        if (solicitud.TipoTratamiento?.Trim().Length > 50)
            return "El tipo de tratamiento no puede superar los 50 caracteres.";

        return null;
    }

    private IQueryable<TratamientoRespuestaDto> ConsultaBase() =>
        context.Tratamientos
            .AsNoTracking()
            .Select(t => new TratamientoRespuestaDto
            {
                Id = t.Id,
                TipoTratamiento = t.TipoTratamiento,
                Descripcion = t.Descripcion,
                Dosis = t.Dosis,
                Precio = t.Precio,
                UsaStock = t.UsaStock,
                Stock = t.Stock,
                Activo = t.Activo
            });
}
