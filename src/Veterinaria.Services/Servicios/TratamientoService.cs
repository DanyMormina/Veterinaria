using Microsoft.EntityFrameworkCore;
using Veterinaria.CrossCutting.Comunes;
using Veterinaria.Domain.Dtos;
using Veterinaria.Domain.Entidades;
using Veterinaria.Infrastructure;
using Veterinaria.Interfaces.Interfaces;

namespace Veterinaria.Services.Servicios;

/// <summary>
/// Servicio para la gestión del catálogo de Tratamientos según DER v2.
/// </summary>
public class TratamientoService(VeterinariaDbContext context) : ITratamientoService
{
    public async Task<Result<IEnumerable<TratamientoResponseDto>>> ObtenerTodosAsync()
    {
        var tratamientos = await context.Tratamientos
            .AsNoTracking()
            .Select(t => new TratamientoResponseDto
            {
                Id = t.Id,
                TipoTratamiento = t.TipoTratamiento,
                Descripcion = t.Descripcion,
                Dosis = t.Dosis,
                Precio = t.Precio,
                Activo = t.Activo
            })
            .ToListAsync();

        return Result<IEnumerable<TratamientoResponseDto>>.Ok(tratamientos);
    }

    public async Task<Result<TratamientoResponseDto>> ObtenerPorIdAsync(long id)
    {
        if (id <= 0)
            return Result<TratamientoResponseDto>.Falla("El identificador del tratamiento debe ser mayor a cero.");

        var tratamiento = await context.Tratamientos
            .AsNoTracking()
            .Where(t => t.Id == id)
            .Select(t => new TratamientoResponseDto
            {
                Id = t.Id,
                TipoTratamiento = t.TipoTratamiento,
                Descripcion = t.Descripcion,
                Dosis = t.Dosis,
                Precio = t.Precio,
                Activo = t.Activo
            })
            .FirstOrDefaultAsync();

        if (tratamiento is null)
            return Result<TratamientoResponseDto>.Falla($"No se encontró el tratamiento con ID {id}.");

        return Result<TratamientoResponseDto>.Ok(tratamiento);
    }

    public async Task<Result<long>> CrearAsync(TratamientoRequestDto request)
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

    public async Task<Result> ActualizarAsync(long id, TratamientoRequestDto request)
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

    public async Task<Result> EliminarAsync(long id)
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
}
