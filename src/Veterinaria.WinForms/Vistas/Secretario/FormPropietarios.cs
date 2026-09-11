using Veterinaria.WinForms.Sesion;

namespace Veterinaria.WinForms.Vistas.Secretario;

/// <summary>
/// Formulario de gestión y consulta de propietarios para el rol Secretario / Recepción.
/// </summary>
public partial class FormPropietarios : Form
{
    public FormPropietarios()
    {
        InitializeComponent();
    }

    private void FormPropietarios_Load(object? sender, EventArgs e)
    {
        lblUsuarioSesion.Text = SesionActual.EstaAutenticado
            ? $"Recepción: {SesionActual.NombreCompleto} | {SesionActual.Rol}"
            : "Recepción: Secretario";
    }

    private void btnVolver_Click(object? sender, EventArgs e)
    {
        Close();
    }
}
