using Veterinaria.WinForms.Session;

namespace Veterinaria.WinForms.Views.Veterinario;

public partial class FormConsultas : Form
{
    public FormConsultas()
    {
        InitializeComponent();
    }

    private void FormConsultas_Load(object? sender, EventArgs e)
    {
        lblUsuarioSesion.Text = SesionActual.EstaAutenticado
            ? $"Dr./Dra. {SesionActual.NombreCompleto} | {SesionActual.Rol}"
            : "Dr./Dra. Lucía Pérez | Veterinario";

        lblStatusInfo.Text = $"Sesión clínica activa: {SesionActual.Username} - {DateTime.Now:dd/MM/yyyy}";
    }

    private void btnVolver_Click(object? sender, EventArgs e)
    {
        Close();
    }
}
