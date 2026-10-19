using System.Globalization;

namespace Veterinaria.WinForms.Controles;

/// <summary>
/// Arma los períodos del eje de un gráfico según el rango filtrado:
/// por día (hasta 31 días), por mes (hasta un año) o por año.
/// </summary>
public sealed class PeriodosGrafico
{
    private static readonly CultureInfo Cultura = new("es-AR");

    public IReadOnlyList<DateTime> Claves { get; }
    public IReadOnlyList<string> Etiquetas { get; }
    public string Unidad { get; }
    private readonly Func<DateTime, DateTime> _clave;

    private PeriodosGrafico(IReadOnlyList<DateTime> claves, IReadOnlyList<string> etiquetas, string unidad, Func<DateTime, DateTime> clave)
    {
        Claves = claves;
        Etiquetas = etiquetas;
        Unidad = unidad;
        _clave = clave;
    }

    public static PeriodosGrafico Armar(DateTime desde, DateTime hasta)
    {
        desde = desde.Date;
        hasta = hasta.Date;
        var dias = (hasta - desde).TotalDays + 1;

        if (dias <= 31)
        {
            var claves = Enumerable.Range(0, (int)dias).Select(i => desde.AddDays(i)).ToList();
            return new(claves, claves.Select(d => d.ToString("dd/MM", Cultura)).ToList(), "día", d => d.Date);
        }

        if (dias <= 366)
        {
            var claves = new List<DateTime>();
            for (var m = new DateTime(desde.Year, desde.Month, 1); m <= hasta; m = m.AddMonths(1))
                claves.Add(m);
            return new(claves, claves.Select(m => m.ToString("MMM yyyy", Cultura)).ToList(), "mes", d => new DateTime(d.Year, d.Month, 1));
        }

        var anios = Enumerable.Range(desde.Year, hasta.Year - desde.Year + 1).Select(a => new DateTime(a, 1, 1)).ToList();
        return new(anios, anios.Select(a => a.Year.ToString(Cultura)).ToList(), "año", d => new DateTime(d.Year, 1, 1));
    }

    /// <summary>
    /// Suma el valor de cada elemento en su período; los períodos sin datos quedan en 0.
    /// </summary>
    public List<decimal> Acumular<T>(IEnumerable<T> datos, Func<T, DateTime> fecha, Func<T, decimal> valor)
    {
        var totales = Claves.ToDictionary(c => c, _ => 0m);
        foreach (var d in datos)
        {
            var clave = _clave(fecha(d));
            if (totales.ContainsKey(clave))
                totales[clave] += valor(d);
        }
        return Claves.Select(c => totales[c]).ToList();
    }
}
