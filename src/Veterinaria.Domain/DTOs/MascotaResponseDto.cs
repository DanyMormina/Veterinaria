namespace Veterinaria.Domain.Dtos;

public class MascotaResponseDto
{
    public long Id { get; set; }
    public long IdPropietario { get; set; }
    public string NombrePropietario { get; set; } = string.Empty;
    public long IdRaza { get; set; }
    public string NombreRaza { get; set; } = string.Empty;
    public long IdEspecie { get; set; }
    public string NombreEspecie { get; set; } = string.Empty;
    public string Nombre { get; set; } = string.Empty;
    public string Sexo { get; set; } = string.Empty;
    public DateTime? FechaNacimiento { get; set; }
    public string? Color { get; set; }
    public bool Activo { get; set; }
    public int CantidadConsultas { get; set; }
}
