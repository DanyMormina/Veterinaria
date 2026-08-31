using Veterinaria.Domain.Comunes;

namespace Veterinaria.Domain.Entidades;

public class Propietario : Auditable
{
    public string DNI { get; set; } = string.Empty;
    public string Nombre { get; set; } = string.Empty;
    public string Apellido { get; set; } = string.Empty;
    public string? Telefono { get; set; }
    public string? Email { get; set; }
    public string? Direccion { get; set; }

    public ICollection<Mascota> Mascotas { get; set; } = [];
}