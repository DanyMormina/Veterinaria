using Microsoft.EntityFrameworkCore;
using Veterinaria.CrossCutting.Comunes;
using Veterinaria.Domain.Dtos;
using Veterinaria.Domain.Entidades;
using Veterinaria.Infrastructure;
using Veterinaria.Interfaces.Interfaces;

namespace Veterinaria.Services.Servicios;

/// <summary>
/// Servicio para la gestión de Métodos de Pago.
/// </summary>
public class MetodoPagoService(VeterinariaDbContext context) : IMetodoPagoService
{
    public async Task<Result<IEnumerable<MetodoPagoResponseDto>>> ObtenerTodosAsync()
    {
        var metodos = await context.MetodosPago
            .AsNoTracking()
            .Select(m => new MetodoPagoResponseDto
            {
                Id = m.Id,
                Nombre = m.Nombre,
                Activo = m.Activo
            })
            .ToListAsync();

        return Result<IEnumerable<MetodoPagoResponseDto>>.Ok(metodos);
    }

    public async Task<Result<MetodoPagoResponseDto>> ObtenerPorIdAsync(long id)
    {
        if (id <= 0)
            return Result<MetodoPagoResponseDto>.Falla("El identificador del método de pago debe ser mayor a cero.");

        var metodo = await context.MetodosPago
            .AsNoTracking()
            .Where(m => m.Id == id)
            .Select(m => new MetodoPagoResponseDto
            {
                Id = m.Id,
                Nombre = m.Nombre,
                Activo = m.Activo
            })
            .FirstOrDefaultAsync();

        if (metodo is null)
            return Result<MetodoPagoResponseDto>.Falla($"No se encontró el método de pago con ID {id}.");

        return Result<MetodoPagoResponseDto>.Ok(metodo);
    }

    public async Task<Result<long>> CrearAsync(MetodoPagoRequestDto request)
    {
        if (string.IsNullOrWhiteSpace(request.Nombre))
            return Result<long>.Falla("El nombre del método de pago es obligatorio.");

        var nombreNormalizado = request.Nombre.Trim();
        var existe = await context.MetodosPago
            .AnyAsync(m => m.Nombre.ToLower() == nombreNormalizado.ToLower());

        if (existe)
            return Result<long>.Falla($"Ya existe un método de pago registrado con el nombre '{nombreNormalizado}'.");

        var entidad = new MetodoPago
        {
            Nombre = nombreNormalizado,
            Activo = true
        };

        context.MetodosPago.Add(entidad);
        await context.SaveChangesAsync();

        return Result<long>.Ok(entidad.Id, "Método de pago creado exitosamente.");
    }

    public async Task<Result> ActualizarAsync(long id, MetodoPagoRequestDto request)
    {
        if (id <= 0)
            return Result.Falla("El identificador del método de pago debe ser mayor a cero.");

        if (string.IsNullOrWhiteSpace(request.Nombre))
            return Result.Falla("El nombre del método de pago es obligatorio.");

        var entidad = await context.MetodosPago.FirstOrDefaultAsync(m => m.Id == id);
        if (entidad is null)
            return Result.Falla($"No se encontró el método de pago con ID {id}.");

        var nombreNormalizado = request.Nombre.Trim();
        var existeDuplicado = await context.MetodosPago
            .AnyAsync(m => m.Id != id && m.Nombre.ToLower() == nombreNormalizado.ToLower());

        if (existeDuplicado)
            return Result.Falla($"Ya existe otro método de pago registrado con el nombre '{nombreNormalizado}'.");

        entidad.Nombre = nombreNormalizado;
        await context.SaveChangesAsync();

        return Result.Ok("Método de pago actualizado exitosamente.");
    }

    public async Task<Result> EliminarAsync(long id)
    {
        if (id <= 0)
            return Result.Falla("El identificador del método de pago debe ser mayor a cero.");

        var entidad = await context.MetodosPago.FirstOrDefaultAsync(m => m.Id == id);
        if (entidad is null)
            return Result.Falla($"No se encontró el método de pago con ID {id}.");

        entidad.Activo = false;
        await context.SaveChangesAsync();

        return Result.Ok("Método de pago eliminado exitosamente.");
    }
}
