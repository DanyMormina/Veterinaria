using System.Runtime.InteropServices;
using Microsoft.Extensions.DependencyInjection;
using Veterinaria.Controllers.Controladores;
using Veterinaria.WinForms.Sesion;
using Veterinaria.Domain.Dtos;
using Veterinaria.WinForms.Vistas.Administrador;
using Veterinaria.WinForms.Vistas.Secretario;
using Veterinaria.WinForms.Vistas.Veterinario;

namespace Veterinaria.WinForms.Vistas.Autenticacion;

/// <summary>
/// Formulario de autenticación compacto (200x200) con paleta romantic executive y ruteo basado en roles/tipos de usuario.
/// </summary>
public partial class FormInicioSesion : Form
{
    private const string PlaceholderUsuario = "Usuario";
    private const string PlaceholderContrasena = "Contraseña";

    private readonly UsuarioControlador _usuarioControlador;
    private readonly IServiceProvider _serviceProvider;

    [DllImport("user32.dll")]
    private static extern bool ReleaseCapture();

    [DllImport("user32.dll")]
    private static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);

    private const int WM_NCLBUTTONDOWN = 0xA1;
    private const int HT_CAPTION = 0x2;

    public FormInicioSesion(
        UsuarioControlador usuarioControlador,
        IServiceProvider serviceProvider)
    {
        _usuarioControlador = usuarioControlador;
        _serviceProvider = serviceProvider;

        InitializeComponent();
    }

    private void FormInicioSesion_MouseDown(object? sender, MouseEventArgs e)
    {
        if (e.Button == MouseButtons.Left)
        {
            ReleaseCapture();
            SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
        }
    }

    private void btnCerrar_Click(object? sender, EventArgs e)
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
            txtUsuario.ForeColor = Color.FromArgb(58, 53, 59);
        }
    }

    private void txtUsuario_Leave(object? sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(txtUsuario.Text))
        {
            txtUsuario.Text = PlaceholderUsuario;
            txtUsuario.ForeColor = Color.FromArgb(142, 130, 138);
        }
    }

    private void txtContrasena_Enter(object? sender, EventArgs e)
    {
        if (txtContrasena.Text == PlaceholderContrasena)
        {
            txtContrasena.Text = string.Empty;
            txtContrasena.UseSystemPasswordChar = true;
            txtContrasena.ForeColor = Color.FromArgb(58, 53, 59);
        }
    }

    private void txtContrasena_Leave(object? sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(txtContrasena.Text))
        {
            txtContrasena.UseSystemPasswordChar = false;
            txtContrasena.Text = PlaceholderContrasena;
            txtContrasena.ForeColor = Color.FromArgb(142, 130, 138);
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
            MostrarError("Ingrese su nombre de usuario.");
            txtUsuario.Focus();
            return;
        }

        if (contrasena == PlaceholderContrasena || string.IsNullOrWhiteSpace(contrasena))
        {
            MostrarError("Ingrese su contraseña.");
            txtContrasena.Focus();
            return;
        }

        btnIngresar.Enabled = false;
        btnIngresar.Text = "VALIDANDO...";
        lblError.Visible = false;

        try
        {
            // Autenticación asíncrona mediante el controlador
            var resultadoAuth = await _usuarioControlador.AutenticarAsync(nombreUsuario, contrasena);

            if (!resultadoAuth.EsExitoso || resultadoAuth.Valor is null)
            {
                MostrarError(resultadoAuth.Mensaje);
                return;
            }

            var usuario = resultadoAuth.Valor;

            if (!usuario.Activo)
            {
                MostrarError("El usuario se encuentra inactivo.");
                return;
            }

            // Establecer sesión global en memoria
            SesionActual.IniciarSesion(usuario);

            // Despachar al formulario correspondiente según el tipo de usuario
            DespacharSegunRol(usuario.NombreTipoUsuario);
        }
        catch (Exception ex)
        {
            MostrarError($"Error: {ex.Message}");
        }
        finally
        {
            btnIngresar.Enabled = true;
            btnIngresar.Text = "INGRESAR";
        }
    }

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
            // Fallback por defecto al panel principal
            formularioDestino = _serviceProvider.GetRequiredService<FormAdminPrincipal>();
        }

        formularioDestino.FormClosed += (_, _) => Application.Exit();
        Hide();
        formularioDestino.Show();
    }

    private void MostrarError(string mensaje)
    {
        lblError.Text = mensaje;
        lblError.Visible = true;
    }
}
