using Veterinaria.Domain.Comunes;

namespace Veterinaria.Domain.Entidades;

public class TipoUsuario : Satelite
{
    public ICollection<Usuario> Usuarios { get; set; } = [];
}
