using System.Globalization;
using Veterinaria.Controllers.Controladores;
using Veterinaria.Domain.Dtos;

namespace Veterinaria.WinForms.Vistas.Veterinario;

/// <summary>
/// Diálogo modal auxiliar para modificar o dar de baja lógica tratamientos existentes en el catálogo.
/// </summary>
public partial class FormModificarTratamiento : Form
{
    private readonly TratamientoControlador? _tratamientoControlador;
    private readonly long? _idInicial;
    private List<TratamientoRespuestaDto> _tratamientos = [];
    private bool _cargandoDatos = false;

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

    public FormModificarTratamiento() : this(null, null)
    {
    }

    public FormModificarTratamiento(TratamientoControlador? tratamientoControlador, long? idInicial = null)
    {
        InitializeComponent();
        _tratamientoControlador = tratamientoControlador;
        _idInicial = idInicial;
    }

    private async void FormModificarTratamiento_Load(object? sender, EventArgs e)
    {
        cboTipo.Items.Clear();
        cboTipo.Items.AddRange(TiposTratamientoPredefinidos);

        await CargarTratamientosAsync();
    }

    private async Task CargarTratamientosAsync()
    {
        if (_tratamientoControlador is null)
        {
            MessageBox.Show(
                "No se pudo acceder al controlador de tratamientos clínicos.",
                "Error de Dependencia",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
            return;
        }

        _cargandoDatos = true;
        try
        {
            var resultado = await _tratamientoControlador.ObtenerTodosAsync();
            if (!resultado.EsExitoso || resultado.Valor is null)
            {
                MessageBox.Show(
                    resultado.Mensaje ?? "No se pudieron obtener los tratamientos del catálogo.",
                    "Error de Carga",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return;
            }

            _tratamientos = resultado.Valor.Where(t => t.Activo).OrderBy(t => t.Descripcion).ToList();

            cboSeleccionarTratamiento.DataSource = null;
            cboSeleccionarTratamiento.DisplayMember = nameof(TratamientoItemVista.Texto);
            cboSeleccionarTratamiento.ValueMember = nameof(TratamientoItemVista.Id);

            var items = _tratamientos.Select(t => new TratamientoItemVista
            {
                Id = t.Id,
                Texto = $"{t.Descripcion} - Precio: {t.Precio.ToString("C2", new CultureInfo("es-AR"))}"
            }).ToList();

            cboSeleccionarTratamiento.DataSource = items;

            if (items.Count > 0)
            {
                if (_idInicial.HasValue && items.Any(i => i.Id == _idInicial.Value))
                {
                    cboSeleccionarTratamiento.SelectedValue = _idInicial.Value;
                }
                else
                {
                    cboSeleccionarTratamiento.SelectedIndex = 0;
                }

                PoblarCamposEdicion();
            }
            else
            {
                HabilitarEdicion(false);
            }
        }
        finally
        {
            _cargandoDatos = false;
        }
    }

    private void cboSeleccionarTratamiento_SelectedIndexChanged(object? sender, EventArgs e)
    {
        if (_cargandoDatos) return;
        PoblarCamposEdicion();
    }

    private void PoblarCamposEdicion()
    {
        if (cboSeleccionarTratamiento.SelectedValue is not long id)
        {
            HabilitarEdicion(false);
            return;
        }

        var seleccionado = _tratamientos.FirstOrDefault(t => t.Id == id);
        if (seleccionado is null)
        {
            HabilitarEdicion(false);
            return;
        }

        HabilitarEdicion(true);

        // Seleccionar tipo o agregar si no está en la lista estándar
        if (!cboTipo.Items.Contains(seleccionado.TipoTratamiento))
        {
            cboTipo.Items.Add(seleccionado.TipoTratamiento);
        }
        cboTipo.SelectedItem = seleccionado.TipoTratamiento;

        txtDescripcion.Text = seleccionado.Descripcion;
        txtDosis.Text = seleccionado.Dosis ?? string.Empty;
        numPrecio.Value = Math.Clamp(seleccionado.Precio, numPrecio.Minimum, numPrecio.Maximum);
    }

    private void HabilitarEdicion(bool habilitar)
    {
        cboTipo.Enabled = habilitar;
        txtDescripcion.Enabled = habilitar;
        txtDosis.Enabled = habilitar;
        numPrecio.Enabled = habilitar;
        btnModificar.Enabled = habilitar;
        btnBaja.Enabled = habilitar;

        if (!habilitar)
        {
            txtDescripcion.Clear();
            txtDosis.Clear();
            numPrecio.Value = 0;
        }
    }

    private async void btnModificar_Click(object? sender, EventArgs e)
    {
        if (_tratamientoControlador is null) return;

        if (cboSeleccionarTratamiento.SelectedValue is not long id || id <= 0)
        {
            MessageBox.Show(
                "Debe seleccionar un tratamiento para modificar.",
                "Selección Requerida",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning);
            return;
        }

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

        var solicitud = new TratamientoSolicitudDto
        {
            TipoTratamiento = tipo,
            Descripcion = descripcion,
            Dosis = dosis,
            Precio = precio
        };

        btnModificar.Enabled = false;

        try
        {
            var resultado = await _tratamientoControlador.ActualizarAsync(id, solicitud);
            if (!resultado.EsExitoso)
            {
                MessageBox.Show(
                    resultado.Mensaje,
                    "Error al Modificar",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                btnModificar.Enabled = true;
                return;
            }

            MessageBox.Show(
                "Tratamiento modificado exitosamente.",
                "Operación Exitosa",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);

            DialogResult = DialogResult.OK;
            Close();
        }
        catch (Exception ex)
        {
            MessageBox.Show(
                $"Ocurrió un error inesperado al modificar: {ex.Message}",
                "Error Inesperado",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
            btnModificar.Enabled = true;
        }
    }

    private async void btnBaja_Click(object? sender, EventArgs e)
    {
        if (_tratamientoControlador is null) return;

        if (cboSeleccionarTratamiento.SelectedValue is not long id || id <= 0)
        {
            MessageBox.Show(
                "Debe seleccionar un tratamiento para dar de baja.",
                "Selección Requerida",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning);
            return;
        }

        var tratamiento = _tratamientos.FirstOrDefault(t => t.Id == id);
        var nombre = tratamiento?.Descripcion ?? $"ID {id}";

        var confirmacion = MessageBox.Show(
            $"¿Está seguro de que desea dar de baja el tratamiento \"{nombre}\"?\n\nEl tratamiento no estará disponible para nuevas aplicaciones clínicas.",
            "Confirmar Baja de Tratamiento",
            MessageBoxButtons.YesNo,
            MessageBoxIcon.Question,
            MessageBoxDefaultButton.Button2);

        if (confirmacion != DialogResult.Yes)
            return;

        btnBaja.Enabled = false;

        try
        {
            var resultado = await _tratamientoControlador.EliminarAsync(id);
            if (!resultado.EsExitoso)
            {
                MessageBox.Show(
                    resultado.Mensaje,
                    "Error al Dar de Baja",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                btnBaja.Enabled = true;
                return;
            }

            MessageBox.Show(
                "El tratamiento fue dado de baja correctamente.",
                "Baja Exitosa",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);

            DialogResult = DialogResult.OK;
            Close();
        }
        catch (Exception ex)
        {
            MessageBox.Show(
                $"Ocurrió un error inesperado al procesar la baja: {ex.Message}",
                "Error Inesperado",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
            btnBaja.Enabled = true;
        }
    }

    private void btnCancelar_Click(object? sender, EventArgs e)
    {
        DialogResult = DialogResult.Cancel;
        Close();
    }

    private sealed class TratamientoItemVista
    {
        public long Id { get; init; }
        public string Texto { get; init; } = string.Empty;
    }
}
