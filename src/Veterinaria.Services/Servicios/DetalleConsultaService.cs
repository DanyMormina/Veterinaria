using Microsoft.EntityFrameworkCore;
using Veterinaria.CrossCutting.Comunes;
using Veterinaria.Domain.Dtos;
using Veterinaria.Domain.Entidades;
using Veterinaria.Infrastructure;
using Veterinaria.Interfaces.Interfaces;

namespace Veterinaria.Services.Servicios;

public class DetalleConsultaService(VeterinariaDbContext context) : IDetalleConsultaService
{
    public async Task<Result<IEnumerable<DetalleConsultaResponseDto>>> ObtenerPorConsultaAsync(long idConsulta)
    {
        var items = await context.DetalleConsultas
            .AsNoTracking()
            .Include(d => d.Tratamiento)
            .Where(d => d.IdConsulta == idConsulta)
            .Select(d => new DetalleConsultaResponseDto
            {
                Id = d.Id,
                IdConsulta = d.IdConsulta,
                IdTratamiento = d.IdTratamiento,
                TipoTratamiento = d.Tratamiento != null ? d.Tratamiento.TipoTratamiento : string.Empty,
                DescripcionTratamiento = d.Tratamiento != null ? d.Tratamiento.Descripcion : string.Empty,
                Cantidad = d.Cantidad,
                PrecioUnitario = d.PrecioUnitario,
                Subtotal = d.Subtotal,
                Indicaciones = d.Indicaciones,
                Activo = d.Activo
            })
            .ToListAsync();

        return Result<IEnumerable<DetalleConsultaResponseDto>>.Ok(items);
    }

    public async Task<Result<DetalleConsultaResponseDto>> ObtenerPorIdAsync(long id)
    {
        if (id <= 0)
            return Result<DetalleConsultaResponseDto>.Falla("El identificador debe ser mayor a cero.");

        var item = await context.DetalleConsultas
            .AsNoTracking()
            .Include(d => d.Tratamiento)
            .Where(d => d.Id == id)
            .Select(d => new DetalleConsultaResponseDto
            {
                Id = d.Id,
                IdConsulta = d.IdConsulta,
                IdTratamiento = d.IdTratamiento,
                TipoTratamiento = d.Tratamiento != null ? d.Tratamiento.TipoTratamiento : string.Empty,
                DescripcionTratamiento = d.Tratamiento != null ? d.Tratamiento.Descripcion : string.Empty,
                Cantidad = d.Cantidad,
                PrecioUnitario = d.PrecioUnitario,
                Subtotal = d.Subtotal,
                Indicaciones = d.Indicaciones,
                Activo = d.Activo
            })
            .FirstOrDefaultAsync();

        if (item is null)
            return Result<DetalleConsultaResponseDto>.Falla($"No se encontró el detalle con ID {id}.");

        return Result<DetalleConsultaResponseDto>.Ok(item);
    }

    public async Task<Result<long>> CrearAsync(DetalleConsultaRequestDto request)
    {
        if (request.IdConsulta <= 0)
            return Result<long>.Falla("La consulta es obligatoria.");

        if (request.IdTratamiento <= 0)
            return Result<long>.Falla("El tratamiento es obligatorio.");

        if (request.Cantidad <= 0)
            return Result<long>.Falla("La cantidad debe ser mayor a cero.");

        var consultaExiste = await context.Consultas.AnyAsync(c => c.Id == request.IdConsulta);
        if (!consultaExiste)
            return Result<long>.Falla($"No existe la consulta con ID {request.IdConsulta}.");

        var tratamiento = await context.Tratamientos.FirstOrDefaultAsync(t => t.Id == request.IdTratamiento);
        if (tratamiento is null)
            return Result<long>.Falla($"No existe el tratamiento con ID {request.IdTratamiento}.");

        var precioUnitario = request.PrecioUnitario > 0 ? request.PrecioUnitario : tratamiento.Precio;
        var subtotal = request.Subtotal > 0 ? request.Subtotal : (precioUnitario * request.Cantidad);

        var entidad = new DetalleConsulta
        {
            IdConsulta = request.IdConsulta,
            IdTratamiento = request.IdTratamiento,
            Cantidad = request.Cantidad,
            PrecioUnitario = precioUnitario,
            Subtotal = subtotal,
            Indicaciones = string.IsNullOrWhiteSpace(request.Indicaciones) ? null : request.Indicaciones.Trim(),
            Activo = true
        };

        context.DetalleConsultas.Add(entidad);
        await context.SaveChangesAsync();

        return Result<long>.Ok(entidad.Id, "Detalle de tratamiento agregado a la consulta.");
    }

    public async Task<Result> ActualizarAsync(long id, DetalleConsultaRequestDto request)
    {
        if (id <= 0)
            return Result.Falla("El identificador debe ser mayor a cero.");

        var entidad = await context.DetalleConsultas.FirstOrDefaultAsync(d => d.Id == id);
        if (entidad is null)
            return Result.Falla($"No se encontró el detalle con ID {id}.");

        var precioUnitario = request.PrecioUnitario > 0 ? request.PrecioUnitario : entidad.PrecioUnitario;
        var subtotal = request.Subtotal > 0 ? request.Subtotal : (precioUnitario * request.Cantidad);

        entidad.IdTratamiento = request.IdTratamiento;
        entidad.Cantidad = request.Cantidad;
        entidad.PrecioUnitario = precioUnitario;
        entidad.Subtotal = subtotal;
        entidad.Indicaciones = string.IsNullOrWhiteSpace(request.Indicaciones) ? null : request.Indicaciones.Trim();

        await context.SaveChangesAsync();

        return Result.Ok("Detalle de consulta actualizado exitosamente.");
    }

    public async Task<Result> EliminarAsync(long id)
    {
        if (id <= 0)
            return Result.Falla("El identificador debe ser mayor a cero.");

        var entidad = await context.DetalleConsultas.FirstOrDefaultAsync(d => d.Id == id);
        if (entidad is null)
            return Result.Falla($"No se encontró el detalle con ID {id}.");

        context.DetalleConsultas.Remove(entidad);
        await context.SaveChangesAsync();

        return Result.Ok("Detalle eliminado exitosamente.");
    }
}
