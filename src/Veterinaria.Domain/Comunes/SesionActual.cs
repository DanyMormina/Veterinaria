using Veterinaria.Domain.Dtos;
using Veterinaria.Domain.Entidades;

namespace Veterinaria.CrossCutting.Session;

/// <summary>
/// Mantiene el estado en memoria de la sesión autenticada actual del sistema.
/// </summary>
public static class SesionActual
{
    public static long? IdUsuario { get; private set; }
    public static string NombreUsuario { get; private set; } = string.Empty;
    public static string NombreCompleto { get; private set; } = string.Empty;
    public static string TipoUsuario { get; private set; } = string.Empty;
    public static string Rol => TipoUsuario;
    public static bool EstaAutenticado => IdUsuario.HasValue && IdUsuario.Value > 0;

    /// <summary>
    /// Inicia sesión a partir de una entidad de dominio Usuario.
    /// </summary>
    public static void IniciarSesion(Usuario usuario)
    {
        ArgumentNullException.ThrowIfNull(usuario);

        IdUsuario = usuario.Id;
        NombreUsuario = usuario.NombreUsuario;
        NombreCompleto = $"{usuario.Nombre} {usuario.Apellido}".Trim();
        TipoUsuario = usuario.TipoUsuario != null ? usuario.TipoUsuario.Nombre : string.Empty;
    }

    /// <summary>
    /// Inicia sesión a partir de un DTO de respuesta de usuario.
    /// </summary>
    public static void IniciarSesion(UsuarioRespuestaDto usuario)
    {
        ArgumentNullException.ThrowIfNull(usuario);

        IdUsuario = usuario.Id;
        NombreUsuario = usuario.NombreUsuario;
        NombreCompleto = $"{usuario.Nombre} {usuario.Apellido}".Trim();
        TipoUsuario = usuario.NombreTipoUsuario;
    }

    /// <summary>
    /// Inicia sesión de forma manual con parámetros explícitos.
    /// </summary>
    public static void IniciarSesion(long idUsuario, string nombreUsuario, string nombreCompleto, string tipoUsuario)
    {
        IdUsuario = idUsuario;
        NombreUsuario = nombreUsuario;
        NombreCompleto = nombreCompleto;
        TipoUsuario = tipoUsuario;
    }

    /// <summary>
    /// Restablece los datos de sesión activa a su estado inicial.
    /// </summary>
    public static void CerrarSesion()
    {
        IdUsuario = null;
        NombreUsuario = string.Empty;
        NombreCompleto = string.Empty;
        TipoUsuario = string.Empty;
    }
}
