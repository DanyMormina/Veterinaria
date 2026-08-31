using Microsoft.EntityFrameworkCore;
using Veterinaria.CrossCutting.Comunes;
using Veterinaria.Domain.Dtos;
using Veterinaria.Domain.Entidades;
using Veterinaria.Infrastructure;
using Veterinaria.Interfaces.Interfaces;

namespace Veterinaria.Services.Servicios;

public class TipoUsuarioService(VeterinariaDbContext context) : ITipoUsuarioService
{
    public async Task<Result<IEnumerable<TipoUsuarioResponseDto>>> ObtenerTodosAsync()
    {
        var list = await context.TiposUsuario
            .AsNoTracking()
            .Select(t => new TipoUsuarioResponseDto
            {
                Id = t.Id,
                Nombre = t.Nombre,
                Activo = t.Activo
            })
            .ToListAsync();

        return Result<IEnumerable<TipoUsuarioResponseDto>>.Ok(list);
    }

    public async Task<Result<TipoUsuarioResponseDto>> ObtenerPorIdAsync(long id)
    {
        if (id <= 0)
            return Result<TipoUsuarioResponseDto>.Falla("El identificador debe ser mayor a cero.");

        var item = await context.TiposUsuario
            .AsNoTracking()
            .Where(t => t.Id == id)
            .Select(t => new TipoUsuarioResponseDto
            {
                Id = t.Id,
                Nombre = t.Nombre,
                Activo = t.Activo
            })
            .FirstOrDefaultAsync();

        if (item is null)
            return Result<TipoUsuarioResponseDto>.Falla($"No se encontró el tipo de usuario con ID {id}.");

        return Result<TipoUsuarioResponseDto>.Ok(item);
    }

    public async Task<Result<long>> CrearAsync(TipoUsuarioRequestDto request)
    {
        if (string.IsNullOrWhiteSpace(request.Nombre))
            return Result<long>.Falla("El nombre del tipo de usuario es obligatorio.");

        var nombreNormalizado = request.Nombre.Trim();
        var existe = await context.TiposUsuario.AnyAsync(t => t.Nombre.ToLower() == nombreNormalizado.ToLower());
        if (existe)
            return Result<long>.Falla($"Ya existe un tipo de usuario con el nombre '{nombreNormalizado}'.");

        var entidad = new TipoUsuario
        {
            Nombre = nombreNormalizado,
            Activo = true
        };

        context.TiposUsuario.Add(entidad);
        await context.SaveChangesAsync();

        return Result<long>.Ok(entidad.Id, "Tipo de usuario registrado exitosamente.");
    }

    public async Task<Result> ActualizarAsync(long id, TipoUsuarioRequestDto request)
    {
        if (id <= 0)
            return Result.Falla("El identificador debe ser mayor a cero.");

        if (string.IsNullOrWhiteSpace(request.Nombre))
            return Result.Falla("El nombre del tipo de usuario es obligatorio.");

        var entidad = await context.TiposUsuario.FirstOrDefaultAsync(t => t.Id == id);
        if (entidad is null)
            return Result.Falla($"No se encontró el tipo de usuario con ID {id}.");

        var nombreNormalizado = request.Nombre.Trim();
        var existe = await context.TiposUsuario.AnyAsync(t => t.Id != id && t.Nombre.ToLower() == nombreNormalizado.ToLower());
        if (existe)
            return Result.Falla($"Ya existe otro tipo de usuario con el nombre '{nombreNormalizado}'.");

        entidad.Nombre = nombreNormalizado;
        await context.SaveChangesAsync();

        return Result.Ok("Tipo de usuario actualizado exitosamente.");
    }

    public async Task<Result> EliminarAsync(long id)
    {
        if (id <= 0)
            return Result.Falla("El identificador debe ser mayor a cero.");

        var entidad = await context.TiposUsuario.FirstOrDefaultAsync(t => t.Id == id);
        if (entidad is null)
            return Result.Falla($"No se encontró el tipo de usuario con ID {id}.");

        entidad.Activo = false;
        await context.SaveChangesAsync();

        return Result.Ok("Tipo de usuario eliminado exitosamente.");
    }
}
