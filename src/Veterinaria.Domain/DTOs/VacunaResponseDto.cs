namespace Veterinaria.Domain.Dtos;

public class VacunaResponseDto
{
    public long Id { get; set; }
    public long IdEspecie { get; set; }
    public string NombreEspecie { get; set; } = string.Empty;
    public string Nombre { get; set; } = string.Empty;
    public int PeriodoMesesRecomendado { get; set; }
    public decimal Precio { get; set; }
    public bool Activo { get; set; }
}
