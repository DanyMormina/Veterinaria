using Microsoft.Extensions.DependencyInjection;
using Veterinaria.WinForms.Sesion;

namespace Veterinaria.WinForms.Vistas.Veterinario;

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

        lblInfoEstado.Text = $"Sesión clínica activa: {SesionActual.NombreUsuario} - {DateTime.Now:dd/MM/yyyy}";
    }

    private void button1_Click(object sender, EventArgs e)
    {

    }

    private void BTCONSULTAS_Click(object sender, EventArgs e)
    {
        using var alcance = _serviceProvider.CreateScope();
        var vista = alcance.ServiceProvider.GetRequiredService<FormConsultas>();
        vista.ShowDialog(this);
    }

    private void BTFICHAMEDICA_Click(object sender, EventArgs e)
    {
        using var alcance = _serviceProvider.CreateScope();
        var vista = alcance.ServiceProvider.GetRequiredService<FormFichaMedica>();
        vista.ShowDialog(this);
    }

    private void BTTRATAMIENTOS_Click(object sender, EventArgs e)
    {
        using var alcance = _serviceProvider.CreateScope();
        var vista = alcance.ServiceProvider.GetRequiredService<FormTratamientos>();
        vista.ShowDialog(this);
    }

    private void BTVACUNAS_Click(object sender, EventArgs e)
    {
        using var alcance = _serviceProvider.CreateScope();
        var vista = alcance.ServiceProvider.GetRequiredService<FormVacunasControles>();
        vista.ShowDialog(this);
    }

    private void btnCerrarSesion_Click(object? sender, EventArgs e)
    {
        Close();
    }

}
