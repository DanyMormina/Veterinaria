using System.Globalization;
using Microsoft.Extensions.DependencyInjection;
using Veterinaria.Controllers.Controladores;
using Veterinaria.Domain.Dtos;
using Veterinaria.WinForms.Sesion;

namespace Veterinaria.WinForms.Vistas.Veterinario;
// Formulario para la gestión y registro de consultas clínicas veterinarias.
// Incorpora restricciones en tiempo real, validaciones con feedback visual
// y sincronización inteligente entre tutores y pacientes.
public partial class FormConsultas : Form
{
    private readonly ConsultaControlador _consultaControlador;
    private readonly MascotaControlador _mascotaControlador;
    private readonly PropietarioControlador _propietarioControlador;
    private readonly IServiceProvider _serviceProvider;

    // Estado interno del formulario
    private long? _idConsultaSeleccionada = null;
    private DateTime? _fechaHoraConsultaSeleccionada = null;
    private bool _sincronizandoCombos = false;
    private string? _snapshotEdicion;
    private List<PropietarioRespuestaDto> _propietarios = [];
    private List<MascotaRespuestaDto> _mascotas = [];
    private List<ConsultaRespuestaDto> _consultas = [];

    private readonly Dictionary<Control, Label> _mapaEtiquetas = [];

    public FormConsultas(
        ConsultaControlador consultaControlador,
        MascotaControlador mascotaControlador,
        PropietarioControlador propietarioControlador,
        IServiceProvider serviceProvider)
    {
        InitializeComponent();
        Controles.ComboBuscable.Habilitar(cboPropietario, cboMascota);
        _consultaControlador = consultaControlador;
        _mascotaControlador = mascotaControlador;
        _propietarioControlador = propietarioControlador;
        _serviceProvider = serviceProvider;
    }
    // Inicializa la sesión del profesional, enlaza eventos de validación y carga catálogos clínicos.
    private async void FormConsultas_Load(object? sender, EventArgs e)
    {
        lblUsuarioSesion.Text = SesionActual.EstaAutenticado
            ? $"Dr./Dra. {SesionActual.NombreCompleto} | {SesionActual.Rol}"
            : "Dr./Dra. Lucía Pérez | Veterinario";

        lblInfoEstado.Text = $"Sesión clínica activa: {SesionActual.NombreUsuario} - {DateTime.Now:dd/MM/yyyy}";

        // Mapeo de controles con sus etiquetas para feedback visual
        _mapaEtiquetas[cboMascota] = lblMascota;
        _mapaEtiquetas[cboPropietario] = lblPropietario;
        _mapaEtiquetas[txtMotivo] = lblMotivo;
        _mapaEtiquetas[txtPeso] = lblPeso;
        _mapaEtiquetas[txtTemperatura] = lblTemperatura;
        _mapaEtiquetas[txtDiagnostico] = lblDiagnostico;
        _mapaEtiquetas[txtObservaciones] = lblObservaciones;

        ConfigurarRestriccionesTeclado();
        ConfigurarLimpiezaErroresEnInteraccion();

        dtpProximoControl.Value = DateTime.Today.AddDays(15);

        btnGuardar.Enabled = true;
        btnModificar.Enabled = false;

        // Carga asíncrona inicial
        await CargarPropietariosAsync();
        await CargarMascotasAsync();
        await CargarHistorialConsultasAsync();
        LimpiarFormulario();

        // Enlazar eventos de sincronización entre propietario y mascota
        cboPropietario.SelectedIndexChanged += cboPropietario_SelectedIndexChanged;
        cboMascota.SelectedIndexChanged += cboMascota_SelectedIndexChanged;
    }
    // Aplica filtros de teclado estrictos para campos numéricos, de texto clínico y selectores de búsqueda.
    private void ConfigurarRestriccionesTeclado()
    {
        // Peso: 0,10–100,00 kg. Temperatura: 36,0–41,0 °C. No se permite tipear fuera de rango.
        txtPeso.MaxLength = 6;
        txtTemperatura.MaxLength = 4;
        txtPeso.KeyPress += (s, e) => ValidarDecimalLimitado_KeyPress(s, e, 3, 2);
        txtTemperatura.KeyPress += (s, e) => ValidarDecimalLimitado_KeyPress(s, e, 2, 1);
        txtPeso.TextChanged += TxtPeso_TextChanged;
        txtTemperatura.TextChanged += TxtTemperatura_TextChanged;
        txtPeso.Leave += (_, _) => CompletarOLimpiarPeso();
        txtTemperatura.Leave += (_, _) => CompletarOLimpiarTemperatura();
        txtPeso.Enter += (_, _) => RestablecerAyudaPeso();
        txtTemperatura.Enter += (_, _) => RestablecerAyudaTemperatura();
        txtMotivo.KeyPress += ValidarTextoGeneral_KeyPress;
        cboMascota.KeyPress += ValidarSoloLetras_KeyPress;
        cboPropietario.KeyPress += ValidarSoloLetras_KeyPress;
    }

