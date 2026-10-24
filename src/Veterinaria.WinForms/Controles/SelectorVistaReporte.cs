namespace Veterinaria.WinForms.Controles;

/// <summary>
/// Agrega sobre la grilla de un reporte la opción "Ver como: Gráfico / Tabla", el selector
/// "Agrupar por" (Automático / Día / Mes / Año) y el área de gráficos, ocupando el mismo lugar
/// que la grilla. El área muestra un gráfico principal y, opcionalmente, uno secundario de resumen.
/// Por defecto se muestra el gráfico.
/// </summary>
public sealed class SelectorVistaReporte
{
    private const float ProporcionPrincipal = 0.6f;

    private readonly DataGridView _grilla;
    private readonly RadioButton _rbGrafico;
    private readonly TableLayoutPanel _areaGraficos;
    private readonly Label _lblAgrupar;
    private readonly ComboBox _cboAgrupar;
    private bool _agrupacionDisponible = true;

    public GraficoReporte Grafico { get; }
    public GraficoReporte GraficoSecundario { get; }

    public bool MostrandoGrafico => _rbGrafico.Checked;
    public bool UsaGraficoSecundario => GraficoSecundario.Visible;

    public AgrupacionPeriodo Agrupacion => _cboAgrupar.SelectedIndex switch
    {
        1 => AgrupacionPeriodo.Dia,
        2 => AgrupacionPeriodo.Mes,
        3 => AgrupacionPeriodo.Anio,
        _ => AgrupacionPeriodo.Automatico
    };

    /// <summary>Se dispara al pasar de Gráfico a Tabla o viceversa.</summary>
    public event EventHandler? VistaCambiada;

    /// <summary>Se dispara al cambiar la agrupación (día / mes / año) del eje temporal.</summary>
    public event EventHandler? AgrupacionCambiada;

