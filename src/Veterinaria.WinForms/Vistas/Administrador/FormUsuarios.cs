using System.Net.Mail;
using System.Text.RegularExpressions;
using Veterinaria.Controllers.Controladores;
using Veterinaria.Domain.Dtos;
using Veterinaria.WinForms.Sesion;

namespace Veterinaria.WinForms.Vistas.Administrador;

/// <summary>
/// ABM de usuarios: alta, baja lógica y modificación.
/// </summary>
public partial class FormUsuarios : Form
{
    private static readonly Regex SoloDigitos = new(@"^\d+$", RegexOptions.Compiled);
    private static readonly Regex TelefonoValido = new(@"^[\d\s\+\-\(\)]+$", RegexOptions.Compiled);
    private static readonly Color ColorEtiquetaNormal = Color.FromArgb(58, 53, 59);
    private static readonly Color ColorError = Color.FromArgb(178, 34, 34);

    private readonly ErrorProvider _errores = new();
    private readonly UsuarioControlador _usuarioControlador;
    private readonly TipoUsuarioControlador _tipoUsuarioControlador;

    private long? _idSeleccionado;
    private List<UsuarioRespuestaDto> _usuarios = [];

    public FormUsuarios(
        UsuarioControlador usuarioControlador,
        TipoUsuarioControlador tipoUsuarioControlador)
    {
        _usuarioControlador = usuarioControlador;
        _tipoUsuarioControlador = tipoUsuarioControlador;
        InitializeComponent();
        ConfigurarValidacionVisual();
    }

    private void ConfigurarValidacionVisual()
    {
        _errores.ContainerControl = this;
        _errores.BlinkStyle = ErrorBlinkStyle.NeverBlink;

        lblErrorValidacion.ForeColor = ColorError;
        lblErrorValidacion.Visible = false;

        foreach (Control control in new Control[]
                 {
                     txtNombre, txtApellido, txtDni, txtDireccion, txtTelefono,
                     txtCorreoElectronico, txtUsuario, txtContrasena,
                     dtpFechaNacimiento, rbHombre, rbMujer, cboPerfil
                 })
        {
            control.Enter += (_, _) => LimpiarErrorDe(control);
            if (control is TextBox texto)
                texto.TextChanged += (_, _) => LimpiarErrorDe(texto);
            if (control is ComboBox combo)
                combo.SelectedIndexChanged += (_, _) => LimpiarErrorDe(combo);
            if (control is DateTimePicker fecha)
                fecha.ValueChanged += (_, _) => LimpiarErrorDe(fecha);
            if (control is RadioButton radio)
                radio.CheckedChanged += (_, _) =>
                {
                    LimpiarErrorDe(rbHombre);
                    LimpiarErrorDe(rbMujer);
                    lblSexo.ForeColor = ColorEtiquetaNormal;
                };
        }
    }

