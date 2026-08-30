using Microsoft.EntityFrameworkCore;
using Veterinaria.CrossCutting.Comunes;
using Veterinaria.Domain.Dtos;
using Veterinaria.Domain.Entidades;
using Veterinaria.Infrastructure;
using Veterinaria.Interfaces.Interfaces;

namespace Veterinaria.Services.Servicios;

/// <summary>
/// Servicio para la gestión de Razas asociadas a una Especie.
/// </summary>
public class RazaService(VeterinariaDbContext context) : IRazaService
{
    public async Task<Result<IEnumerable<RazaResponseDto>>> ObtenerTodosAsync()
    {
        var razas = await context.Razas
            .AsNoTracking()
            .Include(r => r.Especie)
            .Select(r => new RazaResponseDto
            {
                Id = r.Id,
                Nombre = r.Nombre,
                IdEspecie = r.IdEspecie,
                NombreEspecie = r.Especie != null ? r.Especie.Nombre : string.Empty,
                Activo = r.Activo
            })
            .ToListAsync();

        return Result<IEnumerable<RazaResponseDto>>.Ok(razas);
    }

    public async Task<Result<RazaResponseDto>> ObtenerPorIdAsync(long id)
    {
        if (id <= 0)
            return Result<RazaResponseDto>.Falla("El identificador de la raza debe ser mayor a cero.");

        var raza = await context.Razas
            .AsNoTracking()
            .Include(r => r.Especie)
            .Where(r => r.Id == id)
            .Select(r => new RazaResponseDto
            {
                Id = r.Id,
                Nombre = r.Nombre,
                IdEspecie = r.IdEspecie,
                NombreEspecie = r.Especie != null ? r.Especie.Nombre : string.Empty,
                Activo = r.Activo
            })
            .FirstOrDefaultAsync();

        if (raza is null)
            return Result<RazaResponseDto>.Falla($"No se encontró la raza con ID {id}.");

        return Result<RazaResponseDto>.Ok(raza);
    }

    public async Task<Result<long>> CrearAsync(RazaRequestDto request)
    {
        if (string.IsNullOrWhiteSpace(request.Nombre))
            return Result<long>.Falla("El nombre de la raza es obligatorio.");

        if (request.IdEspecie <= 0)
            return Result<long>.Falla("El identificador de la especie debe ser mayor a cero.");

        var especieExiste = await context.Especies.AnyAsync(e => e.Id == request.IdEspecie);
        if (!especieExiste)
            return Result<long>.Falla($"No existe una especie registrada con ID {request.IdEspecie}.");

        var nombreNormalizado = request.Nombre.Trim();
        var existeDuplicado = await context.Razas
            .AnyAsync(r => r.IdEspecie == request.IdEspecie && r.Nombre.ToLower() == nombreNormalizado.ToLower());

        if (existeDuplicado)
            return Result<long>.Falla($"Ya existe una raza con el nombre '{nombreNormalizado}' para la especie seleccionada.");

        var entidad = new Raza
        {
            Nombre = nombreNormalizado,
            IdEspecie = request.IdEspecie,
            Activo = true
        };

        context.Razas.Add(entidad);
        await context.SaveChangesAsync();

        return Result<long>.Ok(entidad.Id, "Raza creada exitosamente.");
    }

    public async Task<Result> ActualizarAsync(long id, RazaRequestDto request)
    {
        if (id <= 0)
            return Result.Falla("El identificador de la raza debe ser mayor a cero.");

        if (string.IsNullOrWhiteSpace(request.Nombre))
            return Result.Falla("El nombre de la raza es obligatorio.");

        if (request.IdEspecie <= 0)
            return Result.Falla("El identificador de la especie debe ser mayor a cero.");

        var entidad = await context.Razas.FirstOrDefaultAsync(r => r.Id == id);
        if (entidad is null)
            return Result.Falla($"No se encontró la raza con ID {id}.");

        var especieExiste = await context.Especies.AnyAsync(e => e.Id == request.IdEspecie);
        if (!especieExiste)
            return Result.Falla($"No existe una especie registrada con ID {request.IdEspecie}.");

        var nombreNormalizado = request.Nombre.Trim();
        var existeDuplicado = await context.Razas
            .AnyAsync(r => r.Id != id && r.IdEspecie == request.IdEspecie && r.Nombre.ToLower() == nombreNormalizado.ToLower());

        if (existeDuplicado)
            return Result.Falla($"Ya existe otra raza con el nombre '{nombreNormalizado}' para la especie seleccionada.");

        entidad.Nombre = nombreNormalizado;
        entidad.IdEspecie = request.IdEspecie;
        await context.SaveChangesAsync();

        return Result.Ok("Raza actualizada exitosamente.");
    }

    public async Task<Result> EliminarAsync(long id)
    {
        if (id <= 0)
            return Result.Falla("El identificador de la raza debe ser mayor a cero.");

        var entidad = await context.Razas.FirstOrDefaultAsync(r => r.Id == id);
        if (entidad is null)
            return Result.Falla($"No se encontró la raza con ID {id}.");

        entidad.Activo = false;
        await context.SaveChangesAsync();

        return Result.Ok("Raza eliminada exitosamente.");
    }
}
