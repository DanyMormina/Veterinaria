using Veterinaria.Controllers.Controladores;
using Veterinaria.Domain.Dtos;

namespace Veterinaria.WinForms.Vistas.Veterinario;

/// <summary>
/// Diálogo modal auxiliar para dar de alta un nuevo tratamiento en el catálogo médico.
/// </summary>
public partial class FormAltaTratamiento : Form
{
    private readonly TratamientoControlador? _tratamientoControlador;

    private static readonly string[] TiposTratamientoPredefinidos =
    [
        "General",
        "Medicamentoso",
        "Quirúrgico",
        "Terapéutico",
        "Preventivo",
        "Dermatológico",
        "Odontológico",
        "Higiene y Estética",
        "Nutricional"
    ];

    public FormAltaTratamiento() : this(null)
    {
    }

    public FormAltaTratamiento(TratamientoControlador? tratamientoControlador)
    {
        InitializeComponent();
        _tratamientoControlador = tratamientoControlador;
        InicializarControles();
    }

    private void InicializarControles()
    {
        // 1. Cargar tipos de tratamiento en el selector
        cboTipo.Items.Clear();
        cboTipo.Items.AddRange(TiposTratamientoPredefinidos);
        cboTipo.SelectedIndex = 0;

        // 2. Establecer foco inicial en la descripción
        ActiveControl = txtDescripcion;
    }

    private async void btnGuardar_Click(object? sender, EventArgs e)
    {
        // 1. Validar que se haya especificado el controlador
        if (_tratamientoControlador is null)
        {
            MessageBox.Show(
                "No se pudo acceder al controlador de tratamientos clínicos.",
                "Error de Dependencia",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
            return;
        }

        // 2. Validaciones preventivas de entrada
        var descripcion = txtDescripcion.Text.Trim();
        if (string.IsNullOrWhiteSpace(descripcion))
        {
            MessageBox.Show(
                "La descripción o nombre comercial del tratamiento es obligatorio.",
                "Dato Requerido",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning);
            txtDescripcion.Focus();
            return;
        }

        var tipo = cboTipo.SelectedItem?.ToString() ?? "General";
        var dosis = string.IsNullOrWhiteSpace(txtDosis.Text) ? null : txtDosis.Text.Trim();
        var precio = numPrecio.Value;

        // 3. Crear DTO de solicitud
        var solicitud = new TratamientoSolicitudDto
        {
            TipoTratamiento = tipo,
            Descripcion = descripcion,
            Dosis = dosis,
            Precio = precio
        };

        btnGuardar.Enabled = false;

        try
        {
            // 4. Ejecutar creación asíncrona mediante Result pattern
            var resultado = await _tratamientoControlador.CrearAsync(solicitud);

            if (!resultado.EsExitoso)
            {
                MessageBox.Show(
                    resultado.Mensaje,
                    "Error al Guardar",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                btnGuardar.Enabled = true;
                return;
            }

            MessageBox.Show(
                "Tratamiento incorporado al catálogo exitosamente.",
                "Registro Exitoso",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);

            DialogResult = DialogResult.OK;
            Close();
        }
        catch (Exception ex)
        {
            MessageBox.Show(
                $"Ocurrió un error inesperado: {ex.Message}",
                "Error Inesperado",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
            btnGuardar.Enabled = true;
        }
    }

    private void btnCancelar_Click(object? sender, EventArgs e)
    {
        DialogResult = DialogResult.Cancel;
        Close();
    }
}
