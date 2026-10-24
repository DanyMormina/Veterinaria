-- ==============================================================================
-- SCRIPT DDL: BASE DE DATOS VETERINARIA (DER v2 - PKs como 'Id' y BIGINT)
-- Motor: Microsoft SQL Server 2019+ / 2022 / Azure SQL / Docker / LocalDB
-- Todas las Claves Primarias nombradas de forma estándar como 'Id' (BIGINT)
-- Relación jerárquica: Mascota -> Raza (IdRaza) -> Especie (IdEspecie)
-- ==============================================================================

USE master;
GO

-- 1. Recreación Limpia de la Base de Datos si ya existe
IF EXISTS (SELECT name FROM sys.databases WHERE name = N'VeterinariaDb')
BEGIN
    ALTER DATABASE VeterinariaDb SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
    DROP DATABASE VeterinariaDb;
END
GO

CREATE DATABASE VeterinariaDb;
GO

USE VeterinariaDb;
GO

-- ------------------------------------------------------------------------------
-- 2. TABLAS MAESTRAS Y CATALOGOS (Satélites y Entidades Base)
-- ------------------------------------------------------------------------------

-- Tabla: TipoUsuario
CREATE TABLE dbo.TipoUsuario (
    Id BIGINT IDENTITY(1,1) NOT NULL,
    Nombre NVARCHAR(50) NOT NULL,
    Activo BIT NOT NULL CONSTRAINT DF_TipoUsuario_Activo DEFAULT (1),
    CONSTRAINT PK_TipoUsuario PRIMARY KEY CLUSTERED (Id ASC)
);
GO

-- Tabla: Usuario
CREATE TABLE dbo.Usuario (
    Id BIGINT IDENTITY(1,1) NOT NULL,
    IdTipoUsuario BIGINT NOT NULL,
    NombreUsuario NVARCHAR(50) NOT NULL,
    HashContrasena NVARCHAR(255) NOT NULL,
    Nombre NVARCHAR(100) NOT NULL,
    Apellido NVARCHAR(100) NOT NULL,
    DNI NVARCHAR(20) NOT NULL,
    Direccion NVARCHAR(200) NULL,
    Telefono NVARCHAR(30) NULL,
    CorreoElectronico NVARCHAR(100) NULL,
    FechaNacimiento DATE NULL,
    Sexo NVARCHAR(10) NULL,
    Matricula NVARCHAR(50) NULL,
    PuedeRestaurarBD BIT NOT NULL CONSTRAINT DF_Usuario_PuedeRestaurarBD DEFAULT (0),
    Activo BIT NOT NULL CONSTRAINT DF_Usuario_Activo DEFAULT (1),
    CONSTRAINT PK_Usuario PRIMARY KEY CLUSTERED (Id ASC),
    CONSTRAINT FK_Usuario_TipoUsuario FOREIGN KEY (IdTipoUsuario) REFERENCES dbo.TipoUsuario (Id),
    CONSTRAINT UQ_Usuario_NombreUsuario UNIQUE (NombreUsuario)
);
GO

-- Tabla: Propietario
CREATE TABLE dbo.Propietario (
    Id BIGINT IDENTITY(1,1) NOT NULL,
    DNI NVARCHAR(20) NOT NULL,
    Nombre NVARCHAR(100) NOT NULL,
    Apellido NVARCHAR(100) NOT NULL,
    Telefono NVARCHAR(30) NULL,
    CorreoElectronico NVARCHAR(100) NULL,
    Direccion NVARCHAR(200) NULL,
    Activo BIT NOT NULL CONSTRAINT DF_Propietario_Activo DEFAULT (1),
    CONSTRAINT PK_Propietario PRIMARY KEY CLUSTERED (Id ASC),
    CONSTRAINT UQ_Propietario_DNI UNIQUE (DNI)
);
GO

-- Tabla: Especie
CREATE TABLE dbo.Especie (
    Id BIGINT IDENTITY(1,1) NOT NULL,
    Nombre NVARCHAR(50) NOT NULL,
    Activo BIT NOT NULL CONSTRAINT DF_Especie_Activo DEFAULT (1),
    CONSTRAINT PK_Especie PRIMARY KEY CLUSTERED (Id ASC)
);
GO

