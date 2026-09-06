using Microsoft.EntityFrameworkCore;
using Veterinaria.Domain.Comunes;
using Veterinaria.Domain.Dtos;
using Veterinaria.Domain.Entidades;
using Veterinaria.Infrastructure;

namespace Veterinaria.Controllers.Controladores;

public class PagoController(VeterinariaDbContext context)
{
    public async Task<Result<IEnumerable<PagoResponseDto>>> ObtenerPorConsultaAsync(long idConsulta)
    {
        try
        {
            var items = await ConsultaBase()
                .Where(p => p.IdConsulta == idConsulta)
                .ToListAsync();

            return Result<IEnumerable<PagoResponseDto>>.Ok(items);
        }
        catch (Exception ex)
        {
            return Result<IEnumerable<PagoResponseDto>>.Falla($"Error al obtener pagos de la consulta: {ex.Message}");
        }
    }

    public async Task<Result<IEnumerable<PagoResponseDto>>> ObtenerTodosAsync()
    {
        try
        {
            var items = await ConsultaBase().ToListAsync();
            return Result<IEnumerable<PagoResponseDto>>.Ok(items);
        }
        catch (Exception ex)
        {
            return Result<IEnumerable<PagoResponseDto>>.Falla($"Error al obtener pagos: {ex.Message}");
        }
    }

    public async Task<Result<PagoResponseDto>> ObtenerPorIdAsync(long id)
    {
        try
        {
            if (id <= 0)
                return Result<PagoResponseDto>.Falla("El identificador debe ser mayor a cero.");

            var item = await ConsultaBase().FirstOrDefaultAsync(p => p.Id == id);
            if (item is null)
                return Result<PagoResponseDto>.Falla($"No se encontró el pago con ID {id}.");

            return Result<PagoResponseDto>.Ok(item);
        }
        catch (Exception ex)
        {
            return Result<PagoResponseDto>.Falla($"Error al obtener el pago: {ex.Message}");
        }
    }

    public async Task<Result<long>> RegistrarPagoAsync(PagoRequestDto request)
    {
        try
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
        catch (Exception ex)
        {
            return Result<long>.Falla($"Error al registrar el pago: {ex.Message}");
        }
    }

    public async Task<Result> AnularPagoAsync(long id)
    {
        try
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
        catch (Exception ex)
        {
            return Result.Falla($"Error al anular el pago: {ex.Message}");
        }
    }

    private IQueryable<PagoResponseDto> ConsultaBase() =>
        context.Pagos
            .AsNoTracking()
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
            });
}
