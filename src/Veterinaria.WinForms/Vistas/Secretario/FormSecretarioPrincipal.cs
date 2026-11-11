using Microsoft.Extensions.DependencyInjection;
using Veterinaria.WinForms.Sesion;

namespace Veterinaria.WinForms.Vistas.Secretario;
// Formulario principal de shell para el rol Secretario / Recepción.
// Funciona como punto de entrada para atención al público: registro de propietarios, mascotas, cobros y reportes operativos.
public partial class FormSecretarioPrincipal : Form
{
    private readonly IServiceProvider _serviceProvider; // Contenedor de Inyección de Dependencias

    // Constructor con inyección del proveedor de servicios
    public FormSecretarioPrincipal(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
        InitializeComponent();
    }

    // Muestra en pantalla el nombre del recepcionista autenticado y la fecha actual de trabajo
    private void FormSecretarioPrincipal_Load(object? sender, EventArgs e)
    {
        lblUsuarioSesion.Text = SesionActual.EstaAutenticado
            ? $"Recepción: {SesionActual.NombreCompleto} | {SesionActual.Rol}"
            : "Recepción: Secretario";

        lblInfoEstado.Text = $"Operando como {SesionActual.NombreUsuario} - {DateTime.Now:dd/MM/yyyy}";
    }

    // Abre el formulario de Propietarios para alta, búsqueda y actualización de clientes
    private void BTPROPIETARIOS_Click(object? sender, EventArgs e)
    {
        using var alcance = _serviceProvider.CreateScope();
        var vistaPropietarios = alcance.ServiceProvider.GetService<FormPropietarios>() ?? new FormPropietarios();
        vistaPropietarios.ShowDialog(this);
    }

    // Abre el módulo de gestión de Mascotas (ABM completo, razas y especies)
    private void BTMASCOTAS_Click(object? sender, EventArgs e)
    {
        using var alcance = _serviceProvider.CreateScope();
        var vistaMascotas = alcance.ServiceProvider.GetService<FormMascotas>() ?? new FormMascotas();
        vistaMascotas.ShowDialog(this);
    }

    // Abre el módulo de Caja y Cobros de consultas veterinarias pendientes
    private void BTCOBROS_Click(object? sender, EventArgs e)
    {
        using var alcance = _serviceProvider.CreateScope();
        var vistaCobros = alcance.ServiceProvider.GetService<FormCobros>() ?? new FormCobros();
        vistaCobros.ShowDialog(this);
    }

    // Abre el centro de Reportes operativos habilitados para el rol de Secretaría
    private void BTREPORTES_Click(object? sender, EventArgs e)
    {
        using var alcance = _serviceProvider.CreateScope();
        var vista = alcance.ServiceProvider.GetRequiredService<FormReportes>();
        vista.ShowDialog(this);
    }

    // Cierra la sesión activa de recepción y vuelve al login principal
    private void btnCerrarSesion_Click(object? sender, EventArgs e)
    {
        Close();
    }
}
