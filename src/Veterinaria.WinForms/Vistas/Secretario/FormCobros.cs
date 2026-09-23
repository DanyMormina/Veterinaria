using Veterinaria.WinForms.Sesion;

namespace Veterinaria.WinForms.Vistas.Secretario;

/// <summary>
/// Maqueta visual de cobros en Secretaría (sin persistencia).
/// </summary>
public partial class FormCobros : Form
{
    public FormCobros()
    {
        InitializeComponent();
    }

    private void FormCobros_Load(object? sender, EventArgs e)
    {
        lblUsuarioSesion.Text = SesionActual.EstaAutenticado
            ? $"Recepción: {SesionActual.NombreCompleto} | {SesionActual.Rol}"
            : "Recepción: Secretario";

        cboMetodoPago.Items.Clear();
        cboMetodoPago.Items.AddRange(
        [
            "Efectivo",
            "Tarjeta de débito",
            "Tarjeta de crédito",
            "Transferencia bancaria"
        ]);
        cboMetodoPago.SelectedIndex = 0;

        cboEstado.Items.Clear();
        cboEstado.Items.AddRange(["Pendiente", "Pagado"]);
        cboEstado.SelectedIndex = 0;

        dtpFecha.Value = DateTime.Today;
        lblInfoEstado.Text = "Módulo de cobros — maqueta visual";
    }

    private void btnCancelar_Click(object? sender, EventArgs e)
    {
        Close();
    }
}
