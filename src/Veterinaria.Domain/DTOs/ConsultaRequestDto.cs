namespace Veterinaria.Domain.Dtos;

public class ConsultaRequestDto
{
    public long IdUsuario { get; set; }
    public long IdMascota { get; set; }
    public DateTime FechaHora { get; set; } = DateTime.Now;
    public string? Motivo { get; set; }
    public decimal? PesoKg { get; set; }
    public decimal? Temperatura { get; set; }
    public string Diagnostico { get; set; } = string.Empty;
    public string? Observaciones { get; set; }

    // Compatibilidad si se enviaba como IdVeterinario
    public long IdVeterinario
    {
        get => IdUsuario;
        set => IdUsuario = value;
    }
}
