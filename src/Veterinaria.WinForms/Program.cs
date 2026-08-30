using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Veterinaria.Infrastructure;
using Veterinaria.Services;
using Veterinaria.WinForms.Views.Admin;
using Veterinaria.WinForms.Views.Auth;
using Veterinaria.WinForms.Views.Secretario;
using Veterinaria.WinForms.Views.Veterinario;

namespace Veterinaria.WinForms;

/// <summary>
/// Punto de entrada principal de la aplicación WinForms (.NET 10).
/// </summary>
internal static class Program
{
    private const string FallbackConnectionString =
        "Server=(localdb)\\mssqllocaldb;Database=VeterinariaDb;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=True";

    /// <summary>
    /// Punto de inicio de la aplicación con validación de base de datos e inyección de dependencias.
    /// </summary>
    [STAThread]
    private static void Main()
    {
        // 1. Inicializar configuración de HighDPI y estilos visuales de Windows Forms
        ApplicationConfiguration.Initialize();

        // 2. Obtener cadena de conexión desde appsettings.json o fallback por defecto
        var connectionString = ObtenerCadenaConexion();

        // 3. Configurar contenedor de Inyección de Dependencias
        var services = new ServiceCollection();

        services.AddDbContext<VeterinariaDbContext>(options =>
        {
            options.UseSqlServer(connectionString, sqlOptions =>
            {
                sqlOptions.EnableRetryOnFailure(
                    maxRetryCount: 3,
                    maxRetryDelay: TimeSpan.FromSeconds(5),
                    errorNumbersToAdd: null);
            });
        });

        // Registrar servicios de negocio y controladores del sistema
        services.AddApplicationServices();

        // Registrar vistas de la interfaz de usuario en el contenedor
        services.AddTransient<FormLogin>();
        services.AddTransient<FormAdminPrincipal>();
        services.AddTransient<FormVeterinarioPrincipal>();
        services.AddTransient<FormSecretarioPrincipal>();

        var serviceProvider = services.BuildServiceProvider();

        // 4. Validador previo e inicializador de base de datos
        if (!ValidarEInicializarBaseDeDatos(serviceProvider, connectionString))
        {
            return;
        }

        // 5. Resolver y ejecutar el formulario de inicio de sesión
        var formLogin = serviceProvider.GetRequiredService<FormLogin>();
        Application.Run(formLogin);
    }

    /// <summary>
    /// Valida que el servidor de base de datos esté accesible e inicializa datos iniciales si no existen.
    /// </summary>
    private static bool ValidarEInicializarBaseDeDatos(IServiceProvider serviceProvider, string connectionString)
    {
        try
        {
            using var scope = serviceProvider.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<VeterinariaDbContext>();

            // Inicializar esquema y sembrado inicial de datos si la base está vacía
            DatabaseSeeder.InicializarAsync(dbContext).GetAwaiter().GetResult();

            return true;
        }
        catch (Exception ex)
        {
            MostrarErrorConexion(
                "Error al conectar con la base de datos SQL Server.",
                connectionString,
                ex.Message);
            return false;
        }
    }

    private static void MostrarErrorConexion(string mensajePrincipal, string connectionString, string detalleTecnico)
    {
        var mensaje = $"{mensajePrincipal}\n\n" +
                      $"Detalle:\n{detalleTecnico}\n\n" +
                      $"Cadena de conexión configurada:\n{connectionString}\n\n" +
                      "Por favor verifique que el servicio de SQL Server / LocalDB esté iniciado y accesible antes de abrir la aplicación.";

        MessageBox.Show(
            mensaje,
            "Error de Conexión a Base de Datos",
            MessageBoxButtons.OK,
            MessageBoxIcon.Error);
    }

    private static string ObtenerCadenaConexion()
    {
        try
        {
            const string appSettingsPath = "appsettings.json";
            if (File.Exists(appSettingsPath))
            {
                var json = File.ReadAllText(appSettingsPath);
                using var doc = JsonDocument.Parse(json);
                if (doc.RootElement.TryGetProperty("ConnectionStrings", out var csElement) &&
                    csElement.TryGetProperty("DefaultConnection", out var defaultCsElement))
                {
                    var conn = defaultCsElement.GetString();
                    if (!string.IsNullOrWhiteSpace(conn))
                        return conn;
                }
            }
        }
        catch
        {
            // Ignorar y usar fallback
        }

        return FallbackConnectionString;
    }
}