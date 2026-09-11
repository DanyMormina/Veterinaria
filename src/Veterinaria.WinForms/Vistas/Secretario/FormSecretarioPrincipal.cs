using Veterinaria.WinForms.Sesion;

namespace Veterinaria.WinForms.Vistas.Secretario;

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

        lblInfoEstado.Text = $"Operando como {SesionActual.NombreUsuario} - {DateTime.Now:dd/MM/yyyy}";
    }

    private void BTPROPIETARIOS_Click(object? sender, EventArgs e)
    {
        using var vistaPropietarios = new FormPropietarios();
        vistaPropietarios.ShowDialog(this);
    }

    private void BTMASCOTAS_Click(object? sender, EventArgs e)
    {
        using var vistaMascotas = new FormMascotas();
        vistaMascotas.ShowDialog(this);
    }
}
