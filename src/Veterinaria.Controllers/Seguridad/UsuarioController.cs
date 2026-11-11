using Veterinaria.CrossCutting.Session;
using Veterinaria.Domain.Comunes;
using Veterinaria.Domain.Dtos;
using Veterinaria.Interfaces.Services.Seguridad;

namespace Veterinaria.Controllers.Seguridad;
// Controlador de la capa de aplicación/presentación para operaciones de usuario y autorización de restauración.
public class UsuarioController
{
    private readonly IUsuarioService _usuarioService;

    public UsuarioController(IUsuarioService usuarioService)
    {
        _usuarioService = usuarioService;
    }
    // Valida que el usuario actualmente autenticado en sesión posea autorización para restaurar la base de datos.
    public async Task<Result<bool>> ValidarAutorizacionRestoreAsync()
    {
        var idSesion = SesionActual.IdUsuario;
        if (!idSesion.HasValue || idSesion.Value <= 0)
        {
            return Result<bool>.Falla("No se pudo identificar una sesión activa de usuario válida.");
        }

        return await _usuarioService.ValidarAutorizacionRestoreAsync(idSesion.Value);
    }
    // Valida los permisos de restauración para un identificador de usuario explícito.
    public async Task<Result<bool>> ValidarAutorizacionRestoreAsync(long idUsuario)
    {
        return await _usuarioService.ValidarAutorizacionRestoreAsync(idUsuario);
    }

    public async Task<Result<UsuarioRespuestaDto>> ObtenerPorUsernameAsync(string username) =>
        await _usuarioService.ObtenerPorUsernameAsync(username);

    public async Task<Result<UsuarioRespuestaDto>> ObtenerPorIdAsync(long idUsuario) =>
        await _usuarioService.ObtenerPorIdAsync(idUsuario);

    public async Task<Result<IEnumerable<UsuarioRespuestaDto>>> ObtenerTodosAsync() =>
        await _usuarioService.ObtenerTodosAsync();

    public async Task<Result<UsuarioRespuestaDto>> AutenticarAsync(string nombreUsuario, string contrasena) =>
        await _usuarioService.AutenticarAsync(nombreUsuario, contrasena);

    public async Task<Result<long>> CrearAsync(UsuarioSolicitudDto solicitud) =>
        await _usuarioService.CrearAsync(solicitud);

    public async Task<Result> ActualizarAsync(long id, UsuarioSolicitudDto solicitud) =>
        await _usuarioService.ActualizarAsync(id, solicitud);

    public async Task<Result> CambiarEstadoAsync(long id, bool activo, long? idUsuarioSesion = null) =>
        await _usuarioService.CambiarEstadoAsync(id, activo, idUsuarioSesion);
}
