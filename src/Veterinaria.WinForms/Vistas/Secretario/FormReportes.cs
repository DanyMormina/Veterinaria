using System.Drawing.Printing;
using System.Text;
using Veterinaria.Controllers.Controladores;
using Veterinaria.Domain.Dtos;
using Veterinaria.WinForms.Controles;
using Veterinaria.WinForms.Sesion;

namespace Veterinaria.WinForms.Vistas.Secretario;
// Reporte de cobros por periodo para el rol Secretario.
public partial class FormReportes : Form
{
    private readonly ReporteControlador _reporteControlador;
    private int _filaActualImpresion;
    private int _paginaActualImpresion;
    private string _filtrosActuales = string.Empty;
    private SelectorVistaReporte? _vista;
    private bool _reporteGenerado;

    public FormReportes(ReporteControlador reporteControlador)
    {
        _reporteControlador = reporteControlador;
        InitializeComponent();
    }

    // Maneja la carga inicial del formulario de reportes de cobros
    private void FormReportes_Load(object? sender, EventArgs e)
    {
        // Identifica al usuario en recepción en el encabezado
        lblUsuarioSesion.Text = SesionActual.EstaAutenticado
            ? $"Recepción: {SesionActual.NombreCompleto} | {SesionActual.Rol}"
            : "Recepción: Secretario";

        // Vincula el selector de vista interactivo (Tablas o Gráficos con GDI+)
        _vista = SelectorVistaReporte.Configurar(dgvReporte);
        _vista.VistaCambiada += async (_, _) =>
        {
            if (_reporteGenerado)
                await GenerarAsync();
        };
        _vista.AgrupacionCambiada += async (_, _) =>
        {
            dtpFechaDesde.Value = PeriodosGrafico.DesdeParaAgrupacion(dtpFechaDesde.Value, dtpFechaHasta.Value, _vista.Agrupacion);
            if (_reporteGenerado)
                await GenerarAsync();
        };

        // Aplica el rango del mes actual por defecto
        AplicarPeriodoMes();

        // Enlace de botones de acceso rápido de períodos y acciones
        btnHoy.Click += (_, _) => AplicarPeriodoHoy();
        btnSemana.Click += (_, _) => AplicarPeriodoSemana();
        btnMes.Click += (_, _) => AplicarPeriodoMes();
        btnGenerar.Click += async (_, _) => await GenerarAsync();
        btnImprimir.Click += (_, _) => ImprimirReporte();
        btnExportarPdf.Click += (_, _) => ExportarPdf();
        btnVolver.Click += (_, _) => Close();

        // Deshabilita imprimir y exportar hasta que se genere el primer reporte con datos
        btnImprimir.Enabled = false;
        btnExportarPdf.Enabled = false;
    }

    // Configura el rango temporal para el día de hoy
    private void AplicarPeriodoHoy()
    {
        dtpFechaDesde.Value = DateTime.Today;
        dtpFechaHasta.Value = DateTime.Today;
    }

    // Configura el rango temporal para la semana en curso (lunes a hoy)
    private void AplicarPeriodoSemana()
    {
        var hoy = DateTime.Today;
        var diff = ((int)hoy.DayOfWeek + 6) % 7;
        dtpFechaDesde.Value = hoy.AddDays(-diff);
        dtpFechaHasta.Value = hoy;
    }

    // Configura el rango temporal para el mes completo en curso (primer día del mes a hoy)
    private void AplicarPeriodoMes()
    {
        var hoy = DateTime.Today;
        dtpFechaDesde.Value = new DateTime(hoy.Year, hoy.Month, 1);
        dtpFechaHasta.Value = hoy;
    }

