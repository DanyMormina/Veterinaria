using System.Drawing.Printing;
using System.Text;
using Veterinaria.Controllers.Controladores;
using Veterinaria.Domain.Dtos;
using Veterinaria.WinForms.Sesion;

namespace Veterinaria.WinForms.Vistas.Administrador;

/// <summary>
/// Vista administrativa para la generación, visualización, impresión y exportación
/// de reportes clínicos y demográficos de la clínica veterinaria.
/// </summary>
public partial class FormReportes : Form
{
    private readonly ReporteControlador _reporteControlador;
    private readonly EspecieControlador _especieControlador;

    // Estado para renderizado y paginación en PrintDocument
    private int _filaActualImpresion = 0;
    private int _paginaActual = 0;
    private string _tituloReporteActual = string.Empty;
    private string _filtrosReporteActual = string.Empty;

    public FormReportes(ReporteControlador reporteControlador, EspecieControlador especieControlador)
    {
        InitializeComponent();
        _reporteControlador = reporteControlador;
        _especieControlador = especieControlador;
    }

    /// <summary>
    /// Inicializa la sesión, configura los estilos del sistema de diseño romántico pastel,
    /// y realiza la carga asíncrona de catálogos y filtros iniciales.
    /// </summary>
    private async void FormReportes_Load(object? sender, EventArgs e)
    {
        lblUsuarioSesion.Text = SesionActual.EstaAutenticado
            ? $"Usuario: {SesionActual.NombreCompleto} | Rol: {SesionActual.Rol}"
            : "Usuario: Administrador";

        // Ajustes de componentes según alcance clínico (Facturación excluida)
        lblTotalFacturado.Visible = false;
        lblTotalRegistros.Text = "Total de registros: 0";
        btnImprimir.Enabled = false;
        btnExportarPdf.Enabled = false;

        ConfigurarEstiloGrilla();

        // Limpieza de feedback visual en contenedor de fechas ante interacción
        dtpFechaDesde.ValueChanged += (s, ev) => RestaurarBordeFechas();
        dtpFechaHasta.ValueChanged += (s, ev) => RestaurarBordeFechas();
        dtpFechaDesde.Enter += (s, ev) => RestaurarBordeFechas();
        dtpFechaHasta.Enter += (s, ev) => RestaurarBordeFechas();

        // Rango de fechas por defecto: primer día del mes actual hasta la fecha de hoy
        var hoy = DateTime.Today;
        dtpFechaDesde.Value = new DateTime(hoy.Year, hoy.Month, 1);
        dtpFechaHasta.Value = hoy;

        // Inicialización de tipos de reporte administrativo
        cboTipoReporte.Items.Clear();
        cboTipoReporte.Items.AddRange(new object[]
        {
            "Consultas Clínicas",
            "Censo de Pacientes (Mascotas)"
        });
        cboTipoReporte.SelectedIndex = 0;

        // Carga asíncrona de veterinarios y especies
        await CargarVeterinariosAsync();
        await CargarEspeciesAsync();
    }

    /// <summary>
    /// Aplica la paleta de colores oficial "Ejecutivo Romántico Pastel" a la grilla de resultados.
    /// </summary>
    private void ConfigurarEstiloGrilla()
    {
        dgvReporte.AutoGenerateColumns = false;
        dgvReporte.EnableHeadersVisualStyles = false;
        dgvReporte.ColumnHeadersDefaultCellStyle.BackColor = Color.White;
        dgvReporte.ColumnHeadersDefaultCellStyle.ForeColor = Color.Black;
        dgvReporte.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
        dgvReporte.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
        dgvReporte.ColumnHeadersHeight = 32;

        dgvReporte.DefaultCellStyle.Font = new Font("Segoe UI", 9F);
        dgvReporte.DefaultCellStyle.ForeColor = Color.FromArgb(58, 53, 59); // Gris pizarra carbón (#3A353B)
        dgvReporte.DefaultCellStyle.SelectionBackColor = Color.FromArgb(226, 217, 220); // Gris pastel (#E2D9DC)
        dgvReporte.DefaultCellStyle.SelectionForeColor = Color.FromArgb(58, 53, 59);
        dgvReporte.RowHeadersVisible = false;
        dgvReporte.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(250, 244, 244); // Crema rubor (#FAF4F4)
        dgvReporte.RowTemplate.Height = 28;
    }

