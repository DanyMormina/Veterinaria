using Microsoft.EntityFrameworkCore;
using Veterinaria.Domain.Comunes;
using Veterinaria.Domain.Dtos;
using Veterinaria.Domain.Entidades;
using Veterinaria.Infrastructure;

namespace Veterinaria.Controllers.Controladores;

public class MascotaController(VeterinariaDbContext context)
{
    public async Task<Result<IEnumerable<MascotaResponseDto>>> ObtenerTodosAsync()
    {
        try
        {
            var mascotas = await ConsultaBase().ToListAsync();
            return Result<IEnumerable<MascotaResponseDto>>.Ok(mascotas);
        }
        catch (Exception ex)
        {
            return Result<IEnumerable<MascotaResponseDto>>.Falla($"Error interno al obtener mascotas: {ex.Message}");
        }
    }

    public async Task<Result<MascotaResponseDto>> ObtenerPorIdAsync(long id)
    {
        try
        {
            if (id <= 0)
                return Result<MascotaResponseDto>.Falla("El identificador de la mascota debe ser mayor a cero.");

            var mascota = await ConsultaBase().FirstOrDefaultAsync(m => m.Id == id);
            if (mascota is null)
                return Result<MascotaResponseDto>.Falla($"No se encontró la mascota con ID {id}.");

            return Result<MascotaResponseDto>.Ok(mascota);
        }
        catch (Exception ex)
        {
            return Result<MascotaResponseDto>.Falla($"Error interno al obtener la mascota: {ex.Message}");
        }
    }

    public async Task<Result<IEnumerable<MascotaResponseDto>>> ObtenerPorPropietarioAsync(long idPropietario)
    {
        try
        {
            if (idPropietario <= 0)
                return Result<IEnumerable<MascotaResponseDto>>.Falla("El identificador del propietario debe ser mayor a cero.");

            var mascotas = await ConsultaBase()
                .Where(m => m.IdPropietario == idPropietario)
                .ToListAsync();

            return Result<IEnumerable<MascotaResponseDto>>.Ok(mascotas);
        }
        catch (Exception ex)
        {
            return Result<IEnumerable<MascotaResponseDto>>.Falla($"Error interno al obtener mascotas del propietario: {ex.Message}");
        }
    }

    public async Task<Result<long>> CrearAsync(MascotaRequestDto request)
    {
        try
        {
            var validacion = Validar(request);
            if (validacion is not null)
                return Result<long>.Falla(validacion);

            var propietarioExiste = await context.Propietarios.AnyAsync(p => p.Id == request.IdPropietario);
            if (!propietarioExiste)
                return Result<long>.Falla($"No existe el propietario con ID {request.IdPropietario}.");

            var razaExiste = await context.Razas.AnyAsync(r => r.Id == request.IdRaza);
            if (!razaExiste)
                return Result<long>.Falla($"No existe la raza con ID {request.IdRaza}.");

            var entidad = new Mascota
            {
                IdPropietario = request.IdPropietario,
                IdRaza = request.IdRaza,
                Nombre = request.Nombre.Trim(),
                Sexo = request.Sexo?.Trim() ?? string.Empty,
                FechaNacimiento = request.FechaNacimiento,
                Color = request.Color?.Trim(),
                Activo = true
            };

            context.Mascotas.Add(entidad);
            await context.SaveChangesAsync();
            return Result<long>.Ok(entidad.Id, "Mascota registrada exitosamente.");
        }
        catch (Exception ex)
        {
            return Result<long>.Falla($"Error interno al registrar la mascota: {ex.Message}");
        }
    }

    public async Task<Result> ActualizarAsync(long id, MascotaRequestDto request)
    {
        try
        {
            if (id <= 0)
                return Result.Falla("El identificador de la mascota debe ser mayor a cero.");

            var validacion = Validar(request);
            if (validacion is not null)
                return Result.Falla(validacion);

            var entidad = await context.Mascotas.FirstOrDefaultAsync(m => m.Id == id);
            if (entidad is null)
                return Result.Falla($"No se encontró la mascota con ID {id}.");

            var propietarioExiste = await context.Propietarios.AnyAsync(p => p.Id == request.IdPropietario);
            if (!propietarioExiste)
                return Result.Falla($"No existe el propietario con ID {request.IdPropietario}.");

            var razaExiste = await context.Razas.AnyAsync(r => r.Id == request.IdRaza);
            if (!razaExiste)
                return Result.Falla($"No existe la raza con ID {request.IdRaza}.");

            entidad.IdPropietario = request.IdPropietario;
            entidad.IdRaza = request.IdRaza;
            entidad.Nombre = request.Nombre.Trim();
            entidad.Sexo = request.Sexo?.Trim() ?? string.Empty;
            entidad.FechaNacimiento = request.FechaNacimiento;
            entidad.Color = request.Color?.Trim();

            await context.SaveChangesAsync();
            return Result.Ok("Mascota actualizada exitosamente.");
        }
        catch (Exception ex)
        {
            return Result.Falla($"Error interno al actualizar la mascota: {ex.Message}");
        }
    }

    public async Task<Result> EliminarAsync(long id)
    {
        try
        {
            if (id <= 0)
                return Result.Falla("El identificador de la mascota debe ser mayor a cero.");

            var entidad = await context.Mascotas.FirstOrDefaultAsync(m => m.Id == id);
            if (entidad is null)
                return Result.Falla($"No se encontró la mascota con ID {id}.");

            entidad.Activo = false;
            await context.SaveChangesAsync();
            return Result.Ok("Mascota eliminada exitosamente.");
        }
        catch (Exception ex)
        {
            return Result.Falla($"Error interno al eliminar la mascota: {ex.Message}");
        }
    }

    private IQueryable<MascotaResponseDto> ConsultaBase() =>
        context.Mascotas
            .AsNoTracking()
            .Select(m => new MascotaResponseDto
            {
                Id = m.Id,
                IdPropietario = m.IdPropietario,
                NombrePropietario = $"{m.Propietario.Nombre} {m.Propietario.Apellido}",
                IdRaza = m.IdRaza,
                NombreRaza = m.Raza.Nombre,
                IdEspecie = m.Raza.IdEspecie,
                NombreEspecie = m.Raza.Especie.Nombre,
                Nombre = m.Nombre,
                Sexo = m.Sexo,
                FechaNacimiento = m.FechaNacimiento,
                Color = m.Color,
                Activo = m.Activo,
                CantidadConsultas = m.Consultas.Count
            });

    private static string? Validar(MascotaRequestDto request)
    {
        if (string.IsNullOrWhiteSpace(request.Nombre))
            return "El nombre de la mascota es obligatorio.";

        if (request.IdPropietario <= 0)
            return "Debe asociar un propietario válido.";

        if (request.IdRaza <= 0)
            return "Debe seleccionar una raza válida.";

        return null;
    }
}
