namespace Veterinaria.Domain.Dtos;

/// <summary>
/// DTO de respuesta para la entidad Raza.
/// </summary>
public class RazaResponseDto
{
    public long Id { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public long IdEspecie { get; set; }
    public string NombreEspecie { get; set; } = string.Empty;
    public bool Activo { get; set; }
}
