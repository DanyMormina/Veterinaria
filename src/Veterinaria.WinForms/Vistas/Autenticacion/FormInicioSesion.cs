using System.Runtime.InteropServices;
using Microsoft.Extensions.DependencyInjection;
using Veterinaria.Controllers.Controladores;
using Veterinaria.WinForms.Sesion;
using Veterinaria.WinForms.Vistas.Administrador;
using Veterinaria.WinForms.Vistas.Secretario;
using Veterinaria.WinForms.Vistas.Veterinario;

namespace Veterinaria.WinForms.Vistas.Autenticacion;

/// <summary>
/// Formulario de autenticación con estética Ejecutivo Romántico Pastel y ruteo basado en roles.
/// </summary>
public partial class FormInicioSesion : Form
{
    private const string PlaceholderUsuario = "Ingrese su usuario";
    private const string PlaceholderContrasena = "Ingrese su contraseña";

    private readonly UsuarioControlador _usuarioControlador;
    private readonly IServiceProvider _serviceProvider;

    public FormInicioSesion(
        UsuarioControlador usuarioControlador,
        IServiceProvider serviceProvider)
    {
        _usuarioControlador = usuarioControlador;
        _serviceProvider = serviceProvider;

        InitializeComponent();
        txtUsuario.TextChanged += txtUsuario_TextChanged;
        CargarCredencialesRecordadas();
    }

    private void ConfigurarAutocompletado()
    {
        var nombres = GestorCredencialesLocales.ObtenerNombresUsuarios();
        var coleccion = new AutoCompleteStringCollection();
        if (nombres.Length > 0)
        {
            coleccion.AddRange(nombres);
        }

        txtUsuario.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
        txtUsuario.AutoCompleteSource = AutoCompleteSource.CustomSource;
        txtUsuario.AutoCompleteCustomSource = coleccion;
    }

    private void CargarCredencialesRecordadas()
    {
        ConfigurarAutocompletado();

        var ultimo = GestorCredencialesLocales.ObtenerUltimo();
        if (ultimo is not null && !string.IsNullOrWhiteSpace(ultimo.Usuario))
        {
            chkRecordarUsuario.Checked = true;
            txtUsuario.Text = ultimo.Usuario;
            txtUsuario.ForeColor = Color.FromArgb(45, 40, 46);

            if (!string.IsNullOrEmpty(ultimo.Contrasena))
            {
                txtContrasena.UseSystemPasswordChar = true;
                txtContrasena.Text = ultimo.Contrasena;
                txtContrasena.ForeColor = Color.FromArgb(45, 40, 46);
            }
        }
        else
        {
            chkRecordarUsuario.Checked = false;
            LimpiarCamposLogin();
        }
    }

    private void txtUsuario_TextChanged(object? sender, EventArgs e)
    {
        var usuario = txtUsuario.Text.Trim();
        if (usuario == PlaceholderUsuario || string.IsNullOrWhiteSpace(usuario))
        {
            return;
        }

        var credencial = GestorCredencialesLocales.ObtenerPorUsuario(usuario);
        if (credencial is not null)
        {
            chkRecordarUsuario.Checked = true;
            if (!string.IsNullOrEmpty(credencial.Contrasena))
            {
                txtContrasena.UseSystemPasswordChar = true;
                txtContrasena.Text = credencial.Contrasena;
                txtContrasena.ForeColor = Color.FromArgb(45, 40, 46);
            }
        }
    }

    private void btnSalir_Click(object? sender, EventArgs e)
    {
        Application.Exit();
    }

    protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
    {
        if (keyData == Keys.Escape)
        {
            Application.Exit();
            return true;
        }

        return base.ProcessCmdKey(ref msg, keyData);
    }

    private void txtUsuario_Enter(object? sender, EventArgs e)
    {
        if (txtUsuario.Text == PlaceholderUsuario)
        {
            txtUsuario.Text = string.Empty;
            txtUsuario.ForeColor = Color.FromArgb(45, 40, 46);
        }
    }

