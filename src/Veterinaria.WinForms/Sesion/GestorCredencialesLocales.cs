using System.Text.Json;

namespace Veterinaria.WinForms.Sesion;

/// <summary>
/// Modelo de datos para almacenar múltiples cuentas recordadas.
/// </summary>
public class CredencialUsuario
{
    public string Usuario { get; set; } = string.Empty;
    public string Contrasena { get; set; } = string.Empty;
    public DateTime UltimoAcceso { get; set; } = DateTime.UtcNow;
}

public class ConfigCredenciales
{
    public string? UltimoUsuario { get; set; }
    public List<CredencialUsuario> Cuentas { get; set; } = [];
}

/// <summary>
/// Gestor persistente de múltiples credenciales de inicio de sesión con soporte de autocompletado.
/// </summary>
public static class GestorCredencialesLocales
{
    private static readonly string RutaDirectorio = Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
        "Veterinaria");

    private static readonly string RutaArchivo = Path.Combine(RutaDirectorio, "credenciales.json");

    public static void Guardar(string usuario, string contrasena)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(usuario))
                return;

            if (!Directory.Exists(RutaDirectorio))
            {
                Directory.CreateDirectory(RutaDirectorio);
            }

            var config = CargarConfiguracion() ?? new ConfigCredenciales();
            var cuentaExistente = config.Cuentas.FirstOrDefault(c =>
                string.Equals(c.Usuario, usuario.Trim(), StringComparison.OrdinalIgnoreCase));

            if (cuentaExistente is not null)
            {
                cuentaExistente.Usuario = usuario.Trim();
                cuentaExistente.Contrasena = contrasena;
                cuentaExistente.UltimoAcceso = DateTime.UtcNow;
            }
            else
            {
                config.Cuentas.Add(new CredencialUsuario
                {
                    Usuario = usuario.Trim(),
                    Contrasena = contrasena,
                    UltimoAcceso = DateTime.UtcNow
                });
            }

            config.UltimoUsuario = usuario.Trim();

            var json = JsonSerializer.Serialize(config, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(RutaArchivo, json);
        }
        catch
        {
            // Silencioso ante excepciones de I/O para no bloquear el flujo
        }
    }

    public static void Eliminar(string usuario)
    {
        try
        {
            var config = CargarConfiguracion();
            if (config is null)
                return;

            config.Cuentas.RemoveAll(c =>
                string.Equals(c.Usuario, usuario.Trim(), StringComparison.OrdinalIgnoreCase));

            if (string.Equals(config.UltimoUsuario, usuario.Trim(), StringComparison.OrdinalIgnoreCase))
            {
                config.UltimoUsuario = config.Cuentas
                    .OrderByDescending(c => c.UltimoAcceso)
                    .FirstOrDefault()?.Usuario;
            }

            var json = JsonSerializer.Serialize(config, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(RutaArchivo, json);
        }
        catch
        {
            // Silencioso
        }
    }

    public static CredencialUsuario? ObtenerUltimo()
    {
        var config = CargarConfiguracion();
        if (config is null || config.Cuentas.Count == 0)
            return null;

        if (!string.IsNullOrWhiteSpace(config.UltimoUsuario))
        {
            var ultimo = config.Cuentas.FirstOrDefault(c =>
                string.Equals(c.Usuario, config.UltimoUsuario, StringComparison.OrdinalIgnoreCase));
            if (ultimo is not null)
                return ultimo;
        }

        return config.Cuentas.OrderByDescending(c => c.UltimoAcceso).FirstOrDefault();
    }

    public static CredencialUsuario? ObtenerPorUsuario(string usuario)
    {
        if (string.IsNullOrWhiteSpace(usuario))
            return null;

        var config = CargarConfiguracion();
        return config?.Cuentas.FirstOrDefault(c =>
            string.Equals(c.Usuario, usuario.Trim(), StringComparison.OrdinalIgnoreCase));
    }

    public static string[] ObtenerNombresUsuarios()
    {
        var config = CargarConfiguracion();
        if (config is null || config.Cuentas.Count == 0)
            return [];

        return config.Cuentas
            .OrderByDescending(c => c.UltimoAcceso)
            .Select(c => c.Usuario)
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .ToArray();
    }

    private static ConfigCredenciales? CargarConfiguracion()
    {
        try
        {
            if (!File.Exists(RutaArchivo))
                return null;

            var json = File.ReadAllText(RutaArchivo);
            return JsonSerializer.Deserialize<ConfigCredenciales>(json);
        }
        catch
        {
            return null;
        }
    }
}
