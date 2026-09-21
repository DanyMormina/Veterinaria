using Veterinaria.Controllers.Controladores;
using Veterinaria.Domain.Dtos;
using Veterinaria.WinForms.Sesion;

namespace Veterinaria.WinForms.Vistas.Veterinario;

/// <summary>
/// Formulario de Ficha Médica e Historial Clínico de Pacientes.
/// Incorpora búsqueda predictiva sin caracteres numéricos, visualización en solo lectura estricta,
/// carga cronológica de consultas y feedback visual con el estándar "Ejecutivo Romántico Pastel".
/// </summary>
public partial class FormFichaMedica : Form
{
    private static readonly Color ColorError = ColorTranslator.FromHtml("#B85D69");
    private static readonly Color ColorNeutro = ColorTranslator.FromHtml("#E2D9DC");

    private readonly MascotaControlador? _mascotaControlador;
    private readonly ConsultaControlador? _consultaControlador;

    private List<MascotaRespuestaDto> _listaMascotas = [];
    private MascotaRespuestaDto? _mascotaSeleccionada = null;
    private bool _cargandoMascotas = false;

    /// <summary>
    /// Compatibilidad hacia atrás para referencias a la grilla con el nombre dgvHistorial.
    /// </summary>
    public DataGridView dgvHistorial => dgvHistorialClinico;

    /// <summary>
    /// Constructor por defecto para compatibilidad con el diseñador de Windows Forms.
    /// </summary>
    public FormFichaMedica() : this(null, null)
    {
    }

    /// <summary>
    /// Constructor principal con inyección de controladores de Mascota y Consulta.
    /// </summary>
    public FormFichaMedica(
        MascotaControlador? mascotaControlador,
        ConsultaControlador? consultaControlador)
    {
        _mascotaControlador = mascotaControlador;
        _consultaControlador = consultaControlador;

        InitializeComponent();
        ConfigurarPoliticaSoloLectura();
        ConfigurarEventosBusqueda();
    }

    /// <summary>
    /// Aplica estrictamente la política de campos no editables para el perfil del paciente.
    /// </summary>
    private void ConfigurarPoliticaSoloLectura()
    {
        txtNombre.ReadOnly = true;
        txtNombre.BackColor = Color.White;

        txtPropietario.ReadOnly = true;
        txtPropietario.BackColor = Color.White;

        txtEspecie.ReadOnly = true;
        txtEspecie.BackColor = Color.White;

        txtRaza.ReadOnly = true;
        txtRaza.BackColor = Color.White;

        rbMacho.Enabled = false;
        rbHembra.Enabled = false;

        dtpFechaNacimiento.Enabled = false;

        txtColor.ReadOnly = true;
        txtColor.BackColor = Color.White;
    }

    /// <summary>
    /// Enlaza los manejadores de eventos para el control de teclado y retroalimentación visual en búsqueda.
    /// </summary>
    private void ConfigurarEventosBusqueda()
    {
        cboBuscarMascota.KeyPress += cboBuscarMascota_KeyPress;
        cboBuscarMascota.TextChanged += cboBuscarMascota_InputInteract;
        cboBuscarMascota.Enter += cboBuscarMascota_InputInteract;
        cboBuscarMascota.SelectedIndexChanged += cboBuscarMascota_SelectedIndexChanged;
        cboBuscarMascota.Leave += cboBuscarMascota_Leave;
    }

    /// <summary>
    /// Obtiene el sexo seleccionado ("Macho", "Hembra" o vacío).
    /// </summary>
    public string ObtenerSexo()
    {
        if (rbMacho.Checked) return "Macho";
        if (rbHembra.Checked) return "Hembra";
        return string.Empty;
    }

    /// <summary>
    /// Establece la selección de sexo de la mascota ("Macho" o "Hembra").
    /// </summary>
    public void EstablecerSexo(string? sexo)
    {
        if (string.Equals(sexo, "Macho", StringComparison.OrdinalIgnoreCase))
        {
            rbMacho.Checked = true;
            rbHembra.Checked = false;
        }
        else if (string.Equals(sexo, "Hembra", StringComparison.OrdinalIgnoreCase))
        {
            rbHembra.Checked = true;
            rbMacho.Checked = false;
        }
        else
        {
            rbMacho.Checked = false;
            rbHembra.Checked = false;
        }
    }

