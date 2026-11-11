using System.Runtime.InteropServices;
using Microsoft.Extensions.DependencyInjection;
using Veterinaria.Controllers.Controladores;
using Veterinaria.WinForms.Sesion;
using Veterinaria.WinForms.Vistas.Administrador;
using Veterinaria.WinForms.Vistas.Secretario;
using Veterinaria.WinForms.Vistas.Veterinario;

namespace Veterinaria.WinForms.Vistas.Autenticacion;

// Formulario de autenticación con estética Ejecutivo Romántico Pastel y ruteo basado en roles.
public partial class FormInicioSesion : Form
{
    private const string PlaceholderUsuario = "Ingrese su usuario";
    private const string PlaceholderContrasena = "Ingrese su contraseña";

    private readonly UsuarioControlador _usuarioControlador;
    private readonly IServiceProvider _serviceProvider;

    // -----------------------------------------------------------------------------------------
    // CONSTRUCTOR E INYECCIÓN DE DEPENDENCIAS
    // -----------------------------------------------------------------------------------------
    public FormInicioSesion(
        UsuarioControlador usuarioControlador,
        IServiceProvider serviceProvider)
    {
        // 1. Inyectamos el controlador de negocio para autenticar usuarios mediante BCrypt
        _usuarioControlador = usuarioControlador;
        // 2. Inyectamos el proveedor de servicios para resolver los formularios principales según el rol
        _serviceProvider = serviceProvider;

        // 3. Inicializa los componentes gráficos del diseñador (botones, cajas de texto, etc.)
        InitializeComponent();

        // 4. Suscribe el evento para detectar cuando se tipea un usuario y auto-completar su contraseña si fue recordada
        txtUsuario.TextChanged += txtUsuario_TextChanged;

        // 5. Carga las credenciales guardadas localmente (si existen) al abrir el formulario
        CargarCredencialesRecordadas();
    }

    // Configura la lista de autocompletado en la caja de texto del usuario con las cuentas previamente recordadas
    private void ConfigurarAutocompletado()
    {
        // Obtiene el listado de nombres de usuario guardados en el archivo local 'credenciales.json'
        var nombres = GestorCredencialesLocales.ObtenerNombresUsuarios();
        var coleccion = new AutoCompleteStringCollection();
        if (nombres.Length > 0)
        {
            coleccion.AddRange(nombres);
        }

        // Habilita el modo de sugerencia y anexo mientras el usuario escribe en la caja de texto
        txtUsuario.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
        txtUsuario.AutoCompleteSource = AutoCompleteSource.CustomSource;
        txtUsuario.AutoCompleteCustomSource = coleccion;
    }

    // Carga la última cuenta utilizada en los campos de login si la casilla 'Recordar' estuvo activa
    private void CargarCredencialesRecordadas()
    {
        // Actualiza la lista de autocompletado
        ConfigurarAutocompletado();

        // Recupera el último usuario que inició sesión exitosamente
        var ultimo = GestorCredencialesLocales.ObtenerUltimo();
        if (ultimo is not null && !string.IsNullOrWhiteSpace(ultimo.Usuario))
        {
            // Marca el CheckBox y rellena el usuario
            chkRecordarUsuario.Checked = true;
            txtUsuario.Text = ultimo.Usuario;
            txtUsuario.ForeColor = Color.FromArgb(45, 40, 46);

            // Si tiene contraseña recordada, oculta los caracteres con asteriscos y rellena el campo
            if (!string.IsNullOrEmpty(ultimo.Contrasena))
            {
                txtContrasena.UseSystemPasswordChar = true;
                txtContrasena.Text = ultimo.Contrasena;
                txtContrasena.ForeColor = Color.FromArgb(45, 40, 46);
            }
        }
        else
        {
            // Si no hay cuenta previa, deja los campos limpios con sus textos de ayuda (Placeholders)
            chkRecordarUsuario.Checked = false;
            LimpiarCamposLogin();
        }
    }

