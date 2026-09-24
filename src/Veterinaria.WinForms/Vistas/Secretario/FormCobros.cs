using System.Globalization;
using System.Text;
using Veterinaria.WinForms.Sesion;

namespace Veterinaria.WinForms.Vistas.Secretario;

/// <summary>
/// Maqueta visual de cobros en Secretaría con restricción numérica y validación.
/// </summary>
public partial class FormCobros : Form
{
    // Paleta de diseño: Ejecutivo Romántico Pastel
    private static readonly Color ColorFondoError = ColorTranslator.FromHtml("#FDECEF");
    private static readonly Color ColorEtiquetaError = ColorTranslator.FromHtml("#B85D69");
    private static readonly Color ColorTextoNormal = ColorTranslator.FromHtml("#3A353B");
    private static readonly Color ColorExito = ColorTranslator.FromHtml("#8FA89B");

    public FormCobros()
    {
        InitializeComponent();
        ConfigurarEventos();
    }

    /// <summary>
    /// Configura los eventos de entrada, restricciones numéricas y acciones de los botones.
    /// </summary>
    private void ConfigurarEventos()
    {
        // 1. Restricción en tiempo real de caracteres numéricos y separador decimal
        txtImporte.KeyPress += TxtImporte_KeyPress;

        // 2. Limpieza de estado de error y sanitización ante pegado
        txtImporte.TextChanged += TxtImporte_TextChanged;

        // 3. Formateo a 2 decimales al perder el foco
        txtImporte.Leave += TxtImporte_Leave;

        // 4. Acciones de botones
        btnCobrar.Click += BtnCobrar_Click;
        btnNuevo.Click += BtnNuevo_Click;
    }

    private void FormCobros_Load(object? sender, EventArgs e)
    {
        lblUsuarioSesion.Text = SesionActual.EstaAutenticado
            ? $"Recepción: {SesionActual.NombreCompleto} | {SesionActual.Rol}"
            : "Recepción: Secretario";

        cboConsultaClinica.Items.Clear();
        cboConsultaClinica.Items.AddRange(
        [
            "Seleccione una consulta clínica",
            "Consulta #101 - Luna (Canino) - Dr. Pérez",
            "Consulta #102 - Milo (Felino) - Dra. Gómez",
            "Consulta #103 - Thor (Canino) - Dr. Pérez"
        ]);
        cboConsultaClinica.SelectedIndex = 0;

        cboMetodoPago.Items.Clear();
        cboMetodoPago.Items.AddRange(
        [
            "Efectivo",
            "Tarjeta de débito",
            "Tarjeta de crédito",
            "Transferencia bancaria"
        ]);
        cboMetodoPago.SelectedIndex = 0;

        cboEstado.Items.Clear();
        cboEstado.Items.AddRange(["Pendiente", "Pagado"]);
        cboEstado.SelectedIndex = 0;

        dtpFecha.Value = DateTime.Today;
        lblInfoEstado.Text = "Módulo de cobros — recepción";
    }

    /// <summary>
    /// Restringe la entrada en el campo de importe exclusivamente a números (dígitos) y un único separador decimal (coma o punto).
    /// Bloquea de inmediato letras, símbolos y espacios.
    /// </summary>
    private void TxtImporte_KeyPress(object? sender, KeyPressEventArgs e)
    {
        // 1. Permitir teclas de control del sistema (retroceso, suprimir, tab, atajos)
        if (char.IsControl(e.KeyChar))
            return;

        // 2. Permitir exclusivamente dígitos numéricos (0 al 9)
        if (char.IsDigit(e.KeyChar))
            return;

        // 3. Permitir un único separador decimal (punto o coma)
        if (e.KeyChar is '.' or ',')
        {
            if (sender is TextBox tb)
            {
                var texto = tb.Text;
                var seleccion = tb.SelectedText;
                var yaTieneSeparador = (texto.Contains('.') || texto.Contains(',')) &&
                                       !(seleccion.Contains('.') || seleccion.Contains(','));

                if (!yaTieneSeparador)
                    return;
            }
        }

        // 4. Bloquear cualquier otro carácter (letras, espacios y símbolos no numéricos)
        e.Handled = true;
    }

    /// <summary>
    /// Restablece los estilos visuales ante la edición y previene caracteres no numéricos ingresados mediante pegado.
    /// </summary>
    private void TxtImporte_TextChanged(object? sender, EventArgs e)
    {
        RestaurarEstiloImporte();

        if (string.IsNullOrEmpty(txtImporte.Text))
            return;

        // Sanitización defensiva en caso de pegado con letras o símbolos desde el portapapeles
        var texto = txtImporte.Text;
        var textoLimpio = new StringBuilder();
        var tieneSeparador = false;

        foreach (var c in texto)
        {
            if (char.IsDigit(c))
            {
                textoLimpio.Append(c);
            }
            else if (c is '.' or ',' && !tieneSeparador)
            {
                textoLimpio.Append(c);
                tieneSeparador = true;
            }
        }

        if (textoLimpio.ToString() != texto)
        {
            var cursor = txtImporte.SelectionStart;
            txtImporte.Text = textoLimpio.ToString();
            txtImporte.SelectionStart = Math.Min(cursor, txtImporte.Text.Length);
        }
    }