-- Tabla: Raza (vinculada a Especie)
CREATE TABLE dbo.Raza (
    Id BIGINT IDENTITY(1,1) NOT NULL,
    IdEspecie BIGINT NOT NULL,
    Nombre NVARCHAR(80) NOT NULL,
    Activo BIT NOT NULL CONSTRAINT DF_Raza_Activo DEFAULT (1),
    CONSTRAINT PK_Raza PRIMARY KEY CLUSTERED (Id ASC),
    CONSTRAINT FK_Raza_Especie FOREIGN KEY (IdEspecie) REFERENCES dbo.Especie (Id)
);
GO

-- Tabla: Mascota (vinculada a Raza y Propietario)
CREATE TABLE dbo.Mascota (
    Id BIGINT IDENTITY(1,1) NOT NULL,
    IdPropietario BIGINT NOT NULL,
    IdRaza BIGINT NOT NULL,
    Nombre NVARCHAR(80) NOT NULL,
    Sexo NVARCHAR(10) NOT NULL,
    FechaNacimiento DATE NULL,
    Color NVARCHAR(50) NULL,
    Activo BIT NOT NULL CONSTRAINT DF_Mascota_Activo DEFAULT (1),
    CONSTRAINT PK_Mascota PRIMARY KEY CLUSTERED (Id ASC),
    CONSTRAINT FK_Mascota_Propietario FOREIGN KEY (IdPropietario) REFERENCES dbo.Propietario (Id),
    CONSTRAINT FK_Mascota_Raza FOREIGN KEY (IdRaza) REFERENCES dbo.Raza (Id)
);
GO

-- Tabla: Vacuna
CREATE TABLE dbo.Vacuna (
    Id BIGINT IDENTITY(1,1) NOT NULL,
    IdEspecie BIGINT NOT NULL,
    Nombre NVARCHAR(100) NOT NULL,
    PeriodoMesesRecomendado INT NOT NULL CONSTRAINT DF_Vacuna_Periodo DEFAULT (12),
    Precio DECIMAL(18,2) NOT NULL CONSTRAINT DF_Vacuna_Precio DEFAULT (0),
    Activo BIT NOT NULL CONSTRAINT DF_Vacuna_Activo DEFAULT (1),
    CONSTRAINT PK_Vacuna PRIMARY KEY CLUSTERED (Id ASC),
    CONSTRAINT FK_Vacuna_Especie FOREIGN KEY (IdEspecie) REFERENCES dbo.Especie (Id)
);
GO

-- Tabla: MetodoPago
CREATE TABLE dbo.MetodoPago (
    Id BIGINT IDENTITY(1,1) NOT NULL,
    Nombre NVARCHAR(50) NOT NULL,
    Activo BIT NOT NULL CONSTRAINT DF_MetodoPago_Activo DEFAULT (1),
    CONSTRAINT PK_MetodoPago PRIMARY KEY CLUSTERED (Id ASC)
);
GO

-- Tabla: Tratamiento (Catálogo Maestro)
CREATE TABLE dbo.Tratamiento (
    Id BIGINT IDENTITY(1,1) NOT NULL,
    TipoTratamiento NVARCHAR(50) NOT NULL,
    Descripcion NVARCHAR(MAX) NOT NULL,
    Dosis NVARCHAR(100) NULL,
    Precio DECIMAL(18,2) NOT NULL CONSTRAINT DF_Tratamiento_Precio DEFAULT (0),
    Activo BIT NOT NULL CONSTRAINT DF_Tratamiento_Activo DEFAULT (1),
    CONSTRAINT PK_Tratamiento PRIMARY KEY CLUSTERED (Id ASC)
);
GO

-- Tabla: PrecioConsulta (precio fijo de la consulta por especie; se modifica solo en la base)
CREATE TABLE dbo.PrecioConsulta (
    Id BIGINT IDENTITY(1,1) NOT NULL,
    IdEspecie BIGINT NOT NULL,
    Precio DECIMAL(18,2) NOT NULL,
    CONSTRAINT PK_PrecioConsulta PRIMARY KEY CLUSTERED (Id ASC),
    CONSTRAINT FK_PrecioConsulta_Especie FOREIGN KEY (IdEspecie) REFERENCES dbo.Especie (Id)
);
CREATE UNIQUE NONCLUSTERED INDEX IX_PrecioConsulta_IdEspecie ON dbo.PrecioConsulta (IdEspecie);
GO

