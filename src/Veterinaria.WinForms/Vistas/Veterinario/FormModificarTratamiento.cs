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
        "Nutricional",
        "Diagnóstico",
        "Servicio"
    ];

    public FormModificarTratamiento() : this(null, null)
    {
    }

    public FormModificarTratamiento(TratamientoControlador? tratamientoControlador, long? idInicial = null)
    {
        InitializeComponent();
        Controles.ComboBuscable.Habilitar(cboSeleccionarTratamiento);
        _tratamientoControlador = tratamientoControlador;
        _idInicial = idInicial;
    }

    private async void FormModificarTratamiento_Load(object? sender, EventArgs e)
    {
        cboTipo.Items.Clear();
        cboTipo.Items.AddRange(TiposTratamientoPredefinidos);
        chkUsaStock.CheckedChanged += (_, _) => ActualizarVisibilidadStock();

        await CargarTratamientosAsync();
    }

    private void ActualizarVisibilidadStock()
    {
        var usaStock = chkUsaStock.Checked;
        lblStock.Visible = usaStock;
        numStock.Visible = usaStock;
        numStock.Enabled = usaStock && cboTipo.Enabled;
        if (!usaStock)
        {
            numStock.Minimum = 0;
            numStock.Value = 0;
        }
        else
        {
            var id = cboSeleccionarTratamiento.SelectedValue as long?;
            var seleccionado = id.HasValue ? _tratamientos.FirstOrDefault(t => t.Id == id.Value) : null;
            if (seleccionado is not null && seleccionado.UsaStock)
            {
                numStock.Minimum = seleccionado.Stock;
                if (numStock.Value < seleccionado.Stock)
                    numStock.Value = Math.Clamp(seleccionado.Stock, (int)numStock.Minimum, (int)numStock.Maximum);
            }
            else
            {
                numStock.Minimum = 0;
            }
        }
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

            _tratamientos = [.. resultado.Valor.Where(t => t.Activo).OrderBy(t => t.Descripcion)];

            cboSeleccionarTratamiento.DataSource = null;
            cboSeleccionarTratamiento.DisplayMember = nameof(TratamientoItemVista.Texto);
            cboSeleccionarTratamiento.ValueMember = nameof(TratamientoItemVista.Id);

            var items = _tratamientos.Select(t => new TratamientoItemVista
            {
                Id = t.Id,
                Texto = FormatearTextoCatalogo(t)
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

    private void CboSeleccionarTratamiento_SelectedIndexChanged(object? sender, EventArgs e)
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
        chkUsaStock.Checked = seleccionado.UsaStock;
        // Inmutabilidad de tipo: si ya controla stock, no puede convertirse en servicio sin stock
        chkUsaStock.Enabled = !seleccionado.UsaStock;
        chkUsaStock.Text = seleccionado.UsaStock
            ? "¿Controla stock? (Producto inventariable - para discontinuar use 'Dar de Baja')"
            : "¿Controla stock? (sí: productos; no: servicios)";
        numStock.Minimum = seleccionado.UsaStock ? seleccionado.Stock : 0;
        numStock.Value = seleccionado.UsaStock
            ? Math.Clamp(seleccionado.Stock, (int)numStock.Minimum, (int)numStock.Maximum)
            : 0;
        ActualizarVisibilidadStock();
    }

    private void HabilitarEdicion(bool habilitar)
    {
        cboTipo.Enabled = habilitar;
        txtDescripcion.Enabled = habilitar;
        txtDosis.Enabled = habilitar;
        numPrecio.Enabled = habilitar;
        chkUsaStock.Enabled = habilitar;
        numStock.Enabled = habilitar && chkUsaStock.Checked;
        btnModificar.Enabled = habilitar;
        btnBaja.Enabled = habilitar;

        if (!habilitar)
        {
            txtDescripcion.Clear();
            txtDosis.Clear();
            numPrecio.Value = 0;
            chkUsaStock.Checked = false;
            chkUsaStock.Text = "¿Controla stock? (sí: productos; no: servicios)";
            numStock.Minimum = 0;
            numStock.Value = 0;
            ActualizarVisibilidadStock();
        }
    }

    private async void BtnModificar_Click(object? sender, EventArgs e)
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

        var usaStock = chkUsaStock.Checked;
        if (usaStock && numStock.Value < 0)
        {
            MessageBox.Show(
                "Si el tratamiento controla stock, la cantidad no puede ser negativa.",
                "Stock inválido",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning);
            numStock.Focus();
            return;
        }

        var solicitud = new TratamientoSolicitudDto
        {
            TipoTratamiento = tipo,
            Descripcion = descripcion,
            Dosis = dosis,
            Precio = precio,
            UsaStock = usaStock,
            Stock = usaStock ? (int)numStock.Value : 0
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

    private async void BtnBaja_Click(object? sender, EventArgs e)
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

    private void BtnCancelar_Click(object? sender, EventArgs e)
    {
        DialogResult = DialogResult.Cancel;
        Close();
    }

    private static string FormatearTextoCatalogo(TratamientoRespuestaDto t)
    {
        var cultura = new CultureInfo("es-AR");
        var baseTexto = $"{t.Descripcion} - Precio: {t.Precio.ToString("C2", cultura)}";
        if (!t.UsaStock)
            return baseTexto;
        return t.Stock > 0
            ? $"{baseTexto} - Stock: {t.Stock}"
            : $"{baseTexto} - Sin stock";
    }

    private sealed class TratamientoItemVista
    {
        public long Id { get; init; }
        public string Texto { get; init; } = string.Empty;
    }
}

