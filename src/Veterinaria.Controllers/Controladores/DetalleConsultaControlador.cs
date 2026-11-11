using Microsoft.EntityFrameworkCore;
using Veterinaria.Domain.Comunes;
using Veterinaria.Domain.Dtos;
using Veterinaria.Domain.Entidades;
using Veterinaria.Infrastructure;

namespace Veterinaria.Controllers.Controladores;

public class DetalleConsultaControlador(ContextoVeterinaria context)
{
    public async Task<Resultado<IEnumerable<DetalleConsultaRespuestaDto>>> ObtenerPorConsultaAsync(long idConsulta)
    {
        try
        {
            var items = await ConsultaBase()
                .Where(d => d.IdConsulta == idConsulta)
                .ToListAsync();

            return Resultado<IEnumerable<DetalleConsultaRespuestaDto>>.Exito(items);
        }
        catch (Exception ex)
        {
            return Resultado<IEnumerable<DetalleConsultaRespuestaDto>>.Falla($"Error al obtener detalles de la consulta: {ex.Message}");
        }
    }

    public async Task<Resultado<DetalleConsultaRespuestaDto>> ObtenerPorIdAsync(long id)
    {
        try
        {
            if (id <= 0)
                return Resultado<DetalleConsultaRespuestaDto>.Falla("El identificador debe ser mayor a cero.");

            var item = await ConsultaBase().FirstOrDefaultAsync(d => d.Id == id);
            if (item is null)
                return Resultado<DetalleConsultaRespuestaDto>.Falla($"No se encontró el detalle con ID {id}.");

            return Resultado<DetalleConsultaRespuestaDto>.Exito(item);
        }
        catch (Exception ex)
        {
            return Resultado<DetalleConsultaRespuestaDto>.Falla($"Error al obtener el detalle: {ex.Message}");
        }
    }

