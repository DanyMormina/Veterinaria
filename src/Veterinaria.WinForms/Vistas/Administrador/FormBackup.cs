using Veterinaria.Controllers.Sistema;
using Veterinaria.Domain.Dtos;
using Veterinaria.WinForms.Sesion;

namespace Veterinaria.WinForms.Vistas.Administrador;

/// <summary>
/// Vista administrativa para la configuración, generación y consulta histórica de copias de seguridad (Backups).
/// Diseñada bajo el estándar visual "Ejecutivo Romántico Pastel".
/// </summary>
public partial class FormBackup : Form
{
    private readonly BackupController _backupController;

    /// <summary>
    /// Constructor principal que recibe el controlador mediante inyección de dependencias.
    /// </summary>
    /// <param name="backupController">Controlador del módulo de copias de seguridad.</param>
    public FormBackup(BackupController backupController)
    {
        _backupController = backupController;
        InitializeComponent();

        // Enlace de eventos del ciclo de vida del formulario
        Load += FormBackup_Load;
        btnGenerarBackup.Click += btnGenerarBackup_Click;
        btnVolver.Click += (s, e) => Close();
    }

    /// <summary>
    /// Inicializa la sesión, la ruta configurada, la sugerencia del nombre de archivo y la grilla de historial.
    /// </summary>
    private async void FormBackup_Load(object? sender, EventArgs e)
    {
        // 1. Cabecera institucional con usuario en sesión
        lblUsuarioSesion.Text = SesionActual.EstaAutenticado
            ? $"Usuario: {SesionActual.NombreUsuario} | Rol: {SesionActual.Rol}"
            : "Usuario: Admin";

        // 2. Mostrar la ruta fija configurada para el sistema
        string rutaDestino = _backupController.ObtenerDirectorioDestino();
        lblRutaInfo.Text = $"Ruta de guardado: {rutaDestino}\\";

        // 3. Generar nombre de archivo sugerido con fecha y hora actual
        SugerirNuevoNombreArchivo();

        // 4. Aplicar estilos oficiales de tabla
        ConfigurarEstiloGrilla();

        // 5. Cargar historial de copias de seguridad asíncronamente
        await CargarHistorialAsync();
    }

    /// <summary>
    /// Genera y asigna un nombre sugerido automático con marca de tiempo precisa.
    /// </summary>
    private void SugerirNuevoNombreArchivo()
    {
        txtNombreArchivo.Text = $"Backup_{DateTime.Now:yyyyMMdd_HHmmss}";
    }

    /// <summary>
    /// Configura la apariencia de DataGridView según las reglas de GEMINI.md.
    /// </summary>
    private void ConfigurarEstiloGrilla()
    {
        dgvBackups.AutoGenerateColumns = false;
        dgvBackups.EnableHeadersVisualStyles = false;
        dgvBackups.ColumnHeadersDefaultCellStyle.BackColor = Color.White;
        dgvBackups.ColumnHeadersDefaultCellStyle.ForeColor = Color.Black;
        dgvBackups.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
        dgvBackups.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
        dgvBackups.ColumnHeadersHeight = 34;

        dgvBackups.DefaultCellStyle.Font = new Font("Segoe UI", 9F);
        dgvBackups.DefaultCellStyle.ForeColor = Color.FromArgb(58, 53, 59);
        dgvBackups.DefaultCellStyle.SelectionBackColor = Color.FromArgb(226, 217, 220);
        dgvBackups.DefaultCellStyle.SelectionForeColor = Color.FromArgb(58, 53, 59);
        dgvBackups.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(250, 244, 244);
        dgvBackups.RowHeadersVisible = false;
        dgvBackups.RowTemplate.Height = 30;
    }

    /// <summary>
    /// Consulta y puebla la grilla central con el historial registrado.
    /// </summary>
    private async Task CargarHistorialAsync()
    {
        var resultadoHistorial = await _backupController.CargarHistorialAsync();

        if (resultadoHistorial.EsExitoso && resultadoHistorial.Valor != null)
        {
            var lista = resultadoHistorial.Valor.ToList();
            dgvBackups.DataSource = lista;
            lblTotalBackups.Text = $"Total de copias registradas: {lista.Count}";
        }
        else
        {
            dgvBackups.DataSource = new List<BackupHistorialDto>();
            lblTotalBackups.Text = "No se pudieron obtener registros históricos.";
        }
    }

    /// <summary>
    /// Dispara el proceso de respaldo validando la sesión y gestionando el feedback visual.
    /// </summary>
    private async void btnGenerarBackup_Click(object? sender, EventArgs e)
    {
        // 1. Bloqueo temporal y cursor de espera
        btnGenerarBackup.Enabled = false;
        btnVolver.Enabled = false;
        Cursor = Cursors.WaitCursor;

        try
        {
            // 2. Extraer usuario responsable
            long? idUsuarioSesion = SesionActual.EstaAutenticado
                ? SesionActual.IdUsuario
                : 1; // Fallback al administrador predeterminado si se prueba en modo desacoplado

            // 3. Ejecutar backup vía controlador
            string nombreIngresado = txtNombreArchivo.Text.Trim();
            var resultado = await _backupController.RealizarBackupAsync(nombreIngresado, idUsuarioSesion);

            if (resultado.EsExitoso)
            {
                // Éxito: mensaje informativo, refresco de grilla y nuevo nombre sugerido
                MessageBox.Show(
                    resultado.Mensaje,
                    "Copia de Seguridad Completada",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                SugerirNuevoNombreArchivo();
                await CargarHistorialAsync();
            }
            else
            {
                txtNombreArchivo.Focus();

                MessageBox.Show(
                    resultado.Mensaje,
                    "Error al Generar Backup",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show(
                $"Ocurrió un error inesperado durante el respaldo:\n{ex.Message}",
                "Error Inesperado",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
        }
        finally
        {
            // 4. Restauración de controles y cursor
            btnGenerarBackup.Enabled = true;
            btnVolver.Enabled = true;
            Cursor = Cursors.Default;
        }
    }
}
