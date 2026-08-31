using Veterinaria.CrossCutting.Comunes;
using Veterinaria.Domain.Dtos;
using Veterinaria.Interfaces.Interfaces;

namespace Veterinaria.Controllers.Controladores;

public class TipoUsuarioController(ITipoUsuarioService tipoUsuarioService)
{
    public async Task<Result<IEnumerable<TipoUsuarioResponseDto>>> ObtenerTodosAsync()
    {
        try
        {
            return await tipoUsuarioService.ObtenerTodosAsync();
        }
        catch (Exception ex)
        {
            return Result<IEnumerable<TipoUsuarioResponseDto>>.Falla($"Error al obtener tipos de usuario: {ex.Message}");
        }
    }

    public async Task<Result<TipoUsuarioResponseDto>> ObtenerPorIdAsync(long id)
    {
        try
        {
            return await tipoUsuarioService.ObtenerPorIdAsync(id);
        }
        catch (Exception ex)
        {
            return Result<TipoUsuarioResponseDto>.Falla($"Error al obtener el tipo de usuario: {ex.Message}");
        }
    }

    public async Task<Result<long>> CrearAsync(TipoUsuarioRequestDto request)
    {
        try
        {
            return await tipoUsuarioService.CrearAsync(request);
        }
        catch (Exception ex)
        {
            return Result<long>.Falla($"Error al registrar el tipo de usuario: {ex.Message}");
        }
    }

    public async Task<Result> ActualizarAsync(long id, TipoUsuarioRequestDto request)
    {
        try
        {
            return await tipoUsuarioService.ActualizarAsync(id, request);
        }
        catch (Exception ex)
        {
            return Result.Falla($"Error al actualizar el tipo de usuario: {ex.Message}");
        }
    }

    public async Task<Result> EliminarAsync(long id)
    {
        try
        {
            return await tipoUsuarioService.EliminarAsync(id);
        }
        catch (Exception ex)
        {
            return Result.Falla($"Error al eliminar el tipo de usuario: {ex.Message}");
        }
    }
}
