namespace Veterinaria.Domain.Dtos;

/// <summary>
/// DTO para la creación y actualización de una Mascota.
/// </summary>
public class MascotaRequestDto
{
    public long IdPropietario { get; set; }
    public long IdRaza { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string Sexo { get; set; } = string.Empty;
    public DateTime FechaNacimiento { get; set; }
    public string? Color { get; set; }
    public decimal Peso { get; set; }
}
