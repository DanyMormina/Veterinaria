using Veterinaria.CrossCutting.Comunes;
using Veterinaria.Domain.Dtos;
using Veterinaria.Interfaces.Interfaces;

namespace Veterinaria.Controllers.Controladores;

/// <summary>
/// Controlador para la gestión de Tratamientos.
/// </summary>
public class TratamientoController(ITratamientoService tratamientoService)
{
    public async Task<Result<IEnumerable<TratamientoResponseDto>>> ObtenerTodosAsync()
    {
        try
        {
            return await tratamientoService.ObtenerTodosAsync();
        }
        catch (Exception ex)
        {
            return Result<IEnumerable<TratamientoResponseDto>>.Falla($"Error interno al obtener tratamientos: {ex.Message}");
        }
    }

    public async Task<Result<TratamientoResponseDto>> ObtenerPorIdAsync(long id)
    {
        try
        {
            return await tratamientoService.ObtenerPorIdAsync(id);
        }
        catch (Exception ex)
        {
            return Result<TratamientoResponseDto>.Falla($"Error interno al obtener el tratamiento: {ex.Message}");
        }
    }

    public async Task<Result<long>> CrearAsync(TratamientoRequestDto request)
    {
        try
        {
            return await tratamientoService.CrearAsync(request);
        }
        catch (Exception ex)
        {
            return Result<long>.Falla($"Error interno al registrar el tratamiento: {ex.Message}");
        }
    }

    public async Task<Result> ActualizarAsync(long id, TratamientoRequestDto request)
    {
        try
        {
            return await tratamientoService.ActualizarAsync(id, request);
        }
        catch (Exception ex)
        {
            return Result.Falla($"Error interno al actualizar el tratamiento: {ex.Message}");
        }
    }

    public async Task<Result> EliminarAsync(long id)
    {
        try
        {
            return await tratamientoService.EliminarAsync(id);
        }
        catch (Exception ex)
        {
            return Result.Falla($"Error interno al eliminar el tratamiento: {ex.Message}");
        }
    }
}
