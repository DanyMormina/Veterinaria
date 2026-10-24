namespace Veterinaria.Domain.Dtos;

/// <summary>
/// DTO de lectura para la consulta histórica de copias de seguridad en la grilla administrativa.
/// </summary>
public class BackupHistorialDto
{
    public long IdBackup { get; set; }
    public string NombreArchivo { get; set; } = string.Empty;
    public DateTime FechaCreacion { get; set; }
    public string FechaHoraFormateada => FechaCreacion.ToString("dd/MM/yyyy HH:mm:ss");
    public string UsuarioResponsable { get; set; } = string.Empty;
    public string RutaDestino { get; set; } = string.Empty;
}