    // Consulta los cobros en el rango de fechas y actualiza tanto la grilla como los gráficos interactivos
    private async Task GenerarAsync()
    {
        // Validación temporal defensiva
        if (dtpFechaDesde.Value.Date > dtpFechaHasta.Value.Date)
        {
            MessageBox.Show("La fecha desde no puede ser mayor que la fecha hasta.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        try
        {
            btnGenerar.Enabled = false;
            Cursor = Cursors.WaitCursor;

            // Filtro exclusivo para cobros completados en el rango indicado
            var filtro = new FiltroReporteCobroDto
            {
                FechaDesde = dtpFechaDesde.Value.Date,
                FechaHasta = dtpFechaHasta.Value.Date,
                Estado = "Completado"
            };

            var resultado = await _reporteControlador.ObtenerReporteCobrosAsync(filtro);
            dgvReporte.Rows.Clear();
            _vista?.LimpiarGraficos();

            if (!resultado.EsExitoso || resultado.Valor is null)
            {
                MessageBox.Show(resultado.Mensaje, "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                lblTotal.Text = "Total: 0 registros | $ 0,00";
                btnImprimir.Enabled = false;
                btnExportarPdf.Enabled = false;
                return;
            }

            // Puebla las filas de la tabla con los cobros recuperados
            foreach (var r in resultado.Valor)
            {
                var metodo = r.MetodoPago;
                if (r.Cuotas is > 0)
                    metodo = r.Cuotas == 1 ? $"{metodo} (1 pago)" : $"{metodo} ({r.Cuotas} cuotas)";

                dgvReporte.Rows.Add(
                    r.FechaFormateada,
                    r.IdConsulta,
                    r.Mascota,
                    r.Propietario,
                    metodo,
                    r.Importe.ToString("N2"));
            }

            var considerados = resultado.Valor.ToList();
            var suma = considerados.Sum(x => x.Importe);

            // Gráfico 1 (Dona GDI+): Distribución de ingresos por método de pago (Efectivo, Tarjeta, etc.)
            var porMetodo = considerados
                .GroupBy(x => string.IsNullOrWhiteSpace(x.MetodoPago) ? "Sin método" : x.MetodoPago)
                .OrderByDescending(gr => gr.Sum(x => x.Importe))
                .ToList();
            _vista?.Grafico.MostrarDona(
                "Cobrado por método de pago",
                porMetodo.Select(gr => gr.Key).ToList(),
                porMetodo.Select(gr => gr.Sum(x => x.Importe)).ToList(),
                v => v.ToString("C0", new System.Globalization.CultureInfo("es-AR")));

            // Gráfico 2 (Barras / Evolución GDI+): Recaudación acumulada por período temporal
            var periodos = PeriodosGrafico.Armar(filtro.FechaDesde, filtro.FechaHasta, _vista?.Agrupacion ?? AgrupacionPeriodo.Automatico);
            _vista?.UsarGraficoSecundario(true);
            _vista?.GraficoSecundario.MostrarEvolucion(
                $"Cobrado por {periodos.Unidad}",
                periodos.Etiquetas,
                [new SerieGrafico("Cobrado", GraficoReporte.Paleta[6], periodos.Acumular(considerados, x => x.Fecha, x => x.Importe))],
                v => v.ToString("C0", new System.Globalization.CultureInfo("es-AR")));

            _reporteGenerado = true;
            lblTotal.Text = $"Total: {dgvReporte.Rows.Count} registros | $ {suma:N2}";
            _filtrosActuales = $"Desde {filtro.FechaDesde:dd/MM/yyyy} hasta {filtro.FechaHasta:dd/MM/yyyy}";
            btnImprimir.Enabled = dgvReporte.Rows.Count > 0;
            btnExportarPdf.Enabled = dgvReporte.Rows.Count > 0;
        }
        finally
        {
            btnGenerar.Enabled = true;
            Cursor = Cursors.Default;
        }
    }

    // Abre el diálogo de vista previa de impresión para visualizar el reporte antes de imprimirlo en papel
    private void ImprimirReporte()
    {
        // Si no se generó ninguna fila, cancela la acción para no imprimir una hoja en blanco
        if (dgvReporte.Rows.Count == 0)
        {
            MessageBox.Show("No hay datos en la grilla para imprimir.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        try
        {
            // Instancia el documento de impresión configurado en apaisado (Landscape)
            using var pd = CrearPrintDocument();
            using var vista = new PrintPreviewDialog
            {
                Document = pd,
                Width = 1050,
                Height = 720,
                StartPosition = FormStartPosition.CenterScreen,
                Text = "Vista previa --- Reportes de cobros"
            };
            // Muestra la ventana modal con la vista previa del reporte
            vista.ShowDialog(this);
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error al preparar la impresión: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    // Permite exportar el reporte generado a formato PDF o CSV según elija el usuario
    private void ExportarPdf()
    {
        // Verifica que existan registros cargados en la tabla
        if (dgvReporte.Rows.Count == 0)
        {
            MessageBox.Show("No hay datos en la grilla para exportar.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        // Abre el explorador de archivos para elegir dónde guardar y con qué nombre
        using var sfd = new SaveFileDialog
        {
            Filter = "Archivo PDF (*.pdf)|*.pdf|Archivo CSV (*.csv)|*.csv",
            FileName = $"Reporte_Cobros_{DateTime.Now:yyyyMMdd_HHmm}.pdf",
            Title = "Exportar reporte de cobros"
        };

        if (sfd.ShowDialog(this) != DialogResult.OK)
            return;

        try
        {
            // Si el usuario eligió extensión .csv, genera el archivo delimitado por punto y coma
            if (sfd.FileName.EndsWith(".csv", StringComparison.OrdinalIgnoreCase))
            {
                ExportarCsv(sfd.FileName);
                MessageBox.Show("Reporte exportado a CSV.", "Exportación exitosa", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            // Verifica si Windows cuenta con la impresora virtual 'Microsoft Print to PDF'
            var pdfOk = PrinterSettings.InstalledPrinters
                .Cast<string>()
                .Any(p => p.Equals("Microsoft Print to PDF", StringComparison.OrdinalIgnoreCase));

            // Si la máquina no tiene la impresora virtual instalada, ofrece exportar como CSV alternativo
            if (!pdfOk)
            {
                var r = MessageBox.Show(
                    "No está disponible 'Microsoft Print to PDF'. ¿Exportar como CSV?",
                    "PDF no disponible",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);
                if (r == DialogResult.Yes)
                {
                    var csv = Path.ChangeExtension(sfd.FileName, ".csv");
                    ExportarCsv(csv);
                    MessageBox.Show($"Exportado a:\n{csv}", "Exportación exitosa", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                return;
            }

            // Imprime directamente al archivo PDF utilizando el controlador de Windows
            using var pd = CrearPrintDocument();
            pd.PrinterSettings.PrinterName = "Microsoft Print to PDF";
            pd.PrinterSettings.PrintToFile = true;
            pd.PrinterSettings.PrintFileName = sfd.FileName;
            pd.Print();
            MessageBox.Show($"PDF generado en:\n{sfd.FileName}", "Exportación exitosa", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error al exportar: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    // Exporta los datos visibles de la grilla a un archivo de texto separado por punto y coma (CSV compatible con Excel)
    private void ExportarCsv(string ruta)
    {
        var sb = new StringBuilder();
        // Toma únicamente las columnas visibles ordenadas por su índice de pantalla
        var cols = dgvReporte.Columns.Cast<DataGridViewColumn>().Where(c => c.Visible).OrderBy(c => c.DisplayIndex).ToList();
        // Agrega la fila de encabezados
        sb.AppendLine(string.Join(";", cols.Select(c => c.HeaderText)));
        // Recorre cada fila de datos de la grilla
        foreach (DataGridViewRow row in dgvReporte.Rows)
        {
            if (row.IsNewRow) continue;
            sb.AppendLine(string.Join(";", cols.Select(c =>
            {
                var v = row.Cells[c.Index].FormattedValue?.ToString() ?? string.Empty;
                return v.Contains(';') ? $"\"{v}\"" : v; // Escapa punto y coma entre comillas
            })));
        }
        // Guarda el archivo con codificación UTF-8
        File.WriteAllText(ruta, sb.ToString(), Encoding.UTF8);
    }

    // Crea y configura el objeto PrintDocument para imprimir en orientación horizontal (apaisado)
    private PrintDocument CrearPrintDocument()
    {
        var pd = new PrintDocument();
        pd.DefaultPageSettings.Landscape = true; // Orientación horizontal para que entren todas las columnas
        pd.DefaultPageSettings.Margins = new Margins(40, 40, 40, 40); // Márgenes de 40 píxeles
        pd.BeginPrint += (_, _) =>
        {
            // Reinicia los contadores al iniciar la impresión
            _filaActualImpresion = 0;
            _paginaActualImpresion = 0;
        };
        pd.PrintPage += ImprimirPagina; // Asocia el método de dibujo por página
        return pd;
    }

    // Dibuja el contenido de cada página a imprimir (encabezado institucional, tabla paginada o gráficos GDI+)
    private void ImprimirPagina(object sender, PrintPageEventArgs e)
    {
        _paginaActualImpresion++;
        var g = e.Graphics;
        if (g is null)
        {
            e.HasMorePages = false;
            return;
        }

        var bounds = e.MarginBounds;
        // Tipografías y pinceles que respetan la paleta "Ejecutivo Romántico Pastel"
        using var fontTitulo = new Font("Segoe UI", 12F, FontStyle.Bold);
        using var fontSub = new Font("Segoe UI", 9.5F, FontStyle.Bold);
        using var fontInfo = new Font("Segoe UI", 8F);
        using var fontCab = new Font("Segoe UI", 8.5F, FontStyle.Bold);
        using var fontCel = new Font("Segoe UI", 8F);
        using var fontPie = new Font("Segoe UI", 8F, FontStyle.Italic);
        using var brush = new SolidBrush(Color.FromArgb(58, 53, 59));
        using var brushRosa = new SolidBrush(Color.FromArgb(200, 138, 150));
        using var brushCab = new SolidBrush(Color.FromArgb(240, 235, 237));
        using var brushAlt = new SolidBrush(Color.FromArgb(250, 244, 244));
        using var pen = new Pen(Color.FromArgb(200, 138, 150), 1.5f);
        using var penLinea = new Pen(Color.FromArgb(226, 217, 220));

        float y = bounds.Top;
        // Dibuja el encabezado completo en la página 1
        if (_paginaActualImpresion == 1)
        {
            g.DrawString("CLÍNICA VETERINARIA --- REPORTES DE COBROS", fontTitulo, brushRosa, bounds.Left, y);
            y += 22;
            g.DrawString(_filtrosActuales, fontInfo, brush, bounds.Left, y);
            y += 15;
            g.DrawString($"Emisión: {DateTime.Now:dd/MM/yyyy HH:mm} | {lblTotal.Text}", fontInfo, Brushes.Gray, bounds.Left, y);
            y += 16;
            g.DrawLine(pen, bounds.Left, y, bounds.Right, y);
            y += 8;
        }
        // En páginas posteriores dibuja un encabezado reducido
        else
        {
            g.DrawString($"REPORTES DE COBROS (Pág. {_paginaActualImpresion})", fontSub, brushRosa, bounds.Left, y);
            y += 18;
            g.DrawLine(pen, bounds.Left, y, bounds.Right, y);
            y += 8;
        }

        // Si el usuario está viendo gráficos, imprime los gráficos interactivos dibujados con GDI+
        if (_vista is { MostrandoGrafico: true })
        {
            _vista.DibujarGraficos(g, Rectangle.FromLTRB(bounds.Left, (int)y, bounds.Right, bounds.Bottom - 28));
            e.HasMorePages = false;
            g.DrawString($"Página {_paginaActualImpresion}", fontPie, brush, bounds.Left, bounds.Bottom - 18);
            return;
        }

        // Calcula anchos proporcionales de columnas para aprovechar el ancho disponible de la hoja
        var columnas = dgvReporte.Columns.Cast<DataGridViewColumn>().Where(c => c.Visible).OrderBy(c => c.DisplayIndex).ToList();
        float totalW = columnas.Sum(c => Math.Max(c.Width, 40));
        var anchos = columnas.Select(c => (Math.Max(c.Width, 40) / totalW) * bounds.Width).ToArray();
        const float altoCab = 22f;
        const float altoFila = 20f;

        using var fmt = new StringFormat
        {
            Alignment = StringAlignment.Near,
            LineAlignment = StringAlignment.Center,
            Trimming = StringTrimming.EllipsisCharacter,
            FormatFlags = StringFormatFlags.NoWrap
        };

        // Dibuja el fondo y texto de la cabecera de la tabla
        g.FillRectangle(brushCab, bounds.Left, y, bounds.Width, altoCab);
        g.DrawRectangle(penLinea, bounds.Left, y, bounds.Width, altoCab);
        float x = bounds.Left;
        for (var i = 0; i < columnas.Count; i++)
        {
            g.DrawString(columnas[i].HeaderText, fontCab, brush, new RectangleF(x + 3, y, anchos[i] - 6, altoCab), fmt);
            x += anchos[i];
        }
        y += altoCab;

        // Dibuja fila por fila verificando si entra en la hoja actual
        while (_filaActualImpresion < dgvReporte.Rows.Count)
        {
            // Si la siguiente fila supera el margen inferior, salta a una nueva página
            if (y + altoFila > bounds.Bottom - 28)
            {
                e.HasMorePages = true;
                g.DrawString($"Página {_paginaActualImpresion}", fontPie, brush, bounds.Left, bounds.Bottom - 18);
                return;
            }

            var fila = dgvReporte.Rows[_filaActualImpresion];
            // Alterna color de fondo para facilitar la lectura ejecutiva
            if (_filaActualImpresion % 2 == 1)
                g.FillRectangle(brushAlt, bounds.Left, y, bounds.Width, altoFila);
            g.DrawRectangle(penLinea, bounds.Left, y, bounds.Width, altoFila);
            x = bounds.Left;
            for (var i = 0; i < columnas.Count; i++)
            {
                var valor = fila.Cells[columnas[i].Index].FormattedValue?.ToString() ?? string.Empty;
                g.DrawString(valor, fontCel, brush, new RectangleF(x + 3, y, anchos[i] - 6, altoFila), fmt);
                x += anchos[i];
            }
            y += altoFila;
            _filaActualImpresion++;
        }

        // Fin de la impresión: no quedan más páginas
        e.HasMorePages = false;
        g.DrawString($"Página {_paginaActualImpresion}", fontPie, brush, bounds.Left, bounds.Bottom - 18);
    }
}
