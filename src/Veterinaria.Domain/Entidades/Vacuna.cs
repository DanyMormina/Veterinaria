using Veterinaria.Domain.Comunes;

namespace Veterinaria.Domain.Entidades
{
    public class Vacuna : Auditable
    {
        public string Nombre { get; set; } = string.Empty;
        public long PeriodoMesesRecomendado { get; set; }
    }
}