using Veterinaria.Domain.Comunes;

namespace Veterinaria.Domain.Entidades
{
    public class DetalleFactura : Auditable
    {
        public long IdFactura { get; set; }
        public string Concepto { get; set; } = string.Empty;
        public long Cantidad { get; set; } = 1;
        public decimal PrecioUnitario { get; set; }
        public decimal Subtotal { get; set; }
        public Factura Factura { get; set; } = null!;
    }
}