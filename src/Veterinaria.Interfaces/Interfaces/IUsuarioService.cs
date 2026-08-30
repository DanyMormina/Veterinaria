using Veterinaria.CrossCutting.Comunes;
using Veterinaria.Domain.Dtos;

namespace Veterinaria.Interfaces.Interfaces;

/// <summary>
/// Contrato para el servicio de gestión de Usuarios.
/// </summary>
public interface IUsuarioService
{
    Task<Result<IEnumerable<UsuarioResponseDto>>> ObtenerTodosAsync();
    Task<Result<UsuarioResponseDto>> ObtenerPorIdAsync(long id);
    Task<Result<UsuarioResponseDto>> ObtenerPorUsernameAsync(string username);
    Task<Result<UsuarioResponseDto>> AutenticarAsync(string username, string password);
    Task<Result<long>> CrearAsync(UsuarioRequestDto request);
    Task<Result> ActualizarAsync(long id, UsuarioRequestDto request);
    Task<Result> EliminarAsync(long id);
}
