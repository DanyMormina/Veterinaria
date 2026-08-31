-- ==============================================================================
-- SCRIPT DDL: BASE DE DATOS VETERINARIA (DER v2)
-- Motor: Microsoft SQL Server 2019+ / 2022 / Azure SQL / Docker / LocalDB
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
-- 2. TABLAS MAESTRAS Y CATALOGOS (Sustituto de Roles y Satélites)
-- ------------------------------------------------------------------------------

-- Tabla: TipoUsuario
CREATE TABLE dbo.TipoUsuario (
    IdTipoUsuario INT IDENTITY(1,1) NOT NULL,
    Nombre NVARCHAR(50) NOT NULL,
    Activo BIT NOT NULL CONSTRAINT DF_TipoUsuario_Activo DEFAULT (1),
    CONSTRAINT PK_TipoUsuario PRIMARY KEY CLUSTERED (IdTipoUsuario ASC)
);
GO

-- Tabla: Usuario
CREATE TABLE dbo.Usuario (
    IdUsuario INT IDENTITY(1,1) NOT NULL,
    IdTipoUsuario INT NOT NULL,
    Username NVARCHAR(50) NOT NULL,
    PasswordHash NVARCHAR(255) NOT NULL,
    Nombre NVARCHAR(100) NOT NULL,
    Apellido NVARCHAR(100) NOT NULL,
    DNI NVARCHAR(20) NOT NULL,
    Matricula NVARCHAR(50) NULL,
    Activo BIT NOT NULL CONSTRAINT DF_Usuario_Activo DEFAULT (1),
    CONSTRAINT PK_Usuario PRIMARY KEY CLUSTERED (IdUsuario ASC),
    CONSTRAINT FK_Usuario_TipoUsuario FOREIGN KEY (IdTipoUsuario) REFERENCES dbo.TipoUsuario (IdTipoUsuario),
    CONSTRAINT UQ_Usuario_Username UNIQUE (Username)
);
GO

-- Tabla: Sesion
CREATE TABLE dbo.Sesion (
    IdSesion INT IDENTITY(1,1) NOT NULL,
    IdUsuario INT NOT NULL,
    FechaInicio DATETIME2(0) NOT NULL,
    FechaFin DATETIME2(0) NULL,
    IpAddress NVARCHAR(50) NULL,
    Activo BIT NOT NULL CONSTRAINT DF_Sesion_Activo DEFAULT (1),
    CONSTRAINT PK_Sesion PRIMARY KEY CLUSTERED (IdSesion ASC),
    CONSTRAINT FK_Sesion_Usuario FOREIGN KEY (IdUsuario) REFERENCES dbo.Usuario (IdUsuario)
);
GO

-- Tabla: Auditoria
CREATE TABLE dbo.Auditoria (
    IdAuditoria INT IDENTITY(1,1) NOT NULL,
    IdUsuario INT NOT NULL,
    FechaHora DATETIME2(0) NOT NULL,
    Accion NVARCHAR(100) NOT NULL,
    TablaAfectada NVARCHAR(100) NOT NULL,
    Detalle NVARCHAR(2000) NULL,
    Activo BIT NOT NULL CONSTRAINT DF_Auditoria_Activo DEFAULT (1),
    CONSTRAINT PK_Auditoria PRIMARY KEY CLUSTERED (IdAuditoria ASC),
    CONSTRAINT FK_Auditoria_Usuario FOREIGN KEY (IdUsuario) REFERENCES dbo.Usuario (IdUsuario)
);
GO

-- Tabla: Propietario
CREATE TABLE dbo.Propietario (
    IdPropietario INT IDENTITY(1,1) NOT NULL,
    DNI NVARCHAR(20) NOT NULL,
    Nombre NVARCHAR(100) NOT NULL,
    Apellido NVARCHAR(100) NOT NULL,
    Telefono NVARCHAR(30) NULL,
    Email NVARCHAR(100) NULL,
    Direccion NVARCHAR(200) NULL,
    Activo BIT NOT NULL CONSTRAINT DF_Propietario_Activo DEFAULT (1),
    CONSTRAINT PK_Propietario PRIMARY KEY CLUSTERED (IdPropietario ASC),
    CONSTRAINT UQ_Propietario_DNI UNIQUE (DNI)
);
GO

-- Tabla: Especie
CREATE TABLE dbo.Especie (
    IdEspecie INT IDENTITY(1,1) NOT NULL,
    Nombre NVARCHAR(50) NOT NULL,
    Activo BIT NOT NULL CONSTRAINT DF_Especie_Activo DEFAULT (1),
    CONSTRAINT PK_Especie PRIMARY KEY CLUSTERED (IdEspecie ASC)
);
GO

-- Tabla: Raza
CREATE TABLE dbo.Raza (
    IdRaza INT IDENTITY(1,1) NOT NULL,
    IdEspecie INT NOT NULL,
    Nombre NVARCHAR(80) NOT NULL,
    Activo BIT NOT NULL CONSTRAINT DF_Raza_Activo DEFAULT (1),
    CONSTRAINT PK_Raza PRIMARY KEY CLUSTERED (IdRaza ASC),
    CONSTRAINT FK_Raza_Especie FOREIGN KEY (IdEspecie) REFERENCES dbo.Especie (IdEspecie)
);
GO

