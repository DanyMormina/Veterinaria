using Veterinaria.CrossCutting.Comunes;
using Veterinaria.Domain.Dtos;
using Veterinaria.Interfaces.Interfaces;

namespace Veterinaria.Controllers.Controladores;

/// <summary>
/// Controlador para la gestión de Propietarios.
/// </summary>
public class PropietarioController(IPropietarioService propietarioService)
{
    public async Task<Result<IEnumerable<PropietarioResponseDto>>> ObtenerTodosAsync()
    {
        try
        {
            return await propietarioService.ObtenerTodosAsync();
        }
        catch (Exception ex)
        {
            return Result<IEnumerable<PropietarioResponseDto>>.Falla($"Error interno al obtener propietarios: {ex.Message}");
        }
    }

    public async Task<Result<PropietarioResponseDto>> ObtenerPorIdAsync(long id)
    {
        try
        {
            return await propietarioService.ObtenerPorIdAsync(id);
        }
        catch (Exception ex)
        {
            return Result<PropietarioResponseDto>.Falla($"Error interno al obtener el propietario: {ex.Message}");
        }
    }

    public async Task<Result<long>> CrearAsync(PropietarioRequestDto request)
    {
        try
        {
            return await propietarioService.CrearAsync(request);
        }
        catch (Exception ex)
        {
            return Result<long>.Falla($"Error interno al crear el propietario: {ex.Message}");
        }
    }

    public async Task<Result> ActualizarAsync(long id, PropietarioRequestDto request)
    {
        try
        {
            return await propietarioService.ActualizarAsync(id, request);
        }
        catch (Exception ex)
        {
            return Result.Falla($"Error interno al actualizar el propietario: {ex.Message}");
        }
    }

    public async Task<Result> EliminarAsync(long id)
    {
        try
        {
            return await propietarioService.EliminarAsync(id);
        }
        catch (Exception ex)
        {
            return Result.Falla($"Error interno al eliminar el propietario: {ex.Message}");
        }
    }
}
