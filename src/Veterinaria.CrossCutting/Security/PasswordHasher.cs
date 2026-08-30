namespace Veterinaria.CrossCutting.Security;

/// <summary>
/// Provee métodos utilitarios para el hashing y verificación segura de contraseñas mediante BCrypt.
/// </summary>
public static class PasswordHasher
{
    /// <summary>
    /// Genera un hash seguro BCrypt a partir de la contraseña en texto plano.
    /// </summary>
    public static string Hash(string password)
    {
        if (string.IsNullOrWhiteSpace(password))
            return string.Empty;

        return BCrypt.Net.BCrypt.HashPassword(password.Trim());
    }

    /// <summary>
    /// Verifica si la contraseña coincide con el valor almacenado en base de datos.
    /// Valida primero contra texto plano directo y secundariamente contra hash BCrypt.
    /// </summary>
    public static bool Verificar(string password, string passwordHash)
    {
        if (string.IsNullOrWhiteSpace(password) || string.IsNullOrWhiteSpace(passwordHash))
            return false;

        var input = password.Trim();
        var almacenado = passwordHash.Trim();

        // 1. Comparación directa sobre texto plano (para credenciales existentes sin hash)
        if (string.Equals(input, almacenado, StringComparison.Ordinal))
            return true;

        // 2. Comparación mediante BCrypt (para contraseñas hasheadas)
        try
        {
            return BCrypt.Net.BCrypt.Verify(input, almacenado);
        }
        catch
        {
            return false;
        }
    }
}
