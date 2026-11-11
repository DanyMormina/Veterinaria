using Microsoft.Extensions.DependencyInjection;
using Veterinaria.WinForms.Sesion;

namespace Veterinaria.WinForms.Vistas.Veterinario;
// Formulario principal de shell para el rol Veterinario (Atención clínica).
// Centraliza las operaciones del profesional: Consultas, Fichas Médicas, Tratamientos, Vacunas y Reportes clínicos.
public partial class FormVeterinarioPrincipal : Form
{
    private readonly IServiceProvider _serviceProvider; // Contenedor de Inyección de Dependencias

    // Constructor con inyección del proveedor de servicios
    public FormVeterinarioPrincipal(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
        InitializeComponent();
    }

    // Muestra en el encabezado el nombre del profesional veterinario conectado y su estado clínico
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

    // Abre el módulo de Consultas clínicas (registro de motivo, diagnóstico, recetas y vinculación con cobro)
    private void BTCONSULTAS_Click(object sender, EventArgs e)
    {
        using var alcance = _serviceProvider.CreateScope();
        var vista = alcance.ServiceProvider.GetRequiredService<FormConsultas>();
        vista.ShowDialog(this);
    }

    // Abre la Ficha Médica e Historial Clínico consolidado de la mascota
    private void BTFICHAMEDICA_Click(object sender, EventArgs e)
    {
        using var alcance = _serviceProvider.CreateScope();
        var vista = alcance.ServiceProvider.GetRequiredService<FormFichaMedica>();
        vista.ShowDialog(this);
    }

    // Abre el módulo de gestión y prescripción de Tratamientos médicos
    private void BTTRATAMIENTOS_Click(object sender, EventArgs e)
    {
        using var alcance = _serviceProvider.CreateScope();
        var vista = alcance.ServiceProvider.GetRequiredService<FormTratamientos>();
        vista.ShowDialog(this);
    }

    // Abre el control y calendario de Vacunas y Desparasitaciones de los pacientes
    private void BTVACUNAS_Click(object sender, EventArgs e)
    {
        using var alcance = _serviceProvider.CreateScope();
        var vista = alcance.ServiceProvider.GetRequiredService<FormVacunasControles>();
        vista.ShowDialog(this);
    }

    // Abre los Reportes estadísticos médicos habilitados para el rol de veterinario
    private void BTREPORTES_Click(object? sender, EventArgs e)
    {
        using var alcance = _serviceProvider.CreateScope();
        var vista = alcance.ServiceProvider.GetRequiredService<FormReportes>();
        vista.ShowDialog(this);
    }

    // Finaliza la sesión clínica activa y regresa a la pantalla de login
    private void btnCerrarSesion_Click(object? sender, EventArgs e)
    {
        Close();
    }
}