-- ------------------------------------------------------------------------------
-- 3. TABLAS TRANSACCIONALES CLÍNICAS Y PAGOS
-- ------------------------------------------------------------------------------

-- Tabla: Consulta
CREATE TABLE dbo.Consulta (
    Id BIGINT IDENTITY(1,1) NOT NULL,
    IdUsuario BIGINT NOT NULL,
    IdMascota BIGINT NOT NULL,
    FechaHora DATETIME2(0) NOT NULL,
    Motivo NVARCHAR(250) NULL,
    PesoKg DECIMAL(6,2) NULL,
    Temperatura DECIMAL(4,2) NULL,
    Diagnostico NVARCHAR(MAX) NOT NULL,
    Observaciones NVARCHAR(MAX) NULL,
    Importe DECIMAL(18,2) NOT NULL CONSTRAINT DF_Consulta_Importe DEFAULT (0),
    Activo BIT NOT NULL CONSTRAINT DF_Consulta_Activo DEFAULT (1),
    CONSTRAINT PK_Consulta PRIMARY KEY CLUSTERED (Id ASC),
    CONSTRAINT FK_Consulta_Usuario FOREIGN KEY (IdUsuario) REFERENCES dbo.Usuario (Id),
    CONSTRAINT FK_Consulta_Mascota FOREIGN KEY (IdMascota) REFERENCES dbo.Mascota (Id)
);
GO

-- Tabla: DetalleConsulta
CREATE TABLE dbo.DetalleConsulta (
    Id BIGINT IDENTITY(1,1) NOT NULL,
    IdConsulta BIGINT NOT NULL,
    IdTratamiento BIGINT NOT NULL,
    Cantidad INT NOT NULL CONSTRAINT DF_DetalleConsulta_Cantidad DEFAULT (1),
    PrecioUnitario DECIMAL(18,2) NOT NULL,
    Subtotal DECIMAL(18,2) NOT NULL,
    Indicaciones NVARCHAR(MAX) NULL,
    Activo BIT NOT NULL CONSTRAINT DF_DetalleConsulta_Activo DEFAULT (1),
    CONSTRAINT PK_DetalleConsulta PRIMARY KEY CLUSTERED (Id ASC),
    CONSTRAINT FK_DetalleConsulta_Consulta FOREIGN KEY (IdConsulta) REFERENCES dbo.Consulta (Id) ON DELETE CASCADE,
    CONSTRAINT FK_DetalleConsulta_Tratamiento FOREIGN KEY (IdTratamiento) REFERENCES dbo.Tratamiento (Id)
);
GO

-- Tabla: AplicacionVacuna
CREATE TABLE dbo.AplicacionVacuna (
    Id BIGINT IDENTITY(1,1) NOT NULL,
    IdConsulta BIGINT NOT NULL,
    IdVacuna BIGINT NOT NULL,
    FechaAplicacion DATE NOT NULL,
    ProximaDosis DATE NULL,
    PrecioUnitario DECIMAL(18,2) NULL,
    Observaciones NVARCHAR(250) NULL,
    CONSTRAINT PK_AplicacionVacuna PRIMARY KEY CLUSTERED (Id ASC),
    CONSTRAINT FK_AplicacionVacuna_Consulta FOREIGN KEY (IdConsulta) REFERENCES dbo.Consulta (Id) ON DELETE CASCADE,
    CONSTRAINT FK_AplicacionVacuna_Vacuna FOREIGN KEY (IdVacuna) REFERENCES dbo.Vacuna (Id)
);
GO

-- Tabla: Pago
CREATE TABLE dbo.Pago (
    Id BIGINT IDENTITY(1,1) NOT NULL,
    IdConsulta BIGINT NOT NULL,
    IdMetodoPago BIGINT NOT NULL,
    Fecha DATETIME2(0) NOT NULL,
    Importe DECIMAL(18,2) NOT NULL,
    Estado NVARCHAR(30) NOT NULL CONSTRAINT DF_Pago_Estado DEFAULT ('Completado'),
    Cuotas INT NULL,
    Activo BIT NOT NULL CONSTRAINT DF_Pago_Activo DEFAULT (1),
    CONSTRAINT PK_Pago PRIMARY KEY CLUSTERED (Id ASC),
    CONSTRAINT FK_Pago_Consulta FOREIGN KEY (IdConsulta) REFERENCES dbo.Consulta (Id),
    CONSTRAINT FK_Pago_MetodoPago FOREIGN KEY (IdMetodoPago) REFERENCES dbo.MetodoPago (Id)
);
GO

