namespace Veterinaria.Domain.Dtos;

/// <summary>
/// DTO de respuesta para la entidad Vacuna.
/// </summary>
public class VacunaResponseDto
{
    public long Id { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public long PeriodoMesesRecomendado { get; set; }
    public bool Activo { get; set; }
}
