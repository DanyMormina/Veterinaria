namespace Veterinaria.Domain.Dtos;

public class TratamientoSolicitudDto
{
    public string TipoTratamiento { get; set; } = string.Empty;
    public string Descripcion { get; set; } = string.Empty;
    public string? Dosis { get; set; }
    public decimal Precio { get; set; }
}
