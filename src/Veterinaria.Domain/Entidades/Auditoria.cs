using Veterinaria.Domain.Comunes;

namespace Veterinaria.Domain.Entidades
{
    public class Auditoria : Auditable
    {
        public long IdUsuario { get; set; }
        public DateTime FechaHora { get; set; } = DateTime.Now;
        public string Accion { get; set; } = string.Empty;
        public string TablaAfectada { get; set; } = string.Empty;
        public string Detalle { get; set; } = string.Empty;
        public Usuario Usuario { get; set; } = null!;
    }
}