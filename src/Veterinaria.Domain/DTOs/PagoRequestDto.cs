namespace Veterinaria.Domain.Dtos;

public class PagoRequestDto
{
    public long IdConsulta { get; set; }
    public long IdMetodoPago { get; set; }
    public DateTime Fecha { get; set; } = DateTime.Now;
    public decimal Importe { get; set; }
    public string Estado { get; set; } = "Completado";
}
