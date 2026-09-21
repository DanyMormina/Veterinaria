using System.Globalization;
using Veterinaria.Controllers.Controladores;
using Veterinaria.Domain.Dtos;

namespace Veterinaria.WinForms.Vistas.Veterinario;

/// <summary>
/// Diálogo modal auxiliar para modificar o dar de baja lógica vacunas existentes en el catálogo médico.
/// </summary>
public partial class FormModificarVacuna : Form
{
    private readonly VacunaControlador? _vacunaControlador;
    private readonly long? _idInicial;
    private List<VacunaRespuestaDto> _vacunas = [];
    private bool _cargandoDatos = false;

    private static readonly CultureInfo CulturaArgentina = new("es-AR");

    public FormModificarVacuna() : this(null, null)
    {
    }

    public FormModificarVacuna(VacunaControlador? vacunaControlador, long? idInicial = null)
    {
        InitializeComponent();
        _vacunaControlador = vacunaControlador;
        _idInicial = idInicial;

        ConfigurarEventos();
    }

    private void ConfigurarEventos()
    {
        Load += FormModificarVacuna_Load;
        cboSeleccionarVacuna.SelectedIndexChanged += CboSeleccionarVacuna_SelectedIndexChanged;
        txtPrecioUnitario.KeyPress += TxtPrecioUnitario_KeyPress;
    }

    private async void FormModificarVacuna_Load(object? sender, EventArgs e)
    {
        await CargarVacunasAsync();
    }

