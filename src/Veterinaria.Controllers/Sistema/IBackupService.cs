using Veterinaria.Domain.Comunes;
using Veterinaria.Domain.Dtos;

namespace Veterinaria.Interfaces.Services.Sistema;

/// <summary>
/// Contrato de servicio para la gestión de copias de seguridad del sistema.
/// </summary>
public interface IBackupService
{
    /// <summary>
    /// Genera una copia de seguridad física de la base de datos SQL Server y registra la auditoría.
    /// </summary>
    /// <param name="nombreSugerido">Nombre deseado para el archivo .bak.</param>
    /// <param name="idUsuario">Identificador del usuario que solicita la operación.</param>
    /// <returns>Resultado de la operación.</returns>
    Task<Result> GenerarBackupAsync(string nombreSugerido, int idUsuario);

    /// <summary>
    /// Obtiene el historial completo de copias de seguridad registradas en el sistema.
    /// </summary>
    /// <returns>Colección de DTOs con los registros históricos.</returns>
    Task<Result<IEnumerable<BackupHistorialDto>>> ObtenerHistorialAsync();

    /// <summary>
    /// Restaura una copia de seguridad física (.bak) sobre la base de datos SQL Server.
    /// Requiere privilegios administrativos exclusivos y desconexión de usuarios concurrentes.
    /// </summary>
    /// <param name="nombreArchivoBak">Nombre del archivo de respaldo ubicado en el directorio predeterminado.</param>
    /// <param name="idUsuarioResponsable">Identificador del usuario con permisos que autorizó la operación.</param>
    /// <returns>Resultado de la restauración.</returns>
    Task<Result> RestaurarBaseDatosAsync(string nombreArchivoBak, long idUsuarioResponsable);

    /// <summary>
    /// Retorna la ruta física por defecto configurada para el almacenamiento de respaldos.
    /// </summary>
    /// <returns>Ruta de directorio (ej. C:\Backup_Veterinaria).</returns>
    string ObtenerRutaDestinoPorDefecto();
}

