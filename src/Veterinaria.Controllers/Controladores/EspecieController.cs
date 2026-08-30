using Veterinaria.CrossCutting.Comunes;
using Veterinaria.Domain.Dtos;
using Veterinaria.Interfaces.Interfaces;

namespace Veterinaria.Controllers.Controladores;

/// <summary>
/// Controlador para la gestión de Especies.
/// </summary>
public class EspecieController(IEspecieService especieService)
{
    public async Task<Result<IEnumerable<EspecieResponseDto>>> ObtenerTodosAsync()
    {
        try
        {
            return await especieService.ObtenerTodosAsync();
        }
        catch (Exception ex)
        {
            return Result<IEnumerable<EspecieResponseDto>>.Falla($"Error interno al obtener especies: {ex.Message}");
        }
    }

    public async Task<Result<EspecieResponseDto>> ObtenerPorIdAsync(long id)
    {
        try
        {
            return await especieService.ObtenerPorIdAsync(id);
        }
        catch (Exception ex)
        {
            return Result<EspecieResponseDto>.Falla($"Error interno al obtener la especie: {ex.Message}");
        }
    }

    public async Task<Result<long>> CrearAsync(EspecieRequestDto request)
    {
        try
        {
            return await especieService.CrearAsync(request);
        }
        catch (Exception ex)
        {
            return Result<long>.Falla($"Error interno al crear la especie: {ex.Message}");
        }
    }

    public async Task<Result> ActualizarAsync(long id, EspecieRequestDto request)
    {
        try
        {
            return await especieService.ActualizarAsync(id, request);
        }
        catch (Exception ex)
        {
            return Result.Falla($"Error interno al actualizar la especie: {ex.Message}");
        }
    }

    public async Task<Result> EliminarAsync(long id)
    {
        try
        {
            return await especieService.EliminarAsync(id);
        }
        catch (Exception ex)
        {
            return Result.Falla($"Error interno al eliminar la especie: {ex.Message}");
        }
    }
}
