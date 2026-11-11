using Microsoft.Extensions.DependencyInjection;
using System.Text.RegularExpressions;
using Veterinaria.Controllers.Controladores;
using Veterinaria.Domain.Dtos;
using Veterinaria.WinForms.Sesion;

namespace Veterinaria.WinForms.Vistas.Secretario;
// Formulario de gestión y consulta de mascotas para el rol Secretario / Recepción.
// Implementa operaciones de ABM completo, binding con la grilla, validaciones defensivas y cascada de catálogos.
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

    private static readonly Color ColorBotonActivarHabilitado = ColorTranslator.FromHtml("#8FA89B");
    private static readonly Color ColorBotonDesactivarHabilitado = ColorTranslator.FromHtml("#B85D69");
    private static readonly Color ColorBotonDeshabilitado = ColorTranslator.FromHtml("#E2D9DC");
    private static readonly Color ColorTextoBotonDeshabilitado = ColorTranslator.FromHtml("#888888");

    private readonly MascotaControlador? _mascotaControlador;
    private readonly PropietarioControlador? _propietarioControlador;
    private readonly EspecieControlador? _especieControlador;
    private readonly RazaControlador? _razaControlador;
    private readonly MascotaService? _mascotaService;
    private readonly IServiceProvider? _serviceProvider;

    private bool _cargandoDatos = false;
    private readonly System.Windows.Forms.Timer _timerBusqueda = new() { Interval = 300 };
    private long _idMascotaSeleccionada = 0;
    private string? _snapshotEdicion;
    // Constructor por defecto para soporte del Diseñador de Windows Forms.
    public FormMascotas() : this(null, null, null, null, null, null)
    {
    }
    // Constructor con Inyección de Dependencias.
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
        Controles.ComboBuscable.Habilitar(cboPropietario, cboEspecie, cboRaza);
        Controles.ComboBuscable.Habilitar(permitirVacio: false, cboFiltroEspecie);
        dtpFechaNacimiento.MaxDate = DateTime.Today.AddDays(1).AddTicks(-1);
        ConfigurarRestriccionesTeclado();
        ConfigurarValidacionVisual();
        ConfigurarEventosControles();
    }
    // Asocia los manejadores de eventos para todos los botones, grilla y desplegables.
    private void ConfigurarEventosControles()
    {
        // Botones de acción principales del formulario
        btnGuardar.Click += async (_, _) => await GuardarMascotaAsync(); // Registra una nueva mascota en la base de datos
        btnModificar.Click += async (_, _) => await ModificarMascotaAsync(); // Actualiza los datos de la mascota seleccionada
        btnLimpiar.Click += (_, _) => LimpiarFormulario(); // Limpia los campos y vuelve al modo de creación
        btnVolver.Click += (_, _) => Close(); // Cierra la ventana y regresa al panel principal
        btnActivar.Click += async (_, _) => await btnActivar_Click(); // Reactiva una mascota inactiva (Soft Delete)
        btnDesactivar.Click += async (_, _) => await btnDesactivar_Click(); // Desactiva lógicamente una mascota activa

        // Botón auxiliar para dar de alta un propietario rápido sin perder los datos de la mascota
        btnNuevoPropietario.Click += btnNuevoPropietario_Click;

        // Botones satélites para abrir modales de creación y modificación de especies y razas
        btnAltaEspecie.Click += async (_, _) => await AbrirGestionSateliteAsync(ModoGestion.AltaEspecie);
        btnModEspecie.Click += async (_, _) => await AbrirGestionSateliteAsync(ModoGestion.ModificarEspecie);
        btnAltaRaza.Click += async (_, _) => await AbrirGestionSateliteAsync(ModoGestion.AltaRaza);
        btnModRaza.Click += async (_, _) => await AbrirGestionSateliteAsync(ModoGestion.ModificarRaza);

        // Dispara la carga en cascada de razas cuando el usuario cambia la especie
        cboEspecie.SelectedIndexChanged += async (_, _) => await cboEspecie_SelectedIndexChangedAsync();

        // Búsqueda y filtros sobre la grilla
        btnBuscar.Click += async (_, _) => await CargarMascotasGrillaAsync(); // Búsqueda manual al presionar el botón
        cboFiltroEspecie.SelectedIndexChanged += async (_, _) => // Filtrado inmediato al cambiar el desplegable de especies
        {
            if (!_cargandoDatos)
                await CargarMascotasGrillaAsync();
        };

        // Permite disparar la búsqueda presionando la tecla Enter en la caja de texto
        txtBuscar.KeyDown += async (_, e) =>
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true; // Evita el sonido de alerta del sistema operativo
                await CargarMascotasGrillaAsync();
            }
        };

        // Búsqueda reactiva con debounce: reinicia el timer cada vez que el usuario teclea
        txtBuscar.TextChanged += (_, _) =>
        {
            _timerBusqueda.Stop();
            _timerBusqueda.Start();
        };
        // Al transcurrir los 300 ms sin escribir, el timer se detiene y ejecuta la consulta SQL
        _timerBusqueda.Tick += async (_, _) =>
        {
            _timerBusqueda.Stop();
            await CargarMascotasGrillaAsync();
        };
        // Libera los recursos del timer cuando se cierra el formulario
        FormClosed += (_, _) => _timerBusqueda.Dispose();

        // Selección de filas en la grilla para cargar los datos en el formulario superior
        dgvMascotas.CellClick += async (_, e) =>
        {
            if (e.RowIndex >= 0)
                await SeleccionarMascotaDeGrillaAsync(e.RowIndex);
        };

        // Sincroniza la selección también al navegar con las flechas del teclado
        dgvMascotas.SelectionChanged += async (_, _) =>
        {
            if (!_cargandoDatos && dgvMascotas.CurrentRow != null && dgvMascotas.CurrentRow.Index >= 0)
            {
                await SeleccionarMascotaDeGrillaAsync(dgvMascotas.CurrentRow.Index);
            }
        };
    }
    // Aplica restricciones de tipeo en tiempo real (KeyPress) bloqueando caracteres no permitidos.
    private void ConfigurarRestriccionesTeclado()
    {
        // txtNombre, txtColor, txtBuscar y desplegables de búsqueda (Especie y Raza): solo letras, acentos y espacios simples
        var validarSoloTexto = new KeyPressEventHandler((sender, e) =>
        {
            // Permite teclas de control del sistema (BackSpace, Delete, Enter, Flechas)
            if (char.IsControl(e.KeyChar))
                return;

            // Permite caracteres alfabéticos (incluye letras mayúsculas, minúsculas y tildes)
            if (char.IsLetter(e.KeyChar))
                return;

            // Obtiene el texto actual según el tipo de control (TextBox o ComboBox)
            var textoActual = sender switch
            {
                TextBox txt => txt.Text,
                ComboBox cbo => cbo.Text,
                _ => string.Empty
            };

            // Permite un espacio solo si ya hay texto previo y no termina con espacio (evita espacios dobles o iniciales)
            if (e.KeyChar == ' ' && !string.IsNullOrEmpty(textoActual) && !textoActual.EndsWith(' '))
                return;

            // Si es un número o carácter especial, cancela la pulsación impidiendo que se escriba en el campo
            e.Handled = true;
        });

        // Vincula el validador a los campos de texto
        txtNombre.KeyPress += validarSoloTexto;
        txtColor.KeyPress += validarSoloTexto;
        txtBuscar.KeyPress += validarSoloTexto;

        // Vincula el validador a los desplegables de búsqueda (evita ingresar números al tipear especies o razas)
        cboEspecie.KeyPress += validarSoloTexto;
        cboRaza.KeyPress += validarSoloTexto;
        cboFiltroEspecie.KeyPress += validarSoloTexto;

        // Sanitización defensiva ante pegado (Ctrl+V) para limpiar números o símbolos automáticamente
        void SanitizarTexto(object? sender, EventArgs _)
        {
            if (sender is TextBox txt && !string.IsNullOrEmpty(txt.Text))
            {
                var textoLimpio = Regex.Replace(txt.Text, @"[^a-zA-ZáéíóúÁÉÍÓÚñÑüÜ\s]", "");
                if (textoLimpio != txt.Text)
                {
                    var pos = txt.SelectionStart;
                    txt.Text = textoLimpio;
                    txt.SelectionStart = Math.Min(pos, txt.Text.Length); // Conserva la posición del cursor
                }
            }
            else if (sender is ComboBox cbo && !string.IsNullOrEmpty(cbo.Text))
            {
                var textoLimpio = Regex.Replace(cbo.Text, @"[^a-zA-ZáéíóúÁÉÍÓÚñÑüÜ\s]", "");
                if (textoLimpio != cbo.Text)
                {
                    var pos = cbo.SelectionStart;
                    cbo.Text = textoLimpio;
                    cbo.SelectionStart = Math.Min(pos, cbo.Text.Length); // Conserva la posición del cursor
                }
            }
        }

        // Asocia la sanitización al evento TextChanged de cada control
        txtBuscar.TextChanged += SanitizarTexto;
        cboEspecie.TextChanged += SanitizarTexto;
        cboRaza.TextChanged += SanitizarTexto;
        cboFiltroEspecie.TextChanged += SanitizarTexto;
    }
    // Vincula los eventos para limpiar las marcas de error visual cuando el usuario interactúa.
    private void ConfigurarValidacionVisual()
    {
        // Al entrar o modificar el campo, se remueve el fondo de error y la etiqueta vuelve a su color normal
        txtNombre.Enter += (_, _) => LimpiarErrorControl(txtNombre, lblNombre);
        txtNombre.TextChanged += (_, _) => LimpiarErrorControl(txtNombre, lblNombre);

        cboPropietario.Enter += (_, _) => LimpiarErrorControl(cboPropietario, lblPropietario);
        cboPropietario.SelectedIndexChanged += (_, _) => LimpiarErrorControl(cboPropietario, lblPropietario);
        cboPropietario.TextChanged += (_, _) => LimpiarErrorControl(cboPropietario, lblPropietario);

        cboEspecie.Enter += (_, _) => LimpiarErrorControl(cboEspecie, lblEspecie);
        cboEspecie.SelectedIndexChanged += (_, _) => LimpiarErrorControl(cboEspecie, lblEspecie);

        cboRaza.Enter += (_, _) => LimpiarErrorControl(cboRaza, lblRaza);
        cboRaza.SelectedIndexChanged += (_, _) => LimpiarErrorControl(cboRaza, lblRaza);

        // Limpia el color de advertencia de la etiqueta de sexo al seleccionar una opción
        rbMacho.CheckedChanged += (_, _) => LimpiarErrorSexo();
        rbHembra.CheckedChanged += (_, _) => LimpiarErrorSexo();

        dtpFechaNacimiento.ValueChanged += (_, _) => LimpiarErrorControl(dtpFechaNacimiento, lblFechaNacimiento);

        txtColor.Enter += (_, _) => LimpiarErrorControl(txtColor, lblColor);
        txtColor.TextChanged += (_, _) => LimpiarErrorControl(txtColor, lblColor);
    }

    // Maneja el evento de carga inicial del formulario
    private async void FormMascotas_Load(object? sender, EventArgs e)
    {
        // Muestra en el encabezado el nombre completo y rol del usuario autenticado en la sesión actual
        lblUsuarioSesion.Text = SesionActual.EstaAutenticado
            ? $"Recepción: {SesionActual.NombreCompleto} | {SesionActual.Rol}"
            : "Recepción: Secretario";

        // Establece el estado inicial: modo creación (Guardar habilitado, Modificar deshabilitado)
        btnGuardar.Enabled = true;
        btnModificar.Enabled = false;

        // Deshabilita los botones de Activar y Desactivar hasta que el usuario elija un registro de la grilla
        ActualizarEstadoBotonesAccion(null);

        // Inicializa los catálogos en los ComboBox y consulta las mascotas en la grilla
        await InicializarCatalogosAsync();
        await CargarMascotasGrillaAsync();
        LimpiarFormulario(); // Deja el formulario listo para un nuevo registro
    }
    // Carga asíncronamente los propietarios, especies y resetea las razas.
    private async Task InicializarCatalogosAsync()
    {
        _cargandoDatos = true; // Bandera para evitar disparar eventos indeseados durante la carga
        try
        {
            // 1. Carga los propietarios activos ordenados alfabéticamente en cboPropietario
            cboPropietario.Items.Clear();
            if (_propietarioControlador is not null)
            {
                var resProp = await _propietarioControlador.ObtenerTodosAsync();
                if (resProp.EsExitoso && resProp.Valor is not null)
                {
                    foreach (var p in resProp.Valor.Where(x => x.Activo).OrderBy(x => x.Nombre))
                    {
                        // Guarda el ID y muestra el Nombre, Apellido y DNI en el desplegable
                        cboPropietario.Items.Add(new ItemCombo(p.Id, $"{p.Nombre} {p.Apellido} - DNI: {p.DNI}"));
                    }
                }
            }

            // 2. Carga las especies activas en el combo del formulario y en el filtro inferior
            cboEspecie.Items.Clear();
            cboFiltroEspecie.Items.Clear();
            cboFiltroEspecie.Items.Add(new ItemCombo(null, "(Todas las especies)")); // Opción neutral de filtro

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

            cboFiltroEspecie.SelectedIndex = 0; // Selecciona por defecto "(Todas las especies)"

            // 3. Regla de negocio: cboRaza inicia vacío y deshabilitado hasta que se elija una especie
            cboRaza.Items.Clear();
            cboRaza.Enabled = false;

            // Limpia cualquier selección residual en los combos superiores
            cboPropietario.SelectedIndex = -1;
            cboPropietario.Text = string.Empty;
            cboEspecie.SelectedIndex = -1;
            cboEspecie.Text = string.Empty;
        }
        finally
        {
            _cargandoDatos = false; // Restablece la bandera de carga
        }
    }
    // Implementa el comportamiento en cascada de especies hacia razas.
    private async Task cboEspecie_SelectedIndexChangedAsync()
    {
        if (_cargandoDatos) return; // Evita ejecuciones si se están reseteando datos

        var especieSeleccionada = cboEspecie.SelectedItem as ItemCombo;
        var idEspecie = especieSeleccionada?.Id;

        cboRaza.Items.Clear(); // Limpia el combo de razas antes de recargar

        // Si se seleccionó una especie válida, consulta asíncronamente sus razas asociadas
        if (idEspecie.HasValue && idEspecie.Value > 0)
        {
            cboRaza.Enabled = true; // Habilita el control de razas

            if (_razaControlador is not null)
            {
                // Trae exclusivamente las razas activas que pertenezcan a la especie elegida
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
            // Si el usuario deseleccionó la especie, deshabilita y vacía el combo de razas
            cboRaza.Enabled = false;
            cboRaza.SelectedIndex = -1;
            cboRaza.Text = string.Empty;
        }
    }
    // Valida integralmente los campos de entrada según las reglas de negocio del formulario.
    // Algoritmo de validación defensiva paso a paso:
    // 1. Valida nombre de mascota (obligatorio, entre 2 y 80 caracteres alfabéticos).
    // 2. Valida selección obligatoria de propietario (ID válido > 0).
    // 3. Valida selección obligatoria de especie (ID válido > 0).
    // 4. Valida selección obligatoria de raza (ID válido > 0).
    // 5. Valida sexo biológico (exactamente uno de rbMacho o rbHembra seleccionado).
    // 6. Valida fecha de nacimiento (no puede ser una fecha futura).
    // 7. Valida color (obligatorio, entre 3 y 50 caracteres alfabéticos).
    private bool ValidarCampos(out string mensajeError)
    {
        RestablecerBordes(); // Restablece colores normales de fondo y etiquetas

        // Paso 1: Validación del nombre de la mascota
        var nombre = txtNombre.Text.Trim();
        if (string.IsNullOrWhiteSpace(nombre) || nombre.Length < 2 || nombre.Length > 80 || !RegexSoloTexto.IsMatch(nombre))
        {
            MarcarError(txtNombre, lblNombre, "El nombre de la mascota es obligatorio, debe tener entre 2 y 80 caracteres y contener solo letras.");
            mensajeError = "El nombre de la mascota debe tener entre 2 y 80 caracteres y contener solo letras.";
            return false;
        }

        // Paso 2: Validación del propietario seleccionado
        var propItem = cboPropietario.SelectedItem as ItemCombo;
        if (propItem?.Id is null || propItem.Id <= 0)
        {
            MarcarError(cboPropietario, lblPropietario, "Debe seleccionar un propietario registrado.");
            mensajeError = "Debe seleccionar un propietario registrado de la lista desplegable.";
            return false;
        }

        // Paso 3: Validación de la especie seleccionada
        var espItem = cboEspecie.SelectedItem as ItemCombo;
        if (espItem?.Id is null || espItem.Id <= 0)
        {
            MarcarError(cboEspecie, lblEspecie, "Debe seleccionar una especie válida.");
            mensajeError = "Debe seleccionar una especie válida para la mascota.";
            return false;
        }

        // Paso 4: Validación de la raza seleccionada
        var razaItem = cboRaza.SelectedItem as ItemCombo;
        if (razaItem?.Id is null || razaItem.Id <= 0)
        {
            MarcarError(cboRaza, lblRaza, "Debe seleccionar una raza válida.");
            mensajeError = "Debe seleccionar una raza asociada a la especie seleccionada.";
            return false;
        }

        // Paso 5: Validación del sexo biológico mediante RadioButtons
        if (!rbMacho.Checked && !rbHembra.Checked)
        {
            lblSexo.ForeColor = ColorEtiquetaError;
            rbMacho.Focus();
            mensajeError = "Debe seleccionar el sexo de la mascota (Macho o Hembra).";
            return false;
        }

        // Paso 6: Validación de fecha de nacimiento (regla temporal)
        if (dtpFechaNacimiento.Value.Date > DateTime.Today)
        {
            MarcarError(dtpFechaNacimiento, lblFechaNacimiento, "La fecha de nacimiento no puede ser una fecha futura.");
            mensajeError = "La fecha de nacimiento de la mascota no puede ser posterior a la fecha actual.";
            return false;
        }

        // Paso 7: Validación del color del pelaje
        var color = txtColor.Text.Trim();
        if (string.IsNullOrWhiteSpace(color) || color.Length < 3 || color.Length > 50 || !RegexSoloTexto.IsMatch(color))
        {
            MarcarError(txtColor, lblColor, "El color es obligatorio y debe contener solo letras (mínimo 3 caracteres).");
            mensajeError = "El color es obligatorio y debe contener solo letras (mínimo 3 caracteres).";
            return false;
        }

        mensajeError = string.Empty;
        return true; // Todos los campos son válidos
    }
    // Guarda una nueva mascota en la base de datos previa validación defensiva.
    private async Task GuardarMascotaAsync()
    {
        // 1. Ejecuta la validación de todos los campos del formulario
        if (!ValidarCampos(out var mensajeError))
        {
            MessageBox.Show(mensajeError, "Validación de Datos", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        // 2. Extrae los identificadores foráneos de los combos seleccionados
        var propItem = (ItemCombo)cboPropietario.SelectedItem!;
        var razaItem = (ItemCombo)cboRaza.SelectedItem!;

        // 3. Mapea los controles de la interfaz al Data Transfer Object (DTO)
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
            // Bloquea el botón para prevenir el "doble clic involuntario" y que no se dupliquen registros en la base de datos
            btnGuardar.Enabled = false;
            // Cambia el cursor al puntero de espera (WaitCursor) para dar feedback visual de que el sistema está trabajando
            Cursor = Cursors.WaitCursor;

            // 4. Invoca al controlador de negocio para persistir la entidad en SQL Server
            var resultado = await _mascotaControlador.CrearAsync(dto);
            if (resultado.EsExitoso)
            {
                MessageBox.Show(resultado.Mensaje ?? "Mascota registrada exitosamente.", "Registro Exitoso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LimpiarFormulario(); // Restablece los campos al modo creación
                await CargarMascotasGrillaAsync(); // Refresca la grilla para mostrar la nueva mascota
            }
            else
            {
                MessageBox.Show(resultado.Mensaje, "Error al Guardar", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        finally
        {
            // El bloque finally garantiza que el botón se reactive y el cursor vuelva a Default, incluso si ocurrió un error
            btnGuardar.Enabled = true;
            Cursor = Cursors.Default; // Restaura el mouse a su estado normal para no dejar bloqueada la interfaz
        }
    }
    // Modifica los datos de la mascota actualmente seleccionada.
    private async Task ModificarMascotaAsync()
    {
        // Verifica que se haya seleccionado previamente una mascota válida de la grilla
        if (_idMascotaSeleccionada <= 0)
        {
            MessageBox.Show("Debe seleccionar una mascota de la grilla para poder modificarla.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        // Valida que los nuevos datos ingresados cumplan con las reglas de negocio
        if (!ValidarCampos(out var mensajeError))
        {
            MessageBox.Show(mensajeError, "Validación de Datos", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        // Optimización por Snapshot: compara los valores actuales con la copia tomada al seleccionar
        if (string.Equals(ObtenerSnapshotFormulario(), _snapshotEdicion, StringComparison.Ordinal))
        {
            MessageBox.Show("No se detectaron cambios para guardar.", "Sin cambios", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return; // Cancela la operación sin enviar peticiones innecesarias a la base de datos
        }

        // Extrae los ítems y construye el DTO con los datos actualizados
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
            // Deshabilita el botón temporalmente y activa el cursor de espera para evitar envíos duplicados concurrentes
            btnModificar.Enabled = false;
            Cursor = Cursors.WaitCursor; // Informa visualmente al usuario que la actualización está en progreso

            // Invoca al controlador para actualizar la mascota en la base de datos
            var resultado = await _mascotaControlador.ActualizarAsync(_idMascotaSeleccionada, dto);
            if (resultado.EsExitoso)
            {
                MessageBox.Show(resultado.Mensaje ?? "Mascota actualizada exitosamente.", "Modificación Exitosa", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LimpiarFormulario(); // Regresa al modo creación
                await CargarMascotasGrillaAsync(); // Refresca la grilla con los cambios
            }
            else
            {
                MessageBox.Show(resultado.Mensaje, "Error al Modificar", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        finally
        {
            // Bloque finally para asegurar que el mouse y los botones se restablezcan ante cualquier resultado o excepción
            btnModificar.Enabled = true;
            Cursor = Cursors.Default; // Vuelve el cursor al estado predeterminado
        }
    }
    // Restablece todos los campos del formulario al modo de Creación inicial.
    private void LimpiarFormulario()
    {
        _cargandoDatos = true; // Evita disparar eventos de selección durante el reseteo
        try
        {
            // Limpia los datos de texto y selecciones
            txtNombre.Clear();
            cboPropietario.SelectedIndex = -1;
            cboPropietario.Text = string.Empty;

            cboEspecie.SelectedIndex = -1;
            cboEspecie.Text = string.Empty;

            // Restablece el combo de razas a su estado inicial (vacío y deshabilitado)
            cboRaza.Items.Clear();
            cboRaza.SelectedIndex = -1;
            cboRaza.Text = string.Empty;
            cboRaza.Enabled = false;

            // Desmarca las opciones de sexo
            rbMacho.Checked = false;
            rbHembra.Checked = false;

            // Restablece la fecha a hoy y limpia el color
            dtpFechaNacimiento.Value = DateTime.Today;
            txtColor.Clear();

            // Resetea el identificador de edición y la instantánea en memoria
            _idMascotaSeleccionada = 0;
            _snapshotEdicion = null;

            // Transición de botones: pasa a Modo Creación (Guardar activo, Modificar inactivo)
            btnGuardar.Enabled = true;
            btnModificar.Enabled = false;

            // Deshabilita los botones de estado porque no hay ningún registro seleccionado
            ActualizarEstadoBotonesAccion(null);

            // Quita cualquier color de error y deselecciona filas en la grilla
            RestablecerBordes();
            dgvMascotas.ClearSelection();
        }
        finally
        {
            _cargandoDatos = false;
        }

        // Coloca el foco en el primer campo para facilitar la carga al usuario
        txtNombre.Focus();
    }
    // Carga y enlaza en los controles superiores los datos de la mascota seleccionada en la grilla.
    private async Task SeleccionarMascotaDeGrillaAsync(int rowIndex)
    {
        // Valida que el índice de fila se encuentre dentro del rango válido
        if (rowIndex < 0 || rowIndex >= dgvMascotas.Rows.Count)
            return;

        // Extrae el ID de la mascota desde la columna oculta de la fila seleccionada
        var fila = dgvMascotas.Rows[rowIndex];
        var valorId = fila.Cells["colId"].Value?.ToString();
        if (string.IsNullOrEmpty(valorId) || !long.TryParse(valorId, out var idMascota) || idMascota <= 0)
            return;

        if (_mascotaControlador is null) return;

        // Consulta asíncronamente a la base de datos el detalle completo de la mascota
        var resultado = await _mascotaControlador.ObtenerPorIdAsync(idMascota);
        if (!resultado.EsExitoso || resultado.Valor is null)
            return;

        var mascota = resultado.Valor;
        _cargandoDatos = true;
        try
        {
            _idMascotaSeleccionada = mascota.Id; // Guarda el ID de la mascota en edición
            txtNombre.Text = mascota.Nombre;

            // Preselecciona el propietario asociado en el combo
            SeleccionarEnComboPorId(cboPropietario, mascota.IdPropietario);

            // Preselecciona la especie
            SeleccionarEnComboPorId(cboEspecie, mascota.IdEspecie);

            // Carga asíncrona de razas correspondientes a la especie de la mascota seleccionada
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
            // Preselecciona la raza de la mascota una vez poblado el combo
            SeleccionarEnComboPorId(cboRaza, mascota.IdRaza);

            // Vincula el sexo biológico con el RadioButton correspondiente
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

            // Asigna la fecha de nacimiento y color
            dtpFechaNacimiento.Value = mascota.FechaNacimiento ?? DateTime.Today;
            txtColor.Text = mascota.Color ?? string.Empty;

            // Transición a Modo Edición: deshabilita Guardar y habilita Modificar
            btnGuardar.Enabled = false;
            btnModificar.Enabled = true;

            // Genera una instantánea (snapshot) de los datos recién cargados para detectar cambios futuros
            _snapshotEdicion = ObtenerSnapshotFormulario();

            // Configura de forma reactiva los botones de Activar/Desactivar según el estado activo de la mascota
            ActualizarEstadoBotonesAccion(mascota.Activo);

            // Limpia marcas previas de error visual
            RestablecerBordes();
        }
        finally
        {
            _cargandoDatos = false;
        }
    }

    // Serializa en una cadena delimitada por tuberías (|) el estado actual de los controles para auditoría de cambios en memoria
    private string ObtenerSnapshotFormulario()
    {
        var propId = (cboPropietario.SelectedItem as ItemCombo)?.Id ?? 0;
        var razaId = (cboRaza.SelectedItem as ItemCombo)?.Id ?? 0;
        var sexo = rbMacho.Checked ? "Macho" : rbHembra.Checked ? "Hembra" : "";
        return string.Join("|",
            txtNombre.Text.Trim(),
            propId,
            razaId,
            sexo,
            dtpFechaNacimiento.Value.Date.ToString("yyyy-MM-dd"),
            txtColor.Text.Trim());
    }
    // Consulta y puebla la grilla de mascotas aplicando los filtros de búsqueda exclusiva por nombre de mascota y de especie.
    private async Task CargarMascotasGrillaAsync()
    {
        // Obtiene el texto del filtro de nombre y el ID de la especie seleccionada
        var textoFiltro = txtBuscar.Text.Trim();
        var especieItem = cboFiltroEspecie.SelectedItem as ItemCombo;
        var idEspecie = especieItem?.Id;

        try
        {
            // Bloquea el botón de búsqueda y activa el cursor de espera para feedback de carga
            _cargandoDatos = true;
            btnBuscar.Enabled = false;
            Cursor = Cursors.WaitCursor;

            IEnumerable<MascotaRespuestaDto> listado;

            // Si está inyectado el servicio especializado de búsqueda, lo prioriza
            if (_mascotaService is not null)
            {
                // Realiza la búsqueda en base de datos filtrando exclusivamente por nombre de la mascota y especie
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
                // Consulta todas las mascotas y aplica el filtrado en memoria como alternativa
                var resultado = await _mascotaControlador.ObtenerTodosAsync();
                listado = resultado.EsExitoso && resultado.Valor is not null ? resultado.Valor : [];

                if (idEspecie.HasValue && idEspecie.Value > 0)
                {
                    listado = listado.Where(m => m.IdEspecie == idEspecie.Value);
                }

                if (!string.IsNullOrWhiteSpace(textoFiltro))
                {
                    listado = listado.Where(m =>
                        m.Nombre.Contains(textoFiltro, StringComparison.OrdinalIgnoreCase));
                }
            }
            else
            {
                listado = [];
            }

            // Vuelca los resultados en las filas del DataGridView
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
                    m.Activo ? "Activo" : "Inactivo"); // Indica visualmente el estado del borrado lógico
            }

            // Deselecciona celdas iniciales para mantener el formulario en modo creación
            dgvMascotas.ClearSelection();
            dgvMascotas.CurrentCell = null;
            ActualizarTotalRegistros(); // Actualiza la etiqueta con la cantidad de filas visibles
        }
        finally
        {
            btnBuscar.Enabled = true;
            Cursor = Cursors.Default;
            _cargandoDatos = false;
        }
    }
    // Abre el formulario modal satélite polimórfico en el modo especificado y actualiza los desplegables.
    private async Task AbrirGestionSateliteAsync(ModoGestion modo)
    {
        // Conserva la especie que esté actualmente seleccionada en el combo
        var especieItem = cboEspecie.SelectedItem as ItemCombo;
        var idEspeciePreseleccionada = especieItem?.Id;

        // Instancia el formulario modal pasándole el modo (Alta o Modificación) y los controladores correspondientes
        using var modal = new FormGestionSatelite(
            modo,
            _especieControlador,
            _razaControlador,
            idEspeciePreseleccionada);

        // Si el usuario confirmó la operación (DialogResult.OK), refresca los datos dependientes
        if (modal.ShowDialog(this) == DialogResult.OK)
        {
            // Refresca desplegables de especies
            await InicializarCatalogosAsync();

            // Si había una especie seleccionada previamente, la restablece y recarga sus razas
            if (idEspeciePreseleccionada.HasValue && idEspeciePreseleccionada.Value > 0)
            {
                SeleccionarEnComboPorId(cboEspecie, idEspeciePreseleccionada.Value);
                await cboEspecie_SelectedIndexChangedAsync();
            }

            // Refresca la grilla para reflejar cualquier cambio en nombres de especies o razas
            await CargarMascotasGrillaAsync();
        }
    }
    // Abre la vista de propietarios para alta rápida y vuelve a mascotas sin perder los datos en carga.
    private async void btnNuevoPropietario_Click(object? sender, EventArgs e)
    {
        long? idPropietarioCreado = null;

        // Resuelve la vista de propietarios a través del contenedor de inyección de dependencias
        if (_serviceProvider is not null)
        {
            using var alcance = _serviceProvider.CreateScope();
            using var vistaPropietarios = alcance.ServiceProvider.GetService<FormPropietarios>() ?? new FormPropietarios();
            vistaPropietarios.RetornarAMascotas = true; // Configura el formulario para retorno modal
            vistaPropietarios.ShowDialog(this);
            idPropietarioCreado = vistaPropietarios.UltimoIdCreado; // Recupera el ID del propietario recién creado
        }
        else
        {
            using var vistaPropietarios = new FormPropietarios();
            vistaPropietarios.RetornarAMascotas = true;
            vistaPropietarios.ShowDialog(this);
            idPropietarioCreado = vistaPropietarios.UltimoIdCreado;
        }

        // Recarga el desplegable de propietarios y preselecciona automáticamente el que se acaba de registrar
        await RecargarPropietariosAsync(idPropietarioCreado);
    }
    // Recarga el combo de propietarios sin tocar el resto del formulario en edición/alta.
    private async Task RecargarPropietariosAsync(long? seleccionarId = null)
    {
        var idPrevio = (cboPropietario.SelectedItem as ItemCombo)?.Id;
        var idASeleccionar = seleccionarId ?? idPrevio;

        _cargandoDatos = true;
        try
        {
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
        }
        finally
        {
            _cargandoDatos = false;
        }

        // Si se indicó un ID a seleccionar, lo busca en los ítems y lo marca como seleccionado
        if (idASeleccionar.HasValue && idASeleccionar.Value > 0)
            SeleccionarEnComboPorId(cboPropietario, idASeleccionar.Value);
    }
    // Resalta con color de fondo suave y etiqueta en borgoña el control que falló la validación.
    private static void MarcarError(Control control, Label? etiqueta, string mensaje)
    {
        control.BackColor = ColorFondoError; // Fondo en tono rosa pastel suave

        if (etiqueta is not null)
        {
            etiqueta.ForeColor = ColorEtiquetaError; // Etiqueta en color borgoña
        }

        control.Focus(); // Pone el foco directamente en el campo que causó el error
    }
    // Restablece un control específico a su color de fondo blanco y etiqueta neutral.
    private static void LimpiarErrorControl(Control control, Label? etiqueta)
    {
        control.BackColor = ColorFondoNormal;

        if (etiqueta is not null)
        {
            etiqueta.ForeColor = ColorEtiquetaNormal;
        }
    }

    // Restablece el color de la etiqueta de sexo al color neutral
    private void LimpiarErrorSexo()
    {
        lblSexo.ForeColor = ColorEtiquetaNormal;
    }
    // Restablece todos los campos del formulario a su color de fondo y etiquetas normales.
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
    // Selecciona un elemento dentro de un ComboBox a partir de su ID encapsulado en un ItemCombo.
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
    // Actualiza la etiqueta con el total de registros presentes en la grilla de mascotas.
    public void ActualizarTotalRegistros()
    {
        var total = dgvMascotas.Rows.Count;
        lblTotalRegistros.Text = $"Total de registros: {total}";
    }

    // Manejador del botón volver para cerrar el formulario
    private void btnVolver_Click(object? sender, EventArgs e) => Close();

    // Eventos del DataGridView para mantener sincronizado el contador de filas
    private void dgvMascotas_RowsAdded(object? sender, DataGridViewRowsAddedEventArgs e) => ActualizarTotalRegistros();
    private void dgvMascotas_RowsRemoved(object? sender, DataGridViewRowsRemovedEventArgs e) => ActualizarTotalRegistros();

    // Elemento auxiliar inmutable (record) para vincular identificadores con leyendas en desplegables.
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
    // Actualiza de forma reactiva el estado y colores de los botones de estado (Activar / Desactivar).
    // null = sin selección (ambos deshabilitados en gris).
    // true = registro activo (Desactivar habilitado en borgoña; Activar deshabilitado).
    // false = registro inactivo (Activar habilitado en verde salvia; Desactivar deshabilitado).
    private void ActualizarEstadoBotonesAccion(bool? activo)
    {
        if (!activo.HasValue)
        {
            // Sin selección: ambos botones apagados en gris
            btnActivar.Enabled = false;
            btnActivar.BackColor = ColorBotonDeshabilitado;
            btnActivar.ForeColor = ColorTextoBotonDeshabilitado;

            btnDesactivar.Enabled = false;
            btnDesactivar.BackColor = ColorBotonDeshabilitado;
            btnDesactivar.ForeColor = ColorTextoBotonDeshabilitado;
        }
        else if (activo.Value)
        {
            // Mascota activa: solo se puede Desactivar
            btnActivar.Enabled = false;
            btnActivar.BackColor = ColorBotonDeshabilitado;
            btnActivar.ForeColor = ColorTextoBotonDeshabilitado;

            btnDesactivar.Enabled = true;
            btnDesactivar.BackColor = ColorBotonDesactivarHabilitado;
            btnDesactivar.ForeColor = Color.White;
        }
        else
        {
            // Mascota inactiva: solo se puede Activar (reactivar)
            btnActivar.Enabled = true;
            btnActivar.BackColor = ColorBotonActivarHabilitado;
            btnActivar.ForeColor = Color.White;

            btnDesactivar.Enabled = false;
            btnDesactivar.BackColor = ColorBotonDeshabilitado;
            btnDesactivar.ForeColor = ColorTextoBotonDeshabilitado;
        }
    }

    // Reactiva una mascota dada de baja lógica previa confirmación del usuario
    private async Task btnActivar_Click()
    {
        if (_idMascotaSeleccionada <= 0)
        {
            MessageBox.Show("Debe seleccionar una mascota de la grilla.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        // Cuadro de diálogo de confirmación para evitar reactivaciones accidentales
        var confirmacion = MessageBox.Show(
            "¿Desea reactivar este registro?",
            "Confirmar Activación",
            MessageBoxButtons.YesNo,
            MessageBoxIcon.Question);

        if (confirmacion != DialogResult.Yes)
            return;

        if (_mascotaControlador is null)
            return;

        // Invoca al controlador con activo = true (soft delete invertido)
        var resultado = await _mascotaControlador.CambiarEstadoAsync(_idMascotaSeleccionada, true);
        if (resultado.EsExitoso)
        {
            MessageBox.Show(resultado.Mensaje, "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
            LimpiarFormulario(); // Vuelve al modo creación
            await CargarMascotasGrillaAsync(); // Refresca la grilla con el nuevo estado
        }
        else
        {
            MessageBox.Show(resultado.Mensaje, "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
    }

    // Realiza la baja lógica (Soft Delete) de la mascota sin eliminar físicamente sus datos
    private async Task btnDesactivar_Click()
    {
        if (_idMascotaSeleccionada <= 0)
        {
            MessageBox.Show("Debe seleccionar una mascota de la grilla.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        // Cuadro de diálogo de confirmación para prevenir desactivaciones accidentales
        var confirmacion = MessageBox.Show(
            "¿Está seguro de que desea desactivar este registro?",
            "Confirmar Desactivación",
            MessageBoxButtons.YesNo,
            MessageBoxIcon.Question);

        if (confirmacion != DialogResult.Yes)
            return;

        if (_mascotaControlador is null)
            return;

        // Invoca al controlador con activo = false para preservar historial clínico y facturación
        var resultado = await _mascotaControlador.CambiarEstadoAsync(_idMascotaSeleccionada, false);
        if (resultado.EsExitoso)
        {
            MessageBox.Show(resultado.Mensaje, "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
            LimpiarFormulario(); // Regresa al modo creación
            await CargarMascotasGrillaAsync(); // Refresca la grilla reflejando el estado inactivo
        }
        else
        {
            MessageBox.Show(resultado.Mensaje, "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
    }
}
