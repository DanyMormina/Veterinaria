using System.Runtime.InteropServices;
using Microsoft.Extensions.DependencyInjection;
using Veterinaria.Controllers.Controladores;
using Veterinaria.CrossCutting.Security;
using Veterinaria.CrossCutting.Session;
using Veterinaria.Domain.Dtos;
using Veterinaria.WinForms.Views.Admin;
using Veterinaria.WinForms.Views.Secretario;
using Veterinaria.WinForms.Views.Veterinario;

namespace Veterinaria.WinForms.Views.Auth;

/// <summary>
/// Formulario de autenticación compacto (200x200) con paleta romantic executive y ruteo basado en roles/tipos de usuario.
/// </summary>
public partial class FormLogin : Form
{
    private const string PlaceholderUsuario = "Usuario";
    private const string PlaceholderPassword = "Contraseña";

    private readonly UsuarioController _usuarioController;
    private readonly IServiceProvider _serviceProvider;

    [DllImport("user32.dll")]
    private static extern bool ReleaseCapture();

    [DllImport("user32.dll")]
    private static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);

    private const int WM_NCLBUTTONDOWN = 0xA1;
    private const int HT_CAPTION = 0x2;

    public FormLogin(
        UsuarioController usuarioController,
        IServiceProvider serviceProvider)
    {
        _usuarioController = usuarioController;
        _serviceProvider = serviceProvider;

        InitializeComponent();
    }

    private void FormLogin_MouseDown(object? sender, MouseEventArgs e)
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

    private void txtPassword_Enter(object? sender, EventArgs e)
    {
        if (txtPassword.Text == PlaceholderPassword)
        {
            txtPassword.Text = string.Empty;
            txtPassword.UseSystemPasswordChar = true;
            txtPassword.ForeColor = Color.FromArgb(58, 53, 59);
        }
    }

    private void txtPassword_Leave(object? sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(txtPassword.Text))
        {
            txtPassword.UseSystemPasswordChar = false;
            txtPassword.Text = PlaceholderPassword;
            txtPassword.ForeColor = Color.FromArgb(142, 130, 138);
        }
    }

    private void txtCampos_KeyDown(object? sender, KeyEventArgs e)
    {
        if (e.KeyCode == Keys.Enter)
        {
            e.SuppressKeyPress = true;
            btnLogin_Click(sender, EventArgs.Empty);
        }
    }

    private async void btnLogin_Click(object? sender, EventArgs e)
    {
        var username = txtUsuario.Text.Trim();
        var password = txtPassword.Text;

        if (username == PlaceholderUsuario || string.IsNullOrWhiteSpace(username))
        {
            MostrarError("Ingrese su nombre de usuario.");
            txtUsuario.Focus();
            return;
        }

        if (password == PlaceholderPassword || string.IsNullOrWhiteSpace(password))
        {
            MostrarError("Ingrese su contraseña.");
            txtPassword.Focus();
            return;
        }

        btnLogin.Enabled = false;
        btnLogin.Text = "VALIDANDO...";
        lblError.Visible = false;

        try
        {
            // Autenticación asíncrona mediante el controlador
            var authResult = await _usuarioController.AutenticarAsync(username, password);

            if (!authResult.EsExitoso || authResult.Valor is null)
            {
                MostrarError(authResult.Mensaje);
                return;
            }

            var usuario = authResult.Valor;

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
            btnLogin.Enabled = true;
            btnLogin.Text = "INGRESAR";
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
