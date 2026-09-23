using System.Text.RegularExpressions;
using Veterinaria.Controllers.Controladores;
using Veterinaria.Domain.Dtos;
using Veterinaria.WinForms.Sesion;

namespace Veterinaria.WinForms.Vistas.Administrador;

public partial class FormMascotas : Form
{
    // =========================================================================
    // Expresiones Regulares para Validación de Campos de Texto
    // =========================================================================
    private static readonly Regex RegexNombre = new(@"^[a-zA-ZáéíóúÁÉÍÓÚñÑüÜ\s]{2,80}$", RegexOptions.Compiled);
    private static readonly Regex RegexColor = new(@"^[a-zA-ZáéíóúÁÉÍÓÚñÑüÜ\s]{3,50}$", RegexOptions.Compiled);

    // =========================================================================
    // Paleta de Colores Oficial: Sistema "Ejecutivo Romántico Pastel"
    // =========================================================================
    private static readonly Color ColorBordeError = ColorTranslator.FromHtml("#B85D69");
    private static readonly Color ColorBordeNeutro = ColorTranslator.FromHtml("#E2D9DC");
    private static readonly Color ColorFondoError = ColorTranslator.FromHtml("#FDECEF");
    private static readonly Color ColorFondoNormal = Color.White;
    private static readonly Color ColorEtiquetaNormal = ColorTranslator.FromHtml("#3A353B");
    private static readonly Color ColorEtiquetaError = ColorTranslator.FromHtml("#B85D69");

    private static readonly Color ColorBotonActivarHabilitado = ColorTranslator.FromHtml("#8FA89B");
    private static readonly Color ColorBotonDesactivarHabilitado = ColorTranslator.FromHtml("#B85D69");
    private static readonly Color ColorBotonDeshabilitado = ColorTranslator.FromHtml("#E2D9DC");
    private static readonly Color ColorTextoBotonDeshabilitado = ColorTranslator.FromHtml("#888888");

    private readonly ErrorProvider _errores = new();
    private readonly MascotaService? _mascotaService;
    private readonly PropietarioControlador? _propietarioControlador;
    private readonly EspecieControlador? _especieControlador;
    private readonly RazaControlador? _razaControlador;
    private Control[] _controlesEntrada = [];
    private List<MascotaRespuestaDto> _mascotas = [];
    private bool _cargandoDatos = false;
    private long? _idSeleccionado;

    /// <summary>
    /// Constructor por defecto para soporte del Diseñador de Windows Forms.
    /// </summary>
    public FormMascotas() : this(null, null, null, null)
    {
    }

    /// <summary>
    /// Constructor con Inyección de Dependencias.
    /// </summary>
    public FormMascotas(
        MascotaService? mascotaService,
        PropietarioControlador? propietarioControlador = null,
        EspecieControlador? especieControlador = null,
        RazaControlador? razaControlador = null)
    {
        _mascotaService = mascotaService;
        _propietarioControlador = propietarioControlador;
        _especieControlador = especieControlador;
        _razaControlador = razaControlador;

        InitializeComponent();
        ConfigurarValidacionVisual();
        ConfigurarRestriccionesTeclado();
        ConfigurarEventosInteraccion();
    }

    /// <summary>
    /// Inicializa el ErrorProvider y registra los controles que participan en la auditoría visual.
    /// </summary>
    private void ConfigurarValidacionVisual()
    {
        _errores.ContainerControl = this;
        _errores.BlinkStyle = ErrorBlinkStyle.NeverBlink;

        _controlesEntrada = [txtNombre, txtPropietario, cboEspecie, cboRaza, cboSexo, txtColor];

        foreach (var control in _controlesEntrada)
        {
            control.Enter += Control_LimpiarError;
            if (control is TextBox txt)
            {
                txt.TextChanged += Control_LimpiarError;
            }
            else if (control is ComboBox cbo)
            {
                cbo.SelectedIndexChanged += Control_LimpiarError;
            }
        }
    }

