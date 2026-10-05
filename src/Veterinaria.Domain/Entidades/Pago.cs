using Veterinaria.Domain.Comunes;

namespace Veterinaria.Domain.Entidades;

public class Pago : EntidadAuditable
{
    public long IdConsulta { get; set; }
    public long IdMetodoPago { get; set; }
    public DateTime Fecha { get; set; } = DateTime.Now;
    public decimal Importe { get; set; }
    public string Estado { get; set; } = "Completado";
    /// <summary>Cantidad de cuotas (solo aplica a tarjeta de crédito).</summary>
    public int? Cuotas { get; set; }

    public Consulta Consulta { get; set; } = null!;
    public MetodoPago MetodoPago { get; set; } = null!;
}
