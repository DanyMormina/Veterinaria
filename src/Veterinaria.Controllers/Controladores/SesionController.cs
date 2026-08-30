using Veterinaria.CrossCutting.Comunes;
using Veterinaria.Domain.Dtos;
using Veterinaria.Interfaces.Interfaces;

namespace Veterinaria.Controllers.Controladores;

/// <summary>
/// Controlador para la gestión de Sesiones de usuario.
/// </summary>
public class SesionController(ISesionService sesionService)
{
    public async Task<Result<IEnumerable<SesionResponseDto>>> ObtenerTodosAsync()
    {
        try
        {
            return await sesionService.ObtenerTodosAsync();
        }
        catch (Exception ex)
        {
            return Result<IEnumerable<SesionResponseDto>>.Falla($"Error interno al obtener sesiones: {ex.Message}");
        }
    }

    public async Task<Result<SesionResponseDto>> ObtenerPorIdAsync(long id)
    {
        try
        {
            return await sesionService.ObtenerPorIdAsync(id);
        }
        catch (Exception ex)
        {
            return Result<SesionResponseDto>.Falla($"Error interno al obtener la sesión: {ex.Message}");
        }
    }

    public async Task<Result<long>> CrearAsync(SesionRequestDto request)
    {
        try
        {
            return await sesionService.CrearAsync(request);
        }
        catch (Exception ex)
        {
            return Result<long>.Falla($"Error interno al registrar la sesión: {ex.Message}");
        }
    }

    public async Task<Result> ActualizarAsync(long id, SesionRequestDto request)
    {
        try
        {
            return await sesionService.ActualizarAsync(id, request);
        }
        catch (Exception ex)
        {
            return Result.Falla($"Error interno al actualizar la sesión: {ex.Message}");
        }
    }

    public async Task<Result> EliminarAsync(long id)
    {
        try
        {
            return await sesionService.EliminarAsync(id);
        }
        catch (Exception ex)
        {
            return Result.Falla($"Error interno al eliminar la sesión: {ex.Message}");
        }
    }
}
