using System.Globalization;
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
        if (!ValidarCampos(esAlta: true))
            return;

        var solicitud = ArmarSolicitud();
        var resultado = await _usuarioControlador.CrearAsync(solicitud);

        if (!resultado.EsExitoso)
        {
            MessageBox.Show(
                resultado.Mensaje,
                "Alta de usuario",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning);
            return;
        }

        MessageBox.Show(
            "Usuario creado correctamente.",
            "Alta de usuario",
            MessageBoxButtons.OK,
            MessageBoxIcon.Information);

        await CargarUsuariosAsync();
        LimpiarFormulario();
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

        if (!ValidarCampos(esAlta: false))
            return;

        var solicitud = ArmarSolicitud();
        var resultado = await _usuarioControlador.ActualizarAsync(_idSeleccionado.Value, solicitud);

        if (!resultado.EsExitoso)
        {
            MessageBox.Show(
                resultado.Mensaje,
                "Modificar usuario",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning);
            return;
        }

        MessageBox.Show(
            "Usuario modificado correctamente.",
            "Modificar usuario",
            MessageBoxButtons.OK,
            MessageBoxIcon.Information);

        await CargarUsuariosAsync();
        LimpiarFormulario();
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
            dtpFechaNacimiento.Value = DateTime.Today;

        rbHombre.Checked = string.Equals(usuario.Sexo, "Hombre", StringComparison.OrdinalIgnoreCase);
        rbMujer.Checked = string.Equals(usuario.Sexo, "Mujer", StringComparison.OrdinalIgnoreCase);

        if (cboPerfil.DataSource is not null)
            cboPerfil.SelectedValue = usuario.IdTipoUsuario;
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
        if (string.IsNullOrWhiteSpace(txtNombre.Text))
        {
            MostrarValidacion("Ingrese el nombre.", txtNombre);
            return false;
        }

        if (string.IsNullOrWhiteSpace(txtApellido.Text))
        {
            MostrarValidacion("Ingrese el apellido.", txtApellido);
            return false;
        }

        var dni = txtDni.Text.Trim();
        if (string.IsNullOrWhiteSpace(dni))
        {
            MostrarValidacion("Ingrese el DNI.", txtDni);
            return false;
        }

        if (!SoloDigitos.IsMatch(dni) || dni.Length is < 7 or > 8)
        {
            MostrarValidacion("El DNI debe tener 7 u 8 dígitos numéricos.", txtDni);
            return false;
        }

        var direccion = txtDireccion.Text.Trim();
        if (string.IsNullOrWhiteSpace(direccion))
        {
            MostrarValidacion("Ingrese la dirección.", txtDireccion);
            return false;
        }

        var telefono = txtTelefono.Text.Trim();
        if (string.IsNullOrWhiteSpace(telefono))
        {
            MostrarValidacion("Ingrese el teléfono.", txtTelefono);
            return false;
        }

        if (!TelefonoValido.IsMatch(telefono) || ContarDigitos(telefono) < 8)
        {
            MostrarValidacion("El teléfono debe tener al menos 8 dígitos.", txtTelefono);
            return false;
        }

        var correo = txtCorreoElectronico.Text.Trim();
        if (string.IsNullOrWhiteSpace(correo))
        {
            MostrarValidacion("Ingrese el correo electrónico.", txtCorreoElectronico);
            return false;
        }

        if (!EsCorreoValido(correo))
        {
            MostrarValidacion("Ingrese un correo electrónico válido.", txtCorreoElectronico);
            return false;
        }

        if (string.IsNullOrWhiteSpace(txtUsuario.Text))
        {
            MostrarValidacion("Ingrese el usuario.", txtUsuario);
            return false;
        }

        var contrasena = txtContrasena.Text;
        if (esAlta && string.IsNullOrWhiteSpace(contrasena))
        {
            MostrarValidacion("Ingrese la contraseña.", txtContrasena);
            return false;
        }

        if (!string.IsNullOrWhiteSpace(contrasena) && contrasena.Trim().Length < 6)
        {
            MostrarValidacion("La contraseña debe tener al menos 6 caracteres.", txtContrasena);
            return false;
        }

        if (dtpFechaNacimiento.Value.Date > DateTime.Today)
        {
            MostrarValidacion("La fecha de nacimiento no puede ser futura.", dtpFechaNacimiento);
            return false;
        }

        var edad = CalcularEdad(dtpFechaNacimiento.Value.Date);
        if (edad < 16)
        {
            MostrarValidacion("El usuario debe tener al menos 16 años.", dtpFechaNacimiento);
            return false;
        }

        if (!rbHombre.Checked && !rbMujer.Checked)
        {
            MostrarValidacion("Seleccione el sexo.", rbHombre);
            return false;
        }

        if (cboPerfil.SelectedIndex < 0 || cboPerfil.SelectedValue is null)
        {
            MostrarValidacion("Seleccione un perfil.", cboPerfil);
            return false;
        }

        return true;
    }

    private static void MostrarValidacion(string mensaje, Control control)
    {
        MessageBox.Show(mensaje, "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        control.Focus();
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
