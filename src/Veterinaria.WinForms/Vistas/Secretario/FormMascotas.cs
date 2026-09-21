using Microsoft.Extensions.DependencyInjection;
using System.Text.RegularExpressions;
using Veterinaria.Controllers.Controladores;
using Veterinaria.Domain.Dtos;
using Veterinaria.WinForms.Sesion;

namespace Veterinaria.WinForms.Vistas.Secretario;

/// <summary>
/// Formulario de gestión y consulta de mascotas para el rol Secretario / Recepción.
/// Implementa operaciones de ABM completo, binding con la grilla, validaciones defensivas y cascada de catálogos.
/// </summary>
public partial class FormMascotas : Form
{
    // =========================================================================
    // Expresiones Regulares para Restricción y Validación Alfabética
    // =========================================================================
    private static readonly Regex RegexSoloTexto = new(@"^[a-zA-ZáéíóúÁÉÍÓÚñÑüÜ\s]+$", RegexOptions.Compiled);

    // =========================================================================
    // Paleta de Colores Oficial: Sistema "Ejecutivo Romántico Pastel" (GEMINI.md)
    // =========================================================================
    private static readonly Color ColorFondoError = ColorTranslator.FromHtml("#FDECEF");
    private static readonly Color ColorFondoNormal = Color.White;
    private static readonly Color ColorEtiquetaNormal = ColorTranslator.FromHtml("#3A353B");
    private static readonly Color ColorEtiquetaError = ColorTranslator.FromHtml("#B85D69");

    private readonly MascotaControlador? _mascotaControlador;
    private readonly PropietarioControlador? _propietarioControlador;
    private readonly EspecieControlador? _especieControlador;
    private readonly RazaControlador? _razaControlador;
    private readonly MascotaService? _mascotaService;
    private readonly IServiceProvider? _serviceProvider;

    private bool _cargandoDatos = false;
    private long _idMascotaSeleccionada = 0;

    /// <summary>
    /// Constructor por defecto para soporte del Diseñador de Windows Forms.
    /// </summary>
    public FormMascotas() : this(null, null, null, null, null, null)
    {
    }

    /// <summary>
    /// Constructor con Inyección de Dependencias.
    /// </summary>
    public FormMascotas(
        MascotaControlador? mascotaControlador,
        PropietarioControlador? propietarioControlador = null,
        EspecieControlador? especieControlador = null,
        RazaControlador? razaControlador = null,
        MascotaService? mascotaService = null,
        IServiceProvider? serviceProvider = null)
    {
        _mascotaControlador = mascotaControlador;
        _propietarioControlador = propietarioControlador;
        _especieControlador = especieControlador;
        _razaControlador = razaControlador;
        _mascotaService = mascotaService;
        _serviceProvider = serviceProvider;

        InitializeComponent();
        ConfigurarRestriccionesTeclado();
        ConfigurarValidacionVisual();
        ConfigurarEventosControles();
    }