    /// <summary>
    /// Aplica restricciones de tipeo en tiempo real (KeyPress) bloqueando caracteres no permitidos.
    /// </summary>
    private void ConfigurarRestriccionesTeclado()
    {
        var validarSoloTexto = new KeyPressEventHandler((sender, e) =>
        {
            if (char.IsControl(e.KeyChar))
                return;

            if (char.IsLetter(e.KeyChar))
                return;

            if (e.KeyChar == ' ' && sender is TextBox txt && !txt.Text.EndsWith(' '))
                return;

            e.Handled = true;
        });

        // 1. txtNombre: Solo caracteres alfabéticos, tildes y espacios individuales
        txtNombre.KeyPress += validarSoloTexto;

        // 2. txtPropietario: Ingreso manual de nombre y apellido (solo letras y espacios)
        txtPropietario.KeyPress += validarSoloTexto;

        // 3. txtColor: Solo caracteres alfabéticos, tildes y espacios
        txtColor.KeyPress += validarSoloTexto;
    }

    /// <summary>
    /// Asocia los eventos de botones y filtros en cascada de forma reactiva.
    /// </summary>
    private void ConfigurarEventosInteraccion()
    {
        // 1. Filtros reactivos instantáneos sobre la grilla
        txtBuscar.TextChanged += (_, _) => AplicarFiltros();
        cboFiltroEstado.SelectedIndexChanged += (_, _) => AplicarFiltros();

        cboFiltroEspecie.SelectedIndexChanged += async (_, _) =>
        {
            if (_cargandoDatos) return;
            var item = cboFiltroEspecie.SelectedItem as ItemCombo;
            _cargandoDatos = true;
            try
            {
                if (item?.Id != null)
                    SeleccionarEnComboPorId(cboEspecie, item.Id.Value);
                else
                    cboEspecie.SelectedIndex = 0;
            }
            finally
            {
                _cargandoDatos = false;
            }

            await cboEspecie_SelectedIndexChangedAsync();
            AplicarFiltros();
        };

        // 2. Filtros del panel izquierdo en cascada
        cboEspecie.SelectedIndexChanged += async (_, _) =>
        {
            await cboEspecie_SelectedIndexChangedAsync();
            AplicarFiltros();
        };
        cboRaza.SelectedIndexChanged += (_, _) => AplicarFiltros();
        cboSexo.SelectedIndexChanged += (_, _) => AplicarFiltros();

        // 3. Botones de acción
        btnBuscar.Click += (_, _) => AplicarFiltros();
        btnLimpiar.Click += async (_, _) => await RestablecerEstadoInicialAsync();

        // 4. Selección de fila en la grilla para consultar detalles
        dgvMascotas.CellClick += dgvMascotas_CellClick;
        dgvMascotas.SelectionChanged += dgvMascotas_SelectionChanged;
    }

    private async void FormMascotas_Load(object? sender, EventArgs e)
    {
        lblUsuarioSesion.Text = SesionActual.EstaAutenticado
            ? $"Usuario: {SesionActual.NombreCompleto} | Rol: {SesionActual.Rol}"
            : "Usuario: Administrador";

        ActualizarEstadoBotonesAccion(null);

        await InicializarCombosAsync();
        await CargarMascotasAsync();
    }

