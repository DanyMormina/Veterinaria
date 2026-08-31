namespace Veterinaria.Domain.Dtos;

public class ConsultaResponseDto
{
    public long Id { get; set; }
    public long IdUsuario { get; set; }
    public string NombreUsuario { get; set; } = string.Empty;
    public long IdMascota { get; set; }
    public string NombreMascota { get; set; } = string.Empty;
    public string NombrePropietario { get; set; } = string.Empty;
    public DateTime FechaHora { get; set; }
    public string? Motivo { get; set; }
    public decimal? PesoKg { get; set; }
    public decimal? Temperatura { get; set; }
    public string Diagnostico { get; set; } = string.Empty;
    public string? Observaciones { get; set; }
    public bool Activo { get; set; }
    public int CantidadTratamientos { get; set; }
    public int CantidadVacunas { get; set; }
    public int CantidadPagos { get; set; }

    // Compatibilidad con vistas previas
    public long IdVeterinario => IdUsuario;
    public string NombreVeterinario => NombreUsuario;
}