    private const decimal PesoMinimoKg = 0.1m;
    private const decimal PesoMaximoKg = 100m;
    private const decimal TemperaturaMinimaC = 36.0m;
    private const decimal TemperaturaMaximaC = 41.0m;

    private string _textoPesoAnterior = string.Empty;
    private string _textoTemperaturaAnterior = string.Empty;
    private bool _ajustandoPeso;
    private bool _ajustandoTemperatura;

    private void TxtPeso_TextChanged(object? sender, EventArgs e)
    {
        if (_ajustandoPeso)
            return;

        var texto = txtPeso.Text;
        if (EsEntradaPesoPermitida(texto))
        {
            _textoPesoAnterior = texto;
            RestablecerAyudaPeso();
            return;
        }

        _ajustandoPeso = true;
        txtPeso.Text = _textoPesoAnterior;
        txtPeso.SelectionStart = txtPeso.Text.Length;
        _ajustandoPeso = false;
        lblAyudaPeso.Text = "¡Máximo permitido: 100,00 kg!";
        lblAyudaPeso.Font = new Font("Segoe UI", 7.5F, FontStyle.Bold);
    }

    private void TxtTemperatura_TextChanged(object? sender, EventArgs e)
    {
        if (_ajustandoTemperatura)
            return;

        var texto = txtTemperatura.Text;
        if (EsEntradaTemperaturaPermitida(texto))
        {
            _textoTemperaturaAnterior = texto;
            RestablecerAyudaTemperatura();
            return;
        }

        _ajustandoTemperatura = true;
        txtTemperatura.Text = _textoTemperaturaAnterior;
        txtTemperatura.SelectionStart = txtTemperatura.Text.Length;
        _ajustandoTemperatura = false;
        lblAyudaTemperatura.Text = "¡Rango: 36,0 a 41,0 °C!";
        lblAyudaTemperatura.Font = new Font("Segoe UI", 7.5F, FontStyle.Bold);
    }

    private void CompletarOLimpiarPeso()
    {
        if (string.IsNullOrWhiteSpace(txtPeso.Text))
        {
            _textoPesoAnterior = string.Empty;
            RestablecerAyudaPeso();
            return;
        }

        if (!TryParsePeso(txtPeso.Text, out _))
        {
            _ajustandoPeso = true;
            txtPeso.Clear();
            _textoPesoAnterior = string.Empty;
            _ajustandoPeso = false;
            lblAyudaPeso.Text = "¡Borrado: no alcanzaba 0,10 kg!";
            lblAyudaPeso.Font = new Font("Segoe UI", 7.5F, FontStyle.Bold);
        }
        else
        {
            RestablecerAyudaPeso();
        }
    }

    private void CompletarOLimpiarTemperatura()
    {
        if (string.IsNullOrWhiteSpace(txtTemperatura.Text))
        {
            _textoTemperaturaAnterior = string.Empty;
            RestablecerAyudaTemperatura();
            return;
        }

        if (!TryParseTemperatura(txtTemperatura.Text, out _))
        {
            _ajustandoTemperatura = true;
            txtTemperatura.Clear();
            _textoTemperaturaAnterior = string.Empty;
            _ajustandoTemperatura = false;
            lblAyudaTemperatura.Text = "¡Borrado: no alcanzaba 36,0 °C!";
            lblAyudaTemperatura.Font = new Font("Segoe UI", 7.5F, FontStyle.Bold);
        }
        else
        {
            RestablecerAyudaTemperatura();
        }
    }

    private void RestablecerAyudaPeso()
    {
        lblAyudaPeso.Text = "Rango: 0,10 a 100,00 kg";
        lblAyudaPeso.Font = new Font("Segoe UI", 7.5F, FontStyle.Regular);
        lblAyudaPeso.ForeColor = Color.FromArgb(184, 93, 105);
    }

    private void RestablecerAyudaTemperatura()
    {
        lblAyudaTemperatura.Text = "Rango: 36,0 a 41,0 °C";
        lblAyudaTemperatura.Font = new Font("Segoe UI", 7.5F, FontStyle.Regular);
        lblAyudaTemperatura.ForeColor = Color.FromArgb(184, 93, 105);
    }