    /// <summary>
    /// Inicializa la sesión del profesional y precarga el catálogo de mascotas activas.
    /// </summary>
    private async void FormFichaMedica_Load(object? sender, EventArgs e)
    {
        lblUsuarioSesion.Text = SesionActual.EstaAutenticado
            ? $"Dr./Dra. {SesionActual.NombreCompleto} | {SesionActual.Rol}"
            : "Dr./Dra. Lucía Pérez | Veterinario";

        lblInfoEstado.Text = $"Sesión clínica activa: {SesionActual.NombreUsuario} - {DateTime.Now:dd/MM/yyyy}";

        pnlContenedorBusqueda.BackColor = ColorNeutro;

        await CargarMascotasAsync();
    }

    /// <summary>
    /// Restringe la entrada de teclado en el buscador: bloquea números y caracteres no alfabéticos.
    /// </summary>
    private void cboBuscarMascota_KeyPress(object? sender, KeyPressEventArgs e)
    {
        // 1. Restaurar borde a color neutral ante cualquier pulsación
        pnlContenedorBusqueda.BackColor = ColorNeutro;

        // 2. Permitir teclas de control del sistema (retroceso, copiar, pegar, tab)
        if (char.IsControl(e.KeyChar))
            return;

        // 3. Bloqueo estricto de cualquier dígito numérico (0-9)
        if (char.IsDigit(e.KeyChar))
        {
            e.Handled = true; // No numbers allowed
            return;
        }

        // 4. Permitir caracteres alfabéticos (incluye letras acentuadas áéíóú, ñ y diéresis)
        if (char.IsLetter(e.KeyChar))
            return;

        // 5. Permitir un único espacio si no es al inicio ni consecutivo
        if (e.KeyChar == ' ' && sender is ComboBox cb)
        {
            if (cb.SelectionStart > 0 && cb.Text.Length > 0 && cb.Text[cb.SelectionStart - 1] != ' ')
                return;
        }

        // 6. Bloquear cualquier otro carácter especial o símbolo no alfabético
        e.Handled = true;
    }

    /// <summary>
    /// Restaura el contenedor de búsqueda al color neutral pastel (#E2D9DC) al interactuar.
    /// </summary>
    private void cboBuscarMascota_InputInteract(object? sender, EventArgs e)
    {
        pnlContenedorBusqueda.BackColor = ColorNeutro;
    }

    /// <summary>
    /// Valida si el texto ingresado coincide con un paciente al salir del control.
    /// </summary>
    private void cboBuscarMascota_Leave(object? sender, EventArgs e)
    {
        var texto = cboBuscarMascota.Text.Trim();
        if (string.IsNullOrEmpty(texto))
        {
            pnlContenedorBusqueda.BackColor = ColorNeutro;
            return;
        }

        // Si ya hay un elemento coincidente seleccionado, mantener estado neutro
        if (cboBuscarMascota.SelectedItem is MascotaComboItem seleccionado &&
            seleccionado.Texto.Equals(texto, StringComparison.OrdinalIgnoreCase))
        {
            pnlContenedorBusqueda.BackColor = ColorNeutro;
            return;
        }

        // Buscar coincidencia en la lista en memoria
        var match = _listaMascotas.FirstOrDefault(m =>
            $"{m.Nombre} - {m.NombrePropietario}".Trim().Equals(texto, StringComparison.OrdinalIgnoreCase) ||
            m.Nombre.Equals(texto, StringComparison.OrdinalIgnoreCase));

        if (match != null)
        {
            pnlContenedorBusqueda.BackColor = ColorNeutro;
            cboBuscarMascota.SelectedValue = match.Id;
        }
        else
        {
            // Búsqueda fallida o sin coincidencia: resaltar en Borgoña suave
            pnlContenedorBusqueda.BackColor = ColorError;
            lblInfoEstado.Text = $"No se encontró ninguna mascota que coincida con \"{texto}\".";
        }
    }

