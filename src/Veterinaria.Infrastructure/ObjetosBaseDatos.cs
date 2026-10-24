using Microsoft.EntityFrameworkCore;

namespace Veterinaria.Infrastructure;

/// <summary>
/// Procedimientos almacenados de reportes e índices únicos.
/// Se aplican en cada inicio de forma idempotente (CREATE OR ALTER / IF NOT EXISTS),
/// porque EnsureCreated no los genera ni altera bases existentes.
/// </summary>
public static class ObjetosBaseDatos
{
    public static async Task AsegurarProcedimientosReportesAsync(ContextoVeterinaria context)
    {
        foreach (var procedimiento in Procedimientos)
            await context.Database.ExecuteSqlRawAsync(procedimiento);
    }

    public static async Task AsegurarIndicesUnicosAsync(ContextoVeterinaria context)
    {
        // Solo se crean si los datos actuales no tienen repetidos; en catálogos aplican a registros activos
        await context.Database.ExecuteSqlRawAsync("""
            IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'UQ_Usuario_DNI' AND object_id = OBJECT_ID('dbo.Usuario'))
               AND NOT EXISTS (SELECT DNI FROM dbo.Usuario GROUP BY DNI HAVING COUNT(*) > 1)
                CREATE UNIQUE NONCLUSTERED INDEX UQ_Usuario_DNI ON dbo.Usuario (DNI);

            IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'UQ_Especie_Nombre' AND object_id = OBJECT_ID('dbo.Especie'))
               AND NOT EXISTS (SELECT Nombre FROM dbo.Especie WHERE Activo = 1 GROUP BY Nombre HAVING COUNT(*) > 1)
                CREATE UNIQUE NONCLUSTERED INDEX UQ_Especie_Nombre ON dbo.Especie (Nombre) WHERE Activo = 1;

            IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'UQ_Raza_Especie_Nombre' AND object_id = OBJECT_ID('dbo.Raza'))
               AND NOT EXISTS (SELECT IdEspecie, Nombre FROM dbo.Raza WHERE Activo = 1 GROUP BY IdEspecie, Nombre HAVING COUNT(*) > 1)
                CREATE UNIQUE NONCLUSTERED INDEX UQ_Raza_Especie_Nombre ON dbo.Raza (IdEspecie, Nombre) WHERE Activo = 1;

            IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'UQ_Vacuna_Especie_Nombre' AND object_id = OBJECT_ID('dbo.Vacuna'))
               AND NOT EXISTS (SELECT IdEspecie, Nombre FROM dbo.Vacuna WHERE Activo = 1 GROUP BY IdEspecie, Nombre HAVING COUNT(*) > 1)
                CREATE UNIQUE NONCLUSTERED INDEX UQ_Vacuna_Especie_Nombre ON dbo.Vacuna (IdEspecie, Nombre) WHERE Activo = 1;
            """);
    }

