namespace Veterinaria.Domain.Dtos;

/// <summary>
/// DTO de respuesta para la entidad Auditoría.
/// </summary>
public class AuditoriaResponseDto
{
    public long Id { get; set; }
    public long IdUsuario { get; set; }
    public string NombreUsuario { get; set; } = string.Empty;
    public DateTime FechaHora { get; set; }
    public string Accion { get; set; } = string.Empty;
    public string TablaAfectada { get; set; } = string.Empty;
    public string Detalle { get; set; } = string.Empty;
    public bool Activo { get; set; }
}
