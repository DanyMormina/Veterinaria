using Veterinaria.WinForms.Sesion;

namespace Veterinaria.WinForms.Vistas.Veterinario;

public partial class FormFichaMedica : Form
{
    public FormFichaMedica()
    {
        InitializeComponent();
    }

    private void FormFichaMedica_Load(object? sender, EventArgs e)
    {
        lblUsuarioSesion.Text = SesionActual.EstaAutenticado
            ? $"Dr./Dra. {SesionActual.NombreCompleto} | {SesionActual.Rol}"
            : "Dr./Dra. Lucía Pérez | Veterinario";

        lblInfoEstado.Text = $"Sesión clínica activa: {SesionActual.NombreUsuario} - {DateTime.Now:dd/MM/yyyy}";
    }

    private void btnVolver_Click(object? sender, EventArgs e)
    {
        Close();
    }
}
