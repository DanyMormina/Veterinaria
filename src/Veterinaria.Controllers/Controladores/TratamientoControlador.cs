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
