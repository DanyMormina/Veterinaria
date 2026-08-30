using Veterinaria.Domain.Comunes;

namespace Veterinaria.Domain.Entidades
{
    public class Mascota : Auditable
    {
        public long IdPropietario { get; set; }
        public long IdRaza { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Sexo { get; set; } = string.Empty;
        public DateTime FechaNacimiento { get; set; }
        public string? Color { get; set; }
        public decimal Peso { get; set; }
        public Propietario Propietario { get; set; } = null!;
        public Raza Raza { get; set; } = null!;
    }
}