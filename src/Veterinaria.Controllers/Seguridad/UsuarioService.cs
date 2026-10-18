using Microsoft.EntityFrameworkCore;
using Veterinaria.Controllers.Controladores;
using Veterinaria.Domain.Comunes;
using Veterinaria.Domain.Dtos;
using Veterinaria.Domain.Entidades;
using Veterinaria.Infrastructure;
using Veterinaria.Interfaces.Services.Seguridad;

namespace Veterinaria.Services.Seguridad;

/// <summary>
/// Implementación de la capa de servicios para la administración y validación de privilegios de usuarios.
/// </summary>
public class UsuarioService : IUsuarioService
{
    private readonly ContextoVeterinaria _contexto;
    private readonly UsuarioControlador _controlador;

    public UsuarioService(ContextoVeterinaria contexto, UsuarioControlador controlador)
    {
        _contexto = contexto;
        _controlador = controlador;
    }

    /// <summary>
    /// Consulta un usuario por su nombre de usuario único con proyección no rastreada.
    /// </summary>
    public async Task<Result<UsuarioRespuestaDto>> ObtenerPorUsernameAsync(string username)
    {
        var res = await _controlador.ObtenerPorNombreUsuarioAsync(username);
        return res.EsExitoso && res.Valor != null
            ? Result<UsuarioRespuestaDto>.Ok(res.Valor, res.Mensaje)
            : Result<UsuarioRespuestaDto>.Falla(res.Mensaje);
    }

    /// <summary>
    /// Consulta un usuario por su ID primario.
    /// </summary>
    public async Task<Result<UsuarioRespuestaDto>> ObtenerPorIdAsync(long idUsuario)
    {
        var res = await _controlador.ObtenerPorIdAsync(idUsuario);
        return res.EsExitoso && res.Valor != null
            ? Result<UsuarioRespuestaDto>.Ok(res.Valor, res.Mensaje)
            : Result<UsuarioRespuestaDto>.Falla(res.Mensaje);
    }

    /// <summary>
    /// Valida que el usuario exista, esté activo y posea privilegios exclusivos para restaurar la base de datos.
    /// Realiza una consulta directa y en tiempo real a la base de datos proyectando únicamente las columnas requeridas.
    /// </summary>
    public async Task<Result<bool>> ValidarAutorizacionRestoreAsync(long idUsuario)
    {
        if (idUsuario <= 0)
            return Result<bool>.Falla("Identificador de usuario inválido.");

        var usuario = await _contexto.Usuarios
            .AsNoTracking()
            .Where(u => u.Id == idUsuario && u.Activo)
            .Select(u => new { u.Id, u.PuedeRestaurarBD })
            .FirstOrDefaultAsync();

        if (usuario == null)
            return Result<bool>.Falla("Usuario no encontrado o inactivo.");

        if (!usuario.PuedeRestaurarBD)
            return Result<bool>.Falla("No cuenta con privilegios administrativos para realizar la restauración de la base de datos.");

        return Result<bool>.Ok(true);
    }

    /// <summary>
    /// Consulta el listado completo de usuarios en el sistema.
    /// </summary>
    public async Task<Result<IEnumerable<UsuarioRespuestaDto>>> ObtenerTodosAsync()
    {
        var res = await _controlador.ObtenerTodosAsync();
        return res.EsExitoso && res.Valor != null
            ? Result<IEnumerable<UsuarioRespuestaDto>>.Ok(res.Valor, res.Mensaje)
            : Result<IEnumerable<UsuarioRespuestaDto>>.Falla(res.Mensaje);
    }

    /// <summary>
    /// Autentica las credenciales de acceso del usuario.
    /// </summary>
    public async Task<Result<UsuarioRespuestaDto>> AutenticarAsync(string nombreUsuario, string contrasena)
    {
        var res = await _controlador.AutenticarAsync(nombreUsuario, contrasena);
        return res.EsExitoso && res.Valor != null
            ? Result<UsuarioRespuestaDto>.Ok(res.Valor, res.Mensaje)
            : Result<UsuarioRespuestaDto>.Falla(res.Mensaje);
    }

    /// <summary>
    /// Da de alta un nuevo usuario registrando su estado y permisos.
    /// </summary>
    public async Task<Result<long>> CrearAsync(UsuarioSolicitudDto solicitud)
    {
        var res = await _controlador.CrearAsync(solicitud);
        return res.EsExitoso
            ? Result<long>.Ok(res.Valor, res.Mensaje)
            : Result<long>.Falla(res.Mensaje);
    }

    /// <summary>
    /// Actualiza los datos y privilegios del usuario especificado.
    /// </summary>
    public async Task<Result> ActualizarAsync(long id, UsuarioSolicitudDto solicitud)
    {
        var res = await _controlador.ActualizarAsync(id, solicitud);
        return res.EsExitoso
            ? Result.Ok(res.Mensaje)
            : Result.Falla(res.Mensaje);
    }

    /// <summary>
    /// Conmuta el estado de actividad lógica del usuario.
    /// </summary>
    public async Task<Result> CambiarEstadoAsync(long id, bool activo, long? idUsuarioSesion = null)
    {
        var res = await _controlador.CambiarEstadoAsync(id, activo, idUsuarioSesion);
        return res.EsExitoso
            ? Result.Ok(res.Mensaje)
            : Result.Falla(res.Mensaje);
    }
}
