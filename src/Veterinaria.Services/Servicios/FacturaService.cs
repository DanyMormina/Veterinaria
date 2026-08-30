using Microsoft.EntityFrameworkCore;
using Veterinaria.CrossCutting.Comunes;
using Veterinaria.Domain.Dtos;
using Veterinaria.Domain.Entidades;
using Veterinaria.Infrastructure;
using Veterinaria.Interfaces.Interfaces;

namespace Veterinaria.Services.Servicios;

/// <summary>
/// Servicio para la gestión transaccional de Facturación y comprobantes de cobro.
/// </summary>
public class FacturaService(VeterinariaDbContext context) : IFacturaService
{
    public async Task<Result<IEnumerable<FacturaResponseDto>>> ObtenerTodosAsync()
    {
        var facturas = await context.Facturas
            .AsNoTracking()
            .Include(f => f.Propietario)
            .Include(f => f.Usuario)
            .Include(f => f.MetodoPago)
            .Include(f => f.Detalles)
            .Select(f => new FacturaResponseDto
            {
                Id = f.Id,
                IdPropietario = f.IdPropietario,
                NombrePropietario = f.Propietario != null
                    ? $"{f.Propietario.Nombre} {f.Propietario.Apellido}".Trim()
                    : string.Empty,
                IdUsuario = f.IdUsuario,
                NombreUsuario = f.Usuario != null
                    ? $"{f.Usuario.Nombre} {f.Usuario.Apellido}".Trim()
                    : string.Empty,
                IdMetodoPago = f.IdMetodoPago,
                NombreMetodoPago = f.MetodoPago != null ? f.MetodoPago.Nombre : string.Empty,
                FechaEmision = f.FechaEmision,
                Total = f.Total,
                Activo = f.Activo,
                Detalles = f.Detalles.Select(d => new DetalleFacturaResponseDto
                {
                    Id = d.Id,
                    IdFactura = d.IdFactura,
                    Concepto = d.Concepto,
                    Cantidad = d.Cantidad,
                    PrecioUnitario = d.PrecioUnitario,
                    Subtotal = d.Subtotal,
                    Activo = d.Activo
                }).ToList()
            })
            .ToListAsync();

        return Result<IEnumerable<FacturaResponseDto>>.Ok(facturas);
    }

    public async Task<Result<FacturaResponseDto>> ObtenerPorIdAsync(long id)
    {
        if (id <= 0)
            return Result<FacturaResponseDto>.Falla("El identificador de la factura debe ser mayor a cero.");

        var factura = await context.Facturas
            .AsNoTracking()
            .Include(f => f.Propietario)
            .Include(f => f.Usuario)
            .Include(f => f.MetodoPago)
            .Include(f => f.Detalles)
            .Where(f => f.Id == id)
            .Select(f => new FacturaResponseDto
            {
                Id = f.Id,
                IdPropietario = f.IdPropietario,
                NombrePropietario = f.Propietario != null
                    ? $"{f.Propietario.Nombre} {f.Propietario.Apellido}".Trim()
                    : string.Empty,
                IdUsuario = f.IdUsuario,
                NombreUsuario = f.Usuario != null
                    ? $"{f.Usuario.Nombre} {f.Usuario.Apellido}".Trim()
                    : string.Empty,
                IdMetodoPago = f.IdMetodoPago,
                NombreMetodoPago = f.MetodoPago != null ? f.MetodoPago.Nombre : string.Empty,
                FechaEmision = f.FechaEmision,
                Total = f.Total,
                Activo = f.Activo,
                Detalles = f.Detalles.Select(d => new DetalleFacturaResponseDto
                {
                    Id = d.Id,
                    IdFactura = d.IdFactura,
                    Concepto = d.Concepto,
                    Cantidad = d.Cantidad,
                    PrecioUnitario = d.PrecioUnitario,
                    Subtotal = d.Subtotal,
                    Activo = d.Activo
                }).ToList()
            })
            .FirstOrDefaultAsync();

        if (factura is null)
            return Result<FacturaResponseDto>.Falla($"No se encontró la factura con ID {id}.");

        return Result<FacturaResponseDto>.Ok(factura);
    }