    /// <summary>
    /// Carga el catálogo de mascotas activas en el ComboBox de búsqueda con formato predictivo.
    /// </summary>
    private async Task CargarMascotasAsync()
    {
        if (_mascotaControlador == null) return;

        try
        {
            _cargandoMascotas = true;
            var resultado = await _mascotaControlador.ObtenerTodosAsync();
            if (!resultado.EsExitoso || resultado.Valor == null)
            {
                pnlContenedorBusqueda.BackColor = ColorError;
                lblInfoEstado.Text = "No se pudieron obtener los pacientes registrados.";
                return;
            }

            _listaMascotas = resultado.Valor
                .Where(m => m.Activo)
                .OrderBy(m => m.Nombre)
                .ToList();

            var items = _listaMascotas.Select(m => new MascotaComboItem
            {
                Mascota = m
            }).ToList();

            cboBuscarMascota.DataSource = null;
            cboBuscarMascota.DisplayMember = nameof(MascotaComboItem.Texto);
            cboBuscarMascota.ValueMember = nameof(MascotaComboItem.Id);
            cboBuscarMascota.DataSource = items;
            cboBuscarMascota.SelectedIndex = -1;
            cboBuscarMascota.Text = string.Empty;
            pnlContenedorBusqueda.BackColor = ColorNeutro;
        }
        catch (Exception ex)
        {
            pnlContenedorBusqueda.BackColor = ColorError;
            lblInfoEstado.Text = $"Error al cargar mascotas: {ex.Message}";
        }
        finally
        {
            _cargandoMascotas = false;
        }
    }

    /// <summary>
    /// Carga los datos del perfil y el historial clínico al seleccionar un paciente en el desplegable.
    /// </summary>
    private async void cboBuscarMascota_SelectedIndexChanged(object? sender, EventArgs e)
    {
        if (_cargandoMascotas) return;

        if (cboBuscarMascota.SelectedItem is MascotaComboItem item && item.Mascota != null)
        {
            pnlContenedorBusqueda.BackColor = ColorNeutro;
            var mascota = item.Mascota;
            _mascotaSeleccionada = mascota;

            // 1. Población de datos del perfil de la mascota
            txtNombre.Text = mascota.Nombre;
            txtPropietario.Text = mascota.NombrePropietario;
            txtEspecie.Text = mascota.EspecieNombre;
            txtRaza.Text = mascota.RazaNombre;
            EstablecerSexo(mascota.Sexo);
            dtpFechaNacimiento.Value = mascota.FechaNacimiento ?? DateTime.Today;
            txtColor.Text = mascota.Color ?? string.Empty;

            // 2. Carga asíncrona del historial clínico
            await CargarHistorialClinicoAsync(mascota.Id);
            lblInfoEstado.Text = $"Ficha médica cargada: {mascota.Nombre} ({mascota.NombrePropietario}).";
        }
        else if (cboBuscarMascota.SelectedIndex == -1 && string.IsNullOrWhiteSpace(cboBuscarMascota.Text))
        {
            LimpiarCamposPaciente();
            dgvHistorialClinico.Rows.Clear();
            pnlContenedorBusqueda.BackColor = ColorNeutro;
        }
    }

    /// <summary>
    /// Consulta el historial clínico mediante ConsultaController y puebla la grilla ordenado de más reciente a más antiguo.
    /// </summary>
    private async Task CargarHistorialClinicoAsync(long idMascota)
    {
        dgvHistorialClinico.Rows.Clear();
        if (_consultaControlador == null) return;

        try
        {
            Cursor = Cursors.WaitCursor;
            var resultado = await _consultaControlador.ObtenerHistorialPorMascotaIdAsync(idMascota);
            if (!resultado.EsExitoso || resultado.Valor == null)
            {
                lblInfoEstado.Text = "No se pudieron obtener las consultas clínicas de la mascota.";
                return;
            }

            // Ordenamiento cronológico de la más reciente a la más antigua
            var consultas = resultado.Valor
                .OrderByDescending(c => c.FechaHora)
                .ToList();

            foreach (var c in consultas)
            {
                var fechaTexto = c.FechaHora.ToString("dd/MM/yyyy HH:mm");
                var motivoTexto = string.IsNullOrWhiteSpace(c.Motivo) ? "-" : c.Motivo;
                var diagTexto = string.IsNullOrWhiteSpace(c.Diagnostico) ? "-" : c.Diagnostico;
                var pesoTexto = c.PesoKg.HasValue ? $"{c.PesoKg.Value:0.00} kg" : "-";
                var tempTexto = c.Temperatura.HasValue ? $"{c.Temperatura.Value:0.0} °C" : "-";
                var tratamientoTexto = !string.IsNullOrWhiteSpace(c.Tratamiento) ? c.Tratamiento : "-";
                var controlTexto = c.ProximoControl.HasValue ? c.ProximoControl.Value.ToString("dd/MM/yyyy") : "-";

                int idx = dgvHistorialClinico.Rows.Add(
                    fechaTexto,
                    motivoTexto,
                    diagTexto,
                    pesoTexto,
                    tempTexto,
                    tratamientoTexto,
                    controlTexto
                );
                dgvHistorialClinico.Rows[idx].Tag = c;
            }

            dgvHistorialClinico.ClearSelection();
        }
        catch (Exception ex)
        {
            lblInfoEstado.Text = $"Error al cargar historial clínico: {ex.Message}";
        }
        finally
        {
            Cursor = Cursors.Default;
        }
    }