-- ------------------------------------------------------------------------------
-- 4. INDICES PARA OPTIMIZACION DE CONSULTAS
-- ------------------------------------------------------------------------------
CREATE NONCLUSTERED INDEX IX_Usuario_IdTipoUsuario ON dbo.Usuario(IdTipoUsuario);
CREATE NONCLUSTERED INDEX IX_Mascota_IdPropietario ON dbo.Mascota(IdPropietario);
CREATE NONCLUSTERED INDEX IX_Mascota_IdRaza ON dbo.Mascota(IdRaza);
CREATE NONCLUSTERED INDEX IX_Raza_IdEspecie ON dbo.Raza(IdEspecie);
CREATE NONCLUSTERED INDEX IX_Vacuna_IdEspecie ON dbo.Vacuna(IdEspecie);
CREATE NONCLUSTERED INDEX IX_Consulta_IdUsuario ON dbo.Consulta(IdUsuario);
CREATE NONCLUSTERED INDEX IX_Consulta_IdMascota ON dbo.Consulta(IdMascota);
CREATE NONCLUSTERED INDEX IX_Consulta_FechaHora ON dbo.Consulta(FechaHora);
CREATE NONCLUSTERED INDEX IX_DetalleConsulta_IdConsulta ON dbo.DetalleConsulta(IdConsulta);
CREATE NONCLUSTERED INDEX IX_DetalleConsulta_IdTratamiento ON dbo.DetalleConsulta(IdTratamiento);
CREATE NONCLUSTERED INDEX IX_AplicacionVacuna_IdConsulta ON dbo.AplicacionVacuna(IdConsulta);
CREATE NONCLUSTERED INDEX IX_AplicacionVacuna_IdVacuna ON dbo.AplicacionVacuna(IdVacuna);
CREATE NONCLUSTERED INDEX IX_Pago_IdConsulta ON dbo.Pago(IdConsulta);
CREATE NONCLUSTERED INDEX IX_Pago_IdMetodoPago ON dbo.Pago(IdMetodoPago);
GO

-- ------------------------------------------------------------------------------
-- 5. SEMBRADO DE DATOS INICIALES (SEEDING)
-- ------------------------------------------------------------------------------
SET IDENTITY_INSERT dbo.TipoUsuario ON;
INSERT INTO dbo.TipoUsuario (Id, Nombre, Activo) VALUES
(1, 'Administrador', 1),
(2, 'Veterinario', 1),
(3, 'Secretario', 1),
(4, 'Gerente', 1); -- Sin vistas: solo autoriza backups y restauraciones (doble autorización)
SET IDENTITY_INSERT dbo.TipoUsuario OFF;
GO

SET IDENTITY_INSERT dbo.Especie ON;
INSERT INTO dbo.Especie (Id, Nombre, Activo) VALUES
(1, 'Canino', 1),
(2, 'Felino', 1),
(3, 'Ave', 1),
(4, 'Roedor', 1);
SET IDENTITY_INSERT dbo.Especie OFF;
GO

INSERT INTO dbo.PrecioConsulta (IdEspecie, Precio) VALUES
(1, 10000.00),
(2, 10000.00),
(3, 10000.00),
(4, 10000.00);
GO

SET IDENTITY_INSERT dbo.Raza ON;
INSERT INTO dbo.Raza (Id, IdEspecie, Nombre, Activo) VALUES
(1, 1, 'Labrador Retriever', 1),
(2, 1, 'Pastor Alemán', 1),
(3, 1, 'Caniche', 1),
(4, 1, 'Mestizo Canino', 1),
(5, 2, 'Siamés', 1),
(6, 2, 'Persa', 1),
(7, 2, 'Mestizo Felino', 1);
SET IDENTITY_INSERT dbo.Raza OFF;
GO

