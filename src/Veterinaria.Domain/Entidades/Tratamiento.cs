using Veterinaria.Domain.Comunes;

namespace Veterinaria.Domain.Entidades;

public class Tratamiento : Auditable
{
    public string TipoTratamiento { get; set; } = string.Empty;
    public string Descripcion { get; set; } = string.Empty;
    public string? Dosis { get; set; }
    public decimal Precio { get; set; }

    public ICollection<DetalleConsulta> DetallesConsulta { get; set; } = [];
}