    /// <summary>
    /// Restablece completamente el formulario a su estado inicial.
    /// </summary>
    private void btnLimpiar_Click(object? sender, EventArgs e)
    {
        // 1. Restablecer selector de búsqueda de mascotas
        cboBuscarMascota.SelectedIndex = -1;
        cboBuscarMascota.Text = string.Empty;
        pnlContenedorBusqueda.BackColor = ColorNeutro;

        // 2. Limpiar todos los campos de texto del perfil
        LimpiarCamposPaciente();

        // 3. Desmarcar radio buttons de sexo
        rbMacho.Checked = false;
        rbHembra.Checked = false;

        // 4. Restablecer fecha de nacimiento a la fecha actual por defecto
        dtpFechaNacimiento.Value = DateTime.Today;

        // 5. Vaciar todas las filas de la grilla de historial
        dgvHistorialClinico.Rows.Clear();

        // 6. Reubicar foco en el ComboBox de búsqueda
        cboBuscarMascota.Focus();
        lblInfoEstado.Text = "Formulario restablecido. Seleccione o busque una mascota.";
    }

    /// <summary>
    /// Limpia los campos de texto del paciente y la mascota activa en memoria.
    /// </summary>
    private void LimpiarCamposPaciente()
    {
        _mascotaSeleccionada = null;
        txtNombre.Clear();
        txtPropietario.Clear();
        txtEspecie.Clear();
        txtRaza.Clear();
        txtColor.Clear();
    }

    /// <summary>
    /// Permite al usuario refrescar manualmente el historial de la mascota seleccionada.
    /// </summary>
    private async void btnVerHistorial_Click(object? sender, EventArgs e)
    {
        if (_mascotaSeleccionada != null)
        {
            await CargarHistorialClinicoAsync(_mascotaSeleccionada.Id);
            lblInfoEstado.Text = $"Historial clínico actualizado para {_mascotaSeleccionada.Nombre}.";
        }
        else
        {
            pnlContenedorBusqueda.BackColor = ColorError;
            lblInfoEstado.Text = "Debe buscar y seleccionar una mascota para consultar su historial.";
            cboBuscarMascota.Focus();
        }
    }

    /// <summary>
    /// Gestiona la impresión de la ficha médica o notifica si aún no hay paciente seleccionado.
    /// </summary>
    private void btnImprimirFicha_Click(object? sender, EventArgs e)
    {
        if (_mascotaSeleccionada is null)
        {
            pnlContenedorBusqueda.BackColor = ColorError;
            lblInfoEstado.Text = "Por favor, busque y seleccione una mascota para imprimir su ficha médica.";
            MessageBox.Show(
                "Por favor, busque y seleccione una mascota para imprimir su ficha médica.",
                "Mascota Requerida",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning);
            cboBuscarMascota.Focus();
            return;
        }

        MessageBox.Show(
            $"Preparando impresión de la ficha médica de {_mascotaSeleccionada.Nombre} (Propietario: {_mascotaSeleccionada.NombrePropietario}).\nHistorial con {dgvHistorialClinico.Rows.Count} consulta(s) registrada(s).",
            "Impresión de Ficha Médica",
            MessageBoxButtons.OK,
            MessageBoxIcon.Information);
    }

    private void btnVolver_Click(object? sender, EventArgs e)
    {
        Close();
    }

    /// <summary>
    /// Estructura interna para los elementos del selector de mascotas con formato personalizado.
    /// </summary>
    private sealed class MascotaComboItem
    {
        public required MascotaRespuestaDto Mascota { get; init; }
        public long Id => Mascota.Id;
        public string Texto => $"{Mascota.Nombre} - {Mascota.NombrePropietario}".Trim();
        public override string ToString() => Texto;
    }
}

/// <summary>
/// Subclase de compatibilidad que expone FormHistorialClinico como alias de FormFichaMedica.
/// </summary>
public class FormHistorialClinico : FormFichaMedica
{
    public FormHistorialClinico() : base()
    {
    }

    public FormHistorialClinico(
        MascotaControlador? mascotaControlador,
        ConsultaControlador? consultaControlador)
        : base(mascotaControlador, consultaControlador)
    {
    }
}
