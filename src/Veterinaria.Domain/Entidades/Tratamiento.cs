using Veterinaria.Domain.Comunes;

namespace Veterinaria.Domain.Entidades
{
    public class Tratamiento : Auditable
    {
        public long IdConsulta { get; set; }
        public long? IdVacuna { get; set; }
        public string TipoTratamiento { get; set; } = "Medicamento";
        public string Descripcion { get; set; } = string.Empty;
        public string? Dosis { get; set; }
        public string? Indicaciones { get; set; }
        public DateTime? FechaProximoControl { get; set; }
        public Consulta Consulta { get; set; } = null!; 
        public Vacuna? Vacuna { get; set; }
    }
}