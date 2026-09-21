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

    private readonly ErrorProvider _errores = new();
    private readonly MascotaService? _mascotaService;
    private readonly PropietarioControlador? _propietarioControlador;
    private readonly EspecieControlador? _especieControlador;
    private readonly RazaControlador? _razaControlador;

    private Control[] _controlesEntrada = [];
    private List<MascotaRespuestaDto> _mascotas = [];
    private bool _cargandoDatos = false;

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

        _controlesEntrada = [txtNombre, txtPropietario, cboEspecie, cboRaza, cboSexo, txtColor, cboEstado];

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
    /// Asocia los eventos de botones y filtros en cascada.
    /// </summary>
    private void ConfigurarEventosInteraccion()
    {
        btnBuscar.Click += async (_, _) => await EjecutarBusquedaAsync();
        btnLimpiar.Click += async (_, _) => await RestablecerEstadoInicialAsync();
        cboEspecie.SelectedIndexChanged += async (_, _) => await cboEspecie_SelectedIndexChangedAsync();

        // Búsqueda rápida desde el cuadro de texto del listado
        txtBuscar.KeyDown += async (_, e) =>
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;
                await EjecutarBusquedaAsync(permitirVacio: true);
            }
        };
        txtBuscar.TextChanged += async (_, _) =>
        {
            if (_cargandoDatos) return;
            await EjecutarBusquedaAsync(permitirVacio: true);
        };

        // Filtro rápido de especie sobre la grilla
        cboFiltroEspecie.SelectedIndexChanged += async (_, _) =>
        {
            if (_cargandoDatos) return;
            var item = cboFiltroEspecie.SelectedItem as ItemCombo;
            var idEspecie = item?.Id;

            // Sincronizar con el combo principal si se utiliza el filtro rápido
            if (idEspecie.HasValue)
            {
                SeleccionarEnComboPorId(cboEspecie, idEspecie.Value);
            }
            else
            {
                cboEspecie.SelectedIndex = 0;
            }

            await EjecutarBusquedaAsync(permitirVacio: true);
        };

        // Filtro rápido de estado sobre la grilla
        cboFiltroEstado.SelectedIndexChanged += async (_, _) =>
        {
            if (_cargandoDatos) return;
            await EjecutarBusquedaAsync(permitirVacio: true);
        };

        // Selección de fila en la grilla para consultar detalles
        dgvMascotas.CellClick += dgvMascotas_CellClick;
    }

    private async void FormMascotas_Load(object? sender, EventArgs e)
    {
        lblUsuarioSesion.Text = SesionActual.EstaAutenticado
            ? $"Usuario: {SesionActual.NombreCompleto} | Rol: {SesionActual.Rol}"
            : "Usuario: Administrador";

        await InicializarCombosAsync();
        await EjecutarBusquedaAsync(permitirVacio: true);
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

            // 6. Estado en el panel de datos
            cboEstado.Items.Clear();
            cboEstado.Items.AddRange(["Activo", "Inactivo"]);
            cboEstado.SelectedIndex = 0;
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
    /// Valida la integridad de formato de los filtros de búsqueda opcionales si fueron provistos.
    /// </summary>
    private bool ValidarFiltros(out Control? controlConError, out string? mensajeError)
    {
        LimpiarErroresValidacion();

        // 1. txtNombre: opcional, pero si tiene texto debe tener al menos 2 caracteres y formato alfabético
        var nombre = txtNombre.Text.Trim();
        if (!string.IsNullOrWhiteSpace(nombre))
        {
            if (nombre.Length < 2 || !RegexNombre.IsMatch(nombre))
            {
                controlConError = txtNombre;
                mensajeError = "El nombre de la mascota debe tener entre 2 y 80 caracteres y contener solo letras y espacios.";
                return false;
            }
        }

        // 2. txtPropietario: opcional, pero si tiene texto debe tener al menos 2 caracteres y formato alfabético
        var propietario = txtPropietario.Text.Trim();
        if (!string.IsNullOrWhiteSpace(propietario))
        {
            if (propietario.Length < 2 || !RegexNombre.IsMatch(propietario))
            {
                controlConError = txtPropietario;
                mensajeError = "El nombre o apellido del propietario debe tener entre 2 y 80 caracteres y contener solo letras y espacios.";
                return false;
            }
        }

        // 3. txtColor: opcional, pero si tiene texto debe tener al menos 3 caracteres y formato alfabético
        var color = txtColor.Text.Trim();
        if (!string.IsNullOrWhiteSpace(color))
        {
            if (color.Length < 3 || !RegexColor.IsMatch(color))
            {
                controlConError = txtColor;
                mensajeError = "El color debe tener entre 3 y 50 caracteres y contener solo letras y espacios.";
                return false;
            }
        }

        controlConError = null;
        mensajeError = null;
        return true;
    }

    /// <summary>
    /// Ejecuta la consulta de búsqueda asíncrona hacia MascotaService y llena el DataGridView.
    /// Combina acumulativamente todos los criterios provistos (AND): mientras más campos se completen, más restringida será la búsqueda.
    /// </summary>
    private async Task EjecutarBusquedaAsync(bool permitirVacio = false)
    {
        var nombre = txtNombre.Text.Trim();
        var propietario = txtPropietario.Text.Trim();
        var espItem = cboEspecie.SelectedItem as ItemCombo;
        var razaItem = cboRaza.SelectedItem as ItemCombo;
        var sexo = cboSexo.SelectedItem?.ToString();
        var color = txtColor.Text.Trim();
        var textoGeneral = txtBuscar.Text.Trim();

        bool tieneEspecie = espItem?.Id.HasValue == true;
        bool tieneRaza = razaItem?.Id.HasValue == true;
        bool tieneSexo = !string.IsNullOrWhiteSpace(sexo) && !sexo.Equals("(Todos)", StringComparison.OrdinalIgnoreCase);

        // Si no es carga inicial y no completó ningún campo, advertir que debe completar al menos uno
        if (!permitirVacio &&
            string.IsNullOrWhiteSpace(nombre) &&
            string.IsNullOrWhiteSpace(propietario) &&
            !tieneEspecie &&
            !tieneRaza &&
            !tieneSexo &&
            string.IsNullOrWhiteSpace(color) &&
            string.IsNullOrWhiteSpace(textoGeneral))
        {
            MessageBox.Show(
                $"Debe completar al menos un campo{Environment.NewLine}para poder realizar la búsqueda.",
                "Búsqueda de Mascotas",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning);
            txtNombre.Focus();
            return;
        }

        if (!ValidarFiltros(out var controlConError, out var mensajeError))
        {
            MarcarError(controlConError, mensajeError!);
            MessageBox.Show(
                mensajeError,
                "Búsqueda de Mascotas",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning);
            return;
        }

        if (_mascotaService is null)
            return;

        try
        {
            btnBuscar.Enabled = false;
            btnBuscar.Text = "Buscando...";
            Cursor = Cursors.WaitCursor;

            var estadoFiltro = cboFiltroEstado.SelectedIndex;
            bool? activoFiltro = estadoFiltro switch
            {
                1 => true,
                2 => false,
                _ => null
            };

            // Cada filtro activo se combina con AND: mientras más datos ingrese, más restringida será la búsqueda
            var resultado = await _mascotaService.BuscarMascotasAsync(
                string.IsNullOrWhiteSpace(nombre) ? null : nombre,
                string.IsNullOrWhiteSpace(propietario) ? null : propietario,
                espItem?.Id,
                razaItem?.Id,
                tieneSexo ? sexo : null,
                string.IsNullOrWhiteSpace(color) ? null : color,
                string.IsNullOrWhiteSpace(textoGeneral) ? null : textoGeneral,
                activoFiltro);

            if (!resultado.EsExitoso || resultado.Valor is null)
            {
                MessageBox.Show(
                    resultado.Mensaje,
                    "Búsqueda de Mascotas",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return;
            }

            _mascotas = resultado.Valor.ToList();
            MostrarMascotasEnGrilla(_mascotas);

            if (_mascotas.Count == 0 && (!string.IsNullOrWhiteSpace(nombre) || !string.IsNullOrWhiteSpace(propietario) || espItem?.Id != null || !string.IsNullOrWhiteSpace(textoGeneral)))
            {
                MessageBox.Show(
                    $"No se encontraron mascotas que coincidan{Environment.NewLine}con los criterios de búsqueda especificados.",
                    "Búsqueda de Mascotas",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
        }
        finally
        {
            btnBuscar.Enabled = true;
            btnBuscar.Text = "Buscar";
            Cursor = Cursors.Default;
        }
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
        if (control == cboEstado) return lblEstado;
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

            if (cboEstado.Items.Count > 0)
                cboEstado.SelectedIndex = 0;

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

        await EjecutarBusquedaAsync(permitirVacio: true);
        txtNombre.Focus();
    }

    private void dgvMascotas_CellClick(object? sender, DataGridViewCellEventArgs e)
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
        if (mascota.IdEspecie > 0)
            SeleccionarEnComboPorId(cboEspecie, mascota.IdEspecie);
        if (mascota.IdRaza > 0)
            SeleccionarEnComboPorId(cboRaza, mascota.IdRaza);
        cboSexo.SelectedItem = !string.IsNullOrWhiteSpace(mascota.Sexo) ? mascota.Sexo : "(Todos)";
        txtColor.Text = mascota.Color ?? string.Empty;
        cboEstado.SelectedItem = mascota.Activo ? "Activo" : "Inactivo";
    }

    private void btnVolver_Click(object? sender, EventArgs e)
    {
        Close();
    }

    private void btnBuscar_Click(object sender, EventArgs e)
    {

    }

    /// <summary>
    /// Elemento auxiliar para vincular objetos con identificador numérico y texto en ComboBoxes.
    /// </summary>
    private sealed record ItemCombo(long? Id, string Texto)
    {
        public override string ToString() => Texto;
    }
}