    /// <summary>
    /// Puebla de forma asíncrona los ComboBox de Especies y Sexo, y configura sugerencias en Propietario.
    /// </summary>
    private async Task InicializarCombosAsync()
    {
        _cargandoDatos = true;
        try
        {
            // 1. Sugerencias autocompletadas opcionales en el ingreso manual de Propietario
            txtPropietario.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            txtPropietario.AutoCompleteSource = AutoCompleteSource.CustomSource;
            var autocompletarPropietarios = new AutoCompleteStringCollection();

            if (_propietarioControlador is not null)
            {
                var resultadoProp = await _propietarioControlador.ObtenerTodosAsync();
                if (resultadoProp.EsExitoso && resultadoProp.Valor is not null)
                {
                    foreach (var p in resultadoProp.Valor.Where(x => x.Activo).OrderBy(x => x.Nombre))
                    {
                        autocompletarPropietarios.Add($"{p.Nombre} {p.Apellido}");
                    }
                }
            }
            txtPropietario.AutoCompleteCustomSource = autocompletarPropietarios;

            // 2. Especies
            cboEspecie.Items.Clear();
            cboEspecie.Items.Add(new ItemCombo(null, "(Todas las especies)"));

            cboFiltroEspecie.Items.Clear();
            cboFiltroEspecie.Items.Add(new ItemCombo(null, "(Todas las especies)"));

            if (_especieControlador is not null)
            {
                var resultadoEsp = await _especieControlador.ObtenerTodosAsync();
                if (resultadoEsp.EsExitoso && resultadoEsp.Valor is not null)
                {
                    foreach (var esp in resultadoEsp.Valor.Where(x => x.Activo).OrderBy(x => x.Nombre))
                    {
                        var item = new ItemCombo(esp.Id, esp.Nombre);
                        cboEspecie.Items.Add(item);
                        cboFiltroEspecie.Items.Add(item);
                    }
                }
            }
            cboEspecie.SelectedIndex = 0;
            cboFiltroEspecie.SelectedIndex = 0;

            // 3. Sexo
            cboSexo.Items.Clear();
            cboSexo.Items.AddRange(["(Todos)", "Macho", "Hembra"]);
            cboSexo.SelectedIndex = 0;

            // 4. Razas (inicia reseteada y deshabilitada hasta seleccionar una especie)
            cboRaza.Items.Clear();
            cboRaza.Items.Add(new ItemCombo(null, "(Todas las razas)"));
            cboRaza.SelectedIndex = 0;
            cboRaza.Enabled = false;

            // 5. Filtro de Estado sobre la grilla
            cboFiltroEstado.Items.Clear();
            cboFiltroEstado.Items.AddRange(["(Todos)", "Solo Activos", "Solo Inactivos"]);
            cboFiltroEstado.SelectedIndex = 0;
        }
        finally
        {
            _cargandoDatos = false;
        }
    }

    /// <summary>
    /// Maneja la dependencia en cascada entre cboEspecie y cboRaza.
    /// Si se selecciona una especie específica, carga asíncronamente solo las razas asociadas a ella.
    /// Si se selecciona "(Todas)", resetea y deshabilita cboRaza.
    /// </summary>
    private async Task cboEspecie_SelectedIndexChangedAsync()
    {
        if (_cargandoDatos) return;

        var especieSeleccionada = cboEspecie.SelectedItem as ItemCombo;
        var idEspecie = especieSeleccionada?.Id;

        cboRaza.Items.Clear();
        cboRaza.Items.Add(new ItemCombo(null, "(Todas las razas)"));

        if (idEspecie.HasValue && idEspecie.Value > 0)
        {
            cboRaza.Enabled = true;

            if (_razaControlador is not null)
            {
                var resultadoRazas = await _razaControlador.ObtenerPorEspecieAsync(idEspecie.Value);
                if (resultadoRazas.EsExitoso && resultadoRazas.Valor is not null)
                {
                    foreach (var raza in resultadoRazas.Valor)
                    {
                        cboRaza.Items.Add(new ItemCombo(raza.Id, raza.Nombre));
                    }
                }
            }
        }
        else
        {
            cboRaza.Enabled = false;
        }

        cboRaza.SelectedIndex = 0;
    }


    /// <summary>
    /// Carga la nómina completa de mascotas desde la base de datos a la memoria
    /// y aplica los filtros vigentes para renderizar la grilla.
    /// </summary>
    private async Task CargarMascotasAsync()
    {
        if (_mascotaService is null)
            return;

        var resultado = await _mascotaService.ObtenerTodosAsync();
        if (!resultado.EsExitoso || resultado.Valor is null)
        {
            MessageBox.Show(
                resultado.Mensaje,
                "Mascotas",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning);
            return;
        }

        _mascotas = resultado.Valor.ToList();
        AplicarFiltros();
    }

