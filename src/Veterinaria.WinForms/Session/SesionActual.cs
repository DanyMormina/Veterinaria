using Veterinaria.Domain.Dtos;

namespace Veterinaria.WinForms.Session;

public static class SesionActual
{
    public static long? IdUsuario { get; private set; }
    public static string Username { get; private set; } = string.Empty;
    public static string NombreCompleto { get; private set; } = string.Empty;
    public static string TipoUsuario { get; private set; } = string.Empty;
    public static string Rol => TipoUsuario;
    public static bool EstaAutenticado => IdUsuario.HasValue && IdUsuario.Value > 0;

    public static void IniciarSesion(UsuarioResponseDto usuario)
    {
        ArgumentNullException.ThrowIfNull(usuario);

        IdUsuario = usuario.Id;
        Username = usuario.Username;
        NombreCompleto = $"{usuario.Nombre} {usuario.Apellido}".Trim();
        TipoUsuario = usuario.NombreTipoUsuario;
    }

    public static void IniciarSesion(long idUsuario, string username, string nombreCompleto, string tipoUsuario)
    {
        IdUsuario = idUsuario;
        Username = username;
        NombreCompleto = nombreCompleto;
        TipoUsuario = tipoUsuario;
    }

    public static void CerrarSesion()
    {
        IdUsuario = null;
        Username = string.Empty;
        NombreCompleto = string.Empty;
        TipoUsuario = string.Empty;
    }
}
