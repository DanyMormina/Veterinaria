using System.Drawing.Drawing2D;
using System.Globalization;

namespace Veterinaria.WinForms.Controles;

/// <summary>
/// Serie de valores para los gráficos de columnas, barras y línea.
/// </summary>
public sealed record SerieGrafico(string Nombre, Color Color, IReadOnlyList<decimal> Valores);

/// <summary>
/// Control de gráficos dibujado con GDI+ (sin librerías externas) para los reportes:
/// columnas agrupadas, barras horizontales, dona y línea. Incluye tooltips interactivos,
/// cabeceras adaptativas sin solapamiento y formato compacto de valores.
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
    private static readonly Color ColorTextoSecundario = Color.FromArgb(110, 105, 112);
    private static readonly Color ColorGrilla = Color.FromArgb(226, 217, 220);
    private static readonly CultureInfo Cultura = new("es-AR");

    private TipoGrafico _tipo = TipoGrafico.Ninguno;
    private string _titulo = string.Empty;
    private string _mensajeVacio = "Genere un reporte para ver el gráfico.";
    private IReadOnlyList<string> _categorias = [];
    private IReadOnlyList<SerieGrafico> _series = [];
    private Func<decimal, string> _formato = v => v.ToString("N0", Cultura);

    private readonly ToolTip _toolTip = new()
    {
        InitialDelay = 80,
        ReshowDelay = 80,
        AutoPopDelay = 5000
    };
    private string? _ultimoTextoTooltip;

    private sealed class ElementoInteractivo
    {
        public RectangleF Rectangulo { get; init; }
        public bool EsSectorCircular { get; init; }
        public PointF Centro { get; init; }
        public float RadioInterno { get; init; }
        public float RadioExterno { get; init; }
        public float AnguloInicio { get; init; }
        public float AnguloBarrido { get; init; }
        public string TextoTooltip { get; init; } = string.Empty;

        public bool Contiene(PointF pt)
        {
            if (EsSectorCircular)
            {
                var dx = pt.X - Centro.X;
                var dy = pt.Y - Centro.Y;
                var dist = Math.Sqrt(dx * dx + dy * dy);
                if (dist < RadioInterno || dist > RadioExterno)
                    return false;

                var angulo = Math.Atan2(dy, dx) * 180.0 / Math.PI;
                var norm = (angulo - AnguloInicio) % 360.0;
                if (norm < 0) norm += 360.0;
                return norm <= AnguloBarrido;
            }

            return Rectangulo.Contains(pt);
        }
    }

    private readonly List<ElementoInteractivo> _elementosInteractivos = [];

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
        _elementosInteractivos.Clear();
        _ultimoTextoTooltip = null;
        _toolTip.Hide(this);
        Invalidate();
    }

    public void MostrarColumnas(string titulo, IReadOnlyList<string> categorias, IReadOnlyList<SerieGrafico> series, Func<decimal, string>? formato = null) =>
        Mostrar(TipoGrafico.Columnas, titulo, categorias, series, formato);

    public void MostrarBarrasHorizontales(string titulo, IReadOnlyList<string> categorias, SerieGrafico serie, Func<decimal, string>? formato = null) =>
        Mostrar(TipoGrafico.BarrasHorizontales, titulo, categorias, [serie], formato);

    public void MostrarLinea(string titulo, IReadOnlyList<string> categorias, SerieGrafico serie, Func<decimal, string>? formato = null) =>
        Mostrar(categorias.Count < 3 ? TipoGrafico.Columnas : TipoGrafico.Linea, titulo, categorias, [serie], formato);

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
        _elementosInteractivos.Clear();
        _ultimoTextoTooltip = null;
        _toolTip.Hide(this);
        Invalidate();
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        base.OnPaint(e);
        Dibujar(e.Graphics, ClientRectangle);
    }

    protected override void OnMouseMove(MouseEventArgs e)
    {
        base.OnMouseMove(e);
        if (_tipo == TipoGrafico.Ninguno) return;

        var elemento = _elementosInteractivos.FirstOrDefault(elem => elem.Contiene(e.Location));
        if (elemento is not null)
        {
            if (_ultimoTextoTooltip != elemento.TextoTooltip)
            {
                _ultimoTextoTooltip = elemento.TextoTooltip;
                _toolTip.Show(elemento.TextoTooltip, this, e.X + 12, e.Y + 16, 4000);
            }
        }
        else
        {
            if (_ultimoTextoTooltip is not null)
            {
                _ultimoTextoTooltip = null;
                _toolTip.Hide(this);
            }
        }
    }

    protected override void OnMouseLeave(EventArgs e)
    {
        base.OnMouseLeave(e);
        _ultimoTextoTooltip = null;
        _toolTip.Hide(this);
    }

    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            _toolTip.Dispose();
        }
        base.Dispose(disposing);
    }

    /// <summary>
    /// Dibuja el gráfico actual dentro del área indicada (pantalla o página de impresión).
    /// </summary>
    public void Dibujar(Graphics g, Rectangle area)
    {
        g.SmoothingMode = SmoothingMode.AntiAlias;
        g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;

        using var fuente = new Font("Segoe UI", 8.5F);
        using var fuenteTitulo = new Font("Segoe UI", 10.5F, FontStyle.Bold);
        using var pincelTexto = new SolidBrush(ColorTexto);

        if (_tipo == TipoGrafico.Ninguno)
        {
            using var formatoCentro = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center };
            g.DrawString(_mensajeVacio, fuente, Brushes.Gray, area, formatoCentro);
            return;
        }

        // Si estamos dibujando en pantalla (mismo tamaño que el control), actualizamos la lista interactiva
        if (area.Width == ClientRectangle.Width && area.Height == ClientRectangle.Height)
        {
            _elementosInteractivos.Clear();
        }

        // Evaluación dinámica del encabezado: si título y leyenda no entran juntos, se colocan en 2 renglones
        var hayLeyendaSuperior = (_tipo == TipoGrafico.Columnas && _series.Count > 0) ||
                                  (_tipo == TipoGrafico.Linea && _series.Count > 1);

        float anchoLeyenda = 0;
        if (hayLeyendaSuperior)
        {
            foreach (var s in _series)
            {
                anchoLeyenda += g.MeasureString(s.Nombre, fuente).Width + 24f;
            }
        }

        var tamTitulo = g.MeasureString(_titulo, fuenteTitulo);
        var anchoDisponible = (float)(area.Width - 24);
        var cabeEnUnaLinea = !hayLeyendaSuperior || (tamTitulo.Width + anchoLeyenda + 20f <= anchoDisponible);

        int altoEncabezado;
        if (cabeEnUnaLinea)
        {
            altoEncabezado = 36;
            g.DrawString(_titulo, fuenteTitulo, pincelTexto, area.Left + 12, area.Top + 8);
            if (hayLeyendaSuperior)
            {
                DibujarLeyendaHorizontal(g, area.Right - 12, area.Top + 10, fuente, pincelTexto);
            }
        }
        else
        {
            // Título arriba, leyenda en segundo renglón
            altoEncabezado = 54;
            g.DrawString(_titulo, fuenteTitulo, pincelTexto, area.Left + 12, area.Top + 6);
            if (hayLeyendaSuperior)
            {
                DibujarLeyendaHorizontal(g, area.Right - 12, area.Top + 30, fuente, pincelTexto);
            }
        }

        var cuerpo = Rectangle.FromLTRB(area.Left + 12, area.Top + altoEncabezado, area.Right - 12, area.Bottom - 10);
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

    private void DibujarLeyendaHorizontal(Graphics g, float rightX, float topY, Font fuente, Brush pincelTexto)
    {
        var x = rightX;
        for (var i = _series.Count - 1; i >= 0; i--)
        {
            var anchoTexto = g.MeasureString(_series[i].Nombre, fuente).Width;
            x -= (anchoTexto + 22f);
            using var pincel = new SolidBrush(_series[i].Color);
            g.FillRectangle(pincel, x, topY + 2f, 11f, 11f);
            g.DrawString(_series[i].Nombre, fuente, pincelTexto, x + 15f, topY);
        }
    }

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
        var maximoDatos = _series.SelectMany(s => s.Valores).DefaultIfEmpty(0).Max();
        var (trazado, maximo) = DibujarEjes(g, cuerpo, fuente, pincelTexto, maximoDatos);

        var anchoGrupo = trazado.Width / _categorias.Count;
        var anchoBarra = Math.Clamp(anchoGrupo * 0.75f / _series.Count, 2f, AnchoMaximoBarra);

        for (var c = 0; c < _categorias.Count; c++)
        {
            var inicioGrupo = trazado.Left + anchoGrupo * c + (anchoGrupo - anchoBarra * _series.Count) / 2;
            for (var s = 0; s < _series.Count; s++)
            {
                var valor = c < _series[s].Valores.Count ? _series[s].Valores[c] : 0;
                var alto = maximo > 0 ? (float)(valor / maximo) * trazado.Height : 0;
                var rect = new RectangleF(inicioGrupo + anchoBarra * s, trazado.Bottom - alto, Math.Max(1f, anchoBarra - 1), alto);
                using var pincel = new SolidBrush(_series[s].Color);
                g.FillRectangle(pincel, rect);

                // Registrar para hover tooltip
                _elementosInteractivos.Add(new ElementoInteractivo
                {
                    Rectangulo = new RectangleF(rect.Left, rect.Top - 8, Math.Max(rect.Width, 8), Math.Max(rect.Height + 8, 16)),
                    TextoTooltip = $"{_categorias[c]}\n{_series[s].Nombre}: {_formato(valor)}"
                });
                // Sin etiquetas encima de las barras: el valor se lee en el eje Y y al pasar el mouse
            }
        }

        DibujarEtiquetasCategorias(g, trazado, fuente, pincelTexto, anchoGrupo, true);
    }

    private void DibujarLinea(Graphics g, Rectangle cuerpo, Font fuente, Brush pincelTexto)
    {
        var variasSeries = _series.Count > 1;
        var maximoDatos = _series.SelectMany(s => s.Valores).DefaultIfEmpty(0).Max();
        var (trazado, maximo) = DibujarEjes(g, cuerpo, fuente, pincelTexto, maximoDatos);

        var paso = _categorias.Count > 1 ? trazado.Width / (_categorias.Count - 1) : 0;
        using var formatoCentro = new StringFormat { Alignment = StringAlignment.Center };

        foreach (var serie in _series)
        {
            var puntos = serie.Valores
                .Select((v, i) => new PointF(
                    _categorias.Count > 1 ? trazado.Left + paso * i : trazado.Left + trazado.Width / 2,
                    maximo > 0 ? trazado.Bottom - (float)(v / maximo) * trazado.Height : trazado.Bottom))
                .ToArray();

            if (puntos.Length > 1)
            {
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
            for (var i = 0; i < puntos.Length; i++)
            {
                g.FillEllipse(pincelPunto, puntos[i].X - radio, puntos[i].Y - radio, radio * 2, radio * 2);
                // Sin etiquetas encima de la línea: el valor se lee en el eje Y y al pasar el mouse

                _elementosInteractivos.Add(new ElementoInteractivo
                {
                    Rectangulo = new RectangleF(puntos[i].X - 10, puntos[i].Y - 10, 20, 20),
                    TextoTooltip = $"{_categorias[i]}\n{serie.Nombre}: {_formato(serie.Valores[i])}"
                });
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
            var ancho = maximo > 0 ? (float)(valor / maximo) * trazado.Width : 0;
            using var pincel = new SolidBrush(Paleta[i % Paleta.Length]);
            g.FillRectangle(pincel, trazado.Left, y + altoFila * 0.15f, ancho, altoFila * 0.7f);
            g.DrawString(_categorias[i], fuente, pincelTexto, new RectangleF(cuerpo.Left, y, anchoEtiqueta - 8, altoFila), formatoEtiqueta);
            g.DrawString(_formato(valor), fuente, pincelTexto, new RectangleF(trazado.Left + ancho + 4, y, anchoValor + 40, altoFila), formatoValor);

            _elementosInteractivos.Add(new ElementoInteractivo
            {
                Rectangulo = new RectangleF(trazado.Left, y, trazado.Width + anchoValor, altoFila),
                TextoTooltip = $"{_categorias[i]}: {_formato(valor)}"
            });
        }
    }

    private void DibujarDona(Graphics g, Rectangle cuerpo, Font fuente, Brush pincelTexto)
    {
        var valores = _series[0].Valores;
        var total = valores.Sum();
        if (valores.Count == 0 || total <= 0)
        {
            using var formatoCentro = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center };
            g.DrawString("Sin datos registrados para graficar.", fuente, Brushes.Gray, cuerpo, formatoCentro);
            return;
        }

        using var fuenteSecundaria = new Font("Segoe UI", 8F);
        using var pincelGris = new SolidBrush(ColorTextoSecundario);

        float anchoLeyendaMaximo = 0;
        for (var i = 0; i < valores.Count; i++)
        {
            var textoCompleto = $"{_categorias[i]}: {_formato(valores[i])} ({(valores[i] / total * 100):0.#}%)";
            var ancho = g.MeasureString(textoCompleto, fuente).Width;
            if (ancho > anchoLeyendaMaximo) anchoLeyendaMaximo = ancho;
        }

        // Asignar espacio equilibrado entre la dona y la leyenda
        var anchoLeyendaDeseado = Math.Clamp(anchoLeyendaMaximo + 26f, cuerpo.Width * 0.45f, cuerpo.Width * 0.60f);
        var espacioDonut = cuerpo.Width - anchoLeyendaDeseado - 16f;

        var diametro = Math.Min(cuerpo.Height - 12, (int)espacioDonut);
        diametro = Math.Max(diametro, 80);

        var circulo = new Rectangle(
            (int)(cuerpo.Left + Math.Max(4, (espacioDonut - diametro) / 2)),
            cuerpo.Top + (cuerpo.Height - diametro) / 2,
            diametro,
            diametro);

        var radioExterno = diametro / 2f;
        var radioInterno = diametro / 4f;
        var centroX = circulo.Left + radioExterno;
        var centroY = circulo.Top + radioExterno;

        float angulo = -90;
        for (var i = 0; i < valores.Count; i++)
        {
            var barrido = (float)(valores[i] / total) * 360f;
            using var pincel = new SolidBrush(Paleta[i % Paleta.Length]);
            g.FillPie(pincel, circulo, angulo, barrido);

            var porcentaje = valores[i] / total * 100;
            _elementosInteractivos.Add(new ElementoInteractivo
            {
                EsSectorCircular = true,
                Centro = new PointF(centroX, centroY),
                RadioInterno = radioInterno,
                RadioExterno = radioExterno,
                AnguloInicio = angulo,
                AnguloBarrido = barrido,
                TextoTooltip = $"{_categorias[i]}\n{_formato(valores[i])} ({porcentaje:0.#}% del total)"
            });

            angulo += barrido;
        }

        var hueco = Rectangle.Inflate(circulo, -(int)radioInterno, -(int)radioInterno);
        using (var pincelFondo = new SolidBrush(Color.White))
            g.FillEllipse(pincelFondo, hueco);

        using var fuenteTotal = new Font("Segoe UI", diametro > 140 ? 11F : 9.5F, FontStyle.Bold);
        using var formatoCentroTotal = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center };
        g.DrawString($"Total\n{_formato(total)}", fuenteTotal, pincelTexto, hueco, formatoCentroTotal);

        // Leyenda
        var xLeyenda = circulo.Right + 16f;
        var anchoLeyendaDisponible = cuerpo.Right - xLeyenda;
        var altoFila = Math.Clamp((float)(cuerpo.Height - 8) / Math.Max(valores.Count, 1), 24f, 38f);
        var yLeyenda = cuerpo.Top + (cuerpo.Height - altoFila * valores.Count) / 2;

        for (var i = 0; i < valores.Count; i++)
        {
            var porcentaje = valores[i] / total * 100;
            var y = yLeyenda + altoFila * i;
            using var pincel = new SolidBrush(Paleta[i % Paleta.Length]);
            g.FillRectangle(pincel, xLeyenda, y + 6f, 11f, 11f);

            var textoLineaUnica = $"{_categorias[i]}: {_formato(valores[i])} ({porcentaje:0.#}%)";
            var entraEnUnaLinea = g.MeasureString(textoLineaUnica, fuente).Width <= anchoLeyendaDisponible - 18f;
            var rectTexto = new RectangleF(xLeyenda + 16f, y, anchoLeyendaDisponible - 16f, altoFila);

            if (entraEnUnaLinea || altoFila < 28f)
            {
                using var formatoSimple = new StringFormat { LineAlignment = StringAlignment.Center, Trimming = StringTrimming.EllipsisCharacter, FormatFlags = StringFormatFlags.NoWrap };
                g.DrawString(textoLineaUnica, fuente, pincelTexto, rectTexto, formatoSimple);
            }
            else
            {
                // Formato en 2 líneas estructuradas: categoría arriba, importe abajo
                using var formatoNoWrap = new StringFormat { Trimming = StringTrimming.EllipsisCharacter, FormatFlags = StringFormatFlags.NoWrap };
                g.DrawString(_categorias[i], fuente, pincelTexto, new RectangleF(rectTexto.Left, y + 1, rectTexto.Width, 15), formatoNoWrap);
                g.DrawString($"{_formato(valores[i])} ({porcentaje:0.#}%)", fuenteSecundaria, pincelGris, new RectangleF(rectTexto.Left, y + 16, rectTexto.Width, 14), formatoNoWrap);
            }
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
