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
        CargarImagenLogo();
    }

    /// <summary>
    /// Carga la ilustración representativa de la clínica veterinaria en el panel lateral de autenticación.
    /// </summary>
    private void CargarImagenLogo()
    {
        var ruta = Path.Combine(AppContext.BaseDirectory, "Resources", "vet-login.png");
        if (File.Exists(ruta))
        {
            using var original = Image.FromFile(ruta);
            picLogo.Image = new Bitmap(original);
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

        // Validaciones de campos de entrada
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
        btnIngresar.Text = "Validando...";
        lblError.Visible = false;

        try
        {
            // 1. Autenticación asíncrona mediante el controlador de negocio
            var resultadoAuth = await _usuarioControlador.AutenticarAsync(nombreUsuario, contrasena);

            if (!resultadoAuth.EsExitoso || resultadoAuth.Valor is null)
            {
                MostrarError(resultadoAuth.Mensaje);
                return;
            }

            var usuario = resultadoAuth.Valor;

            // 2. Validación de estado de usuario activo
            if (!usuario.Activo)
            {
                MostrarError("El usuario se encuentra inactivo.");
                return;
            }

            // 3. Establecer sesión global en memoria
            SesionActual.IniciarSesion(usuario);

            // 4. Despachar al formulario correspondiente según el tipo de usuario / rol
            DespacharSegunRol(usuario.NombreTipoUsuario);
        }
        catch (Exception ex)
        {
            MostrarError($"Error: {ex.Message}");
        }
        finally
        {
            btnIngresar.Enabled = true;
            btnIngresar.Text = "🐾  Ingresar";
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
