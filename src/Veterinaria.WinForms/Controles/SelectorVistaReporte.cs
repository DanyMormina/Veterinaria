namespace Veterinaria.WinForms.Controles;

/// <summary>
/// Agrega sobre la grilla de un reporte la opción "Ver como: Gráfico / Tabla" y el gráfico,
/// ocupando el mismo lugar que la grilla. Por defecto se muestra el gráfico.
/// </summary>
public sealed class SelectorVistaReporte
{
    private readonly DataGridView _grilla;
    private readonly RadioButton _rbGrafico;

    public GraficoReporte Grafico { get; }

    public bool MostrandoGrafico => _rbGrafico.Checked;

    /// <summary>Se dispara al pasar de Gráfico a Tabla o viceversa.</summary>
    public event EventHandler? VistaCambiada;

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

        var lblVer = new Label
        {
            Text = "Ver como:",
            AutoSize = true,
            Font = new Font("Segoe UI", 9F, FontStyle.Bold),
            ForeColor = Color.FromArgb(58, 53, 59),
            Margin = new Padding(0, 7, 8, 0)
        };
        _rbGrafico = CrearOpcion("Gráfico", true);
        var rbTabla = CrearOpcion("Tabla", false);
        panel.Controls.AddRange([lblVer, _rbGrafico, rbTabla]);

        grilla.Top += 34;
        grilla.Height -= 34;

        Grafico = new GraficoReporte
        {
            Bounds = grilla.Bounds,
            Anchor = grilla.Anchor
        };

        contenedor.Controls.Add(panel);
        contenedor.Controls.Add(Grafico);
        panel.BringToFront();
        Grafico.BringToFront();

        _rbGrafico.CheckedChanged += (_, _) =>
        {
            AplicarVista();
            VistaCambiada?.Invoke(this, EventArgs.Empty);
        };
        AplicarVista();
    }

    public static SelectorVistaReporte Configurar(DataGridView grilla) => new(grilla);

    private void AplicarVista()
    {
        Grafico.Visible = _rbGrafico.Checked;
        _grilla.Visible = !_rbGrafico.Checked;
    }

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