    /// <summary>
    /// Carga la lista de veterinarios activos en el ComboBox de profesionales.
    /// </summary>
    private async Task CargarVeterinariosAsync()
    {
        try
        {
            var resultado = await _reporteControlador.ObtenerVeterinariosActivosAsync();
            var lista = new List<UsuarioRespuestaDto>
            {
                new UsuarioRespuestaDto { Id = 0, Nombre = "(Todos)", Apellido = string.Empty, Activo = true }
            };

            if (resultado.EsExitoso && resultado.Valor != null)
            {
                lista.AddRange(resultado.Valor);
            }

            cboVeterinario.DisplayMember = "NombreCompleto";
            cboVeterinario.ValueMember = "Id";
            cboVeterinario.DataSource = lista;
            cboVeterinario.SelectedIndex = 0;
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error al cargar profesionales veterinarios: {ex.Message}", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
    }

    /// <summary>
    /// Carga la lista de especies activas en el ComboBox de filtro por especie.
    /// </summary>
    private async Task CargarEspeciesAsync()
    {
        try
        {
            var resultado = await _especieControlador.ObtenerTodosAsync();
            var lista = new List<EspecieRespuestaDto>
            {
                new EspecieRespuestaDto { Id = 0, Nombre = "(Todas)", Activo = true }
            };

            if (resultado.EsExitoso && resultado.Valor != null)
            {
                lista.AddRange(resultado.Valor.Where(e => e.Activo));
            }

            cboEspecie.DisplayMember = "Nombre";
            cboEspecie.ValueMember = "Id";
            cboEspecie.DataSource = lista;
            cboEspecie.SelectedIndex = 0;
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error al cargar especies: {ex.Message}", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
    }

    /// <summary>
    /// Adapta la disponibilidad de filtros de acuerdo a la naturaleza del reporte seleccionado.
    /// </summary>
    private void cboTipoReporte_SelectedIndexChanged(object? sender, EventArgs e)
    {
        RestaurarBordeFechas();
        dgvReporte.DataSource = null;
        dgvReporte.Columns.Clear();
        lblTotalRegistros.Text = "Total de registros: 0";
        btnImprimir.Enabled = false;
        btnExportarPdf.Enabled = false;

        string tipo = cboTipoReporte.SelectedItem?.ToString() ?? string.Empty;

        switch (tipo)
        {
            case "Consultas Clínicas":
                dtpFechaDesde.Enabled = true;
                dtpFechaHasta.Enabled = true;
                cboVeterinario.Enabled = true;
                cboEspecie.Enabled = true;
                break;

            case "Censo de Pacientes (Mascotas)":
                dtpFechaDesde.Enabled = false; // El censo es sobre el padrón activo actual
                dtpFechaHasta.Enabled = false;
                cboVeterinario.Enabled = false;
                cboEspecie.Enabled = true;
                break;

            default:
                dtpFechaDesde.Enabled = true;
                dtpFechaHasta.Enabled = true;
                cboVeterinario.Enabled = true;
                cboEspecie.Enabled = true;
                break;
        }
    }

    /// <summary>
    /// Restablece el color del contenedor de fechas a su estado neutral.
    /// </summary>
    private void RestaurarBordeFechas()
    {
        pnlContenedorFechas.BackColor = Color.FromArgb(226, 217, 220); // Gris pastel neutro (#E2D9DC)
    }

    /// <summary>
    /// Valida la consistencia de los filtros antes de iniciar la consulta.
    /// </summary>
    private bool ValidarFiltros()
    {
        string tipo = cboTipoReporte.SelectedItem?.ToString() ?? string.Empty;

        // Si el reporte actual requiere fechas, validar cronología
        if (tipo != "Censo de Pacientes (Mascotas)")
        {
            if (dtpFechaDesde.Value.Date > dtpFechaHasta.Value.Date)
            {
                pnlContenedorFechas.BackColor = Color.FromArgb(184, 93, 105); // Borgoña suave (#B85D69)
                dtpFechaDesde.Focus();
                MessageBox.Show(
                    "La fecha inicial ('Fecha desde') no puede ser posterior a la fecha final ('Fecha hasta').",
                    "Rango de fechas inválido",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return false;
            }
        }

        RestaurarBordeFechas();
        return true;
    }

    /// <summary>
    /// Ejecuta la consulta del reporte seleccionado y reconstruye dinámicamente las columnas de la grilla.
    /// </summary>
    private async void btnGenerar_Click(object? sender, EventArgs e)
    {
        if (!ValidarFiltros())
            return;

        try
        {
            btnGenerar.Enabled = false;
            Cursor = Cursors.WaitCursor;

            string tipo = cboTipoReporte.SelectedItem?.ToString() ?? string.Empty;
            long? idVet = (cboVeterinario.SelectedValue is long vId && vId > 0) ? vId : null;
            long? idEsp = (cboEspecie.SelectedValue is long eId && eId > 0) ? eId : null;

            var filtro = new FiltroReporteDto
            {
                TipoReporte = tipo,
                FechaDesde = dtpFechaDesde.Value.Date,
                FechaHasta = dtpFechaHasta.Value.Date,
                IdVeterinario = idVet,
                IdEspecie = idEsp
            };

            // Metadatos para encabezados impresos y exportados
            _tituloReporteActual = tipo.ToUpperInvariant();
            _filtrosReporteActual = tipo == "Censo de Pacientes (Mascotas)"
                ? $"Especie: {(idEsp.HasValue ? cboEspecie.Text : "Todas")}"
                : $"Rango: {filtro.FechaDesde:dd/MM/yyyy} al {filtro.FechaHasta:dd/MM/yyyy} | Profesional: {(idVet.HasValue ? cboVeterinario.Text : "Todos")} | Especie: {(idEsp.HasValue ? cboEspecie.Text : "Todas")}";

            switch (tipo)
            {
                case "Consultas Clínicas":
                    await GenerarReporteConsultasClinicasAsync(filtro);
                    break;
                case "Censo de Pacientes (Mascotas)":
                    await GenerarReporteCensoMascotasAsync(filtro);
                    break;
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show(
                $"Ocurrió un error inesperado al generar el reporte: {ex.Message}",
                "Error",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
        }
        finally
        {
            btnGenerar.Enabled = true;
            Cursor = Cursors.Default;
        }
    }

    /// <summary>
    /// Configura y renderiza el reporte de Consultas Clínicas.
    /// </summary>
    private async Task GenerarReporteConsultasClinicasAsync(FiltroReporteDto filtro)
    {
        var res = await _reporteControlador.ObtenerReporteConsultasClinicasAsync(filtro);
        if (!res.EsExitoso || res.Valor == null)
        {
            MessageBox.Show(res.Mensaje, "Error al generar reporte", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return;
        }

        dgvReporte.DataSource = null;
        dgvReporte.Columns.Clear();

        dgvReporte.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "Fecha", HeaderText = "Fecha", Width = 95 });
        dgvReporte.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "Hora", HeaderText = "Hora", Width = 65 });
        dgvReporte.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "Mascota", HeaderText = "Mascota", Width = 130 });
        dgvReporte.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "Especie", HeaderText = "Especie", Width = 110 });
        dgvReporte.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "Raza", HeaderText = "Raza", Width = 120 });
        dgvReporte.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "Propietario", HeaderText = "Propietario", Width = 160 });
        dgvReporte.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "Veterinario", HeaderText = "Veterinario", Width = 160 });
        dgvReporte.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "Diagnostico", HeaderText = "Diagnóstico / Motivo", AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill });

        var datos = res.Valor.ToList();
        dgvReporte.DataSource = datos;
        ActualizarResumen(datos.Count);
    }


    /// <summary>
    /// Configura y renderiza el Censo Demográfico de Mascotas registradas.
    /// </summary>
    private async Task GenerarReporteCensoMascotasAsync(FiltroReporteDto filtro)
    {
        var res = await _reporteControlador.ObtenerReporteCensoMascotasAsync(filtro);
        if (!res.EsExitoso || res.Valor == null)
        {
            MessageBox.Show(res.Mensaje, "Error al generar reporte", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return;
        }

        dgvReporte.DataSource = null;
        dgvReporte.Columns.Clear();

        dgvReporte.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "Id", HeaderText = "ID", Width = 60 });
        dgvReporte.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "Nombre", HeaderText = "Mascota", Width = 130 });
        dgvReporte.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "Especie", HeaderText = "Especie", Width = 110 });
        dgvReporte.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "Raza", HeaderText = "Raza", Width = 120 });
        dgvReporte.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "Sexo", HeaderText = "Sexo", Width = 90 });
        dgvReporte.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "FechaNacimientoFormateada", HeaderText = "Fec. Nacimiento", Width = 110 });
        dgvReporte.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "Color", HeaderText = "Color", Width = 110 });
        dgvReporte.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "Propietario", HeaderText = "Propietario", AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill });

        var datos = res.Valor.ToList();
        dgvReporte.DataSource = datos;
        ActualizarResumen(datos.Count);
    }

    /// <summary>
    /// Actualiza las métricas del pie y el estado de habilitación de exportación e impresión.
    /// </summary>
    private void ActualizarResumen(int total)
    {
        lblTotalRegistros.Text = $"Total de registros: {total}";
        btnImprimir.Enabled = total > 0;
        btnExportarPdf.Enabled = total > 0;

        if (total == 0)
        {
            MessageBox.Show(
                "No se encontraron registros que coincidan con los filtros seleccionados.",
                "Sin resultados",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
        }
    }

    /// <summary>
    /// Construye y configura el objeto PrintDocument para orientación apaisada y márgenes óptimos.
    /// </summary>
    private PrintDocument CrearPrintDocument()
    {
        // 1. Instanciamos el componente estándar de .NET para impresión (System.Drawing.Printing)
        var pd = new PrintDocument();

        // 2. Definimos orientación horizontal (apaisada/landscape) y márgenes de 40 puntos para que las tablas encajen bien
        pd.DefaultPageSettings.Landscape = true;
        pd.DefaultPageSettings.Margins = new Margins(40, 40, 40, 40);

        // 3. Al iniciar cada trabajo de impresión, reiniciamos los contadores de página y fila para arrancar desde cero
        pd.BeginPrint += (s, ev) =>
        {
            _filaActualImpresion = 0;
            _paginaActual = 0;
        };

        // 4. Conectamos el evento PrintPage con nuestro método que dibuja fila por fila con GDI+
        pd.PrintPage += ImprimirPaginaReporte;
        return pd;
    }

    /// <summary>
    /// Abre el cuadro de diálogo de vista previa de impresión.
    /// </summary>
    private void btnImprimir_Click(object? sender, EventArgs e)
    {
        // Validación: no permitir imprimir si la grilla está vacía
        if (dgvReporte.Rows.Count == 0)
        {
            MessageBox.Show("No hay datos en la grilla para imprimir.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        try
        {
            // Creamos el documento de impresión configurado
            using var pd = CrearPrintDocument();

            // Usamos la ventana estándar de Windows Forms para previsualizar las hojas antes de mandar a la impresora física
            using var printDialog = new PrintPreviewDialog
            {
                Document = pd,
                Width = 1050,
                Height = 720,
                StartPosition = FormStartPosition.CenterScreen,
                Text = $"Vista Previa de Impresión — {_tituloReporteActual}"
            };
            printDialog.ShowDialog(this);
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error al preparar la vista previa de impresión: {ex.Message}", "Error de Impresión", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    /// <summary>
    /// Exporta los datos a formato PDF (o CSV como alternativa compatible) según las capacidades del sistema.
    /// </summary>
    private void btnExportarPdf_Click(object? sender, EventArgs e)
    {
        // Validación: verificar que existan registros cargados en pantalla
        if (dgvReporte.Rows.Count == 0)
        {
            MessageBox.Show("No hay datos en la grilla para exportar.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        // Diálogo para que el usuario elija dónde guardar el archivo y con qué nombre
        using var sfd = new SaveFileDialog
        {
            Filter = "Archivo PDF (*.pdf)|*.pdf|Archivo CSV (*.csv)|*.csv",
            FileName = $"Reporte_{_tituloReporteActual.Replace(' ', '_')}_{DateTime.Now:yyyyMMdd_HHmm}.pdf",
            Title = "Exportar Reporte"
        };

        // Si el usuario canceló la ventana de guardado, no hacemos nada
        if (sfd.ShowDialog(this) != DialogResult.OK)
            return;

        try
        {
            // OPCIÓN 1: Si el usuario seleccionó guardar directamente en formato CSV (texto delimitado por punto y coma)
            if (sfd.FileName.EndsWith(".csv", StringComparison.OrdinalIgnoreCase))
            {
                ExportarCsv(sfd.FileName);
                MessageBox.Show("El reporte ha sido exportado exitosamente a formato CSV.", "Exportación Exitosa", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            // OPCIÓN 2: Exportación a PDF sin librerías externas de terceros.
            // Para no depender de librerías pesadas o de pago (como iText, QuestPDF, etc.),
            // aprovechamos la impresora virtual nativa de Windows: "Microsoft Print to PDF".
            // Aquí comprobamos si está instalada en el sistema operativo del usuario.
            bool impresoraPdfDisponible = PrinterSettings.InstalledPrinters
                .Cast<string>()
                .Any(p => p.Equals("Microsoft Print to PDF", StringComparison.OrdinalIgnoreCase));

            // Si por alguna razón la impresora virtual de Windows no está instalada o está deshabilitada:
            if (!impresoraPdfDisponible)
            {
                var respuesta = MessageBox.Show(
                    "La impresora virtual 'Microsoft Print to PDF' no se encuentra disponible en este equipo. ¿Desea exportar los datos en formato CSV compatible con Excel?",
                    "Impresora PDF no disponible",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (respuesta == DialogResult.Yes)
                {
                    // Cambiamos la extensión a .csv y generamos el archivo alternativo
                    var rutaCsv = Path.ChangeExtension(sfd.FileName, ".csv");
                    ExportarCsv(rutaCsv);
                    MessageBox.Show($"Reporte exportado exitosamente a:\n{rutaCsv}", "Exportación Exitosa", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                return;
            }

            // Si "Microsoft Print to PDF" sí está disponible, la usamos de forma silenciosa para generar el PDF:
            using var pd = CrearPrintDocument();

            // 1. Asignamos la impresora virtual de Windows como destino
            pd.PrinterSettings.PrinterName = "Microsoft Print to PDF";

            // 2. Le indicamos a Windows que la salida no saldrá en papel, sino que se escribirá a un archivo
            pd.PrinterSettings.PrintToFile = true;

            // 3. Establecemos la ruta y nombre exacto del archivo PDF que seleccionó el usuario
            pd.PrinterSettings.PrintFileName = sfd.FileName;

            // 4. Ejecutamos la impresión (esto ejecuta el motor gráfico ImprimirPaginaReporte y vuelca todo al PDF)
            pd.Print();

            MessageBox.Show($"El reporte se exportó correctamente como archivo PDF en:\n{sfd.FileName}", "Exportación Exitosa", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error al exportar el reporte: {ex.Message}", "Error de Exportación", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    /// <summary>
    /// Genera un archivo CSV con formato UTF-8 y delimitador estándar de lista.
    /// </summary>
    private void ExportarCsv(string rutaArchivo)
    {
        var sb = new StringBuilder();

        var columnasVisibles = dgvReporte.Columns.Cast<DataGridViewColumn>()
            .Where(c => c.Visible)
            .OrderBy(c => c.DisplayIndex)
            .ToList();

        // Encabezados
        sb.AppendLine(string.Join(";", columnasVisibles.Select(c => $"\"{c.HeaderText.Replace("\"", "\"\"")}\"")));

        // Filas
        foreach (DataGridViewRow row in dgvReporte.Rows)
        {
            if (row.IsNewRow) continue;
            var celdas = columnasVisibles.Select(c =>
            {
                var val = row.Cells[c.Index].Value?.ToString() ?? string.Empty;
                return $"\"{val.Replace("\"", "\"\"")}\"";
            });
            sb.AppendLine(string.Join(";", celdas));
        }

        File.WriteAllText(rutaArchivo, sb.ToString(), Encoding.UTF8);
    }

    /// <summary>
    /// Dibuja cada página de impresión respetando paginación, proporciones de columnas y diseño institucional.
    /// </summary>
    private void ImprimirPaginaReporte(object sender, PrintPageEventArgs e)
    {
        _paginaActual++;
        var g = e.Graphics;
        if (g == null) return;

        var bounds = e.MarginBounds;

        using var fontTitulo = new Font("Segoe UI", 12F, FontStyle.Bold);
        using var fontSubtitulo = new Font("Segoe UI", 9.5F, FontStyle.Bold);
        using var fontInfo = new Font("Segoe UI", 8F, FontStyle.Regular);
        using var fontCabecera = new Font("Segoe UI", 8.5F, FontStyle.Bold);
        using var fontCelda = new Font("Segoe UI", 8F, FontStyle.Regular);
        using var fontPie = new Font("Segoe UI", 8F, FontStyle.Italic);

        using var brushTexto = new SolidBrush(Color.FromArgb(58, 53, 59)); // #3A353B
        using var brushPrimario = new SolidBrush(Color.FromArgb(200, 138, 150)); // #C88A96
        using var brushCabeceraFondo = new SolidBrush(Color.FromArgb(240, 235, 237));
        using var brushFilaAlt = new SolidBrush(Color.FromArgb(250, 244, 244));
        using var penSeparador = new Pen(Color.FromArgb(200, 138, 150), 1.5f);
        using var penLinea = new Pen(Color.FromArgb(226, 217, 220), 1f);

        float y = bounds.Top;

        // Encabezado institucional
        if (_paginaActual == 1)
        {
            g.DrawString("CLÍNICA VETERINARIA — GESTIÓN ADMINISTRATIVA", fontTitulo, brushPrimario, bounds.Left, y);
            y += 20;

            g.DrawString($"REPORTE: {_tituloReporteActual}", fontSubtitulo, brushTexto, bounds.Left, y);
            y += 18;

            g.DrawString($"Filtros: {_filtrosReporteActual}", fontInfo, brushTexto, bounds.Left, y);
            y += 15;

            g.DrawString($"Emisión: {DateTime.Now:dd/MM/yyyy HH:mm} | Operador: {(SesionActual.EstaAutenticado ? SesionActual.NombreCompleto : "Administrador")}", fontInfo, Brushes.Gray, bounds.Left, y);
            y += 16;

            g.DrawLine(penSeparador, bounds.Left, y, bounds.Right, y);
            y += 8;
        }
        else
        {
            g.DrawString($"CLÍNICA VETERINARIA — {_tituloReporteActual} (Pág. {_paginaActual})", fontSubtitulo, brushPrimario, bounds.Left, y);
            y += 18;
            g.DrawLine(penSeparador, bounds.Left, y, bounds.Right, y);
            y += 8;
        }

        // Columnas visibles a imprimir
        var columnas = dgvReporte.Columns.Cast<DataGridViewColumn>()
            .Where(c => c.Visible)
            .OrderBy(c => c.DisplayIndex)
            .ToList();

        if (columnas.Count == 0)
        {
            e.HasMorePages = false;
            return;
        }

        // Anchos proporcionales calculados sobre el ancho imprimible
        float totalAnchoDgv = columnas.Sum(c => c.Width);
        float[] anchos = new float[columnas.Count];
        for (int i = 0; i < columnas.Count; i++)
        {
            anchos[i] = (columnas[i].Width / totalAnchoDgv) * bounds.Width;
        }

        float altoCabecera = 22;
        float altoFila = 20;

        // Dibujar fila de cabecera
        g.FillRectangle(brushCabeceraFondo, bounds.Left, y, bounds.Width, altoCabecera);
        g.DrawRectangle(penLinea, bounds.Left, y, bounds.Width, altoCabecera);

        using var formatTexto = new StringFormat
        {
            Alignment = StringAlignment.Near,
            LineAlignment = StringAlignment.Center,
            Trimming = StringTrimming.EllipsisCharacter,
            FormatFlags = StringFormatFlags.NoWrap
        };

        float xActual = bounds.Left;
        for (int i = 0; i < columnas.Count; i++)
        {
            var rectCabecera = new RectangleF(xActual + 4, y, anchos[i] - 8, altoCabecera);
            g.DrawString(columnas[i].HeaderText, fontCabecera, brushTexto, rectCabecera, formatTexto);
            xActual += anchos[i];
            if (i < columnas.Count - 1)
            {
                g.DrawLine(penLinea, xActual, y, xActual, y + altoCabecera);
            }
        }

        y += altoCabecera;

        // Dibujar filas de datos paginadas
        while (_filaActualImpresion < dgvReporte.Rows.Count)
        {
            // Espacio reservado para pie de página
            if (y + altoFila > bounds.Bottom - 35)
            {
                e.HasMorePages = true;
                DibujarPieDePagina(g, bounds, fontPie, brushTexto, penSeparador);
                return;
            }

            var fila = dgvReporte.Rows[_filaActualImpresion];

            // Alternancia visual de filas
            if (_filaActualImpresion % 2 == 1)
            {
                g.FillRectangle(brushFilaAlt, bounds.Left, y, bounds.Width, altoFila);
            }

            g.DrawRectangle(penLinea, bounds.Left, y, bounds.Width, altoFila);

            xActual = bounds.Left;
            for (int i = 0; i < columnas.Count; i++)
            {
                var valor = fila.Cells[columnas[i].Index].Value?.ToString() ?? string.Empty;
                var rectCelda = new RectangleF(xActual + 4, y, anchos[i] - 8, altoFila);
                g.DrawString(valor, fontCelda, brushTexto, rectCelda, formatTexto);
                xActual += anchos[i];
                if (i < columnas.Count - 1)
                {
                    g.DrawLine(penLinea, xActual, y, xActual, y + altoFila);
                }
            }

            y += altoFila;
            _filaActualImpresion++;
        }

        e.HasMorePages = false;
        DibujarPieDePagina(g, bounds, fontPie, brushTexto, penSeparador);
    }

    /// <summary>
    /// Renderiza el pie de página institucional en cada hoja impresa.
    /// </summary>
    private void DibujarPieDePagina(Graphics g, Rectangle bounds, Font fontPie, Brush brushTexto, Pen penSeparador)
    {
        float yPie = bounds.Bottom - 20;
        g.DrawLine(penSeparador, bounds.Left, yPie - 4, bounds.Right, yPie - 4);

        g.DrawString($"Total de registros: {dgvReporte.Rows.Count}", fontPie, brushTexto, bounds.Left, yPie);

        string textoPagina = $"Página {_paginaActual}";
        var sz = g.MeasureString(textoPagina, fontPie);
        g.DrawString(textoPagina, fontPie, brushTexto, bounds.Right - sz.Width, yPie);
    }

    /// <summary>
    /// Cierra el formulario y regresa al panel administrativo principal.
    /// </summary>
    private void btnVolver_Click(object? sender, EventArgs e)
    {
        Close();
    }
}
