using Microsoft.EntityFrameworkCore;
using Veterinaria.Domain.Comunes;
using Veterinaria.Domain.Dtos;
using Veterinaria.Domain.Entidades;
using Veterinaria.Infrastructure;

namespace Veterinaria.Controllers.Controladores;

public class EspecieController(VeterinariaDbContext context)
{
    public async Task<Result<IEnumerable<EspecieResponseDto>>> ObtenerTodosAsync()
    {
        try
        {
            var especies = await context.Especies
                .AsNoTracking()
                .Select(e => new EspecieResponseDto { Id = e.Id, Nombre = e.Nombre, Activo = e.Activo })
                .ToListAsync();

            return Result<IEnumerable<EspecieResponseDto>>.Ok(especies);
        }
        catch (Exception ex)
        {
            return Result<IEnumerable<EspecieResponseDto>>.Falla($"Error interno al obtener especies: {ex.Message}");
        }
    }

    public async Task<Result<EspecieResponseDto>> ObtenerPorIdAsync(long id)
    {
        try
        {
            if (id <= 0)
                return Result<EspecieResponseDto>.Falla("El identificador de la especie debe ser mayor a cero.");

            var especie = await context.Especies
                .AsNoTracking()
                .Where(e => e.Id == id)
                .Select(e => new EspecieResponseDto { Id = e.Id, Nombre = e.Nombre, Activo = e.Activo })
                .FirstOrDefaultAsync();

            if (especie is null)
                return Result<EspecieResponseDto>.Falla($"No se encontró la especie con ID {id}.");

            return Result<EspecieResponseDto>.Ok(especie);
        }
        catch (Exception ex)
        {
            return Result<EspecieResponseDto>.Falla($"Error interno al obtener la especie: {ex.Message}");
        }
    }

    public async Task<Result<long>> CrearAsync(EspecieRequestDto request)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(request.Nombre))
                return Result<long>.Falla("El nombre de la especie es obligatorio.");

            var nombreNormalizado = request.Nombre.Trim();
            var existe = await context.Especies.AnyAsync(e => e.Nombre.ToLower() == nombreNormalizado.ToLower());
            if (existe)
                return Result<long>.Falla($"Ya existe una especie registrada con el nombre '{nombreNormalizado}'.");

            var entidad = new Especie { Nombre = nombreNormalizado, Activo = true };
            context.Especies.Add(entidad);
            await context.SaveChangesAsync();
            return Result<long>.Ok(entidad.Id, "Especie creada exitosamente.");
        }
        catch (Exception ex)
        {
            return Result<long>.Falla($"Error interno al crear la especie: {ex.Message}");
        }
    }

    public async Task<Result> ActualizarAsync(long id, EspecieRequestDto request)
    {
        try
        {
            if (id <= 0)
                return Result.Falla("El identificador de la especie debe ser mayor a cero.");

            if (string.IsNullOrWhiteSpace(request.Nombre))
                return Result.Falla("El nombre de la especie es obligatorio.");

            var entidad = await context.Especies.FirstOrDefaultAsync(e => e.Id == id);
            if (entidad is null)
                return Result.Falla($"No se encontró la especie con ID {id}.");

            var nombreNormalizado = request.Nombre.Trim();
            var existeDuplicado = await context.Especies.AnyAsync(e => e.Id != id && e.Nombre.ToLower() == nombreNormalizado.ToLower());
            if (existeDuplicado)
                return Result.Falla($"Ya existe otra especie registrada con el nombre '{nombreNormalizado}'.");

            entidad.Nombre = nombreNormalizado;
            await context.SaveChangesAsync();
            return Result.Ok("Especie actualizada exitosamente.");
        }
        catch (Exception ex)
        {
            return Result.Falla($"Error interno al actualizar la especie: {ex.Message}");
        }
    }

    public async Task<Result> EliminarAsync(long id)
    {
        try
        {
            if (id <= 0)
                return Result.Falla("El identificador de la especie debe ser mayor a cero.");

            var entidad = await context.Especies.FirstOrDefaultAsync(e => e.Id == id);
            if (entidad is null)
                return Result.Falla($"No se encontró la especie con ID {id}.");

            entidad.Activo = false;
            await context.SaveChangesAsync();
            return Result.Ok("Especie eliminada exitosamente.");
        }
        catch (Exception ex)
        {
            return Result.Falla($"Error interno al eliminar la especie: {ex.Message}");
        }
    }
}
