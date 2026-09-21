using System.Text.RegularExpressions;
using Veterinaria.Controllers.Controladores;
using Veterinaria.Domain.Dtos;
using Veterinaria.WinForms.Sesion;

namespace Veterinaria.WinForms.Vistas.Administrador;

/// <summary>
/// Formulario de Administración de Usuarios: Alta, Baja Lógica, Modificación y Consulta con estilo Ejecutivo Romántico Pastel.
/// </summary>
public partial class FormUsuarios : Form
{
    private static readonly Regex RegexNombreApellido = new(@"^[a-zA-ZáéíóúÁÉÍÓÚñÑüÜ\s]{2,100}$", RegexOptions.Compiled);
    private static readonly Regex SoloDigitos = new(@"^\d+$", RegexOptions.Compiled);
    private static readonly Regex RegexCorreo = new(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,63}$", RegexOptions.Compiled);

    private static readonly Color ColorBordeError = ColorTranslator.FromHtml("#B85D69");
    private static readonly Color ColorBordeNeutro = ColorTranslator.FromHtml("#E2D9DC");
    private static readonly Color ColorFondoError = ColorTranslator.FromHtml("#FDECEF");
    private static readonly Color ColorFondoNormal = Color.White;
    private static readonly Color ColorEtiquetaNormal = ColorTranslator.FromHtml("#3A353B");
    private static readonly Color ColorEtiquetaError = ColorTranslator.FromHtml("#B85D69");

    private readonly ErrorProvider _errores = new();
    private readonly UsuarioControlador _usuarioControlador;
    private readonly TipoUsuarioControlador _tipoUsuarioControlador;

    private long? _idSeleccionado;
    private List<UsuarioRespuestaDto> _usuarios = [];
    private Control[] _controlesEntrada = [];

    public FormUsuarios(
        UsuarioControlador usuarioControlador,
        TipoUsuarioControlador tipoUsuarioControlador)
    {
        _usuarioControlador = usuarioControlador;
        _tipoUsuarioControlador = tipoUsuarioControlador;
        InitializeComponent();
        ConfigurarValidacionVisual();
        ConfigurarEventosEntrada();
    }

    private void ConfigurarEventosEntrada()
    {
        // Restricción de entrada solo para dígitos en DNI
        txtDni.KeyPress += (_, e) =>
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        };

