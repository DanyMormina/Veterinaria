namespace Veterinaria.Domain.Dtos;

/// <summary>
/// DTO de respuesta para la entidad Consulta.
/// </summary>
public class ConsultaResponseDto
{
    public long Id { get; set; }
    public long IdMascota { get; set; }
    public string NombreMascota { get; set; } = string.Empty;
    public string NombrePropietario { get; set; } = string.Empty;
    public long IdVeterinario { get; set; }
    public string NombreVeterinario { get; set; } = string.Empty;
    public DateTime FechaHora { get; set; }
    public decimal PesoKg { get; set; }
    public decimal Temperatura { get; set; }
    public string Diagnostico { get; set; } = string.Empty;
    public string? Observaciones { get; set; }
    public bool Activo { get; set; }
    public int CantidadTratamientos { get; set; }
}
