using Microsoft.EntityFrameworkCore;
using Veterinaria.CrossCutting.Comunes;
using Veterinaria.Domain.Dtos;
using Veterinaria.Domain.Entidades;
using Veterinaria.Infrastructure;
using Veterinaria.Interfaces.Interfaces;

namespace Veterinaria.Services.Servicios;

/// <summary>
/// Servicio para la gestión y asignación de Turnos de atención veterinaria.
/// </summary>
public class TurnoService(VeterinariaDbContext context) : ITurnoService
{
    public async Task<Result<IEnumerable<TurnoResponseDto>>> ObtenerTodosAsync()
    {
        var turnos = await context.Turnos
            .AsNoTracking()
            .Include(t => t.Mascota)
                .ThenInclude(m => m.Propietario)
            .Include(t => t.Veterinario)
            .Select(t => new TurnoResponseDto
            {
                Id = t.Id,
                IdMascota = t.IdMascota,
                NombreMascota = t.Mascota != null ? t.Mascota.Nombre : string.Empty,
                NombrePropietario = t.Mascota != null && t.Mascota.Propietario != null
                    ? $"{t.Mascota.Propietario.Nombre} {t.Mascota.Propietario.Apellido}".Trim()
                    : string.Empty,
                IdVeterinario = t.IdVeterinario,
                NombreVeterinario = t.Veterinario != null
                    ? $"{t.Veterinario.Nombre} {t.Veterinario.Apellido}".Trim()
                    : string.Empty,
                IdConsulta = t.IdConsulta,
                FechaHora = t.FechaHora,
                Motivo = t.Motivo,
                Estado = t.Estado,
                Activo = t.Activo
            })
            .ToListAsync();

        return Result<IEnumerable<TurnoResponseDto>>.Ok(turnos);
    }

    public async Task<Result<TurnoResponseDto>> ObtenerPorIdAsync(long id)
    {
        if (id <= 0)
            return Result<TurnoResponseDto>.Falla("El identificador del turno debe ser mayor a cero.");

        var turno = await context.Turnos
            .AsNoTracking()
            .Include(t => t.Mascota)
                .ThenInclude(m => m.Propietario)
            .Include(t => t.Veterinario)
            .Where(t => t.Id == id)
            .Select(t => new TurnoResponseDto
            {
                Id = t.Id,
                IdMascota = t.IdMascota,
                NombreMascota = t.Mascota != null ? t.Mascota.Nombre : string.Empty,
                NombrePropietario = t.Mascota != null && t.Mascota.Propietario != null
                    ? $"{t.Mascota.Propietario.Nombre} {t.Mascota.Propietario.Apellido}".Trim()
                    : string.Empty,
                IdVeterinario = t.IdVeterinario,
                NombreVeterinario = t.Veterinario != null
                    ? $"{t.Veterinario.Nombre} {t.Veterinario.Apellido}".Trim()
                    : string.Empty,
                IdConsulta = t.IdConsulta,
                FechaHora = t.FechaHora,
                Motivo = t.Motivo,
                Estado = t.Estado,
                Activo = t.Activo
            })
            .FirstOrDefaultAsync();

        if (turno is null)
            return Result<TurnoResponseDto>.Falla($"No se encontró el turno con ID {id}.");

        return Result<TurnoResponseDto>.Ok(turno);
    }

