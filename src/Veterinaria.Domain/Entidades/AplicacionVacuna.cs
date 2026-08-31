using Veterinaria.Domain.Comunes;

namespace Veterinaria.Domain.Entidades;

public class AplicacionVacuna : Auditable
{
    public long IdConsulta { get; set; }
    public long IdVacuna { get; set; }
    public DateTime FechaAplicacion { get; set; } = DateTime.Today;
    public DateTime? ProximaDosis { get; set; }
    public string? Observaciones { get; set; }
    public decimal PrecioAplicado { get; set; }

    public Consulta Consulta { get; set; } = null!;
    public Vacuna Vacuna { get; set; } = null!;
}