    // Detecta reactivamente si el nombre tipeado coincide con una cuenta guardada para autocompletar su contraseña
    private void txtUsuario_TextChanged(object? sender, EventArgs e)
    {
        var usuario = txtUsuario.Text.Trim();
        // Ignora si está vacío o si tiene el texto de ayuda predeterminado
        if (usuario == PlaceholderUsuario || string.IsNullOrWhiteSpace(usuario))
        {
            return;
        }

        // Si coincide con una cuenta en el JSON local, completa automáticamente su contraseña
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

    // -----------------------------------------------------------------------------------------
    // FLUJO PRINCIPAL DE AUTENTICACIÓN
    // -----------------------------------------------------------------------------------------
    private async void btnIngresar_Click(object? sender, EventArgs e)
    {
        // 1. Extrae los valores ingresados por el usuario eliminando espacios en blanco en los extremos
        var nombreUsuario = txtUsuario.Text.Trim();
        var contrasena = txtContrasena.Text;

        // 2. Validación preventiva: comprueba que el usuario haya ingresado su identificador
        if (nombreUsuario == PlaceholderUsuario || string.IsNullOrWhiteSpace(nombreUsuario))
        {
            MostrarError("Ingrese su usuario.");
            txtUsuario.Focus();
            return;
        }

        // 3. Validación preventiva: comprueba que no se intente ingresar con la contraseña vacía
        if (contrasena == PlaceholderContrasena || string.IsNullOrWhiteSpace(contrasena))
        {
            MostrarError("Ingrese su contraseña.");
            txtContrasena.Focus();
            return;
        }

        // 4. Bloquea temporalmente el botón para prevenir múltiples clics y avisa al usuario
        BTINGRESAR.Enabled = false;
        BTINGRESAR.Text = "Validando...";
        lblError.Visible = false;

        try
        {
            // 5. LLAMADA AL CONTROLADOR DE NEGOCIO:
            // Consulta SQL Server mediante Entity Framework Core y compara la contraseña contra el hash BCrypt
            var resultadoAuth = await _usuarioControlador.AutenticarAsync(nombreUsuario, contrasena);

            // 6. Si las credenciales no coinciden o el usuario fue dado de baja lógica (Activo = false)
            if (!resultadoAuth.EsExitoso || resultadoAuth.Valor is null || !resultadoAuth.Valor.Activo)
            {
                // Si fue desactivado, purga sus credenciales guardadas en el equipo para que no queden registradas
                if (!resultadoAuth.EsExitoso && resultadoAuth.Mensaje.Contains("inactivo", StringComparison.OrdinalIgnoreCase))
                {
                    GestorCredencialesLocales.Eliminar(nombreUsuario);
                    CargarCredencialesRecordadas();
                }

                MostrarError("Usuario o contraseña incorrectos.");
                return;
            }

            // 7. Obtenemos el objeto DTO del usuario autenticado con su rol asignado
            var usuario = resultadoAuth.Valor;

            // 8. POLÍTICA DE SEGURIDAD PARA EL ROL GERENTE:
            // La cuenta 'Gerente' no opera la interfaz cotidiana; solo existe para firmar autorizaciones de Backup/Restore
            if (string.Equals(usuario.NombreTipoUsuario, "Gerente", StringComparison.OrdinalIgnoreCase))
            {
                MostrarError("El perfil Gerente no tiene acceso al sistema; solo autoriza backups y restauraciones.");
                return;
            }

            // 9. INICIALIZACIÓN DE LA SESIÓN GLOBAL:
            // Guarda el Id, nombre y rol en la memoria de la aplicación para que estén disponibles en todas las ventanas
            SesionActual.IniciarSesion(usuario);

            // 10. GESTIÓN DE CREDENCIALES LOCALES (RECORDAR USUARIO):
            // Si el checkbox está marcado, persiste los datos en 'credenciales.json'; de lo contrario, los elimina
            if (chkRecordarUsuario.Checked)
            {
                GestorCredencialesLocales.Guardar(nombreUsuario, contrasena);
            }
            else
            {
                GestorCredencialesLocales.Eliminar(nombreUsuario);
            }

            // 11. REDIRECCIÓN DINÁMICA: Envía al usuario a su panel de control según su rol
            DespacharSegunRol(usuario.NombreTipoUsuario);
        }
        catch (Exception)
        {
            // Captura defensiva de cualquier error de conexión o base de datos
            MostrarError("Usuario o contraseña incorrectos.");
        }
        finally
        {
            // 12. Restablece el botón de inicio de sesión a su estado normal
            BTINGRESAR.Enabled = true;
            BTINGRESAR.Text = "INGRESAR";
        }
    }

    // Rutea al usuario autenticado hacia su panel correspondiente según el rol asignado
    private void DespacharSegunRol(string rol)
    {
        Form formularioDestino;
        var rolNormalizado = rol.ToLowerInvariant();

        // 1. Evaluación de rol 'Administrador': Abre el panel administrativo general
        if (rolNormalizado.Contains("admin"))
        {
            formularioDestino = _serviceProvider.GetRequiredService<FormAdminPrincipal>();
        }
        // 2. Evaluación de rol 'Veterinario': Abre el panel de atención médica, consultas y tratamientos
        else if (rolNormalizado.Contains("vet"))
        {
            formularioDestino = _serviceProvider.GetRequiredService<FormVeterinarioPrincipal>();
        }
        // 3. Evaluación de rol 'Secretario / Recepcionista': Abre el panel de recepción, registro de turnos y cobros
        else if (rolNormalizado.Contains("secretar") || rolNormalizado.Contains("recep") || rolNormalizado.Contains("asist"))
        {
            formularioDestino = _serviceProvider.GetRequiredService<FormSecretarioPrincipal>();
        }
        else
        {
            // Fallback por defecto en caso de un rol no contemplado
            formularioDestino = _serviceProvider.GetRequiredService<FormAdminPrincipal>();
        }

        // 4. SUSCRIPCIÓN AL CIERRE DEL PANEL DESTINO:
        // Cuando el usuario cierre sesión o cierre su ventana principal, se ejecuta este código:
        formularioDestino.FormClosed += (_, _) =>
        {
            // Limpia la sesión activa en memoria
            SesionActual.CerrarSesion();
            // Recarga las credenciales recordadas
            CargarCredencialesRecordadas();
            lblError.Visible = false;
            // Vuelve a hacer visible el formulario de Login
            Show();
        };

        // 5. Oculta el formulario de Login actual
        Hide();
        // 6. Muestra la ventana principal del usuario
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
