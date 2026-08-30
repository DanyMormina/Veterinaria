using Microsoft.EntityFrameworkCore;
using Veterinaria.CrossCutting.Comunes;
using Veterinaria.Domain.Dtos;
using Veterinaria.Domain.Entidades;
using Veterinaria.Infrastructure;
using Veterinaria.Interfaces.Interfaces;

namespace Veterinaria.Services.Servicios;

/// <summary>
/// Servicio para la gestión de Roles.
/// </summary>
public class RolService(VeterinariaDbContext context) : IRolService
{
    public async Task<Result<IEnumerable<RolResponseDto>>> ObtenerTodosAsync()
    {
        var roles = await context.Roles
            .AsNoTracking()
            .Select(r => new RolResponseDto
            {
                Id = r.Id,
                Nombre = r.Nombre,
                Activo = r.Activo
            })
            .ToListAsync();

        return Result<IEnumerable<RolResponseDto>>.Ok(roles);
    }

    public async Task<Result<RolResponseDto>> ObtenerPorIdAsync(long id)
    {
        if (id <= 0)
            return Result<RolResponseDto>.Falla("El identificador del rol debe ser mayor a cero.");

        var rol = await context.Roles
            .AsNoTracking()
            .Where(r => r.Id == id)
            .Select(r => new RolResponseDto
            {
                Id = r.Id,
                Nombre = r.Nombre,
                Activo = r.Activo
            })
            .FirstOrDefaultAsync();

        if (rol is null)
            return Result<RolResponseDto>.Falla($"No se encontró el rol con ID {id}.");

        return Result<RolResponseDto>.Ok(rol);
    }

    public async Task<Result<long>> CrearAsync(RolRequestDto request)
    {
        if (string.IsNullOrWhiteSpace(request.Nombre))
            return Result<long>.Falla("El nombre del rol es obligatorio.");

        var nombreNormalizado = request.Nombre.Trim();
        var existe = await context.Roles
            .AnyAsync(r => r.Nombre.ToLower() == nombreNormalizado.ToLower());

        if (existe)
            return Result<long>.Falla($"Ya existe un rol registrado con el nombre '{nombreNormalizado}'.");

        var entidad = new Rol
        {
            Nombre = nombreNormalizado,
            Activo = true
        };

        context.Roles.Add(entidad);
        await context.SaveChangesAsync();

        return Result<long>.Ok(entidad.Id, "Rol creado exitosamente.");
    }

    public async Task<Result> ActualizarAsync(long id, RolRequestDto request)
    {
        if (id <= 0)
            return Result.Falla("El identificador del rol debe ser mayor a cero.");

        if (string.IsNullOrWhiteSpace(request.Nombre))
            return Result.Falla("El nombre del rol es obligatorio.");

        var entidad = await context.Roles.FirstOrDefaultAsync(r => r.Id == id);
        if (entidad is null)
            return Result.Falla($"No se encontró el rol con ID {id}.");

        var nombreNormalizado = request.Nombre.Trim();
        var existeDuplicado = await context.Roles
            .AnyAsync(r => r.Id != id && r.Nombre.ToLower() == nombreNormalizado.ToLower());

        if (existeDuplicado)
            return Result.Falla($"Ya existe otro rol registrado con el nombre '{nombreNormalizado}'.");

        entidad.Nombre = nombreNormalizado;
        await context.SaveChangesAsync();

        return Result.Ok("Rol actualizado exitosamente.");
    }

    public async Task<Result> EliminarAsync(long id)
    {
        if (id <= 0)
            return Result.Falla("El identificador del rol debe ser mayor a cero.");

        var entidad = await context.Roles.FirstOrDefaultAsync(r => r.Id == id);
        if (entidad is null)
            return Result.Falla($"No se encontró el rol con ID {id}.");

        entidad.Activo = false;
        await context.SaveChangesAsync();

        return Result.Ok("Rol eliminado exitosamente.");
    }
}
