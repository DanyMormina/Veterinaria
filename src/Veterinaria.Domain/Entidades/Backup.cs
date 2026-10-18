using Veterinaria.Domain.Comunes;

namespace Veterinaria.Domain.Entidades;

/// <summary>
/// Representa el registro histórico de una copia de seguridad generada en la base de datos.
/// </summary>
public class Backup : ISoftDeletable
{
    public long IdBackup { get; set; }
    public long IdUsuario { get; set; }
    public string NombreArchivo { get; set; } = string.Empty;
    public DateTime FechaCreacion { get; set; } = DateTime.Now;
    public string RutaDestino { get; set; } = string.Empty;
    public bool Activo { get; set; } = true;

    public virtual Usuario? Usuario { get; set; }
}
