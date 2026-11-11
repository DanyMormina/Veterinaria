using System.Globalization;
using System.Text;
using Veterinaria.Controllers.Controladores;
using Veterinaria.Domain.Dtos;
using Veterinaria.WinForms.Sesion;

namespace Veterinaria.WinForms.Vistas.Secretario;
// Gestión de cobros en Secretaría vinculada a consultas clínicas.
// Cobro en pago único con importe no modificable y calculadora de vuelto en efectivo.
public partial class FormCobros : Form
{
    private static readonly Color ColorFondoError = ColorTranslator.FromHtml("#FDECEF");
    private static readonly Color ColorEtiquetaError = ColorTranslator.FromHtml("#B85D69");
    private static readonly Color ColorTextoNormal = ColorTranslator.FromHtml("#3A353B");
    private static readonly Color ColorExito = ColorTranslator.FromHtml("#2E7D32");
    private static readonly Color ColorFondoBloqueado = ColorTranslator.FromHtml("#F7F5F8");

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

    // Configura las restricciones de controles y vincula los eventos de la pantalla
    private void ConfigurarEventos()
    {
        // El importe a cobrar es de solo lectura y calculado por el sistema (el usuario no lo puede alterar a mano)
        txtImporte.ReadOnly = true;
        txtImporte.TabStop = false;
        pnlImporte.BackColor = ColorFondoBloqueado;
        txtImporte.BackColor = ColorFondoBloqueado;
        lblSimboloPeso.BackColor = ColorFondoBloqueado;

        // Eventos para validar que en efectivo solo se escriban números y coma decimal
        txtAbonaCon.KeyPress += TxtAbonaCon_KeyPress;
        txtAbonaCon.TextChanged += TxtAbonaCon_TextChanged;
        txtAbonaCon.Leave += TxtAbonaCon_Leave;

        // Enlace de clics de botones principales
        btnCobrar.Click += async (_, _) => await CobrarAsync();
        btnLimpiar.Click += (_, _) => LimpiarFormulario();
        txtBuscarCobro.TextChanged += (_, _) => MostrarCobros();
        btnImprimirTicket.Click += async (_, _) => await ImprimirTicketSeleccionadoAsync();
        cboConsultaClinica.SelectedIndexChanged += async (_, _) => await OnConsultaSeleccionadaAsync();
        cboMetodoPago.SelectedIndexChanged += (_, _) => ActualizarVisibilidadMetodoPago();
        dgvCobros.CellClick += (_, _) => CargarPagoSeleccionado();
        dgvCobros.SelectionChanged += (_, _) => CargarPagoSeleccionado();

        // Opciones de cuotas disponibles para tarjetas
        cboCuotas.Items.Clear();
        cboCuotas.Items.AddRange(["1 pago", "3 cuotas", "6 cuotas", "9 cuotas", "12 cuotas"]);
        cboCuotas.SelectedIndex = 0;
        ActualizarVisibilidadMetodoPago();
    }

    // Evento que se dispara al abrir la pantalla de Cobros
    private async void FormCobros_Load(object? sender, EventArgs e)
    {
        // Muestra en la cabecera el usuario autenticado en la recepción
        lblUsuarioSesion.Text = SesionActual.EstaAutenticado
            ? $"Recepción: {SesionActual.NombreCompleto} | {SesionActual.Rol}"
            : "Recepción: Secretario";

        // Carga los métodos de pago y las consultas pendientes de cobro
        await CargarCatalogosAsync();
        // Carga el historial de cobros realizados en la grilla inferior
        await CargarCobrosAsync();
        // Deja el formulario en blanco listo para cobrar
        LimpiarFormulario();
        lblInfoEstado.Text = "Módulo de cobros --- recepción";
    }

