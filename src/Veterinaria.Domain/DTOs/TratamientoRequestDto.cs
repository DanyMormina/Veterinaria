namespace Veterinaria.Domain.Dtos;

/// <summary>
/// DTO para la creación y actualización de un Tratamiento.
/// </summary>
public class TratamientoRequestDto
{
    public long IdConsulta { get; set; }
    public long? IdVacuna { get; set; }
    public string TipoTratamiento { get; set; } = "Medicamento";
    public string Descripcion { get; set; } = string.Empty;
    public string? Dosis { get; set; }
    public string? Indicaciones { get; set; }
    public DateTime? FechaProximoControl { get; set; }
}
