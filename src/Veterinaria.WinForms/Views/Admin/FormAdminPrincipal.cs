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

    private void button1_Click(object sender, EventArgs e)
    {

    }

    private void BTUSUARIOS_Click(object sender, EventArgs e)
    {
        // Como ambos formularios están en la carpeta Admin, Visual Studio los conecta directamente
        FormUsuarios vistaUsuarios = new FormUsuarios();

        vistaUsuarios.ShowDialog();
    }

    private void BTMASCOTAS_Click(object sender, EventArgs e)
    {

    }

    private void BTREPORTES_Click(object sender, EventArgs e)
    {

    }
}
