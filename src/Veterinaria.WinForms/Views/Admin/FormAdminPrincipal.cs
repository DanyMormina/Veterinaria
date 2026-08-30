using Veterinaria.CrossCutting.Session;

namespace Veterinaria.WinForms.Views.Admin;

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

        lblStatusInfo.Text = $"Conectado como {SesionActual.Username} ({SesionActual.Rol}) - {DateTime.Now:dd/MM/yyyy}";
    }
}