    private async Task CargarVacunasAsync()
    {
        if (_vacunaControlador is null)
        {
            MessageBox.Show(
                "No se pudo acceder al controlador del catálogo de vacunas.",
                "Error de Dependencia",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
            return;
        }

        _cargandoDatos = true;
        try
        {
            // 1. Obtener todas las vacunas activas
            var resultado = await _vacunaControlador.ObtenerTodosAsync();
            if (!resultado.EsExitoso || resultado.Valor is null)
            {
                MessageBox.Show(
                    resultado.Mensaje ?? "No se pudieron obtener las vacunas registradas.",
                    "Error de Carga",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return;
            }

            _vacunas = resultado.Valor
                .Where(v => v.Activo)
                .OrderBy(v => v.Nombre)
                .ToList();

            cboSeleccionarVacuna.DataSource = null;
            cboSeleccionarVacuna.DisplayMember = nameof(VacunaItemVista.Texto);
            cboSeleccionarVacuna.ValueMember = nameof(VacunaItemVista.Id);

            var items = _vacunas.Select(v => new VacunaItemVista
            {
                Id = v.Id,
                Texto = string.IsNullOrWhiteSpace(v.NombreEspecie)
                    ? $"{v.Nombre} — Precio: {v.Precio.ToString("C2", CulturaArgentina)}"
                    : $"{v.Nombre} ({v.NombreEspecie}) — Precio: {v.Precio.ToString("C2", CulturaArgentina)}"
            }).ToList();

            cboSeleccionarVacuna.DataSource = items;

            if (items.Count > 0)
            {
                if (_idInicial.HasValue && items.Any(i => i.Id == _idInicial.Value))
                {
                    cboSeleccionarVacuna.SelectedValue = _idInicial.Value;
                }
                else
                {
                    cboSeleccionarVacuna.SelectedIndex = 0;
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

    private void CboSeleccionarVacuna_SelectedIndexChanged(object? sender, EventArgs e)
    {
        if (_cargandoDatos) return;
        PoblarCamposEdicion();
    }

    private void PoblarCamposEdicion()
    {
        if (cboSeleccionarVacuna.SelectedValue is not long id || id <= 0)
        {
            HabilitarEdicion(false);
            return;
        }

        var seleccionado = _vacunas.FirstOrDefault(v => v.Id == id);
        if (seleccionado is null)
        {
            HabilitarEdicion(false);
            return;
        }

        HabilitarEdicion(true);

        txtNombre.Text = seleccionado.Nombre;
        numPeriodoMeses.Value = Math.Clamp(seleccionado.PeriodoMesesRecomendado, numPeriodoMeses.Minimum, numPeriodoMeses.Maximum);
        txtPrecioUnitario.Text = seleccionado.Precio.ToString("N2", CulturaArgentina);
    }

    private void HabilitarEdicion(bool habilitar)
    {
        txtNombre.Enabled = habilitar;
        numPeriodoMeses.Enabled = habilitar;
        txtPrecioUnitario.Enabled = habilitar;
        btnModificar.Enabled = habilitar;
        btnBaja.Enabled = habilitar;

        if (!habilitar)
        {
            txtNombre.Clear();
            numPeriodoMeses.Value = 12;
            txtPrecioUnitario.Text = "0,00";
        }
    }

    private void TxtPrecioUnitario_KeyPress(object? sender, KeyPressEventArgs e)
    {
        if (char.IsControl(e.KeyChar) || char.IsDigit(e.KeyChar))
        {
            return;
        }

        if ((e.KeyChar == ',' || e.KeyChar == '.') &&
            !txtPrecioUnitario.Text.Contains(',') &&
            !txtPrecioUnitario.Text.Contains('.'))
        {
            return;
        }

        e.Handled = true;
    }

    private async void btnModificar_Click(object? sender, EventArgs e)
    {
        if (_vacunaControlador is null) return;

        // 1. Validar vacuna seleccionada
        if (cboSeleccionarVacuna.SelectedValue is not long id || id <= 0)
        {
            MessageBox.Show(
                "Debe seleccionar una vacuna para modificar.",
                "Selección Requerida",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning);
            return;
        }

        var seleccionado = _vacunas.FirstOrDefault(v => v.Id == id);
        if (seleccionado is null)
        {
            MessageBox.Show(
                "No se encontró la vacuna seleccionada en el catálogo cargado.",
                "Error de Selección",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning);
            return;
        }

        // 2. Validar nombre
        var nombre = txtNombre.Text.Trim();
        if (string.IsNullOrWhiteSpace(nombre))
        {
            MessageBox.Show(
                "El nombre de la vacuna es obligatorio.",
                "Dato Requerido",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning);
            txtNombre.Focus();
            return;
        }

        // 3. Parsear precio
        var textoPrecio = txtPrecioUnitario.Text.Trim().Replace("$", string.Empty).Trim();
        if (!decimal.TryParse(textoPrecio, NumberStyles.Any, CulturaArgentina, out var precio) &&
            !decimal.TryParse(textoPrecio, NumberStyles.Any, CultureInfo.InvariantCulture, out precio))
        {
            MessageBox.Show(
                "El precio unitario no posee un formato numérico válido.",
                "Formato Inválido",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning);
            txtPrecioUnitario.Focus();
            return;
        }

        if (precio < 0)
        {
            MessageBox.Show(
                "El precio unitario no puede ser negativo.",
                "Valor Inválido",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning);
            txtPrecioUnitario.Focus();
            return;
        }

        // 4. Preparar solicitud conservando especie original
        var solicitud = new VacunaSolicitudDto
        {
            IdEspecie = seleccionado.IdEspecie,
            Nombre = nombre,
            PeriodoMesesRecomendado = (int)numPeriodoMeses.Value,
            Precio = precio
        };

        btnModificar.Enabled = false;

        try
        {
            // 5. Ejecutar actualización con Result Pattern
            var resultado = await _vacunaControlador.ActualizarAsync(id, solicitud);
            if (!resultado.EsExitoso)
            {
                MessageBox.Show(
                    resultado.Mensaje,
                    "Error al Modificar Vacuna",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                btnModificar.Enabled = true;
                return;
            }

            MessageBox.Show(
                "Vacuna modificada exitosamente en el catálogo.",
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
        if (_vacunaControlador is null) return;

        // 1. Validar selección activa
        if (cboSeleccionarVacuna.SelectedValue is not long id || id <= 0)
        {
            MessageBox.Show(
                "Debe seleccionar una vacuna para dar de baja.",
                "Selección Requerida",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning);
            return;
        }

        var vacuna = _vacunas.FirstOrDefault(v => v.Id == id);
        var nombre = vacuna?.Nombre ?? $"ID {id}";

        // 2. Confirmación previa de baja lógica
        var confirmacion = MessageBox.Show(
            $"¿Está seguro de que desea dar de baja la vacuna \"{nombre}\"?\n\nLa vacuna no estará disponible para nuevas aplicaciones clínicas.",
            "Confirmar Baja de Vacuna",
            MessageBoxButtons.YesNo,
            MessageBoxIcon.Question,
            MessageBoxDefaultButton.Button2);

        if (confirmacion != DialogResult.Yes)
        {
            return;
        }

        btnBaja.Enabled = false;

        try
        {
            // 3. Ejecutar baja lógica asíncrona
            var resultado = await _vacunaControlador.EliminarAsync(id);
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
                "La vacuna fue dada de baja correctamente.",
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

    private sealed class VacunaItemVista
    {
        public long Id { get; init; }
        public string Texto { get; init; } = string.Empty;
    }
}
