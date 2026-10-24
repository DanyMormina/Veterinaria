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
    /// Verifica la contraseña contra los gerentes activos registrados en la base (segunda autorización del restore).
    /// </summary>
    private async Task<bool> VerificarPasswordGerenteAsync(string passwordGerente)
    {
        if (string.IsNullOrWhiteSpace(passwordGerente))
            return false;

        var hashesGerentes = await _contexto.Usuarios
            .AsNoTracking()
            .Where(u => u.Activo && u.TipoUsuario.Nombre == "Gerente")
            .Select(u => u.HashContrasena)
            .ToListAsync();

        return hashesGerentes.Any(hash => PasswordHasher.Verificar(passwordGerente, hash));
    }

    /// <summary>
    /// Primer paso del restore: permiso PuedeRestaurarBD y contraseña del administrador en sesión.
    /// </summary>
    public async Task<Result> VerificarAdministradorParaRestoreAsync(string passwordConfirmacion, long? idUsuarioSesion = null)
    {
        long? idUsuarioActual = idUsuarioSesion ?? (SesionActual.EstaAutenticado ? SesionActual.IdUsuario : null);

        if (!idUsuarioActual.HasValue || idUsuarioActual.Value <= 0)
        {
            return Result.Falla("No se pudo identificar una sesión activa de usuario válida.");
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

        // Paso B: Verificación de contraseña del administrador
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

        return Result.Ok("Contraseña del administrador verificada.");
    }

    /// <summary>
    /// Envoltorio seguro para consultar el historial de copias registradas.
    /// </summary>
    public async Task<Result<IEnumerable<BackupHistorialDto>>> CargarHistorialAsync()
    {
        return await _backupService.ObtenerHistorialAsync();
    }

    /// <summary>
    /// Solicita y ejecuta la restauración de una copia de seguridad tras validar doble autorización:
    /// 1. Permiso PuedeRestaurarBD y contraseña del administrador en sesión.
    /// 2. Contraseña de un gerente activo registrado en la base de datos.
    /// </summary>
    /// <param name="nombreArchivoBak">Nombre del archivo .bak ubicado en el directorio de backups.</param>
    /// <param name="passwordConfirmacion">Contraseña del administrador en sesión.</param>
    /// <param name="passwordGerente">Contraseña del gerente que autoriza la operación.</param>
    /// <param name="idUsuarioSesion">ID opcional del usuario en sesión para desacoplar de contexto estático.</param>
    /// <returns>Resultado de la operación de restauración.</returns>
    public async Task<Result> SolicitarRestauracionAsync(
        string nombreArchivoBak,
        string passwordConfirmacion,
        string passwordGerente,
        long? idUsuarioSesion = null)
    {
        long? idUsuarioActual = idUsuarioSesion ?? (SesionActual.EstaAutenticado ? SesionActual.IdUsuario : null);

        if (string.IsNullOrWhiteSpace(nombreArchivoBak))
        {
            return Result.Falla("Debe especificar el archivo de copia de seguridad a restaurar.");
        }

        // Paso A y B: permiso y contraseña del administrador
        var verificacionAdmin = await VerificarAdministradorParaRestoreAsync(passwordConfirmacion, idUsuarioActual);
        if (!verificacionAdmin.EsExitoso)
        {
            return verificacionAdmin;
        }

        // Paso B2: Autorización del gerente
        if (!await VerificarPasswordGerenteAsync(passwordGerente))
        {
            return Result.Falla("La contraseña del gerente es incorrecta. Operación no autorizada.");
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

