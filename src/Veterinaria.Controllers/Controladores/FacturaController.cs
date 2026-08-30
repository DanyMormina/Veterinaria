using Veterinaria.CrossCutting.Comunes;
using Veterinaria.Domain.Dtos;
using Veterinaria.Interfaces.Interfaces;

namespace Veterinaria.Controllers.Controladores;

/// <summary>
/// Controlador para la gestión de Facturas.
/// </summary>
public class FacturaController(IFacturaService facturaService)
{
    public async Task<Result<IEnumerable<FacturaResponseDto>>> ObtenerTodosAsync()
    {
        try
        {
            return await facturaService.ObtenerTodosAsync();
        }
        catch (Exception ex)
        {
            return Result<IEnumerable<FacturaResponseDto>>.Falla($"Error interno al obtener facturas: {ex.Message}");
        }
    }

    public async Task<Result<FacturaResponseDto>> ObtenerPorIdAsync(long id)
    {
        try
        {
            return await facturaService.ObtenerPorIdAsync(id);
        }
        catch (Exception ex)
        {
            return Result<FacturaResponseDto>.Falla($"Error interno al obtener la factura: {ex.Message}");
        }
    }

    public async Task<Result<long>> CrearAsync(FacturaRequestDto request)
    {
        try
        {
            return await facturaService.CrearAsync(request);
        }
        catch (Exception ex)
        {
            return Result<long>.Falla($"Error interno al emitir la factura: {ex.Message}");
        }
    }

    public async Task<Result> ActualizarAsync(long id, FacturaRequestDto request)
    {
        try
        {
            return await facturaService.ActualizarAsync(id, request);
        }
        catch (Exception ex)
        {
            return Result.Falla($"Error interno al actualizar la factura: {ex.Message}");
        }
    }

    public async Task<Result> EliminarAsync(long id)
    {
        try
        {
            return await facturaService.EliminarAsync(id);
        }
        catch (Exception ex)
        {
            return Result.Falla($"Error interno al anular/eliminar la factura: {ex.Message}");
        }
    }
}
