using Veterinaria.Domain.Comunes;
using Veterinaria.Domain.Dtos;

namespace Veterinaria.Interfaces.Services.Seguridad;

/// <summary>
/// Contrato de servicio para operaciones de seguridad, autenticación y privilegios de usuario.
/// </summary>
public interface IUsuarioService
{
    /// <summary>
    /// Consulta un usuario por su nombre de usuario único, proyectando su estado y privilegios de restauración.
    /// </summary>
    Task<Result<UsuarioRespuestaDto>> ObtenerPorUsernameAsync(string username);

    /// <summary>
    /// Consulta un usuario por su identificador único (ID).
    /// </summary>
    Task<Result<UsuarioRespuestaDto>> ObtenerPorIdAsync(long idUsuario);

    /// <summary>
    /// Valida que el usuario exista, esté activo y posea autorización explícita para restaurar la base de datos.
    /// </summary>
    /// <param name="idUsuario">Identificador único del usuario.</param>
    /// <returns>Result indicando si posee permiso exclusivo de restauración.</returns>
    Task<Result<bool>> ValidarAutorizacionRestoreAsync(long idUsuario);

    /// <summary>
    /// Obtiene todos los usuarios del sistema.
    /// </summary>
    Task<Result<IEnumerable<UsuarioRespuestaDto>>> ObtenerTodosAsync();

    /// <summary>
    /// Autentica las credenciales de un usuario.
    /// </summary>
    Task<Result<UsuarioRespuestaDto>> AutenticarAsync(string nombreUsuario, string contrasena);

    /// <summary>
    /// Registra un nuevo usuario en el sistema.
    /// </summary>
    Task<Result<long>> CrearAsync(UsuarioSolicitudDto solicitud);

    /// <summary>
    /// Actualiza los datos de un usuario existente.
    /// </summary>
    Task<Result> ActualizarAsync(long id, UsuarioSolicitudDto solicitud);

    /// <summary>
    /// Cambia el estado de activación de un usuario.
    /// </summary>
    Task<Result> CambiarEstadoAsync(long id, bool activo, long? idUsuarioSesion = null);
}
