using System.Drawing.Printing;
using System.Text;
using Veterinaria.Controllers.Controladores;
using Veterinaria.Domain.Dtos;
using Veterinaria.WinForms.Controles;
using Veterinaria.WinForms.Sesion;

namespace Veterinaria.WinForms.Vistas.Veterinario;

/// <summary>
/// Reportes del periodo para el rol Veterinario: mis consultas, o vacunas y tratamientos aplicados juntos.
/// </summary>
public partial class FormReportes : Form
{
    private const string TipoConsultas = "Mis consultas";
    private const string TipoVacunasTratamientos = "Vacunas y tratamientos";

    private readonly ReporteControlador _reporteControlador;
    private int _filaActualImpresion;
    private int _paginaActualImpresion;
    private string _filtrosActuales = string.Empty;
    private string _tituloReporte = "REPORTES DE CONSULTAS";
    private SelectorVistaReporte? _vista;

    public FormReportes(ReporteControlador reporteControlador)
    {
        _reporteControlador = reporteControlador;
        InitializeComponent();
    }

    private void FormReportes_Load(object? sender, EventArgs e)
    {
        lblUsuarioSesion.Text = SesionActual.EstaAutenticado
            ? $"Dr./Dra. {SesionActual.NombreCompleto} | {SesionActual.Rol}"
            : "Médico: Veterinario";

        var hoy = DateTime.Today;
        dtpFechaDesde.Value = new DateTime(hoy.Year, hoy.Month, 1);
        dtpFechaHasta.Value = hoy;

        _vista = SelectorVistaReporte.Configurar(dgvReporte);
        cboTipoReporte.Items.AddRange([TipoConsultas, TipoVacunasTratamientos]);
        cboTipoReporte.SelectedIndex = 0;
        cboTipoReporte.SelectedIndexChanged += (_, _) => PrepararGrilla();
        PrepararGrilla();

        btnGenerar.Click += async (_, _) => await GenerarAsync();
        btnImprimir.Click += (_, _) => ImprimirReporte();
        btnExportarPdf.Click += (_, _) => ExportarPdf();
        btnVolver.Click += (_, _) => Close();

        btnImprimir.Enabled = false;
        btnExportarPdf.Enabled = false;
    }

