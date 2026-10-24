using System.Drawing.Drawing2D;
using System.Globalization;

namespace Veterinaria.WinForms.Controles;

/// <summary>
/// Serie de valores para los gráficos de columnas, barras y línea.
/// </summary>
public sealed record SerieGrafico(string Nombre, Color Color, IReadOnlyList<decimal> Valores);

/// <summary>
/// Control de gráficos dibujado con GDI+ (sin librerías externas) para los reportes:
/// columnas agrupadas, barras horizontales, dona y línea. Se puede dibujar también sobre una hoja impresa.
/// </summary>
public sealed class GraficoReporte : Control
{
    private enum TipoGrafico { Ninguno, Columnas, BarrasHorizontales, Dona, Linea }

    public static readonly Color[] Paleta =
    [
        Color.FromArgb(200, 138, 150),
        Color.FromArgb(152, 196, 164),
        Color.FromArgb(143, 168, 203),
        Color.FromArgb(232, 184, 120),
        Color.FromArgb(170, 150, 200),
        Color.FromArgb(120, 180, 190),
        Color.FromArgb(184, 93, 105),
        Color.FromArgb(200, 190, 120),
        Color.FromArgb(150, 150, 150)
    ];

    private static readonly Color ColorTexto = Color.FromArgb(58, 53, 59);
    private static readonly Color ColorGrilla = Color.FromArgb(226, 217, 220);
    private static readonly CultureInfo Cultura = new("es-AR");

    private TipoGrafico _tipo = TipoGrafico.Ninguno;
    private string _titulo = string.Empty;
    private string _mensajeVacio = "Genere un reporte para ver el gráfico.";
    private IReadOnlyList<string> _categorias = [];
    private IReadOnlyList<SerieGrafico> _series = [];
    private Func<decimal, string> _formato = v => v.ToString("N0", Cultura);

