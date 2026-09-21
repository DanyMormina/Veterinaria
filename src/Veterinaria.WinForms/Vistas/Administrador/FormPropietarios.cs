using System.Text.RegularExpressions;
using Veterinaria.Controllers.Controladores;
using Veterinaria.Domain.Comunes;
using Veterinaria.Domain.Dtos;
using Veterinaria.WinForms.Sesion;

namespace Veterinaria.WinForms.Vistas.Administrador;

/// <summary>
/// Formulario de consulta y gestión de propietarios y sus mascotas asociadas para el rol Administrador.
/// </summary>
public partial class FormPropietarios : Form
{
    private static readonly Regex RegexNombreApellido = new(@"^[a-zA-ZáéíóúÁÉÍÓÚñÑüÜ\s]{2,100}$", RegexOptions.Compiled);
    private static readonly Regex RegexDni = new(@"^\d{7,8}$", RegexOptions.Compiled);
    private static readonly Regex RegexTelefono = new(@"^\d{6,13}$", RegexOptions.Compiled);
    private static readonly Regex SoloDigitos = new(@"^\d+$", RegexOptions.Compiled);

    private static readonly Color ColorBordeError = ColorTranslator.FromHtml("#B85D69");
    private static readonly Color ColorBordeNeutro = ColorTranslator.FromHtml("#E2D9DC");
    private static readonly Color ColorFondoError = ColorTranslator.FromHtml("#FDECEF");
    private static readonly Color ColorFondoNormal = Color.White;
    private static readonly Color ColorEtiquetaNormal = ColorTranslator.FromHtml("#3A353B");
    private static readonly Color ColorEtiquetaError = ColorTranslator.FromHtml("#B85D69");

    private readonly ErrorProvider _errores = new();
    private readonly PropietarioControlador? _propietarioControlador;
    private readonly MascotaControlador? _mascotaControlador;

    private List<PropietarioRespuestaDto> _propietarios = [];
    private Control[] _controlesEntrada = [];
    private long? _idSeleccionado;

    public FormPropietarios() : this(null, null)
    {
    }

    public FormPropietarios(
        PropietarioControlador? propietarioControlador,
        MascotaControlador? mascotaControlador = null)
    {
        _propietarioControlador = propietarioControlador;
        _mascotaControlador = mascotaControlador;
        InitializeComponent();
        ConfigurarValidacionVisual();
        ConfigurarEventosEntrada();
    }

    private void ConfigurarEventosEntrada()
    {
        // 1. Restricción de entrada solo para dígitos en DNI y Teléfono (char.IsDigit y char.IsControl)
        var validarDigitos = new KeyPressEventHandler((_, e) =>
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        });

        txtDni.KeyPress += validarDigitos;
        txtTelefono.KeyPress += validarDigitos;

        // 2. Restricción de caracteres especiales en Nombre y Apellido (solo letras, vocales acentuadas y espacios simples)
        var validarLetras = new KeyPressEventHandler((sender, e) =>
        {
            if (char.IsControl(e.KeyChar))
                return;

            if (char.IsLetter(e.KeyChar))
                return;

            if (e.KeyChar == ' ' && sender is TextBox txt && !txt.Text.EndsWith(' '))
                return;

            e.Handled = true;
        });

        txtNombre.KeyPress += validarLetras;
        txtApellido.KeyPress += validarLetras;

        // 3. Restricción en Correo Electrónico (sin espacios en blanco)
        txtCorreoElectronico.KeyPress += (_, e) =>
        {
            if (char.IsWhiteSpace(e.KeyChar))
            {
                e.Handled = true;
            }
        };