    public async Task<Result<long>> CrearAsync(TurnoRequestDto request)
    {
        if (request.IdMascota <= 0)
            return Result<long>.Falla("El identificador de la mascota debe ser mayor a cero.");

        if (request.IdVeterinario <= 0)
            return Result<long>.Falla("El identificador del veterinario debe ser mayor a cero.");

        if (request.FechaHora == default)
            return Result<long>.Falla("La fecha y hora del turno son obligatorias.");

        var mascotaExiste = await context.Mascotas.AnyAsync(m => m.Id == request.IdMascota);
        if (!mascotaExiste)
            return Result<long>.Falla($"No existe una mascota registrada con ID {request.IdMascota}.");

        var veterinarioExiste = await context.Usuarios.AnyAsync(u => u.Id == request.IdVeterinario);
        if (!veterinarioExiste)
            return Result<long>.Falla($"No existe un usuario veterinario registrado con ID {request.IdVeterinario}.");

        if (request.IdConsulta.HasValue && request.IdConsulta.Value > 0)
        {
            var consultaExiste = await context.Consultas.AnyAsync(c => c.Id == request.IdConsulta.Value);
            if (!consultaExiste)
                return Result<long>.Falla($"No existe una consulta registrada con ID {request.IdConsulta.Value}.");
        }

        var entidad = new Turno
        {
            IdMascota = request.IdMascota,
            IdVeterinario = request.IdVeterinario,
            IdConsulta = request.IdConsulta,
            FechaHora = request.FechaHora,
            Motivo = string.IsNullOrWhiteSpace(request.Motivo) ? null : request.Motivo.Trim(),
            Estado = string.IsNullOrWhiteSpace(request.Estado) ? "Pendiente" : request.Estado.Trim(),
            Activo = true
        };

        context.Turnos.Add(entidad);
        await context.SaveChangesAsync();

        return Result<long>.Ok(entidad.Id, "Turno registrado exitosamente.");
    }

    public async Task<Result> ActualizarAsync(long id, TurnoRequestDto request)
    {
        if (id <= 0)
            return Result.Falla("El identificador del turno debe ser mayor a cero.");

        if (request.IdMascota <= 0)
            return Result.Falla("El identificador de la mascota debe ser mayor a cero.");

        if (request.IdVeterinario <= 0)
            return Result.Falla("El identificador del veterinario debe ser mayor a cero.");

        if (request.FechaHora == default)
            return Result.Falla("La fecha y hora del turno son obligatorias.");

        var entidad = await context.Turnos.FirstOrDefaultAsync(t => t.Id == id);
        if (entidad is null)
            return Result.Falla($"No se encontró el turno con ID {id}.");

        var mascotaExiste = await context.Mascotas.AnyAsync(m => m.Id == request.IdMascota);
        if (!mascotaExiste)
            return Result.Falla($"No existe una mascota registrada con ID {request.IdMascota}.");

        var veterinarioExiste = await context.Usuarios.AnyAsync(u => u.Id == request.IdVeterinario);
        if (!veterinarioExiste)
            return Result.Falla($"No existe un usuario veterinario registrado con ID {request.IdVeterinario}.");

        if (request.IdConsulta.HasValue && request.IdConsulta.Value > 0)
        {
            var consultaExiste = await context.Consultas.AnyAsync(c => c.Id == request.IdConsulta.Value);
            if (!consultaExiste)
                return Result.Falla($"No existe una consulta registrada con ID {request.IdConsulta.Value}.");
        }

        entidad.IdMascota = request.IdMascota;
        entidad.IdVeterinario = request.IdVeterinario;
        entidad.IdConsulta = request.IdConsulta;
        entidad.FechaHora = request.FechaHora;
        entidad.Motivo = string.IsNullOrWhiteSpace(request.Motivo) ? null : request.Motivo.Trim();
        entidad.Estado = string.IsNullOrWhiteSpace(request.Estado) ? "Pendiente" : request.Estado.Trim();

        await context.SaveChangesAsync();

        return Result.Ok("Turno actualizado exitosamente.");
    }

    public async Task<Result> EliminarAsync(long id)
    {
        if (id <= 0)
            return Result.Falla("El identificador del turno debe ser mayor a cero.");

        var entidad = await context.Turnos.FirstOrDefaultAsync(t => t.Id == id);
        if (entidad is null)
            return Result.Falla($"No se encontró el turno con ID {id}.");

        entidad.Activo = false;
        await context.SaveChangesAsync();

        return Result.Ok("Turno eliminado exitosamente.");
    }
}