    public GraficoReporte()
    {
        SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer |
                 ControlStyles.ResizeRedraw | ControlStyles.UserPaint, true);
        BackColor = Color.White;
        Font = new Font("Segoe UI", 9F);
    }

    public void Limpiar(string mensaje = "Genere un reporte para ver el gráfico.")
    {
        _tipo = TipoGrafico.Ninguno;
        _mensajeVacio = mensaje;
        Invalidate();
    }

    public void MostrarColumnas(string titulo, IReadOnlyList<string> categorias, IReadOnlyList<SerieGrafico> series, Func<decimal, string>? formato = null) =>
        Mostrar(TipoGrafico.Columnas, titulo, categorias, series, formato);

    public void MostrarBarrasHorizontales(string titulo, IReadOnlyList<string> categorias, SerieGrafico serie, Func<decimal, string>? formato = null) =>
        Mostrar(TipoGrafico.BarrasHorizontales, titulo, categorias, [serie], formato);

    /// <summary>
    /// Línea de tendencia; con menos de 3 períodos una línea no muestra evolución, así que se dibujan columnas.
    /// </summary>
    public void MostrarLinea(string titulo, IReadOnlyList<string> categorias, SerieGrafico serie, Func<decimal, string>? formato = null) =>
        Mostrar(categorias.Count < 3 ? TipoGrafico.Columnas : TipoGrafico.Linea, titulo, categorias, [serie], formato);

    /// <summary>
    /// Evolución en el tiempo eligiendo el gráfico según la cantidad de períodos:
    /// columnas para pocos períodos (se comparan valores puntuales) y línea para muchos (se lee la tendencia).
    /// </summary>
    public void MostrarEvolucion(string titulo, IReadOnlyList<string> categorias, IReadOnlyList<SerieGrafico> series, Func<decimal, string>? formato = null) =>
        Mostrar(categorias.Count <= MaximoPeriodosColumnas ? TipoGrafico.Columnas : TipoGrafico.Linea, titulo, categorias, series, formato);

    private const int MaximoPeriodosColumnas = 12;
    private const float AnchoMaximoBarra = 46f;

    public void MostrarDona(string titulo, IReadOnlyList<string> categorias, IReadOnlyList<decimal> valores, Func<decimal, string>? formato = null) =>
        Mostrar(TipoGrafico.Dona, titulo, categorias, [new SerieGrafico(titulo, Paleta[0], valores)], formato);

    private void Mostrar(TipoGrafico tipo, string titulo, IReadOnlyList<string> categorias, IReadOnlyList<SerieGrafico> series, Func<decimal, string>? formato)
    {
        _tipo = categorias.Count == 0 ? TipoGrafico.Ninguno : tipo;
        _mensajeVacio = "No hay datos para graficar con los filtros seleccionados.";
        _titulo = titulo;
        _categorias = categorias;
        _series = series;
        _formato = formato ?? (v => v.ToString("N0", Cultura));
        Invalidate();
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        base.OnPaint(e);
        Dibujar(e.Graphics, ClientRectangle);
    }

    /// <summary>
    /// Dibuja el gráfico actual dentro del área indicada (pantalla o página de impresión).
    /// </summary>
    public void Dibujar(Graphics g, Rectangle area)
    {
        g.SmoothingMode = SmoothingMode.AntiAlias;
        g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;

        using var fuente = new Font("Segoe UI", 8.5F);
        using var fuenteTitulo = new Font("Segoe UI", 11F, FontStyle.Bold);
        using var pincelTexto = new SolidBrush(ColorTexto);

        if (_tipo == TipoGrafico.Ninguno)
        {
            using var formatoCentro = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center };
            g.DrawString(_mensajeVacio, fuente, Brushes.Gray, area, formatoCentro);
            return;
        }

        g.DrawString(_titulo, fuenteTitulo, pincelTexto, area.Left + 12, area.Top + 8);
        var cuerpo = Rectangle.FromLTRB(area.Left + 12, area.Top + 36, area.Right - 12, area.Bottom - 10);
        if (cuerpo.Width < 80 || cuerpo.Height < 80)
            return;

        switch (_tipo)
        {
            case TipoGrafico.Columnas:
                DibujarColumnas(g, cuerpo, fuente, pincelTexto);
                break;
            case TipoGrafico.BarrasHorizontales:
                DibujarBarrasHorizontales(g, cuerpo, fuente, pincelTexto);
                break;
            case TipoGrafico.Dona:
                DibujarDona(g, cuerpo, fuente, pincelTexto);
                break;
            case TipoGrafico.Linea:
                DibujarLinea(g, cuerpo, fuente, pincelTexto);
                break;
        }
    }

    private void DibujarLeyenda(Graphics g, Rectangle cuerpo, Font fuente, Brush pincelTexto)
    {
        var x = (float)cuerpo.Right;
        for (var i = _series.Count - 1; i >= 0; i--)
        {
            var ancho = g.MeasureString(_series[i].Nombre, fuente).Width;
            x -= ancho + 22;
            using var pincel = new SolidBrush(_series[i].Color);
            g.FillRectangle(pincel, x, cuerpo.Top - 26, 12, 12);
            g.DrawString(_series[i].Nombre, fuente, pincelTexto, x + 15, cuerpo.Top - 28);
        }
    }

    /// <summary>
    /// Dibuja ejes y líneas guía; devuelve el área de trazado y el máximo de la escala.
    /// </summary>
    private (RectangleF Trazado, decimal Maximo) DibujarEjes(Graphics g, Rectangle cuerpo, Font fuente, Brush pincelTexto, decimal maximoDatos)
    {
        var maximo = MaximoRedondeado(maximoDatos);
        var anchoEtiqueta = Math.Max(g.MeasureString(_formato(maximo), fuente).Width, 30) + 8;
        var trazado = RectangleF.FromLTRB(cuerpo.Left + anchoEtiqueta, cuerpo.Top + 8, cuerpo.Right - 8, cuerpo.Bottom - 34);

        using var lapizGrilla = new Pen(ColorGrilla);
        using var formatoDerecha = new StringFormat { Alignment = StringAlignment.Far, LineAlignment = StringAlignment.Center };
        const int divisiones = 5;
        for (var i = 0; i <= divisiones; i++)
        {
            var y = trazado.Bottom - trazado.Height * i / divisiones;
            g.DrawLine(lapizGrilla, trazado.Left, y, trazado.Right, y);
            g.DrawString(_formato(maximo * i / divisiones), fuente, pincelTexto,
                new RectangleF(cuerpo.Left, y - 10, anchoEtiqueta - 6, 20), formatoDerecha);
        }

        return (trazado, maximo);
    }

    private void DibujarEtiquetasCategorias(Graphics g, RectangleF trazado, Font fuente, Brush pincelTexto, float anchoGrupo, bool centradoEnGrupo)
    {
        var anchoMaximo = _categorias.Max(c => g.MeasureString(c, fuente).Width) + 6;
        var salto = Math.Max(1, (int)Math.Ceiling(anchoMaximo / Math.Max(anchoGrupo, 1)));
        using var formato = new StringFormat { Alignment = StringAlignment.Center, Trimming = StringTrimming.EllipsisCharacter, FormatFlags = StringFormatFlags.NoWrap };

        for (var i = 0; i < _categorias.Count; i += salto)
        {
            var centro = centradoEnGrupo ? trazado.Left + anchoGrupo * (i + 0.5f) : trazado.Left + anchoGrupo * i;
            var ancho = Math.Max(anchoGrupo * salto, anchoMaximo);
            g.DrawString(_categorias[i], fuente, pincelTexto, new RectangleF(centro - ancho / 2, trazado.Bottom + 6, ancho, 20), formato);
        }
    }

    private void DibujarColumnas(Graphics g, Rectangle cuerpo, Font fuente, Brush pincelTexto)
    {
        DibujarLeyenda(g, cuerpo, fuente, pincelTexto);
        var maximoDatos = _series.SelectMany(s => s.Valores).DefaultIfEmpty(0).Max();
        var (trazado, maximo) = DibujarEjes(g, cuerpo, fuente, pincelTexto, maximoDatos);

        var anchoGrupo = trazado.Width / _categorias.Count;
        var anchoBarra = Math.Clamp(anchoGrupo * 0.75f / _series.Count, 2f, AnchoMaximoBarra);
        using var formatoCentro = new StringFormat { Alignment = StringAlignment.Center };

        for (var c = 0; c < _categorias.Count; c++)
        {
            var inicioGrupo = trazado.Left + anchoGrupo * c + (anchoGrupo - anchoBarra * _series.Count) / 2;
            for (var s = 0; s < _series.Count; s++)
            {
                var valor = c < _series[s].Valores.Count ? _series[s].Valores[c] : 0;
                var alto = (float)(valor / maximo) * trazado.Height;
                var rect = new RectangleF(inicioGrupo + anchoBarra * s, trazado.Bottom - alto, anchoBarra - 1, alto);
                using var pincel = new SolidBrush(_series[s].Color);
                g.FillRectangle(pincel, rect);

                if (valor > 0 && anchoBarra >= 18)
                    g.DrawString(_formato(valor), fuente, pincelTexto, new RectangleF(rect.Left - 20, rect.Top - 16, rect.Width + 40, 16), formatoCentro);
            }
        }

        DibujarEtiquetasCategorias(g, trazado, fuente, pincelTexto, anchoGrupo, true);
    }

    private void DibujarLinea(Graphics g, Rectangle cuerpo, Font fuente, Brush pincelTexto)
    {
        var variasSeries = _series.Count > 1;
        if (variasSeries)
            DibujarLeyenda(g, cuerpo, fuente, pincelTexto);

        var maximoDatos = _series.SelectMany(s => s.Valores).DefaultIfEmpty(0).Max();
        var (trazado, maximo) = DibujarEjes(g, cuerpo, fuente, pincelTexto, maximoDatos);

        var paso = _categorias.Count > 1 ? trazado.Width / (_categorias.Count - 1) : 0;
        using var formatoCentro = new StringFormat { Alignment = StringAlignment.Center };

        foreach (var serie in _series)
        {
            var puntos = serie.Valores
                .Select((v, i) => new PointF(
                    _categorias.Count > 1 ? trazado.Left + paso * i : trazado.Left + trazado.Width / 2,
                    trazado.Bottom - (float)(v / maximo) * trazado.Height))
                .ToArray();

            if (puntos.Length > 1)
            {
                // Con varias series el relleno taparía las otras líneas
                if (!variasSeries)
                {
                    using var relleno = new SolidBrush(Color.FromArgb(50, serie.Color));
                    var area = puntos.Prepend(new PointF(puntos[0].X, trazado.Bottom)).Append(new PointF(puntos[^1].X, trazado.Bottom)).ToArray();
                    g.FillPolygon(relleno, area);
                }
                using var lapiz = new Pen(serie.Color, 2.5f);
                g.DrawLines(lapiz, puntos);
            }

            using var pincelPunto = new SolidBrush(serie.Color);
            var radio = puntos.Length > 40 ? 2.5f : 4f;
            var mostrarValores = !variasSeries && puntos.Length <= 16;
            for (var i = 0; i < puntos.Length; i++)
            {
                g.FillEllipse(pincelPunto, puntos[i].X - radio, puntos[i].Y - radio, radio * 2, radio * 2);
                if (mostrarValores)
                    g.DrawString(_formato(serie.Valores[i]), fuente, pincelTexto, new RectangleF(puntos[i].X - 50, puntos[i].Y - 20, 100, 16), formatoCentro);
            }
        }

        var anchoGrupo = _categorias.Count > 1 ? paso : trazado.Width;
        if (_categorias.Count > 1)
            DibujarEtiquetasCategorias(g, trazado, fuente, pincelTexto, anchoGrupo, false);
        else
            g.DrawString(_categorias[0], fuente, pincelTexto, new RectangleF(trazado.Left, trazado.Bottom + 6, trazado.Width, 20), formatoCentro);
    }

    private void DibujarBarrasHorizontales(Graphics g, Rectangle cuerpo, Font fuente, Brush pincelTexto)
    {
        var serie = _series[0];
        var maximo = MaximoRedondeado(serie.Valores.DefaultIfEmpty(0).Max());
        var anchoEtiqueta = Math.Min(cuerpo.Width * 0.35f, _categorias.Max(c => g.MeasureString(c, fuente).Width) + 10);
        var anchoValor = g.MeasureString(_formato(maximo), fuente).Width + 10;
        var trazado = RectangleF.FromLTRB(cuerpo.Left + anchoEtiqueta, cuerpo.Top, cuerpo.Right - anchoValor, cuerpo.Bottom);

        var altoFila = Math.Min(40f, trazado.Height / _categorias.Count);
        using var formatoEtiqueta = new StringFormat { Alignment = StringAlignment.Far, LineAlignment = StringAlignment.Center, Trimming = StringTrimming.EllipsisCharacter, FormatFlags = StringFormatFlags.NoWrap };
        using var formatoValor = new StringFormat { LineAlignment = StringAlignment.Center };

        for (var i = 0; i < _categorias.Count; i++)
        {
            var y = trazado.Top + altoFila * i;
            var valor = i < serie.Valores.Count ? serie.Valores[i] : 0;
            var ancho = (float)(valor / maximo) * trazado.Width;
            using var pincel = new SolidBrush(Paleta[i % Paleta.Length]);
            g.FillRectangle(pincel, trazado.Left, y + altoFila * 0.15f, ancho, altoFila * 0.7f);
            g.DrawString(_categorias[i], fuente, pincelTexto, new RectangleF(cuerpo.Left, y, anchoEtiqueta - 8, altoFila), formatoEtiqueta);
            g.DrawString(_formato(valor), fuente, pincelTexto, new RectangleF(trazado.Left + ancho + 4, y, anchoValor + 40, altoFila), formatoValor);
        }
    }

    private void DibujarDona(Graphics g, Rectangle cuerpo, Font fuente, Brush pincelTexto)
    {
        var valores = _series[0].Valores;
        var total = valores.Sum();
        var diametro = Math.Min(cuerpo.Height - 10, (int)(cuerpo.Width * 0.5));
        var circulo = new Rectangle(cuerpo.Left + 20, cuerpo.Top + (cuerpo.Height - diametro) / 2, diametro, diametro);

        float angulo = -90;
        for (var i = 0; i < valores.Count; i++)
        {
            if (total <= 0) break;
            var barrido = (float)(valores[i] / total) * 360f;
            using var pincel = new SolidBrush(Paleta[i % Paleta.Length]);
            g.FillPie(pincel, circulo, angulo, barrido);
            angulo += barrido;
        }

        var hueco = Rectangle.Inflate(circulo, -diametro / 4, -diametro / 4);
        using (var pincelFondo = new SolidBrush(Color.White))
            g.FillEllipse(pincelFondo, hueco);

        using var fuenteTotal = new Font("Segoe UI", 12F, FontStyle.Bold);
        using var formatoCentro = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center };
        g.DrawString($"Total\n{_formato(total)}", fuenteTotal, pincelTexto, hueco, formatoCentro);

        var xLeyenda = circulo.Right + 30;
        var altoFila = Math.Min(26f, (float)cuerpo.Height / Math.Max(valores.Count, 1));
        var yLeyenda = cuerpo.Top + (cuerpo.Height - altoFila * valores.Count) / 2;
        using var formatoLeyenda = new StringFormat { LineAlignment = StringAlignment.Center, Trimming = StringTrimming.EllipsisCharacter };
        for (var i = 0; i < valores.Count; i++)
        {
            var porcentaje = total > 0 ? valores[i] / total * 100 : 0;
            using var pincel = new SolidBrush(Paleta[i % Paleta.Length]);
            var y = yLeyenda + altoFila * i;
            g.FillRectangle(pincel, xLeyenda, y + altoFila / 2 - 6, 12, 12);
            g.DrawString($"{_categorias[i]}: {_formato(valores[i])} ({porcentaje:0.#}%)", fuente, pincelTexto,
                new RectangleF(xLeyenda + 18, y, cuerpo.Right - xLeyenda - 18, altoFila), formatoLeyenda);
        }
    }

    private static decimal MaximoRedondeado(decimal valor)
    {
        if (valor <= 5) return 5;
        var exponente = Math.Pow(10, Math.Floor(Math.Log10((double)valor)));
        var fraccion = (double)valor / exponente;
        var redondeo = fraccion <= 1 ? 1 : fraccion <= 2 ? 2 : fraccion <= 5 ? 5 : 10;
        return (decimal)(redondeo * exponente);
    }
}
