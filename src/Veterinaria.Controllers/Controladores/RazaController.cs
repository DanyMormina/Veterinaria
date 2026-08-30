using Veterinaria.CrossCutting.Comunes;
using Veterinaria.Domain.Dtos;
using Veterinaria.Interfaces.Interfaces;

namespace Veterinaria.Controllers.Controladores;

/// <summary>
/// Controlador para la gestión de Razas.
/// </summary>
public class RazaController(IRazaService razaService)
{
    public async Task<Result<IEnumerable<RazaResponseDto>>> ObtenerTodosAsync()
    {
        try
        {
            return await razaService.ObtenerTodosAsync();
        }
        catch (Exception ex)
        {
            return Result<IEnumerable<RazaResponseDto>>.Falla($"Error interno al obtener razas: {ex.Message}");
        }
    }

    public async Task<Result<RazaResponseDto>> ObtenerPorIdAsync(long id)
    {
        try
        {
            return await razaService.ObtenerPorIdAsync(id);
        }
        catch (Exception ex)
        {
            return Result<RazaResponseDto>.Falla($"Error interno al obtener la raza: {ex.Message}");
        }
    }

    public async Task<Result<long>> CrearAsync(RazaRequestDto request)
    {
        try
        {
            return await razaService.CrearAsync(request);
        }
        catch (Exception ex)
        {
            return Result<long>.Falla($"Error interno al crear la raza: {ex.Message}");
        }
    }

    public async Task<Result> ActualizarAsync(long id, RazaRequestDto request)
    {
        try
        {
            return await razaService.ActualizarAsync(id, request);
        }
        catch (Exception ex)
        {
            return Result.Falla($"Error interno al actualizar la raza: {ex.Message}");
        }
    }

    public async Task<Result> EliminarAsync(long id)
    {
        try
        {
            return await razaService.EliminarAsync(id);
        }
        catch (Exception ex)
        {
            return Result.Falla($"Error interno al eliminar la raza: {ex.Message}");
        }
    }
}