    /// <summary>
    /// Aplica los filtros combinados de búsqueda en memoria sobre la lista de mascotas,
    /// actualizando la grilla de forma instantánea conforme a la misma filosofía de FormPropietarios.
    /// </summary>
    private void AplicarFiltros()
    {
        // 1. Obtener texto de búsqueda rápida y estado seleccionado
        var texto = txtBuscar.Text.Trim();
        var estadoSeleccionado = cboFiltroEstado.SelectedIndex; // 0 = (Todos), 1 = Solo Activos, 2 = Solo Inactivos

        // 2. Obtener filtros opcionales de especie, raza y sexo
        var espItem = cboFiltroEspecie.SelectedItem as ItemCombo ?? cboEspecie.SelectedItem as ItemCombo;
        var idEspecie = espItem?.Id;
        var razaItem = cboRaza.SelectedItem as ItemCombo;
        var idRaza = razaItem?.Id;
        var sexo = cboSexo.SelectedItem?.ToString();

        // 3. Obtener criterios de los campos individuales si se usó la búsqueda izquierda
        var nombre = txtNombre.Text.Trim();
        var propietario = txtPropietario.Text.Trim();
        var color = txtColor.Text.Trim();

        var consulta = _mascotas.AsEnumerable();

        // 4. Filtrar por estado (Activo / Inactivo)
        if (estadoSeleccionado == 1)
        {
            consulta = consulta.Where(m => m.Activo);
        }
        else if (estadoSeleccionado == 2)
        {
            consulta = consulta.Where(m => !m.Activo);
        }

        // 5. Filtrar por especie
        if (idEspecie.HasValue && idEspecie.Value > 0)
        {
            consulta = consulta.Where(m => m.IdEspecie == idEspecie.Value);
        }

        // 6. Filtrar por raza
        if (idRaza.HasValue && idRaza.Value > 0)
        {
            consulta = consulta.Where(m => m.IdRaza == idRaza.Value);
        }

        // 7. Filtrar por sexo
        if (!string.IsNullOrWhiteSpace(sexo) && !sexo.Equals("(Todos)", StringComparison.OrdinalIgnoreCase))
        {
            consulta = consulta.Where(m => string.Equals(m.Sexo, sexo, StringComparison.OrdinalIgnoreCase));
        }

        // 8. Filtrar por texto de búsqueda rápida (multi-campo)
        if (!string.IsNullOrWhiteSpace(texto))
        {
            consulta = consulta.Where(m =>
                m.Nombre.Contains(texto, StringComparison.OrdinalIgnoreCase) ||
                (m.NombrePropietario?.Contains(texto, StringComparison.OrdinalIgnoreCase) ?? false) ||
                (m.NombreEspecie?.Contains(texto, StringComparison.OrdinalIgnoreCase) ?? false) ||
                (m.NombreRaza?.Contains(texto, StringComparison.OrdinalIgnoreCase) ?? false) ||
                (m.Color?.Contains(texto, StringComparison.OrdinalIgnoreCase) ?? false));
        }
        else
        {
            // O filtrar por campos específicos si fueron ingresados
            if (!string.IsNullOrWhiteSpace(nombre))
                consulta = consulta.Where(m => m.Nombre.Contains(nombre, StringComparison.OrdinalIgnoreCase));
            if (!string.IsNullOrWhiteSpace(propietario))
                consulta = consulta.Where(m => m.NombrePropietario != null && m.NombrePropietario.Contains(propietario, StringComparison.OrdinalIgnoreCase));
            if (!string.IsNullOrWhiteSpace(color))
                consulta = consulta.Where(m => m.Color != null && m.Color.Contains(color, StringComparison.OrdinalIgnoreCase));
        }

        var filtradas = consulta.ToList();
        MostrarMascotasEnGrilla(filtradas);
    }

