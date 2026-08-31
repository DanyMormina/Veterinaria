using Microsoft.EntityFrameworkCore;
using Veterinaria.CrossCutting.Comunes;
using Veterinaria.Domain.Dtos;
using Veterinaria.Domain.Entidades;
using Veterinaria.Infrastructure;
using Veterinaria.Interfaces.Interfaces;

namespace Veterinaria.Services.Servicios;

/// <summary>
/// Servicio para la gestión de Mascotas (Pacientes) y su historial clínico.
/// </summary>
public class MascotaService(VeterinariaDbContext context) : IMascotaService
{
    public async Task<Result<IEnumerable<MascotaResponseDto>>> ObtenerTodosAsync()
    {
        var mascotas = await context.Mascotas
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
            })
            .ToListAsync();

        return Result<IEnumerable<MascotaResponseDto>>.Ok(mascotas);
    }

    public async Task<Result<MascotaResponseDto>> ObtenerPorIdAsync(long id)
    {
        if (id <= 0)
            return Result<MascotaResponseDto>.Falla("El identificador de la mascota debe ser mayor a cero.");

        var mascota = await context.Mascotas
            .AsNoTracking()
            .Where(m => m.Id == id)
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
            })
            .FirstOrDefaultAsync();

        if (mascota is null)
            return Result<MascotaResponseDto>.Falla($"No se encontró la mascota con ID {id}.");

        return Result<MascotaResponseDto>.Ok(mascota);
    }

    public async Task<Result<IEnumerable<MascotaResponseDto>>> ObtenerPorPropietarioAsync(long idPropietario)
    {
        if (idPropietario <= 0)
            return Result<IEnumerable<MascotaResponseDto>>.Falla("El identificador del propietario debe ser mayor a cero.");

        var mascotas = await context.Mascotas
            .AsNoTracking()
            .Where(m => m.IdPropietario == idPropietario)
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
            })
            .ToListAsync();

        return Result<IEnumerable<MascotaResponseDto>>.Ok(mascotas);
    }

    public async Task<Result<long>> CrearAsync(MascotaRequestDto request)
    {
        if (string.IsNullOrWhiteSpace(request.Nombre))
            return Result<long>.Falla("El nombre de la mascota es obligatorio.");

        if (request.IdPropietario <= 0)
            return Result<long>.Falla("Debe asociar un propietario válido.");

        if (request.IdRaza <= 0)
            return Result<long>.Falla("Debe seleccionar una raza válida.");

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

    public async Task<Result> ActualizarAsync(long id, MascotaRequestDto request)
    {
        if (id <= 0)
            return Result.Falla("El identificador de la mascota debe ser mayor a cero.");

        if (string.IsNullOrWhiteSpace(request.Nombre))
            return Result.Falla("El nombre de la mascota es obligatorio.");

        if (request.IdPropietario <= 0)
            return Result.Falla("Debe asociar un propietario válido.");

        if (request.IdRaza <= 0)
            return Result.Falla("Debe seleccionar una raza válida.");

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

    public async Task<Result> EliminarAsync(long id)
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
}