-- Tabla: Mascota (FK directa a Especie y Propietario)
CREATE TABLE dbo.Mascota (
    IdMascota INT IDENTITY(1,1) NOT NULL,
    IdPropietario INT NOT NULL,
    IdEspecie INT NOT NULL,
    Nombre NVARCHAR(80) NOT NULL,
    Sexo NVARCHAR(10) NOT NULL,
    FechaNacimiento DATE NULL,
    Color NVARCHAR(50) NULL,
    Activo BIT NOT NULL CONSTRAINT DF_Mascota_Activo DEFAULT (1),
    CONSTRAINT PK_Mascota PRIMARY KEY CLUSTERED (IdMascota ASC),
    CONSTRAINT FK_Mascota_Propietario FOREIGN KEY (IdPropietario) REFERENCES dbo.Propietario (IdPropietario),
    CONSTRAINT FK_Mascota_Especie FOREIGN KEY (IdEspecie) REFERENCES dbo.Especie (IdEspecie)
);
GO

-- Tabla: Vacuna
CREATE TABLE dbo.Vacuna (
    IdVacuna INT IDENTITY(1,1) NOT NULL,
    Nombre NVARCHAR(100) NOT NULL,
    PeriodoMesesRecomendado INT NOT NULL CONSTRAINT DF_Vacuna_Periodo DEFAULT (12),
    Activo BIT NOT NULL CONSTRAINT DF_Vacuna_Activo DEFAULT (1),
    CONSTRAINT PK_Vacuna PRIMARY KEY CLUSTERED (IdVacuna ASC)
);
GO

-- Tabla: MetodoPago
CREATE TABLE dbo.MetodoPago (
    IdMetodoPago INT IDENTITY(1,1) NOT NULL,
    Nombre NVARCHAR(50) NOT NULL,
    Activo BIT NOT NULL CONSTRAINT DF_MetodoPago_Activo DEFAULT (1),
    CONSTRAINT PK_MetodoPago PRIMARY KEY CLUSTERED (IdMetodoPago ASC)
);
GO

-- Tabla: Tratamiento (Catálogo Maestro)
CREATE TABLE dbo.Tratamiento (
    IdTratamiento INT IDENTITY(1,1) NOT NULL,
    TipoTratamiento NVARCHAR(50) NOT NULL,
    Descripcion NVARCHAR(MAX) NOT NULL,
    Dosis NVARCHAR(100) NULL,
    Precio DECIMAL(18,2) NOT NULL CONSTRAINT DF_Tratamiento_Precio DEFAULT (0),
    Activo BIT NOT NULL CONSTRAINT DF_Tratamiento_Activo DEFAULT (1),
    CONSTRAINT PK_Tratamiento PRIMARY KEY CLUSTERED (IdTratamiento ASC)
);
GO

-- ------------------------------------------------------------------------------
-- 3. TABLAS TRANSACCIONALES CLÍNICAS Y PAGOS
-- ------------------------------------------------------------------------------

-- Tabla: Consulta
CREATE TABLE dbo.Consulta (
    IdConsulta INT IDENTITY(1,1) NOT NULL,
    IdUsuario INT NOT NULL,
    IdMascota INT NOT NULL,
    FechaHora DATETIME2(0) NOT NULL,
    Motivo NVARCHAR(250) NULL,
    PesoKg DECIMAL(6,2) NULL,
    Temperatura DECIMAL(4,2) NULL,
    Diagnostico NVARCHAR(MAX) NOT NULL,
    Observaciones NVARCHAR(MAX) NULL,
    Activo BIT NOT NULL CONSTRAINT DF_Consulta_Activo DEFAULT (1),
    CONSTRAINT PK_Consulta PRIMARY KEY CLUSTERED (IdConsulta ASC),
    CONSTRAINT FK_Consulta_Usuario FOREIGN KEY (IdUsuario) REFERENCES dbo.Usuario (IdUsuario),
    CONSTRAINT FK_Consulta_Mascota FOREIGN KEY (IdMascota) REFERENCES dbo.Mascota (IdMascota)
);
GO

-- Tabla: DetalleConsulta
CREATE TABLE dbo.DetalleConsulta (
    IdDetalleConsulta INT IDENTITY(1,1) NOT NULL,
    IdConsulta INT NOT NULL,
    IdTratamiento INT NOT NULL,
    Cantidad INT NOT NULL CONSTRAINT DF_DetalleConsulta_Cantidad DEFAULT (1),
    PrecioUnitario DECIMAL(18,2) NOT NULL,
    Subtotal DECIMAL(18,2) NOT NULL,
    Indicaciones NVARCHAR(MAX) NULL,
    Activo BIT NOT NULL CONSTRAINT DF_DetalleConsulta_Activo DEFAULT (1),
    CONSTRAINT PK_DetalleConsulta PRIMARY KEY CLUSTERED (IdDetalleConsulta ASC),
    CONSTRAINT FK_DetalleConsulta_Consulta FOREIGN KEY (IdConsulta) REFERENCES dbo.Consulta (IdConsulta) ON DELETE CASCADE,
    CONSTRAINT FK_DetalleConsulta_Tratamiento FOREIGN KEY (IdTratamiento) REFERENCES dbo.Tratamiento (IdTratamiento)
);
GO

