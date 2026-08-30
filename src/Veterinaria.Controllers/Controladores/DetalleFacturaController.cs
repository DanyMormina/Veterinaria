using Veterinaria.CrossCutting.Comunes;
using Veterinaria.Domain.Dtos;
using Veterinaria.Interfaces.Interfaces;

namespace Veterinaria.Controllers.Controladores;

/// <summary>
/// Controlador para la gestión de Detalles de Factura.
/// </summary>
public class DetalleFacturaController(IDetalleFacturaService detalleFacturaService)
{
    public async Task<Result<IEnumerable<DetalleFacturaResponseDto>>> ObtenerTodosAsync()
    {
        try
        {
            return await detalleFacturaService.ObtenerTodosAsync();
        }
        catch (Exception ex)
        {
            return Result<IEnumerable<DetalleFacturaResponseDto>>.Falla($"Error interno al obtener detalles de factura: {ex.Message}");
        }
    }

    public async Task<Result<DetalleFacturaResponseDto>> ObtenerPorIdAsync(long id)
    {
        try
        {
            return await detalleFacturaService.ObtenerPorIdAsync(id);
        }
        catch (Exception ex)
        {
            return Result<DetalleFacturaResponseDto>.Falla($"Error interno al obtener el detalle de factura: {ex.Message}");
        }
    }

    public async Task<Result<long>> CrearAsync(DetalleFacturaRequestDto request)
    {
        try
        {
            return await detalleFacturaService.CrearAsync(request);
        }
        catch (Exception ex)
        {
            return Result<long>.Falla($"Error interno al crear el detalle de factura: {ex.Message}");
        }
    }

    public async Task<Result> ActualizarAsync(long id, DetalleFacturaRequestDto request)
    {
        try
        {
            return await detalleFacturaService.ActualizarAsync(id, request);
        }
        catch (Exception ex)
        {
            return Result.Falla($"Error interno al actualizar el detalle de factura: {ex.Message}");
        }
    }

    public async Task<Result> EliminarAsync(long id)
    {
        try
        {
            return await detalleFacturaService.EliminarAsync(id);
        }
        catch (Exception ex)
        {
            return Result.Falla($"Error interno al eliminar el detalle de factura: {ex.Message}");
        }
    }
}
