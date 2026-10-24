namespace Veterinaria.WinForms.Vistas.Administrador;

/// <summary>
/// Segundo paso de la restauración: se muestra recién cuando la contraseña del administrador
/// fue verificada y solicita únicamente la contraseña del gerente.
/// </summary>
public partial class FormAutorizacionGerente : Form
{
    public string PasswordGerente => txtPasswordGerente.Text;

    public FormAutorizacionGerente()
    {
        InitializeComponent();

        btnAutorizar.Click += btnAutorizar_Click;
        btnCancelar.Click += (s, e) => { DialogResult = DialogResult.Cancel; Close(); };
        Load += (s, e) => txtPasswordGerente.Focus();
    }

    private void btnAutorizar_Click(object? sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(txtPasswordGerente.Text))
        {
            MessageBox.Show(
                "Debe ingresar la contraseña del gerente para autorizar la operación.",
                "Validación de Seguridad",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning);

            txtPasswordGerente.Focus();
            return;
        }

        DialogResult = DialogResult.OK;
        Close();
    }
}
