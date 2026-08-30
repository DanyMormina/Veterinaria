namespace Veterinaria.Domain.Dtos;

/// <summary>
/// DTO de respuesta para la entidad Mascota con detalles de navegación aplanados.
/// </summary>
public class MascotaResponseDto
{
    public long Id { get; set; }
    public long IdPropietario { get; set; }
    public string NombrePropietario { get; set; } = string.Empty;
    public long IdRaza { get; set; }
    public string NombreRaza { get; set; } = string.Empty;
    public string NombreEspecie { get; set; } = string.Empty;
    public string Nombre { get; set; } = string.Empty;
    public string Sexo { get; set; } = string.Empty;
    public DateTime FechaNacimiento { get; set; }
    public string? Color { get; set; }
    public decimal Peso { get; set; }
    public bool Activo { get; set; }
}