    private SelectorVistaReporte(DataGridView grilla)
    {
        _grilla = grilla;
        var contenedor = grilla.Parent ?? throw new InvalidOperationException("La grilla debe estar dentro de un contenedor.");

        var panel = new FlowLayoutPanel
        {
            Location = grilla.Location,
            Size = new Size(grilla.Width, 30),
            Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right,
            BackColor = Color.Transparent,
            WrapContents = false
        };

        var lblVer = CrearEtiqueta("Ver como:", new Padding(0, 7, 8, 0));
        _rbGrafico = CrearOpcion("Gráfico", true);
        var rbTabla = CrearOpcion("Tabla", false);

        _lblAgrupar = CrearEtiqueta("Agrupar por:", new Padding(24, 7, 8, 0));
        _cboAgrupar = new ComboBox
        {
            DropDownStyle = ComboBoxStyle.DropDownList,
            Font = new Font("Segoe UI", 9F),
            Width = 120,
            Margin = new Padding(0, 3, 0, 0)
        };
        _cboAgrupar.Items.AddRange(["Automático", "Día", "Mes", "Año"]);
        _cboAgrupar.SelectedIndex = 0;

        panel.Controls.AddRange([lblVer, _rbGrafico, rbTabla, _lblAgrupar, _cboAgrupar]);

        grilla.Top += 34;
        grilla.Height -= 34;

        Grafico = new GraficoReporte { Dock = DockStyle.Fill, Margin = Padding.Empty };
        GraficoSecundario = new GraficoReporte { Dock = DockStyle.Fill, Margin = new Padding(6, 0, 0, 0), Visible = false };

        _areaGraficos = new TableLayoutPanel
        {
            Bounds = grilla.Bounds,
            Anchor = grilla.Anchor,
            ColumnCount = 2,
            RowCount = 1,
            BackColor = Color.White,
            Margin = Padding.Empty,
            Padding = Padding.Empty
        };
        _areaGraficos.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
        _areaGraficos.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
        _areaGraficos.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 0));
        _areaGraficos.Controls.Add(Grafico, 0, 0);
        _areaGraficos.Controls.Add(GraficoSecundario, 1, 0);

        contenedor.Controls.Add(panel);
        contenedor.Controls.Add(_areaGraficos);
        panel.BringToFront();
        _areaGraficos.BringToFront();

        _rbGrafico.CheckedChanged += (_, _) =>
        {
            AplicarVista();
            VistaCambiada?.Invoke(this, EventArgs.Empty);
        };
        _cboAgrupar.SelectedIndexChanged += (_, _) => AgrupacionCambiada?.Invoke(this, EventArgs.Empty);
        AplicarVista();
    }

    public static SelectorVistaReporte Configurar(DataGridView grilla) => new(grilla);

    /// <summary>
    /// Muestra u oculta el gráfico secundario de resumen al costado del principal.
    /// </summary>
    public void UsarGraficoSecundario(bool usar)
    {
        GraficoSecundario.Visible = usar;
        _areaGraficos.SuspendLayout();
        _areaGraficos.ColumnStyles[0] = new ColumnStyle(SizeType.Percent, usar ? ProporcionPrincipal * 100 : 100);
        _areaGraficos.ColumnStyles[1] = usar
            ? new ColumnStyle(SizeType.Percent, (1 - ProporcionPrincipal) * 100)
            : new ColumnStyle(SizeType.Absolute, 0);
        _areaGraficos.ResumeLayout();
        if (!usar)
            GraficoSecundario.Limpiar();
    }

    /// <summary>
    /// Habilita el selector "Agrupar por" solo en reportes con eje temporal (no aplica al censo).
    /// </summary>
    public void PermitirAgrupacion(bool permitir)
    {
        _agrupacionDisponible = permitir;
        AplicarVista();
    }

    /// <summary>
    /// Limpia ambos gráficos (por ejemplo al cambiar de tipo de reporte).
    /// </summary>
    public void LimpiarGraficos(string? mensaje = null)
    {
        if (mensaje is null) Grafico.Limpiar(); else Grafico.Limpiar(mensaje);
        UsarGraficoSecundario(false);
    }

    /// <summary>
    /// Dibuja el/los gráficos visibles en el área indicada (impresión / PDF).
    /// En hojas verticales los apila uno debajo del otro; en horizontales los pone lado a lado.
    /// </summary>
    public void DibujarGraficos(Graphics g, Rectangle area)
    {
        if (!UsaGraficoSecundario)
        {
            Grafico.Dibujar(g, area);
            return;
        }

        if (area.Height > area.Width)
        {
            var altoPrincipal = (int)(area.Height * ProporcionPrincipal);
            Grafico.Dibujar(g, new Rectangle(area.Left, area.Top, area.Width, altoPrincipal));
            GraficoSecundario.Dibujar(g, Rectangle.FromLTRB(area.Left, area.Top + altoPrincipal + 6, area.Right, area.Bottom));
            return;
        }

        var anchoPrincipal = (int)(area.Width * ProporcionPrincipal);
        Grafico.Dibujar(g, new Rectangle(area.Left, area.Top, anchoPrincipal, area.Height));
        GraficoSecundario.Dibujar(g, Rectangle.FromLTRB(area.Left + anchoPrincipal + 6, area.Top, area.Right, area.Bottom));
    }

    private void AplicarVista()
    {
        _areaGraficos.Visible = _rbGrafico.Checked;
        _grilla.Visible = !_rbGrafico.Checked;
        _lblAgrupar.Visible = _agrupacionDisponible && _rbGrafico.Checked;
        _cboAgrupar.Visible = _agrupacionDisponible && _rbGrafico.Checked;
    }

    private static Label CrearEtiqueta(string texto, Padding margen) => new()
    {
        Text = texto,
        AutoSize = true,
        Font = new Font("Segoe UI", 9F, FontStyle.Bold),
        ForeColor = Color.FromArgb(58, 53, 59),
        Margin = margen
    };

    private static RadioButton CrearOpcion(string texto, bool marcado) => new()
    {
        Text = texto,
        Checked = marcado,
        AutoSize = true,
        Font = new Font("Segoe UI", 9F),
        ForeColor = Color.FromArgb(58, 53, 59),
        Margin = new Padding(0, 4, 12, 0),
        Cursor = Cursors.Hand
    };
}
