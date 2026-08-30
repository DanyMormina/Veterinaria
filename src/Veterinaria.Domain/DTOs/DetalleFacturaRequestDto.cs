namespace Veterinaria.Domain.Dtos;

/// <summary>
/// DTO para la creación y actualización de un Detalle de Factura.
/// </summary>
public class DetalleFacturaRequestDto
{
    public long IdFactura { get; set; }
    public string Concepto { get; set; } = string.Empty;
    public long Cantidad { get; set; } = 1;
    public decimal PrecioUnitario { get; set; }
    public decimal Subtotal { get; set; }
}