        // Restricción de entrada solo para dígitos en Teléfono (máximo 13 dígitos numéricos)
        txtTelefono.KeyPress += (_, e) =>
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        };

        // Restricción de caracteres especiales en Nombre y Apellido
        var validarLetras = new KeyPressEventHandler((_, e) =>
        {
            if (!char.IsControl(e.KeyChar) && !char.IsLetter(e.KeyChar) && !char.IsWhiteSpace(e.KeyChar))
            {
                e.Handled = true;
            }
        });

        txtNombre.KeyPress += validarLetras;
        txtApellido.KeyPress += validarLetras;

        // Validación de unicidad de usuario en tiempo real (instantánea mientras se escribe)
        txtUsuario.TextChanged += ValidarUnicidadUsuarioEnTiempoReal;

        // Filtros reactivos instantáneos
        txtBuscar.TextChanged += (_, _) => AplicarFiltros();
        cboFiltroRol.SelectedIndexChanged += (_, _) => AplicarFiltros();
        cboFiltroEstado.SelectedIndexChanged += (_, _) => AplicarFiltros();
    }

    private void ConfigurarValidacionVisual()
    {
        _errores.ContainerControl = this;
        _errores.BlinkStyle = ErrorBlinkStyle.NeverBlink;

        lblErrorValidacion.ForeColor = ColorBordeError;
        lblErrorValidacion.Visible = false;

        _controlesEntrada =
        [
            txtNombre, txtApellido, txtDni, txtDireccion, txtTelefono,
            txtCorreoElectronico, txtUsuario, txtContrasena,
            dtpFechaNacimiento, rbHombre, rbMujer, cboPerfil, cboEstado
        ];

        foreach (Control control in new Control[]
                 {
                     txtNombre, txtApellido, txtDni, txtDireccion, txtTelefono,
                     txtCorreoElectronico, txtContrasena,
                     dtpFechaNacimiento, rbHombre, rbMujer, cboPerfil, cboEstado
                 })
        {
            control.Enter += Control_LimpiarError;
            if (control is TextBox texto)
                texto.TextChanged += Control_LimpiarError;
            if (control is ComboBox combo)
                combo.SelectedIndexChanged += Control_LimpiarError;
            if (control is DateTimePicker fecha)
                fecha.ValueChanged += Control_LimpiarError;
            if (control is RadioButton radio)
                radio.CheckedChanged += Control_LimpiarError;
        }

        // txtUsuario maneja su propio foco
        txtUsuario.Enter += Control_LimpiarError;
    }

    private async void FormUsuarios_Load(object? sender, EventArgs e)
    {
        lblUsuarioSesion.Text = SesionActual.EstaAutenticado
            ? $"Usuario: {SesionActual.NombreCompleto} | Rol: {SesionActual.Rol}"
            : "Usuario: Administrador";

        cboFiltroEstado.Items.Clear();
        cboFiltroEstado.Items.AddRange(["(Todos)", "Solo Activos", "Solo Inactivos"]);
        cboFiltroEstado.SelectedIndex = 0;

        cboEstado.Items.Clear();
        cboEstado.Items.AddRange(["Activo", "Inactivo"]);
        cboEstado.SelectedIndex = 0;

        await CargarPerfilesAsync();
        await CargarUsuariosAsync();
        EstablecerEstadoInicial();
    }

    private async Task CargarPerfilesAsync()
    {
        var resultado = await _tipoUsuarioControlador.ObtenerTodosAsync();
        if (!resultado.EsExitoso || resultado.Valor is null)
        {
            MessageBox.Show(
                resultado.Mensaje,
                "Perfiles",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning);
            return;
        }

        var listaPerfiles = resultado.Valor.ToList();

        // Combo del formulario (solo activos)
        cboPerfil.DisplayMember = "Nombre";
        cboPerfil.ValueMember = "Id";
        cboPerfil.DataSource = listaPerfiles.Where(t => t.Activo).ToList();
        cboPerfil.SelectedIndex = -1;

        // Combo de filtro (incluye opción "(Todos los roles)" con Id 0)
        var opcionesFiltroRol = new List<TipoUsuarioRespuestaDto>
        {
            new TipoUsuarioRespuestaDto { Id = 0, Nombre = "(Todos los roles)", Activo = true }
        };
        opcionesFiltroRol.AddRange(listaPerfiles);

        cboFiltroRol.DisplayMember = "Nombre";
        cboFiltroRol.ValueMember = "Id";
        cboFiltroRol.DataSource = opcionesFiltroRol;
        cboFiltroRol.SelectedIndex = 0;
    }

    private async Task CargarUsuariosAsync()
    {
        var resultado = await _usuarioControlador.ObtenerTodosAsync();
        if (!resultado.EsExitoso || resultado.Valor is null)
        {
            MessageBox.Show(
                resultado.Mensaje,
                "Usuarios",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning);
            return;
        }

        _usuarios = resultado.Valor.ToList();
        MostrarUsuariosEnGrilla(_usuarios);
    }

    private void MostrarUsuariosEnGrilla(IEnumerable<UsuarioRespuestaDto> usuarios)
    {
        dgvUsuarios.Rows.Clear();

        foreach (var usuario in usuarios)
        {
            dgvUsuarios.Rows.Add(
                usuario.Id,
                usuario.Nombre,
                usuario.Apellido,
                usuario.NombreTipoUsuario,
                usuario.DNI,
                usuario.Telefono ?? string.Empty,
                usuario.CorreoElectronico ?? string.Empty,
                usuario.Direccion ?? string.Empty,
                usuario.Activo ? "Activo" : "Inactivo");
        }
    }

    // =========================================================================
    // Gestor de Ciclo de Vida y Estados del Formulario (Lifecycle Management)
    // =========================================================================

    private void EstablecerEstadoInicial()
    {
        _idSeleccionado = null;
        LimpiarCamposFormulario();
        HabilitarControlesEntrada(true);
        txtUsuario.Enabled = true;

        lblEstado.Visible = false;
        cboEstado.Visible = false;
        cboEstado.SelectedIndex = 0; // "Activo"
        cboEstado.Enabled = false;

        btnGuardar.Enabled = true;
        btnModificar.Enabled = false;
        btnLimpiar.Enabled = true;

        dgvUsuarios.ClearSelection();
    }

    private void ActivarModoEdicion(UsuarioRespuestaDto usuario)
    {
        _idSeleccionado = usuario.Id;
        LimpiarErroresValidacion();
        HabilitarControlesEntrada(true);

        // En modo edición no se permite cambiar el username por consistencia de auditoría
        txtUsuario.Enabled = false;

        txtNombre.Text = usuario.Nombre;
        txtApellido.Text = usuario.Apellido;
        txtDni.Text = usuario.DNI;
        txtDireccion.Text = usuario.Direccion ?? string.Empty;
        txtTelefono.Text = usuario.Telefono ?? string.Empty;
        txtCorreoElectronico.Text = usuario.CorreoElectronico ?? string.Empty;
        txtUsuario.Text = usuario.NombreUsuario;
        txtContrasena.Text = string.Empty; // Dejar vacía para indicar "sin cambios"

        if (usuario.FechaNacimiento.HasValue)
        {
            var fechaMax = DateTime.Today.AddYears(-18);
            dtpFechaNacimiento.Value = usuario.FechaNacimiento.Value <= fechaMax
                ? usuario.FechaNacimiento.Value
                : fechaMax;
        }
        else
        {
            dtpFechaNacimiento.Value = DateTime.Today.AddYears(-18);
        }

        rbHombre.Checked = string.Equals(usuario.Sexo, "Hombre", StringComparison.OrdinalIgnoreCase);
        rbMujer.Checked = string.Equals(usuario.Sexo, "Mujer", StringComparison.OrdinalIgnoreCase);

        if (cboPerfil.DataSource is not null)
            cboPerfil.SelectedValue = usuario.IdTipoUsuario;

        lblEstado.Visible = true;
        cboEstado.Visible = true;
        cboEstado.SelectedItem = usuario.Activo ? "Activo" : "Inactivo";
        cboEstado.Enabled = true; // Se habilita y muestra únicamente al seleccionar un usuario para modificar

        btnGuardar.Enabled = false;
        btnModificar.Enabled = true;
        btnLimpiar.Enabled = true;
    }

    private void HabilitarControlesEntrada(bool habilitar)
    {
        txtNombre.Enabled = habilitar;
        txtApellido.Enabled = habilitar;
        txtDni.Enabled = habilitar;
        txtDireccion.Enabled = habilitar;
        txtTelefono.Enabled = habilitar;
        txtCorreoElectronico.Enabled = habilitar;
        txtUsuario.Enabled = habilitar;
        txtContrasena.Enabled = habilitar;
        dtpFechaNacimiento.Enabled = habilitar;
        rbHombre.Enabled = habilitar;
        rbMujer.Enabled = habilitar;
        cboPerfil.Enabled = habilitar;
    }

    private void LimpiarCamposFormulario()
    {
        LimpiarErroresValidacion();
        txtNombre.Clear();
        txtApellido.Clear();
        txtDni.Clear();
        txtUsuario.Clear();
        txtContrasena.Clear();
        txtDireccion.Clear();
        txtTelefono.Clear();
        txtCorreoElectronico.Clear();
        dtpFechaNacimiento.Value = DateTime.Today.AddYears(-18);
        rbHombre.Checked = false;
        rbMujer.Checked = false;
        cboPerfil.SelectedIndex = -1;
        lblEstado.Visible = false;
        cboEstado.Visible = false;
        cboEstado.SelectedIndex = 0;
        cboEstado.Enabled = false;
    }

    // =========================================================================
    // Manejo de Acciones (Botones)
    // =========================================================================

    private async void btnGuardar_Click(object? sender, EventArgs e)
    {
        if (!ValidarCampos(esAlta: true, out var mensajeError, out var controlConFoco))
        {
            MarcarError(controlConFoco, mensajeError);
            MessageBox.Show(
                mensajeError,
                "Alta de Usuario",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning);
            return;
        }

        var solicitud = ArmarSolicitud();
        var resultado = await _usuarioControlador.CrearAsync(solicitud);

        if (!resultado.EsExitoso)
        {
            var controlError = IdentificarControlDesdeMensaje(resultado.Mensaje);
            MarcarError(controlError, resultado.Mensaje);
            MessageBox.Show(
                resultado.Mensaje,
                "Alta de Usuario",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning);
            return;
        }

        MessageBox.Show(
            "Usuario registrado exitosamente.",
            "Alta de Usuario",
            MessageBoxButtons.OK,
            MessageBoxIcon.Information);

        await CargarUsuariosAsync();
        EstablecerEstadoInicial();
    }

    private async void btnModificar_Click(object? sender, EventArgs e)
    {
        if (_idSeleccionado is null)
        {
            MessageBox.Show(
                "Seleccione un usuario de la lista.",
                "Modificar Usuario",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
            return;
        }

        if (!ValidarCampos(esAlta: false, out var mensajeError, out var controlConFoco))
        {
            MarcarError(controlConFoco, mensajeError);
            MessageBox.Show(
                mensajeError,
                "Modificar Usuario",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning);
            return;
        }

        var solicitud = ArmarSolicitud();
        var resultado = await _usuarioControlador.ActualizarAsync(_idSeleccionado.Value, solicitud);

        if (!resultado.EsExitoso)
        {
            var controlError = IdentificarControlDesdeMensaje(resultado.Mensaje);
            MarcarError(controlError, resultado.Mensaje);
            MessageBox.Show(
                resultado.Mensaje,
                "Modificar Usuario",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning);
            return;
        }

        MessageBox.Show(
            "Usuario actualizado exitosamente.",
            "Modificar Usuario",
            MessageBoxButtons.OK,
            MessageBoxIcon.Information);

        var idActualizado = _idSeleccionado.Value;
        await CargarUsuariosAsync();

        var usuarioActualizado = _usuarios.FirstOrDefault(u => u.Id == idActualizado);
        if (usuarioActualizado is not null)
            ActivarModoEdicion(usuarioActualizado);
        else
            EstablecerEstadoInicial();
    }

    private void btnLimpiar_Click(object? sender, EventArgs e)
    {
        EstablecerEstadoInicial();
        txtNombre.Focus();
    }

    private void btnCancelar_Click(object? sender, EventArgs e)
    {
        btnLimpiar_Click(sender, e);
    }

    private void btnVolver_Click(object? sender, EventArgs e)
    {
        Close();
    }

    private void btnBuscar_Click(object? sender, EventArgs e)
    {
        AplicarFiltros();
    }

    private void btnLimpiarFiltros_Click(object? sender, EventArgs e)
    {
        txtBuscar.Clear();
        if (cboFiltroRol.Items.Count > 0)
            cboFiltroRol.SelectedIndex = 0;
        if (cboFiltroEstado.Items.Count > 0)
            cboFiltroEstado.SelectedIndex = 0;
        AplicarFiltros();
    }

    private void AplicarFiltros()
    {
        var texto = txtBuscar.Text.Trim();
        long rolSeleccionado = 0;
        if (cboFiltroRol.SelectedValue is not null && long.TryParse(cboFiltroRol.SelectedValue.ToString(), out var rId))
        {
            rolSeleccionado = rId;
        }

        var estadoSeleccionado = cboFiltroEstado.SelectedIndex; // 0 = Todos, 1 = Activos, 2 = Inactivos

        var consulta = _usuarios.AsEnumerable();

        if (rolSeleccionado > 0)
        {
            consulta = consulta.Where(u => u.IdTipoUsuario == rolSeleccionado);
        }

        if (estadoSeleccionado == 1)
        {
            consulta = consulta.Where(u => u.Activo);
        }
        else if (estadoSeleccionado == 2)
        {
            consulta = consulta.Where(u => !u.Activo);
        }

        if (!string.IsNullOrWhiteSpace(texto))
        {
            consulta = consulta.Where(u =>
                u.Nombre.Contains(texto, StringComparison.OrdinalIgnoreCase) ||
                u.Apellido.Contains(texto, StringComparison.OrdinalIgnoreCase) ||
                u.NombreUsuario.Contains(texto, StringComparison.OrdinalIgnoreCase) ||
                u.NombreTipoUsuario.Contains(texto, StringComparison.OrdinalIgnoreCase) ||
                u.DNI.Contains(texto, StringComparison.OrdinalIgnoreCase) ||
                (u.CorreoElectronico?.Contains(texto, StringComparison.OrdinalIgnoreCase) ?? false) ||
                (u.Direccion?.Contains(texto, StringComparison.OrdinalIgnoreCase) ?? false) ||
                (u.Telefono?.Contains(texto, StringComparison.OrdinalIgnoreCase) ?? false));
        }

        MostrarUsuariosEnGrilla(consulta.ToList());
    }



    private void dgvUsuarios_CellClick(object? sender, DataGridViewCellEventArgs e)
    {
        if (e.RowIndex < 0)
            return;

        var fila = dgvUsuarios.Rows[e.RowIndex];
        if (fila.Cells[0].Value is null)
            return;

        var id = Convert.ToInt64(fila.Cells[0].Value);
        var usuario = _usuarios.FirstOrDefault(u => u.Id == id);
        if (usuario is null)
            return;

        ActivarModoEdicion(usuario);
    }

    // =========================================================================
    // Construcción de Solicitud y Pipeline Unificado de Validación
    // =========================================================================

    private UsuarioSolicitudDto ArmarSolicitud()
    {
        string? sexo = null;
        if (rbHombre.Checked)
            sexo = "Hombre";
        else if (rbMujer.Checked)
            sexo = "Mujer";

        return new UsuarioSolicitudDto
        {
            IdTipoUsuario = cboPerfil.SelectedValue is not null ? Convert.ToInt64(cboPerfil.SelectedValue) : 0,
            NombreUsuario = txtUsuario.Text.Trim(),
            Contrasena = txtContrasena.Text,
            Nombre = txtNombre.Text.Trim(),
            Apellido = txtApellido.Text.Trim(),
            DNI = txtDni.Text.Trim(),
            Direccion = txtDireccion.Text.Trim(),
            Telefono = txtTelefono.Text.Trim(),
            CorreoElectronico = txtCorreoElectronico.Text.Trim(),
            FechaNacimiento = dtpFechaNacimiento.Value.Date,
            Sexo = sexo,
            Matricula = null,
            Activo = cboEstado.Visible ? string.Equals(cboEstado.SelectedItem?.ToString(), "Activo", StringComparison.OrdinalIgnoreCase) : true
        };
    }

    private void ValidarUnicidadUsuarioEnTiempoReal(object? sender, EventArgs e)
    {
        var usuario = txtUsuario.Text.Trim();
        if (string.IsNullOrWhiteSpace(usuario))
        {
            LimpiarErrorDeControl(txtUsuario);
            return;
        }

        // Comprobar si el nombre de usuario ya existe en memoria entre los usuarios registrados
        var existe = _usuarios.Any(u =>
            (!_idSeleccionado.HasValue || u.Id != _idSeleccionado.Value) &&
            string.Equals(u.NombreUsuario, usuario, StringComparison.OrdinalIgnoreCase));

        if (existe)
        {
            MarcarError(txtUsuario, $"El nombre de usuario '{usuario}' ya está en uso.", enfocar: false);
        }
        else
        {
            LimpiarErrorDeControl(txtUsuario);
        }
    }

    private bool ValidarCampos(bool esAlta, out string mensajeError, out Control? controlConFoco)
    {
        LimpiarErroresValidacion();

        // 1. Nombre
        var nombre = txtNombre.Text.Trim();
        if (string.IsNullOrWhiteSpace(nombre) || nombre.Length < 2)
        {
            mensajeError = "El nombre es obligatorio y debe tener al menos 2 caracteres.";
            controlConFoco = txtNombre;
            return false;
        }

        if (!RegexNombreApellido.IsMatch(nombre))
        {
            mensajeError = "El nombre solo debe contener letras, tildes y espacios.";
            controlConFoco = txtNombre;
            return false;
        }

        // 2. Apellido
        var apellido = txtApellido.Text.Trim();
        if (string.IsNullOrWhiteSpace(apellido) || apellido.Length < 2)
        {
            mensajeError = "El apellido es obligatorio y debe tener al menos 2 caracteres.";
            controlConFoco = txtApellido;
            return false;
        }

        if (!RegexNombreApellido.IsMatch(apellido))
        {
            mensajeError = "El apellido solo debe contener letras, tildes y espacios.";
            controlConFoco = txtApellido;
            return false;
        }

        // 3. DNI
        var dni = txtDni.Text.Trim();
        if (string.IsNullOrWhiteSpace(dni))
        {
            mensajeError = "El DNI es obligatorio.";
            controlConFoco = txtDni;
            return false;
        }

        if (!SoloDigitos.IsMatch(dni) || dni.Length is < 7 or > 8)
        {
            mensajeError = "El DNI debe tener entre 7 y 8 dígitos numéricos.";
            controlConFoco = txtDni;
            return false;
        }

        // 4. Dirección
        var direccion = txtDireccion.Text.Trim();
        if (string.IsNullOrWhiteSpace(direccion) || direccion.Length < 3)
        {
            mensajeError = "La dirección es obligatoria y debe tener al menos 3 caracteres.";
            controlConFoco = txtDireccion;
            return false;
        }

        // 5. Teléfono (obligatorio, puramente numérico, máximo 13 dígitos)
        var telefono = txtTelefono.Text.Trim();
        if (string.IsNullOrWhiteSpace(telefono))
        {
            mensajeError = "El número de teléfono es obligatorio.";
            controlConFoco = txtTelefono;
            return false;
        }

        if (!SoloDigitos.IsMatch(telefono) || telefono.Length is < 6 or > 13)
        {
            mensajeError = "El número de teléfono debe contener entre 6 y 13 dígitos numéricos.";
            controlConFoco = txtTelefono;
            return false;
        }

        // 6. Correo Electrónico
        var correo = txtCorreoElectronico.Text.Trim();
        var errorCorreo = UsuarioControlador.ValidarFormatoCorreo(correo);
        if (errorCorreo is not null)
        {
            mensajeError = errorCorreo;
            controlConFoco = txtCorreoElectronico;
            return false;
        }

        // 7. Nombre de Usuario
        var usuario = txtUsuario.Text.Trim();
        if (string.IsNullOrWhiteSpace(usuario) || usuario.Length < 4)
        {
            mensajeError = "El nombre de usuario es obligatorio y debe tener al menos 4 caracteres.";
            controlConFoco = txtUsuario;
            return false;
        }

        if (usuario.Any(char.IsWhiteSpace))
        {
            mensajeError = "El nombre de usuario no puede contener espacios en blanco.";
            controlConFoco = txtUsuario;
            return false;
        }

        var existeUsuario = _usuarios.Any(u =>
            (!_idSeleccionado.HasValue || u.Id != _idSeleccionado.Value) &&
            string.Equals(u.NombreUsuario, usuario, StringComparison.OrdinalIgnoreCase));

        if (existeUsuario)
        {
            mensajeError = $"El nombre de usuario '{usuario}' ya está en uso.";
            controlConFoco = txtUsuario;
            return false;
        }

        // 8. Contraseña
        var contrasena = txtContrasena.Text;
        if (esAlta && string.IsNullOrWhiteSpace(contrasena))
        {
            mensajeError = "La contraseña es obligatoria para el alta de usuario.";
            controlConFoco = txtContrasena;
            return false;
        }

        if (esAlta && contrasena.Length < 6)
        {
            mensajeError = "La contraseña debe tener al menos 6 caracteres.";
            controlConFoco = txtContrasena;
            return false;
        }

        if (!esAlta && !string.IsNullOrWhiteSpace(contrasena) && contrasena.Length < 6)
        {
            mensajeError = "La nueva contraseña debe tener al menos 6 caracteres.";
            controlConFoco = txtContrasena;
            return false;
        }

        // 9. Fecha de Nacimiento
        var fechaMax = DateTime.Today.AddYears(-18);
        if (dtpFechaNacimiento.Value.Date > fechaMax)
        {
            mensajeError = "El usuario debe tener al menos 18 años para trabajar legalmente.";
            controlConFoco = dtpFechaNacimiento;
            return false;
        }

        // 10. Sexo
        if (!rbHombre.Checked && !rbMujer.Checked)
        {
            mensajeError = "Debe seleccionar el sexo del usuario.";
            controlConFoco = rbHombre;
            return false;
        }

        // 11. Perfil
        if (cboPerfil.SelectedIndex < 0 || cboPerfil.SelectedValue is null || Convert.ToInt64(cboPerfil.SelectedValue) <= 0)
        {
            mensajeError = "Seleccione un perfil o rol válido.";
            controlConFoco = cboPerfil;
            return false;
        }

        mensajeError = string.Empty;
        controlConFoco = null;
        return true;
    }

    private void MarcarError(Control? control, string mensaje, bool enfocar = true)
    {
        if (control is null) return;

        if (control is TextBox txt)
        {
            txt.BackColor = ColorFondoError;
        }

        var etiqueta = ObtenerEtiquetaDeControl(control);
        if (etiqueta is not null)
        {
            etiqueta.ForeColor = ColorEtiquetaError;
        }

        _errores.SetError(control, mensaje);
        lblErrorValidacion.Text = mensaje;
        lblErrorValidacion.Visible = true;
        if (enfocar)
        {
            control.Focus();
        }
    }

    private void Control_LimpiarError(object? sender, EventArgs e)
    {
        if (sender is Control control)
        {
            LimpiarErrorDeControl(control);
        }
    }

    private void LimpiarErrorDeControl(Control control)
    {
        if (control is TextBox txt)
        {
            txt.BackColor = ColorFondoNormal;
        }

        if (control is RadioButton)
        {
            _errores.SetError(rbHombre, string.Empty);
            _errores.SetError(rbMujer, string.Empty);
            lblSexo.ForeColor = ColorEtiquetaNormal;
        }

        var etiqueta = ObtenerEtiquetaDeControl(control);
        if (etiqueta is not null)
        {
            etiqueta.ForeColor = ColorEtiquetaNormal;
        }

        _errores.SetError(control, string.Empty);

        if (!HayErroresActivos())
        {
            lblErrorValidacion.Visible = false;
            lblErrorValidacion.Text = string.Empty;
        }
    }

    private Control IdentificarControlDesdeMensaje(string mensaje)
    {
        var msg = mensaje.ToLowerInvariant();
        if (msg.Contains("dni"))
            return txtDni;
        if (msg.Contains("nombre de usuario") || msg.Contains("nombreusuario") || msg.Contains("usuario '") || msg.Contains("usuario ya"))
            return txtUsuario;
        if (msg.Contains("correo") || msg.Contains("email"))
            return txtCorreoElectronico;
        if (msg.Contains("teléfono") || msg.Contains("telefono"))
            return txtTelefono;
        if (msg.Contains("rol") || msg.Contains("perfil") || msg.Contains("tipo"))
            return cboPerfil;
        if (msg.Contains("contraseña") || msg.Contains("contrasena"))
            return txtContrasena;
        if (msg.Contains("apellido"))
            return txtApellido;
        if (msg.Contains("nombre"))
            return txtNombre;
        if (msg.Contains("fecha") || msg.Contains("nacimiento") || msg.Contains("edad"))
            return dtpFechaNacimiento;
        if (msg.Contains("sexo"))
            return rbHombre;

        return txtNombre;
    }

    private Label? ObtenerEtiquetaDeControl(Control control) => control switch
    {
        _ when ReferenceEquals(control, txtNombre) => lblNombre,
        _ when ReferenceEquals(control, txtApellido) => lblApellido,
        _ when ReferenceEquals(control, txtDni) => lblDni,
        _ when ReferenceEquals(control, txtDireccion) => lblDireccion,
        _ when ReferenceEquals(control, txtTelefono) => lblTelefono,
        _ when ReferenceEquals(control, txtCorreoElectronico) => lblCorreoElectronico,
        _ when ReferenceEquals(control, txtUsuario) => lblUsuario,
        _ when ReferenceEquals(control, txtContrasena) => lblContrasena,
        _ when ReferenceEquals(control, dtpFechaNacimiento) => lblFechaNacimiento,
        _ when ReferenceEquals(control, rbHombre) || ReferenceEquals(control, rbMujer) => lblSexo,
        _ when ReferenceEquals(control, cboPerfil) => lblPerfil,
        _ when ReferenceEquals(control, cboEstado) => lblEstado,
        _ => null
    };

    private bool HayErroresActivos()
    {
        foreach (var control in _controlesEntrada)
        {
            if (!string.IsNullOrEmpty(_errores.GetError(control)))
                return true;
        }

        return false;
    }

    private void LimpiarErroresValidacion()
    {
        _errores.Clear();
        lblErrorValidacion.Visible = false;
        lblErrorValidacion.Text = string.Empty;

        txtNombre.BackColor = ColorFondoNormal;
        txtApellido.BackColor = ColorFondoNormal;
        txtDni.BackColor = ColorFondoNormal;
        txtDireccion.BackColor = ColorFondoNormal;
        txtTelefono.BackColor = ColorFondoNormal;
        txtCorreoElectronico.BackColor = ColorFondoNormal;
        txtUsuario.BackColor = ColorFondoNormal;
        txtContrasena.BackColor = ColorFondoNormal;

        foreach (var etiqueta in new[]
                 {
                     lblNombre, lblApellido, lblDni, lblDireccion, lblTelefono,
                     lblCorreoElectronico, lblUsuario, lblContrasena,
                     lblFechaNacimiento, lblSexo, lblPerfil, lblEstado
                 })
        {
            etiqueta.ForeColor = ColorEtiquetaNormal;
        }
    }
}