    /// <summary>
    /// Llena las filas del DataGridView con los resultados de la consulta.
    /// </summary>
    private void MostrarMascotasEnGrilla(IEnumerable<MascotaRespuestaDto> mascotas)
    {
        dgvMascotas.Rows.Clear();

        foreach (var m in mascotas)
        {
            dgvMascotas.Rows.Add(
                m.Id,
                m.Nombre,
                m.NombrePropietario,
                m.NombreEspecie,
                m.NombreRaza,
                m.Sexo,
                m.FechaNacimiento.HasValue ? m.FechaNacimiento.Value.ToString("dd/MM/yyyy") : "-",
                m.Color ?? string.Empty,
                m.Activo ? "Activo" : "Inactivo");
        }
    }

    /// <summary>
    /// Resalta visualmente un control en estado de error aplicando el estilo Ejecutivo Romántico Pastel.
    /// </summary>
    private void MarcarError(Control? control, string mensaje, bool enfocar = true)
    {
        if (control is null) return;

        if (control is TextBox txt)
        {
            txt.BackColor = ColorFondoError;
        }

        if (control.Parent is Panel pnlContenedor && pnlContenedor != pnlContenido && pnlContenedor != pnlListado)
        {
            pnlContenedor.BackColor = ColorBordeError;
        }

        var etiqueta = ObtenerEtiquetaDeControl(control);
        if (etiqueta is not null)
        {
            etiqueta.ForeColor = ColorEtiquetaError;
        }

        _errores.SetError(control, mensaje);

        if (enfocar)
        {
            control.Focus();
        }
    }

    /// <summary>
    /// Limpia el error visual de un control al interactuar con él (Enter, TextChanged, SelectedIndexChanged).
    /// </summary>
    private void Control_LimpiarError(object? sender, EventArgs e)
    {
        if (sender is not Control control) return;

        if (control is TextBox txt)
        {
            txt.BackColor = ColorFondoNormal;
        }

        if (control.Parent is Panel pnlContenedor && pnlContenedor != pnlContenido && pnlContenedor != pnlListado)
        {
            pnlContenedor.BackColor = ColorBordeNeutro;
        }

        var etiqueta = ObtenerEtiquetaDeControl(control);
        if (etiqueta is not null)
        {
            etiqueta.ForeColor = ColorEtiquetaNormal;
        }

        _errores.SetError(control, string.Empty);
    }

    /// <summary>
    /// Restaura todos los controles de entrada a su estado visual neutral.
    /// </summary>
    private void LimpiarErroresValidacion()
    {
        _errores.Clear();

        foreach (var control in _controlesEntrada)
        {
            if (control is TextBox txt)
            {
                txt.BackColor = ColorFondoNormal;
            }

            if (control.Parent is Panel pnlContenedor && pnlContenedor != pnlContenido && pnlContenedor != pnlListado)
            {
                pnlContenedor.BackColor = ColorBordeNeutro;
            }

            var etiqueta = ObtenerEtiquetaDeControl(control);
            if (etiqueta is not null)
            {
                etiqueta.ForeColor = ColorEtiquetaNormal;
            }
        }
    }

    /// <summary>
    /// Devuelve la etiqueta correspondiente a un control de entrada para manipular su color de texto.
    /// </summary>
    private Label? ObtenerEtiquetaDeControl(Control control)
    {
        if (control == txtNombre) return lblNombre;
        if (control == txtPropietario) return lblPropietario;
        if (control == cboEspecie) return lblEspecie;
        if (control == cboRaza) return lblRaza;
        if (control == cboSexo) return lblSexo;
        if (control == txtColor) return lblColor;
        return null;
    }

