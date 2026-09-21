using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Veterinaria.Infrastructure;
using Veterinaria.WinForms.Vistas.Administrador;
using Veterinaria.WinForms.Vistas.Autenticacion;
using Veterinaria.WinForms.Vistas.Secretario;
using Veterinaria.WinForms.Vistas.Veterinario;

namespace Veterinaria.WinForms;

/// <summary>
/// Punto de entrada principal de la aplicación WinForms (.NET 10).
/// </summary>
internal static class Program
{
    private const string CadenaConexionRespaldo =
        "Server=localhost,1433;Database=VeterinariaDb;User Id=sa;Password=Pass123456!;TrustServerCertificate=True;MultipleActiveResultSets=true;";

    /// <summary>
    /// Punto de inicio de la aplicación con validación de base de datos e inyección de dependencias.
    /// </summary>
    [STAThread]
    private static void Main()
    {
        // 1. Inicializar configuración de HighDPI y estilos visuales de Windows Forms
        ApplicationConfiguration.Initialize();

        // 2. Obtener cadena de conexión desde appsettings.json o fallback por defecto
        var cadenaConexion = ObtenerCadenaConexion();

        // 3. Configurar contenedor de Inyección de Dependencias
        var services = new ServiceCollection();

        services.AddDbContext<ContextoVeterinaria>(options =>
        {
            options.UseSqlServer(cadenaConexion, sqlOptions =>
            {
                sqlOptions.EnableRetryOnFailure(
                    maxRetryCount: 3,
                    maxRetryDelay: TimeSpan.FromSeconds(5),
                    errorNumbersToAdd: null);
            });
        });

        services.AgregarControladoresAplicacion();

        // Registrar vistas de la interfaz de usuario en el contenedor
        services.AddTransient<FormInicioSesion>();
        services.AddTransient<FormAdminPrincipal>();
        services.AddTransient<FormUsuarios>();
        services.AddTransient<Veterinaria.WinForms.Vistas.Administrador.FormPropietarios>();
        services.AddTransient<Veterinaria.WinForms.Vistas.Administrador.FormMascotas>();
        services.AddTransient<Veterinaria.WinForms.Vistas.Administrador.FormReportes>();
        services.AddTransient<Veterinaria.WinForms.Vistas.Secretario.FormPropietarios>();
        services.AddTransient<Veterinaria.WinForms.Vistas.Secretario.FormMascotas>();
        services.AddTransient<FormConsultas>();
        services.AddTransient<FormFichaMedica>();
        services.AddTransient<FormHistorialClinico>();
        services.AddTransient<FormTratamientos>();
        services.AddTransient<FormVacunasControles>();
        services.AddTransient<FormVeterinarioPrincipal>();
        services.AddTransient<FormSecretarioPrincipal>();

        var serviceProvider = services.BuildServiceProvider();

        // 4. Validador previo e inicializador de base de datos
        if (!ValidarEInicializarBaseDeDatos(serviceProvider, cadenaConexion))
        {
            return;
        }

        // 5. Resolver y ejecutar el formulario de inicio de sesión
        var formLogin = serviceProvider.GetRequiredService<FormInicioSesion>();
        Application.Run(formLogin);
    }

    /// <summary>
    /// Valida que el servidor de base de datos esté accesible e inicializa datos iniciales si no existen.
    /// </summary>
    private static bool ValidarEInicializarBaseDeDatos(IServiceProvider serviceProvider, string cadenaConexion)
    {
        try
        {
            using var scope = serviceProvider.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<ContextoVeterinaria>();

            // 1. Validar conectividad física con el servidor de base de datos
            if (!dbContext.Database.CanConnect())
            {
                MostrarErrorConexion(
                    "Error al conectar con el servidor de base de datos SQL Server.",
                    cadenaConexion,
                    "No se pudo establecer conexión con el servidor especificado.",
                    "Error de Conexión a Base de Datos");
                return false;
            }

            // 2. Inicializar esquema y sembrado inicial de datos si la base está vacía
            InicializadorDatos.InicializarAsync(dbContext).GetAwaiter().GetResult();

            return true;
        }
        catch (Exception ex)
        {
            var detalle = ObtenerMensajeExcepcionCompleto(ex);
            MostrarErrorConexion(
                "Error al inicializar los datos base del sistema.",
                cadenaConexion,
                detalle,
                "Error de Inicialización de Base de Datos");
            return false;
        }
    }

    private static string ObtenerMensajeExcepcionCompleto(Exception ex)
    {
        var sb = new System.Text.StringBuilder();
        sb.AppendLine(ex.Message);

        var actual = ex.InnerException;
        while (actual != null)
        {
            sb.AppendLine($"Detalle interno: {actual.Message}");
            actual = actual.InnerException;
        }

        return sb.ToString().TrimEnd();
    }

    private static void MostrarErrorConexion(string mensajePrincipal, string cadenaConexion, string detalleTecnico, string titulo = "Error de Conexión a Base de Datos")
    {
        var mensaje = $"{mensajePrincipal}\n\n" +
                      $"Detalle:\n{detalleTecnico}\n\n" +
                      $"Cadena de conexión configurada:\n{cadenaConexion}\n\n" +
                      "Por favor verifique que el servicio de SQL Server / LocalDB esté iniciado y accesible antes de abrir la aplicación.";

        MessageBox.Show(
            mensaje,
            titulo,
            MessageBoxButtons.OK,
            MessageBoxIcon.Error);
    }

    private static string ObtenerCadenaConexion()
    {
        try
        {
            var appSettingsPath = Path.Combine(AppContext.BaseDirectory, "appsettings.json");
            if (!File.Exists(appSettingsPath))
            {
                appSettingsPath = "appsettings.json";
            }

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

        return CadenaConexionRespaldo;
    }
}