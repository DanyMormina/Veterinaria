using Veterinaria.Domain.Comunes;

namespace Veterinaria.Domain.Entidades;

public class Mascota : Auditable
{
    public long IdPropietario { get; set; }
    public long IdEspecie { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string Sexo { get; set; } = string.Empty;
    public DateTime? FechaNacimiento { get; set; }
    public string? Color { get; set; }

    public Propietario Propietario { get; set; } = null!;
    public Especie Especie { get; set; } = null!;
    public ICollection<Consulta> Consultas { get; set; } = [];
}