        // 4. Filtros reactivos y eventos de grilla
        txtBuscar.TextChanged += (_, _) => AplicarFiltros();
        cboFiltroEstado.SelectedIndexChanged += (_, _) => AplicarFiltros();
        dgvPropietarios.CellClick += dgvPropietarios_CellClick;
    }

    private void ConfigurarValidacionVisual()
    {
        _errores.ContainerControl = this;
        _errores.BlinkStyle = ErrorBlinkStyle.NeverBlink;

        // Colección de controles auditados para validación y reseteo
        _controlesEntrada = [txtDni, txtNombre, txtApellido, txtTelefono, txtCorreoElectronico, txtDireccion];

        // Enlazar Enter, TextChanged y KeyPress para limpiar el color de error automáticamente cuando el usuario interactúa o tipea
        foreach (var control in _controlesEntrada)
        {
            control.Enter += Control_LimpiarError;
            if (control is TextBox txt)
            {
                txt.TextChanged += Control_LimpiarError;
                txt.KeyPress += Control_LimpiarError;
            }
        }
    }

    /// <summary>
    /// Valida los campos del formulario conforme a las reglas de negocio y restricciones de formato.
    /// Retorna un Resultado indicando el éxito o el mensaje de falla junto con el control a enfocar.
    /// </summary>
    public Resultado ValidarCampos(out Control? controlConFoco)
    {
        LimpiarErroresValidacion();

        // 1. Nombre: Obligatorio, longitud 2 a 100 caracteres, solo letras, tildes y espacios
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

        // 2. Apellido: Obligatorio, longitud 2 a 100 caracteres, solo letras, tildes y espacios
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

        // 3. DNI: Obligatorio, solo numérico, exactamente 7 a 8 dígitos
        var dni = txtDni.Text.Trim();
        if (string.IsNullOrWhiteSpace(dni))
        {
            controlConFoco = txtDni;
            return Resultado.Falla("El DNI es obligatorio.");
        }

        if (!RegexDni.IsMatch(dni))
        {
            controlConFoco = txtDni;
            return Resultado.Falla("El DNI debe contener entre 7 y 8 dígitos numéricos.");
        }

        // 4. Dirección: Obligatoria, longitud 3 a 200 caracteres
        var direccion = txtDireccion.Text.Trim();
        if (string.IsNullOrWhiteSpace(direccion) || direccion.Length < 3 || direccion.Length > 200)
        {
            controlConFoco = txtDireccion;
            return Resultado.Falla("La dirección es obligatoria y debe tener entre 3 y 200 caracteres.");
        }

        // 5. Teléfono: Obligatorio, solo numérico, longitud entre 6 y 13 dígitos
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

        // 6. Correo Electrónico: Obligatorio y formato válido
        var correo = txtCorreoElectronico.Text.Trim();
        var errorCorreo = UsuarioControlador.ValidarFormatoCorreo(correo);
        if (errorCorreo is not null)
        {
            controlConFoco = txtCorreoElectronico;
            return Resultado.Falla(errorCorreo);
        }

        controlConFoco = null;
        return Resultado.Exito();
    }

    /// <summary>
    /// Sobrecarga booleana de ValidarCampos para compatibilidad.
    /// </summary>
    public bool ValidarCampos(out string mensajeError, out Control? controlConFoco)
    {
        var resultado = ValidarCampos(out controlConFoco);
        mensajeError = resultado.EsExitoso ? string.Empty : resultado.Mensaje;
        return resultado.EsExitoso;
    }

    private void MarcarError(Control? control, string mensaje, bool enfocar = true)
    {
        if (control is null) return;

        if (control is TextBox txt)
        {
            txt.BackColor = ColorFondoError;
        }

        // Si el control está contenido dentro de un panel decorador, resaltar borde en Borgoña suave
        if (control.Parent is Panel pnlContenedor)
        {
            pnlContenedor.BackColor = ColorBordeError;
        }

        var etiqueta = ObtenerEtiquetaDeControl(control);
        if (etiqueta is not null)
        {
            etiqueta.ForeColor = ColorEtiquetaError;
        }

        _errores.SetError(control, mensaje);

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

        // Restaurar color neutro del panel contenedor
        if (control.Parent is Panel pnlContenedor)
        {
            pnlContenedor.BackColor = ColorBordeNeutro;
        }

        var etiqueta = ObtenerEtiquetaDeControl(control);
        if (etiqueta is not null)
        {
            etiqueta.ForeColor = ColorEtiquetaNormal;
        }

        _errores.SetError(control, string.Empty);
    }

    private Control IdentificarControlDesdeMensaje(string mensaje)
    {
        var msg = mensaje.ToLowerInvariant();
        if (msg.Contains("dni"))
            return txtDni;
        if (msg.Contains("teléfono") || msg.Contains("telefono"))
            return txtTelefono;
        if (msg.Contains("correo") || msg.Contains("email"))
            return txtCorreoElectronico;
        if (msg.Contains("dirección") || msg.Contains("direccion"))
            return txtDireccion;
        if (msg.Contains("apellido"))
            return txtApellido;
        if (msg.Contains("nombre"))
            return txtNombre;

        return txtDni;
    }

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

    private void LimpiarErroresValidacion()
    {
        _errores.Clear();

        foreach (var control in _controlesEntrada)
        {
            if (control is TextBox txt)
            {
                txt.BackColor = ColorFondoNormal;
            }

            if (control.Parent is Panel pnlContenedor)
            {
                pnlContenedor.BackColor = ColorBordeNeutro;
            }
        }

        lblDni.ForeColor = ColorEtiquetaNormal;
        lblNombre.ForeColor = ColorEtiquetaNormal;
        lblApellido.ForeColor = ColorEtiquetaNormal;
        lblTelefono.ForeColor = ColorEtiquetaNormal;
        lblCorreoElectronico.ForeColor = ColorEtiquetaNormal;
        lblDireccion.ForeColor = ColorEtiquetaNormal;
        lblEstado.ForeColor = ColorEtiquetaNormal;
    }

    private async void FormPropietarios_Load(object? sender, EventArgs e)
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

        await CargarPropietariosAsync();
    }

    private async Task CargarPropietariosAsync()
    {
        if (_propietarioControlador is null)
            return;

        var resultado = await _propietarioControlador.ObtenerTodosAsync();
        if (!resultado.EsExitoso || resultado.Valor is null)
        {
            MessageBox.Show(
                resultado.Mensaje,
                "Propietarios",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning);
            return;
        }

        _propietarios = resultado.Valor.ToList();
        MostrarPropietariosEnGrilla(_propietarios);

        if (_propietarios.Count > 0)
        {
            await SeleccionarPropietarioAsync(_propietarios[0]);
        }
        else
        {
            LimpiarDetalles();
        }
    }

    private void MostrarPropietariosEnGrilla(IEnumerable<PropietarioRespuestaDto> propietarios)
    {
        dgvPropietarios.Rows.Clear();

        foreach (var p in propietarios)
        {
            dgvPropietarios.Rows.Add(
                p.Id,
                p.Nombre,
                p.Apellido,
                p.DNI,
                p.Telefono ?? string.Empty,
                p.CorreoElectronico ?? string.Empty,
                p.Direccion ?? string.Empty,
                p.Activo ? "Activo" : "Inactivo");
        }
    }

    private void AplicarFiltros()
    {
        var texto = txtBuscar.Text.Trim();
        var estadoSeleccionado = cboFiltroEstado.SelectedIndex; // 0 = Todos, 1 = Solo Activos, 2 = Solo Inactivos

        var consulta = _propietarios.AsEnumerable();

        if (estadoSeleccionado == 1)
        {
            consulta = consulta.Where(p => p.Activo);
        }
        else if (estadoSeleccionado == 2)
        {
            consulta = consulta.Where(p => !p.Activo);
        }

        if (!string.IsNullOrWhiteSpace(texto))
        {
            consulta = consulta.Where(p =>
                p.Nombre.Contains(texto, StringComparison.OrdinalIgnoreCase) ||
                p.Apellido.Contains(texto, StringComparison.OrdinalIgnoreCase) ||
                p.DNI.Contains(texto, StringComparison.OrdinalIgnoreCase) ||
                (p.Telefono?.Contains(texto, StringComparison.OrdinalIgnoreCase) ?? false) ||
                (p.CorreoElectronico?.Contains(texto, StringComparison.OrdinalIgnoreCase) ?? false) ||
                (p.Direccion?.Contains(texto, StringComparison.OrdinalIgnoreCase) ?? false));
        }

        var filtrados = consulta.ToList();
        MostrarPropietariosEnGrilla(filtrados);

        if (filtrados.Count > 0)
        {
            _ = SeleccionarPropietarioAsync(filtrados[0]);
        }
        else
        {
            LimpiarDetalles();
        }
    }

    private async void dgvPropietarios_CellClick(object? sender, DataGridViewCellEventArgs e)
    {
        if (e.RowIndex < 0)
            return;

        var fila = dgvPropietarios.Rows[e.RowIndex];
        if (fila.Cells[0].Value is null)
            return;

        var id = Convert.ToInt64(fila.Cells[0].Value);
        var propietario = _propietarios.FirstOrDefault(p => p.Id == id);
        if (propietario is null)
            return;

        await SeleccionarPropietarioAsync(propietario);
    }

    private async Task SeleccionarPropietarioAsync(PropietarioRespuestaDto propietario)
    {
        LimpiarErroresValidacion();
        _idSeleccionado = propietario.Id;
        txtDni.Text = propietario.DNI;
        txtNombre.Text = propietario.Nombre;
        txtApellido.Text = propietario.Apellido;
        txtTelefono.Text = propietario.Telefono ?? string.Empty;
        txtCorreoElectronico.Text = propietario.CorreoElectronico ?? string.Empty;
        txtDireccion.Text = propietario.Direccion ?? string.Empty;
        cboEstado.SelectedItem = propietario.Activo ? "Activo" : "Inactivo";

        await CargarMascotasDelPropietarioAsync(propietario.Id);
    }

    private async Task CargarMascotasDelPropietarioAsync(long idPropietario)
    {
        dgvMascotasPropietario.Rows.Clear();

        if (_mascotaControlador is null)
            return;

        var resultado = await _mascotaControlador.ObtenerPorPropietarioAsync(idPropietario);
        if (!resultado.EsExitoso || resultado.Valor is null)
            return;

        foreach (var m in resultado.Valor)
        {
            dgvMascotasPropietario.Rows.Add(
                m.Id,
                m.Nombre,
                m.NombreEspecie,
                m.NombreRaza,
                m.Sexo,
                m.Activo ? "Activo" : "Inactivo");
        }
    }

    private void LimpiarDetalles()
    {
        _idSeleccionado = null;
        LimpiarErroresValidacion();
        txtDni.Clear();
        txtNombre.Clear();
        txtApellido.Clear();
        txtTelefono.Clear();
        txtCorreoElectronico.Clear();
        txtDireccion.Clear();
        cboEstado.SelectedIndex = 0;
        dgvMascotasPropietario.Rows.Clear();
    }

    private void btnBuscar_Click(object? sender, EventArgs e)
    {
        var textoBuscar = txtBuscar.Text.Trim();
        var dni = txtDni.Text.Trim();
        var nombre = txtNombre.Text.Trim();
        var apellido = txtApellido.Text.Trim();
        var telefono = txtTelefono.Text.Trim();
        var correo = txtCorreoElectronico.Text.Trim();
        var direccion = txtDireccion.Text.Trim();

        // 1. Si no completó ningún campo, indicar con un mensaje que debe completar al menos uno para buscar
        if (string.IsNullOrWhiteSpace(textoBuscar) &&
            string.IsNullOrWhiteSpace(dni) &&
            string.IsNullOrWhiteSpace(nombre) &&
            string.IsNullOrWhiteSpace(apellido) &&
            string.IsNullOrWhiteSpace(telefono) &&
            string.IsNullOrWhiteSpace(correo) &&
            string.IsNullOrWhiteSpace(direccion))
        {
            MessageBox.Show(
                $"Debe completar al menos un campo{Environment.NewLine}para poder realizar la búsqueda.",
                "Búsqueda de Propietario",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning);
            txtBuscar.Focus();
            return;
        }

        // 2. Validación de Teléfono si fue ingresado en los datos del propietario
        if (!string.IsNullOrWhiteSpace(telefono) && !RegexTelefono.IsMatch(telefono))
        {
            var error = "El número de teléfono debe contener entre 6 y 13 dígitos numéricos para poder buscar.";
            MarcarError(txtTelefono, error);
            MessageBox.Show(error, "Búsqueda de Propietario", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        // 3. Validación de Correo Electrónico si fue ingresado en los datos del propietario
        if (!string.IsNullOrWhiteSpace(correo))
        {
            var errorCorreo = UsuarioControlador.ValidarFormatoCorreo(correo);
            if (errorCorreo is not null)
            {
                MarcarError(txtCorreoElectronico, errorCorreo);
                MessageBox.Show(errorCorreo, "Búsqueda de Propietario", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
        }

        // 4. Validación de DNI si fue ingresado en los datos del propietario
        if (!string.IsNullOrWhiteSpace(dni) && !RegexDni.IsMatch(dni))
        {
            var error = "El DNI debe contener entre 7 y 8 dígitos numéricos para poder buscar.";
            MarcarError(txtDni, error);
            MessageBox.Show(error, "Búsqueda de Propietario", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        // 5. Validación de Nombre si fue ingresado
        if (!string.IsNullOrWhiteSpace(nombre) && (nombre.Length < 2 || nombre.Length > 100 || !RegexNombreApellido.IsMatch(nombre)))
        {
            var error = "El nombre debe contener entre 2 y 100 caracteres y solo letras o espacios.";
            MarcarError(txtNombre, error);
            MessageBox.Show(error, "Búsqueda de Propietario", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        // 6. Validación de Apellido si fue ingresado
        if (!string.IsNullOrWhiteSpace(apellido) && (apellido.Length < 2 || apellido.Length > 100 || !RegexNombreApellido.IsMatch(apellido)))
        {
            var error = "El apellido debe contener entre 2 y 100 caracteres y solo letras o espacios.";
            MarcarError(txtApellido, error);
            MessageBox.Show(error, "Búsqueda de Propietario", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        // 7. Validación de Dirección si fue ingresada
        if (!string.IsNullOrWhiteSpace(direccion) && (direccion.Length < 3 || direccion.Length > 200))
        {
            var error = "La dirección debe tener entre 3 y 200 caracteres.";
            MarcarError(txtDireccion, error);
            MessageBox.Show(error, "Búsqueda de Propietario", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        EjecutarBusqueda();
    }

    private void EjecutarBusqueda()
    {
        var texto = txtBuscar.Text.Trim();
        var estadoSeleccionado = cboFiltroEstado.SelectedIndex;

        var consulta = _propietarios.AsEnumerable();

        if (estadoSeleccionado == 1)
        {
            consulta = consulta.Where(p => p.Activo);
        }
        else if (estadoSeleccionado == 2)
        {
            consulta = consulta.Where(p => !p.Activo);
        }

        if (!string.IsNullOrWhiteSpace(texto))
        {
            consulta = consulta.Where(p =>
                p.Nombre.Contains(texto, StringComparison.OrdinalIgnoreCase) ||
                p.Apellido.Contains(texto, StringComparison.OrdinalIgnoreCase) ||
                p.DNI.Contains(texto, StringComparison.OrdinalIgnoreCase) ||
                (p.Telefono?.Contains(texto, StringComparison.OrdinalIgnoreCase) ?? false) ||
                (p.CorreoElectronico?.Contains(texto, StringComparison.OrdinalIgnoreCase) ?? false) ||
                (p.Direccion?.Contains(texto, StringComparison.OrdinalIgnoreCase) ?? false));
        }
        else
        {
            var dni = txtDni.Text.Trim();
            var nombre = txtNombre.Text.Trim();
            var apellido = txtApellido.Text.Trim();
            var telefono = txtTelefono.Text.Trim();
            var correo = txtCorreoElectronico.Text.Trim();
            var direccion = txtDireccion.Text.Trim();

            if (!string.IsNullOrWhiteSpace(dni))
                consulta = consulta.Where(p => p.DNI.Contains(dni, StringComparison.OrdinalIgnoreCase));
            if (!string.IsNullOrWhiteSpace(nombre))
                consulta = consulta.Where(p => p.Nombre.Contains(nombre, StringComparison.OrdinalIgnoreCase));
            if (!string.IsNullOrWhiteSpace(apellido))
                consulta = consulta.Where(p => p.Apellido.Contains(apellido, StringComparison.OrdinalIgnoreCase));
            if (!string.IsNullOrWhiteSpace(telefono))
                consulta = consulta.Where(p => p.Telefono != null && p.Telefono.Contains(telefono, StringComparison.OrdinalIgnoreCase));
            if (!string.IsNullOrWhiteSpace(correo))
                consulta = consulta.Where(p => p.CorreoElectronico != null && p.CorreoElectronico.Contains(correo, StringComparison.OrdinalIgnoreCase));
            if (!string.IsNullOrWhiteSpace(direccion))
                consulta = consulta.Where(p => p.Direccion != null && p.Direccion.Contains(direccion, StringComparison.OrdinalIgnoreCase));
        }

        var filtrados = consulta.ToList();
        MostrarPropietariosEnGrilla(filtrados);

        if (filtrados.Count > 0)
        {
            _ = SeleccionarPropietarioAsync(filtrados[0]);
        }
        else
        {
            LimpiarDetalles();
            MessageBox.Show(
                $"No se encontraron propietarios que coincidan{Environment.NewLine}con los criterios de búsqueda.",
                "Búsqueda de Propietario",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
        }
    }

    private void btnLimpiar_Click(object? sender, EventArgs e)
    {
        txtBuscar.Clear();
        if (cboFiltroEstado.Items.Count > 0)
            cboFiltroEstado.SelectedIndex = 0;
        LimpiarDetalles();
        MostrarPropietariosEnGrilla(_propietarios);
        dgvPropietarios.ClearSelection();
        txtDni.Focus();
    }

    private void btnVolver_Click(object? sender, EventArgs e)
    {
        Close();
    }
}
