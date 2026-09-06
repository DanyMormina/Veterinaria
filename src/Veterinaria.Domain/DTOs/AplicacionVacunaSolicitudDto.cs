namespace Veterinaria.Domain.Dtos;

public class AplicacionVacunaSolicitudDto
{
    public long IdConsulta { get; set; }
    public long IdVacuna { get; set; }
    public DateTime FechaAplicacion { get; set; } = DateTime.Today;
    public DateTime? ProximaDosis { get; set; }
    public string? Observaciones { get; set; }
}
