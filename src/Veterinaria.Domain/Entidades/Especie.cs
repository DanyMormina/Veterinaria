using Veterinaria.Domain.Comunes;

namespace Veterinaria.Domain.Entidades;

public class Especie : Satelite
{
    public ICollection<Raza> Razas { get; set; } = [];
}