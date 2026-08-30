namespace Veterinaria.Domain.Dtos;

/// <summary>
/// DTO de respuesta para la entidad Propietario.
/// </summary>
public class PropietarioResponseDto
{
    public long Id { get; set; }
    public string DNI { get; set; } = string.Empty;
    public string Nombre { get; set; } = string.Empty;
    public string Apellido { get; set; } = string.Empty;
    public string? Telefono { get; set; }
    public string? Email { get; set; }
    public string? Direccion { get; set; }
    public bool Activo { get; set; }
    public int CantidadMascotas { get; set; }
}
