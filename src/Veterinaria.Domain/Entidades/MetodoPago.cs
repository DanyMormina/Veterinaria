using Veterinaria.Domain.Comunes;

namespace Veterinaria.Domain.Entidades;

public class MetodoPago : Satelite
{
    public ICollection<Pago> Pagos { get; set; } = [];
}