    // Ejecuta consultas a la base de datos protegiéndolas con SemaphoreSlim
    // Evita errores de concurrencia ya que el DbContext de EF Core no admite dos consultas simultáneas
    private async Task<T> ConsultarAsync<T>(Func<Task<T>> consulta)
    {
        await _accesoDatos.WaitAsync(); // Espera su turno si hay otra consulta en curso
        try
        {
            return await consulta();    // Ejecuta la consulta a la base de datos
        }
        finally
        {
            _accesoDatos.Release();      // Libera el semáforo para la siguiente consulta
        }
    }

    // Carga los desplegables de Métodos de Pago y de Consultas Clínicas que aún no fueron cobradas
    private async Task CargarCatalogosAsync()
    {
        _cargando = true;
        try
        {
            // 1. Llena el combo de Métodos de Pago (Efectivo, Tarjeta, Mercado Pago, etc.)
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

            // 2. Obtiene las consultas que ya tienen un pago registrado para no duplicar cobros
            var idsYaCobradas = new HashSet<long>();
            if (_pagoControlador is not null)
            {
                var resPagos = await ConsultarAsync(_pagoControlador.ObtenerTodosAsync);
                if (IsDisposed)
                    return;
                if (resPagos.EsExitoso && resPagos.Valor is not null)
                {
                    foreach (var p in resPagos.Valor.Where(x =>
                                 x.Activo &&
                                 !string.Equals(x.Estado, "Anulado", StringComparison.OrdinalIgnoreCase)))
                        idsYaCobradas.Add(p.IdConsulta);
                }
            }

            // 3. Llena el combo únicamente con las consultas pendientes de pago
            cboConsultaClinica.Items.Clear();
            cboConsultaClinica.Items.Add(new ItemCombo(null, "Seleccione una consulta clínica pendiente"));

            if (_consultaControlador is not null)
            {
                var res = await ConsultarAsync(_consultaControlador.ObtenerTodosAsync);
                if (IsDisposed)
                    return;
                if (res.EsExitoso && res.Valor is not null)
                {
                    foreach (var c in res.Valor
                                 .Where(x => x.Activo && !idsYaCobradas.Contains(x.Id))
                                 .OrderByDescending(x => x.FechaHora))
                    {
                        var texto = $"Consulta #{c.Id} - {c.NombreMascota} ({c.NombrePropietario}) - {c.FechaHora:dd/MM/yyyy HH:mm}";
                        cboConsultaClinica.Items.Add(new ItemCombo(c.Id, texto));
                    }
                }
            }

            cboConsultaClinica.SelectedIndex = 0;
            var pendientes = Math.Max(0, cboConsultaClinica.Items.Count - 1);
            lblInfoEstado.Text = pendientes > 0
                ? $"{pendientes} consulta(s) clínica(s) pendiente(s) de cobro."
                : "No hay consultas pendientes de cobro.";
        }
        finally
        {
            _cargando = false;
        }
    }

