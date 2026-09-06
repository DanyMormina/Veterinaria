namespace Veterinaria.Controllers.Seguridad;

public static class PasswordHasher
{
    public static string Hash(string password)
    {
        if (string.IsNullOrWhiteSpace(password))
            return string.Empty;

        return BCrypt.Net.BCrypt.HashPassword(password.Trim());
    }

    public static bool Verificar(string password, string passwordHash)
    {
        if (string.IsNullOrWhiteSpace(password) || string.IsNullOrWhiteSpace(passwordHash))
            return false;

        var input = password.Trim();
        var almacenado = passwordHash.Trim();

        if (string.Equals(input, almacenado, StringComparison.Ordinal))
            return true;

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
