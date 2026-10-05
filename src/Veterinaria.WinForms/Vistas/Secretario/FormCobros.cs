using System.Globalization;
using System.Text;
using Veterinaria.Controllers.Controladores;
using Veterinaria.Domain.Dtos;
using Veterinaria.WinForms.Sesion;

namespace Veterinaria.WinForms.Vistas.Secretario;

/// <summary>
/// Gestión de cobros en Secretaría vinculada a consultas clínicas.
/// </summary>
public partial class FormCobros : Form
{
    private static readonly Color ColorFondoError = ColorTranslator.FromHtml("#FDECEF");
    private static readonly Color ColorEtiquetaError = ColorTranslator.FromHtml("#B85D69");
    private static readonly Color ColorTextoNormal = ColorTranslator.FromHtml("#3A353B");
    private static readonly Color ColorExito = ColorTranslator.FromHtml("#8FA89B");

    private readonly PagoControlador? _pagoControlador;
    private readonly ConsultaControlador? _consultaControlador;
    private readonly DetalleConsultaControlador? _detalleConsultaControlador;
    private readonly AplicacionVacunaControlador? _aplicacionVacunaControlador;
    private readonly MetodoPagoControlador? _metodoPagoControlador;

    private long? _idPagoSeleccionado;
    private bool _cargando;
    private string? _snapshotEdicion;

    private sealed record ItemCombo(long? Id, string Texto)
    {
        public override string ToString() => Texto;
    }

    public FormCobros() : this(null, null, null, null, null)
    {
    }

    public FormCobros(
        PagoControlador? pagoControlador,
        ConsultaControlador? consultaControlador,
        DetalleConsultaControlador? detalleConsultaControlador,
        AplicacionVacunaControlador? aplicacionVacunaControlador,
        MetodoPagoControlador? metodoPagoControlador)
    {
        _pagoControlador = pagoControlador;
        _consultaControlador = consultaControlador;
        _detalleConsultaControlador = detalleConsultaControlador;
        _aplicacionVacunaControlador = aplicacionVacunaControlador;
        _metodoPagoControlador = metodoPagoControlador;

        InitializeComponent();
        ConfigurarEventos();
    }

    private void ConfigurarEventos()
    {
        txtImporte.KeyPress += TxtImporte_KeyPress;
        txtImporte.TextChanged += TxtImporte_TextChanged;
        txtImporte.Leave += TxtImporte_Leave;

        btnCobrar.Click += async (_, _) => await CobrarAsync();
        btnModificar.Click += async (_, _) => await ModificarAsync();
        btnLimpiar.Click += (_, _) => LimpiarFormulario();
        cboConsultaClinica.SelectedIndexChanged += async (_, _) => await OnConsultaSeleccionadaAsync();
        cboMetodoPago.SelectedIndexChanged += (_, _) => ActualizarVisibilidadCuotas();
        dgvCobros.CellClick += (_, _) => CargarPagoSeleccionado();
        dgvCobros.SelectionChanged += (_, _) => CargarPagoSeleccionado();

        cboCuotas.Items.Clear();
        cboCuotas.Items.AddRange(["1 pago", "3 cuotas", "6 cuotas", "9 cuotas", "12 cuotas"]);
        cboCuotas.SelectedIndex = 0;
        ActualizarVisibilidadCuotas();
    }

    private async void FormCobros_Load(object? sender, EventArgs e)
    {
        lblUsuarioSesion.Text = SesionActual.EstaAutenticado
            ? $"Recepción: {SesionActual.NombreCompleto} | {SesionActual.Rol}"
            : "Recepción: Secretario";

        cboEstado.Items.Clear();
        cboEstado.Items.AddRange(["Pendiente", "Completado", "Anulado"]);
        btnModificar.Enabled = false;

        await CargarCatalogosAsync();
        await CargarCobrosAsync();
        LimpiarFormulario();
        lblInfoEstado.Text = "Módulo de cobros — recepción";
    }