    private static bool EsEntradaPesoPermitida(string texto)
    {
        if (string.IsNullOrWhiteSpace(texto))
            return true;

        var limpio = texto.Trim().Replace(',', '.');
        if (limpio is ".")
            return true;
        if (limpio.EndsWith('.'))
        {
            var parte = limpio[..^1];
            return parte.Length == 0
                   || (decimal.TryParse(parte, NumberStyles.Any, CultureInfo.InvariantCulture, out var prefijo)
                       && prefijo <= PesoMaximoKg);
        }

        if (!decimal.TryParse(limpio, NumberStyles.Any, CultureInfo.InvariantCulture, out var valor))
            return false;

        if (valor > PesoMaximoKg)
            return false;

        if (valor >= PesoMinimoKg)
            return true;

        // Prefijos válidos hacia 0,1 (0 / 0,0)
        return limpio.StartsWith('0');
    }

    private static bool EsEntradaTemperaturaPermitida(string texto)
    {
        if (string.IsNullOrWhiteSpace(texto))
            return true;

        var limpio = texto.Trim().Replace(',', '.');
        if (limpio is ".")
            return true;
        if (limpio.EndsWith('.'))
        {
            var parte = limpio[..^1];
            return parte.Length == 0
                   || (decimal.TryParse(parte, NumberStyles.Any, CultureInfo.InvariantCulture, out var prefijo)
                       && prefijo <= TemperaturaMaximaC
                       && (prefijo >= TemperaturaMinimaC || parte is "3" or "4"));
        }

        if (!decimal.TryParse(limpio, NumberStyles.Any, CultureInfo.InvariantCulture, out var valor))
            return false;

        if (valor > TemperaturaMaximaC)
            return false;

        if (valor >= TemperaturaMinimaC)
            return true;

        // Solo permitir el primer dígito 3 o 4 mientras se escribe 36–41
        var enteros = limpio.Split('.')[0];
        return enteros is "3" or "4";
    }

    private static bool TryParsePeso(string texto, out decimal peso)
    {
        peso = 0;
        var limpio = texto.Trim().Replace(',', '.');
        return decimal.TryParse(limpio, NumberStyles.Any, CultureInfo.InvariantCulture, out peso)
               && peso >= PesoMinimoKg
               && peso <= PesoMaximoKg;
    }

