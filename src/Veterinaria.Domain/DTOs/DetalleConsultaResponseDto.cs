namespace Veterinaria.Domain.Dtos;

public class DetalleConsultaResponseDto
{
    public long Id { get; set; }
    public long IdConsulta { get; set; }
    public long IdTratamiento { get; set; }
    public string TipoTratamiento { get; set; } = string.Empty;
    public string DescripcionTratamiento { get; set; } = string.Empty;
    public int Cantidad { get; set; }
    public decimal PrecioUnitario { get; set; }
    public decimal Subtotal { get; set; }
    public string? Indicaciones { get; set; }
    public bool Activo { get; set; }
}
