using Veterinaria.Domain.Comunes;

namespace Veterinaria.Domain.Entidades;

public class Pago : Auditable
{
    public long IdConsulta { get; set; }
    public long IdMetodoPago { get; set; }
    public DateTime Fecha { get; set; } = DateTime.Now;
    public decimal Importe { get; set; }
    public string Estado { get; set; } = "Completado";

    public Consulta Consulta { get; set; } = null!;
    public MetodoPago MetodoPago { get; set; } = null!;
}
