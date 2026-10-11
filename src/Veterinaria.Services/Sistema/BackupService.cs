using System.Data;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Veterinaria.Domain.Comunes;
using Veterinaria.Domain.Dtos;
using Veterinaria.Domain.Entidades;
using Veterinaria.Infrastructure;
using Veterinaria.Interfaces.Services.Sistema;

namespace Veterinaria.Services.Sistema;

/// <summary>
/// Servicio de negocio para la gestión, ejecución y auditoría de copias de seguridad.
/// Compatible tanto con SQL Server local en Windows como en entornos contenerizados Docker / Linux.
/// </summary>
public class BackupService : IBackupService
{
    private readonly ContextoVeterinaria _contexto;
    private const string RutaPorDefecto = @"C:\Backup_Veterinaria";

    public BackupService(ContextoVeterinaria contexto)
    {
        _contexto = contexto;
    }

    /// <summary>
    /// Retorna la ruta física por defecto configurada para el almacenamiento de respaldos.
    /// </summary>
    public string ObtenerRutaDestinoPorDefecto()
    {
        return RutaPorDefecto;
    }

    /// <summary>
    /// Genera la copia de seguridad física de la base de datos y asienta el registro en dbo.Backup.
    /// Si el motor de base de datos corre en Linux/Docker, genera el respaldo dentro del contenedor
    /// y transmite el archivo .bak automáticamente a la máquina anfitriona Windows.
    /// </summary>
    public async Task<Result> GenerarBackupAsync(string nombreSugerido, int idUsuario)
    {
        try
        {
            // 1. Validación de nombre de archivo no vacío
            if (string.IsNullOrWhiteSpace(nombreSugerido))
            {
                return Result.Falla("El nombre del archivo de backup no puede estar vacío.");
            }

            // 2. Limpieza y validación de caracteres prohibidos
            string nombreLimpio = nombreSugerido.Trim();
            char[] caracteresInvalidos = Path.GetInvalidFileNameChars();
            if (nombreLimpio.Any(c => caracteresInvalidos.Contains(c)))
            {
                return Result.Falla("El nombre del archivo contiene caracteres no válidos para el sistema de archivos.");
            }

            // 3. Asegurar la extensión estándar .bak
            if (!nombreLimpio.EndsWith(".bak", StringComparison.OrdinalIgnoreCase))
            {
                nombreLimpio += ".bak";
            }

            // 4. Verificación y creación automática de directorio en Windows
            string directorio = ObtenerRutaDestinoPorDefecto();
            AsegurarDirectorioConPermisos(directorio);

            string rutaCompletaWindows = Path.Combine(directorio, nombreLimpio);

            // 5. Determinar nombre de la base de datos activa
            var conexion = _contexto.Database.GetDbConnection();
            string nombreBd = !string.IsNullOrWhiteSpace(conexion.Database)
                ? conexion.Database
                : "VeterinariaDb";

            // 6. Detectar si el motor SQL Server corre sobre Linux / Docker
            bool esLinux = await EsMotorLinuxAsync();

            if (esLinux)
            {
                // Escenario Docker / Linux: El motor SQL Server no conoce discos 'C:\'.
                // Se genera en el almacenamiento interno de SQL Server y se transmite a Windows.
                string rutaLinuxInterna = $"/var/opt/mssql/data/{nombreLimpio}";

                string comandoSql = $@"
                    BACKUP DATABASE [{nombreBd}]
                    TO DISK = @ruta
                    WITH FORMAT, INIT, NAME = @nombre;";

                var paramRuta = new SqlParameter("@ruta", rutaLinuxInterna);
                var paramNombre = new SqlParameter("@nombre", $"Respaldo_{nombreLimpio}");

                await _contexto.Database.ExecuteSqlRawAsync(comandoSql, paramRuta, paramNombre);

                // Transmitir y guardar el archivo .bak desde SQL Server a la carpeta local de Windows
                await ExtraerArchivoBackupDesdeServidorAsync(rutaLinuxInterna, rutaCompletaWindows);
            }
            else
            {
                // Escenario Windows nativo
                bool backupExitoso = false;
                try
                {
                    string comandoSql = $@"
                        BACKUP DATABASE [{nombreBd}]
                        TO DISK = @ruta
                        WITH FORMAT, INIT, NAME = @nombre;";

                    var paramRuta = new SqlParameter("@ruta", rutaCompletaWindows);
                    var paramNombre = new SqlParameter("@nombre", $"Respaldo_{nombreLimpio}");

                    await _contexto.Database.ExecuteSqlRawAsync(comandoSql, paramRuta, paramNombre);
                    backupExitoso = true;
                }
                catch (SqlException sqlEx) when (sqlEx.Number == 3201)
                {
                    // Fallback para Windows cuando el servicio no tiene permisos en la carpeta elegida
                    string? rutaServidor = await ObtenerRutaBackupPredeterminadaServidorAsync();
                    if (!string.IsNullOrWhiteSpace(rutaServidor) && Directory.Exists(rutaServidor))
                    {
                        string rutaTemp = Path.Combine(rutaServidor, nombreLimpio);
                        string cmdFallback = $@"
                            BACKUP DATABASE [{nombreBd}]
                            TO DISK = @rutaTemp
                            WITH FORMAT, INIT, NAME = @nombre;";

                        var pTemp = new SqlParameter("@rutaTemp", rutaTemp);
                        var pNombre = new SqlParameter("@nombre", $"Respaldo_{nombreLimpio}");

                        await _contexto.Database.ExecuteSqlRawAsync(cmdFallback, pTemp, pNombre);

                        if (File.Exists(rutaTemp))
                        {
                            File.Copy(rutaTemp, rutaCompletaWindows, overwrite: true);
                            backupExitoso = true;
                        }
                    }

                    if (!backupExitoso)
                    {
                        throw;
                    }
                }
            }

            // 7. Persistencia de auditoría en la tabla dbo.[Backup]
            var entidadBackup = new Backup
            {
                IdUsuario = idUsuario,
                NombreArchivo = nombreLimpio,
                RutaDestino = directorio,
                FechaCreacion = DateTime.Now,
                Activo = true
            };

            _contexto.Backups.Add(entidadBackup);
            await _contexto.SaveChangesAsync();

            return Result.Ok($"Copia de seguridad '{nombreLimpio}' generada exitosamente en '{directorio}'.");
        }
        catch (SqlException sqlEx)
        {
            string detalle = $"Error del motor SQL Server ({sqlEx.Number}): {sqlEx.Message}";
            if (sqlEx.Number == 3201)
            {
                detalle += $"\n\nSugerencia: El servicio de SQL Server no cuenta con permisos de escritura sobre '{RutaPorDefecto}'. Verifique los permisos de seguridad de la carpeta.";
            }
            return Result.Falla(detalle);
        }
        catch (Exception ex)
        {
            var mensaje = ex.InnerException != null
                ? $"{ex.Message} ({ex.InnerException.Message})"
                : ex.Message;
            return Result.Falla($"Error al generar el backup: {mensaje}");
        }
    }

