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

            var entidad = new Tratamiento
            {
                TipoTratamiento = string.IsNullOrWhiteSpace(solicitud.TipoTratamiento) ? "General" : solicitud.TipoTratamiento.Trim(),
                Descripcion = solicitud.Descripcion.Trim(),
                Dosis = string.IsNullOrWhiteSpace(solicitud.Dosis) ? null : solicitud.Dosis.Trim(),
                Precio = solicitud.Precio,
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

    /// <summary>
    /// Registra la aplicación de un tratamiento a una consulta médica veterinaria (DetalleConsulta).
    /// </summary>
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

            var tratamiento = await context.Tratamientos.FirstOrDefaultAsync(t => t.Id == solicitud.IdTratamiento);
            if (tratamiento is null)
                return Resultado<long>.Falla($"No existe el tratamiento con ID {solicitud.IdTratamiento}.");

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

            context.DetalleConsultas.Add(entidad);
            await context.SaveChangesAsync();
            return Resultado<long>.Exito(entidad.Id, "Tratamiento aplicado exitosamente a la consulta.");
        }
        catch (Exception ex)
        {
            return Resultado<long>.Falla($"Error interno al aplicar el tratamiento: {ex.Message}");
        }
    }

    /// <summary>
    /// Obtiene todos los tratamientos aplicados a una consulta médica específica.
    /// </summary>
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

    public async Task<Resultado> ActualizarAsync(long id, TratamientoSolicitudDto solicitud)
    {
        try
        {
            if (id <= 0)
                return Resultado.Falla("El identificador del tratamiento debe ser mayor a cero.");

            if (string.IsNullOrWhiteSpace(solicitud.Descripcion))
                return Resultado.Falla("La descripción del tratamiento es obligatoria.");

            var entidad = await context.Tratamientos.FirstOrDefaultAsync(t => t.Id == id);
            if (entidad is null)
                return Resultado.Falla($"No se encontró el tratamiento con ID {id}.");

            entidad.TipoTratamiento = string.IsNullOrWhiteSpace(solicitud.TipoTratamiento) ? "General" : solicitud.TipoTratamiento.Trim();
            entidad.Descripcion = solicitud.Descripcion.Trim();
            entidad.Dosis = string.IsNullOrWhiteSpace(solicitud.Dosis) ? null : solicitud.Dosis.Trim();
            entidad.Precio = solicitud.Precio;

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
                Activo = t.Activo
            });
}
