namespace Veterinaria.WinForms.Vistas.Administrador;

/// <summary>
/// Modal de doble factor para la autorización y confirmación de restauración de la base de datos.
/// Adhiere a la estética "Ejecutivo Romántico Pastel".
/// </summary>
public partial class FormConfirmarRestore : Form
{
    public string NombreArchivo { get; }

    public string PasswordConfirmacion => txtPasswordConfirmacion.Text;

    public FormConfirmarRestore(string nombreArchivo)
    {
        NombreArchivo = nombreArchivo;
        InitializeComponent();

        lblArchivoNombre.Text = nombreArchivo;

        btnConfirmar.Click += btnConfirmar_Click;
        btnCancelar.Click += (s, e) => { DialogResult = DialogResult.Cancel; Close(); };
        Load += (s, e) => txtPasswordConfirmacion.Focus();
    }

    private void btnConfirmar_Click(object? sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(txtPasswordConfirmacion.Text))
        {
            MessageBox.Show(
                "Debe ingresar su contraseña de administrador para autorizar la operación.",
                "Validación de Seguridad",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning);

            txtPasswordConfirmacion.Focus();
            return;
        }

        DialogResult = DialogResult.OK;
        Close();
    }
}
