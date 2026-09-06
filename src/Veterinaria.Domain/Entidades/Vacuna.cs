using Veterinaria.Domain.Comunes;

namespace Veterinaria.Domain.Entidades;

public class Vacuna : Auditable
{
    public long IdEspecie { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public int PeriodoMesesRecomendado { get; set; } = 12;
    public decimal Precio { get; set; }

    public Especie Especie { get; set; } = null!;
    public ICollection<AplicacionVacuna> AplicacionesVacuna { get; set; } = [];
}
