using Microsoft.EntityFrameworkCore;
using Veterinaria.Domain.Comunes;
using Veterinaria.Domain.Dtos;
using Veterinaria.Domain.Entidades;
using Veterinaria.Infrastructure;

namespace Veterinaria.Controllers.Controladores;

public class RazaController(VeterinariaDbContext context)
{
    public async Task<Result<IEnumerable<RazaResponseDto>>> ObtenerTodosAsync()
    {
        try
        {
            var razas = await ConsultaBase().ToListAsync();
            return Result<IEnumerable<RazaResponseDto>>.Ok(razas);
        }
        catch (Exception ex)
        {
            return Result<IEnumerable<RazaResponseDto>>.Falla($"Error interno al obtener razas: {ex.Message}");
        }
    }

    public async Task<Result<RazaResponseDto>> ObtenerPorIdAsync(long id)
    {
        try
        {
            if (id <= 0)
                return Result<RazaResponseDto>.Falla("El identificador de la raza debe ser mayor a cero.");

            var raza = await ConsultaBase().FirstOrDefaultAsync(r => r.Id == id);
            if (raza is null)
                return Result<RazaResponseDto>.Falla($"No se encontró la raza con ID {id}.");

            return Result<RazaResponseDto>.Ok(raza);
        }
        catch (Exception ex)
        {
            return Result<RazaResponseDto>.Falla($"Error interno al obtener la raza: {ex.Message}");
        }
    }

    public async Task<Result<long>> CrearAsync(RazaRequestDto request)
    {
        try
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
        catch (Exception ex)
        {
            return Result<long>.Falla($"Error interno al crear la raza: {ex.Message}");
        }
    }

    public async Task<Result> ActualizarAsync(long id, RazaRequestDto request)
    {
        try
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
        catch (Exception ex)
        {
            return Result.Falla($"Error interno al actualizar la raza: {ex.Message}");
        }
    }

    public async Task<Result> EliminarAsync(long id)
    {
        try
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
        catch (Exception ex)
        {
            return Result.Falla($"Error interno al eliminar la raza: {ex.Message}");
        }
    }

    private IQueryable<RazaResponseDto> ConsultaBase() =>
        context.Razas
            .AsNoTracking()
            .Select(r => new RazaResponseDto
            {
                Id = r.Id,
                Nombre = r.Nombre,
                IdEspecie = r.IdEspecie,
                NombreEspecie = r.Especie != null ? r.Especie.Nombre : string.Empty,
                Activo = r.Activo
            });
}
