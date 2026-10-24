using System.Globalization;

namespace Veterinaria.WinForms.Controles;

public enum AgrupacionPeriodo { Automatico, Dia, Mes, Anio }

/// <summary>
/// Arma los períodos del eje de un gráfico por día, mes o año según la agrupación elegida.
/// En modo automático depende del rango: por día (hasta 31 días), por mes (hasta 24 meses) o por año.
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

    public static PeriodosGrafico Armar(DateTime desde, DateTime hasta, AgrupacionPeriodo agrupacion = AgrupacionPeriodo.Automatico)
    {
        desde = desde.Date;
        hasta = hasta.Date;
        var dias = (hasta - desde).TotalDays + 1;

        var meses = (hasta.Year - desde.Year) * 12 + hasta.Month - desde.Month + 1;

        if (agrupacion == AgrupacionPeriodo.Automatico)
            agrupacion = dias <= 31 ? AgrupacionPeriodo.Dia : meses <= 24 ? AgrupacionPeriodo.Mes : AgrupacionPeriodo.Anio;

        // Demasiados períodos no se pueden leer: se pasa a la unidad siguiente
        if (agrupacion == AgrupacionPeriodo.Dia && dias > MaximoDias)
            agrupacion = AgrupacionPeriodo.Mes;
        if (agrupacion == AgrupacionPeriodo.Mes && meses > MaximoMeses)
            agrupacion = AgrupacionPeriodo.Anio;

        if (agrupacion == AgrupacionPeriodo.Dia)
        {
            var claves = Enumerable.Range(0, (int)dias).Select(i => desde.AddDays(i)).ToList();
            var formato = desde.Year == hasta.Year ? "dd/MM" : "dd/MM/yy";
            return new(claves, claves.Select(d => d.ToString(formato, Cultura)).ToList(), "día", d => d.Date);
        }

        if (agrupacion == AgrupacionPeriodo.Mes)
        {
            var claves = new List<DateTime>();
            for (var m = new DateTime(desde.Year, desde.Month, 1); m <= hasta; m = m.AddMonths(1))
                claves.Add(m);
            var mismoAnio = desde.Year == hasta.Year;
            return new(claves, claves.Select(m => EtiquetaMes(m, mismoAnio)).ToList(), "mes", d => new DateTime(d.Year, d.Month, 1));
        }

        var anios = Enumerable.Range(desde.Year, hasta.Year - desde.Year + 1).Select(a => new DateTime(a, 1, 1)).ToList();
        return new(anios, anios.Select(a => a.Year.ToString(Cultura)).ToList(), "año", d => new DateTime(d.Year, 1, 1));
    }

    private const int MaximoDias = 92;
    private const int MaximoMeses = 60;

    /// <summary>Etiqueta corta: "Ene" dentro del mismo año, "Ene 25" si el rango abarca varios años.</summary>
    private static string EtiquetaMes(DateTime mes, bool mismoAnio)
    {
        var nombre = Cultura.DateTimeFormat.GetAbbreviatedMonthName(mes.Month).TrimEnd('.');
        nombre = char.ToUpper(nombre[0], Cultura) + nombre[1..];
        return mismoAnio ? nombre : $"{nombre} {mes:yy}";
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