    public async Task<Result<long>> CrearAsync(FacturaRequestDto request)
    {
        if (request.IdPropietario <= 0)
            return Result<long>.Falla("El identificador del propietario debe ser mayor a cero.");

        if (request.IdUsuario <= 0)
            return Result<long>.Falla("El identificador del usuario debe ser mayor a cero.");

        if (request.IdMetodoPago <= 0)
            return Result<long>.Falla("El identificador del método de pago debe ser mayor a cero.");

        if (request.Total < 0)
            return Result<long>.Falla("El total de la factura no puede ser negativo.");

        var propietarioExiste = await context.Propietarios.AnyAsync(p => p.Id == request.IdPropietario);
        if (!propietarioExiste)
            return Result<long>.Falla($"No existe un propietario registrado con ID {request.IdPropietario}.");

        var usuarioExiste = await context.Usuarios.AnyAsync(u => u.Id == request.IdUsuario);
        if (!usuarioExiste)
            return Result<long>.Falla($"No existe un usuario registrado con ID {request.IdUsuario}.");

        var metodoExiste = await context.MetodosPago.AnyAsync(m => m.Id == request.IdMetodoPago);
        if (!metodoExiste)
            return Result<long>.Falla($"No existe un método de pago registrado con ID {request.IdMetodoPago}.");

        var entidad = new Factura
        {
            IdPropietario = request.IdPropietario,
            IdUsuario = request.IdUsuario,
            IdMetodoPago = request.IdMetodoPago,
            FechaEmision = request.FechaEmision == default ? DateTime.Now : request.FechaEmision,
            Total = request.Total,
            Activo = true
        };

        if (request.Detalles != null && request.Detalles.Count > 0)
        {
            decimal totalCalculado = 0;
            foreach (var det in request.Detalles)
            {
                if (string.IsNullOrWhiteSpace(det.Concepto))
                    return Result<long>.Falla("Todos los detalles deben tener un concepto válido.");

                if (det.Cantidad <= 0)
                    return Result<long>.Falla("La cantidad en cada detalle debe ser mayor a cero.");

                var subtotal = det.Subtotal > 0 ? det.Subtotal : (det.Cantidad * det.PrecioUnitario);
                totalCalculado += subtotal;

                entidad.Detalles.Add(new DetalleFactura
                {
                    Concepto = det.Concepto.Trim(),
                    Cantidad = det.Cantidad,
                    PrecioUnitario = det.PrecioUnitario,
                    Subtotal = subtotal,
                    Activo = true
                });
            }

            if (entidad.Total == 0)
            {
                entidad.Total = totalCalculado;
            }
        }

        context.Facturas.Add(entidad);
        await context.SaveChangesAsync();

        return Result<long>.Ok(entidad.Id, "Factura generada exitosamente.");
    }

    public async Task<Result> ActualizarAsync(long id, FacturaRequestDto request)
    {
        if (id <= 0)
            return Result.Falla("El identificador de la factura debe ser mayor a cero.");

        if (request.IdPropietario <= 0)
            return Result.Falla("El identificador del propietario debe ser mayor a cero.");

        if (request.IdUsuario <= 0)
            return Result.Falla("El identificador del usuario debe ser mayor a cero.");

        if (request.IdMetodoPago <= 0)
            return Result.Falla("El identificador del método de pago debe ser mayor a cero.");

        if (request.Total < 0)
            return Result.Falla("El total de la factura no puede ser negativo.");

        var entidad = await context.Facturas.FirstOrDefaultAsync(f => f.Id == id);
        if (entidad is null)
            return Result.Falla($"No se encontró la factura con ID {id}.");

        var propietarioExiste = await context.Propietarios.AnyAsync(p => p.Id == request.IdPropietario);
        if (!propietarioExiste)
            return Result.Falla($"No existe un propietario registrado con ID {request.IdPropietario}.");

        var usuarioExiste = await context.Usuarios.AnyAsync(u => u.Id == request.IdUsuario);
        if (!usuarioExiste)
            return Result.Falla($"No existe un usuario registrado con ID {request.IdUsuario}.");

        var metodoExiste = await context.MetodosPago.AnyAsync(m => m.Id == request.IdMetodoPago);
        if (!metodoExiste)
            return Result.Falla($"No existe un método de pago registrado con ID {request.IdMetodoPago}.");

        entidad.IdPropietario = request.IdPropietario;
        entidad.IdUsuario = request.IdUsuario;
        entidad.IdMetodoPago = request.IdMetodoPago;
        entidad.FechaEmision = request.FechaEmision == default ? entidad.FechaEmision : request.FechaEmision;
        entidad.Total = request.Total;

        await context.SaveChangesAsync();

        return Result.Ok("Factura actualizada exitosamente.");
    }

    public async Task<Result> EliminarAsync(long id)
    {
        if (id <= 0)
            return Result.Falla("El identificador de la factura debe ser mayor a cero.");

        var entidad = await context.Facturas
            .Include(f => f.Detalles)
            .FirstOrDefaultAsync(f => f.Id == id);

        if (entidad is null)
            return Result.Falla($"No se encontró la factura con ID {id}.");

        entidad.Activo = false;
        foreach (var detalle in entidad.Detalles)
        {
            detalle.Activo = false;
        }

        await context.SaveChangesAsync();

        return Result.Ok("Factura eliminada exitosamente.");
    }
}
