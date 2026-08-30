using Veterinaria.CrossCutting.Comunes;
using Veterinaria.Domain.Dtos;
using Veterinaria.Interfaces.Interfaces;

namespace Veterinaria.Controllers.Controladores;

/// <summary>
/// Controlador para la gestión de Vacunas.
/// </summary>
public class VacunaController(IVacunaService vacunaService)
{
    public async Task<Result<IEnumerable<VacunaResponseDto>>> ObtenerTodosAsync()
    {
        try
        {
            return await vacunaService.ObtenerTodosAsync();
        }
        catch (Exception ex)
        {
            return Result<IEnumerable<VacunaResponseDto>>.Falla($"Error interno al obtener vacunas: {ex.Message}");
        }
    }

    public async Task<Result<VacunaResponseDto>> ObtenerPorIdAsync(long id)
    {
        try
        {
            return await vacunaService.ObtenerPorIdAsync(id);
        }
        catch (Exception ex)
        {
            return Result<VacunaResponseDto>.Falla($"Error interno al obtener la vacuna: {ex.Message}");
        }
    }

    public async Task<Result<long>> CrearAsync(VacunaRequestDto request)
    {
        try
        {
            return await vacunaService.CrearAsync(request);
        }
        catch (Exception ex)
        {
            return Result<long>.Falla($"Error interno al crear la vacuna: {ex.Message}");
        }
    }

    public async Task<Result> ActualizarAsync(long id, VacunaRequestDto request)
    {
        try
        {
            return await vacunaService.ActualizarAsync(id, request);
        }
        catch (Exception ex)
        {
            return Result.Falla($"Error interno al actualizar la vacuna: {ex.Message}");
        }
    }

    public async Task<Result> EliminarAsync(long id)
    {
        try
        {
            return await vacunaService.EliminarAsync(id);
        }
        catch (Exception ex)
        {
            return Result.Falla($"Error interno al eliminar la vacuna: {ex.Message}");
        }
    }
}