    private static bool TryParseTemperatura(string texto, out decimal temperatura)
    {
        temperatura = 0;
        var limpio = texto.Trim().Replace(',', '.');
        return decimal.TryParse(limpio, NumberStyles.Any, CultureInfo.InvariantCulture, out temperatura)
               && temperatura >= TemperaturaMinimaC
               && temperatura <= TemperaturaMaximaC;
    }
    // Restablece el color de las cajas de texto y etiquetas al interactuar con ellas.
    private void ConfigurarLimpiezaErroresEnInteraccion()
    {
        foreach (var (control, _) in _mapaEtiquetas)
        {
            control.Enter += Control_LimpiarError;

            if (control is TextBox tb)
            {
                tb.TextChanged += Control_LimpiarError;
            }
            else if (control is ComboBox cb)
            {
                cb.SelectedIndexChanged += Control_LimpiarError;
                cb.TextChanged += Control_LimpiarError;
            }
        }
    }
    // Restringe el ingreso en cajas de texto decimal a dígitos y un único separador.
    private void ValidarDecimal_KeyPress(object? sender, KeyPressEventArgs e)
    {
        if (char.IsControl(e.KeyChar))
            return;

        if (char.IsDigit(e.KeyChar))
            return;

        // Permitir coma o punto decimal una sola vez
        if ((e.KeyChar == '.' || e.KeyChar == ',') && sender is TextBox tb)
        {
            if (!tb.Text.Contains('.') && !tb.Text.Contains(','))
                return;
        }

        e.Handled = true;
    }
    // Igual que <see cref="ValidarDecimal_KeyPress"/>, pero limita la cantidad de dígitos enteros y decimales.
    private void ValidarDecimalLimitado_KeyPress(object? sender, KeyPressEventArgs e, int maxEnteros, int maxDecimales)
    {
        ValidarDecimal_KeyPress(sender, e);
        if (e.Handled || char.IsControl(e.KeyChar) || sender is not TextBox tb)
            return;

        var resultado = tb.Text.Remove(tb.SelectionStart, tb.SelectionLength)
            .Insert(tb.SelectionStart, e.KeyChar.ToString());
        var partes = resultado.Split('.', ',');
        if (partes[0].Length > maxEnteros || (partes.Length > 1 && partes[1].Length > maxDecimales))
            e.Handled = true;
    }
    // Permite caracteres alfabéticos, numéricos, signos de puntuación estándar y previene espacios dobles.
    private void ValidarTextoGeneral_KeyPress(object? sender, KeyPressEventArgs e)
    {
        if (char.IsControl(e.KeyChar))
            return;

        // Bloquear espacios consecutivos
        if (e.KeyChar == ' ' && sender is TextBox tb && (tb.SelectionStart == 0 || (tb.Text.Length > 0 && tb.Text[tb.SelectionStart - 1] == ' ')))
        {
            e.Handled = true;
            return;
        }

        // Caracteres válidos para descripciones clínicas
        if (char.IsLetterOrDigit(e.KeyChar) || e.KeyChar == ' ' || "áéíóúÁÉÍÓÚñÑüÜ,.;:-/()¿?¡!".Contains(e.KeyChar))
            return;

        e.Handled = true;
    }
    // Restringe la búsqueda en los desplegables de mascota y propietario exclusivamente a letras y espacios, bloqueando números y caracteres especiales.
    private void ValidarSoloLetras_KeyPress(object? sender, KeyPressEventArgs e)
    {
        // 1. Permitir teclas de control del sistema (retroceso, cortar/copiar, etc.)
        if (char.IsControl(e.KeyChar))
            return;

        // 2. Permitir exclusivamente caracteres alfabéticos (incluye vocales acentuadas áéíóú, ñ y diéresis)
        if (char.IsLetter(e.KeyChar))
            return;

        // 3. Permitir espacio simple si no es al inicio y no es consecutivo
        if (e.KeyChar == ' ' && sender is ComboBox cb)
        {
            if (cb.SelectionStart > 0 && cb.Text.Length > 0 && cb.Text[cb.SelectionStart - 1] != ' ')
                return;
        }

        // 4. Bloquear cualquier número (0-9), símbolo o carácter especial no alfabético
        e.Handled = true;
    }
    // Carga la lista completa de propietarios activos en el selector desplegable (ComboBox)
    private async Task CargarPropietariosAsync()
    {
        try
        {
            // 1. Consulta al controlador de propietarios de forma asíncrona en SQL Server
            var res = await _propietarioControlador.ObtenerTodosAsync();
            var lista = new List<PropietarioRespuestaDto>();

            // 2. Filtra solo los propietarios activos
            if (res.EsExitoso && res.Valor != null)
            {
                lista.AddRange(res.Valor.Where(p => p.Activo));
            }

            // 3. Guarda la lista en memoria y la enlaza al ComboBox
            _propietarios = lista;
            _sincronizandoCombos = true; // Evita disparar el evento SelectedIndexChanged durante la carga inicial
            cboPropietario.DisplayMember = "NombreCompleto"; // Texto visible en la lista (ej: "Juan Pérez")
            cboPropietario.ValueMember = "Id";               // Valor interno del ID
            cboPropietario.DataSource = _propietarios;       // Asigna la fuente de datos
            cboPropietario.SelectedIndex = -1;              // Deja el desplegable sin selección inicial
            cboPropietario.Text = string.Empty;
            _sincronizandoCombos = false;
        }
        catch (Exception ex)
        {
            lblInfoEstado.Text = $"Error al cargar propietarios: {ex.Message}";
        }
    }

    // Carga la lista completa de mascotas activas desde la base de datos a memoria
    private async Task CargarMascotasAsync()
    {
        try
        {
            // 1. Consulta al controlador de mascotas en SQL Server
            var res = await _mascotaControlador.ObtenerTodosAsync();
            var lista = new List<MascotaRespuestaDto>();

            // 2. Filtra solo las mascotas activas
            if (res.EsExitoso && res.Valor != null)
            {
                lista.AddRange(res.Valor.Where(m => m.Activo));
            }

            // 3. Guarda las mascotas en memoria en la lista _mascotas
            _mascotas = lista;
            // Si ya había un dueño seleccionado, filtra sus mascotas; si no, deja el combo deshabilitado
            CargarMascotasDePropietario(cboPropietario.SelectedValue is long idProp && idProp > 0 ? idProp : null);
        }
        catch (Exception ex)
        {
            lblInfoEstado.Text = $"Error al cargar mascotas: {ex.Message}";
        }
    }

    // Puebla la grilla inferior con el historial de consultas clínicas registradas
    private async Task CargarHistorialConsultasAsync()
    {
        try
        {
            // 1. Consulta todas las atenciones médicas registradas
            var res = await _consultaControlador.ObtenerTodosAsync();
            dgvConsultas.Rows.Clear();

            if (!res.EsExitoso || res.Valor == null)
                return;

            // 2. Ordena de la más reciente a la más antigua
            _consultas = res.Valor.OrderByDescending(c => c.FechaHora).ToList();
            foreach (var c in _consultas)
            {
                int idx = dgvConsultas.Rows.Add(
                    c.Id,
                    c.FechaHora.ToString("dd/MM/yyyy HH:mm"),
                    c.NombreMascota,
                    c.NombrePropietario,
                    c.Motivo ?? "-",
                    c.Diagnostico,
                    c.ProximoControl?.ToString("dd/MM/yyyy") ?? "-"
                );
                dgvConsultas.Rows[idx].Tag = c; // Guarda el DTO completo en el Tag para fácil acceso al hacer clic
            }
            dgvConsultas.ClearSelection();
            dgvConsultas.CurrentCell = null;
        }
        catch (Exception ex)
        {
            lblInfoEstado.Text = $"Error al cargar historial: {ex.Message}";
        }
    }

