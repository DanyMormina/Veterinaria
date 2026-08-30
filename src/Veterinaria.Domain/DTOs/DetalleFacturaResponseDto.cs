namespace Veterinaria.Domain.Dtos;

/// <summary>
/// DTO de respuesta para la entidad Detalle de Factura.
/// </summary>
public class DetalleFacturaResponseDto
{
    public long Id { get; set; }
    public long IdFactura { get; set; }
    public string Concepto { get; set; } = string.Empty;
    public long Cantidad { get; set; }
    public decimal PrecioUnitario { get; set; }
    public decimal Subtotal { get; set; }
    public bool Activo { get; set; }
}
