using Veterinaria.CrossCutting.Comunes;
using Veterinaria.Domain.Dtos;
using Veterinaria.Interfaces.Interfaces;

namespace Veterinaria.Controllers.Controladores;

/// <summary>
/// Controlador para la gestión de Roles.
/// </summary>
public class RolController(IRolService rolService)
{
    public async Task<Result<IEnumerable<RolResponseDto>>> ObtenerTodosAsync()
    {
        try
        {
            return await rolService.ObtenerTodosAsync();
        }
        catch (Exception ex)
        {
            return Result<IEnumerable<RolResponseDto>>.Falla($"Error interno al obtener roles: {ex.Message}");
        }
    }

    public async Task<Result<RolResponseDto>> ObtenerPorIdAsync(long id)
    {
        try
        {
            return await rolService.ObtenerPorIdAsync(id);
        }
        catch (Exception ex)
        {
            return Result<RolResponseDto>.Falla($"Error interno al obtener el rol: {ex.Message}");
        }
    }

    public async Task<Result<long>> CrearAsync(RolRequestDto request)
    {
        try
        {
            return await rolService.CrearAsync(request);
        }
        catch (Exception ex)
        {
            return Result<long>.Falla($"Error interno al crear el rol: {ex.Message}");
        }
    }

    public async Task<Result> ActualizarAsync(long id, RolRequestDto request)
    {
        try
        {
            return await rolService.ActualizarAsync(id, request);
        }
        catch (Exception ex)
        {
            return Result.Falla($"Error interno al actualizar el rol: {ex.Message}");
        }
    }

    public async Task<Result> EliminarAsync(long id)
    {
        try
        {
            return await rolService.EliminarAsync(id);
        }
        catch (Exception ex)
        {
            return Result.Falla($"Error interno al eliminar el rol: {ex.Message}");
        }
    }
}
