using Microsoft.Extensions.DependencyInjection;
using Veterinaria.WinForms.Sesion;

namespace Veterinaria.WinForms.Vistas.Administrador;
// Formulario principal de shell para el rol Administrador.
// Actúa como menú central y despachador de módulos (Usuarios, Propietarios, Mascotas, Reportes y Backups).
public partial class FormAdminPrincipal : Form
{
    private readonly IServiceProvider _serviceProvider; // Contenedor de Inyección de Dependencias de .NET

    // Constructor con inyección del proveedor de servicios
    public FormAdminPrincipal(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
        InitializeComponent();
    }

    // Configura la barra superior y de estado con los datos del usuario logueado en la sesión activa
    private void FormAdminPrincipal_Load(object? sender, EventArgs e)
    {
        lblUsuarioSesion.Text = SesionActual.EstaAutenticado
            ? $"Usuario: {SesionActual.NombreCompleto} | Rol: {SesionActual.Rol}"
            : "Usuario: Administrador";

        lblInfoEstado.Text = $"Conectado como {SesionActual.NombreUsuario} ({SesionActual.Rol}) - {DateTime.Now:dd/MM/yyyy}";
    }

    // Abre el módulo de gestión de Propietarios resolviendo sus dependencias en un alcance (scope) aislado
    private void button1_Click(object sender, EventArgs e)
    {
        using var alcance = _serviceProvider.CreateScope();
        var vistaPropietarios = alcance.ServiceProvider.GetRequiredService<FormPropietarios>();
        vistaPropietarios.ShowDialog(this);
    }

    // Abre el módulo de gestión de Usuarios del sistema (ABM, roles, contraseñas)
    private void BTUSUARIOS_Click(object sender, EventArgs e)
    {
        using var alcance = _serviceProvider.CreateScope();
        var vistaUsuarios = alcance.ServiceProvider.GetRequiredService<FormUsuarios>();
        vistaUsuarios.ShowDialog(this);
    }

    // Abre el módulo de consulta y auditoría de Mascotas para el Administrador
    private void BTMASCOTAS_Click(object sender, EventArgs e)
    {
        using var alcance = _serviceProvider.CreateScope();
        var vistaMascotas = alcance.ServiceProvider.GetRequiredService<FormMascotas>();
        vistaMascotas.ShowDialog(this);
    }

    // Abre el centro de Reportes Estadísticos y Gerenciales (gráficos GDI+, filtros, exportación CSV/impresión)
    private void BTREPORTES_Click(object sender, EventArgs e)
    {
        using var alcance = _serviceProvider.CreateScope();
        var vistaReportes = alcance.ServiceProvider.GetRequiredService<FormReportes>();
        vistaReportes.ShowDialog(this);
    }

    // Abre el centro de Backups y Restauración de la base de datos SQL Server
    private void BTBACKUP_Click(object sender, EventArgs e)
    {
        using var alcance = _serviceProvider.CreateScope();
        var vistaBackup = alcance.ServiceProvider.GetRequiredService<FormBackup>();
        vistaBackup.ShowDialog(this);
    }

    // Cierra la sesión del administrador y regresa a la pantalla de login
    private void btnCerrarSesion_Click(object? sender, EventArgs e)
    {
        Close();
    }

    private void pnlContenido_Paint(object sender, PaintEventArgs e)
    {

    }
}
