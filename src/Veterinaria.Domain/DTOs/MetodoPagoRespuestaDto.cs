namespace Veterinaria.Domain.Dtos;
// DTO de respuesta para la entidad Método de Pago.
public class MetodoPagoRespuestaDto
{
    public long Id { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public bool Activo { get; set; }
}
