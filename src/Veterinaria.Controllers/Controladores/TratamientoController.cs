using Microsoft.EntityFrameworkCore;
using Veterinaria.Domain.Comunes;
using Veterinaria.Domain.Dtos;
using Veterinaria.Domain.Entidades;
using Veterinaria.Infrastructure;

namespace Veterinaria.Controllers.Controladores;

public class TratamientoController(VeterinariaDbContext context)
{
    public async Task<Result<IEnumerable<TratamientoResponseDto>>> ObtenerTodosAsync()
    {
        try
        {
            var tratamientos = await ConsultaBase().ToListAsync();
            return Result<IEnumerable<TratamientoResponseDto>>.Ok(tratamientos);
        }
        catch (Exception ex)
        {
            return Result<IEnumerable<TratamientoResponseDto>>.Falla($"Error interno al obtener tratamientos: {ex.Message}");
        }
    }

    public async Task<Result<TratamientoResponseDto>> ObtenerPorIdAsync(long id)
    {
        try
        {
            if (id <= 0)
                return Result<TratamientoResponseDto>.Falla("El identificador del tratamiento debe ser mayor a cero.");

            var tratamiento = await ConsultaBase().FirstOrDefaultAsync(t => t.Id == id);
            if (tratamiento is null)
                return Result<TratamientoResponseDto>.Falla($"No se encontró el tratamiento con ID {id}.");

            return Result<TratamientoResponseDto>.Ok(tratamiento);
        }
        catch (Exception ex)
        {
            return Result<TratamientoResponseDto>.Falla($"Error interno al obtener el tratamiento: {ex.Message}");
        }
    }

    public async Task<Result<long>> CrearAsync(TratamientoRequestDto request)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(request.Descripcion))
                return Result<long>.Falla("La descripción del tratamiento es obligatoria.");

            var entidad = new Tratamiento
            {
                TipoTratamiento = string.IsNullOrWhiteSpace(request.TipoTratamiento) ? "General" : request.TipoTratamiento.Trim(),
                Descripcion = request.Descripcion.Trim(),
                Dosis = string.IsNullOrWhiteSpace(request.Dosis) ? null : request.Dosis.Trim(),
                Precio = request.Precio,
                Activo = true
            };

            context.Tratamientos.Add(entidad);
            await context.SaveChangesAsync();
            return Result<long>.Ok(entidad.Id, "Tratamiento registrado exitosamente.");
        }
        catch (Exception ex)
        {
            return Result<long>.Falla($"Error interno al registrar el tratamiento: {ex.Message}");
        }
    }

    public async Task<Result> ActualizarAsync(long id, TratamientoRequestDto request)
    {
        try
        {
            if (id <= 0)
                return Result.Falla("El identificador del tratamiento debe ser mayor a cero.");

            if (string.IsNullOrWhiteSpace(request.Descripcion))
                return Result.Falla("La descripción del tratamiento es obligatoria.");

            var entidad = await context.Tratamientos.FirstOrDefaultAsync(t => t.Id == id);
            if (entidad is null)
                return Result.Falla($"No se encontró el tratamiento con ID {id}.");

            entidad.TipoTratamiento = string.IsNullOrWhiteSpace(request.TipoTratamiento) ? "General" : request.TipoTratamiento.Trim();
            entidad.Descripcion = request.Descripcion.Trim();
            entidad.Dosis = string.IsNullOrWhiteSpace(request.Dosis) ? null : request.Dosis.Trim();
            entidad.Precio = request.Precio;

            await context.SaveChangesAsync();
            return Result.Ok("Tratamiento actualizado exitosamente.");
        }
        catch (Exception ex)
        {
            return Result.Falla($"Error interno al actualizar el tratamiento: {ex.Message}");
        }
    }

    public async Task<Result> EliminarAsync(long id)
    {
        try
        {
            if (id <= 0)
                return Result.Falla("El identificador del tratamiento debe ser mayor a cero.");

            var entidad = await context.Tratamientos.FirstOrDefaultAsync(t => t.Id == id);
            if (entidad is null)
                return Result.Falla($"No se encontró el tratamiento con ID {id}.");

            entidad.Activo = false;
            await context.SaveChangesAsync();
            return Result.Ok("Tratamiento eliminado exitosamente.");
        }
        catch (Exception ex)
        {
            return Result.Falla($"Error interno al eliminar el tratamiento: {ex.Message}");
        }
    }

    private IQueryable<TratamientoResponseDto> ConsultaBase() =>
        context.Tratamientos
            .AsNoTracking()
            .Select(t => new TratamientoResponseDto
            {
                Id = t.Id,
                TipoTratamiento = t.TipoTratamiento,
                Descripcion = t.Descripcion,
                Dosis = t.Dosis,
                Precio = t.Precio,
                Activo = t.Activo
            });
}
