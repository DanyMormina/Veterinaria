using System.Text.RegularExpressions;
using Veterinaria.Controllers.Controladores;
using Veterinaria.Domain.Comunes;
using Veterinaria.Domain.Dtos;
using Veterinaria.WinForms.Sesion;

namespace Veterinaria.WinForms.Vistas.Secretario;

/// <summary>
/// Formulario de gestión y consulta de propietarios para el rol Secretario / Recepción.
/// Cumple con las especificaciones de diseño "Ejecutivo Romántico Pastel" y validaciones defensivas en tiempo real.
/// </summary>
public partial class FormPropietarios : Form
{
    // =========================================================================
    // Alias de conveniencia para campos de texto según requerimientos
    // =========================================================================
    private TextBox txtDNI => txtDni;
    private TextBox txtCorreo => txtCorreoElectronico;

    // =========================================================================
    // Expresiones regulares compiladas para restricciones y validación
    // =========================================================================
    private static readonly Regex RegexNombreApellido = new(@"^[a-zA-ZáéíóúÁÉÍÓÚñÑüÜ\s]{2,100}$", RegexOptions.Compiled);
    private static readonly Regex RegexDni = new(@"^\d{7,8}$", RegexOptions.Compiled);
    private static readonly Regex RegexTelefono = new(@"^\d{6,13}$", RegexOptions.Compiled);

    // =========================================================================
    // Paleta de colores oficial: "Ejecutivo Romántico Pastel" (GEMINI.md)
    // =========================================================================
    private static readonly Color ColorBordeError = ColorTranslator.FromHtml("#B85D69");
    private static readonly Color ColorBordeNeutro = ColorTranslator.FromHtml("#E2D9DC");
    private static readonly Color ColorFondoError = ColorTranslator.FromHtml("#FDECEF");
    private static readonly Color ColorFondoNormal = Color.White;
    private static readonly Color ColorEtiquetaNormal = ColorTranslator.FromHtml("#3A353B");
    private static readonly Color ColorEtiquetaError = ColorTranslator.FromHtml("#B85D69");

    // =========================================================================
    // Dependencias y estado del formulario
    // =========================================================================
    private readonly PropietarioControlador? _propietarioControlador;
    private readonly PropietarioService? _propietarioService;

    private Control[] _controlesEntrada = [];
    private long? _idPropietarioSeleccionado = null;
    private bool _cargandoDatos = false;
    private bool _limpiando = false;

    /// <summary>
    /// Constructor por defecto requerido por el diseñador de Windows Forms.
    /// </summary>
    public FormPropietarios() : this(null, null)
    {
    }

    /// <summary>
    /// Constructor con inyección de dependencias de controladores y servicios de propietarios.
    /// </summary>
    public FormPropietarios(
        PropietarioControlador? propietarioControlador,
        PropietarioService? propietarioService = null)
    {
        _propietarioControlador = propietarioControlador;
        _propietarioService = propietarioService ?? (propietarioControlador as PropietarioService);

        InitializeComponent();

        ConfigurarValidacionVisual();
        ConfigurarRestriccionesTeclado();
        ConfigurarEventos();
    }

