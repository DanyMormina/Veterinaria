using Microsoft.EntityFrameworkCore;
using Veterinaria.CrossCutting.Comunes;
using Veterinaria.Domain.Dtos;
using Veterinaria.Domain.Entidades;
using Veterinaria.Infrastructure;
using Veterinaria.Interfaces.Interfaces;

namespace Veterinaria.Services.Servicios;

/// <summary>
/// Servicio para la gestión de Propietarios de pacientes veterinarios.
/// </summary>
public class PropietarioService(VeterinariaDbContext context) : IPropietarioService
{
    public async Task<Result<IEnumerable<PropietarioResponseDto>>> ObtenerTodosAsync()
    {
        var propietarios = await context.Propietarios
            .AsNoTracking()
            .Include(p => p.Mascotas)
            .Select(p => new PropietarioResponseDto
            {
                Id = p.Id,
                DNI = p.DNI,
                Nombre = p.Nombre,
                Apellido = p.Apellido,
                Telefono = p.Telefono,
                Email = p.Email,
                Direccion = p.Direccion,
                Activo = p.Activo,
                CantidadMascotas = p.Mascotas.Count
            })
            .ToListAsync();

        return Result<IEnumerable<PropietarioResponseDto>>.Ok(propietarios);
    }

    public async Task<Result<PropietarioResponseDto>> ObtenerPorIdAsync(long id)
    {
        if (id <= 0)
            return Result<PropietarioResponseDto>.Falla("El identificador del propietario debe ser mayor a cero.");

        var propietario = await context.Propietarios
            .AsNoTracking()
            .Include(p => p.Mascotas)
            .Where(p => p.Id == id)
            .Select(p => new PropietarioResponseDto
            {
                Id = p.Id,
                DNI = p.DNI,
                Nombre = p.Nombre,
                Apellido = p.Apellido,
                Telefono = p.Telefono,
                Email = p.Email,
                Direccion = p.Direccion,
                Activo = p.Activo,
                CantidadMascotas = p.Mascotas.Count
            })
            .FirstOrDefaultAsync();

        if (propietario is null)
            return Result<PropietarioResponseDto>.Falla($"No se encontró el propietario con ID {id}.");

        return Result<PropietarioResponseDto>.Ok(propietario);
    }

    public async Task<Result<long>> CrearAsync(PropietarioRequestDto request)
    {
        if (string.IsNullOrWhiteSpace(request.DNI))
            return Result<long>.Falla("El DNI del propietario es obligatorio.");

        if (string.IsNullOrWhiteSpace(request.Nombre))
            return Result<long>.Falla("El nombre del propietario es obligatorio.");

        if (string.IsNullOrWhiteSpace(request.Apellido))
            return Result<long>.Falla("El apellido del propietario es obligatorio.");

        var dniNormalizado = request.DNI.Trim();
        var existeDni = await context.Propietarios
            .AnyAsync(p => p.DNI.ToLower() == dniNormalizado.ToLower());

        if (existeDni)
            return Result<long>.Falla($"Ya existe un propietario registrado con el DNI '{dniNormalizado}'.");

        var entidad = new Propietario
        {
            DNI = dniNormalizado,
            Nombre = request.Nombre.Trim(),
            Apellido = request.Apellido.Trim(),
            Telefono = string.IsNullOrWhiteSpace(request.Telefono) ? null : request.Telefono.Trim(),
            Email = string.IsNullOrWhiteSpace(request.Email) ? null : request.Email.Trim(),
            Direccion = string.IsNullOrWhiteSpace(request.Direccion) ? null : request.Direccion.Trim(),
            Activo = true
        };

        context.Propietarios.Add(entidad);
        await context.SaveChangesAsync();

        return Result<long>.Ok(entidad.Id, "Propietario registrado exitosamente.");
    }

    public async Task<Result> ActualizarAsync(long id, PropietarioRequestDto request)
    {
        if (id <= 0)
            return Result.Falla("El identificador del propietario debe ser mayor a cero.");

        if (string.IsNullOrWhiteSpace(request.DNI))
            return Result.Falla("El DNI del propietario es obligatorio.");

        if (string.IsNullOrWhiteSpace(request.Nombre))
            return Result.Falla("El nombre del propietario es obligatorio.");

        if (string.IsNullOrWhiteSpace(request.Apellido))
            return Result.Falla("El apellido del propietario es obligatorio.");

        var entidad = await context.Propietarios.FirstOrDefaultAsync(p => p.Id == id);
        if (entidad is null)
            return Result.Falla($"No se encontró el propietario con ID {id}.");

        var dniNormalizado = request.DNI.Trim();
        var existeDni = await context.Propietarios
            .AnyAsync(p => p.Id != id && p.DNI.ToLower() == dniNormalizado.ToLower());

        if (existeDni)
            return Result.Falla($"Ya existe otro propietario registrado con el DNI '{dniNormalizado}'.");

        entidad.DNI = dniNormalizado;
        entidad.Nombre = request.Nombre.Trim();
        entidad.Apellido = request.Apellido.Trim();
        entidad.Telefono = string.IsNullOrWhiteSpace(request.Telefono) ? null : request.Telefono.Trim();
        entidad.Email = string.IsNullOrWhiteSpace(request.Email) ? null : request.Email.Trim();
        entidad.Direccion = string.IsNullOrWhiteSpace(request.Direccion) ? null : request.Direccion.Trim();

        await context.SaveChangesAsync();

        return Result.Ok("Propietario actualizado exitosamente.");
    }

    public async Task<Result> EliminarAsync(long id)
    {
        if (id <= 0)
            return Result.Falla("El identificador del propietario debe ser mayor a cero.");

        var entidad = await context.Propietarios.FirstOrDefaultAsync(p => p.Id == id);
        if (entidad is null)
            return Result.Falla($"No se encontró el propietario con ID {id}.");

        entidad.Activo = false;
        await context.SaveChangesAsync();

        return Result.Ok("Propietario eliminado exitosamente.");
    }
}
