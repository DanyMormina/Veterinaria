namespace Veterinaria.Controllers.Seguridad;

public static class HasheadorContrasena
{
    public static string Hashear(string contrasena)
    {
        if (string.IsNullOrWhiteSpace(contrasena))
            return string.Empty;

        return BCrypt.Net.BCrypt.HashPassword(contrasena.Trim());
    }

    public static bool Verificar(string contrasena, string hashContrasena)
    {
        if (string.IsNullOrWhiteSpace(contrasena) || string.IsNullOrWhiteSpace(hashContrasena))
            return false;

        var input = contrasena.Trim();
        var almacenado = hashContrasena.Trim();

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
