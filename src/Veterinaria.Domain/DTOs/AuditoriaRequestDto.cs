namespace Veterinaria.Domain.Dtos;

/// <summary>
/// DTO para la creación y registro de una Auditoría.
/// </summary>
public class AuditoriaRequestDto
{
    public long IdUsuario { get; set; }
    public string Accion { get; set; } = string.Empty;
    public string TablaAfectada { get; set; } = string.Empty;
    public string Detalle { get; set; } = string.Empty;
}
