using Veterinaria.WinForms.Sesion;

namespace Veterinaria.WinForms.Vistas.Administrador;

public partial class FormMascotas : Form
{
    public FormMascotas()
    {
        InitializeComponent();
    }

    private void FormMascotas_Load(object? sender, EventArgs e)
    {
        lblUsuarioSesion.Text = SesionActual.EstaAutenticado
            ? $"Usuario: {SesionActual.NombreCompleto} | Rol: {SesionActual.Rol}"
            : "Usuario: Administrador";
    }

    private void btnVolver_Click(object? sender, EventArgs e)
    {
        Close();
    }
}