SET IDENTITY_INSERT dbo.MetodoPago ON;
INSERT INTO dbo.MetodoPago (Id, Nombre, Activo) VALUES
(1, 'Efectivo', 1),
(2, 'Tarjeta de Débito', 1),
(3, 'Tarjeta de Crédito', 1),
(4, 'Transferencia Bancaria', 1);
SET IDENTITY_INSERT dbo.MetodoPago OFF;
GO

SET IDENTITY_INSERT dbo.Usuario ON;
INSERT INTO dbo.Usuario (Id, IdTipoUsuario, NombreUsuario, HashContrasena, Nombre, Apellido, DNI, Matricula, PuedeRestaurarBD, Activo)
VALUES (1, 1, 'admin', '$2a$11$q9hM1K9oA5F8VzE9hX2Hke0bT.G7j5Z8U1j.H7y1N8r9o1V7e8i9a', 'Administrador', 'Sistema', '12345678', 'ADM-001', 1, 1);
SET IDENTITY_INSERT dbo.Usuario OFF;
GO
-- El usuario 'gerente' (TipoUsuario Gerente) lo crea la aplicación al iniciar (InicializadorDatos),
-- con su contraseña cifrada con BCrypt.

-- ------------------------------------------------------------------------------
-- 7. TABLA DE SISTEMA: BACKUPS Y COPIAS DE SEGURIDAD
-- ------------------------------------------------------------------------------
IF OBJECT_ID('dbo.[Backup]', 'U') IS NULL
BEGIN
    CREATE TABLE dbo.[Backup] (
        IdBackup INT IDENTITY(1,1) NOT NULL,
        IdUsuario BIGINT NOT NULL,
        NombreArchivo NVARCHAR(260) NOT NULL,
        FechaCreacion DATETIME2(0) NOT NULL CONSTRAINT DF_Backup_FechaCreacion DEFAULT (SYSDATETIME()),
        RutaDestino NVARCHAR(500) NOT NULL,
        Activo BIT NOT NULL CONSTRAINT DF_Backup_Activo DEFAULT (1),
        CONSTRAINT PK_Backup PRIMARY KEY CLUSTERED (IdBackup ASC),
        CONSTRAINT FK_Backup_Usuario FOREIGN KEY (IdUsuario) REFERENCES dbo.Usuario (Id)
    );

    CREATE NONCLUSTERED INDEX IX_Backup_IdUsuario ON dbo.[Backup] (IdUsuario);
    CREATE NONCLUSTERED INDEX IX_Backup_FechaCreacion ON dbo.[Backup] (FechaCreacion);
END
GO

-- ------------------------------------------------------------------------------
-- 8. INDICES UNICOS (en catálogos solo entre registros activos)
-- ------------------------------------------------------------------------------
CREATE UNIQUE NONCLUSTERED INDEX UQ_Usuario_DNI ON dbo.Usuario (DNI);
CREATE UNIQUE NONCLUSTERED INDEX UQ_Especie_Nombre ON dbo.Especie (Nombre) WHERE Activo = 1;
CREATE UNIQUE NONCLUSTERED INDEX UQ_Raza_Especie_Nombre ON dbo.Raza (IdEspecie, Nombre) WHERE Activo = 1;
CREATE UNIQUE NONCLUSTERED INDEX UQ_Vacuna_Especie_Nombre ON dbo.Vacuna (IdEspecie, Nombre) WHERE Activo = 1;
GO

-- ------------------------------------------------------------------------------
-- 9. PROCEDIMIENTOS ALMACENADOS DE REPORTES
--    (la aplicación también los crea/actualiza al iniciar: ObjetosBaseDatos.cs)
-- ------------------------------------------------------------------------------
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
GO

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
GO

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
GO

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
GO

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
GO

-- Ejemplos de uso:
-- EXEC dbo.sp_ReporteConsultasClinicas @FechaDesde = '2026-10-01', @FechaHasta = '2026-10-31';
-- EXEC dbo.sp_ReporteCobros @FechaDesde = '2026-01-01', @FechaHasta = '2026-12-31', @Estado = 'Completado';
-- EXEC dbo.sp_ReporteCensoMascotas @IdEspecie = 1;
