using Veterinaria.Domain.Comunes;

namespace Veterinaria.Domain.Entidades;

public class DetalleConsulta : Auditable
{
    public long IdConsulta { get; set; }
    public long IdTratamiento { get; set; }
    public int Cantidad { get; set; } = 1;
    public decimal PrecioUnitario { get; set; }
    public decimal Subtotal { get; set; }
    public string? Indicaciones { get; set; }

    public Consulta Consulta { get; set; } = null!;
    public Tratamiento Tratamiento { get; set; } = null!;
}
