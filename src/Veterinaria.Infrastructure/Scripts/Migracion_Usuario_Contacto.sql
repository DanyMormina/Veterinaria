-- Migración: campos de contacto y demográficos en dbo.Usuario
-- Idempotente: solo agrega columnas si faltan.

IF COL_LENGTH('dbo.Usuario', 'Direccion') IS NULL
    ALTER TABLE dbo.Usuario ADD Direccion NVARCHAR(200) NULL;

IF COL_LENGTH('dbo.Usuario', 'Telefono') IS NULL
    ALTER TABLE dbo.Usuario ADD Telefono NVARCHAR(30) NULL;

IF COL_LENGTH('dbo.Usuario', 'CorreoElectronico') IS NULL
    ALTER TABLE dbo.Usuario ADD CorreoElectronico NVARCHAR(100) NULL;

IF COL_LENGTH('dbo.Usuario', 'FechaNacimiento') IS NULL
    ALTER TABLE dbo.Usuario ADD FechaNacimiento DATE NULL;

IF COL_LENGTH('dbo.Usuario', 'Sexo') IS NULL
    ALTER TABLE dbo.Usuario ADD Sexo NVARCHAR(10) NULL;
GO
