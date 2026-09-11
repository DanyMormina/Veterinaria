-- ==============================================================================
-- SCRIPT DE MIGRACIÓN: ALINEACIÓN CON EL MODELO DE DATOS ACTUAL (DER v2 / C# .NET 10)
-- Base de Datos: VeterinariaDb
-- Descripción:
--   1. dbo.Usuario: Renombrar Username -> NombreUsuario, PasswordHash -> HashContrasena,
--      y actualizar restricción única.
--   2. dbo.Propietario: Renombrar Email -> CorreoElectronico.
--   3. dbo.Vacuna: Incorporar IdEspecie (FK a Especie), Precio e índice IX_Vacuna_IdEspecie.
--   4. dbo.AplicacionVacuna: Remover columnas y restricciones obsoletas PrecioAplicado y Activo.
-- ==============================================================================

USE VeterinariaDb;
GO

BEGIN TRANSACTION;
BEGIN TRY

    -- --------------------------------------------------------------------------
    -- 1. ALINEACIÓN DE TABLA dbo.Usuario
    -- --------------------------------------------------------------------------
    -- A. Eliminar restricción única previa sobre Username si existe
    IF EXISTS (SELECT 1 FROM sys.key_constraints WHERE name = N'UQ_Usuario_Username')
    BEGIN
        ALTER TABLE dbo.Usuario DROP CONSTRAINT UQ_Usuario_Username;
        PRINT 'Restriccion UQ_Usuario_Username eliminada.';
    END

    -- B. Renombrar columna Username -> NombreUsuario si aún no ha sido renombrada
    IF EXISTS (
        SELECT 1 FROM sys.columns 
        WHERE object_id = OBJECT_ID(N'dbo.Usuario') AND name = N'Username'
    )
    BEGIN
        EXEC sp_rename 'dbo.Usuario.Username', 'NombreUsuario', 'COLUMN';
        PRINT 'Columna dbo.Usuario.Username renombrada a NombreUsuario.';
    END

    -- C. Renombrar columna PasswordHash -> HashContrasena si aún no ha sido renombrada
    IF EXISTS (
        SELECT 1 FROM sys.columns 
        WHERE object_id = OBJECT_ID(N'dbo.Usuario') AND name = N'PasswordHash'
    )
    BEGIN
        EXEC sp_rename 'dbo.Usuario.PasswordHash', 'HashContrasena', 'COLUMN';
        PRINT 'Columna dbo.Usuario.PasswordHash renombrada a HashContrasena.';
    END

    -- D. Crear índice único IX_Usuario_NombreUsuario si no existe
    IF NOT EXISTS (
        SELECT 1 FROM sys.indexes 
        WHERE name = N'IX_Usuario_NombreUsuario' AND object_id = OBJECT_ID(N'dbo.Usuario')
    )
    BEGIN
        CREATE UNIQUE NONCLUSTERED INDEX IX_Usuario_NombreUsuario 
        ON dbo.Usuario (NombreUsuario);
        PRINT 'Indice unico IX_Usuario_NombreUsuario creado.';
    END


    -- --------------------------------------------------------------------------
    -- 2. ALINEACIÓN DE TABLA dbo.Propietario
    -- --------------------------------------------------------------------------
    -- Renombrar columna Email -> CorreoElectronico si aún conserva el nombre anterior
    IF EXISTS (
        SELECT 1 FROM sys.columns 
        WHERE object_id = OBJECT_ID(N'dbo.Propietario') AND name = N'Email'
    )
    BEGIN
        EXEC sp_rename 'dbo.Propietario.Email', 'CorreoElectronico', 'COLUMN';
        PRINT 'Columna dbo.Propietario.Email renombrada a CorreoElectronico.';
    END


    -- --------------------------------------------------------------------------
    -- 3. ALINEACIÓN DE TABLA dbo.Vacuna
    -- --------------------------------------------------------------------------
    -- A. Agregar columna IdEspecie si no existe
    IF NOT EXISTS (
        SELECT 1 FROM sys.columns 
        WHERE object_id = OBJECT_ID(N'dbo.Vacuna') AND name = N'IdEspecie'
    )
    BEGIN
        ALTER TABLE dbo.Vacuna ADD IdEspecie BIGINT NOT NULL;
        PRINT 'Columna IdEspecie agregada a dbo.Vacuna.';
    END

    -- B. Clave foránea FK_Vacuna_Especie
    IF NOT EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = N'FK_Vacuna_Especie')
    BEGIN
        ALTER TABLE dbo.Vacuna 
        ADD CONSTRAINT FK_Vacuna_Especie FOREIGN KEY (IdEspecie) 
        REFERENCES dbo.Especie (Id);
        PRINT 'Clave foranea FK_Vacuna_Especie creada.';
    END

    -- C. Agregar columna Precio si no existe
    IF NOT EXISTS (
        SELECT 1 FROM sys.columns 
        WHERE object_id = OBJECT_ID(N'dbo.Vacuna') AND name = N'Precio'
    )
    BEGIN
        ALTER TABLE dbo.Vacuna 
        ADD Precio DECIMAL(18,2) NOT NULL CONSTRAINT DF_Vacuna_Precio DEFAULT (0);
        PRINT 'Columna Precio con valor default 0 agregada a dbo.Vacuna.';
    END

    -- D. Crear índice para optimización IX_Vacuna_IdEspecie
    IF NOT EXISTS (
        SELECT 1 FROM sys.indexes 
        WHERE name = N'IX_Vacuna_IdEspecie' AND object_id = OBJECT_ID(N'dbo.Vacuna')
    )
    BEGIN
        CREATE NONCLUSTERED INDEX IX_Vacuna_IdEspecie 
        ON dbo.Vacuna (IdEspecie);
        PRINT 'Indice IX_Vacuna_IdEspecie creado.';
    END


    -- --------------------------------------------------------------------------
    -- 4. ALINEACIÓN DE TABLA dbo.AplicacionVacuna
    -- --------------------------------------------------------------------------
    -- A. Quitar constraint DF_AplicacionVacuna_Precio y columna PrecioAplicado
    IF EXISTS (SELECT 1 FROM sys.default_constraints WHERE name = N'DF_AplicacionVacuna_Precio')
    BEGIN
        ALTER TABLE dbo.AplicacionVacuna DROP CONSTRAINT DF_AplicacionVacuna_Precio;
        PRINT 'Restriccion DF_AplicacionVacuna_Precio eliminada.';
    END

    IF EXISTS (
        SELECT 1 FROM sys.columns 
        WHERE object_id = OBJECT_ID(N'dbo.AplicacionVacuna') AND name = N'PrecioAplicado'
    )
    BEGIN
        ALTER TABLE dbo.AplicacionVacuna DROP COLUMN PrecioAplicado;
        PRINT 'Columna obsoleta PrecioAplicado eliminada de dbo.AplicacionVacuna.';
    END

    -- B. Quitar constraint DF_AplicacionVacuna_Activo y columna Activo
    IF EXISTS (SELECT 1 FROM sys.default_constraints WHERE name = N'DF_AplicacionVacuna_Activo')
    BEGIN
        ALTER TABLE dbo.AplicacionVacuna DROP CONSTRAINT DF_AplicacionVacuna_Activo;
        PRINT 'Restriccion DF_AplicacionVacuna_Activo eliminada.';
    END

    IF EXISTS (
        SELECT 1 FROM sys.columns 
        WHERE object_id = OBJECT_ID(N'dbo.AplicacionVacuna') AND name = N'Activo'
    )
    BEGIN
        ALTER TABLE dbo.AplicacionVacuna DROP COLUMN Activo;
        PRINT 'Columna obsoleta Activo eliminada de dbo.AplicacionVacuna.';
    END

    COMMIT TRANSACTION;
    PRINT '==================================================';
    PRINT 'MIGRACION DE VETERINARIADB COMPLETADA EXITOSAMENTE';
    PRINT '==================================================';
END TRY
BEGIN CATCH
    IF @@TRANCOUNT > 0
        ROLLBACK TRANSACTION;

    PRINT 'ERROR DURANTE LA MIGRACION: ' + ERROR_MESSAGE();
    THROW;
END CATCH;
GO
