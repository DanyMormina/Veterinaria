using Microsoft.EntityFrameworkCore;
using Veterinaria.CrossCutting.Comunes;
using Veterinaria.Domain.Dtos;
using Veterinaria.Domain.Entidades;
using Veterinaria.Infrastructure;
using Veterinaria.Interfaces.Interfaces;

namespace Veterinaria.Services.Servicios;

/// <summary>
/// Servicio para el registro y consulta de Auditoría de operaciones del sistema.
/// </summary>
public class AuditoriaService(VeterinariaDbContext context) : IAuditoriaService
{
    public async Task<Result<IEnumerable<AuditoriaResponseDto>>> ObtenerTodosAsync()
    {
        var logs = await context.Auditorias
            .AsNoTracking()
            .Include(a => a.Usuario)
            .Select(a => new AuditoriaResponseDto
            {
                Id = a.Id,
                IdUsuario = a.IdUsuario,
                NombreUsuario = a.Usuario != null ? $"{a.Usuario.Nombre} {a.Usuario.Apellido}".Trim() : string.Empty,
                FechaHora = a.FechaHora,
                Accion = a.Accion,
                TablaAfectada = a.TablaAfectada,
                Detalle = a.Detalle,
                Activo = a.Activo
            })
            .ToListAsync();

        return Result<IEnumerable<AuditoriaResponseDto>>.Ok(logs);
    }

    public async Task<Result<AuditoriaResponseDto>> ObtenerPorIdAsync(long id)
    {
        if (id <= 0)
            return Result<AuditoriaResponseDto>.Falla("El identificador del registro de auditoría debe ser mayor a cero.");

        var log = await context.Auditorias
            .AsNoTracking()
            .Include(a => a.Usuario)
            .Where(a => a.Id == id)
            .Select(a => new AuditoriaResponseDto
            {
                Id = a.Id,
                IdUsuario = a.IdUsuario,
                NombreUsuario = a.Usuario != null ? $"{a.Usuario.Nombre} {a.Usuario.Apellido}".Trim() : string.Empty,
                FechaHora = a.FechaHora,
                Accion = a.Accion,
                TablaAfectada = a.TablaAfectada,
                Detalle = a.Detalle,
                Activo = a.Activo
            })
            .FirstOrDefaultAsync();

        if (log is null)
            return Result<AuditoriaResponseDto>.Falla($"No se encontró el registro de auditoría con ID {id}.");

        return Result<AuditoriaResponseDto>.Ok(log);
    }

    public async Task<Result<long>> CrearAsync(AuditoriaRequestDto request)
    {
        if (request.IdUsuario <= 0)
            return Result<long>.Falla("El identificador del usuario debe ser mayor a cero.");

        if (string.IsNullOrWhiteSpace(request.Accion))
            return Result<long>.Falla("La acción auditada es obligatoria.");

        if (string.IsNullOrWhiteSpace(request.TablaAfectada))
            return Result<long>.Falla("La tabla afectada es obligatoria.");

        var usuarioExiste = await context.Usuarios.AnyAsync(u => u.Id == request.IdUsuario);
        if (!usuarioExiste)
            return Result<long>.Falla($"No existe un usuario registrado con ID {request.IdUsuario}.");

        var entidad = new Auditoria
        {
            IdUsuario = request.IdUsuario,
            FechaHora = DateTime.Now,
            Accion = request.Accion.Trim(),
            TablaAfectada = request.TablaAfectada.Trim(),
            Detalle = request.Detalle?.Trim() ?? string.Empty,
            Activo = true
        };

        context.Auditorias.Add(entidad);
        await context.SaveChangesAsync();

        return Result<long>.Ok(entidad.Id, "Registro de auditoría guardado exitosamente.");
    }

    public async Task<Result> ActualizarAsync(long id, AuditoriaRequestDto request)
    {
        if (id <= 0)
            return Result.Falla("El identificador del registro de auditoría debe ser mayor a cero.");

        if (request.IdUsuario <= 0)
            return Result.Falla("El identificador del usuario debe ser mayor a cero.");

        if (string.IsNullOrWhiteSpace(request.Accion))
            return Result.Falla("La acción auditada es obligatoria.");

        if (string.IsNullOrWhiteSpace(request.TablaAfectada))
            return Result.Falla("La tabla afectada es obligatoria.");

        var entidad = await context.Auditorias.FirstOrDefaultAsync(a => a.Id == id);
        if (entidad is null)
            return Result.Falla($"No se encontró el registro de auditoría con ID {id}.");

        var usuarioExiste = await context.Usuarios.AnyAsync(u => u.Id == request.IdUsuario);
        if (!usuarioExiste)
            return Result.Falla($"No existe un usuario registrado con ID {request.IdUsuario}.");

        entidad.IdUsuario = request.IdUsuario;
        entidad.Accion = request.Accion.Trim();
        entidad.TablaAfectada = request.TablaAfectada.Trim();
        entidad.Detalle = request.Detalle?.Trim() ?? string.Empty;
        await context.SaveChangesAsync();

        return Result.Ok("Registro de auditoría actualizado exitosamente.");
    }

    public async Task<Result> EliminarAsync(long id)
    {
        if (id <= 0)
            return Result.Falla("El identificador del registro de auditoría debe ser mayor a cero.");

        var entidad = await context.Auditorias.FirstOrDefaultAsync(a => a.Id == id);
        if (entidad is null)
            return Result.Falla($"No se encontró el registro de auditoría con ID {id}.");

        entidad.Activo = false;
        await context.SaveChangesAsync();

        return Result.Ok("Registro de auditoría eliminado exitosamente.");
    }
}
