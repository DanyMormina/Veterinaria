namespace Veterinaria.Domain.Dtos;

/// <summary>
/// DTO de respuesta para la entidad Sesión.
/// </summary>
public class SesionResponseDto
{
    public long Id { get; set; }
    public long IdUsuario { get; set; }
    public string NombreUsuario { get; set; } = string.Empty;
    public DateTime FechaInicio { get; set; }
    public DateTime? FechaCierre { get; set; }
    public bool Activo { get; set; }
}
