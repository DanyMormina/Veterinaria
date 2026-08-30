namespace Veterinaria.Domain.Dtos;

/// <summary>
/// DTO para la creación y actualización de una Sesión.
/// </summary>
public class SesionRequestDto
{
    public long IdUsuario { get; set; }
    public DateTime? FechaCierre { get; set; }
}
