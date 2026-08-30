using Veterinaria.CrossCutting.Comunes;
using Veterinaria.Domain.Dtos;
using Veterinaria.Interfaces.Interfaces;

namespace Veterinaria.Controllers.Controladores;

/// <summary>
/// Controlador para la gestión y consulta de Auditoría del sistema.
/// </summary>
public class AuditoriaController(IAuditoriaService auditoriaService)
{
    public async Task<Result<IEnumerable<AuditoriaResponseDto>>> ObtenerTodosAsync()
    {
        try
        {
            return await auditoriaService.ObtenerTodosAsync();
        }
        catch (Exception ex)
        {
            return Result<IEnumerable<AuditoriaResponseDto>>.Falla($"Error interno al obtener registros de auditoría: {ex.Message}");
        }
    }

    public async Task<Result<AuditoriaResponseDto>> ObtenerPorIdAsync(long id)
    {
        try
        {
            return await auditoriaService.ObtenerPorIdAsync(id);
        }
        catch (Exception ex)
        {
            return Result<AuditoriaResponseDto>.Falla($"Error interno al obtener el registro de auditoría: {ex.Message}");
        }
    }

    public async Task<Result<long>> CrearAsync(AuditoriaRequestDto request)
    {
        try
        {
            return await auditoriaService.CrearAsync(request);
        }
        catch (Exception ex)
        {
            return Result<long>.Falla($"Error interno al crear el registro de auditoría: {ex.Message}");
        }
    }

    public async Task<Result> ActualizarAsync(long id, AuditoriaRequestDto request)
    {
        try
        {
            return await auditoriaService.ActualizarAsync(id, request);
        }
        catch (Exception ex)
        {
            return Result.Falla($"Error interno al actualizar el registro de auditoría: {ex.Message}");
        }
    }

    public async Task<Result> EliminarAsync(long id)
    {
        try
        {
            return await auditoriaService.EliminarAsync(id);
        }
        catch (Exception ex)
        {
            return Result.Falla($"Error interno al eliminar el registro de auditoría: {ex.Message}");
        }
    }
}
