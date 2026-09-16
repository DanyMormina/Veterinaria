using Microsoft.Extensions.DependencyInjection;
using Veterinaria.Controllers.Controladores;
using Veterinaria.Domain.Dtos;
using Veterinaria.WinForms.Sesion;

namespace Veterinaria.WinForms.Vistas.Administrador;

/// <summary>
/// ABM de usuarios: alta, baja lógica y modificación.
/// </summary>
public partial class FormUsuarios : Form
{
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
                usuario.NombreUsuario,
                usuario.NombreTipoUsuario,
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
            u.DNI.Contains(texto, StringComparison.OrdinalIgnoreCase));

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
        txtUsuario.Text = usuario.NombreUsuario;
        txtContrasena.Text = string.Empty;

        if (cboPerfil.DataSource is not null)
            cboPerfil.SelectedValue = usuario.IdTipoUsuario;
    }

    private UsuarioSolicitudDto ArmarSolicitud()
    {
        return new UsuarioSolicitudDto
        {
            IdTipoUsuario = Convert.ToInt64(cboPerfil.SelectedValue),
            NombreUsuario = txtUsuario.Text.Trim(),
            Contrasena = txtContrasena.Text,
            Nombre = txtNombre.Text.Trim(),
            Apellido = txtApellido.Text.Trim(),
            DNI = txtDni.Text.Trim(),
            Matricula = null
        };
    }

    private bool ValidarCampos(bool esAlta)
    {
        if (string.IsNullOrWhiteSpace(txtNombre.Text))
        {
            MessageBox.Show("Ingrese el nombre.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            txtNombre.Focus();
            return false;
        }

        if (string.IsNullOrWhiteSpace(txtApellido.Text))
        {
            MessageBox.Show("Ingrese el apellido.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            txtApellido.Focus();
            return false;
        }

        if (string.IsNullOrWhiteSpace(txtDni.Text))
        {
            MessageBox.Show("Ingrese el DNI.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            txtDni.Focus();
            return false;
        }

        if (string.IsNullOrWhiteSpace(txtUsuario.Text))
        {
            MessageBox.Show("Ingrese el usuario.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            txtUsuario.Focus();
            return false;
        }

        if (cboPerfil.SelectedIndex < 0 || cboPerfil.SelectedValue is null)
        {
            MessageBox.Show("Seleccione un perfil.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            cboPerfil.Focus();
            return false;
        }

        if (esAlta && string.IsNullOrWhiteSpace(txtContrasena.Text))
        {
            MessageBox.Show("Ingrese la contraseña.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            txtContrasena.Focus();
            return false;
        }

        return true;
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
        rbHombre.Checked = false;
        rbMujer.Checked = false;
        cboPerfil.SelectedIndex = -1;
    }
}
