using Microsoft.EntityFrameworkCore;
using Veterinaria.CrossCutting.Comunes;
using Veterinaria.Domain.Dtos;
using Veterinaria.Domain.Entidades;
using Veterinaria.Infrastructure;
using Veterinaria.Interfaces.Interfaces;

namespace Veterinaria.Services.Servicios;

/// <summary>
/// Servicio para la gestión de Tratamientos médicos y aplicaciones de vacunas.
/// </summary>
public class TratamientoService(VeterinariaDbContext context) : ITratamientoService
{
    public async Task<Result<IEnumerable<TratamientoResponseDto>>> ObtenerTodosAsync()
    {
        var tratamientos = await context.Tratamientos
            .AsNoTracking()
            .Include(t => t.Vacuna)
            .Select(t => new TratamientoResponseDto
            {
                Id = t.Id,
                IdConsulta = t.IdConsulta,
                IdVacuna = t.IdVacuna,
                NombreVacuna = t.Vacuna != null ? t.Vacuna.Nombre : null,
                TipoTratamiento = t.TipoTratamiento,
                Descripcion = t.Descripcion,
                Dosis = t.Dosis,
                Indicaciones = t.Indicaciones,
                FechaProximoControl = t.FechaProximoControl,
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
            .Include(t => t.Vacuna)
            .Where(t => t.Id == id)
            .Select(t => new TratamientoResponseDto
            {
                Id = t.Id,
                IdConsulta = t.IdConsulta,
                IdVacuna = t.IdVacuna,
                NombreVacuna = t.Vacuna != null ? t.Vacuna.Nombre : null,
                TipoTratamiento = t.TipoTratamiento,
                Descripcion = t.Descripcion,
                Dosis = t.Dosis,
                Indicaciones = t.Indicaciones,
                FechaProximoControl = t.FechaProximoControl,
                Activo = t.Activo
            })
            .FirstOrDefaultAsync();

        if (tratamiento is null)
            return Result<TratamientoResponseDto>.Falla($"No se encontró el tratamiento con ID {id}.");

        return Result<TratamientoResponseDto>.Ok(tratamiento);
    }

    public async Task<Result<long>> CrearAsync(TratamientoRequestDto request)
    {
        if (request.IdConsulta <= 0)
            return Result<long>.Falla("El identificador de la consulta debe ser mayor a cero.");

        if (string.IsNullOrWhiteSpace(request.Descripcion))
            return Result<long>.Falla("La descripción del tratamiento es obligatoria.");

        var consultaExiste = await context.Consultas.AnyAsync(c => c.Id == request.IdConsulta);
        if (!consultaExiste)
            return Result<long>.Falla($"No existe una consulta registrada con ID {request.IdConsulta}.");

        if (request.IdVacuna.HasValue && request.IdVacuna.Value > 0)
        {
            var vacunaExiste = await context.Vacunas.AnyAsync(v => v.Id == request.IdVacuna.Value);
            if (!vacunaExiste)
                return Result<long>.Falla($"No existe una vacuna registrada con ID {request.IdVacuna.Value}.");
        }

        var entidad = new Tratamiento
        {
            IdConsulta = request.IdConsulta,
            IdVacuna = request.IdVacuna,
            TipoTratamiento = string.IsNullOrWhiteSpace(request.TipoTratamiento) ? "Medicamento" : request.TipoTratamiento.Trim(),
            Descripcion = request.Descripcion.Trim(),
            Dosis = string.IsNullOrWhiteSpace(request.Dosis) ? null : request.Dosis.Trim(),
            Indicaciones = string.IsNullOrWhiteSpace(request.Indicaciones) ? null : request.Indicaciones.Trim(),
            FechaProximoControl = request.FechaProximoControl,
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

        if (request.IdConsulta <= 0)
            return Result.Falla("El identificador de la consulta debe ser mayor a cero.");

        if (string.IsNullOrWhiteSpace(request.Descripcion))
            return Result.Falla("La descripción del tratamiento es obligatoria.");

        var entidad = await context.Tratamientos.FirstOrDefaultAsync(t => t.Id == id);
        if (entidad is null)
            return Result.Falla($"No se encontró el tratamiento con ID {id}.");

        var consultaExiste = await context.Consultas.AnyAsync(c => c.Id == request.IdConsulta);
        if (!consultaExiste)
            return Result.Falla($"No existe una consulta registrada con ID {request.IdConsulta}.");

        if (request.IdVacuna.HasValue && request.IdVacuna.Value > 0)
        {
            var vacunaExiste = await context.Vacunas.AnyAsync(v => v.Id == request.IdVacuna.Value);
            if (!vacunaExiste)
                return Result.Falla($"No existe una vacuna registrada con ID {request.IdVacuna.Value}.");
        }

        entidad.IdConsulta = request.IdConsulta;
        entidad.IdVacuna = request.IdVacuna;
        entidad.TipoTratamiento = string.IsNullOrWhiteSpace(request.TipoTratamiento) ? "Medicamento" : request.TipoTratamiento.Trim();
        entidad.Descripcion = request.Descripcion.Trim();
        entidad.Dosis = string.IsNullOrWhiteSpace(request.Dosis) ? null : request.Dosis.Trim();
        entidad.Indicaciones = string.IsNullOrWhiteSpace(request.Indicaciones) ? null : request.Indicaciones.Trim();
        entidad.FechaProximoControl = request.FechaProximoControl;

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
