using Veterinaria.Domain.Comunes;
using Veterinaria.Domain.Dtos;
using Veterinaria.Interfaces.Services.Sistema;

namespace Veterinaria.Controllers.Sistema;

/// <summary>
/// Controlador de la capa de presentación/aplicación para la gestión de copias de seguridad.
/// </summary>
public class BackupController
{
    private readonly IBackupService _backupService;

    public BackupController(IBackupService backupService)
    {
        _backupService = backupService;
    }

    /// <summary>
    /// Extrae el identificador del usuario en sesión y ejecuta la generación de la copia de seguridad.
    /// </summary>
    /// <param name="nombreArchivo">Nombre del archivo de respaldo ingresado por el usuario.</param>
    /// <param name="idUsuarioSesion">ID opcional del usuario en sesión para desacoplar de contexto estático.</param>
    /// <returns>Resultado de la operación.</returns>
    public async Task<Result> RealizarBackupAsync(string nombreArchivo, long? idUsuarioSesion = null)
    {
        // 1. Extraer ID del usuario desde parámetro o sesión
        long? idUsuarioActual = idUsuarioSesion;

        if (!idUsuarioActual.HasValue || idUsuarioActual.Value <= 0)
        {
            return Result.Falla("No se pudo identificar una sesión activa de usuario válida.");
        }

        // 2. Invocar el servicio de negocio
        return await _backupService.GenerarBackupAsync(nombreArchivo, (int)idUsuarioActual.Value);
    }

    /// <summary>
    /// Envoltorio seguro para consultar el historial de copias registradas.
    /// </summary>
    public async Task<Result<IEnumerable<BackupHistorialDto>>> CargarHistorialAsync()
    {
        return await _backupService.ObtenerHistorialAsync();
    }

    /// <summary>
    /// Retorna el directorio destino configurado para mostrar en la interfaz de usuario.
    /// </summary>
    public string ObtenerDirectorioDestino()
    {
        return _backupService.ObtenerRutaDestinoPorDefecto();
    }
}