    /// <summary>
    /// Consulta el historial cronológico descendente de copias de seguridad registradas.
    /// </summary>
    public async Task<Result<IEnumerable<BackupHistorialDto>>> ObtenerHistorialAsync()
    {
        try
        {
            var lista = await _contexto.Backups
                .AsNoTracking()
                .Include(b => b.Usuario)
                .OrderByDescending(b => b.FechaCreacion)
                .Select(b => new BackupHistorialDto
                {
                    IdBackup = b.IdBackup,
                    NombreArchivo = b.NombreArchivo,
                    FechaCreacion = b.FechaCreacion,
                    UsuarioResponsable = b.Usuario != null
                        ? (!string.IsNullOrWhiteSpace(b.Usuario.NombreUsuario) ? b.Usuario.NombreUsuario : $"{b.Usuario.Nombre} {b.Usuario.Apellido}".Trim())
                        : "Admin",
                    RutaDestino = b.RutaDestino
                })
                .ToListAsync();

            return Result<IEnumerable<BackupHistorialDto>>.Ok(lista);
        }
        catch (Exception ex)
        {
            return Result<IEnumerable<BackupHistorialDto>>.Falla($"Error al consultar el historial de backups: {ex.Message}");
        }
    }

    /// <summary>
    /// Determina si el motor SQL Server se está ejecutando sobre plataforma Linux (incluyendo Docker).
    /// </summary>
    private async Task<bool> EsMotorLinuxAsync()
    {
        try
        {
            using var comando = _contexto.Database.GetDbConnection().CreateCommand();
            comando.CommandText = "SELECT host_platform FROM sys.dm_os_host_info;";

            if (comando.Connection?.State != ConnectionState.Open)
            {
                await _contexto.Database.OpenConnectionAsync();
            }

            var plataforma = await comando.ExecuteScalarAsync();
            return string.Equals(plataforma?.ToString(), "Linux", StringComparison.OrdinalIgnoreCase);
        }
        catch
        {
            return false;
        }
    }

