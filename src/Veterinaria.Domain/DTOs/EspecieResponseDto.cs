namespace Veterinaria.Domain.Dtos;

/// <summary>
/// DTO de respuesta para la entidad Especie.
/// </summary>
public class EspecieResponseDto
{
    public long Id { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public bool Activo { get; set; }
}
