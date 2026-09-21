namespace Veterinaria.Domain.Dtos;

/// <summary>
/// Parámetros y filtros para la generación de reportes clínicos y demográficos.
/// </summary>
public record FiltroReporteDto
{
    public string TipoReporte { get; init; } = string.Empty;
    public DateTime FechaDesde { get; init; }
    public DateTime FechaHasta { get; init; }
    public long? IdVeterinario { get; init; }
    public long? IdEspecie { get; init; }
}

/// <summary>
/// Modelo de datos para el reporte de Consultas Clínicas.
/// </summary>
public record ReporteConsultaClinicaDto
{
    public DateTime FechaHora { get; init; }
    public string Fecha => FechaHora.ToString("dd/MM/yyyy");
    public string Hora => FechaHora.ToString("HH:mm");
    public string Mascota { get; init; } = string.Empty;
    public string Especie { get; init; } = string.Empty;
    public string Raza { get; init; } = string.Empty;
    public string Propietario { get; init; } = string.Empty;
    public string Veterinario { get; init; } = string.Empty;
    public string Diagnostico { get; init; } = string.Empty;
}

/// <summary>
/// Modelo de datos para el reporte de Vacunas y Tratamientos aplicados.
/// </summary>
public record ReporteVacunaTratamientoDto
{
    public DateTime Fecha { get; init; }
    public string FechaFormateada => Fecha.ToString("dd/MM/yyyy");
    public string Mascota { get; init; } = string.Empty;
    public string Especie { get; init; } = string.Empty;
    public string TratamientoVacuna { get; init; } = string.Empty;
    public string Tipo { get; init; } = string.Empty; // "Vacunación" o "Tratamiento"
    public string Dosis { get; init; } = string.Empty;
    public string Veterinario { get; init; } = string.Empty;
}

/// <summary>
/// Modelo de datos para el reporte preventivo de Próximos Controles Sanitarios.
/// </summary>
public record ReporteProximoControlDto
{
    public DateTime FechaProximoControl { get; init; }
    public string FechaFormateada => FechaProximoControl.ToString("dd/MM/yyyy");
    public string Mascota { get; init; } = string.Empty;
    public string Especie { get; init; } = string.Empty;
    public string Propietario { get; init; } = string.Empty;
    public string Telefono { get; init; } = string.Empty;
    public string TratamientoAsociado { get; init; } = string.Empty;
}

/// <summary>
/// Modelo de datos para el Censo Demográfico de Pacientes (Mascotas registradas).
/// </summary>
public record ReporteCensoMascotaDto
{
    public long Id { get; init; }
    public string Nombre { get; init; } = string.Empty;
    public string Especie { get; init; } = string.Empty;
    public string Raza { get; init; } = string.Empty;
    public string Sexo { get; init; } = string.Empty;
    public DateTime? FechaNacimiento { get; init; }
    public string FechaNacimientoFormateada => FechaNacimiento.HasValue ? FechaNacimiento.Value.ToString("dd/MM/yyyy") : "-";
    public string Color { get; init; } = string.Empty;
    public string Propietario { get; init; } = string.Empty;
}
