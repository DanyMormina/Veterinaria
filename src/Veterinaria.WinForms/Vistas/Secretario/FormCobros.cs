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

    // Datos fiscales del comercio (ficticios) impresos en el encabezado del ticket
    private const string EmpresaNombreFantasia = "VETERINARIA PATITAS FELICES";
    private const string EmpresaRazonSocial = "Patitas Felices S.R.L.";
    private const string EmpresaCuit = "30-71234567-1";
    private const string EmpresaCondicionIva = "IVA Responsable Inscripto";
    private const string EmpresaIngresosBrutos = "901-712345-6 (Conv. Multilateral)";
    private const string EmpresaInicioActividades = "01/03/2018";
    private const string EmpresaDomicilio = "Av. Rivadavia 4521 - C1424CEK - CABA";
    private const string EmpresaTelefono = "(011) 4567-8910";
    private const string EmpresaCorreo = "contacto@patitasfelices.com.ar";
    private const string EmpresaPuntoVenta = "0001";

    private readonly PagoControlador? _pagoControlador;
    private readonly ConsultaControlador? _consultaControlador;
    private readonly DetalleConsultaControlador? _detalleConsultaControlador;
    private readonly AplicacionVacunaControlador? _aplicacionVacunaControlador;
    private readonly MetodoPagoControlador? _metodoPagoControlador;

    private long? _idPagoSeleccionado;
    private string? _estadoPagoSeleccionado;
    private bool _cargando;
    private List<PagoRespuestaDto> _cobros = [];

    // Los controladores comparten el mismo DbContext, que no admite dos consultas a la vez
    private readonly SemaphoreSlim _accesoDatos = new(1, 1);
    private int _versionConsultaSeleccionada;

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
        FormClosed += (_, _) => _versionConsultaSeleccionada++;
        Controles.ComboBuscable.Habilitar(cboConsultaClinica, cboMetodoPago);
        dtpFecha.MaxDate = DateTime.Today.AddDays(1).AddTicks(-1);
        ConfigurarEventos();
    }

    private void ConfigurarEventos()
    {
        txtImporte.KeyPress += TxtImporte_KeyPress;
        txtImporte.TextChanged += TxtImporte_TextChanged;
        txtImporte.Leave += TxtImporte_Leave;

        btnCobrar.Click += async (_, _) => await CobrarAsync();
        btnLimpiar.Click += (_, _) => LimpiarFormulario();
        txtBuscarCobro.TextChanged += (_, _) => MostrarCobros();
        btnImprimirTicket.Click += async (_, _) => await ImprimirTicketSeleccionadoAsync();
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

        await CargarCatalogosAsync();
        await CargarCobrosAsync();
        LimpiarFormulario();
        lblInfoEstado.Text = "Módulo de cobros — recepción";
    }

    private async Task<T> ConsultarAsync<T>(Func<Task<T>> consulta)
    {
        await _accesoDatos.WaitAsync();
        try
        {
            return await consulta();
        }
        finally
        {
            _accesoDatos.Release();
        }
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
                var res = await ConsultarAsync(_consultaControlador.ObtenerTodosAsync);
                if (IsDisposed)
                    return;
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
                var resMetodos = await ConsultarAsync(_metodoPagoControlador.ObtenerTodosAsync);
                if (IsDisposed)
                    return;
                if (resMetodos.EsExitoso && resMetodos.Valor is not null)
                {
                    foreach (var m in resMetodos.Valor.Where(x => x.Activo).OrderBy(x => x.Nombre))
                        cboMetodoPago.Items.Add(new ItemCombo(m.Id, m.Nombre));
                }
            }

            cboMetodoPago.SelectedIndex = 0;
        }
        finally
        {
            _cargando = false;
        }
    }

    private async Task CargarCobrosAsync()
    {
        _cobros = [];
        if (_pagoControlador is not null)
        {
            var res = await ConsultarAsync(_pagoControlador.ObtenerTodosAsync);
            if (res.EsExitoso && res.Valor is not null)
                _cobros = res.Valor.ToList();
        }

        if (!IsDisposed)
            MostrarCobros();
    }

    /// <summary>
    /// Lista todos los cobros que coinciden con el texto del buscador (en cualquier columna).
    /// </summary>
    private void MostrarCobros()
    {
        var termino = txtBuscarCobro.Text.Trim();

        _cargando = true;
        try
        {
            dgvCobros.Rows.Clear();
            foreach (var p in _cobros)
            {
                var metodo = p.NombreMetodoPago;
                if (p.Cuotas is > 0 && EsTarjetaCredito(metodo))
                    metodo = $"{metodo} ({FormatearCuotas(p.Cuotas.Value)})";

                var fecha = p.Fecha.ToString("dd/MM/yyyy");
                var importe = $"$ {p.Importe:N2}";

                if (termino.Length > 0 &&
                    !new[] { p.Id.ToString(), p.DescripcionConsulta, metodo, fecha, importe, p.Importe.ToString("0.00", CultureInfo.InvariantCulture) }
                        .Any(valor => valor.Contains(termino, StringComparison.OrdinalIgnoreCase)))
                    continue;

                var idx = dgvCobros.Rows.Add(p.Id, p.DescripcionConsulta, metodo, fecha, importe);
                dgvCobros.Rows[idx].Tag = p;
            }

            dgvCobros.ClearSelection();
            dgvCobros.CurrentCell = null;
        }
        finally
        {
            _cargando = false;
        }

        if (termino.Length > 0)
            lblInfoEstado.Text = $"{dgvCobros.Rows.Count} de {_cobros.Count} cobro(s) coinciden con \"{termino}\".";
    }

    private async Task OnConsultaSeleccionadaAsync()
    {
        if (_cargando)
            return;

        // Solo vale la respuesta de la última selección; las anteriores se descartan
        var version = ++_versionConsultaSeleccionada;
        bool EsVigente() => !IsDisposed && version == _versionConsultaSeleccionada;

        var item = cboConsultaClinica.SelectedItem as ItemCombo;
        if (item?.Id is null || item.Id <= 0)
        {
            txtDetalleImporte.Clear();
            txtImporte.Clear();
            return;
        }

        var (detalle, total) = await CalcularDetalleImporteAsync(item.Id.Value);
        if (!EsVigente())
            return;

        txtDetalleImporte.Text = detalle;
        txtImporte.Text = total > 0 ? total.ToString("0.00", CultureInfo.InvariantCulture) : string.Empty;

        if (!_idPagoSeleccionado.HasValue && _pagoControlador is not null)
        {
            var yaCobrada = await ConsultarAsync(() => _pagoControlador.ConsultaYaCobradaAsync(item.Id.Value));
            if (!EsVigente() || _idPagoSeleccionado.HasValue)
                return;

            btnCobrar.Enabled = !yaCobrada;
            if (yaCobrada)
            {
                lblInfoEstado.ForeColor = ColorEtiquetaError;
                lblInfoEstado.Text = $"La consulta #{item.Id.Value} ya fue cobrada. No se puede volver a cobrar.";
            }
            else
            {
                lblInfoEstado.ForeColor = ColorTextoNormal;
                lblInfoEstado.Text = "Módulo de cobros — recepción";
            }
        }
    }

    private async Task<(string Detalle, decimal Total)> CalcularDetalleImporteAsync(long idConsulta)
    {
        var lineas = new List<(string Concepto, decimal Monto)>();
        var total = 0m;

        if (_consultaControlador is not null)
        {
            var resConsulta = await ConsultarAsync(() => _consultaControlador.ObtenerPorIdAsync(idConsulta));
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
            var resDet = await ConsultarAsync(() => _detalleConsultaControlador.ObtenerPorConsultaAsync(idConsulta));
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
            var resVac = await ConsultarAsync(() => _aplicacionVacunaControlador.ObtenerPorConsultaAsync(idConsulta));
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
            var resultado = await ConsultarAsync(() => _pagoControlador.RegistrarPagoAsync(dto));
            if (IsDisposed)
                return;
            if (resultado.EsExitoso)
            {
                lblInfoEstado.ForeColor = ColorExito;
                lblInfoEstado.Text = resultado.Mensaje;

                var metodoTexto = (cboMetodoPago.SelectedItem as ItemCombo)?.Texto ?? string.Empty;
                var ticket = await ArmarTicketAsync(resultado.Valor, dto, metodoTexto);

                LimpiarFormulario();
                await CargarCobrosAsync();

                using var vistaTicket = new FormTicket(ticket);
                vistaTicket.ShowDialog(this);
            }
            else
            {
                MessageBox.Show(resultado.Mensaje, "Error al cobrar", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        finally
        {
            if (!IsDisposed)
                btnCobrar.Enabled = true;
        }
    }

    private async Task ImprimirTicketSeleccionadoAsync()
    {
        if (dgvCobros.CurrentRow?.Tag is not PagoRespuestaDto pago)
        {
            MessageBox.Show("Seleccione un cobro de la lista para imprimir su ticket.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        var dto = new PagoSolicitudDto
        {
            IdConsulta = pago.IdConsulta,
            IdMetodoPago = pago.IdMetodoPago,
            Fecha = pago.Fecha,
            Importe = pago.Importe,
            Estado = pago.Estado,
            Cuotas = pago.Cuotas
        };

        var ticket = await ArmarTicketAsync(pago.Id, dto, pago.NombreMetodoPago, reimpresion: true);
        using var vistaTicket = new FormTicket(ticket);
        vistaTicket.ShowDialog(this);
    }

    private async Task<string> ArmarTicketAsync(long idPago, PagoSolicitudDto pago, string metodoPago, bool reimpresion = false)
    {
        const int ancho = 52;
        var separador = new string('=', ancho);
        var linea = new string('-', ancho);
        var sb = new StringBuilder();

        sb.AppendLine(Centrar(EmpresaNombreFantasia, ancho));
        sb.AppendLine(Centrar($"Razón social: {EmpresaRazonSocial}", ancho));
        sb.AppendLine(Centrar($"CUIT: {EmpresaCuit}", ancho));
        sb.AppendLine(Centrar(EmpresaCondicionIva, ancho));
        sb.AppendLine(Centrar($"Ing. Brutos: {EmpresaIngresosBrutos}", ancho));
        sb.AppendLine(Centrar($"Inicio de actividades: {EmpresaInicioActividades}", ancho));
        sb.AppendLine(Centrar(EmpresaDomicilio, ancho));
        sb.AppendLine(Centrar($"Tel.: {EmpresaTelefono}", ancho));
        sb.AppendLine(Centrar(EmpresaCorreo, ancho));
        sb.AppendLine(separador);
        sb.AppendLine(Centrar("COMPROBANTE DE PAGO", ancho));
        sb.AppendLine(Centrar("DOCUMENTO NO VÁLIDO COMO FACTURA", ancho));
        if (reimpresion)
            sb.AppendLine(Centrar("*** REIMPRESIÓN ***", ancho));
        sb.AppendLine(separador);
        sb.AppendLine($"P.V.: {EmpresaPuntoVenta}   Ticket N°: {EmpresaPuntoVenta}-{idPago:D8}");
        sb.AppendLine($"Fecha de pago: {pago.Fecha:dd/MM/yyyy}");
        sb.AppendLine($"Emitido: {DateTime.Now:dd/MM/yyyy HH:mm}");
        if (SesionActual.EstaAutenticado)
            sb.AppendLine($"Atendió: {SesionActual.NombreCompleto}");
        sb.AppendLine(linea);

        if (_consultaControlador is not null)
        {
            var resConsulta = await ConsultarAsync(() => _consultaControlador.ObtenerPorIdAsync(pago.IdConsulta));
            if (resConsulta.EsExitoso && resConsulta.Valor is not null)
            {
                var c = resConsulta.Valor;
                sb.AppendLine($"Consulta N°: {c.Id}  ({c.FechaHora:dd/MM/yyyy HH:mm})");
                sb.AppendLine($"Propietario: {c.NombrePropietario}");
                sb.AppendLine($"Mascota: {c.NombreMascota}");
                if (!string.IsNullOrWhiteSpace(c.NombreUsuario))
                    sb.AppendLine($"Veterinario: {c.NombreUsuario}");
                if (!string.IsNullOrWhiteSpace(c.Motivo))
                    sb.AppendLine($"Motivo: {c.Motivo}");
                sb.AppendLine(linea);
            }
        }

        sb.AppendLine("DETALLE");
        var (detalle, _) = await CalcularDetalleImporteAsync(pago.IdConsulta);
        sb.AppendLine(detalle);
        sb.AppendLine(separador);

        sb.AppendLine(FormatearLineaTicket("TOTAL ABONADO", pago.Importe, ancho));
        sb.AppendLine($"Método de pago: {metodoPago}");
        if (pago.Cuotas is > 1)
            sb.AppendLine($"Cuotas: {pago.Cuotas} de $ {pago.Importe / pago.Cuotas.Value:N2}");
        sb.AppendLine(separador);
        sb.Append(Centrar("¡Gracias por su visita!", ancho));

        return sb.ToString();
    }

    private static string Centrar(string texto, int ancho) =>
        texto.Length >= ancho ? texto : texto.PadLeft((ancho + texto.Length) / 2).PadRight(ancho);

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
            Estado = _estadoPagoSeleccionado ?? "Completado",
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
        // CellClick y SelectionChanged llegan juntos al hacer clic: el mismo cobro se carga una sola vez
        if (_cargando || dgvCobros.CurrentRow?.Tag is not PagoRespuestaDto pago || _idPagoSeleccionado == pago.Id)
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
            _estadoPagoSeleccionado = pago.Estado;

            btnCobrar.Enabled = false;
            btnImprimirTicket.Enabled = true;
            EstablecerSoloLectura(true);
            lblInfoEstado.Text = $"Cobro #{pago.Id}: los cobros registrados no se modifican; solo se puede reimprimir el ticket.";
        }
        finally
        {
            _cargando = false;
        }

        _ = OnConsultaSeleccionadaAsync();
    }

    /// <summary>
    /// Un cobro ya registrado se muestra bloqueado: por seguridad no se puede editar.
    /// </summary>
    private void EstablecerSoloLectura(bool soloLectura)
    {
        cboConsultaClinica.Enabled = !soloLectura;
        cboMetodoPago.Enabled = !soloLectura;
        cboCuotas.Enabled = !soloLectura;
        dtpFecha.Enabled = !soloLectura;
        txtImporte.ReadOnly = soloLectura;
    }

    private void LimpiarFormulario()
    {
        _idPagoSeleccionado = null;
        _estadoPagoSeleccionado = null;
        EstablecerSoloLectura(false);
        if (cboConsultaClinica.Items.Count > 0)
            cboConsultaClinica.SelectedIndex = 0;
        if (cboMetodoPago.Items.Count > 0)
            cboMetodoPago.SelectedIndex = 0;
        if (cboCuotas.Items.Count > 0)
            cboCuotas.SelectedIndex = 0;
        ActualizarVisibilidadCuotas();
        dtpFecha.Value = DateTime.Today;
        txtImporte.Clear();
        txtDetalleImporte.Clear();
        btnCobrar.Enabled = true;
        btnImprimirTicket.Enabled = false;
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
