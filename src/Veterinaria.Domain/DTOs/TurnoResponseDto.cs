namespace Veterinaria.Domain.Dtos;

/// <summary>
/// DTO de respuesta para la entidad Turno con datos aplanados de Mascota y Veterinario.
/// </summary>
public class TurnoResponseDto
{
    public long Id { get; set; }
    public long IdMascota { get; set; }
    public string NombreMascota { get; set; } = string.Empty;
    public string NombrePropietario { get; set; } = string.Empty;
    public long IdVeterinario { get; set; }
    public string NombreVeterinario { get; set; } = string.Empty;
    public long? IdConsulta { get; set; }
    public DateTime FechaHora { get; set; }
    public string? Motivo { get; set; }
    public string Estado { get; set; } = string.Empty;
    public bool Activo { get; set; }
}
