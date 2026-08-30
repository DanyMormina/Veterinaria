using Veterinaria.CrossCutting.Session;

namespace Veterinaria.WinForms.Views.Veterinario;

/// <summary>
/// Formulario principal de shell para el rol Veterinario (Atención clínica).
/// </summary>
public partial class FormVeterinarioPrincipal : Form
{
    private readonly IServiceProvider _serviceProvider;

    public FormVeterinarioPrincipal(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
        InitializeComponent();
    }

    private void FormVeterinarioPrincipal_Load(object? sender, EventArgs e)
    {
        lblUsuarioSesion.Text = SesionActual.EstaAutenticado
            ? $"Dr./Dra. {SesionActual.NombreCompleto} | {SesionActual.Rol}"
            : "Médico: Veterinario";

        lblStatusInfo.Text = $"Sesión clínica activa: {SesionActual.Username} - {DateTime.Now:dd/MM/yyyy}";
    }
}
