namespace Veterinaria.Domain.Dtos;

/// <summary>
/// DTO para la creación y actualización de un Propietario.
/// </summary>
public class PropietarioRequestDto
{
    public string DNI { get; set; } = string.Empty;
    public string Nombre { get; set; } = string.Empty;
    public string Apellido { get; set; } = string.Empty;
    public string? Telefono { get; set; }
    public string? Email { get; set; }
    public string? Direccion { get; set; }
}