-- Tabla: AplicacionVacuna
CREATE TABLE dbo.AplicacionVacuna (
    IdAplicacionVacuna INT IDENTITY(1,1) NOT NULL,
    IdConsulta INT NOT NULL,
    IdVacuna INT NOT NULL,
    FechaAplicacion DATE NOT NULL,
    ProximaDosis DATE NULL,
    Observaciones NVARCHAR(250) NULL,
    PrecioAplicado DECIMAL(18,2) NOT NULL CONSTRAINT DF_AplicacionVacuna_Precio DEFAULT (0),
    Activo BIT NOT NULL CONSTRAINT DF_AplicacionVacuna_Activo DEFAULT (1),
    CONSTRAINT PK_AplicacionVacuna PRIMARY KEY CLUSTERED (IdAplicacionVacuna ASC),
    CONSTRAINT FK_AplicacionVacuna_Consulta FOREIGN KEY (IdConsulta) REFERENCES dbo.Consulta (IdConsulta) ON DELETE CASCADE,
    CONSTRAINT FK_AplicacionVacuna_Vacuna FOREIGN KEY (IdVacuna) REFERENCES dbo.Vacuna (IdVacuna)
);
GO

-- Tabla: Pago
CREATE TABLE dbo.Pago (
    IdPago INT IDENTITY(1,1) NOT NULL,
    IdConsulta INT NOT NULL,
    IdMetodoPago INT NOT NULL,
    Fecha DATETIME2(0) NOT NULL,
    Importe DECIMAL(18,2) NOT NULL,
    Estado NVARCHAR(30) NOT NULL CONSTRAINT DF_Pago_Estado DEFAULT ('Completado'),
    Activo BIT NOT NULL CONSTRAINT DF_Pago_Activo DEFAULT (1),
    CONSTRAINT PK_Pago PRIMARY KEY CLUSTERED (IdPago ASC),
    CONSTRAINT FK_Pago_Consulta FOREIGN KEY (IdConsulta) REFERENCES dbo.Consulta (IdConsulta),
    CONSTRAINT FK_Pago_MetodoPago FOREIGN KEY (IdMetodoPago) REFERENCES dbo.MetodoPago (IdMetodoPago)
);
GO

-- ------------------------------------------------------------------------------
-- 4. INDICES PARA OPTIMIZACION DE CONSULTAS
-- ------------------------------------------------------------------------------
CREATE NONCLUSTERED INDEX IX_Usuario_IdTipoUsuario ON dbo.Usuario(IdTipoUsuario);
CREATE NONCLUSTERED INDEX IX_Mascota_IdPropietario ON dbo.Mascota(IdPropietario);
CREATE NONCLUSTERED INDEX IX_Mascota_IdEspecie ON dbo.Mascota(IdEspecie);
CREATE NONCLUSTERED INDEX IX_Raza_IdEspecie ON dbo.Raza(IdEspecie);
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
INSERT INTO dbo.TipoUsuario (IdTipoUsuario, Nombre, Activo) VALUES
(1, 'Administrador', 1),
(2, 'Veterinario', 1),
(3, 'Secretario', 1);
SET IDENTITY_INSERT dbo.TipoUsuario OFF;
GO

SET IDENTITY_INSERT dbo.Especie ON;
INSERT INTO dbo.Especie (IdEspecie, Nombre, Activo) VALUES
(1, 'Canino', 1),
(2, 'Felino', 1),
(3, 'Ave', 1),
(4, 'Roedor', 1);
SET IDENTITY_INSERT dbo.Especie OFF;
GO

SET IDENTITY_INSERT dbo.MetodoPago ON;
INSERT INTO dbo.MetodoPago (IdMetodoPago, Nombre, Activo) VALUES
(1, 'Efectivo', 1),
(2, 'Tarjeta de Débito', 1),
(3, 'Tarjeta de Crédito', 1),
(4, 'Transferencia Bancaria', 1);
SET IDENTITY_INSERT dbo.MetodoPago OFF;
GO

-- Usuario Administrador Inicial
SET IDENTITY_INSERT dbo.Usuario ON;
INSERT INTO dbo.Usuario (IdUsuario, IdTipoUsuario, Username, PasswordHash, Nombre, Apellido, DNI, Matricula, Activo)
VALUES (1, 1, 'admin', '$2a$11$q9hM1K9oA5F8VzE9hX2Hke0bT.G7j5Z8U1j.H7y1N8r9o1V7e8i9a', 'Administrador', 'Sistema', '12345678', 'ADM-001', 1);
SET IDENTITY_INSERT dbo.Usuario OFF;
GO
