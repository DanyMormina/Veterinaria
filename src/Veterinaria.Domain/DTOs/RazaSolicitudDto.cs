namespace Veterinaria.Domain.Dtos;
// DTO para la creación y actualización de una Raza.
public class RazaSolicitudDto
{
    public string Nombre { get; set; } = string.Empty;
    public long IdEspecie { get; set; }
}
