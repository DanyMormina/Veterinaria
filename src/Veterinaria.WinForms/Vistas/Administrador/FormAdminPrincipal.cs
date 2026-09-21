using Microsoft.Extensions.DependencyInjection;
using Veterinaria.WinForms.Sesion;

namespace Veterinaria.WinForms.Vistas.Administrador;

/// <summary>
/// Formulario principal de shell para el rol Administrador.
/// </summary>
public partial class FormAdminPrincipal : Form
{
    private readonly IServiceProvider _serviceProvider;

    public FormAdminPrincipal(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
        InitializeComponent();
    }

    private void FormAdminPrincipal_Load(object? sender, EventArgs e)
    {
        lblUsuarioSesion.Text = SesionActual.EstaAutenticado
            ? $"Usuario: {SesionActual.NombreCompleto} | Rol: {SesionActual.Rol}"
            : "Usuario: Administrador";

        lblInfoEstado.Text = $"Conectado como {SesionActual.NombreUsuario} ({SesionActual.Rol}) - {DateTime.Now:dd/MM/yyyy}";
    }

    private void button1_Click(object sender, EventArgs e)
    {
        using var alcance = _serviceProvider.CreateScope();
        var vistaPropietarios = alcance.ServiceProvider.GetRequiredService<FormPropietarios>();
        vistaPropietarios.ShowDialog(this);
    }

    private void BTUSUARIOS_Click(object sender, EventArgs e)
    {
        using var alcance = _serviceProvider.CreateScope();
        var vistaUsuarios = alcance.ServiceProvider.GetRequiredService<FormUsuarios>();
        vistaUsuarios.ShowDialog(this);
    }

    private void BTMASCOTAS_Click(object sender, EventArgs e)
    {
        using var alcance = _serviceProvider.CreateScope();
        var vistaMascotas = alcance.ServiceProvider.GetRequiredService<FormMascotas>();
        vistaMascotas.ShowDialog(this);
    }

    private void BTREPORTES_Click(object sender, EventArgs e)
    {
        using var alcance = _serviceProvider.CreateScope();
        var vistaReportes = alcance.ServiceProvider.GetRequiredService<FormReportes>();
        vistaReportes.ShowDialog(this);
    }

    private void btnCerrarSesion_Click(object? sender, EventArgs e)
    {
        Close();
    }

    private void pnlContenido_Paint(object sender, PaintEventArgs e)
    {

    }
}