    /// <summary>
    /// Extrae un archivo generado dentro del servidor SQL Server (ej. Linux/Docker)
    /// y lo escribe en el sistema de archivos local de Windows mediante flujo binario OPENROWSET.
    /// </summary>
    private async Task ExtraerArchivoBackupDesdeServidorAsync(string rutaRemota, string rutaDestinoLocal)
    {
        var conexion = _contexto.Database.GetDbConnection();
        using var comando = conexion.CreateCommand();

        // OPENROWSET BULK con SINGLE_BLOB permite extraer el flujo completo de bytes del archivo .bak
        comando.CommandText = "DECLARE @sql NVARCHAR(MAX) = N'SELECT BulkColumn FROM OPENROWSET(BULK ' + QUOTENAME(@ruta, '''') + ', SINGLE_BLOB) as f'; EXEC sp_executesql @sql;";

        var param = comando.CreateParameter();
        param.ParameterName = "@ruta";
        param.Value = rutaRemota;
        comando.Parameters.Add(param);

        if (conexion.State != ConnectionState.Open)
        {
            await _contexto.Database.OpenConnectionAsync();
        }

        using var lector = await comando.ExecuteReaderAsync(CommandBehavior.SequentialAccess);
        if (await lector.ReadAsync())
        {
            using var streamRemoto = lector.GetStream(0);
            using var streamLocal = new FileStream(rutaDestinoLocal, FileMode.Create, FileAccess.Write, FileShare.None);
            await streamRemoto.CopyToAsync(streamLocal);
        }
    }

    /// <summary>
    /// Concede permisos de control total al identificador universal *S-1-1-0 (Everyone/Todos)
    /// sobre la carpeta para permitir al servicio de SQL Server escribir directamente si está en Windows.
    /// </summary>
    private static void AsegurarDirectorioConPermisos(string directorio)
    {
        try
        {
            if (!Directory.Exists(directorio))
            {
                Directory.CreateDirectory(directorio);
            }

            var psi = new System.Diagnostics.ProcessStartInfo
            {
                FileName = "icacls.exe",
                Arguments = $"\"{directorio}\" /grant \"*S-1-1-0:(OI)(CI)F\" /T /C /Q",
                CreateNoWindow = true,
                UseShellExecute = false
            };
            using var proceso = System.Diagnostics.Process.Start(psi);
            proceso?.WaitForExit(3000);
        }
        catch
        {
            // Continuar defensivamente
        }
    }

    /// <summary>
    /// Obtiene la ruta del directorio de backups predeterminado configurado en la instancia de SQL Server.
    /// </summary>
    private async Task<string?> ObtenerRutaBackupPredeterminadaServidorAsync()
    {
        try
        {
            using var comando = _contexto.Database.GetDbConnection().CreateCommand();
            comando.CommandText = "SELECT CAST(SERVERPROPERTY('InstanceDefaultBackupPath') AS NVARCHAR(500));";

            if (comando.Connection?.State != ConnectionState.Open)
            {
                await _contexto.Database.OpenConnectionAsync();
            }

            var valor = await comando.ExecuteScalarAsync();
            return valor?.ToString();
        }
        catch
        {
            return null;
        }
    }
}
