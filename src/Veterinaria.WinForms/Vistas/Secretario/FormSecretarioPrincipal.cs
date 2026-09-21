using Microsoft.Extensions.DependencyInjection;
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
        using var alcance = _serviceProvider.CreateScope();
        var vistaPropietarios = alcance.ServiceProvider.GetService<FormPropietarios>() ?? new FormPropietarios();
        vistaPropietarios.ShowDialog(this);
    }

    private void BTMASCOTAS_Click(object? sender, EventArgs e)
    {
        using var alcance = _serviceProvider.CreateScope();
        var vistaMascotas = alcance.ServiceProvider.GetService<FormMascotas>() ?? new FormMascotas();
        vistaMascotas.ShowDialog(this);
    }

    private void btnCerrarSesion_Click(object? sender, EventArgs e)
    {
        Close();
    }
}