    private async Task GenerarAsync()
    {
        if (!SesionActual.IdUsuario.HasValue || SesionActual.IdUsuario.Value <= 0)
        {
            MessageBox.Show("No hay un veterinario autenticado en sesión.", "Sesión", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        if (dtpFechaDesde.Value.Date > dtpFechaHasta.Value.Date)
        {
            MessageBox.Show("La fecha desde no puede ser mayor que la fecha hasta.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        try
        {
            btnGenerar.Enabled = false;
            Cursor = Cursors.WaitCursor;

            var esVacunasTratamientos = EsVacunasTratamientos();
            var filtro = new FiltroReporteDto
            {
                TipoReporte = esVacunasTratamientos ? "Vacunas y Tratamientos" : "Consultas Clínicas",
                FechaDesde = dtpFechaDesde.Value.Date,
                FechaHasta = dtpFechaHasta.Value.Date,
                IdVeterinario = SesionActual.IdUsuario.Value
            };

            PrepararGrilla();

            bool exito;
            string mensaje;
            if (esVacunasTratamientos)
            {
                var resultado = await _reporteControlador.ObtenerReporteVacunasTratamientosAsync(filtro);
                exito = resultado.EsExitoso && resultado.Valor is not null;
                mensaje = resultado.Mensaje;
                if (exito)
                {
                    var filas = resultado.Valor!.ToList();
                    foreach (var r in filas)
                        dgvReporte.Rows.Add(r.FechaFormateada, r.Mascota, r.Especie, r.TratamientoVacuna, r.Tipo, r.Dosis);

                    var vacunas = filas.Count(r => r.Tipo == "Vacunación");
                    lblTotal.Text = $"Total de registros: {filas.Count} ({vacunas} vacuna(s), {filas.Count - vacunas} tratamiento(s))";

                    var porTipo = filas
                        .GroupBy(r => r.Tipo)
                        .OrderByDescending(gr => gr.Count())
                        .ToList();
                    _vista?.Grafico.MostrarDona(
                        "Vacunas y tratamientos aplicados por tipo",
                        porTipo.Select(gr => gr.Key).ToList(),
                        porTipo.Select(gr => (decimal)gr.Count()).ToList());
                }
            }
            else
            {
                var resultado = await _reporteControlador.ObtenerReporteConsultasClinicasAsync(filtro);
                exito = resultado.EsExitoso && resultado.Valor is not null;
                mensaje = resultado.Mensaje;
                if (exito)
                {
                    var consultas = resultado.Valor!.ToList();
                    foreach (var r in consultas)
                        dgvReporte.Rows.Add(r.Fecha, r.Hora, r.Mascota, r.Propietario, r.Motivo, r.Diagnostico);

                    lblTotal.Text = $"Total de consultas: {dgvReporte.Rows.Count}";

                    var periodos = PeriodosGrafico.Armar(filtro.FechaDesde, filtro.FechaHasta);
                    _vista?.Grafico.MostrarColumnas(
                        $"Mis consultas por {periodos.Unidad}",
                        periodos.Etiquetas,
                        [new SerieGrafico("Consultas", GraficoReporte.Paleta[0], periodos.Acumular(consultas, c => c.FechaHora, _ => 1))]);
                }
            }

            if (!exito)
            {
                MessageBox.Show(mensaje, "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                btnImprimir.Enabled = false;
                btnExportarPdf.Enabled = false;
                return;
            }

            _filtrosActuales = $"{cboTipoReporte.SelectedItem} — Desde {filtro.FechaDesde:dd/MM/yyyy} hasta {filtro.FechaHasta:dd/MM/yyyy}";
            btnImprimir.Enabled = dgvReporte.Rows.Count > 0;
            btnExportarPdf.Enabled = dgvReporte.Rows.Count > 0;
        }
        finally
        {
            btnGenerar.Enabled = true;
            Cursor = Cursors.Default;
        }
    }

    private bool EsVacunasTratamientos() =>
        Equals(cboTipoReporte.SelectedItem, TipoVacunasTratamientos);

    /// <summary>
    /// Vacía la grilla y ajusta los encabezados de columna según el tipo de reporte elegido.
    /// </summary>
    private void PrepararGrilla()
    {
        dgvReporte.Rows.Clear();
        _vista?.Grafico.Limpiar();
        btnImprimir.Enabled = false;
        btnExportarPdf.Enabled = false;

        string[] encabezados;
        if (EsVacunasTratamientos())
        {
            encabezados = ["Fecha", "Mascota", "Especie", "Vacuna / Tratamiento", "Tipo", "Dosis"];
            _tituloReporte = "REPORTES DE VACUNAS Y TRATAMIENTOS";
            lblTotal.Text = "Total de registros: 0";
        }
        else
        {
            encabezados = ["Fecha", "Hora", "Mascota", "Propietario", "Motivo", "Diagnóstico"];
            _tituloReporte = "REPORTES DE CONSULTAS";
            lblTotal.Text = "Total de consultas: 0";
        }

        DataGridViewColumn[] columnas = [colFecha, colHora, colMascota, colPropietario, colMotivo, colDiagnostico];
        for (var i = 0; i < columnas.Length; i++)
            columnas[i].HeaderText = encabezados[i];
    }

    private void ImprimirReporte()
    {
        if (dgvReporte.Rows.Count == 0)
        {
            MessageBox.Show("No hay datos en la grilla para imprimir.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        try
        {
            using var pd = CrearPrintDocument();
            using var vista = new PrintPreviewDialog
            {
                Document = pd,
                Width = 1050,
                Height = 720,
                StartPosition = FormStartPosition.CenterScreen,
                Text = $"Vista previa — {cboTipoReporte.SelectedItem}"
            };
            vista.ShowDialog(this);
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error al preparar la impresión: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void ExportarPdf()
    {
        if (dgvReporte.Rows.Count == 0)
        {
            MessageBox.Show("No hay datos en la grilla para exportar.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        using var sfd = new SaveFileDialog
        {
            Filter = "Archivo PDF (*.pdf)|*.pdf|Archivo CSV (*.csv)|*.csv",
            FileName = $"Reporte_{(EsVacunasTratamientos() ? "VacunasTratamientos" : "Consultas")}_{DateTime.Now:yyyyMMdd_HHmm}.pdf",
            Title = "Exportar reporte"
        };

        if (sfd.ShowDialog(this) != DialogResult.OK)
            return;

        try
        {
            if (sfd.FileName.EndsWith(".csv", StringComparison.OrdinalIgnoreCase))
            {
                ExportarCsv(sfd.FileName);
                MessageBox.Show("Reporte exportado a CSV.", "Exportación exitosa", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var pdfOk = PrinterSettings.InstalledPrinters
                .Cast<string>()
                .Any(p => p.Equals("Microsoft Print to PDF", StringComparison.OrdinalIgnoreCase));

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

    private void ExportarCsv(string ruta)
    {
        var sb = new StringBuilder();
        var cols = dgvReporte.Columns.Cast<DataGridViewColumn>().Where(c => c.Visible).OrderBy(c => c.DisplayIndex).ToList();
        sb.AppendLine(string.Join(";", cols.Select(c => c.HeaderText)));
        foreach (DataGridViewRow row in dgvReporte.Rows)
        {
            if (row.IsNewRow) continue;
            sb.AppendLine(string.Join(";", cols.Select(c =>
            {
                var v = row.Cells[c.Index].FormattedValue?.ToString() ?? string.Empty;
                return v.Contains(';') ? $"\"{v}\"" : v;
            })));
        }
        File.WriteAllText(ruta, sb.ToString(), Encoding.UTF8);
    }

    private PrintDocument CrearPrintDocument()
    {
        var pd = new PrintDocument();
        pd.DefaultPageSettings.Landscape = true;
        pd.DefaultPageSettings.Margins = new Margins(40, 40, 40, 40);
        pd.BeginPrint += (_, _) =>
        {
            _filaActualImpresion = 0;
            _paginaActualImpresion = 0;
        };
        pd.PrintPage += ImprimirPagina;
        return pd;
    }

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
        if (_paginaActualImpresion == 1)
        {
            g.DrawString($"CLÍNICA VETERINARIA — {_tituloReporte}", fontTitulo, brushRosa, bounds.Left, y);
            y += 22;
            g.DrawString(_filtrosActuales, fontInfo, brush, bounds.Left, y);
            y += 15;
            var medico = SesionActual.EstaAutenticado ? SesionActual.NombreCompleto : "Veterinario";
            g.DrawString($"Veterinario: {medico} | Emisión: {DateTime.Now:dd/MM/yyyy HH:mm} | {lblTotal.Text}", fontInfo, Brushes.Gray, bounds.Left, y);
            y += 16;
            g.DrawLine(pen, bounds.Left, y, bounds.Right, y);
            y += 8;
        }
        else
        {
            g.DrawString($"{_tituloReporte} (Pág. {_paginaActualImpresion})", fontSub, brushRosa, bounds.Left, y);
            y += 18;
            g.DrawLine(pen, bounds.Left, y, bounds.Right, y);
            y += 8;
        }

        if (_vista is { MostrandoGrafico: true })
        {
            _vista.Grafico.Dibujar(g, Rectangle.FromLTRB(bounds.Left, (int)y, bounds.Right, bounds.Bottom - 28));
            e.HasMorePages = false;
            g.DrawString($"Página {_paginaActualImpresion}", fontPie, brush, bounds.Left, bounds.Bottom - 18);
            return;
        }

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

        g.FillRectangle(brushCab, bounds.Left, y, bounds.Width, altoCab);
        g.DrawRectangle(penLinea, bounds.Left, y, bounds.Width, altoCab);
        float x = bounds.Left;
        for (var i = 0; i < columnas.Count; i++)
        {
            g.DrawString(columnas[i].HeaderText, fontCab, brush, new RectangleF(x + 3, y, anchos[i] - 6, altoCab), fmt);
            x += anchos[i];
        }
        y += altoCab;

        while (_filaActualImpresion < dgvReporte.Rows.Count)
        {
            if (y + altoFila > bounds.Bottom - 28)
            {
                e.HasMorePages = true;
                g.DrawString($"Página {_paginaActualImpresion}", fontPie, brush, bounds.Left, bounds.Bottom - 18);
                return;
            }

            var fila = dgvReporte.Rows[_filaActualImpresion];
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

        e.HasMorePages = false;
        g.DrawString($"Página {_paginaActualImpresion}", fontPie, brush, bounds.Left, bounds.Bottom - 18);
    }
}