    /// <summary>
    /// Selecciona un elemento en un ComboBox a partir de su ID encapsulado en un ItemCombo.
    /// </summary>
    private static void SeleccionarEnComboPorId(ComboBox cbo, long id)
    {
        for (int i = 0; i < cbo.Items.Count; i++)
        {
            if (cbo.Items[i] is ItemCombo item && item.Id == id)
            {
                cbo.SelectedIndex = i;
                return;
            }
        }
    }

    /// <summary>
    /// Restablece todos los campos de búsqueda de la mascota a su estado inicial,
    /// limpia los errores visuales y recarga el listado completo de mascotas.
    /// </summary>
    private async Task RestablecerEstadoInicialAsync()
    {
        _cargandoDatos = true;
        try
        {
            txtNombre.Clear();
            txtPropietario.Clear();

            if (cboEspecie.Items.Count > 0)
                cboEspecie.SelectedIndex = 0;

            cboRaza.Items.Clear();
            cboRaza.Items.Add(new ItemCombo(null, "(Todas las razas)"));
            cboRaza.SelectedIndex = 0;
            cboRaza.Enabled = false;

            if (cboSexo.Items.Count > 0)
                cboSexo.SelectedIndex = 0;

            txtColor.Clear();

            _idSeleccionado = null;
            ActualizarEstadoBotonesAccion(null);

            txtBuscar.Clear();

            if (cboFiltroEspecie.Items.Count > 0)
                cboFiltroEspecie.SelectedIndex = 0;

            if (cboFiltroEstado.Items.Count > 0)
                cboFiltroEstado.SelectedIndex = 0;

            LimpiarErroresValidacion();
            dgvMascotas.ClearSelection();
        }
        finally
        {
            _cargandoDatos = false;
        }

        await CargarMascotasAsync();
        txtNombre.Focus();
    }

    private async void dgvMascotas_CellClick(object? sender, DataGridViewCellEventArgs e)
    {
        if (e.RowIndex < 0) return;
        var fila = dgvMascotas.Rows[e.RowIndex];
        if (fila.Cells[0].Value is null) return;
        var id = Convert.ToInt64(fila.Cells[0].Value);
        var mascota = _mascotas.FirstOrDefault(m => m.Id == id);
        if (mascota is null) return;

        LimpiarErroresValidacion();
        txtNombre.Text = mascota.Nombre;
        txtPropietario.Text = mascota.NombrePropietario ?? string.Empty;

        // Cargar especie y razas de forma secuencial evitando disparos concurrentes
        _cargandoDatos = true;
        try
        {
            if (mascota.IdEspecie > 0)
                SeleccionarEnComboPorId(cboEspecie, mascota.IdEspecie);
            else
                cboEspecie.SelectedIndex = 0;
        }
        finally
        {
            _cargandoDatos = false;
        }

        await cboEspecie_SelectedIndexChangedAsync();

        if (mascota.IdRaza > 0)
            SeleccionarEnComboPorId(cboRaza, mascota.IdRaza);

        cboSexo.SelectedItem = !string.IsNullOrWhiteSpace(mascota.Sexo) ? mascota.Sexo : "(Todos)";
        txtColor.Text = mascota.Color ?? string.Empty;
        _idSeleccionado = mascota.Id;
        ActualizarEstadoBotonesAccion(mascota.Activo);
    }

    private void btnVolver_Click(object? sender, EventArgs e)
    {
        Close();
    }

    private void btnBuscar_Click(object sender, EventArgs e)
    {

    }

    private void dgvMascotas_SelectionChanged(object? sender, EventArgs e)
    {
        if (dgvMascotas.CurrentRow == null || dgvMascotas.CurrentRow.Index < 0)
        {
            _idSeleccionado = null;
            ActualizarEstadoBotonesAccion(null);
            return;
        }

        var fila = dgvMascotas.CurrentRow;
        if (fila.Cells[0].Value is null)
        {
            _idSeleccionado = null;
            ActualizarEstadoBotonesAccion(null);
            return;
        }

        var id = Convert.ToInt64(fila.Cells[0].Value);
        var mascota = _mascotas.FirstOrDefault(m => m.Id == id);
        if (mascota is not null)
        {
            _idSeleccionado = mascota.Id;
            ActualizarEstadoBotonesAccion(mascota.Activo);
        }
    }

