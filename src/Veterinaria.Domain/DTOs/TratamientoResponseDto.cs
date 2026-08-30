namespace Veterinaria.Domain.Dtos;

/// <summary>
/// DTO de respuesta para la entidad Tratamiento.
/// </summary>
public class TratamientoResponseDto
{
    public long Id { get; set; }
    public long IdConsulta { get; set; }
    public long? IdVacuna { get; set; }
    public string? NombreVacuna { get; set; }
    public string TipoTratamiento { get; set; } = string.Empty;
    public string Descripcion { get; set; } = string.Empty;
    public string? Dosis { get; set; }
    public string? Indicaciones { get; set; }
    public DateTime? FechaProximoControl { get; set; }
    public bool Activo { get; set; }
}
