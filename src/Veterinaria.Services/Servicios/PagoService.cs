using Microsoft.EntityFrameworkCore;
using Veterinaria.CrossCutting.Comunes;
using Veterinaria.Domain.Dtos;
using Veterinaria.Domain.Entidades;
using Veterinaria.Infrastructure;
using Veterinaria.Interfaces.Interfaces;

namespace Veterinaria.Services.Servicios;

public class PagoService(VeterinariaDbContext context) : IPagoService
{
    public async Task<Result<IEnumerable<PagoResponseDto>>> ObtenerPorConsultaAsync(long idConsulta)
    {
        var items = await context.Pagos
            .AsNoTracking()
            .Include(p => p.MetodoPago)
            .Where(p => p.IdConsulta == idConsulta)
            .Select(p => new PagoResponseDto
            {
                Id = p.Id,
                IdConsulta = p.IdConsulta,
                IdMetodoPago = p.IdMetodoPago,
                NombreMetodoPago = p.MetodoPago != null ? p.MetodoPago.Nombre : string.Empty,
                Fecha = p.Fecha,
                Importe = p.Importe,
                Estado = p.Estado,
                Activo = p.Activo
            })
            .ToListAsync();

        return Result<IEnumerable<PagoResponseDto>>.Ok(items);
    }

    public async Task<Result<IEnumerable<PagoResponseDto>>> ObtenerTodosAsync()
    {
        var items = await context.Pagos
            .AsNoTracking()
            .Include(p => p.MetodoPago)
            .Select(p => new PagoResponseDto
            {
                Id = p.Id,
                IdConsulta = p.IdConsulta,
                IdMetodoPago = p.IdMetodoPago,
                NombreMetodoPago = p.MetodoPago != null ? p.MetodoPago.Nombre : string.Empty,
                Fecha = p.Fecha,
                Importe = p.Importe,
                Estado = p.Estado,
                Activo = p.Activo
            })
            .ToListAsync();

        return Result<IEnumerable<PagoResponseDto>>.Ok(items);
    }

    public async Task<Result<PagoResponseDto>> ObtenerPorIdAsync(long id)
    {
        if (id <= 0)
            return Result<PagoResponseDto>.Falla("El identificador debe ser mayor a cero.");

        var item = await context.Pagos
            .AsNoTracking()
            .Include(p => p.MetodoPago)
            .Where(p => p.Id == id)
            .Select(p => new PagoResponseDto
            {
                Id = p.Id,
                IdConsulta = p.IdConsulta,
                IdMetodoPago = p.IdMetodoPago,
                NombreMetodoPago = p.MetodoPago != null ? p.MetodoPago.Nombre : string.Empty,
                Fecha = p.Fecha,
                Importe = p.Importe,
                Estado = p.Estado,
                Activo = p.Activo
            })
            .FirstOrDefaultAsync();

        if (item is null)
            return Result<PagoResponseDto>.Falla($"No se encontró el pago con ID {id}.");

        return Result<PagoResponseDto>.Ok(item);
    }

    public async Task<Result<long>> RegistrarPagoAsync(PagoRequestDto request)
    {
        if (request.IdConsulta <= 0)
            return Result<long>.Falla("La consulta asociada es obligatoria.");

        if (request.IdMetodoPago <= 0)
            return Result<long>.Falla("El método de pago es obligatorio.");

        if (request.Importe <= 0)
            return Result<long>.Falla("El importe debe ser mayor a cero.");

        var consultaExiste = await context.Consultas.AnyAsync(c => c.Id == request.IdConsulta);
        if (!consultaExiste)
            return Result<long>.Falla($"No existe la consulta con ID {request.IdConsulta}.");

        var metodoExiste = await context.MetodosPago.AnyAsync(m => m.Id == request.IdMetodoPago);
        if (!metodoExiste)
            return Result<long>.Falla($"No existe el método de pago con ID {request.IdMetodoPago}.");

        var entidad = new Pago
        {
            IdConsulta = request.IdConsulta,
            IdMetodoPago = request.IdMetodoPago,
            Fecha = request.Fecha == default ? DateTime.Now : request.Fecha,
            Importe = request.Importe,
            Estado = string.IsNullOrWhiteSpace(request.Estado) ? "Completado" : request.Estado.Trim(),
            Activo = true
        };

        context.Pagos.Add(entidad);
        await context.SaveChangesAsync();

        return Result<long>.Ok(entidad.Id, "Pago registrado exitosamente.");
    }

    public async Task<Result> AnularPagoAsync(long id)
    {
        if (id <= 0)
            return Result.Falla("El identificador debe ser mayor a cero.");

        var entidad = await context.Pagos.FirstOrDefaultAsync(p => p.Id == id);
        if (entidad is null)
            return Result.Falla($"No se encontró el pago con ID {id}.");

        entidad.Estado = "Anulado";
        entidad.Activo = false;
        await context.SaveChangesAsync();

        return Result.Ok("Pago anulado exitosamente.");
    }
}