    private async void FormUsuarios_Load(object? sender, EventArgs e)
    {
        lblUsuarioSesion.Text = SesionActual.EstaAutenticado
            ? $"Usuario: {SesionActual.NombreCompleto} | Rol: {SesionActual.Rol}"
            : "Usuario: Administrador";

        await CargarPerfilesAsync();
        await CargarUsuariosAsync();
        LimpiarFormulario();
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

        cboPerfil.DisplayMember = "Nombre";
        cboPerfil.ValueMember = "Id";
        cboPerfil.DataSource = resultado.Valor
            .Where(t => t.Activo)
            .ToList();
        cboPerfil.SelectedIndex = -1;
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
                usuario.DNI,
                usuario.Telefono ?? string.Empty,
                usuario.CorreoElectronico ?? string.Empty,
                usuario.Direccion ?? string.Empty,
                usuario.Activo ? "Activo" : "Inactivo");
        }
    }

    private void btnNuevo_Click(object? sender, EventArgs e)
    {
        LimpiarFormulario();
        txtNombre.Focus();
    }

    private async void btnGuardar_Click(object? sender, EventArgs e)
    {
        // Si hay un usuario seleccionado, Guardar actualiza; si no, crea uno nuevo.
        if (_idSeleccionado is not null)
        {
            await GuardarCambiosUsuarioAsync(esAlta: false);
            return;
        }

        await GuardarCambiosUsuarioAsync(esAlta: true);
    }

    private async void btnModificar_Click(object? sender, EventArgs e)
    {
        if (_idSeleccionado is null)
        {
            MessageBox.Show(
                "Seleccione un usuario de la lista.",
                "Modificar usuario",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
            return;
        }

        await GuardarCambiosUsuarioAsync(esAlta: false);
    }

    private async Task GuardarCambiosUsuarioAsync(bool esAlta)
    {
        if (!ValidarCampos(esAlta))
            return;

        var solicitud = ArmarSolicitud();
        var titulo = esAlta ? "Alta de usuario" : "Modificar usuario";
        long idGuardado;

        if (esAlta)
        {
            var resultadoAlta = await _usuarioControlador.CrearAsync(solicitud);
            if (!resultadoAlta.EsExitoso || resultadoAlta.Valor <= 0)
            {
                MessageBox.Show(resultadoAlta.Mensaje, titulo, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            idGuardado = resultadoAlta.Valor;
            MessageBox.Show("Usuario creado correctamente.", titulo, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        else
        {
            var resultadoMod = await _usuarioControlador.ActualizarAsync(_idSeleccionado!.Value, solicitud);
            if (!resultadoMod.EsExitoso)
            {
                MessageBox.Show(resultadoMod.Mensaje, titulo, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            idGuardado = _idSeleccionado.Value;
            MessageBox.Show("Usuario modificado correctamente.", titulo, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        await CargarUsuariosAsync();
        CargarUsuarioEnFormulario(idGuardado);
    }

    private void CargarUsuarioEnFormulario(long id)
    {
        var usuario = _usuarios.FirstOrDefault(u => u.Id == id);
        if (usuario is null)
        {
            LimpiarFormulario();
            return;
        }

        _idSeleccionado = usuario.Id;
        txtNombre.Text = usuario.Nombre;
        txtApellido.Text = usuario.Apellido;
        txtDni.Text = usuario.DNI;
        txtDireccion.Text = usuario.Direccion ?? string.Empty;
        txtTelefono.Text = usuario.Telefono ?? string.Empty;
        txtCorreoElectronico.Text = usuario.CorreoElectronico ?? string.Empty;
        txtUsuario.Text = usuario.NombreUsuario;
        txtContrasena.Text = string.Empty;

        if (usuario.FechaNacimiento.HasValue)
            dtpFechaNacimiento.Value = usuario.FechaNacimiento.Value;
        else
            dtpFechaNacimiento.Value = DateTime.Today.AddYears(-18);

        rbHombre.Checked = string.Equals(usuario.Sexo, "Hombre", StringComparison.OrdinalIgnoreCase);
        rbMujer.Checked = string.Equals(usuario.Sexo, "Mujer", StringComparison.OrdinalIgnoreCase);

        if (cboPerfil.DataSource is not null)
            cboPerfil.SelectedValue = usuario.IdTipoUsuario;

        foreach (DataGridViewRow fila in dgvUsuarios.Rows)
        {
            if (fila.Cells[0].Value is not null && Convert.ToInt64(fila.Cells[0].Value) == id)
            {
                fila.Selected = true;
                dgvUsuarios.CurrentCell = fila.Cells[1];
                break;
            }
        }
    }

    private async void btnDesactivar_Click(object? sender, EventArgs e)
    {
        if (_idSeleccionado is null)
        {
            MessageBox.Show(
                "Seleccione un usuario de la lista.",
                "Baja de usuario",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
            return;
        }

        var confirmar = MessageBox.Show(
            "¿Desea desactivar el usuario seleccionado?",
            "Baja de usuario",
            MessageBoxButtons.YesNo,
            MessageBoxIcon.Question);

        if (confirmar != DialogResult.Yes)
            return;

        var resultado = await _usuarioControlador.EliminarAsync(_idSeleccionado.Value);
        if (!resultado.EsExitoso)
        {
            MessageBox.Show(
                resultado.Mensaje,
                "Baja de usuario",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning);
            return;
        }

        MessageBox.Show(
            "Usuario desactivado correctamente.",
            "Baja de usuario",
            MessageBoxButtons.OK,
            MessageBoxIcon.Information);

        await CargarUsuariosAsync();
        LimpiarFormulario();
    }

    private void btnCancelar_Click(object? sender, EventArgs e)
    {
        LimpiarFormulario();
    }

    private void btnBuscar_Click(object? sender, EventArgs e)
    {
        var texto = txtBuscar.Text.Trim();
        if (string.IsNullOrWhiteSpace(texto))
        {
            MostrarUsuariosEnGrilla(_usuarios);
            return;
        }

        var filtrados = _usuarios.Where(u =>
            u.Nombre.Contains(texto, StringComparison.OrdinalIgnoreCase) ||
            u.Apellido.Contains(texto, StringComparison.OrdinalIgnoreCase) ||
            u.NombreUsuario.Contains(texto, StringComparison.OrdinalIgnoreCase) ||
            u.NombreTipoUsuario.Contains(texto, StringComparison.OrdinalIgnoreCase) ||
            u.DNI.Contains(texto, StringComparison.OrdinalIgnoreCase) ||
            (u.CorreoElectronico?.Contains(texto, StringComparison.OrdinalIgnoreCase) ?? false) ||
            (u.Direccion?.Contains(texto, StringComparison.OrdinalIgnoreCase) ?? false) ||
            (u.Telefono?.Contains(texto, StringComparison.OrdinalIgnoreCase) ?? false));

        MostrarUsuariosEnGrilla(filtrados);
        LimpiarFormularioSinTocarGrilla();
    }

    private void btnActivos_Click(object? sender, EventArgs e)
    {
        MostrarUsuariosEnGrilla(_usuarios.Where(u => u.Activo));
        LimpiarFormularioSinTocarGrilla();
    }

    private void btnInactivos_Click(object? sender, EventArgs e)
    {
        MostrarUsuariosEnGrilla(_usuarios.Where(u => !u.Activo));
        LimpiarFormularioSinTocarGrilla();
    }

    private void btnVolver_Click(object? sender, EventArgs e)
    {
        Close();
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

        _idSeleccionado = usuario.Id;
        CargarUsuarioEnFormulario(usuario.Id);
    }

    private UsuarioSolicitudDto ArmarSolicitud()
    {
        string? sexo = null;
        if (rbHombre.Checked)
            sexo = "Hombre";
        else if (rbMujer.Checked)
            sexo = "Mujer";

        return new UsuarioSolicitudDto
        {
            IdTipoUsuario = Convert.ToInt64(cboPerfil.SelectedValue),
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
            Matricula = null
        };
    }

    private bool ValidarCampos(bool esAlta)
    {
        LimpiarErroresValidacion();

        if (string.IsNullOrWhiteSpace(txtNombre.Text))
            return MostrarValidacion("Ingrese el nombre.", txtNombre, lblNombre);

        if (string.IsNullOrWhiteSpace(txtApellido.Text))
            return MostrarValidacion("Ingrese el apellido.", txtApellido, lblApellido);

        var dni = txtDni.Text.Trim();
        if (string.IsNullOrWhiteSpace(dni))
            return MostrarValidacion("Ingrese el DNI.", txtDni, lblDni);

        if (!SoloDigitos.IsMatch(dni) || dni.Length is < 7 or > 8)
            return MostrarValidacion("El DNI debe tener 7 u 8 dígitos numéricos.", txtDni, lblDni);

        var direccion = txtDireccion.Text.Trim();
        if (string.IsNullOrWhiteSpace(direccion))
            return MostrarValidacion("Ingrese la dirección.", txtDireccion, lblDireccion);

        var telefono = txtTelefono.Text.Trim();
        if (string.IsNullOrWhiteSpace(telefono))
            return MostrarValidacion("Ingrese el teléfono.", txtTelefono, lblTelefono);

        if (!TelefonoValido.IsMatch(telefono) || ContarDigitos(telefono) < 8)
            return MostrarValidacion("El teléfono debe tener al menos 8 dígitos.", txtTelefono, lblTelefono);

        var correo = txtCorreoElectronico.Text.Trim();
        if (string.IsNullOrWhiteSpace(correo))
            return MostrarValidacion("Ingrese el correo electrónico.", txtCorreoElectronico, lblCorreoElectronico);

        if (!EsCorreoValido(correo))
            return MostrarValidacion("Ingrese un correo electrónico válido.", txtCorreoElectronico, lblCorreoElectronico);

        if (string.IsNullOrWhiteSpace(txtUsuario.Text))
            return MostrarValidacion("Ingrese el usuario.", txtUsuario, lblUsuario);

        var contrasena = txtContrasena.Text;
        if (esAlta && string.IsNullOrWhiteSpace(contrasena))
            return MostrarValidacion("Ingrese la contraseña.", txtContrasena, lblContrasena);

        if (!string.IsNullOrWhiteSpace(contrasena) && contrasena.Trim().Length < 6)
            return MostrarValidacion("La contraseña debe tener al menos 6 caracteres.", txtContrasena, lblContrasena);

        if (dtpFechaNacimiento.Value.Date > DateTime.Today)
            return MostrarValidacion("La fecha de nacimiento no puede ser futura.", dtpFechaNacimiento, lblFechaNacimiento);

        var edad = CalcularEdad(dtpFechaNacimiento.Value.Date);
        if (edad < 16)
            return MostrarValidacion("El usuario debe tener al menos 16 años.", dtpFechaNacimiento, lblFechaNacimiento);

        if (!rbHombre.Checked && !rbMujer.Checked)
            return MostrarValidacion("Seleccione el sexo.", rbHombre, lblSexo);

        if (cboPerfil.SelectedIndex < 0 || cboPerfil.SelectedValue is null)
            return MostrarValidacion("Seleccione un perfil.", cboPerfil, lblPerfil);

        return true;
    }

    private bool MostrarValidacion(string mensaje, Control control, Label? etiqueta = null)
    {
        _errores.SetError(control, mensaje);
        if (etiqueta is not null)
            etiqueta.ForeColor = ColorError;

        lblErrorValidacion.Text = mensaje;
        lblErrorValidacion.Visible = true;
        control.Focus();
        return false;
    }

    private void LimpiarErrorDe(Control control)
    {
        _errores.SetError(control, string.Empty);

        Label? etiqueta = control switch
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
            _ => null
        };

        if (etiqueta is not null)
            etiqueta.ForeColor = ColorEtiquetaNormal;

        if (!HayErroresActivos())
        {
            lblErrorValidacion.Visible = false;
            lblErrorValidacion.Text = string.Empty;
        }
    }

    private bool HayErroresActivos()
    {
        foreach (Control control in grpDatos.Controls)
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

        foreach (var etiqueta in new[]
                 {
                     lblNombre, lblApellido, lblDni, lblDireccion, lblTelefono,
                     lblCorreoElectronico, lblUsuario, lblContrasena,
                     lblFechaNacimiento, lblSexo, lblPerfil
                 })
        {
            etiqueta.ForeColor = ColorEtiquetaNormal;
        }
    }

    private static bool EsCorreoValido(string correo)
    {
        try
        {
            var direccion = new MailAddress(correo);
            return string.Equals(direccion.Address, correo, StringComparison.OrdinalIgnoreCase)
                   && correo.Contains('.', StringComparison.Ordinal);
        }
        catch (FormatException)
        {
            return false;
        }
    }

    private static int ContarDigitos(string valor) => valor.Count(char.IsDigit);

    private static int CalcularEdad(DateTime fechaNacimiento)
    {
        var hoy = DateTime.Today;
        var edad = hoy.Year - fechaNacimiento.Year;
        if (fechaNacimiento.Date > hoy.AddYears(-edad))
            edad--;
        return edad;
    }

    private void LimpiarFormulario()
    {
        LimpiarFormularioSinTocarGrilla();
        dgvUsuarios.ClearSelection();
    }

    private void LimpiarFormularioSinTocarGrilla()
    {
        LimpiarErroresValidacion();
        _idSeleccionado = null;
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
    }
}
