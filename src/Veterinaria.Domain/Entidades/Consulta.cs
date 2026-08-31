using Veterinaria.Domain.Comunes;

namespace Veterinaria.Domain.Entidades;

public class Consulta : Auditable
{
    public long IdUsuario { get; set; }
    public long IdMascota { get; set; }
    public DateTime FechaHora { get; set; } = DateTime.Now;
    public string? Motivo { get; set; }
    public decimal? PesoKg { get; set; }
    public decimal? Temperatura { get; set; }
    public string Diagnostico { get; set; } = string.Empty;
    public string? Observaciones { get; set; }

    public Usuario Usuario { get; set; } = null!;
    public Mascota Mascota { get; set; } = null!;
    public ICollection<DetalleConsulta> DetallesConsulta { get; set; } = [];
    public ICollection<AplicacionVacuna> AplicacionesVacuna { get; set; } = [];
    public ICollection<Pago> Pagos { get; set; } = [];
}