namespace Veterinaria.Domain.Dtos;

/// <summary>
/// DTO de respuesta para la entidad Factura con detalles y nombres aplanados.
/// </summary>
public class FacturaResponseDto
{
    public long Id { get; set; }
    public long IdPropietario { get; set; }
    public string NombrePropietario { get; set; } = string.Empty;
    public long IdUsuario { get; set; }
    public string NombreUsuario { get; set; } = string.Empty;
    public long IdMetodoPago { get; set; }
    public string NombreMetodoPago { get; set; } = string.Empty;
    public DateTime FechaEmision { get; set; }
    public decimal Total { get; set; }
    public bool Activo { get; set; }
    public List<DetalleFacturaResponseDto> Detalles { get; set; } = [];
}