    // Consulta en la base de datos todos los cobros históricos realizados
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
            MostrarCobros(); // Dibuja la lista en la grilla visual
    }

    // Filtra y dibuja los cobros en la grilla según lo que el usuario escribe en la barra de búsqueda
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

                // Si hay texto de búsqueda, compara contra ID, consulta, método, fecha e importe
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

    // Se dispara cuando el secretario elige una consulta del desplegable
    // Calcula automáticamente el importe sumando prácticas y vacunas
    private async Task OnConsultaSeleccionadaAsync()
    {
        if (_cargando)
            return;

        // Si el usuario cambia rápido de opción, descarta las respuestas viejas y solo atiende la última
        var version = ++_versionConsultaSeleccionada;
        bool EsVigente() => !IsDisposed && version == _versionConsultaSeleccionada;

        var item = cboConsultaClinica.SelectedItem as ItemCombo;
        if (item?.Id is null || item.Id <= 0)
        {
            txtDetalleImporte.Clear();
            txtImporte.Clear();
            txtAbonaCon.Clear();
            CalcularVuelto();
            return;
        }

        // Calcula el desglose y el importe total a cobrar para esta consulta
        var (detalle, total) = await CalcularDetalleImporteAsync(item.Id.Value);
        if (!EsVigente())
            return;

        txtDetalleImporte.Text = detalle;

        if (!_idPagoSeleccionado.HasValue)
        {
            // Asigna el importe calculado a la caja de texto (no editable por el usuario)
            txtImporte.Text = total > 0 ? total.ToString("0.00", CultureInfo.InvariantCulture) : string.Empty;
            if (string.IsNullOrWhiteSpace(txtAbonaCon.Text) || txtAbonaCon.Text == "0.00")
                txtAbonaCon.Text = txtImporte.Text;
            CalcularVuelto();

            // Verifica defensivamente en la base de datos que la consulta no haya sido cobrada previamente
            if (_pagoControlador is not null)
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
                    lblInfoEstado.Text = "Módulo de cobros --- recepción";
                }
            }
        }
    }

    // Algoritmo de cálculo de importe:
    // 1. Obtiene el importe base de la consulta médica
    // 2. Suma los tratamientos/servicios realizados (cantidad x subtotal)
    // 3. Suma las vacunas aplicadas en la atención
    // 4. Retorna el texto del desglose y el total monetario
    private async Task<(string Detalle, decimal Total)> CalcularDetalleImporteAsync(long idConsulta)
    {
        var lineas = new List<(string Concepto, decimal Monto)>();
        var total = 0m;

        // 1. Importe base de la consulta
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

        // 2. Tratamientos y servicios aplicados por el veterinario
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

        // 3. Vacunas aplicadas durante la atención
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

        // 4. Formatea el texto con el desglose y el renglón de TOTAL alineado a la derecha
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

    // Ejecuta el cobro de la consulta clínica seleccionada
    private async Task CobrarAsync()
    {
        // 1. Valida los datos requeridos (consulta elegida, método de pago, monto válido y efectivo suficiente)
        if (!ValidarFormulario(out var dto, out var mensaje))
        {
            MessageBox.Show(mensaje, "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        // 2. Comprobación defensiva del controlador
        if (_pagoControlador is null)
        {
            MessageBox.Show("El servicio de cobros no está disponible.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return;
        }

        try
        {
            // 3. Deshabilita el botón Cobrar para evitar doble clic o cobros duplicados accidentales
            btnCobrar.Enabled = false;

            // 4. Envía el registro de pago a la base de datos de manera asíncrona y segura
            var resultado = await ConsultarAsync(() => _pagoControlador.RegistrarPagoAsync(dto));
            if (IsDisposed)
                return;

            // 5. Si la base de datos confirmó el cobro con éxito
            if (resultado.EsExitoso)
            {
                lblInfoEstado.ForeColor = ColorExito;
                lblInfoEstado.Text = resultado.Mensaje;

                var metodoTexto = (cboMetodoPago.SelectedItem as ItemCombo)?.Texto ?? string.Empty;

                // Si pagó en efectivo, toma el valor con el que abonó para plasmarlo en el ticket
                decimal? abonaCon = null;
                if (EsEfectivo(metodoTexto))
                {
                    var textoAbona = txtAbonaCon.Text.Trim().Replace(',', '.');
                    if (decimal.TryParse(textoAbona, CultureInfo.InvariantCulture, out var vAbona))
                        abonaCon = vAbona;
                }

                // 6. Genera el texto del ticket con datos fiscales, desglose de servicios y vuelto
                var ticket = await ArmarTicketAsync(resultado.Valor, dto, metodoTexto, abonaCon: abonaCon);

                // 7. Limpia el formulario y refresca las consultas y cobros
                LimpiarFormulario();
                await CargarCatalogosAsync();
                await CargarCobrosAsync();

                // 8. Muestra la ventana modal del ticket de pago para previsualizar o imprimir
                using var vistaTicket = new FormTicket(ticket);
                vistaTicket.ShowDialog(this);
            }
            // Si la base de datos o validación rechazó la operación
            else
            {
                MessageBox.Show(resultado.Mensaje, "Error al cobrar", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        // El bloque finally se ejecuta SIEMPRE: rehabilita el botón si la pantalla sigue abierta
        finally
        {
            if (!IsDisposed)
                btnCobrar.Enabled = true;
        }
    }

    // Permite reimprimir el ticket fiscal de un cobro histórico seleccionado de la grilla
    private async Task ImprimirTicketSeleccionadoAsync()
    {
        // Verifica que haya una fila seleccionada en la tabla
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

        // Genera el ticket con la marca de [REIMPRESIÓN]
        var ticket = await ArmarTicketAsync(pago.Id, dto, pago.NombreMetodoPago, reimpresion: true);
        using var vistaTicket = new FormTicket(ticket);
        vistaTicket.ShowDialog(this);
    }

    // Construye el texto completo del ticket fiscal térmico para imprimir o visualizar en pantalla
    private async Task<string> ArmarTicketAsync(long idPago, PagoSolicitudDto pago, string metodoPago, decimal? abonaCon = null, bool reimpresion = false)
    {
        const int ancho = 52;
        var separador = new string('=', ancho);
        var linea = new string('-', ancho);
        var sb = new StringBuilder();

        // 1. Encabezado institucional con datos fiscales del comercio
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

        // 2. Datos de emisión, punto de venta y usuario que cobró
        sb.AppendLine($"P.V.: {EmpresaPuntoVenta}   Ticket N°: {EmpresaPuntoVenta}-{idPago:D8}");
        sb.AppendLine($"Fecha de pago: {pago.Fecha:dd/MM/yyyy}");
        sb.AppendLine($"Emitido: {DateTime.Now:dd/MM/yyyy HH:mm}");
        if (SesionActual.EstaAutenticado)
            sb.AppendLine($"Atendió: {SesionActual.NombreCompleto}");
        sb.AppendLine(linea);

        // 3. Datos de la consulta clínica (mascota, dueño, veterinario)
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

        // 4. Desglose detallado de tratamientos y vacunas aplicadas
        sb.AppendLine("DETALLE");
        var (detalle, _) = await CalcularDetalleImporteAsync(pago.IdConsulta);
        sb.AppendLine(detalle);
        sb.AppendLine(separador);

        // 5. Total abonado, método de pago, desglose de cuotas y vuelto si fue en efectivo
        sb.AppendLine(FormatearLineaTicket("TOTAL ABONADO", pago.Importe, ancho));
        sb.AppendLine($"Método de pago: {metodoPago}");
        if (pago.Cuotas is > 1)
            sb.AppendLine($"Cuotas: {pago.Cuotas} de $ {pago.Importe / pago.Cuotas.Value:N2}");

        if (abonaCon.HasValue && abonaCon.Value >= pago.Importe && EsEfectivo(metodoPago))
        {
            var vuelto = abonaCon.Value - pago.Importe;
            sb.AppendLine(FormatearLineaTicket("Abonó con (Efectivo)", abonaCon.Value, ancho));
            sb.AppendLine(FormatearLineaTicket("Su vuelto", vuelto, ancho));
        }

        sb.AppendLine(separador);
        sb.Append(Centrar("¡Gracias por su visita!", ancho));

        return sb.ToString();
    }

    private static string Centrar(string texto, int ancho) =>
        texto.Length >= ancho ? texto : texto.PadLeft((ancho + texto.Length) / 2).PadRight(ancho);

    // Valida las reglas de negocio del cobro antes de enviarlo a la base de datos
    private bool ValidarFormulario(out PagoSolicitudDto dto, out string mensaje)
    {
        dto = new PagoSolicitudDto();
        mensaje = string.Empty;

        // 1. Debe haber seleccionado una consulta clínica
        var consulta = cboConsultaClinica.SelectedItem as ItemCombo;
        if (consulta?.Id is null || consulta.Id <= 0)
        {
            mensaje = "Debe seleccionar una consulta clínica.";
            cboConsultaClinica.Focus();
            return false;
        }

        // 2. Debe haber seleccionado un método de pago
        var metodo = cboMetodoPago.SelectedItem as ItemCombo;
        if (metodo?.Id is null || metodo.Id <= 0)
        {
            mensaje = "Debe seleccionar un método de pago.";
            cboMetodoPago.Focus();
            return false;
        }

        // 3. El importe calculado debe ser mayor a cero
        if (!ValidarImporte(out var importe))
        {
            mensaje = "El importe debe ser un número decimal mayor a 0.";
            return false;
        }

        int? cuotas = null;
        // 4. Si paga con tarjeta de crédito, exige seleccionar la cantidad de cuotas
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
        // 5. Si paga con efectivo, valida que haya ingresado con cuánto abona y que cubra el total
        else if (EsEfectivo(metodo.Texto))
        {
            var textoAbona = txtAbonaCon.Text.Trim().Replace(',', '.');
            if (string.IsNullOrWhiteSpace(textoAbona))
            {
                mensaje = "Debe ingresar el importe con el que abona el cliente.";
                txtAbonaCon.Focus();
                return false;
            }

            if (!decimal.TryParse(textoAbona, CultureInfo.InvariantCulture, out var abonaCon))
            {
                mensaje = "El importe abonado en efectivo debe ser un número decimal válido.";
                txtAbonaCon.Focus();
                return false;
            }

            if (abonaCon < importe)
            {
                mensaje = $"El importe abonado en efectivo (${abonaCon:N2}) no puede ser menor al total a cobrar (${importe:N2}).";
                txtAbonaCon.Focus();
                return false;
            }
        }

        // Construye el DTO con los datos listos para persistir
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

    // Muestra u oculta controles según el método de pago elegido (efectivo vs tarjeta)
    private void ActualizarVisibilidadMetodoPago()
    {
        var metodo = cboMetodoPago.SelectedItem as ItemCombo;
        var esCredito = metodo is not null && EsTarjetaCredito(metodo.Texto);
        var esEfectivo = metodo is not null && EsEfectivo(metodo.Texto);

        // Si es crédito muestra el combo de cuotas
        lblCuotas.Visible = esCredito;
        cboCuotas.Visible = esCredito;
        if (esCredito && cboCuotas.SelectedIndex < 0 && cboCuotas.Items.Count > 0)
            cboCuotas.SelectedIndex = 0;

        // Si es efectivo muestra la calculadora de vuelto y cuánto abona
        lblAbonaCon.Visible = esEfectivo;
        pnlAbonaCon.Visible = esEfectivo;
        lblVueltoEtiqueta.Visible = esEfectivo;
        lblVuelto.Visible = esEfectivo;

        if (esEfectivo)
        {
            if (string.IsNullOrWhiteSpace(txtAbonaCon.Text) && !string.IsNullOrWhiteSpace(txtImporte.Text))
                txtAbonaCon.Text = txtImporte.Text;
            CalcularVuelto();
        }
        else
        {
            txtAbonaCon.Clear();
            lblVuelto.Text = "$ 0,00";
            lblVuelto.ForeColor = ColorTextoNormal;
        }
    }

    private static bool EsTarjetaCredito(string? nombre) =>
        !string.IsNullOrWhiteSpace(nombre) &&
        (nombre.Contains("Crédito", StringComparison.OrdinalIgnoreCase) ||
         nombre.Contains("Credito", StringComparison.OrdinalIgnoreCase));

    private static bool EsEfectivo(string? nombre) =>
        !string.IsNullOrWhiteSpace(nombre) &&
        nombre.Contains("Efectivo", StringComparison.OrdinalIgnoreCase);

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

    // Al hacer clic en un cobro de la grilla, carga sus datos en modo consulta
    private void CargarPagoSeleccionado()
    {
        // CellClick y SelectionChanged llegan juntos al hacer clic: el mismo cobro se carga una sola vez
        if (_cargando || dgvCobros.CurrentRow?.Tag is not PagoRespuestaDto pago || _idPagoSeleccionado == pago.Id)
            return;

        _cargando = true;
        try
        {
            _idPagoSeleccionado = pago.Id;
            AsegurarConsultaEnCombo(pago.IdConsulta, pago.DescripcionConsulta);
            SeleccionarComboPorId(cboConsultaClinica, pago.IdConsulta);
            SeleccionarComboPorId(cboMetodoPago, pago.IdMetodoPago);
            ActualizarVisibilidadMetodoPago();
            SeleccionarCuotas(pago.Cuotas);
            dtpFecha.Value = pago.Fecha.Date;
            txtImporte.Text = pago.Importe.ToString("0.00", CultureInfo.InvariantCulture);
            txtAbonaCon.Text = pago.Importe.ToString("0.00", CultureInfo.InvariantCulture);
            CalcularVuelto();
            _estadoPagoSeleccionado = pago.Estado;

            // En un cobro histórico, se desactiva Cobrar y solo se permite reimprimir el ticket
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

    // Configura los campos del formulario en modo solo lectura o edición
    private void EstablecerSoloLectura(bool soloLectura)
    {
        cboConsultaClinica.Enabled = !soloLectura;
        cboMetodoPago.Enabled = !soloLectura;
        cboCuotas.Enabled = !soloLectura;
        dtpFecha.Enabled = !soloLectura;
        txtImporte.ReadOnly = true; // El importe siempre es de solo lectura (calculado automáticamente)
        txtAbonaCon.ReadOnly = soloLectura;
    }

    // Restablece todos los campos a su estado inicial para cobrar una nueva consulta
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
        ActualizarVisibilidadMetodoPago();
        dtpFecha.Value = DateTime.Today;
        txtImporte.Clear();
        txtAbonaCon.Clear();
        lblVuelto.Text = "$ 0,00";
        lblVuelto.ForeColor = ColorTextoNormal;
        txtDetalleImporte.Clear();
        btnCobrar.Enabled = true;
        btnImprimirTicket.Enabled = false;
        dgvCobros.ClearSelection();
        dgvCobros.CurrentCell = null;
        RestaurarEstiloImporte();
        cboConsultaClinica.Focus();
    }

    // Asegura que una consulta histórica figure en el desplegable (ComboBox)
    // Como el desplegable solo tiene consultas sin pagar, si el usuario toca un cobro viejo en la grilla,
    // esta función la agrega temporalmente para poder mostrar sus datos sin que el sistema dé error.
    private void AsegurarConsultaEnCombo(long idConsulta, string descripcion)
    {
        // 1. Recorre los ítems actuales del desplegable
        foreach (var obj in cboConsultaClinica.Items)
        {
            // Si ya existe en la lista, no hace nada y sale
            if (obj is ItemCombo item && item.Id == idConsulta)
                return;
        }

        // 2. Si no estaba, arma el texto descriptivo
        var texto = string.IsNullOrWhiteSpace(descripcion)
            ? $"Consulta #{idConsulta}"
            : descripcion;

        // 3. La inserta en el desplegable para que pueda ser seleccionada
        cboConsultaClinica.Items.Add(new ItemCombo(idConsulta, texto));
    }

    // Busca un ítem dentro de un ComboBox por su ID numérico y lo deja seleccionado
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

    // Filtra el teclado en tiempo real: solo permite ingresar dígitos numéricos y una única coma o punto decimal
    private void TxtAbonaCon_KeyPress(object? sender, KeyPressEventArgs e)
    {
        // Permite teclas de control (BackSpace, Delete) y dígitos
        if (char.IsControl(e.KeyChar) || char.IsDigit(e.KeyChar))
            return;

        // Permite un solo punto o coma como separador decimal
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

        // Bloquea cualquier otro carácter (letras, símbolos)
        e.Handled = true;
    }

    // Se dispara mientras el usuario tipea para sanitizar pegados y recalcular el vuelto en vivo
    private void TxtAbonaCon_TextChanged(object? sender, EventArgs e)
    {
        if (string.IsNullOrEmpty(txtAbonaCon.Text))
        {
            lblVuelto.Text = "$ 0,00";
            lblVuelto.ForeColor = ColorTextoNormal;
            return;
        }

        var texto = txtAbonaCon.Text;
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

        // Si se pegaron caracteres prohibidos, los limpia conservando la posición del cursor
        if (textoLimpio.ToString() != texto)
        {
            var cursor = txtAbonaCon.SelectionStart;
            txtAbonaCon.Text = textoLimpio.ToString();
            txtAbonaCon.SelectionStart = Math.Min(cursor, txtAbonaCon.Text.Length);
        }

        // Actualiza el cálculo del vuelto en la pantalla
        CalcularVuelto();
    }

    // Al salir del campo (Leave), formatea el número a dos decimales estándar (ej: "1500.00")
    private void TxtAbonaCon_Leave(object? sender, EventArgs e)
    {
        var texto = txtAbonaCon.Text.Trim().Replace(',', '.');
        if (decimal.TryParse(texto, CultureInfo.InvariantCulture, out var valor) && valor >= 0)
        {
            txtAbonaCon.Text = valor.ToString("0.00", CultureInfo.InvariantCulture);
            CalcularVuelto();
        }
    }

    // Calcula el vuelto en tiempo real cuando se cobra en efectivo
    // Si el cliente entrega más dinero, muestra el vuelto en color verde
    // Si entrega menos de lo debido, muestra en rojo cuánto dinero falta
    private void CalcularVuelto()
    {
        var metodo = cboMetodoPago.SelectedItem as ItemCombo;
        if (metodo is null || !EsEfectivo(metodo.Texto))
            return;

        var textoImporte = txtImporte.Text.Trim().Replace(',', '.');
        var textoAbona = txtAbonaCon.Text.Trim().Replace(',', '.');

        decimal.TryParse(textoImporte, CultureInfo.InvariantCulture, out var importe);
        decimal.TryParse(textoAbona, CultureInfo.InvariantCulture, out var abona);

        if (abona <= 0 || importe <= 0)
        {
            lblVuelto.ForeColor = ColorTextoNormal;
            lblVuelto.Text = "$ 0,00";
            return;
        }

        // Si el dinero entregado alcanza para cubrir el total
        if (abona >= importe)
        {
            var vuelto = abona - importe;
            lblVuelto.ForeColor = ColorExito; // Verde
            lblVuelto.Text = $"$ {vuelto:N2}";
        }
        // Si el dinero entregado no es suficiente
        else
        {
            var falta = importe - abona;
            lblVuelto.ForeColor = ColorEtiquetaError; // Rojo
            lblVuelto.Text = $"Faltan $ {falta:N2}";
        }
    }

    // Valida que el importe calculado sea un decimal positivo válido
    private bool ValidarImporte(out decimal importe)
    {
        importe = 0m;
        var texto = txtImporte.Text.Trim().Replace(',', '.');
        if (string.IsNullOrWhiteSpace(texto))
        {
            MarcarErrorImporte("Debe seleccionar una consulta con importe a cobrar.");
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
        lblInfoEstado.ForeColor = ColorEtiquetaError;
        lblInfoEstado.Text = mensaje;
    }

    private void RestaurarEstiloImporte()
    {
        lblImporte.ForeColor = ColorTextoNormal;
        lblInfoEstado.ForeColor = ColorTextoNormal;
        if (_idPagoSeleccionado is null)
            lblInfoEstado.Text = "Módulo de cobros --- recepción";
    }

    private void btnCancelar_Click(object? sender, EventArgs e)
    {
        Close();
    }
}