    /// <summary>
    /// Asocia los manejadores de eventos para todos los botones, grilla y desplegables.
    /// </summary>
    private void ConfigurarEventosControles()
    {
        // Botones de acción principales
        btnGuardar.Click += async (_, _) => await GuardarMascotaAsync();
        btnModificar.Click += async (_, _) => await ModificarMascotaAsync();
        btnLimpiar.Click += (_, _) => LimpiarFormulario();
        btnVolver.Click += (_, _) => Close();

        // Botón auxiliar para redirección a gestión de propietarios
        btnNuevoPropietario.Click += btnNuevoPropietario_Click;

        // Botones de gestión satélite (Especies y Razas)
        btnAltaEspecie.Click += async (_, _) => await AbrirGestionSateliteAsync(ModoGestion.AltaEspecie);
        btnModEspecie.Click += async (_, _) => await AbrirGestionSateliteAsync(ModoGestion.ModificarEspecie);
        btnAltaRaza.Click += async (_, _) => await AbrirGestionSateliteAsync(ModoGestion.AltaRaza);
        btnModRaza.Click += async (_, _) => await AbrirGestionSateliteAsync(ModoGestion.ModificarRaza);

        // Cascada entre especie y razas
        cboEspecie.SelectedIndexChanged += async (_, _) => await cboEspecie_SelectedIndexChangedAsync();

        // Búsqueda y filtros sobre la grilla
        btnBuscar.Click += async (_, _) => await CargarMascotasGrillaAsync();
        cboFiltroEspecie.SelectedIndexChanged += async (_, _) =>
        {
            if (!_cargandoDatos)
                await CargarMascotasGrillaAsync();
        };

        txtBuscar.KeyDown += async (_, e) =>
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;
                await CargarMascotasGrillaAsync();
            }
        };

        // Binding de selección en la grilla
        dgvMascotas.CellClick += async (_, e) =>
        {
            if (e.RowIndex >= 0)
                await SeleccionarMascotaDeGrillaAsync(e.RowIndex);
        };

        dgvMascotas.SelectionChanged += async (_, _) =>
        {
            if (!_cargandoDatos && dgvMascotas.CurrentRow != null && dgvMascotas.CurrentRow.Index >= 0)
            {
                await SeleccionarMascotaDeGrillaAsync(dgvMascotas.CurrentRow.Index);
            }
        };
    }

    /// <summary>
    /// Aplica restricciones de tipeo en tiempo real (KeyPress) bloqueando caracteres no permitidos.
    /// </summary>
    private void ConfigurarRestriccionesTeclado()
    {
        // txtNombre, txtColor y txtBuscar: solo letras, acentos y espacios simples (sin números ni caracteres especiales)
        var validarSoloTexto = new KeyPressEventHandler((sender, e) =>
        {
            if (char.IsControl(e.KeyChar))
                return;

            if (char.IsLetter(e.KeyChar))
                return;

            if (e.KeyChar == ' ' && sender is TextBox txt && !string.IsNullOrEmpty(txt.Text) && !txt.Text.EndsWith(' '))
                return;

            e.Handled = true;
        });

        txtNombre.KeyPress += validarSoloTexto;
        txtColor.KeyPress += validarSoloTexto;
        txtBuscar.KeyPress += validarSoloTexto;

        // Sanitización defensiva ante pegado en el buscador para no admitir números ni caracteres especiales
        txtBuscar.TextChanged += (sender, _) =>
        {
            if (sender is TextBox txt && !string.IsNullOrEmpty(txt.Text))
            {
                var textoLimpio = Regex.Replace(txt.Text, @"[^a-zA-ZáéíóúÁÉÍÓÚñÑüÜ\s]", "");
                if (textoLimpio != txt.Text)
                {
                    var pos = txt.SelectionStart;
                    txt.Text = textoLimpio;
                    txt.SelectionStart = Math.Min(pos, txt.Text.Length);
                }
            }
        };
    }

    /// <summary>
    /// Vincula los eventos para limpiar las marcas de error visual cuando el usuario interactúa.
    /// </summary>
    private void ConfigurarValidacionVisual()
    {
        txtNombre.Enter += (_, _) => LimpiarErrorControl(txtNombre, lblNombre);
        txtNombre.TextChanged += (_, _) => LimpiarErrorControl(txtNombre, lblNombre);

        cboPropietario.Enter += (_, _) => LimpiarErrorControl(cboPropietario, lblPropietario);
        cboPropietario.SelectedIndexChanged += (_, _) => LimpiarErrorControl(cboPropietario, lblPropietario);
        cboPropietario.TextChanged += (_, _) => LimpiarErrorControl(cboPropietario, lblPropietario);

        cboEspecie.Enter += (_, _) => LimpiarErrorControl(cboEspecie, lblEspecie);
        cboEspecie.SelectedIndexChanged += (_, _) => LimpiarErrorControl(cboEspecie, lblEspecie);

        cboRaza.Enter += (_, _) => LimpiarErrorControl(cboRaza, lblRaza);
        cboRaza.SelectedIndexChanged += (_, _) => LimpiarErrorControl(cboRaza, lblRaza);

        rbMacho.CheckedChanged += (_, _) => LimpiarErrorSexo();
        rbHembra.CheckedChanged += (_, _) => LimpiarErrorSexo();

        dtpFechaNacimiento.ValueChanged += (_, _) => LimpiarErrorControl(dtpFechaNacimiento, lblFechaNacimiento);

        txtColor.Enter += (_, _) => LimpiarErrorControl(txtColor, lblColor);
        txtColor.TextChanged += (_, _) => LimpiarErrorControl(txtColor, lblColor);
    }

    private async void FormMascotas_Load(object? sender, EventArgs e)
    {
        lblUsuarioSesion.Text = SesionActual.EstaAutenticado
            ? $"Recepción: {SesionActual.NombreCompleto} | {SesionActual.Rol}"
            : "Recepción: Secretario";

        // Estado inicial de botones CRUD
        btnGuardar.Enabled = true;
        btnModificar.Enabled = false;

        // Carga inicial de catálogos y grilla
        await InicializarCatalogosAsync();
        await CargarMascotasGrillaAsync();
    }

    /// <summary>
    /// Carga asíncronamente los propietarios, especies y resetea las razas.
    /// </summary>
    private async Task InicializarCatalogosAsync()
    {
        _cargandoDatos = true;
        try
        {
            // 1. Propietarios activos en cboPropietario
            cboPropietario.Items.Clear();
            if (_propietarioControlador is not null)
            {
                var resProp = await _propietarioControlador.ObtenerTodosAsync();
                if (resProp.EsExitoso && resProp.Valor is not null)
                {
                    foreach (var p in resProp.Valor.Where(x => x.Activo).OrderBy(x => x.Nombre))
                    {
                        cboPropietario.Items.Add(new ItemCombo(p.Id, $"{p.Nombre} {p.Apellido} - DNI: {p.DNI}"));
                    }
                }
            }

            // 2. Especies activas en cboEspecie y cboFiltroEspecie
            cboEspecie.Items.Clear();
            cboFiltroEspecie.Items.Clear();
            cboFiltroEspecie.Items.Add(new ItemCombo(null, "(Todas las especies)"));

            if (_especieControlador is not null)
            {
                var resEsp = await _especieControlador.ObtenerTodosAsync();
                if (resEsp.EsExitoso && resEsp.Valor is not null)
                {
                    foreach (var esp in resEsp.Valor.Where(x => x.Activo).OrderBy(x => x.Nombre))
                    {
                        var item = new ItemCombo(esp.Id, esp.Nombre);
                        cboEspecie.Items.Add(item);
                        cboFiltroEspecie.Items.Add(item);
                    }
                }
            }

            cboFiltroEspecie.SelectedIndex = 0;

            // 3. Razas: Spec 5: cboRaza must NOT be preloaded on form startup (Enabled = false or empty)
            cboRaza.Items.Clear();
            cboRaza.Enabled = false;
        }
        finally
        {
            _cargandoDatos = false;
        }
    }

    /// <summary>
    /// Implementa el comportamiento en cascada de especies hacia razas (Spec 5).
    /// </summary>
    private async Task cboEspecie_SelectedIndexChangedAsync()
    {
        if (_cargandoDatos) return;

        var especieSeleccionada = cboEspecie.SelectedItem as ItemCombo;
        var idEspecie = especieSeleccionada?.Id;

        cboRaza.Items.Clear();

        // Si se seleccionó una especie válida: habilitar y cargar razas activas asociadas
        if (idEspecie.HasValue && idEspecie.Value > 0)
        {
            cboRaza.Enabled = true;

            if (_razaControlador is not null)
            {
                var resRazas = await _razaControlador.ObtenerPorEspecieAsync(idEspecie.Value);
                if (resRazas.EsExitoso && resRazas.Valor is not null)
                {
                    foreach (var r in resRazas.Valor.Where(x => x.Activo).OrderBy(x => x.Nombre))
                    {
                        cboRaza.Items.Add(new ItemCombo(r.Id, r.Nombre));
                    }
                }
            }
        }
        else
        {
            // Si no hay especie válida seleccionada: vaciar y deshabilitar cboRaza
            cboRaza.Enabled = false;
            cboRaza.SelectedIndex = -1;
            cboRaza.Text = string.Empty;
        }
    }

    /// <summary>
    /// Valida integralmente los campos de entrada según las especificaciones defensivas (Spec 7).
    /// Algoritmo paso a paso:
    /// 1. Valida nombre de mascota (obligatorio, entre 2 y 80 caracteres alfabéticos).
    /// 2. Valida selección obligatoria de propietario (ID válido > 0).
    /// 3. Valida selección obligatoria de especie (ID válido > 0).
    /// 4. Valida selección obligatoria de raza (ID válido > 0).
    /// 5. Valida sexo biológico (exactamente uno de rbMacho o rbHembra seleccionado).
    /// 6. Valida fecha de nacimiento (no puede ser en el futuro).
    /// 7. Valida color (opcional, pero si tiene texto debe tener al menos 3 caracteres alfabéticos).
    /// </summary>
    private bool ValidarCampos(out string mensajeError)
    {
        RestablecerBordes();

        // Paso 1: Nombre de la mascota
        var nombre = txtNombre.Text.Trim();
        if (string.IsNullOrWhiteSpace(nombre) || nombre.Length < 2 || nombre.Length > 80 || !RegexSoloTexto.IsMatch(nombre))
        {
            MarcarError(txtNombre, lblNombre, "El nombre de la mascota es obligatorio, debe tener entre 2 y 80 caracteres y contener solo letras.");
            mensajeError = "El nombre de la mascota debe tener entre 2 y 80 caracteres y contener solo letras.";
            return false;
        }

        // Paso 2: Propietario seleccionado
        var propItem = cboPropietario.SelectedItem as ItemCombo;
        if (propItem?.Id is null || propItem.Id <= 0)
        {
            MarcarError(cboPropietario, lblPropietario, "Debe seleccionar un propietario registrado.");
            mensajeError = "Debe seleccionar un propietario registrado de la lista desplegable.";
            return false;
        }

        // Paso 3: Especie seleccionada
        var espItem = cboEspecie.SelectedItem as ItemCombo;
        if (espItem?.Id is null || espItem.Id <= 0)
        {
            MarcarError(cboEspecie, lblEspecie, "Debe seleccionar una especie válida.");
            mensajeError = "Debe seleccionar una especie válida para la mascota.";
            return false;
        }

        // Paso 4: Raza seleccionada
        var razaItem = cboRaza.SelectedItem as ItemCombo;
        if (razaItem?.Id is null || razaItem.Id <= 0)
        {
            MarcarError(cboRaza, lblRaza, "Debe seleccionar una raza válida.");
            mensajeError = "Debe seleccionar una raza asociada a la especie seleccionada.";
            return false;
        }

        // Paso 5: Sexo (RadioButtons)
        if (!rbMacho.Checked && !rbHembra.Checked)
        {
            lblSexo.ForeColor = ColorEtiquetaError;
            rbMacho.Focus();
            mensajeError = "Debe seleccionar el sexo de la mascota (Macho o Hembra).";
            return false;
        }

        // Paso 6: Fecha de nacimiento
        if (dtpFechaNacimiento.Value.Date > DateTime.Today)
        {
            MarcarError(dtpFechaNacimiento, lblFechaNacimiento, "La fecha de nacimiento no puede ser una fecha futura.");
            mensajeError = "La fecha de nacimiento de la mascota no puede ser posterior a la fecha actual.";
            return false;
        }

        // Paso 7: Color (opcional, pero si fue ingresado debe tener al menos 3 caracteres alfabéticos)
        var color = txtColor.Text.Trim();
        if (!string.IsNullOrWhiteSpace(color))
        {
            if (color.Length < 3 || color.Length > 50 || !RegexSoloTexto.IsMatch(color))
            {
                MarcarError(txtColor, lblColor, "El color debe contener solo letras y tener al menos 3 caracteres.");
                mensajeError = "El color debe contener solo letras y tener al menos 3 caracteres.";
                return false;
            }
        }

        mensajeError = string.Empty;
        return true;
    }

    /// <summary>
    /// Guarda una nueva mascota en la base de datos previa validación defensiva.
    /// </summary>
    private async Task GuardarMascotaAsync()
    {
        if (!ValidarCampos(out var mensajeError))
        {
            MessageBox.Show(mensajeError, "Validación de Datos", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        var propItem = (ItemCombo)cboPropietario.SelectedItem!;
        var razaItem = (ItemCombo)cboRaza.SelectedItem!;

        var dto = new MascotaSolicitudDto
        {
            Nombre = txtNombre.Text.Trim(),
            IdPropietario = propItem.Id!.Value,
            IdRaza = razaItem.Id!.Value,
            Sexo = rbMacho.Checked ? "Macho" : "Hembra",
            FechaNacimiento = dtpFechaNacimiento.Value.Date,
            Color = string.IsNullOrWhiteSpace(txtColor.Text) ? null : txtColor.Text.Trim()
        };

        if (_mascotaControlador is null)
        {
            MessageBox.Show("El controlador de mascotas no se encuentra disponible.", "Error de Sistema", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return;
        }

        try
        {
            btnGuardar.Enabled = false;
            Cursor = Cursors.WaitCursor;

            var resultado = await _mascotaControlador.CrearAsync(dto);
            if (resultado.EsExitoso)
            {
                MessageBox.Show(resultado.Mensaje ?? "Mascota registrada exitosamente.", "Registro Exitoso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LimpiarFormulario();
                await CargarMascotasGrillaAsync();
            }
            else
            {
                MessageBox.Show(resultado.Mensaje, "Error al Guardar", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        finally
        {
            btnGuardar.Enabled = true;
            Cursor = Cursors.Default;
        }
    }

    /// <summary>
    /// Modifica la mascota seleccionada en la base de datos.
    /// </summary>
    private async Task ModificarMascotaAsync()
    {
        if (_idMascotaSeleccionada <= 0)
        {
            MessageBox.Show("Debe seleccionar una mascota de la grilla para poder modificarla.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        if (!ValidarCampos(out var mensajeError))
        {
            MessageBox.Show(mensajeError, "Validación de Datos", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        var propItem = (ItemCombo)cboPropietario.SelectedItem!;
        var razaItem = (ItemCombo)cboRaza.SelectedItem!;

        var dto = new MascotaSolicitudDto
        {
            Nombre = txtNombre.Text.Trim(),
            IdPropietario = propItem.Id!.Value,
            IdRaza = razaItem.Id!.Value,
            Sexo = rbMacho.Checked ? "Macho" : "Hembra",
            FechaNacimiento = dtpFechaNacimiento.Value.Date,
            Color = string.IsNullOrWhiteSpace(txtColor.Text) ? null : txtColor.Text.Trim()
        };

        if (_mascotaControlador is null) return;

        try
        {
            btnModificar.Enabled = false;
            Cursor = Cursors.WaitCursor;

            var resultado = await _mascotaControlador.ActualizarAsync(_idMascotaSeleccionada, dto);
            if (resultado.EsExitoso)
            {
                MessageBox.Show(resultado.Mensaje ?? "Mascota actualizada exitosamente.", "Modificación Exitosa", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LimpiarFormulario();
                await CargarMascotasGrillaAsync();
            }
            else
            {
                MessageBox.Show(resultado.Mensaje, "Error al Modificar", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        finally
        {
            btnModificar.Enabled = true;
            Cursor = Cursors.Default;
        }
    }

    /// <summary>
    /// Restablece todos los campos del formulario al modo de Creación inicial (Spec 6).
    /// </summary>
    private void LimpiarFormulario()
    {
        _cargandoDatos = true;
        try
        {
            txtNombre.Clear();
            cboPropietario.SelectedIndex = -1;
            cboPropietario.Text = string.Empty;

            cboEspecie.SelectedIndex = -1;
            cboEspecie.Text = string.Empty;

            cboRaza.Items.Clear();
            cboRaza.SelectedIndex = -1;
            cboRaza.Text = string.Empty;
            cboRaza.Enabled = false;

            rbMacho.Checked = false;
            rbHembra.Checked = false;

            dtpFechaNacimiento.Value = DateTime.Today;
            txtColor.Clear();

            _idMascotaSeleccionada = 0;

            // Transición a Modo Creación
            btnGuardar.Enabled = true;
            btnModificar.Enabled = false;

            RestablecerBordes();
            dgvMascotas.ClearSelection();
        }
        finally
        {
            _cargandoDatos = false;
        }

        txtNombre.Focus();
    }

    /// <summary>
    /// Carga y enlaza en los controles superiores los datos de la mascota seleccionada en la grilla (Spec 9).
    /// </summary>
    private async Task SeleccionarMascotaDeGrillaAsync(int rowIndex)
    {
        if (rowIndex < 0 || rowIndex >= dgvMascotas.Rows.Count)
            return;

        var fila = dgvMascotas.Rows[rowIndex];
        var valorId = fila.Cells["colId"].Value?.ToString();
        if (string.IsNullOrEmpty(valorId) || !long.TryParse(valorId, out var idMascota) || idMascota <= 0)
            return;

        if (_mascotaControlador is null) return;

        var resultado = await _mascotaControlador.ObtenerPorIdAsync(idMascota);
        if (!resultado.EsExitoso || resultado.Valor is null)
            return;

        var mascota = resultado.Valor;
        _cargandoDatos = true;
        try
        {
            _idMascotaSeleccionada = mascota.Id;
            txtNombre.Text = mascota.Nombre;

            // Seleccionar Propietario
            SeleccionarEnComboPorId(cboPropietario, mascota.IdPropietario);

            // Seleccionar Especie y poblar Razas
            SeleccionarEnComboPorId(cboEspecie, mascota.IdEspecie);

            // Carga asíncrona de razas correspondientes a la especie de la mascota
            cboRaza.Items.Clear();
            if (_razaControlador is not null)
            {
                var resRazas = await _razaControlador.ObtenerPorEspecieAsync(mascota.IdEspecie);
                if (resRazas.EsExitoso && resRazas.Valor is not null)
                {
                    foreach (var r in resRazas.Valor.Where(x => x.Activo).OrderBy(x => x.Nombre))
                    {
                        cboRaza.Items.Add(new ItemCombo(r.Id, r.Nombre));
                    }
                }
            }
            cboRaza.Enabled = true;
            SeleccionarEnComboPorId(cboRaza, mascota.IdRaza);

            // Seleccionar Sexo vía RadioButtons
            if (string.Equals(mascota.Sexo, "Macho", StringComparison.OrdinalIgnoreCase))
            {
                rbMacho.Checked = true;
                rbHembra.Checked = false;
            }
            else if (string.Equals(mascota.Sexo, "Hembra", StringComparison.OrdinalIgnoreCase))
            {
                rbHembra.Checked = true;
                rbMacho.Checked = false;
            }
            else
            {
                rbMacho.Checked = false;
                rbHembra.Checked = false;
            }

            // Fecha y Color
            dtpFechaNacimiento.Value = mascota.FechaNacimiento ?? DateTime.Today;
            txtColor.Text = mascota.Color ?? string.Empty;

            // Modo Edición: Guardar desactivado, Modificar activado
            btnGuardar.Enabled = false;
            btnModificar.Enabled = true;

            RestablecerBordes();
        }
        finally
        {
            _cargandoDatos = false;
        }
    }

    /// <summary>
    /// Consulta y puebla la grilla de mascotas aplicando los filtros de búsqueda exclusiva por nombre de mascota y de especie.
    /// </summary>
    private async Task CargarMascotasGrillaAsync()
    {
        var textoFiltro = txtBuscar.Text.Trim();
        var especieItem = cboFiltroEspecie.SelectedItem as ItemCombo;
        var idEspecie = especieItem?.Id;

        try
        {
            btnBuscar.Enabled = false;
            Cursor = Cursors.WaitCursor;

            IEnumerable<MascotaRespuestaDto> listado;

            if (_mascotaService is not null)
            {
                // Búsqueda exclusivamente por nombre de la mascota
                var resultado = await _mascotaService.BuscarMascotasAsync(
                    nombre: string.IsNullOrWhiteSpace(textoFiltro) ? null : textoFiltro,
                    nombrePropietario: null,
                    idEspecie: idEspecie,
                    idRaza: null,
                    sexo: null,
                    color: null,
                    textoGeneral: null);

                listado = resultado.EsExitoso && resultado.Valor is not null ? resultado.Valor : [];
            }
            else if (_mascotaControlador is not null)
            {
                var resultado = await _mascotaControlador.ObtenerTodosAsync();
                listado = resultado.EsExitoso && resultado.Valor is not null ? resultado.Valor : [];

                if (idEspecie.HasValue && idEspecie.Value > 0)
                {
                    listado = listado.Where(m => m.IdEspecie == idEspecie.Value);
                }

                if (!string.IsNullOrWhiteSpace(textoFiltro))
                {
                    // Filtrar exclusivamente por nombre de la mascota
                    listado = listado.Where(m =>
                        m.Nombre.Contains(textoFiltro, StringComparison.OrdinalIgnoreCase));
                }
            }
            else
            {
                listado = [];
            }

            dgvMascotas.Rows.Clear();
            foreach (var m in listado)
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

            ActualizarTotalRegistros();
        }
        finally
        {
            btnBuscar.Enabled = true;
            Cursor = Cursors.Default;
        }
    }

    /// <summary>
    /// Abre el formulario modal satélite polimórfico en el modo especificado y actualiza los desplegables (Spec 4).
    /// </summary>
    private async Task AbrirGestionSateliteAsync(ModoGestion modo)
    {
        var especieItem = cboEspecie.SelectedItem as ItemCombo;
        var idEspeciePreseleccionada = especieItem?.Id;

        using var modal = new FormGestionSatelite(
            modo,
            _especieControlador,
            _razaControlador,
            idEspeciePreseleccionada);

        if (modal.ShowDialog(this) == DialogResult.OK)
        {
            // Refrescar desplegables de especies
            await InicializarCatalogosAsync();

            // Restaurar selección previa de especie si sigue existiendo y actualizar razas
            if (idEspeciePreseleccionada.HasValue && idEspeciePreseleccionada.Value > 0)
            {
                SeleccionarEnComboPorId(cboEspecie, idEspeciePreseleccionada.Value);
                await cboEspecie_SelectedIndexChangedAsync();
            }

            // Refrescar grilla para reflejar cambios en nombres de especies o razas
            await CargarMascotasGrillaAsync();
        }
    }

    /// <summary>
    /// Abre la vista de gestión de propietarios y cierra la ventana actual de mascotas (Spec 8).
    /// </summary>
    private void btnNuevoPropietario_Click(object? sender, EventArgs e)
    {
        var duenio = this.Owner;

        // Ocultar la ventana actual para una transición limpia
        Hide();

        // Resolver y mostrar FormPropietarios manteniendo el alcance activo durante todo su ciclo de vida
        if (_serviceProvider is not null)
        {
            using var alcance = _serviceProvider.CreateScope();
            using var vistaPropietarios = alcance.ServiceProvider.GetService<FormPropietarios>() ?? new FormPropietarios();
            vistaPropietarios.ShowDialog(duenio);
        }
        else
        {
            using var vistaPropietarios = new FormPropietarios();
            vistaPropietarios.ShowDialog(duenio);
        }

        // Cerrar definitivamente la ventana actual de mascotas
        Close();
    }

    /// <summary>
    /// Resalta con color de fondo suave o etiqueta el control que falló la validación.
    /// </summary>
    private static void MarcarError(Control control, Label? etiqueta, string mensaje)
    {
        control.BackColor = ColorFondoError;

        if (etiqueta is not null)
        {
            etiqueta.ForeColor = ColorEtiquetaError;
        }

        control.Focus();
    }

    /// <summary>
    /// Restablece un control específico a su color de fondo y etiqueta neutral.
    /// </summary>
    private static void LimpiarErrorControl(Control control, Label? etiqueta)
    {
        control.BackColor = ColorFondoNormal;

        if (etiqueta is not null)
        {
            etiqueta.ForeColor = ColorEtiquetaNormal;
        }
    }

    private void LimpiarErrorSexo()
    {
        lblSexo.ForeColor = ColorEtiquetaNormal;
    }

    /// <summary>
    /// Restablece todos los campos a su color de fondo y etiqueta neutral.
    /// </summary>
    private void RestablecerBordes()
    {
        txtNombre.BackColor = ColorFondoNormal;
        lblNombre.ForeColor = ColorEtiquetaNormal;

        cboPropietario.BackColor = ColorFondoNormal;
        lblPropietario.ForeColor = ColorEtiquetaNormal;

        cboEspecie.BackColor = ColorFondoNormal;
        lblEspecie.ForeColor = ColorEtiquetaNormal;

        cboRaza.BackColor = ColorFondoNormal;
        lblRaza.ForeColor = ColorEtiquetaNormal;

        lblSexo.ForeColor = ColorEtiquetaNormal;

        dtpFechaNacimiento.BackColor = ColorFondoNormal;
        lblFechaNacimiento.ForeColor = ColorEtiquetaNormal;

        txtColor.BackColor = ColorFondoNormal;
        lblColor.ForeColor = ColorEtiquetaNormal;
    }

    /// <summary>
    /// Selecciona un elemento dentro de un ComboBox a partir de su ID encapsulado en un ItemCombo.
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
    /// Actualiza la etiqueta con el total de registros presentes en la grilla de mascotas.
    /// </summary>
    public void ActualizarTotalRegistros()
    {
        var total = dgvMascotas.Rows.Count;
        lblTotalRegistros.Text = $"Total de registros: {total}";
    }

    private void btnVolver_Click(object? sender, EventArgs e) => Close();

    private void dgvMascotas_RowsAdded(object? sender, DataGridViewRowsAddedEventArgs e) => ActualizarTotalRegistros();

    private void dgvMascotas_RowsRemoved(object? sender, DataGridViewRowsRemovedEventArgs e) => ActualizarTotalRegistros();

    /// <summary>
    /// Elemento auxiliar para vincular identificadores con leyendas en desplegables.
    /// </summary>
    private sealed record ItemCombo(long? Id, string Texto)
    {
        public override string ToString() => Texto;
    }

    private void txtColor_TextChanged(object sender, EventArgs e)
    {

    }

    private void grpDatos_Enter(object sender, EventArgs e)
    {

    }
}