    /// <summary>
    /// Configura los eventos de interacción del formulario y sus botones de acción.
    /// </summary>
    private void ConfigurarEventos()
    {
        btnGuardar.Click += async (_, _) => await GuardarPropietarioAsync();
        btnModificar.Click += async (_, _) => await ModificarPropietarioAsync();
        btnLimpiar.Click += (_, _) => LimpiarFormulario();
        btnBuscar.Click += async (_, _) => await BuscarPropietariosAsync();

        txtBuscar.KeyDown += async (_, e) =>
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;
                await BuscarPropietariosAsync();
            }
        };

        dgvPropietarios.CellClick += (_, _) => CargarPropietarioSeleccionado();
        dgvPropietarios.SelectionChanged += (_, _) => CargarPropietarioSeleccionado();
    }

    /// <summary>
    /// Configura las restricciones de entrada de teclado en tiempo real (KeyPress).
    /// </summary>
    private void ConfigurarRestriccionesTeclado()
    {
        // 1. txtNombre y txtApellido: permitir ÚNICAMENTE letras, espacios y vocales con acento
        var validarSoloTexto = new KeyPressEventHandler((sender, e) =>
        {
            if (char.IsControl(e.KeyChar))
                return;

            const string letrasAcentuadas = "áéíóúÁÉÍÓÚñÑüÜ";
            if (char.IsLetter(e.KeyChar) || letrasAcentuadas.Contains(e.KeyChar))
                return;

            // Espacio simple permitido si no es al principio y el último carácter no es espacio
            if (e.KeyChar == ' ' && sender is TextBox txt && !string.IsNullOrEmpty(txt.Text) && !txt.Text.EndsWith(' '))
                return;

            e.Handled = true;
        });

        txtNombre.KeyPress += validarSoloTexto;
        txtApellido.KeyPress += validarSoloTexto;

        // 2. txtDNI y txtTelefono: permitir ÚNICAMENTE dígitos numéricos y teclas de control
        var validarSoloDigitos = new KeyPressEventHandler((_, e) =>
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        });

        txtDNI.KeyPress += validarSoloDigitos;
        txtTelefono.KeyPress += validarSoloDigitos;

        // 3. txtCorreo: bloquear espacios en blanco en tiempo real
        txtCorreo.KeyPress += (_, e) =>
        {
            if (char.IsWhiteSpace(e.KeyChar))
            {
                e.Handled = true;
            }
        };
    }

    /// <summary>
    /// Vincula los eventos Enter y TextChanged a los controles de entrada para restaurar
    /// dinámicamente los bordes neutrales (#E2D9DC) y fondos blancos al interactuar.
    /// </summary>
    private void ConfigurarValidacionVisual()
    {
        _controlesEntrada = [txtDNI, txtNombre, txtApellido, txtTelefono, txtCorreo, txtDireccion];

        foreach (var control in _controlesEntrada)
        {
            control.Enter += (_, _) => LimpiarErrorDeControl(control);
            if (control is TextBox txt)
            {
                txt.TextChanged += (_, _) => LimpiarErrorDeControl(control);
            }
        }
    }

    private async void FormPropietarios_Load(object? sender, EventArgs e)
    {
        lblUsuarioSesion.Text = SesionActual.EstaAutenticado
            ? $"Recepción: {SesionActual.NombreCompleto} | {SesionActual.Rol}"
            : "Recepción: Secretario";

        // Estado inicial de los botones: Modo Creación
        btnGuardar.Enabled = true;
        btnModificar.Enabled = false;

        await CargarPropietariosGrillaAsync();
    }

    /// <summary>
    /// Valida integralmente los campos del formulario conforme a las especificaciones de negocio.
    /// Retorna un Resultado indicando el éxito o el mensaje descriptivo junto con el control que falló.
    /// </summary>
    public Resultado ValidarCampos(out Control? controlConFoco)
    {
        RestablecerBordes();

        // 1. DNI: Requerido, únicamente dígitos, entre 7 y 8 caracteres
        var dni = txtDNI.Text.Trim();
        if (string.IsNullOrWhiteSpace(dni))
        {
            controlConFoco = txtDNI;
            return Resultado.Falla("El DNI es obligatorio.");
        }

        if (!RegexDni.IsMatch(dni))
        {
            controlConFoco = txtDNI;
            return Resultado.Falla("El DNI debe contener entre 7 y 8 dígitos numéricos.");
        }

        // 2. Nombre: Requerido, entre 2 y 100 caracteres, solo letras, tildes y espacios
        var nombre = txtNombre.Text.Trim();
        if (string.IsNullOrWhiteSpace(nombre) || nombre.Length < 2 || nombre.Length > 100)
        {
            controlConFoco = txtNombre;
            return Resultado.Falla("El nombre es obligatorio y debe tener entre 2 y 100 caracteres.");
        }

        if (!RegexNombreApellido.IsMatch(nombre))
        {
            controlConFoco = txtNombre;
            return Resultado.Falla("El nombre solo debe contener letras, tildes y espacios.");
        }

        // 3. Apellido: Requerido, entre 2 y 100 caracteres, solo letras, tildes y espacios
        var apellido = txtApellido.Text.Trim();
        if (string.IsNullOrWhiteSpace(apellido) || apellido.Length < 2 || apellido.Length > 100)
        {
            controlConFoco = txtApellido;
            return Resultado.Falla("El apellido es obligatorio y debe tener entre 2 y 100 caracteres.");
        }

        if (!RegexNombreApellido.IsMatch(apellido))
        {
            controlConFoco = txtApellido;
            return Resultado.Falla("El apellido solo debe contener letras, tildes y espacios.");
        }

        // 4. Teléfono: Requerido, numérico entre 6 y 13 dígitos
        var telefono = txtTelefono.Text.Trim();
        if (string.IsNullOrWhiteSpace(telefono))
        {
            controlConFoco = txtTelefono;
            return Resultado.Falla("El número de teléfono es obligatorio.");
        }

        if (!RegexTelefono.IsMatch(telefono))
        {
            controlConFoco = txtTelefono;
            return Resultado.Falla("El número de teléfono debe contener entre 6 y 13 dígitos numéricos.");
        }

        // 5. Correo Electrónico: Requerido y formato válido con verificación estricta de dominios
        var correo = txtCorreo.Text.Trim();
        var errorCorreo = UsuarioControlador.ValidarFormatoCorreo(correo);
        if (errorCorreo is not null)
        {
            controlConFoco = txtCorreo;
            return Resultado.Falla(errorCorreo);
        }

        // 6. Dirección: Requerida, entre 3 y 200 caracteres
        var direccion = txtDireccion.Text.Trim();
        if (string.IsNullOrWhiteSpace(direccion) || direccion.Length < 3 || direccion.Length > 200)
        {
            controlConFoco = txtDireccion;
            return Resultado.Falla("La dirección es obligatoria y debe tener entre 3 y 200 caracteres.");
        }

        controlConFoco = null;
        return Resultado.Exito();
    }

    /// <summary>
    /// Sobrecarga booleana de ValidarCampos para compatibilidad y simplicidad en flujos directos.
    /// </summary>
    public bool ValidarCampos(out string mensajeError, out Control? controlConFoco)
    {
        var resultado = ValidarCampos(out controlConFoco);
        mensajeError = resultado.EsExitoso ? string.Empty : resultado.Mensaje;
        return resultado.EsExitoso;
    }

    /// <summary>
    /// Marca visualmente un control que falló la validación resaltando su contenedor en Borgoña suave (#B85D69).
    /// </summary>
    private void MarcarError(Control? control, string mensaje, bool enfocar = true)
    {
        if (control is null) return;

        // Resaltar borde del panel contenedor si existe
        if (control.Parent is Panel pnlContenedor)
        {
            pnlContenedor.BackColor = ColorBordeError;
        }

        control.BackColor = ColorFondoError;

        var etiqueta = ObtenerEtiquetaDeControl(control);
        if (etiqueta is not null)
        {
            etiqueta.ForeColor = ColorEtiquetaError;
        }

        if (enfocar)
        {
            control.Focus();
        }
    }

    /// <summary>
    /// Restablece un control individual a sus colores neutros (#E2D9DC, fondo blanco, texto carbón).
    /// </summary>
    private void LimpiarErrorDeControl(Control control)
    {
        if (control.Parent is Panel pnlContenedor)
        {
            pnlContenedor.BackColor = ColorBordeNeutro;
        }

        control.BackColor = ColorFondoNormal;

        var etiqueta = ObtenerEtiquetaDeControl(control);
        if (etiqueta is not null)
        {
            etiqueta.ForeColor = ColorEtiquetaNormal;
        }
    }

    /// <summary>
    /// Restablece los bordes y fondos de todos los campos de entrada a estado neutral.
    /// </summary>
    private void RestablecerBordes()
    {
        foreach (var control in _controlesEntrada)
        {
            LimpiarErrorDeControl(control);
        }
    }

    /// <summary>
    /// Obtiene la etiqueta asociada a un control de entrada para manipular su color de forma reactiva.
    /// </summary>
    private Label? ObtenerEtiquetaDeControl(Control control) => control switch
    {
        _ when ReferenceEquals(control, txtDni) => lblDni,
        _ when ReferenceEquals(control, txtNombre) => lblNombre,
        _ when ReferenceEquals(control, txtApellido) => lblApellido,
        _ when ReferenceEquals(control, txtTelefono) => lblTelefono,
        _ when ReferenceEquals(control, txtCorreoElectronico) => lblCorreoElectronico,
        _ when ReferenceEquals(control, txtDireccion) => lblDireccion,
        _ => null
    };

    /// <summary>
    /// Identifica el control que generó un conflicto a partir del mensaje devuelto por la capa de negocio.
    /// </summary>
    private Control IdentificarControlDesdeMensaje(string mensaje)
    {
        var msg = mensaje.ToLowerInvariant();
        if (msg.Contains("dni")) return txtDNI;
        if (msg.Contains("teléfono") || msg.Contains("telefono")) return txtTelefono;
        if (msg.Contains("correo") || msg.Contains("email")) return txtCorreo;
        if (msg.Contains("dirección") || msg.Contains("direccion")) return txtDireccion;
        if (msg.Contains("apellido")) return txtApellido;
        if (msg.Contains("nombre")) return txtNombre;
        return txtDNI;
    }

    /// <summary>
    /// Limpia todos los campos, restablece bordes neutrales, deselecciona filas de la grilla,
    /// anula el ID seleccionado, activa el Modo Creación y ubica el foco en txtDNI.
    /// </summary>
    private void LimpiarFormulario()
    {
        _limpiando = true;
        try
        {
            txtDNI.Text = string.Empty;
            txtNombre.Text = string.Empty;
            txtApellido.Text = string.Empty;
            txtTelefono.Text = string.Empty;
            txtCorreo.Text = string.Empty;
            txtDireccion.Text = string.Empty;

            RestablecerBordes();

            dgvPropietarios.ClearSelection();
            _idPropietarioSeleccionado = null;

            btnGuardar.Enabled = true;
            btnModificar.Enabled = false;

            txtDNI.Focus();
        }
        finally
        {
            _limpiando = false;
        }
    }

    /// <summary>
    /// Registra un nuevo propietario en el sistema previa validación de campos y unicidad de contacto.
    /// Algoritmo paso a paso:
    /// 1. Valida campos requeridos y formatos locales (DNI, Nombre, Apellido, Teléfono, Correo, Dirección).
    /// 2. Construye el DTO de solicitud con los datos limpios.
    /// 3. Llama al servicio asíncrono para comprobar unicidad y persistir en base de datos.
    /// 4. Si la operación es exitosa, notifica al usuario, limpia el formulario y recarga la grilla.
    /// </summary>
    private async Task GuardarPropietarioAsync()
    {
        // Paso 1: Validación local
        var validacion = ValidarCampos(out var controlConError);
        if (!validacion.EsExitoso)
        {
            MarcarError(controlConError, validacion.Mensaje);
            MessageBox.Show(validacion.Mensaje, "Validación de Datos", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        // Paso 2: Construir DTO de solicitud
        var dto = new PropietarioSolicitudDto
        {
            DNI = txtDNI.Text.Trim(),
            Nombre = txtNombre.Text.Trim(),
            Apellido = txtApellido.Text.Trim(),
            Telefono = txtTelefono.Text.Trim(),
            CorreoElectronico = txtCorreo.Text.Trim(),
            Direccion = txtDireccion.Text.Trim()
        };

        if (_propietarioService is null && _propietarioControlador is null)
        {
            MessageBox.Show("El servicio de propietarios no se encuentra disponible.", "Error de Sistema", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return;
        }

        try
        {
            btnGuardar.Enabled = false;
            Cursor = Cursors.WaitCursor;

            // Paso 3: Persistir a través de PropietarioService o PropietarioControlador
            var resultado = _propietarioService is not null
                ? await _propietarioService.CrearAsync(dto)
                : await _propietarioControlador!.CrearAsync(dto);

            if (resultado.EsExitoso)
            {
                MessageBox.Show(resultado.Mensaje ?? "Propietario registrado exitosamente.", "Registro Exitoso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LimpiarFormulario();
                await CargarPropietariosGrillaAsync();
            }
            else
            {
                var ctrlFalla = IdentificarControlDesdeMensaje(resultado.Mensaje);
                MarcarError(ctrlFalla, resultado.Mensaje);
                MessageBox.Show(resultado.Mensaje, "Error al Registrar", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        finally
        {
            btnGuardar.Enabled = true;
            Cursor = Cursors.Default;
        }
    }

    /// <summary>
    /// Modifica los datos del propietario actualmente seleccionado en la grilla.
    /// Algoritmo paso a paso:
    /// 1. Verifica que exista un ID de propietario seleccionado válido.
    /// 2. Valida los campos del formulario con ValidarCampos.
    /// 3. Ejecuta la actualización asíncrona validando unicidad excluyendo el registro actual.
    /// 4. Si la actualización es exitosa, notifica, limpia el formulario y actualiza la grilla.
    /// </summary>
    private async Task ModificarPropietarioAsync()
    {
        if (!_idPropietarioSeleccionado.HasValue || _idPropietarioSeleccionado.Value <= 0)
        {
            MessageBox.Show("Debe seleccionar un propietario de la lista para poder modificarlo.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        // Paso 1: Validación local
        var validacion = ValidarCampos(out var controlConError);
        if (!validacion.EsExitoso)
        {
            MarcarError(controlConError, validacion.Mensaje);
            MessageBox.Show(validacion.Mensaje, "Validación de Datos", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        // Paso 2: Construir DTO de solicitud
        var dto = new PropietarioSolicitudDto
        {
            DNI = txtDNI.Text.Trim(),
            Nombre = txtNombre.Text.Trim(),
            Apellido = txtApellido.Text.Trim(),
            Telefono = txtTelefono.Text.Trim(),
            CorreoElectronico = txtCorreo.Text.Trim(),
            Direccion = txtDireccion.Text.Trim()
        };

        try
        {
            btnModificar.Enabled = false;
            Cursor = Cursors.WaitCursor;

            // Paso 3: Actualizar a través del servicio
            var resultado = _propietarioService is not null
                ? await _propietarioService.ActualizarAsync(_idPropietarioSeleccionado.Value, dto)
                : await _propietarioControlador!.ActualizarAsync(_idPropietarioSeleccionado.Value, dto);

            if (resultado.EsExitoso)
            {
                MessageBox.Show(resultado.Mensaje ?? "Propietario actualizado exitosamente.", "Actualización Exitosa", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LimpiarFormulario();
                await CargarPropietariosGrillaAsync();
            }
            else
            {
                var ctrlFalla = IdentificarControlDesdeMensaje(resultado.Mensaje);
                MarcarError(ctrlFalla, resultado.Mensaje);
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
    /// Ejecuta la búsqueda global multi-campo de propietarios en txtBuscar o recarga los activos si está vacío.
    /// </summary>
    private async Task BuscarPropietariosAsync()
    {
        await CargarPropietariosGrillaAsync(txtBuscar.Text.Trim());
    }

    /// <summary>
    /// Consulta asíncronamente los propietarios con AsNoTracking() aplicando el término de búsqueda si existe.
    /// Refresca la grilla dgvPropietarios sin congelar la interfaz de usuario.
    /// </summary>
    private async Task CargarPropietariosGrillaAsync(string? terminoBusqueda = null)
    {
        try
        {
            _cargandoDatos = true;
            btnBuscar.Enabled = false;
            Cursor = Cursors.WaitCursor;

            IEnumerable<PropietarioRespuestaDto> listado;

            if (_propietarioService is not null)
            {
                var res = await _propietarioService.BuscarPropietariosAsync(terminoBusqueda, soloActivos: true);
                listado = res.EsExitoso && res.Valor is not null ? res.Valor : [];
            }
            else if (_propietarioControlador is not null)
            {
                var res = await _propietarioControlador.ObtenerActivosAsync();
                listado = res.EsExitoso && res.Valor is not null ? res.Valor : [];

                if (!string.IsNullOrWhiteSpace(terminoBusqueda))
                {
                    var term = terminoBusqueda.Trim();
                    listado = listado.Where(p =>
                        p.DNI.Contains(term, StringComparison.OrdinalIgnoreCase) ||
                        p.Nombre.Contains(term, StringComparison.OrdinalIgnoreCase) ||
                        p.Apellido.Contains(term, StringComparison.OrdinalIgnoreCase) ||
                        (p.Telefono != null && p.Telefono.Contains(term, StringComparison.OrdinalIgnoreCase)) ||
                        (p.CorreoElectronico != null && p.CorreoElectronico.Contains(term, StringComparison.OrdinalIgnoreCase)) ||
                        (p.Direccion != null && p.Direccion.Contains(term, StringComparison.OrdinalIgnoreCase)));
                }
            }
            else
            {
                listado = [];
            }

            dgvPropietarios.Rows.Clear();

            foreach (var p in listado)
            {
                var rowIndex = dgvPropietarios.Rows.Add(
                    p.Id,
                    p.DNI,
                    p.Nombre,
                    p.Apellido,
                    p.Telefono ?? string.Empty,
                    p.CorreoElectronico ?? string.Empty,
                    p.Direccion ?? string.Empty,
                    p.Activo ? "Activo" : "Inactivo"
                );

                dgvPropietarios.Rows[rowIndex].Tag = p;
            }

            lblInfoEstado.Text = $"Se encontraron {dgvPropietarios.Rows.Count} propietarios activos en el sistema.";
        }
        catch (Exception ex)
        {
            lblInfoEstado.Text = $"Error al cargar propietarios: {ex.Message}";
        }
        finally
        {
            btnBuscar.Enabled = true;
            Cursor = Cursors.Default;
            _cargandoDatos = false;
        }
    }

    /// <summary>
    /// Sincroniza la fila seleccionada en dgvPropietarios poblando los campos de texto superiores,
    /// asignando _idPropietarioSeleccionado y conmutando el formulario al Modo Edición.
    /// </summary>
    private void CargarPropietarioSeleccionado()
    {
        if (_cargandoDatos || _limpiando || dgvPropietarios.CurrentRow is null || dgvPropietarios.CurrentRow.Index < 0)
        {
            return;
        }

        var fila = dgvPropietarios.CurrentRow.Tag as PropietarioRespuestaDto;
        if (fila is null)
        {
            // Fallback leyendo directamente las celdas de la grilla
            var row = dgvPropietarios.CurrentRow;
            if (row.Cells["colId"].Value is object cellVal && long.TryParse(cellVal.ToString(), out var idLeido))
            {
                fila = new PropietarioRespuestaDto
                {
                    Id = idLeido,
                    DNI = row.Cells["colDni"].Value?.ToString() ?? string.Empty,
                    Nombre = row.Cells["colNombre"].Value?.ToString() ?? string.Empty,
                    Apellido = row.Cells["colApellido"].Value?.ToString() ?? string.Empty,
                    Telefono = row.Cells["colTelefono"].Value?.ToString(),
                    CorreoElectronico = row.Cells["colCorreoElectronico"].Value?.ToString(),
                    Direccion = row.Cells["colDireccion"].Value?.ToString(),
                    Activo = true
                };
            }
        }

        if (fila is null) return;

        // 1. Poblar campos superiores con la información del propietario seleccionado
        txtDNI.Text = fila.DNI;
        txtNombre.Text = fila.Nombre;
        txtApellido.Text = fila.Apellido;
        txtTelefono.Text = fila.Telefono ?? string.Empty;
        txtCorreo.Text = fila.Correo ?? string.Empty;
        txtDireccion.Text = fila.Direccion ?? string.Empty;

        // 2. Almacenar identificador de la entidad seleccionada
        _idPropietarioSeleccionado = fila.Id;

        // 3. Conmutar estado al Modo Edición
        btnGuardar.Enabled = false;
        btnModificar.Enabled = true;

        // 4. Limpiar cualquier resaltado de error previo
        RestablecerBordes();
    }

    private void btnVolver_Click(object? sender, EventArgs e)
    {
        Close();
    }
}
