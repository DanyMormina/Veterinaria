using Microsoft.EntityFrameworkCore;
using Veterinaria.CrossCutting.Comunes;
using Veterinaria.Domain.Dtos;
using Veterinaria.Domain.Entidades;
using Veterinaria.Infrastructure;
using Veterinaria.Interfaces.Interfaces;

namespace Veterinaria.Services.Servicios;

/// <summary>
/// Servicio para la gestión y auditoría de Sesiones de usuario.
/// </summary>
public class SesionService(VeterinariaDbContext context) : ISesionService
{
    public async Task<Result<IEnumerable<SesionResponseDto>>> ObtenerTodosAsync()
    {
        var sesiones = await context.Sesiones
            .AsNoTracking()
            .Include(s => s.Usuario)
            .Select(s => new SesionResponseDto
            {
                Id = s.Id,
                IdUsuario = s.IdUsuario,
                NombreUsuario = s.Usuario != null ? $"{s.Usuario.Nombre} {s.Usuario.Apellido}".Trim() : string.Empty,
                FechaInicio = s.FechaInicio,
                FechaCierre = s.FechaCierre,
                Activo = s.Activo
            })
            .ToListAsync();

        return Result<IEnumerable<SesionResponseDto>>.Ok(sesiones);
    }

    public async Task<Result<SesionResponseDto>> ObtenerPorIdAsync(long id)
    {
        if (id <= 0)
            return Result<SesionResponseDto>.Falla("El identificador de la sesión debe ser mayor a cero.");

        var sesion = await context.Sesiones
            .AsNoTracking()
            .Include(s => s.Usuario)
            .Where(s => s.Id == id)
            .Select(s => new SesionResponseDto
            {
                Id = s.Id,
                IdUsuario = s.IdUsuario,
                NombreUsuario = s.Usuario != null ? $"{s.Usuario.Nombre} {s.Usuario.Apellido}".Trim() : string.Empty,
                FechaInicio = s.FechaInicio,
                FechaCierre = s.FechaCierre,
                Activo = s.Activo
            })
            .FirstOrDefaultAsync();

        if (sesion is null)
            return Result<SesionResponseDto>.Falla($"No se encontró la sesión con ID {id}.");

        return Result<SesionResponseDto>.Ok(sesion);
    }

    public async Task<Result<long>> CrearAsync(SesionRequestDto request)
    {
        if (request.IdUsuario <= 0)
            return Result<long>.Falla("El identificador del usuario debe ser mayor a cero.");

        var usuarioExiste = await context.Usuarios.AnyAsync(u => u.Id == request.IdUsuario);
        if (!usuarioExiste)
            return Result<long>.Falla($"No existe un usuario registrado con ID {request.IdUsuario}.");

        var entidad = new Sesion
        {
            IdUsuario = request.IdUsuario,
            FechaInicio = DateTime.Now,
            FechaCierre = request.FechaCierre,
            Activo = true
        };

        context.Sesiones.Add(entidad);
        await context.SaveChangesAsync();

        return Result<long>.Ok(entidad.Id, "Sesión registrada exitosamente.");
    }

    public async Task<Result> ActualizarAsync(long id, SesionRequestDto request)
    {
        if (id <= 0)
            return Result.Falla("El identificador de la sesión debe ser mayor a cero.");

        if (request.IdUsuario <= 0)
            return Result.Falla("El identificador del usuario debe ser mayor a cero.");

        var entidad = await context.Sesiones.FirstOrDefaultAsync(s => s.Id == id);
        if (entidad is null)
            return Result.Falla($"No se encontró la sesión con ID {id}.");

        var usuarioExiste = await context.Usuarios.AnyAsync(u => u.Id == request.IdUsuario);
        if (!usuarioExiste)
            return Result.Falla($"No existe un usuario registrado con ID {request.IdUsuario}.");

        entidad.IdUsuario = request.IdUsuario;
        entidad.FechaCierre = request.FechaCierre;
        await context.SaveChangesAsync();

        return Result.Ok("Sesión actualizada exitosamente.");
    }

    public async Task<Result> EliminarAsync(long id)
    {
        if (id <= 0)
            return Result.Falla("El identificador de la sesión debe ser mayor a cero.");

        var entidad = await context.Sesiones.FirstOrDefaultAsync(s => s.Id == id);
        if (entidad is null)
            return Result.Falla($"No se encontró la sesión con ID {id}.");

        entidad.Activo = false;
        await context.SaveChangesAsync();

        return Result.Ok("Sesión eliminada exitosamente.");
    }
}