    /// <summary>
    /// Actualiza de forma reactiva el estado y colores de los botones de estado (Activar / Desactivar).
    /// null = sin selección (ambos deshabilitados).
    /// true = registro activo (Desactivar habilitado).
    /// false = registro inactivo (Activar habilitado).
    /// </summary>
    private void ActualizarEstadoBotonesAccion(bool? activo)
    {
        if (!activo.HasValue)
        {
            btnActivar.Enabled = false;
            btnActivar.BackColor = ColorBotonDeshabilitado;
            btnActivar.ForeColor = ColorTextoBotonDeshabilitado;

            btnDesactivar.Enabled = false;
            btnDesactivar.BackColor = ColorBotonDeshabilitado;
            btnDesactivar.ForeColor = ColorTextoBotonDeshabilitado;
        }
        else if (activo.Value)
        {
            // Registro Activo: Desactivar habilitado, Activar deshabilitado
            btnActivar.Enabled = false;
            btnActivar.BackColor = ColorBotonDeshabilitado;
            btnActivar.ForeColor = ColorTextoBotonDeshabilitado;

            btnDesactivar.Enabled = true;
            btnDesactivar.BackColor = ColorBotonDesactivarHabilitado;
            btnDesactivar.ForeColor = Color.White;
        }
        else
        {
            // Registro Inactivo: Activar habilitado, Desactivar deshabilitado
            btnActivar.Enabled = true;
            btnActivar.BackColor = ColorBotonActivarHabilitado;
            btnActivar.ForeColor = Color.White;

            btnDesactivar.Enabled = false;
            btnDesactivar.BackColor = ColorBotonDeshabilitado;
            btnDesactivar.ForeColor = ColorTextoBotonDeshabilitado;
        }
    }

    private async void btnActivar_Click(object? sender, EventArgs e)
    {
        if (!_idSeleccionado.HasValue || _idSeleccionado.Value <= 0)
        {
            MessageBox.Show("Debe seleccionar una mascota de la lista.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        var confirmacion = MessageBox.Show(
            "¿Desea reactivar este registro?",
            "Confirmar Activación",
            MessageBoxButtons.YesNo,
            MessageBoxIcon.Question);

        if (confirmacion != DialogResult.Yes)
            return;

        if (_mascotaService is null)
            return;

        var resultado = await _mascotaService.CambiarEstadoAsync(_idSeleccionado.Value, true);
        if (resultado.EsExitoso)
        {
            MessageBox.Show(resultado.Mensaje, "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
            await CargarMascotasAsync();
        }
        else
        {
            MessageBox.Show(resultado.Mensaje, "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
    }

    private async void btnDesactivar_Click(object? sender, EventArgs e)
    {
        if (!_idSeleccionado.HasValue || _idSeleccionado.Value <= 0)
        {
            MessageBox.Show("Debe seleccionar una mascota de la lista.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        var confirmacion = MessageBox.Show(
            "¿Está seguro de que desea desactivar este registro?",
            "Confirmar Desactivación",
            MessageBoxButtons.YesNo,
            MessageBoxIcon.Question);

        if (confirmacion != DialogResult.Yes)
            return;

        if (_mascotaService is null)
            return;

        var resultado = await _mascotaService.CambiarEstadoAsync(_idSeleccionado.Value, false);
        if (resultado.EsExitoso)
        {
            MessageBox.Show(resultado.Mensaje, "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
            await CargarMascotasAsync();
        }
        else
        {
            MessageBox.Show(resultado.Mensaje, "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
    }

    /// <summary>
    /// Elemento auxiliar para vincular objetos con identificador numérico y texto en ComboBoxes.
    /// </summary>
    private sealed record ItemCombo(long? Id, string Texto)
    {
        public override string ToString() => Texto;
    }
}
