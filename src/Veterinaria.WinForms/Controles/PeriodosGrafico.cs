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
            var etiquetas = claves.Select(EtiquetaMes).ToList();
            var unidad = etiquetas.Count > 1 ? $"mes ({etiquetas[0]} a {etiquetas[^1]})" : $"mes ({etiquetas[0]})";
            return new(claves, etiquetas, unidad, d => new DateTime(d.Year, d.Month, 1));
        }

        var anios = Enumerable.Range(desde.Year, hasta.Year - desde.Year + 1).Select(a => new DateTime(a, 1, 1)).ToList();
        var unidadAnio = anios.Count > 1 ? $"año ({anios[0].Year} a {anios[^1].Year})" : $"año ({anios[0].Year})";
        return new(anios, anios.Select(a => a.Year.ToString(Cultura)).ToList(), unidadAnio, d => new DateTime(d.Year, 1, 1));
    }

    private const int MaximoDias = 92;
    private const int MaximoMeses = 60;
    private const int MesesVistaMensual = 12;
    private const int AniosVistaAnual = 4;

    /// <summary>
    /// Fecha "desde" que necesita la agrupación elegida para mostrar varios períodos:
    /// por mes los últimos 12 meses, por año los últimos 4 años y por día el mes de la fecha "hasta".
    /// Si el rango elegido ya alcanza, se respeta.
    /// </summary>
    public static DateTime DesdeParaAgrupacion(DateTime desde, DateTime hasta, AgrupacionPeriodo agrupacion)
    {
        desde = desde.Date;
        hasta = hasta.Date;
        var meses = (hasta.Year - desde.Year) * 12 + hasta.Month - desde.Month + 1;

        return agrupacion switch
        {
            AgrupacionPeriodo.Dia when (hasta - desde).TotalDays + 1 > 31 => new DateTime(hasta.Year, hasta.Month, 1),
            AgrupacionPeriodo.Mes when meses < MesesVistaMensual => new DateTime(hasta.Year, hasta.Month, 1).AddMonths(1 - MesesVistaMensual),
            AgrupacionPeriodo.Anio when hasta.Year - desde.Year + 1 < AniosVistaAnual => new DateTime(hasta.Year - AniosVistaAnual + 1, 1, 1),
            _ => desde
        };
    }

    /// <summary>Etiqueta con mes y año, por ejemplo "Nov 2025".</summary>
    private static string EtiquetaMes(DateTime mes)
    {
        var nombre = Cultura.DateTimeFormat.GetAbbreviatedMonthName(mes.Month).TrimEnd('.');
        nombre = char.ToUpper(nombre[0], Cultura) + nombre[1..];
        return $"{nombre} {mes.Year}";
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
