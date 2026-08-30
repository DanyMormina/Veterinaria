using Veterinaria.Domain.Comunes;

namespace Veterinaria.Domain.Entidades
{
    public class Sesion : Auditable
    {
        public long IdUsuario { get; set; }
        public DateTime FechaInicio { get; set; } = DateTime.Now;
        public DateTime? FechaCierre { get; set; }
        public Usuario Usuario { get; set; } = null!;
    }
}