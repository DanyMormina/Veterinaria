@echo off
title Veterinaria - Iniciar Base de Datos SQL Server

echo ==============================================================================
echo        INICIANDO CONTENEDOR DE BASE DE DATOS SQL SERVER (VETERINARIA)
echo ==============================================================================
echo.

cd /d "%~dp0"

:: 1. Asegurar carpeta local de backups en el host
if not exist "C:\Backup_Veterinaria" (
    echo [INFO] Creando directorio para respaldos en C:\Backup_Veterinaria...
    mkdir "C:\Backup_Veterinaria" 2>nul
)

:: 2. Iniciar contenedor vía docker compose
echo [INFO] Levantando contenedor mssql2022_veterinaria...
docker compose up -d
if errorlevel 1 goto :docker_error

echo [OK] Contenedor levantado exitosamente.
echo [INFO] Esperando a que el motor SQL Server este listo para recibir consultas...

:: 3. Esperar a que el motor responda (maximo 30 segundos)
set /a SQL_RETRIES=0

:wait_sql
docker exec mssql2022_veterinaria /opt/mssql-tools18/bin/sqlcmd -C -S localhost -U sa -Q "SELECT 1" >nul 2>nul
if not errorlevel 1 goto :sql_ready

set /a SQL_RETRIES+=1
if %SQL_RETRIES% geq 15 goto :sql_timeout
ping 127.0.0.1 -n 3 >nul
goto :wait_sql

:sql_timeout
echo.
echo [ERROR] SQL Server no respondio en el tiempo esperado (30 segundos).
pause
exit /b 1

:docker_error
echo.
echo [ERROR] No se pudo iniciar el contenedor con Docker Compose.
echo Verifique que Docker Desktop este en ejecucion.
pause
exit /b 1

:sql_ready
echo [OK] SQL Server activo y respondiendo.

:: 4. Comprobar si la base de datos VeterinariaDb y sus tablas existen
set "TABLE_COUNT=0"
for /f "tokens=*" %%i in ('docker exec mssql2022_veterinaria /opt/mssql-tools18/bin/sqlcmd -C -S localhost -U sa -d master -h -1 -W -Q "SET NOCOUNT ON; IF EXISTS (SELECT 1 FROM sys.databases WHERE name = 'VeterinariaDb') SELECT COUNT(*) FROM VeterinariaDb.INFORMATION_SCHEMA.TABLES ELSE SELECT 0" 2^>nul') do set "TABLE_COUNT=%%i"

if "%TABLE_COUNT%"=="0" goto :create_db

echo [OK] Base de datos 'VeterinariaDb' encontrada con estructura activa (%TABLE_COUNT% tablas).
goto :db_finished

:create_db
echo [INFO] Base de datos 'VeterinariaDb' no encontrada o sin tablas.
echo [INFO] Desplegando estructura inicial y tablas maestras (Script DDL v2)...
docker cp src/Veterinaria.Infrastructure/Scripts/Script_DDL_Veterinaria_v2.sql mssql2022_veterinaria:/tmp/init_db.sql
docker exec mssql2022_veterinaria /opt/mssql-tools18/bin/sqlcmd -C -S localhost -U sa -i /tmp/init_db.sql >nul
if errorlevel 1 (
    echo [ADVERTENCIA] El script DDL reporto incidencias. La aplicacion aplicara inicializaciones complementarias.
) else (
    echo [OK] Base de datos 'VeterinariaDb' y estructura inicial creadas exitosamente.
)

:db_finished
echo.
echo ==============================================================================
echo  Servidor SQL Server 2022 listo (localhost:1433, SA / Pass123456!)
echo ==============================================================================
echo.
pause
