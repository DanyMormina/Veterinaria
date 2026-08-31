namespace Veterinaria.Domain.Dtos;

public class TratamientoResponseDto
{
    public long Id { get; set; }
    public string TipoTratamiento { get; set; } = string.Empty;
    public string Descripcion { get; set; } = string.Empty;
    public string? Dosis { get; set; }
    public decimal Precio { get; set; }
    public bool Activo { get; set; }
}
