using Veterinaria.Domain.Comunes;

namespace Veterinaria.Domain.Entidades
{
    public class Turno : Auditable
    {
        public long IdMascota { get; set; }
        public long IdVeterinario { get; set; }
        public long? IdConsulta { get; set; } // Null hasta que sea atendido
        public DateTime FechaHora { get; set; }
        public string? Motivo { get; set; }
        public string Estado { get; set; } = "Pendiente"; // Pendiente, Atendido, Cancelado
        public Mascota Mascota { get; set; } = null!;
        public Usuario Veterinario { get; set; } = null!;
        public Consulta? Consulta { get; set; }
    }
}