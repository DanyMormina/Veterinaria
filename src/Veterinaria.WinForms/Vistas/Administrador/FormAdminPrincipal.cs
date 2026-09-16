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
        FormPropietarios vistaPropietarios = new FormPropietarios();
        vistaPropietarios.ShowDialog();
    }

    private void BTUSUARIOS_Click(object sender, EventArgs e)
    {
        var vistaUsuarios = _serviceProvider.GetRequiredService<FormUsuarios>();
        vistaUsuarios.ShowDialog();
    }

    private void BTMASCOTAS_Click(object sender, EventArgs e)
    {
        FormMascotas vistaMascotas = new FormMascotas();
        vistaMascotas.ShowDialog();
    }

    private void BTREPORTES_Click(object sender, EventArgs e)
    {
        FormReportes vistaReportes = new FormReportes();
        vistaReportes.ShowDialog();
    }

    private void pnlContenido_Paint(object sender, PaintEventArgs e)
    {

    }
}
