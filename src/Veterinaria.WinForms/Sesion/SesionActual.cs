using Veterinaria.Domain.Dtos;

namespace Veterinaria.WinForms.Sesion;

public static class SesionActual
{
    public static long? IdUsuario { get; private set; }
    public static string NombreUsuario { get; private set; } = string.Empty;
    public static string NombreCompleto { get; private set; } = string.Empty;
    public static string TipoUsuario { get; private set; } = string.Empty;
    public static string Rol => TipoUsuario;
    public static bool EstaAutenticado => IdUsuario.HasValue && IdUsuario.Value > 0;

    public static void IniciarSesion(UsuarioRespuestaDto usuario)
    {
        ArgumentNullException.ThrowIfNull(usuario);

        IdUsuario = usuario.Id;
        NombreUsuario = usuario.NombreUsuario;
        NombreCompleto = $"{usuario.Nombre} {usuario.Apellido}".Trim();
        TipoUsuario = usuario.NombreTipoUsuario;
    }

    public static void IniciarSesion(long idUsuario, string nombreUsuario, string nombreCompleto, string tipoUsuario)
    {
        IdUsuario = idUsuario;
        NombreUsuario = nombreUsuario;
        NombreCompleto = nombreCompleto;
        TipoUsuario = tipoUsuario;
    }

    public static void CerrarSesion()
    {
        IdUsuario = null;
        NombreUsuario = string.Empty;
        NombreCompleto = string.Empty;
        TipoUsuario = string.Empty;
    }
}
