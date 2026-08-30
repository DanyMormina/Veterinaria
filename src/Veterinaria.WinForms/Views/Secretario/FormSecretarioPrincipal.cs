using Veterinaria.CrossCutting.Session;

namespace Veterinaria.WinForms.Views.Secretario;

/// <summary>
/// Formulario principal de shell para el rol Secretario / Recepción.
/// </summary>
public partial class FormSecretarioPrincipal : Form
{
    private readonly IServiceProvider _serviceProvider;

    public FormSecretarioPrincipal(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
        InitializeComponent();
    }

    private void FormSecretarioPrincipal_Load(object? sender, EventArgs e)
    {
        lblUsuarioSesion.Text = SesionActual.EstaAutenticado
            ? $"Recepción: {SesionActual.NombreCompleto} | {SesionActual.Rol}"
            : "Recepción: Secretario";

        lblStatusInfo.Text = $"Operando como {SesionActual.Username} - {DateTime.Now:dd/MM/yyyy}";
    }
}
