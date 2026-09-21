using System.Globalization;
using Microsoft.Extensions.DependencyInjection;
using Veterinaria.Controllers.Controladores;
using Veterinaria.Domain.Dtos;
using Veterinaria.WinForms.Sesion;

namespace Veterinaria.WinForms.Vistas.Veterinario;

/// <summary>
/// Formulario para la gestión y registro de consultas clínicas veterinarias.
/// Incorpora restricciones en tiempo real, validaciones con feedback visual
/// y sincronización inteligente entre tutores y pacientes.
/// </summary>
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
        _consultaControlador = consultaControlador;
        _mascotaControlador = mascotaControlador;
        _propietarioControlador = propietarioControlador;
        _serviceProvider = serviceProvider;
    }

    /// <summary>
    /// Inicializa la sesión del profesional, enlaza eventos de validación y carga catálogos clínicos.
    /// </summary>
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

        // Enlazar eventos de sincronización entre propietario y mascota
        cboPropietario.SelectedIndexChanged += cboPropietario_SelectedIndexChanged;
        cboMascota.SelectedIndexChanged += cboMascota_SelectedIndexChanged;
    }

    /// <summary>
    /// Aplica filtros de teclado estrictos para campos numéricos, de texto clínico y selectores de búsqueda.
    /// </summary>
    private void ConfigurarRestriccionesTeclado()
    {
        txtPeso.KeyPress += ValidarDecimal_KeyPress;
        txtTemperatura.KeyPress += ValidarDecimal_KeyPress;
        txtMotivo.KeyPress += ValidarTextoGeneral_KeyPress;
        cboMascota.KeyPress += ValidarSoloLetras_KeyPress;
        cboPropietario.KeyPress += ValidarSoloLetras_KeyPress;
    }

    /// <summary>
    /// Restablece el color de las cajas de texto y etiquetas al interactuar con ellas.
    /// </summary>
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

    /// <summary>
    /// Restringe el ingreso en cajas de texto decimal a dígitos y un único separador.
    /// </summary>
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

    /// <summary>
    /// Permite caracteres alfabéticos, numéricos, signos de puntuación estándar y previene espacios dobles.
    /// </summary>
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

    /// <summary>
    /// Restringe la búsqueda en los desplegables de mascota y propietario exclusivamente a letras y espacios, bloqueando números y caracteres especiales.
    /// </summary>
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

    /// <summary>
    /// Carga la lista completa de propietarios activos en el selector desplegable.
    /// </summary>
    private async Task CargarPropietariosAsync()
    {
        try
        {
            var res = await _propietarioControlador.ObtenerTodosAsync();
            var lista = new List<PropietarioRespuestaDto>();

            if (res.EsExitoso && res.Valor != null)
            {
                lista.AddRange(res.Valor.Where(p => p.Activo));
            }

            _propietarios = lista;
            _sincronizandoCombos = true;
            cboPropietario.DisplayMember = "NombreCompleto";
            cboPropietario.ValueMember = "Id";
            cboPropietario.DataSource = _propietarios;
            cboPropietario.SelectedIndex = -1;
            cboPropietario.Text = string.Empty;
            _sincronizandoCombos = false;
        }
        catch (Exception ex)
        {
            lblInfoEstado.Text = $"Error al cargar propietarios: {ex.Message}";
        }
    }

    /// <summary>
    /// Carga la lista completa de mascotas activas en el selector desplegable.
    /// </summary>
    private async Task CargarMascotasAsync()
    {
        try
        {
            var res = await _mascotaControlador.ObtenerTodosAsync();
            var lista = new List<MascotaRespuestaDto>();

            if (res.EsExitoso && res.Valor != null)
            {
                lista.AddRange(res.Valor.Where(m => m.Activo));
            }

            _mascotas = lista;
            _sincronizandoCombos = true;
            cboMascota.DisplayMember = "Nombre";
            cboMascota.ValueMember = "Id";
            cboMascota.DataSource = _mascotas;
            cboMascota.SelectedIndex = -1;
            cboMascota.Text = string.Empty;
            _sincronizandoCombos = false;
        }
        catch (Exception ex)
        {
            lblInfoEstado.Text = $"Error al cargar mascotas: {ex.Message}";
        }
    }

    /// <summary>
    /// Puebla la grilla de historial con las consultas clínicas registradas.
    /// </summary>
    private async Task CargarHistorialConsultasAsync()
    {
        try
        {
            var res = await _consultaControlador.ObtenerTodosAsync();
            dgvConsultas.Rows.Clear();

            if (!res.EsExitoso || res.Valor == null)
                return;

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
                    "-"
                );
                dgvConsultas.Rows[idx].Tag = c;
            }
            dgvConsultas.ClearSelection();
        }
        catch (Exception ex)
        {
            lblInfoEstado.Text = $"Error al cargar historial: {ex.Message}";
        }
    }

    /// <summary>
    /// Filtra las mascotas disponibles al seleccionar un tutor en el ComboBox de Propietario.
    /// </summary>
    private void cboPropietario_SelectedIndexChanged(object? sender, EventArgs e)
    {
        if (_sincronizandoCombos)
            return;

        if (cboPropietario.SelectedValue is long idProp && idProp > 0)
        {
            _sincronizandoCombos = true;
            var mascotasFiltradas = _mascotas.Where(m => m.IdPropietario == idProp).ToList();
            cboMascota.DataSource = null;
            cboMascota.DisplayMember = "Nombre";
            cboMascota.ValueMember = "Id";
            cboMascota.DataSource = mascotasFiltradas;
            if (mascotasFiltradas.Count == 1)
            {
                cboMascota.SelectedIndex = 0;
            }
            else
            {
                cboMascota.SelectedIndex = -1;
                cboMascota.Text = string.Empty;
            }
            _sincronizandoCombos = false;
        }
        else if (cboPropietario.SelectedIndex == -1)
        {
            _sincronizandoCombos = true;
            cboMascota.DataSource = null;
            cboMascota.DisplayMember = "Nombre";
            cboMascota.ValueMember = "Id";
            cboMascota.DataSource = _mascotas;
            cboMascota.SelectedIndex = -1;
            cboMascota.Text = string.Empty;
            _sincronizandoCombos = false;
        }
    }

    /// <summary>
    /// Sincroniza automáticamente el propietario correspondiente cuando se selecciona una mascota.
    /// </summary>
    private void cboMascota_SelectedIndexChanged(object? sender, EventArgs e)
    {
        if (_sincronizandoCombos)
            return;

        if (cboMascota.SelectedValue is long idMascota && idMascota > 0)
        {
            var mascota = _mascotas.FirstOrDefault(m => m.Id == idMascota);
            if (mascota != null && mascota.IdPropietario > 0)
            {
                _sincronizandoCombos = true;
                cboPropietario.SelectedValue = mascota.IdPropietario;
                var mascotasFiltradas = _mascotas.Where(m => m.IdPropietario == mascota.IdPropietario).ToList();
                cboMascota.DataSource = null;
                cboMascota.DisplayMember = "Nombre";
                cboMascota.ValueMember = "Id";
                cboMascota.DataSource = mascotasFiltradas;
                cboMascota.SelectedValue = idMascota;
                _sincronizandoCombos = false;
            }
        }
    }

    /// <summary>
    /// Valida la integridad y consistencia de los datos ingresados en el formulario.
    /// </summary>
    private bool ValidarCampos(out ConsultaSolicitudDto? solicitud)
    {
        solicitud = null;

        if (cboMascota.SelectedItem is not MascotaRespuestaDto mascotaSeleccionada || mascotaSeleccionada.Id <= 0)
        {
            MarcarControlConError(cboMascota, "Debe seleccionar una mascota para registrar la consulta clínica.");
            return false;
        }
        long idMascota = mascotaSeleccionada.Id;

        if (cboPropietario.SelectedItem is not PropietarioRespuestaDto propSeleccionado || propSeleccionado.Id <= 0)
        {
            MarcarControlConError(cboPropietario, "Debe seleccionar el propietario responsable de la mascota.");
            return false;
        }

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
            var textoPeso = txtPeso.Text.Trim().Replace(',', '.');
            if (!decimal.TryParse(textoPeso, NumberStyles.Any, CultureInfo.InvariantCulture, out var p) || p <= 0 || p > 150)
            {
                MarcarControlConError(txtPeso, "El peso debe ser un valor numérico válido mayor a 0 y menor o igual a 150 kg.");
                return false;
            }
            peso = p;
        }

        decimal? temp = null;
        if (!string.IsNullOrWhiteSpace(txtTemperatura.Text))
        {
            var textoTemp = txtTemperatura.Text.Trim().Replace(',', '.');
            if (!decimal.TryParse(textoTemp, NumberStyles.Any, CultureInfo.InvariantCulture, out var t) || t < 34 || t > 44)
            {
                MarcarControlConError(txtTemperatura, "La temperatura debe ser un valor biológico válido entre 34.0 °C y 44.0 °C.");
                return false;
            }
            temp = t;
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
            Observaciones = string.IsNullOrWhiteSpace(txtObservaciones.Text) ? null : txtObservaciones.Text.Trim()
        };

        return true;
    }

    /// <summary>
    /// Resalta el control defectuoso con el color Borgoña suave del sistema de diseño.
    /// </summary>
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

    /// <summary>
    /// Limpia el estado de error visual del control que recibe interacción.
    /// </summary>
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

    /// <summary>
    /// Restaura todos los controles del formulario a sus colores neutrales.
    /// </summary>
    private void LimpiarTodosLosErroresVisuales()
    {
        foreach (var (control, label) in _mapaEtiquetas)
        {
            control.BackColor = Color.White;
            label.ForeColor = Color.FromArgb(58, 53, 59);
        }
    }

    /// <summary>
    /// Registra una nueva consulta clínica en la base de datos tras validar los campos.
    /// </summary>
    private async void btnGuardar_Click(object? sender, EventArgs e)
    {
        if (!ValidarCampos(out var solicitud) || solicitud is null)
            return;

        try
        {
            btnGuardar.Enabled = false;
            Cursor = Cursors.WaitCursor;

            var resultado = await _consultaControlador.CrearAsync(solicitud);
            if (!resultado.EsExitoso)
            {
                MessageBox.Show(resultado.Mensaje, "Error al registrar consulta", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            MessageBox.Show("La consulta clínica fue registrada exitosamente.", "Consulta Registrada", MessageBoxButtons.OK, MessageBoxIcon.Information);
            LimpiarFormulario();
            await CargarHistorialConsultasAsync();
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Ocurrió un error inesperado al guardar la consulta: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        finally
        {
            btnGuardar.Enabled = true;
            Cursor = Cursors.Default;
        }
    }

    /// <summary>
    /// Actualiza la consulta clínica seleccionada en el historial.
    /// </summary>
    private async void btnModificar_Click(object? sender, EventArgs e)
    {
        if (!_idConsultaSeleccionada.HasValue)
        {
            MessageBox.Show("Debe seleccionar una consulta del historial para poder modificarla.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        if (!ValidarCampos(out var solicitud) || solicitud is null)
            return;

        try
        {
            btnModificar.Enabled = false;
            Cursor = Cursors.WaitCursor;

            var resultado = await _consultaControlador.ActualizarAsync(_idConsultaSeleccionada.Value, solicitud);
            if (!resultado.EsExitoso)
            {
                MessageBox.Show(resultado.Mensaje, "Error al actualizar consulta", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

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
            btnModificar.Enabled = false;
            Cursor = Cursors.Default;
        }
    }

    /// <summary>
    /// Carga los datos de la consulta seleccionada en la grilla para su revisión o modificación.
    /// </summary>
    private void dgvConsultas_CellClick(object? sender, DataGridViewCellEventArgs e)
    {
        if (e.RowIndex < 0 || e.RowIndex >= dgvConsultas.Rows.Count)
            return;

        var fila = dgvConsultas.Rows[e.RowIndex];
        if (fila.Tag is not ConsultaRespuestaDto consulta)
            return;

        _idConsultaSeleccionada = consulta.Id;
        _fechaHoraConsultaSeleccionada = consulta.FechaHora;
        LimpiarTodosLosErroresVisuales();

        _sincronizandoCombos = true;
        cboMascota.DataSource = _mascotas;
        cboMascota.SelectedValue = consulta.IdMascota;

        var mascota = _mascotas.FirstOrDefault(m => m.Id == consulta.IdMascota);
        if (mascota != null)
        {
            cboPropietario.SelectedValue = mascota.IdPropietario;
        }
        _sincronizandoCombos = false;

        txtMotivo.Text = consulta.Motivo ?? string.Empty;
        txtPeso.Text = consulta.PesoKg.HasValue ? consulta.PesoKg.Value.ToString("0.00", CultureInfo.InvariantCulture) : string.Empty;
        txtTemperatura.Text = consulta.Temperatura.HasValue ? consulta.Temperatura.Value.ToString("0.0", CultureInfo.InvariantCulture) : string.Empty;
        txtDiagnostico.Text = consulta.Diagnostico;
        txtObservaciones.Text = consulta.Observaciones ?? string.Empty;

        btnGuardar.Enabled = false;
        btnModificar.Enabled = true;

        lblInfoEstado.Text = $"Consulta N° {consulta.Id} ({consulta.FechaHora:dd/MM/yyyy HH:mm}) cargada para modificación.";
    }

    /// <summary>
    /// Limpia todos los campos y restablece el formulario a su estado original para una nueva consulta.
    /// </summary>
    private void btnLimpiar_Click(object? sender, EventArgs e)
    {
        LimpiarFormulario();
    }

    /// <summary>
    /// Restablece todos los campos, combos y selectores del formulario.
    /// </summary>
    private void LimpiarFormulario()
    {
        _idConsultaSeleccionada = null;
        _fechaHoraConsultaSeleccionada = null;
        LimpiarTodosLosErroresVisuales();

        _sincronizandoCombos = true;
        cboPropietario.DataSource = null;
        cboPropietario.DisplayMember = "NombreCompleto";
        cboPropietario.ValueMember = "Id";
        cboPropietario.DataSource = _propietarios;
        cboPropietario.SelectedIndex = -1;
        cboPropietario.Text = string.Empty;

        cboMascota.DataSource = null;
        cboMascota.DisplayMember = "Nombre";
        cboMascota.ValueMember = "Id";
        cboMascota.DataSource = _mascotas;
        cboMascota.SelectedIndex = -1;
        cboMascota.Text = string.Empty;
        _sincronizandoCombos = false;

        dtpProximoControl.Value = DateTime.Today.AddDays(15);

        txtMotivo.Clear();
        txtPeso.Clear();
        txtTemperatura.Clear();
        txtDiagnostico.Clear();
        txtObservaciones.Clear();

        btnGuardar.Enabled = true;
        btnModificar.Enabled = false;
        dgvConsultas.ClearSelection();

        lblInfoEstado.Text = "Formulario listo para registrar una nueva consulta.";
        cboPropietario.Focus();
    }


    /// <summary>
    /// Cierra el formulario y regresa al panel principal.
    /// </summary>
    private void btnVolver_Click(object? sender, EventArgs e)
    {
        Close();
    }
}
