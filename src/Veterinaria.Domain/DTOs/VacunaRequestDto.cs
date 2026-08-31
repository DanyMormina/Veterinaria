namespace Veterinaria.Domain.Dtos;

/// <summary>
/// DTO para la creación y actualización de una Vacuna.
/// </summary>
public class VacunaRequestDto
{
    public string Nombre { get; set; } = string.Empty;
    public int PeriodoMesesRecomendado { get; set; } = 12;
}
