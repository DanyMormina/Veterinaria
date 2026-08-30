namespace Veterinaria.Domain.Dtos;

/// <summary>
/// DTO para la creación y actualización de una Consulta.
/// </summary>
public class ConsultaRequestDto
{
    public long IdMascota { get; set; }
    public long IdVeterinario { get; set; }
    public DateTime FechaHora { get; set; } = DateTime.Now;
    public decimal PesoKg { get; set; }
    public decimal Temperatura { get; set; }
    public string Diagnostico { get; set; } = string.Empty;
    public string? Observaciones { get; set; }
}
