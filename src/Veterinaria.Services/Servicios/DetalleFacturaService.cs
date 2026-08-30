using Microsoft.EntityFrameworkCore;
using Veterinaria.CrossCutting.Comunes;
using Veterinaria.Domain.Dtos;
using Veterinaria.Domain.Entidades;
using Veterinaria.Infrastructure;
using Veterinaria.Interfaces.Interfaces;

namespace Veterinaria.Services.Servicios;

/// <summary>
/// Servicio para la gestión de Ítems y Detalles de Facturación.
/// </summary>
public class DetalleFacturaService(VeterinariaDbContext context) : IDetalleFacturaService
{
    public async Task<Result<IEnumerable<DetalleFacturaResponseDto>>> ObtenerTodosAsync()
    {
        var detalles = await context.DetalleFacturas
            .AsNoTracking()
            .Select(d => new DetalleFacturaResponseDto
            {
                Id = d.Id,
                IdFactura = d.IdFactura,
                Concepto = d.Concepto,
                Cantidad = d.Cantidad,
                PrecioUnitario = d.PrecioUnitario,
                Subtotal = d.Subtotal,
                Activo = d.Activo
            })
            .ToListAsync();

        return Result<IEnumerable<DetalleFacturaResponseDto>>.Ok(detalles);
    }

    public async Task<Result<DetalleFacturaResponseDto>> ObtenerPorIdAsync(long id)
    {
        if (id <= 0)
            return Result<DetalleFacturaResponseDto>.Falla("El identificador del detalle de factura debe ser mayor a cero.");

        var detalle = await context.DetalleFacturas
            .AsNoTracking()
            .Where(d => d.Id == id)
            .Select(d => new DetalleFacturaResponseDto
            {
                Id = d.Id,
                IdFactura = d.IdFactura,
                Concepto = d.Concepto,
                Cantidad = d.Cantidad,
                PrecioUnitario = d.PrecioUnitario,
                Subtotal = d.Subtotal,
                Activo = d.Activo
            })
            .FirstOrDefaultAsync();

        if (detalle is null)
            return Result<DetalleFacturaResponseDto>.Falla($"No se encontró el detalle de factura con ID {id}.");

        return Result<DetalleFacturaResponseDto>.Ok(detalle);
    }

    public async Task<Result<long>> CrearAsync(DetalleFacturaRequestDto request)
    {
        if (request.IdFactura <= 0)
            return Result<long>.Falla("El identificador de la factura debe ser mayor a cero.");

        if (string.IsNullOrWhiteSpace(request.Concepto))
            return Result<long>.Falla("El concepto del detalle de factura es obligatorio.");

        if (request.Cantidad <= 0)
            return Result<long>.Falla("La cantidad debe ser mayor a cero.");

        if (request.PrecioUnitario < 0)
            return Result<long>.Falla("El precio unitario no puede ser negativo.");

        var facturaExiste = await context.Facturas.AnyAsync(f => f.Id == request.IdFactura);
        if (!facturaExiste)
            return Result<long>.Falla($"No existe una factura registrada con ID {request.IdFactura}.");

        var subtotal = request.Subtotal > 0 ? request.Subtotal : (request.Cantidad * request.PrecioUnitario);

        var entidad = new DetalleFactura
        {
            IdFactura = request.IdFactura,
            Concepto = request.Concepto.Trim(),
            Cantidad = request.Cantidad,
            PrecioUnitario = request.PrecioUnitario,
            Subtotal = subtotal,
            Activo = true
        };

        context.DetalleFacturas.Add(entidad);
        await context.SaveChangesAsync();

        return Result<long>.Ok(entidad.Id, "Detalle de factura creado exitosamente.");
    }

    public async Task<Result> ActualizarAsync(long id, DetalleFacturaRequestDto request)
    {
        if (id <= 0)
            return Result.Falla("El identificador del detalle de factura debe ser mayor a cero.");

        if (request.IdFactura <= 0)
            return Result.Falla("El identificador de la factura debe ser mayor a cero.");

        if (string.IsNullOrWhiteSpace(request.Concepto))
            return Result.Falla("El concepto del detalle de factura es obligatorio.");

        if (request.Cantidad <= 0)
            return Result.Falla("La cantidad debe ser mayor a cero.");

        if (request.PrecioUnitario < 0)
            return Result.Falla("El precio unitario no puede ser negativo.");

        var entidad = await context.DetalleFacturas.FirstOrDefaultAsync(d => d.Id == id);
        if (entidad is null)
            return Result.Falla($"No se encontró el detalle de factura con ID {id}.");

        var facturaExiste = await context.Facturas.AnyAsync(f => f.Id == request.IdFactura);
        if (!facturaExiste)
            return Result.Falla($"No existe una factura registrada con ID {request.IdFactura}.");

        var subtotal = request.Subtotal > 0 ? request.Subtotal : (request.Cantidad * request.PrecioUnitario);

        entidad.IdFactura = request.IdFactura;
        entidad.Concepto = request.Concepto.Trim();
        entidad.Cantidad = request.Cantidad;
        entidad.PrecioUnitario = request.PrecioUnitario;
        entidad.Subtotal = subtotal;

        await context.SaveChangesAsync();

        return Result.Ok("Detalle de factura actualizado exitosamente.");
    }

    public async Task<Result> EliminarAsync(long id)
    {
        if (id <= 0)
            return Result.Falla("El identificador del detalle de factura debe ser mayor a cero.");

        var entidad = await context.DetalleFacturas.FirstOrDefaultAsync(d => d.Id == id);
        if (entidad is null)
            return Result.Falla($"No se encontró el detalle de factura con ID {id}.");

        entidad.Activo = false;
        await context.SaveChangesAsync();

        return Result.Ok("Detalle de factura eliminado exitosamente.");
    }
}
