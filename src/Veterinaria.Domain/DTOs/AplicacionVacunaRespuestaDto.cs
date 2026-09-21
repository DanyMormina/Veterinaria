namespace Veterinaria.Domain.Dtos;

public class AplicacionVacunaRespuestaDto
{
    public long Id { get; set; }
    public long IdConsulta { get; set; }
    public long IdVacuna { get; set; }
    public string NombreVacuna { get; set; } = string.Empty;
    public long IdMascota { get; set; }
    public string NombreMascota { get; set; } = string.Empty;
    public DateTime FechaAplicacion { get; set; }
    public DateTime? ProximaDosis { get; set; }
    public string? Observaciones { get; set; }
    public decimal? PrecioUnitario { get; set; }
    public decimal? Precio { get => PrecioUnitario; set => PrecioUnitario = value; }
}
