namespace Veterinaria.Domain.Dtos;

public class PagoResponseDto
{
    public long Id { get; set; }
    public long IdConsulta { get; set; }
    public long IdMetodoPago { get; set; }
    public string NombreMetodoPago { get; set; } = string.Empty;
    public DateTime Fecha { get; set; }
    public decimal Importe { get; set; }
    public string Estado { get; set; } = string.Empty;
    public bool Activo { get; set; }
}
