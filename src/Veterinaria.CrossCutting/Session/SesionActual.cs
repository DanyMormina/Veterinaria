using Veterinaria.Domain.Dtos;

namespace Veterinaria.CrossCutting.Session;

/// <summary>
/// Mantiene el estado en memoria de la sesión del usuario actualmente autenticado en el sistema.
/// </summary>
public static class SesionActual
{
    /// <summary>
    /// Identificador del usuario logueado.
    /// </summary>
    public static long? IdUsuario { get; private set; }

    /// <summary>
    /// Nombre de usuario (username) de la sesión.
    /// </summary>
    public static string Username { get; private set; } = string.Empty;

    /// <summary>
    /// Nombre completo del usuario autenticado.
    /// </summary>
    public static string NombreCompleto { get; private set; } = string.Empty;

    /// <summary>
    /// Nombre del Tipo de Usuario / Rol asignado (e.g., "Administrador", "Veterinario", "Secretario").
    /// </summary>
    public static string TipoUsuario { get; private set; } = string.Empty;

    /// <summary>
    /// Alias para compatibilidad con código existente.
    /// </summary>
    public static string Rol => TipoUsuario;

    /// <summary>
    /// Indica si existe una sesión activa actualmente.
    /// </summary>
    public static bool EstaAutenticado => IdUsuario.HasValue && IdUsuario.Value > 0;

    /// <summary>
    /// Inicia la sesión en memoria a partir de los datos del DTO de respuesta.
    /// </summary>
    public static void IniciarSesion(UsuarioResponseDto usuario)
    {
        ArgumentNullException.ThrowIfNull(usuario);

        IdUsuario = usuario.Id;
        Username = usuario.Username;
        NombreCompleto = $"{usuario.Nombre} {usuario.Apellido}".Trim();
        TipoUsuario = usuario.NombreTipoUsuario;
    }

    /// <summary>
    /// Inicia la sesión en memoria mediante parámetros explícitos.
    /// </summary>
    public static void IniciarSesion(long idUsuario, string username, string nombreCompleto, string tipoUsuario)
    {
        IdUsuario = idUsuario;
        Username = username;
        NombreCompleto = nombreCompleto;
        TipoUsuario = tipoUsuario;
    }

    /// <summary>
    /// Limpia los datos de sesión en memoria al cerrar sesión.
    /// </summary>
    public static void CerrarSesion()
    {
        IdUsuario = null;
        Username = string.Empty;
        NombreCompleto = string.Empty;
        TipoUsuario = string.Empty;
    }
}
