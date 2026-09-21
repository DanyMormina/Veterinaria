using System.Globalization;
using Veterinaria.Controllers.Controladores;
using Veterinaria.Domain.Dtos;

namespace Veterinaria.WinForms.Vistas.Veterinario;

/// <summary>
/// Diálogo modal auxiliar para dar de alta una nueva vacuna en el catálogo médico veterinario.
/// </summary>
public partial class FormAltaVacuna : Form
{
    private readonly VacunaControlador? _vacunaControlador;
    private readonly EspecieControlador? _especieControlador;
    private readonly long? _idEspecieSugerida;

    private static readonly CultureInfo CulturaArgentina = new("es-AR");

    public FormAltaVacuna() : this(null, null, null)
    {
    }

    public FormAltaVacuna(
        VacunaControlador? vacunaControlador,
        EspecieControlador? especieControlador = null,
        long? idEspecieSugerida = null)
    {
        InitializeComponent();
        _vacunaControlador = vacunaControlador;
        _especieControlador = especieControlador;
        _idEspecieSugerida = idEspecieSugerida;

        ConfigurarEventos();
    }

    private void ConfigurarEventos()
    {
        Load += FormAltaVacuna_Load;
        txtPrecioUnitario.KeyPress += TxtPrecioUnitario_KeyPress;
    }

    private async void FormAltaVacuna_Load(object? sender, EventArgs e)
    {
        await CargarEspeciesAsync();
        ActiveControl = txtNombre;
    }

    private async Task CargarEspeciesAsync()
    {
        List<EspecieItemVista> especies = [];

        // 1. Intentar cargar especies registradas desde el controlador
        if (_especieControlador is not null)
        {
            var resultado = await _especieControlador.ObtenerTodosAsync();
            if (resultado.EsExitoso && resultado.Valor is not null)
            {
                especies = resultado.Valor
                    .Where(esp => esp.Activo)
                    .Select(esp => new EspecieItemVista { Id = esp.Id, Nombre = esp.Nombre })
                    .ToList();
            }
        }

        // 2. Si no hay controlador o está vacía, utilizar catálogo estándar del sistema
        if (especies.Count == 0)
        {
            especies =
            [
                new EspecieItemVista { Id = 1, Nombre = "Canino" },
                new EspecieItemVista { Id = 2, Nombre = "Felino" },
                new EspecieItemVista { Id = 3, Nombre = "Ave" },
                new EspecieItemVista { Id = 4, Nombre = "Roedor" }
            ];
        }

        cboEspecie.DataSource = especies;
        cboEspecie.DisplayMember = nameof(EspecieItemVista.Nombre);
        cboEspecie.ValueMember = nameof(EspecieItemVista.Id);

        // 3. Preseleccionar especie sugerida si corresponde
        if (_idEspecieSugerida.HasValue && especies.Any(e => e.Id == _idEspecieSugerida.Value))
        {
            cboEspecie.SelectedValue = _idEspecieSugerida.Value;
        }
        else if (especies.Count > 0)
        {
            cboEspecie.SelectedIndex = 0;
        }
    }

    private void TxtPrecioUnitario_KeyPress(object? sender, KeyPressEventArgs e)
    {
        // Permitir dígitos numéricos y teclas de control
        if (char.IsControl(e.KeyChar) || char.IsDigit(e.KeyChar))
        {
            return;
        }

        // Permitir un único separador decimal (, o .)
        if ((e.KeyChar == ',' || e.KeyChar == '.') &&
            !txtPrecioUnitario.Text.Contains(',') &&
            !txtPrecioUnitario.Text.Contains('.'))
        {
            return;
        }

        e.Handled = true;
    }

    private async void btnGuardar_Click(object? sender, EventArgs e)
    {
        // 1. Validar que se disponga de acceso al controlador
        if (_vacunaControlador is null)
        {
            MessageBox.Show(
                "No se pudo acceder al controlador del catálogo de vacunas.",
                "Error de Dependencia",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
            return;
        }

        // 2. Validar campos requeridos
        var nombre = txtNombre.Text.Trim();
        if (string.IsNullOrWhiteSpace(nombre))
        {
            MessageBox.Show(
                "El nombre comercial de la vacuna es obligatorio.",
                "Dato Requerido",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning);
            txtNombre.Focus();
            return;
        }

        if (cboEspecie.SelectedValue is not long idEspecie || idEspecie <= 0)
        {
            MessageBox.Show(
                "Debe seleccionar la especie a la que corresponde la vacuna.",
                "Dato Requerido",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning);
            cboEspecie.Focus();
            return;
        }

        // 3. Parsear precio unitario
        var textoPrecio = txtPrecioUnitario.Text.Trim().Replace("$", string.Empty).Trim();
        if (!decimal.TryParse(textoPrecio, NumberStyles.Any, CulturaArgentina, out var precio) &&
            !decimal.TryParse(textoPrecio, NumberStyles.Any, CultureInfo.InvariantCulture, out precio))
        {
            MessageBox.Show(
                "El precio unitario ingresado no posee un formato numérico válido.",
                "Formato Inválido",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning);
            txtPrecioUnitario.Focus();
            return;
        }

        if (precio < 0)
        {
            MessageBox.Show(
                "El precio unitario no puede ser un valor negativo.",
                "Valor Inválido",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning);
            txtPrecioUnitario.Focus();
            return;
        }

        // 4. Crear solicitud de alta en catálogo
        var solicitud = new VacunaSolicitudDto
        {
            IdEspecie = idEspecie,
            Nombre = nombre,
            PeriodoMesesRecomendado = (int)numPeriodoMeses.Value,
            Precio = precio
        };

        btnGuardar.Enabled = false;

        try
        {
            // 5. Ejecutar creación asíncrona mediante Result pattern
            var resultado = await _vacunaControlador.CrearAsync(solicitud);

            if (!resultado.EsExitoso)
            {
                MessageBox.Show(
                    resultado.Mensaje,
                    "Error al Guardar Vacuna",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                btnGuardar.Enabled = true;
                return;
            }

            MessageBox.Show(
                "Vacuna incorporada al catálogo exitosamente.",
                "Registro Exitoso",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);

            DialogResult = DialogResult.OK;
            Close();
        }
        catch (Exception ex)
        {
            MessageBox.Show(
                $"Ocurrió un error inesperado: {ex.Message}",
                "Error Inesperado",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
            btnGuardar.Enabled = true;
        }
    }

    private void btnCancelar_Click(object? sender, EventArgs e)
    {
        DialogResult = DialogResult.Cancel;
        Close();
    }

    private sealed class EspecieItemVista
    {
        public long Id { get; init; }
        public string Nombre { get; init; } = string.Empty;
    }
}
