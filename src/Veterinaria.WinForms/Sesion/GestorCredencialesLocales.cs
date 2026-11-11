using System.Text.Json;

namespace Veterinaria.WinForms.Sesion;

// Modelo de datos que representa una cuenta recordada en el equipo del cliente
public class CredencialUsuario
{
    // Nombre de usuario para iniciar sesión (ej: "admin", "vet", "secretario")
    public string Usuario { get; set; } = string.Empty;

    // Contraseña recordada localmente
    public string Contrasena { get; set; } = string.Empty;

    // Fecha y hora del último acceso para ordenar por uso reciente
    public DateTime UltimoAcceso { get; set; } = DateTime.UtcNow;
}

// Estructura raíz del archivo JSON 'credenciales.json'
public class ConfigCredenciales
{
    // Nombre del último usuario que ingresó exitosamente (para precargar la pantalla)
    public string? UltimoUsuario { get; set; }

    // Lista de todas las cuentas guardadas en la máquina
    public List<CredencialUsuario> Cuentas { get; set; } = [];
}

// Gestor de persistencia local de credenciales en el equipo del cliente
// Guarda en un archivo JSON en %localappdata% para permitir recordar cuentas y autocompletar
public static class GestorCredencialesLocales
{
    // Ruta en el perfil del usuario de Windows: C:\Users\<Usuario>\AppData\Local\Veterinaria
    private static readonly string RutaDirectorio = Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
        "Veterinaria");

    // Archivo de almacenamiento físico: credenciales.json
    private static readonly string RutaArchivo = Path.Combine(RutaDirectorio, "credenciales.json");

    // Guarda o actualiza una credencial en el archivo local cuando la casilla 'Recordar' está marcada
    public static void Guardar(string usuario, string contrasena)
    {
        try
        {
            // 1. Si el usuario está vacío, no realiza ninguna acción
            if (string.IsNullOrWhiteSpace(usuario))
                return;

            // 2. Si la carpeta en %localappdata% no existe todavía, la crea
            if (!Directory.Exists(RutaDirectorio))
            {
                Directory.CreateDirectory(RutaDirectorio);
            }

            // 3. Carga las cuentas preexistentes del JSON o inicia una lista nueva
            var config = CargarConfiguracion() ?? new ConfigCredenciales();

            // 4. Comprueba si la cuenta ya existía previamente para actualizarla o agregarla
            var cuentaExistente = config.Cuentas.FirstOrDefault(c =>
                string.Equals(c.Usuario, usuario.Trim(), StringComparison.OrdinalIgnoreCase));

            if (cuentaExistente is not null)
            {
                // Actualiza la contraseña y la fecha de acceso
                cuentaExistente.Usuario = usuario.Trim();
                cuentaExistente.Contrasena = contrasena;
                cuentaExistente.UltimoAcceso = DateTime.UtcNow;
            }
            else
            {
                // Agrega la nueva cuenta a la lista
                config.Cuentas.Add(new CredencialUsuario
                {
                    Usuario = usuario.Trim(),
                    Contrasena = contrasena,
                    UltimoAcceso = DateTime.UtcNow
                });
            }

            // 5. Marca esta cuenta como la última utilizada
            config.UltimoUsuario = usuario.Trim();

            // 6. Serializa la lista en formato JSON con indentación legible y la escribe en disco
            var json = JsonSerializer.Serialize(config, new JsonSerializerOptions { WriteIndented = true });
            EscribirArchivoJson(json);
        }
        catch
        {
            // Manejo defensivo: Silencioso ante excepciones de I/O para nunca trabar el inicio de sesión
        }
    }

    // Elimina una cuenta guardada (por ejemplo, si el usuario desmarca 'Recordar' o si la cuenta fue desactivada)
    public static void Eliminar(string usuario)
    {
        try
        {
            // 1. Carga la configuración actual
            var config = CargarConfiguracion();
            if (config is null)
                return;

            // 2. Remueve la cuenta coincidente por nombre de usuario (sin distinguir mayúsculas)
            config.Cuentas.RemoveAll(c =>
                string.Equals(c.Usuario, usuario.Trim(), StringComparison.OrdinalIgnoreCase));

            // 3. Si la cuenta eliminada era la 'última utilizada', reasigna la más reciente restante
            if (string.Equals(config.UltimoUsuario, usuario.Trim(), StringComparison.OrdinalIgnoreCase))
            {
                config.UltimoUsuario = config.Cuentas
                    .OrderByDescending(c => c.UltimoAcceso)
                    .FirstOrDefault()?.Usuario;
            }

            // 4. Guarda los cambios actualizados en el archivo JSON
            var json = JsonSerializer.Serialize(config, new JsonSerializerOptions { WriteIndented = true });
            EscribirArchivoJson(json);
        }
        catch
        {
            // Silencioso
        }
    }

    // Escribe el contenido JSON en el perfil local y sincroniza la copia del repositorio para desarrollo
    private static void EscribirArchivoJson(string json)
    {
        // 1. Escribe en la ruta oficial del sistema: %localappdata%\Veterinaria\credenciales.json
        File.WriteAllText(RutaArchivo, json);

        try
        {
            // 2. Sincroniza con la copia en la raíz del proyecto para facilitar pruebas en Visual Studio
            var rutaDev = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\..\..\..\credenciales.json"));
            if (File.Exists(rutaDev))
            {
                File.WriteAllText(rutaDev, json);
                return;
            }

            var rutaAlt = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\..\..\credenciales.json"));
            if (File.Exists(rutaAlt))
            {
                File.WriteAllText(rutaAlt, json);
            }
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

        // Prioridad 1: Buscar la cuenta marcada expresamente en 'UltimoUsuario'
        if (!string.IsNullOrWhiteSpace(config.UltimoUsuario))
        {
            var ultimo = config.Cuentas.FirstOrDefault(c =>
                string.Equals(c.Usuario, config.UltimoUsuario, StringComparison.OrdinalIgnoreCase));
            if (ultimo is not null)
                return ultimo;
        }

        // Prioridad 2: Buscar la cuenta con la fecha de acceso más reciente
        return config.Cuentas.OrderByDescending(c => c.UltimoAcceso).FirstOrDefault();
    }

    // Busca si un nombre de usuario tipeado ya tiene contraseña guardada para autocompletar
    public static CredencialUsuario? ObtenerPorUsuario(string usuario)
    {
        if (string.IsNullOrWhiteSpace(usuario))
            return null;

        var config = CargarConfiguracion();
        return config?.Cuentas.FirstOrDefault(c =>
            string.Equals(c.Usuario, usuario.Trim(), StringComparison.OrdinalIgnoreCase));
    }

    // Devuelve todos los nombres de usuario recordados para poblar el AutoCompleteCustomSource de la caja de texto
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

    // Lee y deserializa de forma segura el archivo JSON desde el disco
    private static ConfigCredenciales? CargarConfiguracion()
    {
        try
        {
            // Si el archivo aún no fue creado, devuelve nulo
            if (!File.Exists(RutaArchivo))
                return null;

            // Lee el texto completo y lo convierte al modelo ConfigCredenciales
            var json = File.ReadAllText(RutaArchivo);
            return JsonSerializer.Deserialize<ConfigCredenciales>(json);
        }
        catch
        {
            // Ante cualquier archivo corrupto o ilegible, retorna nulo sin romper la app
            return null;
        }
    }
}
