using Microsoft.EntityFrameworkCore;
using Veterinaria.CrossCutting.Comunes;
using Veterinaria.Domain.Dtos;
using Veterinaria.Domain.Entidades;
using Veterinaria.Infrastructure;
using Veterinaria.Interfaces.Interfaces;

namespace Veterinaria.Services.Servicios;

/// <summary>
/// Servicio para la gestión de Mascotas (pacientes) según DER v2.
/// </summary>
public class MascotaService(VeterinariaDbContext context) : IMascotaService
{
    public async Task<Result<IEnumerable<MascotaResponseDto>>> ObtenerTodosAsync()
    {
        var mascotas = await context.Mascotas
            .AsNoTracking()
            .Include(m => m.Propietario)
            .Include(m => m.Especie)
            .Include(m => m.Consultas)
            .Select(m => new MascotaResponseDto
            {
                Id = m.Id,
                IdPropietario = m.IdPropietario,
                NombrePropietario = m.Propietario != null ? $"{m.Propietario.Nombre} {m.Propietario.Apellido}".Trim() : string.Empty,
                IdEspecie = m.IdEspecie,
                NombreEspecie = m.Especie != null ? m.Especie.Nombre : string.Empty,
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
            .Include(m => m.Propietario)
            .Include(m => m.Especie)
            .Include(m => m.Consultas)
            .Where(m => m.Id == id)
            .Select(m => new MascotaResponseDto
            {
                Id = m.Id,
                IdPropietario = m.IdPropietario,
                NombrePropietario = m.Propietario != null ? $"{m.Propietario.Nombre} {m.Propietario.Apellido}".Trim() : string.Empty,
                IdEspecie = m.IdEspecie,
                NombreEspecie = m.Especie != null ? m.Especie.Nombre : string.Empty,
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

    public async Task<Result<long>> CrearAsync(MascotaRequestDto request)
    {
        if (request.IdPropietario <= 0)
            return Result<long>.Falla("El identificador del propietario debe ser mayor a cero.");

        if (request.IdEspecie <= 0)
            return Result<long>.Falla("El identificador de la especie debe ser mayor a cero.");

        if (string.IsNullOrWhiteSpace(request.Nombre))
            return Result<long>.Falla("El nombre de la mascota es obligatorio.");

        if (string.IsNullOrWhiteSpace(request.Sexo))
            return Result<long>.Falla("El sexo de la mascota es obligatorio.");

        var propietarioExiste = await context.Propietarios.AnyAsync(p => p.Id == request.IdPropietario);
        if (!propietarioExiste)
            return Result<long>.Falla($"No existe un propietario registrado con ID {request.IdPropietario}.");

        var especieExiste = await context.Especies.AnyAsync(e => e.Id == request.IdEspecie);
        if (!especieExiste)
            return Result<long>.Falla($"No existe una especie registrada con ID {request.IdEspecie}.");

        var entidad = new Mascota
        {
            IdPropietario = request.IdPropietario,
            IdEspecie = request.IdEspecie,
            Nombre = request.Nombre.Trim(),
            Sexo = request.Sexo.Trim(),
            FechaNacimiento = request.FechaNacimiento,
            Color = string.IsNullOrWhiteSpace(request.Color) ? null : request.Color.Trim(),
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

        if (request.IdPropietario <= 0)
            return Result.Falla("El identificador del propietario debe ser mayor a cero.");

        if (request.IdEspecie <= 0)
            return Result.Falla("El identificador de la especie debe ser mayor a cero.");

        if (string.IsNullOrWhiteSpace(request.Nombre))
            return Result.Falla("El nombre de la mascota es obligatorio.");

        if (string.IsNullOrWhiteSpace(request.Sexo))
            return Result.Falla("El sexo de la mascota es obligatorio.");

        var entidad = await context.Mascotas.FirstOrDefaultAsync(m => m.Id == id);
        if (entidad is null)
            return Result.Falla($"No se encontró la mascota con ID {id}.");

        var propietarioExiste = await context.Propietarios.AnyAsync(p => p.Id == request.IdPropietario);
        if (!propietarioExiste)
            return Result.Falla($"No existe un propietario registrado con ID {request.IdPropietario}.");

        var especieExiste = await context.Especies.AnyAsync(e => e.Id == request.IdEspecie);
        if (!especieExiste)
            return Result.Falla($"No existe una especie registrada con ID {request.IdEspecie}.");

        entidad.IdPropietario = request.IdPropietario;
        entidad.IdEspecie = request.IdEspecie;
        entidad.Nombre = request.Nombre.Trim();
        entidad.Sexo = request.Sexo.Trim();
        entidad.FechaNacimiento = request.FechaNacimiento;
        entidad.Color = string.IsNullOrWhiteSpace(request.Color) ? null : request.Color.Trim();

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
