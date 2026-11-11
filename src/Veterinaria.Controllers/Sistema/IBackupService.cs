using Veterinaria.Domain.Comunes;
using Veterinaria.Domain.Dtos;

namespace Veterinaria.Interfaces.Services.Sistema;
// Contrato de servicio para la gestión de copias de seguridad del sistema.
public interface IBackupService
{
    // Genera una copia de seguridad física de la base de datos SQL Server y registra la auditoría.
    Task<Result> GenerarBackupAsync(string nombreSugerido, int idUsuario);
    // Obtiene el historial completo de copias de seguridad registradas en el sistema.
    Task<Result<IEnumerable<BackupHistorialDto>>> ObtenerHistorialAsync();
    // Restaura una copia de seguridad física (.bak) sobre la base de datos SQL Server.
    // Requiere privilegios administrativos exclusivos y desconexión de usuarios concurrentes.
    Task<Result> RestaurarBaseDatosAsync(string nombreArchivoBak, long idUsuarioResponsable);
    string ObtenerRutaDestinoPorDefecto();
}

