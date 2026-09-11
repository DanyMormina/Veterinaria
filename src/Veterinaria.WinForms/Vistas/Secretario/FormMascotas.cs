using Veterinaria.WinForms.Sesion;

namespace Veterinaria.WinForms.Vistas.Secretario;

/// <summary>
/// Formulario de gestión y consulta de mascotas para el rol Secretario / Recepción.
/// </summary>
public partial class FormMascotas : Form
{
    public FormMascotas()
    {
        InitializeComponent();
    }

    private void FormMascotas_Load(object? sender, EventArgs e)
    {
        lblUsuarioSesion.Text = SesionActual.EstaAutenticado
            ? $"Recepción: {SesionActual.NombreCompleto} | {SesionActual.Rol}"
            : "Recepción: Secretario";

        // Actualizar contador inicial de registros
        ActualizarTotalRegistros();
    }

    /// <summary>
    /// Actualiza la etiqueta con el total de registros presentes en la grilla de mascotas.
    /// </summary>
    public void ActualizarTotalRegistros()
    {
        var total = dgvMascotas.Rows.Count;
        lblTotalRegistros.Text = $"Total de registros: {total}";
    }

    private void dgvMascotas_RowsAdded(object? sender, DataGridViewRowsAddedEventArgs e)
    {
        ActualizarTotalRegistros();
    }

    private void dgvMascotas_RowsRemoved(object? sender, DataGridViewRowsRemovedEventArgs e)
    {
        ActualizarTotalRegistros();
    }

    private void btnVolver_Click(object? sender, EventArgs e)
    {
        Close();
    }
}
