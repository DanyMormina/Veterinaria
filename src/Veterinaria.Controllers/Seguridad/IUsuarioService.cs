using Veterinaria.Domain.Comunes;
using Veterinaria.Domain.Dtos;

namespace Veterinaria.Interfaces.Services.Seguridad;
// Contrato de servicio para operaciones de seguridad, autenticación y privilegios de usuario.
public interface IUsuarioService
{
    // Consulta un usuario por su nombre de usuario único, proyectando su estado y privilegios de restauración.
    Task<Result<UsuarioRespuestaDto>> ObtenerPorUsernameAsync(string username);
    // Consulta un usuario por su identificador único (ID).
    Task<Result<UsuarioRespuestaDto>> ObtenerPorIdAsync(long idUsuario);
    // Valida que el usuario exista, esté activo y posea autorización explícita para restaurar la base de datos.
    Task<Result<bool>> ValidarAutorizacionRestoreAsync(long idUsuario);
    // Obtiene todos los usuarios del sistema.
    Task<Result<IEnumerable<UsuarioRespuestaDto>>> ObtenerTodosAsync();
    // Autentica las credenciales de un usuario.
    Task<Result<UsuarioRespuestaDto>> AutenticarAsync(string nombreUsuario, string contrasena);
    // Registra un nuevo usuario en el sistema.
    Task<Result<long>> CrearAsync(UsuarioSolicitudDto solicitud);
    // Actualiza los datos de un usuario existente.
    Task<Result> ActualizarAsync(long id, UsuarioSolicitudDto solicitud);
    // Cambia el estado de activación de un usuario.
    Task<Result> CambiarEstadoAsync(long id, bool activo, long? idUsuarioSesion = null);
}
