namespace Veterinaria.Domain.Dtos;

public class DetalleConsultaRequestDto
{
    public long IdConsulta { get; set; }
    public long IdTratamiento { get; set; }
    public int Cantidad { get; set; } = 1;
    public decimal PrecioUnitario { get; set; }
    public decimal Subtotal { get; set; }
    public string? Indicaciones { get; set; }
}
