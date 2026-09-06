namespace Veterinaria.Domain.Dtos;

/// <summary>
/// DTO para la creación y actualización de una Raza.
/// </summary>
public class RazaSolicitudDto
{
    public string Nombre { get; set; } = string.Empty;
    public long IdEspecie { get; set; }
}