    public async Task<Resultado<long>> CrearAsync(DetalleConsultaSolicitudDto solicitud)
    {
        try
        {
            if (solicitud.IdConsulta <= 0)
                return Resultado<long>.Falla("La consulta es obligatoria.");

            if (solicitud.IdTratamiento <= 0)
                return Resultado<long>.Falla("El tratamiento es obligatorio.");

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

            var precioUnitario = solicitud.PrecioUnitario > 0 ? solicitud.PrecioUnitario : tratamiento.Precio;
            var subtotal = solicitud.Subtotal > 0 ? solicitud.Subtotal : (precioUnitario * solicitud.Cantidad);

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

                    return Resultado<long>.Exito(entidad.Id, "Detalle de tratamiento agregado a la consulta.");
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    return Resultado<long>.Falla($"Error al agregar el detalle: {ex.Message}");
                }
            });
        }
        catch (Exception ex)
        {
            return Resultado<long>.Falla($"Error al agregar el detalle: {ex.Message}");
        }
    }

    public async Task<Resultado> ActualizarAsync(long id, DetalleConsultaSolicitudDto solicitud)
    {
        try
        {
            if (id <= 0)
                return Resultado.Falla("El identificador debe ser mayor a cero.");

            var entidad = await context.DetalleConsultas.FirstOrDefaultAsync(d => d.Id == id);
            if (entidad is null)
                return Resultado.Falla($"No se encontró el detalle con ID {id}.");

            if (solicitud.IdTratamiento <= 0)
                return Resultado.Falla("Debe seleccionar un tratamiento válido del catálogo.");

            if (solicitud.Cantidad <= 0)
                return Resultado.Falla("La cantidad debe ser mayor a cero.");

            var tratamientoNuevo = await context.Tratamientos.AsNoTracking().FirstOrDefaultAsync(t => t.Id == solicitud.IdTratamiento);
            if (tratamientoNuevo is null)
                return Resultado.Falla($"No existe el tratamiento con ID {solicitud.IdTratamiento}.");

            var strategy = context.Database.CreateExecutionStrategy();
            return await strategy.ExecuteAsync(async () =>
            {
                using var transaction = await context.Database.BeginTransactionAsync();
                try
                {
                    var cantidadAnterior = entidad.Activo ? entidad.Cantidad : 0;
                    if (entidad.IdTratamiento == solicitud.IdTratamiento)
                    {
                        var delta = solicitud.Cantidad - cantidadAnterior;
                        if (tratamientoNuevo.UsaStock && delta > 0)
                        {
                            var filas = await context.Tratamientos
                                .Where(t => t.Id == solicitud.IdTratamiento && t.Stock >= delta)
                                .ExecuteUpdateAsync(s => s.SetProperty(t => t.Stock, t => t.Stock - delta));

                            if (filas == 0)
                            {
                                await transaction.RollbackAsync();
                                return Resultado.Falla("No hay stock suficiente de este tratamiento para aumentar la cantidad (insuficiente en el servidor).");
                            }
                        }
                        else if (tratamientoNuevo.UsaStock && delta < 0)
                        {
                            var devolver = -delta;
                            await context.Tratamientos
                                .Where(t => t.Id == solicitud.IdTratamiento)
                                .ExecuteUpdateAsync(s => s.SetProperty(t => t.Stock, t => t.Stock + devolver));
                        }
                    }
                    else
                    {
                        if (tratamientoNuevo.UsaStock)
                        {
                            var filas = await context.Tratamientos
                                .Where(t => t.Id == solicitud.IdTratamiento && t.Stock >= solicitud.Cantidad)
                                .ExecuteUpdateAsync(s => s.SetProperty(t => t.Stock, t => t.Stock - solicitud.Cantidad));

                            if (filas == 0)
                            {
                                await transaction.RollbackAsync();
                                return Resultado.Falla("No hay stock suficiente del nuevo tratamiento seleccionado (insuficiente en el servidor).");
                            }
                        }

                        if (cantidadAnterior > 0)
                        {
                            await context.Tratamientos
                                .Where(t => t.Id == entidad.IdTratamiento && t.UsaStock)
                                .ExecuteUpdateAsync(s => s.SetProperty(t => t.Stock, t => t.Stock + cantidadAnterior));
                        }
                    }

                    var precioUnitario = solicitud.PrecioUnitario > 0 ? solicitud.PrecioUnitario : entidad.PrecioUnitario;
                    var subtotal = solicitud.Subtotal > 0 ? solicitud.Subtotal : (precioUnitario * solicitud.Cantidad);

                    entidad.IdTratamiento = solicitud.IdTratamiento;
                    entidad.Cantidad = solicitud.Cantidad;
                    entidad.PrecioUnitario = precioUnitario;
                    entidad.Subtotal = subtotal;
                    entidad.Indicaciones = string.IsNullOrWhiteSpace(solicitud.Indicaciones) ? null : solicitud.Indicaciones.Trim();

                    await context.SaveChangesAsync();
                    await transaction.CommitAsync();

                    return Resultado.Exito("Detalle de consulta actualizado exitosamente.");
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    return Resultado.Falla($"Error al actualizar el detalle: {ex.Message}");
                }
            });
        }
        catch (Exception ex)
        {
            return Resultado.Falla($"Error al actualizar el detalle: {ex.Message}");
        }
    }

    public async Task<Resultado> EliminarAsync(long id)
    {
        try
        {
            if (id <= 0)
                return Resultado.Falla("El identificador debe ser mayor a cero.");

            var entidad = await context.DetalleConsultas.FirstOrDefaultAsync(d => d.Id == id);
            if (entidad is null)
                return Resultado.Falla($"No se encontró el detalle con ID {id}.");

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

                    context.DetalleConsultas.Remove(entidad);
                    await context.SaveChangesAsync();
                    await transaction.CommitAsync();

                    return Resultado.Exito("Detalle eliminado exitosamente.");
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    return Resultado.Falla($"Error al eliminar el detalle: {ex.Message}");
                }
            });
        }
        catch (Exception ex)
        {
            return Resultado.Falla($"Error al eliminar el detalle: {ex.Message}");
        }
    }

    private IQueryable<DetalleConsultaRespuestaDto> ConsultaBase() =>
        context.DetalleConsultas
            .AsNoTracking()
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
            });
}
