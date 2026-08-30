namespace Veterinaria.Domain.Dtos;

/// <summary>
/// DTO para la creación y actualización de un Turno.
/// </summary>
public class TurnoRequestDto
{
    public long IdMascota { get; set; }
    public long IdVeterinario { get; set; }
    public long? IdConsulta { get; set; }
    public DateTime FechaHora { get; set; }
    public string? Motivo { get; set; }
    public string Estado { get; set; } = "Pendiente";
}
