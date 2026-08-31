using Veterinaria.Domain.Comunes;

namespace Veterinaria.Domain.Entidades;

public class Raza : Satelite
{
    public long IdEspecie { get; set; }
    public Especie Especie { get; set; } = null!;
}