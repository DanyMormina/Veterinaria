using Veterinaria.CrossCutting.Comunes;
using Veterinaria.Domain.Dtos;
using Veterinaria.Interfaces.Interfaces;

namespace Veterinaria.Controllers.Controladores;

/// <summary>
/// Controlador para la gestión de Usuarios y Seguridad.
/// </summary>
public class UsuarioController(IUsuarioService usuarioService)
{
    public async Task<Result<IEnumerable<UsuarioResponseDto>>> ObtenerTodosAsync()
    {
        try
        {
            return await usuarioService.ObtenerTodosAsync();
        }
        catch (Exception ex)
        {
            return Result<IEnumerable<UsuarioResponseDto>>.Falla($"Error interno al obtener usuarios: {ex.Message}");
        }
    }

    public async Task<Result<UsuarioResponseDto>> ObtenerPorIdAsync(long id)
    {
        try
        {
            return await usuarioService.ObtenerPorIdAsync(id);
        }
        catch (Exception ex)
        {
            return Result<UsuarioResponseDto>.Falla($"Error interno al obtener el usuario: {ex.Message}");
        }
    }

    public async Task<Result<UsuarioResponseDto>> ObtenerPorUsernameAsync(string username)
    {
        try
        {
            return await usuarioService.ObtenerPorUsernameAsync(username);
        }
        catch (Exception ex)
        {
            return Result<UsuarioResponseDto>.Falla($"Error interno al obtener usuario por username: {ex.Message}");
        }
    }

    public async Task<Result<UsuarioResponseDto>> AutenticarAsync(string username, string password)
    {
        try
        {
            return await usuarioService.AutenticarAsync(username, password);
        }
        catch (Exception ex)
        {
            return Result<UsuarioResponseDto>.Falla($"Error interno al autenticar usuario: {ex.Message}");
        }
    }

    public async Task<Result<long>> CrearAsync(UsuarioRequestDto request)
    {
        try
        {
            return await usuarioService.CrearAsync(request);
        }
        catch (Exception ex)
        {
            return Result<long>.Falla($"Error interno al crear el usuario: {ex.Message}");
        }
    }

    public async Task<Result> ActualizarAsync(long id, UsuarioRequestDto request)
    {
        try
        {
            return await usuarioService.ActualizarAsync(id, request);
        }
        catch (Exception ex)
        {
            return Result.Falla($"Error interno al actualizar el usuario: {ex.Message}");
        }
    }

    public async Task<Result> EliminarAsync(long id)
    {
        try
        {
            return await usuarioService.EliminarAsync(id);
        }
        catch (Exception ex)
        {
            return Result.Falla($"Error interno al eliminar el usuario: {ex.Message}");
        }
    }
}
