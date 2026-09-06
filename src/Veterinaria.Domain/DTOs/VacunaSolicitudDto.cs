namespace Veterinaria.Domain.Dtos;

public class VacunaSolicitudDto
{
    public long IdEspecie { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public int PeriodoMesesRecomendado { get; set; } = 12;
    public decimal Precio { get; set; }
}
