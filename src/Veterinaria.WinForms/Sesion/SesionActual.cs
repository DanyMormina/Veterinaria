using Veterinaria.Domain.Dtos;
using Veterinaria.Domain.Entidades;
using CoreSesion = Veterinaria.CrossCutting.Session.SesionActual;

namespace Veterinaria.WinForms.Sesion;

/// <summary>
/// Fachada de sesión para la capa de presentación Windows Forms sincronizada con Veterinaria.CrossCutting.Session.
/// </summary>
public static class SesionActual
{
    public static long? IdUsuario => CoreSesion.IdUsuario;
    public static string NombreUsuario => CoreSesion.NombreUsuario;
    public static string NombreCompleto => CoreSesion.NombreCompleto;
    public static string TipoUsuario => CoreSesion.TipoUsuario;
    public static string Rol => CoreSesion.Rol;
    public static bool EstaAutenticado => CoreSesion.EstaAutenticado;

    public static void IniciarSesion(Usuario usuario)
    {
        CoreSesion.IniciarSesion(usuario);
    }

    public static void IniciarSesion(UsuarioRespuestaDto usuario)
    {
        CoreSesion.IniciarSesion(usuario);
    }

    public static void IniciarSesion(long idUsuario, string nombreUsuario, string nombreCompleto, string tipoUsuario)
    {
        CoreSesion.IniciarSesion(idUsuario, nombreUsuario, nombreCompleto, tipoUsuario);
    }

    public static void CerrarSesion()
    {
        CoreSesion.CerrarSesion();
    }
}
