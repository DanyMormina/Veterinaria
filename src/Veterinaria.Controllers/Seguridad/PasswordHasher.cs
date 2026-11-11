namespace Veterinaria.Controllers.Seguridad;
// Fachada para el hasheo y verificación segura de contraseñas de usuario (BCrypt).
public static class PasswordHasher
{
    // Genera un hash seguro BCrypt a partir de una contraseña en texto plano.
    public static string Hashear(string contrasena) =>
        HasheadorContrasena.Hashear(contrasena);
    // Verifica si la contraseña proporcionada coincide con el hash almacenado.
    public static bool Verificar(string contrasena, string hashContrasena) =>
        HasheadorContrasena.Verificar(contrasena, hashContrasena);
}
