using Veterinaria.CrossCutting.Comunes;
using Veterinaria.Domain.Dtos;
using Veterinaria.Interfaces.Interfaces;

namespace Veterinaria.Controllers.Controladores;

/// <summary>
/// Controlador para la gestión de Métodos de Pago.
/// </summary>
public class MetodoPagoController(IMetodoPagoService metodoPagoService)
{
    public async Task<Result<IEnumerable<MetodoPagoResponseDto>>> ObtenerTodosAsync()
    {
        try
        {
            return await metodoPagoService.ObtenerTodosAsync();
        }
        catch (Exception ex)
        {
            return Result<IEnumerable<MetodoPagoResponseDto>>.Falla($"Error interno al obtener métodos de pago: {ex.Message}");
        }
    }

    public async Task<Result<MetodoPagoResponseDto>> ObtenerPorIdAsync(long id)
    {
        try
        {
            return await metodoPagoService.ObtenerPorIdAsync(id);
        }
        catch (Exception ex)
        {
            return Result<MetodoPagoResponseDto>.Falla($"Error interno al obtener el método de pago: {ex.Message}");
        }
    }

    public async Task<Result<long>> CrearAsync(MetodoPagoRequestDto request)
    {
        try
        {
            return await metodoPagoService.CrearAsync(request);
        }
        catch (Exception ex)
        {
            return Result<long>.Falla($"Error interno al crear el método de pago: {ex.Message}");
        }
    }

    public async Task<Result> ActualizarAsync(long id, MetodoPagoRequestDto request)
    {
        try
        {
            return await metodoPagoService.ActualizarAsync(id, request);
        }
        catch (Exception ex)
        {
            return Result.Falla($"Error interno al actualizar el método de pago: {ex.Message}");
        }
    }

    public async Task<Result> EliminarAsync(long id)
    {
        try
        {
            return await metodoPagoService.EliminarAsync(id);
        }
        catch (Exception ex)
        {
            return Result.Falla($"Error interno al eliminar el método de pago: {ex.Message}");
        }
    }
}