    private void txtUsuario_Leave(object? sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(txtUsuario.Text))
        {
            txtUsuario.Text = PlaceholderUsuario;
            txtUsuario.ForeColor = Color.FromArgb(160, 140, 148);
        }
    }

    private void txtContrasena_Enter(object? sender, EventArgs e)
    {
        if (txtContrasena.Text == PlaceholderContrasena)
        {
            txtContrasena.Text = string.Empty;
            txtContrasena.UseSystemPasswordChar = true;
            txtContrasena.ForeColor = Color.FromArgb(45, 40, 46);
        }
    }

    private void txtContrasena_Leave(object? sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(txtContrasena.Text))
        {
            txtContrasena.UseSystemPasswordChar = false;
            txtContrasena.Text = PlaceholderContrasena;
            txtContrasena.ForeColor = Color.FromArgb(160, 140, 148);
        }
    }

    private void txtCampos_KeyDown(object? sender, KeyEventArgs e)
    {
        if (e.KeyCode == Keys.Enter)
        {
            e.SuppressKeyPress = true;
            btnIngresar_Click(sender, EventArgs.Empty);
        }
    }

    private async void btnIngresar_Click(object? sender, EventArgs e)
    {
        var nombreUsuario = txtUsuario.Text.Trim();
        var contrasena = txtContrasena.Text;

        if (nombreUsuario == PlaceholderUsuario || string.IsNullOrWhiteSpace(nombreUsuario))
        {
            MostrarError("Ingrese su usuario.");
            txtUsuario.Focus();
            return;
        }

        if (contrasena == PlaceholderContrasena || string.IsNullOrWhiteSpace(contrasena))
        {
            MostrarError("Ingrese su contraseña.");
            txtContrasena.Focus();
            return;
        }

        BTINGRESAR.Enabled = false;
        BTINGRESAR.Text = "Validando...";
        lblError.Visible = false;

        try
        {
            // Credenciales existentes en el proyecto (InicializadorDatos):
            // admin / admin123 → Administrador
            // vet / vet123 → Veterinario
            // secretario / sec123 → Secretario
            var resultadoAuth = await _usuarioControlador.AutenticarAsync(nombreUsuario, contrasena);

            if (!resultadoAuth.EsExitoso || resultadoAuth.Valor is null || !resultadoAuth.Valor.Activo)
            {
                MostrarError("Usuario o contraseña incorrectos.");
                return;
            }

            var usuario = resultadoAuth.Valor;
            SesionActual.IniciarSesion(usuario);

            // Persistencia de credenciales según la opción del usuario
            if (chkRecordarUsuario.Checked)
            {
                GestorCredencialesLocales.Guardar(nombreUsuario, contrasena);
            }
            else
            {
                GestorCredencialesLocales.Eliminar(nombreUsuario);
            }

            DespacharSegunRol(usuario.NombreTipoUsuario);
        }
        catch (Exception)
        {
            MostrarError("Usuario o contraseña incorrectos.");
        }
        finally
        {
            BTINGRESAR.Enabled = true;
            BTINGRESAR.Text = "INGRESAR";
        }
    }

    /// <summary>
    /// Rutea al usuario autenticado hacia su panel correspondiente según el rol asignado.
    /// </summary>
    private void DespacharSegunRol(string rol)
    {
        Form formularioDestino;
        var rolNormalizado = rol.ToLowerInvariant();

        if (rolNormalizado.Contains("admin"))
        {
            formularioDestino = _serviceProvider.GetRequiredService<FormAdminPrincipal>();
        }
        else if (rolNormalizado.Contains("vet"))
        {
            formularioDestino = _serviceProvider.GetRequiredService<FormVeterinarioPrincipal>();
        }
        else if (rolNormalizado.Contains("secretar") || rolNormalizado.Contains("recep") || rolNormalizado.Contains("asist"))
        {
            formularioDestino = _serviceProvider.GetRequiredService<FormSecretarioPrincipal>();
        }
        else
        {
            // Fallback por defecto al panel administrador
            formularioDestino = _serviceProvider.GetRequiredService<FormAdminPrincipal>();
        }

        formularioDestino.FormClosed += (_, _) =>
        {
            SesionActual.CerrarSesion();
            CargarCredencialesRecordadas();
            lblError.Visible = false;
            Show();
        };
        Hide();
        formularioDestino.Show();
    }

    private void LimpiarCamposLogin()
    {
        txtUsuario.Text = PlaceholderUsuario;
        txtUsuario.ForeColor = Color.FromArgb(160, 140, 148);
        txtContrasena.UseSystemPasswordChar = false;
        txtContrasena.Text = PlaceholderContrasena;
        txtContrasena.ForeColor = Color.FromArgb(160, 140, 148);
    }

    private void MostrarError(string mensaje)
    {
        lblError.Text = mensaje;
        lblError.Visible = true;
    }
}
