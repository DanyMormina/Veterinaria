using Microsoft.EntityFrameworkCore;
using Veterinaria.Domain.Comunes;
using Veterinaria.Domain.Dtos;
using Veterinaria.Domain.Entidades;
using Veterinaria.Infrastructure;

namespace Veterinaria.Controllers.Controladores;

public class VacunaController(VeterinariaDbContext context)
{
    public async Task<Result<IEnumerable<VacunaResponseDto>>> ObtenerTodosAsync()
    {
        try
        {
            var vacunas = await ConsultaBase().ToListAsync();
            return Result<IEnumerable<VacunaResponseDto>>.Ok(vacunas);
        }
        catch (Exception ex)
        {
            return Result<IEnumerable<VacunaResponseDto>>.Falla($"Error interno al obtener vacunas: {ex.Message}");
        }
    }

    public async Task<Result<IEnumerable<VacunaResponseDto>>> ObtenerPorEspecieAsync(long idEspecie)
    {
        try
        {
            if (idEspecie <= 0)
                return Result<IEnumerable<VacunaResponseDto>>.Falla("El identificador de la especie debe ser mayor a cero.");

            var vacunas = await ConsultaBase()
                .Where(v => v.IdEspecie == idEspecie)
                .ToListAsync();

            return Result<IEnumerable<VacunaResponseDto>>.Ok(vacunas);
        }
        catch (Exception ex)
        {
            return Result<IEnumerable<VacunaResponseDto>>.Falla($"Error interno al obtener vacunas por especie: {ex.Message}");
        }
    }

    public async Task<Result<VacunaResponseDto>> ObtenerPorIdAsync(long id)
    {
        try
        {
            if (id <= 0)
                return Result<VacunaResponseDto>.Falla("El identificador de la vacuna debe ser mayor a cero.");

            var vacuna = await ConsultaBase().FirstOrDefaultAsync(v => v.Id == id);
            if (vacuna is null)
                return Result<VacunaResponseDto>.Falla($"No se encontró la vacuna con ID {id}.");

            return Result<VacunaResponseDto>.Ok(vacuna);
        }
        catch (Exception ex)
        {
            return Result<VacunaResponseDto>.Falla($"Error interno al obtener la vacuna: {ex.Message}");
        }
    }

    public async Task<Result<long>> CrearAsync(VacunaRequestDto request)
    {
        try
        {
            var validacion = Validar(request);
            if (validacion is not null)
                return Result<long>.Falla(validacion);

            var especieExiste = await context.Especies.AnyAsync(e => e.Id == request.IdEspecie);
            if (!especieExiste)
                return Result<long>.Falla($"No existe una especie registrada con ID {request.IdEspecie}.");

            var nombreNormalizado = request.Nombre.Trim();
            var existe = await context.Vacunas.AnyAsync(v =>
                v.IdEspecie == request.IdEspecie &&
                v.Nombre.ToLower() == nombreNormalizado.ToLower());

            if (existe)
                return Result<long>.Falla($"Ya existe la vacuna '{nombreNormalizado}' para esa especie.");

            var entidad = new Vacuna
            {
                IdEspecie = request.IdEspecie,
                Nombre = nombreNormalizado,
                PeriodoMesesRecomendado = request.PeriodoMesesRecomendado,
                Precio = request.Precio,
                Activo = true
            };

            context.Vacunas.Add(entidad);
            await context.SaveChangesAsync();
            return Result<long>.Ok(entidad.Id, "Vacuna creada exitosamente.");
        }
        catch (Exception ex)
        {
            return Result<long>.Falla($"Error interno al crear la vacuna: {ex.Message}");
        }
    }

    public async Task<Result> ActualizarAsync(long id, VacunaRequestDto request)
    {
        try
        {
            if (id <= 0)
                return Result.Falla("El identificador de la vacuna debe ser mayor a cero.");

            var validacion = Validar(request);
            if (validacion is not null)
                return Result.Falla(validacion);

            var entidad = await context.Vacunas.FirstOrDefaultAsync(v => v.Id == id);
            if (entidad is null)
                return Result.Falla($"No se encontró la vacuna con ID {id}.");

            var especieExiste = await context.Especies.AnyAsync(e => e.Id == request.IdEspecie);
            if (!especieExiste)
                return Result.Falla($"No existe una especie registrada con ID {request.IdEspecie}.");

            var nombreNormalizado = request.Nombre.Trim();
            var existeDuplicado = await context.Vacunas.AnyAsync(v =>
                v.Id != id &&
                v.IdEspecie == request.IdEspecie &&
                v.Nombre.ToLower() == nombreNormalizado.ToLower());

            if (existeDuplicado)
                return Result.Falla($"Ya existe otra vacuna '{nombreNormalizado}' para esa especie.");

            entidad.IdEspecie = request.IdEspecie;
            entidad.Nombre = nombreNormalizado;
            entidad.PeriodoMesesRecomendado = request.PeriodoMesesRecomendado;
            entidad.Precio = request.Precio;
            await context.SaveChangesAsync();

            return Result.Ok("Vacuna actualizada exitosamente.");
        }
        catch (Exception ex)
        {
            return Result.Falla($"Error interno al actualizar la vacuna: {ex.Message}");
        }
    }

    public async Task<Result> EliminarAsync(long id)
    {
        try
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
        catch (Exception ex)
        {
            return Result.Falla($"Error interno al eliminar la vacuna: {ex.Message}");
        }
    }

    private IQueryable<VacunaResponseDto> ConsultaBase() =>
        context.Vacunas
            .AsNoTracking()
            .Select(v => new VacunaResponseDto
            {
                Id = v.Id,
                IdEspecie = v.IdEspecie,
                NombreEspecie = v.Especie != null ? v.Especie.Nombre : string.Empty,
                Nombre = v.Nombre,
                PeriodoMesesRecomendado = v.PeriodoMesesRecomendado,
                Precio = v.Precio,
                Activo = v.Activo
            });

    private static string? Validar(VacunaRequestDto request)
    {
        if (request.IdEspecie <= 0)
            return "Debe indicar la especie de la vacuna.";

        if (string.IsNullOrWhiteSpace(request.Nombre))
            return "El nombre de la vacuna es obligatorio.";

        if (request.PeriodoMesesRecomendado <= 0)
            return "El período recomendado en meses debe ser mayor a cero.";

        if (request.Precio < 0)
            return "El precio de la vacuna no puede ser negativo.";

        return null;
    }
}