    private async Task CargarCatalogosAsync()
    {
        _cargando = true;
        try
        {
            cboConsultaClinica.Items.Clear();
            cboConsultaClinica.Items.Add(new ItemCombo(null, "Seleccione una consulta clínica"));

            if (_consultaControlador is not null)
            {
                var res = await _consultaControlador.ObtenerTodosAsync();
                if (res.EsExitoso && res.Valor is not null)
                {
                    foreach (var c in res.Valor.OrderByDescending(x => x.FechaHora))
                    {
                        var texto = $"Consulta #{c.Id} - {c.NombreMascota} ({c.NombrePropietario}) - {c.FechaHora:dd/MM/yyyy HH:mm}";
                        cboConsultaClinica.Items.Add(new ItemCombo(c.Id, texto));
                    }
                }
            }

            cboConsultaClinica.SelectedIndex = 0;

            cboMetodoPago.Items.Clear();
            cboMetodoPago.Items.Add(new ItemCombo(null, "Seleccione un método de pago"));
            if (_metodoPagoControlador is not null)
            {
                var resMetodos = await _metodoPagoControlador.ObtenerTodosAsync();
                if (resMetodos.EsExitoso && resMetodos.Valor is not null)
                {
                    foreach (var m in resMetodos.Valor.Where(x => x.Activo).OrderBy(x => x.Nombre))
                        cboMetodoPago.Items.Add(new ItemCombo(m.Id, m.Nombre));
                }
            }

            cboMetodoPago.SelectedIndex = 0;
            cboEstado.SelectedIndex = -1;
        }
        finally
        {
            _cargando = false;
        }
    }

    private async Task CargarCobrosAsync()
    {
        _cargando = true;
        try
        {
            dgvCobros.Rows.Clear();
            if (_pagoControlador is null)
                return;

            var res = await _pagoControlador.ObtenerTodosAsync();
            if (!res.EsExitoso || res.Valor is null)
                return;

            foreach (var p in res.Valor)
            {
                var metodo = p.NombreMetodoPago;
                if (p.Cuotas is > 0 && EsTarjetaCredito(metodo))
                    metodo = $"{metodo} ({FormatearCuotas(p.Cuotas.Value)})";

                var idx = dgvCobros.Rows.Add(
                    p.Id,
                    p.DescripcionConsulta,
                    metodo,
                    p.Fecha.ToString("dd/MM/yyyy"),
                    $"$ {p.Importe:N2}",
                    p.Estado);
                dgvCobros.Rows[idx].Tag = p;
            }

            dgvCobros.ClearSelection();
            dgvCobros.CurrentCell = null;
        }
        finally
        {
            _cargando = false;
        }
    }

    private async Task OnConsultaSeleccionadaAsync()
    {
        if (_cargando)
            return;

        var item = cboConsultaClinica.SelectedItem as ItemCombo;
        if (item?.Id is null || item.Id <= 0)
        {
            txtDetalleImporte.Clear();
            txtImporte.Clear();
            return;
        }

        var (detalle, total) = await CalcularDetalleImporteAsync(item.Id.Value);
        txtDetalleImporte.Text = detalle;
        txtImporte.Text = total > 0 ? total.ToString("0.00", CultureInfo.InvariantCulture) : string.Empty;
    }

    private async Task<(string Detalle, decimal Total)> CalcularDetalleImporteAsync(long idConsulta)
    {
        var lineas = new List<(string Concepto, decimal Monto)>();
        var total = 0m;

        if (_consultaControlador is not null)
        {
            var resConsulta = await _consultaControlador.ObtenerPorIdAsync(idConsulta);
            if (resConsulta.EsExitoso && resConsulta.Valor is not null)
            {
                var importeBase = resConsulta.Valor.Importe;
                if (importeBase > 0)
                {
                    lineas.Add(("Consulta", importeBase));
                    total += importeBase;
                }
            }
        }

        if (_detalleConsultaControlador is not null)
        {
            var resDet = await _detalleConsultaControlador.ObtenerPorConsultaAsync(idConsulta);
            if (resDet.EsExitoso && resDet.Valor is not null)
            {
                foreach (var d in resDet.Valor)
                {
                    var concepto = d.Cantidad > 1
                        ? $"Tratamiento - {d.DescripcionTratamiento} x{d.Cantidad}"
                        : $"Tratamiento - {d.DescripcionTratamiento}";
                    lineas.Add((concepto, d.Subtotal));
                    total += d.Subtotal;
                }
            }
        }

        if (_aplicacionVacunaControlador is not null)
        {
            var resVac = await _aplicacionVacunaControlador.ObtenerPorConsultaAsync(idConsulta);
            if (resVac.EsExitoso && resVac.Valor is not null)
            {
                foreach (var v in resVac.Valor)
                {
                    var precio = v.PrecioUnitario ?? 0m;
                    lineas.Add(($"Vacuna - {v.NombreVacuna}", precio));
                    total += precio;
                }
            }
        }

        var sb = new StringBuilder();
        const int ancho = 52;
        if (lineas.Count == 0)
        {
            sb.AppendLine("Sin importes cargados para esta consulta.");
        }
        else
        {
            foreach (var (concepto, monto) in lineas)
                sb.AppendLine(FormatearLineaTicket(concepto, monto, ancho));

            sb.AppendLine(new string('-', ancho));
            sb.Append(FormatearLineaTicket("TOTAL", total, ancho));
        }

        return (sb.ToString().TrimEnd(), total);
    }

