namespace Veterinaria.Domain.Dtos;
// DTO de respuesta para la entidad Especie.
public class EspecieRespuestaDto
{
    public long Id { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public bool Activo { get; set; }
}
