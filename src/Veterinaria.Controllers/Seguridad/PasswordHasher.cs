namespace Veterinaria.Controllers.Seguridad;

/// <summary>
/// Fachada para el hasheo y verificación segura de contraseñas de usuario (BCrypt).
/// </summary>
public static class PasswordHasher
{
    /// <summary>
    /// Genera un hash seguro BCrypt a partir de una contraseña en texto plano.
    /// </summary>
    public static string Hashear(string contrasena) =>
        HasheadorContrasena.Hashear(contrasena);

    /// <summary>
    /// Verifica si la contraseña proporcionada coincide con el hash almacenado.
    /// </summary>
    public static bool Verificar(string contrasena, string hashContrasena) =>
        HasheadorContrasena.Verificar(contrasena, hashContrasena);
}
