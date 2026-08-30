using Veterinaria.Domain.Comunes;

namespace Veterinaria.Domain.Entidades
{
    public class Consulta : Auditable
    {
        public long IdMascota { get; set; }
        public long IdVeterinario { get; set; }
        public DateTime FechaHora { get; set; } = DateTime.Now;
        public decimal PesoKg { get; set; }
        public decimal Temperatura { get; set; }
        public string Diagnostico { get; set; } = string.Empty;
        public string? Observaciones { get; set; }
        public Mascota Mascota { get; set; } = null!;
        public Usuario Veterinario { get; set; } = null!;
        public ICollection<Tratamiento> Tratamientos { get; set; } = [];
    }
}