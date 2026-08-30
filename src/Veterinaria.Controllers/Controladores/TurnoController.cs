using Veterinaria.CrossCutting.Comunes;
using Veterinaria.Domain.Dtos;
using Veterinaria.Interfaces.Interfaces;

namespace Veterinaria.Controllers.Controladores;

/// <summary>
/// Controlador para la gestión de Turnos.
/// </summary>
public class TurnoController(ITurnoService turnoService)
{
    public async Task<Result<IEnumerable<TurnoResponseDto>>> ObtenerTodosAsync()
    {
        try
        {
            return await turnoService.ObtenerTodosAsync();
        }
        catch (Exception ex)
        {
            return Result<IEnumerable<TurnoResponseDto>>.Falla($"Error interno al obtener turnos: {ex.Message}");
        }
    }

    public async Task<Result<TurnoResponseDto>> ObtenerPorIdAsync(long id)
    {
        try
        {
            return await turnoService.ObtenerPorIdAsync(id);
        }
        catch (Exception ex)
        {
            return Result<TurnoResponseDto>.Falla($"Error interno al obtener el turno: {ex.Message}");
        }
    }

    public async Task<Result<long>> CrearAsync(TurnoRequestDto request)
    {
        try
        {
            return await turnoService.CrearAsync(request);
        }
        catch (Exception ex)
        {
            return Result<long>.Falla($"Error interno al crear el turno: {ex.Message}");
        }
    }

    public async Task<Result> ActualizarAsync(long id, TurnoRequestDto request)
    {
        try
        {
            return await turnoService.ActualizarAsync(id, request);
        }
        catch (Exception ex)
        {
            return Result.Falla($"Error interno al actualizar el turno: {ex.Message}");
        }
    }

    public async Task<Result> EliminarAsync(long id)
    {
        try
        {
            return await turnoService.EliminarAsync(id);
        }
        catch (Exception ex)
        {
            return Result.Falla($"Error interno al eliminar el turno: {ex.Message}");
        }
    }
}