    private static string FormatearLineaTicket(string concepto, decimal monto, int ancho)
    {
        var montoTexto = $"$ {monto:N2}";
        var maxConcepto = Math.Max(8, ancho - montoTexto.Length - 1);
        var texto = concepto.Length > maxConcepto
            ? concepto[..(maxConcepto - 1)] + "…"
            : concepto;
        return texto.PadRight(maxConcepto) + " " + montoTexto;
    }

    private async Task CobrarAsync()
    {
        if (!ValidarFormulario(out var dto, out var mensaje))
        {
            MessageBox.Show(mensaje, "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        if (_pagoControlador is null)
        {
            MessageBox.Show("El servicio de cobros no está disponible.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return;
        }

        try
        {
            btnCobrar.Enabled = false;
            var resultado = await _pagoControlador.RegistrarPagoAsync(dto);
            if (resultado.EsExitoso)
            {
                lblInfoEstado.ForeColor = ColorExito;
                lblInfoEstado.Text = resultado.Mensaje;
                LimpiarFormulario();
                await CargarCobrosAsync();
            }
            else
            {
                MessageBox.Show(resultado.Mensaje, "Error al cobrar", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        finally
        {
            btnCobrar.Enabled = true;
        }
    }

    private async Task ModificarAsync()
    {
        if (!_idPagoSeleccionado.HasValue || _idPagoSeleccionado.Value <= 0)
        {
            MessageBox.Show("Seleccione un cobro de la lista para modificarlo.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        if (!ValidarFormulario(out var dto, out var mensaje))
        {
            MessageBox.Show(mensaje, "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        if (string.Equals(ObtenerSnapshotFormulario(), _snapshotEdicion, StringComparison.Ordinal))
        {
            MessageBox.Show("No se detectaron cambios para guardar.", "Sin cambios", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        if (_pagoControlador is null)
            return;

        try
        {
            btnModificar.Enabled = false;
            var resultado = await _pagoControlador.ActualizarAsync(_idPagoSeleccionado.Value, dto);
            if (resultado.EsExitoso)
            {
                lblInfoEstado.ForeColor = ColorExito;
                lblInfoEstado.Text = resultado.Mensaje;
                LimpiarFormulario();
                await CargarCobrosAsync();
            }
            else
            {
                MessageBox.Show(resultado.Mensaje, "Error al modificar", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        finally
        {
            btnModificar.Enabled = _idPagoSeleccionado.HasValue;
        }
    }

    private bool ValidarFormulario(out PagoSolicitudDto dto, out string mensaje)
    {
        dto = new PagoSolicitudDto();
        mensaje = string.Empty;

        var consulta = cboConsultaClinica.SelectedItem as ItemCombo;
        if (consulta?.Id is null || consulta.Id <= 0)
        {
            mensaje = "Debe seleccionar una consulta clínica.";
            cboConsultaClinica.Focus();
            return false;
        }

        var metodo = cboMetodoPago.SelectedItem as ItemCombo;
        if (metodo?.Id is null || metodo.Id <= 0)
        {
            mensaje = "Debe seleccionar un método de pago.";
            cboMetodoPago.Focus();
            return false;
        }

        if (cboEstado.SelectedIndex < 0 || cboEstado.SelectedItem is null)
        {
            mensaje = "Debe seleccionar el estado del cobro.";
            cboEstado.Focus();
            return false;
        }

        if (!ValidarImporte(out var importe))
        {
            mensaje = "El importe debe ser un número decimal mayor a 0.";
            return false;
        }

        int? cuotas = null;
        if (EsTarjetaCredito(metodo.Texto))
        {
            cuotas = ObtenerCuotasSeleccionadas();
            if (cuotas is null)
            {
                mensaje = "Debe seleccionar la cantidad de cuotas para tarjeta de crédito.";
                cboCuotas.Focus();
                return false;
            }
        }

        dto = new PagoSolicitudDto
        {
            IdConsulta = consulta.Id.Value,
            IdMetodoPago = metodo.Id.Value,
            Fecha = dtpFecha.Value,
            Importe = importe,
            Estado = cboEstado.SelectedItem.ToString() ?? "Pendiente",
            Cuotas = cuotas
        };
        return true;
    }

    private void ActualizarVisibilidadCuotas()
    {
        var metodo = cboMetodoPago.SelectedItem as ItemCombo;
        var esCredito = metodo is not null && EsTarjetaCredito(metodo.Texto);
        lblCuotas.Visible = esCredito;
        cboCuotas.Visible = esCredito;
        if (esCredito && cboCuotas.SelectedIndex < 0 && cboCuotas.Items.Count > 0)
            cboCuotas.SelectedIndex = 0;
    }

    private static bool EsTarjetaCredito(string? nombre) =>
        !string.IsNullOrWhiteSpace(nombre) &&
        (nombre.Contains("Crédito", StringComparison.OrdinalIgnoreCase) ||
         nombre.Contains("Credito", StringComparison.OrdinalIgnoreCase));

    private int? ObtenerCuotasSeleccionadas()
    {
        return cboCuotas.SelectedIndex switch
        {
            0 => 1,
            1 => 3,
            2 => 6,
            3 => 9,
            4 => 12,
            _ => null
        };
    }

    private static string FormatearCuotas(int cuotas) =>
        cuotas <= 1 ? "1 pago" : $"{cuotas} cuotas";

    private void SeleccionarCuotas(int? cuotas)
    {
        var idx = cuotas switch
        {
            1 => 0,
            3 => 1,
            6 => 2,
            9 => 3,
            12 => 4,
            _ => 0
        };
        if (cboCuotas.Items.Count > idx)
            cboCuotas.SelectedIndex = idx;
    }

    private void CargarPagoSeleccionado()
    {
        if (_cargando || dgvCobros.CurrentRow?.Tag is not PagoRespuestaDto pago)
            return;

        _cargando = true;
        try
        {
            _idPagoSeleccionado = pago.Id;
            SeleccionarComboPorId(cboConsultaClinica, pago.IdConsulta);
            SeleccionarComboPorId(cboMetodoPago, pago.IdMetodoPago);
            ActualizarVisibilidadCuotas();
            SeleccionarCuotas(pago.Cuotas);
            dtpFecha.Value = pago.Fecha.Date;
            txtImporte.Text = pago.Importe.ToString("0.00", CultureInfo.InvariantCulture);

            var estadoUi = pago.Estado.Equals("Completado", StringComparison.OrdinalIgnoreCase) ? "Completado"
                : pago.Estado.Equals("Anulado", StringComparison.OrdinalIgnoreCase) ? "Anulado"
                : "Pendiente";
            var idxEstado = cboEstado.Items.IndexOf(estadoUi);
            cboEstado.SelectedIndex = idxEstado >= 0 ? idxEstado : 0;

            btnCobrar.Enabled = false;
            btnModificar.Enabled = true;
            _snapshotEdicion = ObtenerSnapshotFormulario();
            lblInfoEstado.Text = $"Cobro #{pago.Id} cargado para modificación.";
        }
        finally
        {
            _cargando = false;
        }

        _ = OnConsultaSeleccionadaAsync();
    }

    private string ObtenerSnapshotFormulario()
    {
        var idConsulta = (cboConsultaClinica.SelectedItem as ItemCombo)?.Id ?? 0;
        var idMetodo = (cboMetodoPago.SelectedItem as ItemCombo)?.Id ?? 0;
        return string.Join("|",
            idConsulta,
            idMetodo,
            ObtenerCuotasSeleccionadas()?.ToString() ?? string.Empty,
            dtpFecha.Value.Date.ToString("yyyy-MM-dd"),
            txtImporte.Text.Trim(),
            cboEstado.SelectedItem?.ToString() ?? string.Empty);
    }

    private void LimpiarFormulario()
    {
        _idPagoSeleccionado = null;
        _snapshotEdicion = null;
        if (cboConsultaClinica.Items.Count > 0)
            cboConsultaClinica.SelectedIndex = 0;
        if (cboMetodoPago.Items.Count > 0)
            cboMetodoPago.SelectedIndex = 0;
        if (cboCuotas.Items.Count > 0)
            cboCuotas.SelectedIndex = 0;
        ActualizarVisibilidadCuotas();
        cboEstado.SelectedIndex = -1;
        dtpFecha.Value = DateTime.Today;
        txtImporte.Clear();
        txtDetalleImporte.Clear();
        btnCobrar.Enabled = true;
        btnModificar.Enabled = false;
        dgvCobros.ClearSelection();
        dgvCobros.CurrentCell = null;
        RestaurarEstiloImporte();
        cboConsultaClinica.Focus();
    }

    private static void SeleccionarComboPorId(ComboBox cbo, long id)
    {
        for (var i = 0; i < cbo.Items.Count; i++)
        {
            if (cbo.Items[i] is ItemCombo item && item.Id == id)
            {
                cbo.SelectedIndex = i;
                return;
            }
        }
    }

    private void TxtImporte_KeyPress(object? sender, KeyPressEventArgs e)
    {
        if (char.IsControl(e.KeyChar) || char.IsDigit(e.KeyChar))
            return;

        if (e.KeyChar is '.' or ',')
        {
            if (sender is TextBox tb)
            {
                var texto = tb.Text;
                var seleccion = tb.SelectedText;
                var yaTieneSeparador = (texto.Contains('.') || texto.Contains(',')) &&
                                       !(seleccion.Contains('.') || seleccion.Contains(','));
                if (!yaTieneSeparador)
                    return;
            }
        }

        e.Handled = true;
    }

    private void TxtImporte_TextChanged(object? sender, EventArgs e)
    {
        RestaurarEstiloImporte();
        if (string.IsNullOrEmpty(txtImporte.Text))
            return;

        var texto = txtImporte.Text;
        var textoLimpio = new StringBuilder();
        var tieneSeparador = false;
        foreach (var c in texto)
        {
            if (char.IsDigit(c))
                textoLimpio.Append(c);
            else if (c is '.' or ',' && !tieneSeparador)
            {
                textoLimpio.Append(c);
                tieneSeparador = true;
            }
        }

        if (textoLimpio.ToString() != texto)
        {
            var cursor = txtImporte.SelectionStart;
            txtImporte.Text = textoLimpio.ToString();
            txtImporte.SelectionStart = Math.Min(cursor, txtImporte.Text.Length);
        }
    }

    private void TxtImporte_Leave(object? sender, EventArgs e)
    {
        var texto = txtImporte.Text.Trim().Replace(',', '.');
        if (decimal.TryParse(texto, CultureInfo.InvariantCulture, out var valor) && valor > 0)
        {
            txtImporte.Text = valor.ToString("0.00", CultureInfo.InvariantCulture);
            RestaurarEstiloImporte();
        }
    }

    private bool ValidarImporte(out decimal importe)
    {
        importe = 0m;
        var texto = txtImporte.Text.Trim().Replace(',', '.');
        if (string.IsNullOrWhiteSpace(texto))
        {
            MarcarErrorImporte("El importe es obligatorio.");
            return false;
        }

        if (!decimal.TryParse(texto, CultureInfo.InvariantCulture, out importe) || importe <= 0)
        {
            MarcarErrorImporte("El importe debe ser un número decimal mayor a 0.");
            return false;
        }

        RestaurarEstiloImporte();
        return true;
    }

    private void MarcarErrorImporte(string mensaje)
    {
        pnlImporte.BackColor = ColorFondoError;
        txtImporte.BackColor = ColorFondoError;
        lblSimboloPeso.BackColor = ColorFondoError;
        lblImporte.ForeColor = ColorEtiquetaError;
        lblInfoEstado.ForeColor = ColorEtiquetaError;
        lblInfoEstado.Text = mensaje;
        txtImporte.Focus();
        txtImporte.SelectAll();
    }

    private void RestaurarEstiloImporte()
    {
        pnlImporte.BackColor = Color.White;
        txtImporte.BackColor = Color.White;
        lblSimboloPeso.BackColor = Color.White;
        lblImporte.ForeColor = ColorTextoNormal;
        lblInfoEstado.ForeColor = ColorTextoNormal;
        if (_idPagoSeleccionado is null)
            lblInfoEstado.Text = "Módulo de cobros — recepción";
    }

    private void btnCancelar_Click(object? sender, EventArgs e)
    {
        Close();
    }
}