    private static readonly string[] Procedimientos =
    [
        """
        CREATE OR ALTER PROCEDURE dbo.sp_ReporteConsultasClinicas
            @FechaDesde DATE, @FechaHasta DATE, @IdVeterinario BIGINT = NULL, @IdEspecie BIGINT = NULL
        AS
        BEGIN
            SET NOCOUNT ON;
            SELECT c.FechaHora,
                   m.Nombre AS Mascota,
                   e.Nombre AS Especie,
                   r.Nombre AS Raza,
                   p.Nombre + ' ' + p.Apellido AS Propietario,
                   u.Nombre + ' ' + u.Apellido AS Veterinario,
                   ISNULL(c.Motivo, '') AS Motivo,
                   c.Diagnostico
            FROM dbo.Consulta c
            JOIN dbo.Mascota m ON m.Id = c.IdMascota AND m.Activo = 1
            JOIN dbo.Raza r ON r.Id = m.IdRaza AND r.Activo = 1
            JOIN dbo.Especie e ON e.Id = r.IdEspecie AND e.Activo = 1
            JOIN dbo.Propietario p ON p.Id = m.IdPropietario AND p.Activo = 1
            JOIN dbo.Usuario u ON u.Id = c.IdUsuario AND u.Activo = 1
            WHERE c.Activo = 1
              AND c.FechaHora >= @FechaDesde AND c.FechaHora < DATEADD(DAY, 1, @FechaHasta)
              AND (@IdVeterinario IS NULL OR c.IdUsuario = @IdVeterinario)
              AND (@IdEspecie IS NULL OR r.IdEspecie = @IdEspecie)
            ORDER BY c.FechaHora DESC;
        END
        """,
        """
        CREATE OR ALTER PROCEDURE dbo.sp_ReporteConsultasDetalladas
            @FechaDesde DATE, @FechaHasta DATE, @IdVeterinario BIGINT = NULL, @IdEspecie BIGINT = NULL
        AS
        BEGIN
            SET NOCOUNT ON;
            SELECT c.Id AS IdConsulta,
                   c.FechaHora,
                   m.Nombre AS Mascota,
                   e.Nombre AS Especie,
                   p.Nombre + ' ' + p.Apellido AS Propietario,
                   u.Nombre + ' ' + u.Apellido AS Veterinario,
                   c.Diagnostico,
                   ISNULL(t.Cantidad, 0) AS CantidadTratamientos,
                   ISNULL(v.Cantidad, 0) AS CantidadVacunas,
                   c.Importe AS ImporteConsulta,
                   ISNULL(t.Importe, 0) AS ImporteTratamientos,
                   ISNULL(v.Importe, 0) AS ImporteVacunas
            FROM dbo.Consulta c
            JOIN dbo.Mascota m ON m.Id = c.IdMascota AND m.Activo = 1
            JOIN dbo.Raza r ON r.Id = m.IdRaza AND r.Activo = 1
            JOIN dbo.Especie e ON e.Id = r.IdEspecie AND e.Activo = 1
            JOIN dbo.Propietario p ON p.Id = m.IdPropietario AND p.Activo = 1
            JOIN dbo.Usuario u ON u.Id = c.IdUsuario AND u.Activo = 1
            OUTER APPLY (SELECT SUM(d.Cantidad) AS Cantidad, SUM(d.Subtotal) AS Importe
                         FROM dbo.DetalleConsulta d WHERE d.IdConsulta = c.Id AND d.Activo = 1) t
            OUTER APPLY (SELECT COUNT(*) AS Cantidad, SUM(a.PrecioUnitario) AS Importe
                         FROM dbo.AplicacionVacuna a WHERE a.IdConsulta = c.Id) v
            WHERE c.Activo = 1
              AND c.FechaHora >= @FechaDesde AND c.FechaHora < DATEADD(DAY, 1, @FechaHasta)
              AND (@IdVeterinario IS NULL OR c.IdUsuario = @IdVeterinario)
              AND (@IdEspecie IS NULL OR r.IdEspecie = @IdEspecie)
            ORDER BY c.FechaHora DESC;
        END
        """,
        """
        CREATE OR ALTER PROCEDURE dbo.sp_ReporteCobros
            @FechaDesde DATE, @FechaHasta DATE, @Estado NVARCHAR(30) = NULL,
            @IdVeterinario BIGINT = NULL, @IdEspecie BIGINT = NULL
        AS
        BEGIN
            SET NOCOUNT ON;
            SELECT pg.Fecha,
                   pg.IdConsulta,
                   m.Nombre AS Mascota,
                   p.Nombre + ' ' + p.Apellido AS Propietario,
                   ISNULL(mp.Nombre, '') AS MetodoPago,
                   pg.Cuotas,
                   pg.Importe,
                   pg.Estado
            FROM dbo.Pago pg
            JOIN dbo.Consulta c ON c.Id = pg.IdConsulta
            JOIN dbo.Mascota m ON m.Id = c.IdMascota
            JOIN dbo.Raza r ON r.Id = m.IdRaza
            JOIN dbo.Propietario p ON p.Id = m.IdPropietario
            LEFT JOIN dbo.MetodoPago mp ON mp.Id = pg.IdMetodoPago
            WHERE pg.Fecha >= @FechaDesde AND pg.Fecha < DATEADD(DAY, 1, @FechaHasta)
              AND (@Estado IS NULL OR pg.Estado = @Estado)
              AND (@IdVeterinario IS NULL OR c.IdUsuario = @IdVeterinario)
              AND (@IdEspecie IS NULL OR r.IdEspecie = @IdEspecie)
            ORDER BY pg.Fecha DESC, pg.Id DESC;
        END
        """,
        """
        CREATE OR ALTER PROCEDURE dbo.sp_ReporteVacunasTratamientos
            @FechaDesde DATE, @FechaHasta DATE, @IdVeterinario BIGINT = NULL, @IdEspecie BIGINT = NULL
        AS
        BEGIN
            SET NOCOUNT ON;
            SELECT Fecha, Mascota, Especie, TratamientoVacuna, Tipo, Dosis, Veterinario
            FROM (
                SELECT c.FechaHora AS Fecha,
                       m.Nombre AS Mascota,
                       e.Nombre AS Especie,
                       CAST(t.Descripcion AS NVARCHAR(MAX)) AS TratamientoVacuna,
                       CAST(CASE WHEN LTRIM(RTRIM(ISNULL(t.TipoTratamiento, ''))) = '' THEN N'Tratamiento' ELSE t.TipoTratamiento END AS NVARCHAR(100)) AS Tipo,
                       CAST(CASE WHEN LTRIM(RTRIM(ISNULL(t.Dosis, ''))) = '' THEN N'-' ELSE t.Dosis END AS NVARCHAR(100)) AS Dosis,
                       u.Nombre + ' ' + u.Apellido AS Veterinario
                FROM dbo.DetalleConsulta d
                JOIN dbo.Consulta c ON c.Id = d.IdConsulta AND c.Activo = 1
                JOIN dbo.Tratamiento t ON t.Id = d.IdTratamiento AND t.Activo = 1
                JOIN dbo.Mascota m ON m.Id = c.IdMascota AND m.Activo = 1
                JOIN dbo.Raza r ON r.Id = m.IdRaza AND r.Activo = 1
                JOIN dbo.Especie e ON e.Id = r.IdEspecie AND e.Activo = 1
                JOIN dbo.Usuario u ON u.Id = c.IdUsuario AND u.Activo = 1
                WHERE d.Activo = 1
                  AND c.FechaHora >= @FechaDesde AND c.FechaHora < DATEADD(DAY, 1, @FechaHasta)
                  AND (@IdVeterinario IS NULL OR c.IdUsuario = @IdVeterinario)
                  AND (@IdEspecie IS NULL OR r.IdEspecie = @IdEspecie)

                UNION ALL

                SELECT CAST(a.FechaAplicacion AS DATETIME2(0)),
                       m.Nombre,
                       e.Nombre,
                       CAST(va.Nombre AS NVARCHAR(MAX)),
                       CAST(N'Vacunación' AS NVARCHAR(100)),
                       CAST(CASE WHEN a.ProximaDosis IS NOT NULL
                                 THEN N'Próx. refuerzo: ' + CONVERT(NVARCHAR(10), a.ProximaDosis, 103)
                                 ELSE N'Dosis completa' END AS NVARCHAR(100)),
                       u.Nombre + ' ' + u.Apellido
                FROM dbo.AplicacionVacuna a
                JOIN dbo.Consulta c ON c.Id = a.IdConsulta AND c.Activo = 1
                JOIN dbo.Vacuna va ON va.Id = a.IdVacuna AND va.Activo = 1
                JOIN dbo.Mascota m ON m.Id = c.IdMascota AND m.Activo = 1
                JOIN dbo.Raza r ON r.Id = m.IdRaza AND r.Activo = 1
                JOIN dbo.Especie e ON e.Id = r.IdEspecie AND e.Activo = 1
                JOIN dbo.Usuario u ON u.Id = c.IdUsuario AND u.Activo = 1
                WHERE a.FechaAplicacion >= @FechaDesde AND a.FechaAplicacion < DATEADD(DAY, 1, @FechaHasta)
                  AND (@IdVeterinario IS NULL OR c.IdUsuario = @IdVeterinario)
                  AND (@IdEspecie IS NULL OR r.IdEspecie = @IdEspecie)
            ) x
            ORDER BY Fecha DESC;
        END
        """,
        """
        CREATE OR ALTER PROCEDURE dbo.sp_ReporteCensoMascotas
            @IdEspecie BIGINT = NULL
        AS
        BEGIN
            SET NOCOUNT ON;
            SELECT m.Id,
                   m.Nombre,
                   e.Nombre AS Especie,
                   r.Nombre AS Raza,
                   m.Sexo,
                   CAST(m.FechaNacimiento AS DATETIME2(0)) AS FechaNacimiento,
                   CASE WHEN LTRIM(RTRIM(ISNULL(m.Color, ''))) = '' THEN '-' ELSE m.Color END AS Color,
                   p.Nombre + ' ' + p.Apellido AS Propietario
            FROM dbo.Mascota m
            JOIN dbo.Raza r ON r.Id = m.IdRaza AND r.Activo = 1
            JOIN dbo.Especie e ON e.Id = r.IdEspecie AND e.Activo = 1
            JOIN dbo.Propietario p ON p.Id = m.IdPropietario AND p.Activo = 1
            WHERE m.Activo = 1
              AND (@IdEspecie IS NULL OR r.IdEspecie = @IdEspecie)
            ORDER BY m.Nombre;
        END
        """
    ];
}