    // Se dispara cuando el veterinario elige un propietario en el ComboBox
    // Filtra las mascotas para que solo aparezcan las de ese dueño
    private void cboPropietario_SelectedIndexChanged(object? sender, EventArgs e)
    {
        if (_sincronizandoCombos)
            return;

        if (!_idConsultaSeleccionada.HasValue)
            txtImporte.Clear();

        // Si se eligió un dueño válido, carga solo sus mascotas
        if (cboPropietario.SelectedValue is long idProp && idProp > 0)
            CargarMascotasDePropietario(idProp);
        else if (cboPropietario.SelectedIndex == -1)
            CargarMascotasDePropietario(null);
    }

    // Filtra las mascotas en memoria con LINQ según el IdPropietario
    private void CargarMascotasDePropietario(long? idPropietario, long? idMascotaSeleccionada = null)
    {
        _sincronizandoCombos = true;
        cboMascota.DataSource = null;
        cboMascota.DisplayMember = "Nombre"; // Muestra el nombre del animal (ej: "Uma")
        cboMascota.ValueMember = "Id";       // Guarda el ID de la mascota

        // Filtra en memoria: solo mascotas donde IdPropietario coincide con el dueño elegido
        cboMascota.DataSource = idPropietario.HasValue
            ? _mascotas.Where(m => m.IdPropietario == idPropietario.Value).ToList()
            : new List<MascotaRespuestaDto>();

        if (idMascotaSeleccionada.HasValue)
        {
            cboMascota.SelectedValue = idMascotaSeleccionada.Value;
        }
        else
        {
            cboMascota.SelectedIndex = -1;
            cboMascota.Text = string.Empty;
        }

        // El combo de mascotas solo se habilita si hay un dueño seleccionado
        cboMascota.Enabled = idPropietario.HasValue;
        _sincronizandoCombos = false;
    }

    // Sincronización inversa: si se elige una mascota, autoselecciona a su dueño en el combo de propietarios
    private async void cboMascota_SelectedIndexChanged(object? sender, EventArgs e)
    {
        if (_sincronizandoCombos)
            return;

        if (cboMascota.SelectedValue is long idMascota && idMascota > 0)
        {
            // Busca la mascota en la lista para saber quién es su tutor
            var mascota = _mascotas.FirstOrDefault(m => m.Id == idMascota);
            if (mascota != null && mascota.IdPropietario > 0)
            {
                _sincronizandoCombos = true;
                // Autoselecciona al dueño en cboPropietario
                cboPropietario.SelectedValue = mascota.IdPropietario;
                var mascotasFiltradas = _mascotas.Where(m => m.IdPropietario == mascota.IdPropietario).ToList();
                cboMascota.DataSource = null;
                cboMascota.DisplayMember = "Nombre";
                cboMascota.ValueMember = "Id";
                cboMascota.DataSource = mascotasFiltradas;
                cboMascota.SelectedValue = idMascota;
                _sincronizandoCombos = false;
            }

            // Consulta y muestra el precio base de la consulta para esta mascota
            var consultaActual = _consultas.FirstOrDefault(c => c.Id == _idConsultaSeleccionada);
            if (consultaActual is null || consultaActual.IdMascota != idMascota)
                await MostrarPrecioConsultaAsync(idMascota);
            else
                txtImporte.Text = consultaActual.Importe.ToString("0.00", CultureInfo.InvariantCulture);
        }
        else
        {
            txtImporte.Clear();
        }
    }

