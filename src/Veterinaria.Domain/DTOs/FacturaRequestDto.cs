namespace Veterinaria.Domain.Dtos;

/// <summary>
/// DTO para la creación y actualización de una Factura.
/// </summary>
public class FacturaRequestDto
{
    public long IdPropietario { get; set; }
    public long IdUsuario { get; set; }
    public long IdMetodoPago { get; set; }
    public DateTime FechaEmision { get; set; } = DateTime.Now;
    public decimal Total { get; set; }
    public List<DetalleFacturaRequestDto> Detalles { get; set; } = [];
}
