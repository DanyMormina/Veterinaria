using Microsoft.EntityFrameworkCore;
using Veterinaria.Controllers.Seguridad;
using Veterinaria.CrossCutting.Session;
using Veterinaria.Domain.Comunes;
using Veterinaria.Domain.Dtos;
using Veterinaria.Infrastructure;
using Veterinaria.Interfaces.Services.Seguridad;
using Veterinaria.Interfaces.Services.Sistema;

namespace Veterinaria.Controllers.Sistema;

/// <summary>
/// Controlador de la capa de presentación/aplicación para la gestión de copias de seguridad y restauración.
/// </summary>
public class BackupController
{
    private readonly IBackupService _backupService;
    private readonly IUsuarioService _usuarioService;
    private readonly ContextoVeterinaria _contexto;

    public BackupController(
        IBackupService backupService,
        IUsuarioService usuarioService,
        ContextoVeterinaria contexto)
    {
        _backupService = backupService;
        _usuarioService = usuarioService;
        _contexto = contexto;
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
    /// Solicita y ejecuta la restauración de una copia de seguridad tras validar doble factor:
    /// 1. Permiso exclusivo PuedeRestaurarBD verificado en tiempo real contra la base de datos.
    /// 2. Verificación de contraseña administrativa del usuario en sesión.
    /// </summary>
    /// <param name="nombreArchivoBak">Nombre del archivo .bak ubicado en el directorio de backups.</param>
    /// <param name="passwordConfirmacion">Contraseña ingresada por el usuario para doble factor de seguridad.</param>
    /// <param name="idUsuarioSesion">ID opcional del usuario en sesión para desacoplar de contexto estático.</param>
    /// <returns>Resultado de la operación de restauración.</returns>
    public async Task<Result> SolicitarRestauracionAsync(
        string nombreArchivoBak,
        string passwordConfirmacion,
        long? idUsuarioSesion = null)
    {
        // 1. Identificar usuario en sesión
        long? idUsuarioActual = idUsuarioSesion ?? (SesionActual.EstaAutenticado ? SesionActual.IdUsuario : null);

        if (!idUsuarioActual.HasValue || idUsuarioActual.Value <= 0)
        {
            return Result.Falla("No se pudo identificar una sesión activa de usuario válida.");
        }

        if (string.IsNullOrWhiteSpace(nombreArchivoBak))
        {
            return Result.Falla("Debe especificar el archivo de copia de seguridad a restaurar.");
        }

        if (string.IsNullOrWhiteSpace(passwordConfirmacion))
        {
            return Result.Falla("Debe ingresar su contraseña de administrador para confirmar la restauración.");
        }

        // Paso A: Verificación dinámica de autorización en la base de datos
        var validacionPermiso = await _usuarioService.ValidarAutorizacionRestoreAsync(idUsuarioActual.Value);
        if (!validacionPermiso.EsExitoso || !validacionPermiso.Valor)
        {
            return Result.Falla(validacionPermiso.Mensaje);
        }

        // Paso B: Doble factor mediante verificación de contraseña
        var usuario = await _contexto.Usuarios
            .AsNoTracking()
            .Where(u => u.Id == idUsuarioActual.Value && u.Activo)
            .Select(u => new { u.Id, PasswordHash = u.HashContrasena })
            .FirstOrDefaultAsync();

        if (usuario == null)
        {
            return Result.Falla("Usuario no encontrado o inactivo.");
        }

        if (!PasswordHasher.Verificar(passwordConfirmacion, usuario.PasswordHash))
        {
            return Result.Falla("La contraseña ingresada es incorrecta. Operación cancelada.");
        }

        // Paso C: Ejecutar la restauración de la base de datos
        return await _backupService.RestaurarBaseDatosAsync(nombreArchivoBak, idUsuarioActual.Value);
    }

    /// <summary>
    /// Retorna el directorio destino configurado para mostrar en la interfaz de usuario.
    /// </summary>
    public string ObtenerDirectorioDestino()
    {
        return _backupService.ObtenerRutaDestinoPorDefecto();
    }
}