    /// <summary>
    /// Al perder el foco, formatea el valor ingresado a formato estándar decimal con 2 decimales.
    /// </summary>
    private void TxtImporte_Leave(object? sender, EventArgs e)
    {
        var texto = txtImporte.Text.Trim().Replace(',', '.');
        if (decimal.TryParse(texto, CultureInfo.InvariantCulture, out var valor) && valor > 0)
        {
            txtImporte.Text = valor.ToString("0.00", CultureInfo.InvariantCulture);
            RestaurarEstiloImporte();
        }
    }

    /// <summary>
    /// Valida que el importe ingresado sea un número decimal positivo mayor a cero.
    /// </summary>
    private bool ValidarImporte(out decimal importe)
    {
        importe = 0m;
        var texto = txtImporte.Text.Trim().Replace(',', '.');

        if (string.IsNullOrWhiteSpace(texto))
        {
            MarcarErrorImporte("El importe es obligatorio.");
            return false;
        }

        if (!decimal.TryParse(texto, CultureInfo.InvariantCulture, out importe) || importe <= 0)
        {
            MarcarErrorImporte("El importe debe ser un número decimal mayor a 0.");
            return false;
        }

        RestaurarEstiloImporte();
        return true;
    }

    /// <summary>
    /// Aplica estilos visuales de error de la paleta Romántico Pastel al campo de importe.
    /// </summary>
    private void MarcarErrorImporte(string mensaje)
    {
        pnlImporte.BackColor = ColorFondoError;
        txtImporte.BackColor = ColorFondoError;
        lblSimboloPeso.BackColor = ColorFondoError;
        lblImporte.ForeColor = ColorEtiquetaError;
        lblInfoEstado.ForeColor = ColorEtiquetaError;
        lblInfoEstado.Text = mensaje;
        txtImporte.Focus();
        txtImporte.SelectAll();
    }

    /// <summary>
    /// Restablece los estilos normales del campo de importe.
    /// </summary>
    private void RestaurarEstiloImporte()
    {
        pnlImporte.BackColor = Color.White;
        txtImporte.BackColor = Color.White;
        lblSimboloPeso.BackColor = Color.White;
        lblImporte.ForeColor = ColorTextoNormal;
        lblInfoEstado.ForeColor = ColorTextoNormal;
        lblInfoEstado.Text = "Módulo de cobros — recepción";
    }

    /// <summary>
    /// Registra el cobro en la grilla visual previa validación estricta de consulta e importe numérico.
    /// </summary>
    private void BtnCobrar_Click(object? sender, EventArgs e)
    {
        if (cboConsultaClinica.SelectedIndex <= 0)
        {
            lblInfoEstado.ForeColor = ColorEtiquetaError;
            lblInfoEstado.Text = "Por favor, seleccione una consulta clínica para registrar el cobro.";
            cboConsultaClinica.Focus();
            return;
        }

        if (!ValidarImporte(out var importe))
        {
            return;
        }

        var nuevoId = dgvCobros.Rows.Count + 1;
        var consulta = cboConsultaClinica.SelectedItem?.ToString() ?? "Consulta General";
        var metodo = cboMetodoPago.SelectedItem?.ToString() ?? "Efectivo";
        var fecha = dtpFecha.Value.ToString("dd/MM/yyyy");
        var importeFormateado = $"$ {importe:N2}";
        var estado = cboEstado.SelectedItem?.ToString() ?? "Pagado";

        dgvCobros.Rows.Add(nuevoId, consulta, metodo, fecha, importeFormateado, estado);

        lblInfoEstado.ForeColor = ColorExito;
        lblInfoEstado.Text = $"Cobro #{nuevoId} registrado exitosamente ({importeFormateado}).";

        // Preparar para siguiente cobro
        txtImporte.Clear();
        cboConsultaClinica.SelectedIndex = 0;
        cboMetodoPago.SelectedIndex = 0;
        cboEstado.SelectedIndex = 0;
        RestaurarEstiloImporte();
    }

    /// <summary>
    /// Limpia los campos de entrada y restablece el formulario.
    /// </summary>
    private void BtnNuevo_Click(object? sender, EventArgs e)
    {
        txtImporte.Clear();
        cboConsultaClinica.SelectedIndex = 0;
        cboMetodoPago.SelectedIndex = 0;
        cboEstado.SelectedIndex = 0;
        dtpFecha.Value = DateTime.Today;
        RestaurarEstiloImporte();
        cboConsultaClinica.Focus();
    }

    private void btnCancelar_Click(object? sender, EventArgs e)
    {
        Close();
    }
}