    // Consulta el valor fijado para la consulta según la especie del animal y lo muestra en txtImporte
    private async Task MostrarPrecioConsultaAsync(long idMascota)
    {
        var res = await _consultaControlador.ObtenerPrecioConsultaAsync(idMascota);
        if (res.EsExitoso)
        {
            // Muestra el importe formateado con dos decimales en modo de solo lectura
            txtImporte.Text = res.Valor.ToString("0.00", CultureInfo.InvariantCulture);
        }
        else
        {
            txtImporte.Clear();
            lblInfoEstado.Text = res.Mensaje;
        }
    }
    // Valida la integridad y consistencia de los datos ingresados en el formulario.
    private bool ValidarCampos(out ConsultaSolicitudDto? solicitud)
    {
        solicitud = null;

        if (cboPropietario.SelectedItem is not PropietarioRespuestaDto propSeleccionado || propSeleccionado.Id <= 0)
        {
            MarcarControlConError(cboPropietario, "Debe seleccionar primero el propietario responsable de la mascota.");
            return false;
        }

        if (cboMascota.SelectedItem is not MascotaRespuestaDto mascotaSeleccionada || mascotaSeleccionada.Id <= 0)
        {
            MarcarControlConError(cboMascota, "Debe seleccionar una mascota para registrar la consulta clínica.");
            return false;
        }
        long idMascota = mascotaSeleccionada.Id;

        var motivo = txtMotivo.Text.Trim();
        if (string.IsNullOrWhiteSpace(motivo) || motivo.Length < 3)
        {
            MarcarControlConError(txtMotivo, "El motivo de la consulta es obligatorio y debe contener al menos 3 caracteres.");
            return false;
        }

        var diagnostico = txtDiagnostico.Text.Trim();
        if (string.IsNullOrWhiteSpace(diagnostico) || diagnostico.Length < 5)
        {
            MarcarControlConError(txtDiagnostico, "El diagnóstico médico es obligatorio y debe contener al menos 5 caracteres.");
            return false;
        }

        decimal? peso = null;
        if (!string.IsNullOrWhiteSpace(txtPeso.Text))
        {
            if (!TryParsePeso(txtPeso.Text, out var p))
            {
                MarcarControlConError(txtPeso, "El peso ingresado no es válido.");
                return false;
            }
            peso = p;
        }

        decimal? temp = null;
        if (!string.IsNullOrWhiteSpace(txtTemperatura.Text))
        {
            if (!TryParseTemperatura(txtTemperatura.Text, out var t))
            {
                MarcarControlConError(txtTemperatura, "La temperatura ingresada no es válida.");
                return false;
            }
            temp = t;
        }

        // El precio fijo solo se exige al crear la consulta o al cambiarle la mascota;
        // las consultas existentes conservan el importe con el que se registraron.
        var consultaOriginal = _consultas.FirstOrDefault(c => c.Id == _idConsultaSeleccionada);
        var requierePrecio = consultaOriginal is null || consultaOriginal.IdMascota != idMascota;

        var textoImporte = txtImporte.Text.Trim().Replace(',', '.');
        decimal.TryParse(textoImporte, NumberStyles.Any, CultureInfo.InvariantCulture, out var importe);
        if (requierePrecio && importe <= 0)
        {
            MarcarControlConError(cboMascota, "No hay un precio de consulta cargado para la especie de esta mascota.");
            return false;
        }

        // Si es modificación, preserva la fecha y hora original; al crear una nueva consulta, se registra automáticamente
        DateTime fechaHora = _idConsultaSeleccionada.HasValue && _fechaHoraConsultaSeleccionada.HasValue
            ? _fechaHoraConsultaSeleccionada.Value
            : DateTime.Now;

        long idVeterinario = SesionActual.EstaAutenticado ? SesionActual.IdUsuario!.Value : 2;

        solicitud = new ConsultaSolicitudDto
        {
            IdUsuario = idVeterinario,
            IdMascota = idMascota,
            FechaHora = fechaHora,
            Motivo = motivo,
            PesoKg = peso,
            Temperatura = temp,
            Diagnostico = diagnostico,
            Observaciones = string.IsNullOrWhiteSpace(txtObservaciones.Text) ? null : txtObservaciones.Text.Trim(),
            ProximoControl = dtpProximoControl.Value.Date,
            Importe = decimal.Round(importe, 2)
        };

        return true;
    }
    // Resalta el control defectuoso con el color Borgoña suave del sistema de diseño.
    private void MarcarControlConError(Control control, string mensaje)
    {
        control.BackColor = Color.FromArgb(253, 236, 239); // Fondo #FDECEF
        if (_mapaEtiquetas.TryGetValue(control, out var lbl))
        {
            lbl.ForeColor = Color.FromArgb(184, 93, 105); // Borgoña suave #B85D69
        }
        control.Focus();
        MessageBox.Show(mensaje, "Validación de Consulta", MessageBoxButtons.OK, MessageBoxIcon.Warning);
    }
    // Limpia el estado de error visual del control que recibe interacción.
    private void Control_LimpiarError(object? sender, EventArgs e)
    {
        if (sender is Control c)
        {
            c.BackColor = Color.White;
            if (_mapaEtiquetas.TryGetValue(c, out var lbl))
            {
                lbl.ForeColor = Color.FromArgb(58, 53, 59); // Gris pizarra carbón #3A353B
            }
        }
    }
    // Restaura todos los controles del formulario a sus colores neutrales.
    private void LimpiarTodosLosErroresVisuales()
    {
        foreach (var (control, label) in _mapaEtiquetas)
        {
            control.BackColor = Color.White;
            label.ForeColor = Color.FromArgb(58, 53, 59);
        }
    }
    // Registra una nueva consulta clínica en la base de datos tras validar los campos
    private async void btnGuardar_Click(object? sender, EventArgs e)
    {
        // 1. Valida campos obligatorios, signos vitales y vigencia de fechas
        if (!ValidarCampos(out var solicitud) || solicitud is null)
            return;

        try
        {
            // 2. Bloquea el botón Guardar y activa el cursor de espera para prevenir doble clic
            btnGuardar.Enabled = false;
            Cursor = Cursors.WaitCursor;

            // 3. Envía el DTO al controlador para insertarlo en SQL Server de forma asíncrona
            var resultado = await _consultaControlador.CrearAsync(solicitud);
            if (!resultado.EsExitoso)
            {
                MessageBox.Show(resultado.Mensaje, "Error al registrar consulta", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // 4. Si se guardó con éxito, notifica, limpia los campos y actualiza la grilla
            MessageBox.Show("La consulta clínica fue registrada exitosamente.", "Consulta Registrada", MessageBoxButtons.OK, MessageBoxIcon.Information);
            LimpiarFormulario();
            await CargarHistorialConsultasAsync();
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Ocurrió un error inesperado al guardar la consulta: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        // El bloque finally se ejecuta SIEMPRE: vuelve a habilitar el botón y restablece el mouse
        finally
        {
            btnGuardar.Enabled = true;
            Cursor = Cursors.Default;
        }
    }

    // Actualiza la consulta clínica seleccionada en el historial
    private async void btnModificar_Click(object? sender, EventArgs e)
    {
        // 1. Verifica que haya una consulta seleccionada de la grilla
        if (!_idConsultaSeleccionada.HasValue)
        {
            MessageBox.Show("Debe seleccionar una consulta del historial para poder modificarla.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        // 2. Valida los nuevos datos ingresados
        if (!ValidarCampos(out var solicitud) || solicitud is null)
            return;

        // 3. Snapshot: compara con la foto inicial; si el usuario no tocó nada, cancela para no molestar a la BD
        if (string.Equals(ObtenerSnapshotFormulario(), _snapshotEdicion, StringComparison.Ordinal))
        {
            MessageBox.Show("No se detectaron cambios para guardar.", "Sin cambios", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        try
        {
            // 4. Bloquea el botón Modificar mientras procesa
            btnModificar.Enabled = false;
            Cursor = Cursors.WaitCursor;

            // 5. Envía la actualización al controlador
            var resultado = await _consultaControlador.ActualizarAsync(_idConsultaSeleccionada.Value, solicitud);
            if (!resultado.EsExitoso)
            {
                MessageBox.Show(resultado.Mensaje, "Error al actualizar consulta", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // 6. Notifica éxito, limpia y recarga la grilla
            MessageBox.Show("La consulta clínica fue modificada exitosamente.", "Consulta Actualizada", MessageBoxButtons.OK, MessageBoxIcon.Information);
            LimpiarFormulario();
            await CargarHistorialConsultasAsync();
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Ocurrió un error inesperado al modificar la consulta: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        finally
        {
            btnModificar.Enabled = _idConsultaSeleccionada.HasValue;
            Cursor = Cursors.Default;
        }
    }
    // Carga los datos de la consulta seleccionada en la grilla para su revisión o modificación.
    private async void dgvConsultas_CellClick(object? sender, DataGridViewCellEventArgs e)
    {
        if (e.RowIndex < 0 || e.RowIndex >= dgvConsultas.Rows.Count)
            return;

        var fila = dgvConsultas.Rows[e.RowIndex];
        if (fila.Tag is not ConsultaRespuestaDto consulta)
            return;

        _idConsultaSeleccionada = consulta.Id;
        _fechaHoraConsultaSeleccionada = consulta.FechaHora;
        LimpiarTodosLosErroresVisuales();

        var mascota = _mascotas.FirstOrDefault(m => m.Id == consulta.IdMascota);
        if (mascota != null)
        {
            _sincronizandoCombos = true;
            cboPropietario.SelectedValue = mascota.IdPropietario;
            _sincronizandoCombos = false;
            CargarMascotasDePropietario(mascota.IdPropietario, consulta.IdMascota);
        }

        txtMotivo.Text = consulta.Motivo ?? string.Empty;
        txtPeso.Text = consulta.PesoKg.HasValue ? consulta.PesoKg.Value.ToString("0.00", CultureInfo.InvariantCulture) : string.Empty;
        txtTemperatura.Text = consulta.Temperatura.HasValue ? consulta.Temperatura.Value.ToString("0.0", CultureInfo.InvariantCulture) : string.Empty;
        txtImporte.Text = consulta.Importe > 0 ? consulta.Importe.ToString("0.00", CultureInfo.InvariantCulture) : string.Empty;
        txtDiagnostico.Text = consulta.Diagnostico;
        txtObservaciones.Text = consulta.Observaciones ?? string.Empty;
        dtpProximoControl.Value = consulta.ProximoControl?.Date ?? DateTime.Today.AddDays(15);

        btnGuardar.Enabled = false;
        btnModificar.Enabled = true;
        _snapshotEdicion = ObtenerSnapshotFormulario();

        var detalleImporte = await ObtenerDetalleImporteConsultaAsync(consulta.Id);
        lblInfoEstado.Text = $"Consulta N° {consulta.Id} ({consulta.FechaHora:dd/MM/yyyy HH:mm}) | {detalleImporte}";
    }

    private string ObtenerSnapshotFormulario()
    {
        var idMascota = cboMascota.SelectedValue is long id ? id : 0L;
        return string.Join("|",
            idMascota,
            txtMotivo.Text.Trim(),
            txtPeso.Text.Trim(),
            txtTemperatura.Text.Trim(),
            txtImporte.Text.Trim(),
            txtDiagnostico.Text.Trim(),
            txtObservaciones.Text.Trim(),
            dtpProximoControl.Value.Date.ToString("yyyy-MM-dd"));
    }

    private async Task<string> ObtenerDetalleImporteConsultaAsync(long idConsulta)
    {
        var sb = new System.Text.StringBuilder();
        var total = 0m;

        var consulta = _consultas.FirstOrDefault(c => c.Id == idConsulta);
        if (consulta is not null && consulta.Importe > 0)
        {
            sb.Append($"[Consulta ${consulta.Importe:N2}] ");
            total += consulta.Importe;
        }

        var detalleCtrl = _serviceProvider.GetService<DetalleConsultaControlador>();
        if (detalleCtrl is not null)
        {
            var resDet = await detalleCtrl.ObtenerPorConsultaAsync(idConsulta);
            if (resDet.EsExitoso && resDet.Valor is not null)
            {
                foreach (var d in resDet.Valor)
                {
                    sb.Append($"[Trat. {d.DescripcionTratamiento} ${d.Subtotal:N2}] ");
                    total += d.Subtotal;
                }
            }
        }

        var vacCtrl = _serviceProvider.GetService<AplicacionVacunaControlador>();
        if (vacCtrl is not null)
        {
            var resVac = await vacCtrl.ObtenerPorConsultaAsync(idConsulta);
            if (resVac.EsExitoso && resVac.Valor is not null)
            {
                foreach (var v in resVac.Valor)
                {
                    var precio = v.PrecioUnitario ?? 0m;
                    sb.Append($"[Vac. {v.NombreVacuna} ${precio:N2}] ");
                    total += precio;
                }
            }
        }

        if (sb.Length == 0)
            return "Sin detalle de importe";

        return $"{sb}Total: ${total:N2}";
    }
    // Limpia todos los campos y restablece el formulario a su estado original para una nueva consulta.
    private void btnLimpiar_Click(object? sender, EventArgs e)
    {
        LimpiarFormulario();
    }
    // Restablece todos los campos, combos y selectores del formulario.
    private void LimpiarFormulario()
    {
        _idConsultaSeleccionada = null;
        _fechaHoraConsultaSeleccionada = null;
        _snapshotEdicion = null;
        LimpiarTodosLosErroresVisuales();

        _sincronizandoCombos = true;
        cboPropietario.DataSource = null;
        cboPropietario.DisplayMember = "NombreCompleto";
        cboPropietario.ValueMember = "Id";
        cboPropietario.DataSource = _propietarios;
        cboPropietario.SelectedIndex = -1;
        cboPropietario.Text = string.Empty;
        _sincronizandoCombos = false;

        CargarMascotasDePropietario(null);

        dtpProximoControl.Value = DateTime.Today.AddDays(15);

        txtMotivo.Clear();
        txtPeso.Clear();
        txtTemperatura.Clear();
        RestablecerAyudaPeso();
        RestablecerAyudaTemperatura();
        txtImporte.Clear();
        txtDiagnostico.Clear();
        txtObservaciones.Clear();

        btnGuardar.Enabled = true;
        btnModificar.Enabled = false;
        dgvConsultas.ClearSelection();

        lblInfoEstado.Text = "Formulario listo para registrar una nueva consulta.";
        cboPropietario.Focus();
    }
    // Cierra el formulario y regresa al panel principal.
    private void btnVolver_Click(object? sender, EventArgs e)
    {
        Close();
    }
}



