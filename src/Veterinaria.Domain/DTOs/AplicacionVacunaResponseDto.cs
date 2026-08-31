namespace Veterinaria.Domain.Dtos;

public class AplicacionVacunaResponseDto
{
    public long Id { get; set; }
    public long IdConsulta { get; set; }
    public long IdVacuna { get; set; }
    public string NombreVacuna { get; set; } = string.Empty;
    public DateTime FechaAplicacion { get; set; }
    public DateTime? ProximaDosis { get; set; }
    public string? Observaciones { get; set; }
    public decimal PrecioAplicado { get; set; }
    public bool Activo { get; set; }
}
