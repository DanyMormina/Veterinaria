using Veterinaria.Domain.Comunes;

namespace Veterinaria.Domain.Entidades
{
    public class Factura : Auditable
    {
        public long IdPropietario { get; set; }
        public long IdUsuario { get; set; }
        public long IdMetodoPago { get; set; }
        public DateTime FechaEmision { get; set; } = DateTime.Now;
        public decimal Total { get; set; }
        public Propietario Propietario { get; set; } = null!;
        public Usuario Usuario { get; set; } = null!;
        public MetodoPago MetodoPago { get; set; } = null!;
        public ICollection<DetalleFactura> Detalles { get; set; } = [];
    }
}