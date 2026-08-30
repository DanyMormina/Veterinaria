using Microsoft.EntityFrameworkCore;
using Veterinaria.CrossCutting.Comunes;
using Veterinaria.Domain.Dtos;
using Veterinaria.Domain.Entidades;
using Veterinaria.Infrastructure;
using Veterinaria.Interfaces.Interfaces;

namespace Veterinaria.Services.Servicios;

/// <summary>
/// Servicio para la gestión de Vacunas y sus cronogramas de aplicación.
/// </summary>
public class VacunaService(VeterinariaDbContext context) : IVacunaService
{
    public async Task<Result<IEnumerable<VacunaResponseDto>>> ObtenerTodosAsync()
    {
        var vacunas = await context.Vacunas
            .AsNoTracking()
            .Select(v => new VacunaResponseDto
            {
                Id = v.Id,
                Nombre = v.Nombre,
                PeriodoMesesRecomendado = v.PeriodoMesesRecomendado,
                Activo = v.Activo
            })
            .ToListAsync();

        return Result<IEnumerable<VacunaResponseDto>>.Ok(vacunas);
    }

    public async Task<Result<VacunaResponseDto>> ObtenerPorIdAsync(long id)
    {
        if (id <= 0)
            return Result<VacunaResponseDto>.Falla("El identificador de la vacuna debe ser mayor a cero.");

        var vacuna = await context.Vacunas
            .AsNoTracking()
            .Where(v => v.Id == id)
            .Select(v => new VacunaResponseDto
            {
                Id = v.Id,
                Nombre = v.Nombre,
                PeriodoMesesRecomendado = v.PeriodoMesesRecomendado,
                Activo = v.Activo
            })
            .FirstOrDefaultAsync();

        if (vacuna is null)
            return Result<VacunaResponseDto>.Falla($"No se encontró la vacuna con ID {id}.");

        return Result<VacunaResponseDto>.Ok(vacuna);
    }

    public async Task<Result<long>> CrearAsync(VacunaRequestDto request)
    {
        if (string.IsNullOrWhiteSpace(request.Nombre))
            return Result<long>.Falla("El nombre de la vacuna es obligatorio.");

        if (request.PeriodoMesesRecomendado <= 0)
            return Result<long>.Falla("El período recomendado en meses debe ser mayor a cero.");

        var nombreNormalizado = request.Nombre.Trim();
        var existe = await context.Vacunas
            .AnyAsync(v => v.Nombre.ToLower() == nombreNormalizado.ToLower());

        if (existe)
            return Result<long>.Falla($"Ya existe una vacuna registrada con el nombre '{nombreNormalizado}'.");

        var entidad = new Vacuna
        {
            Nombre = nombreNormalizado,
            PeriodoMesesRecomendado = request.PeriodoMesesRecomendado,
            Activo = true
        };

        context.Vacunas.Add(entidad);
        await context.SaveChangesAsync();

        return Result<long>.Ok(entidad.Id, "Vacuna creada exitosamente.");
    }

    public async Task<Result> ActualizarAsync(long id, VacunaRequestDto request)
    {
        if (id <= 0)
            return Result.Falla("El identificador de la vacuna debe ser mayor a cero.");

        if (string.IsNullOrWhiteSpace(request.Nombre))
            return Result.Falla("El nombre de la vacuna es obligatorio.");

        if (request.PeriodoMesesRecomendado <= 0)
            return Result.Falla("El período recomendado en meses debe ser mayor a cero.");

        var entidad = await context.Vacunas.FirstOrDefaultAsync(v => v.Id == id);
        if (entidad is null)
            return Result.Falla($"No se encontró la vacuna con ID {id}.");

        var nombreNormalizado = request.Nombre.Trim();
        var existeDuplicado = await context.Vacunas
            .AnyAsync(v => v.Id != id && v.Nombre.ToLower() == nombreNormalizado.ToLower());

        if (existeDuplicado)
            return Result.Falla($"Ya existe otra vacuna registrada con el nombre '{nombreNormalizado}'.");

        entidad.Nombre = nombreNormalizado;
        entidad.PeriodoMesesRecomendado = request.PeriodoMesesRecomendado;
        await context.SaveChangesAsync();

        return Result.Ok("Vacuna actualizada exitosamente.");
    }

    public async Task<Result> EliminarAsync(long id)
    {
        if (id <= 0)
            return Result.Falla("El identificador de la vacuna debe ser mayor a cero.");

        var entidad = await context.Vacunas.FirstOrDefaultAsync(v => v.Id == id);
        if (entidad is null)
            return Result.Falla($"No se encontró la vacuna con ID {id}.");

        entidad.Activo = false;
        await context.SaveChangesAsync();

        return Result.Ok("Vacuna eliminada exitosamente.");
    }
}
