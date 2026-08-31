using Veterinaria.Domain.Comunes;

namespace Veterinaria.Domain.Entidades;

public class Vacuna : Auditable
{
    public string Nombre { get; set; } = string.Empty;
    public int PeriodoMesesRecomendado { get; set; } = 12;

    public ICollection<AplicacionVacuna> AplicacionesVacuna { get; set; } = [];
}