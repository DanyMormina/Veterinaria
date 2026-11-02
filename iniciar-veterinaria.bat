@echo off
title Sistema Veterinario - Lanzador

echo ==============================================================================
echo              SISTEMA DE GESTION CLINICA VETERINARIA (.NET 10)
echo ==============================================================================
echo.

:: Posicionarse en el directorio del script
cd /d "%~dp0"

:: 1. Validar presencia del SDK de .NET
where dotnet >nul 2>nul
if errorlevel 1 (
    echo [ERROR] No se encontro el comando 'dotnet' en el sistema.
    echo.
    echo Por favor descargue e instale el SDK de .NET 10 o superior:
    echo https://dotnet.microsoft.com/download/dotnet/10.0
    echo.
    pause
    exit /b 1
)

for /f "tokens=*" %%v in ('dotnet --version 2^>nul') do set "DOTNET_VER=%%v"
echo [OK] .NET SDK detectado: %DOTNET_VER%

:: 2. Asegurar carpeta local de backups en el host (volumen mapeado en Docker)
if not exist "C:\Backup_Veterinaria" (
    echo [INFO] Creando directorio para respaldos en C:\Backup_Veterinaria...
    mkdir "C:\Backup_Veterinaria" 2>nul
)

:: 3. Verificar estado de Docker y contenedor de base de datos SQL Server
where docker >nul 2>nul
if errorlevel 1 goto :no_docker

docker info >nul 2>nul
if errorlevel 1 goto :docker_not_running

echo [INFO] Docker detectado. Asegurando contenedor de base de datos SQL Server...
docker compose up -d
if errorlevel 1 (
    echo [ADVERTENCIA] No se pudo iniciar el contenedor con Docker Compose.
    goto :database_ready
)

echo [INFO] Aguardando que SQL Server este listo para recibir consultas...
set /a SQL_RETRIES=0

:wait_sql
docker exec mssql2022_veterinaria /opt/mssql-tools18/bin/sqlcmd -C -S localhost -U sa -Q "SELECT 1" >nul 2>nul
if not errorlevel 1 goto :sql_ready

set /a SQL_RETRIES+=1
if %SQL_RETRIES% geq 15 (
    echo [ADVERTENCIA] SQL Server no respondio en 30 segundos. Intentando continuar...
    goto :database_ready
)
ping 127.0.0.1 -n 3 >nul
goto :wait_sql

:sql_ready
echo [OK] SQL Server activo y respondiendo.

:: Comprobar si la base de datos VeterinariaDb y sus tablas existen en el contenedor
set "TABLE_COUNT=0"
for /f "tokens=*" %%i in ('docker exec mssql2022_veterinaria /opt/mssql-tools18/bin/sqlcmd -C -S localhost -U sa -d master -h -1 -W -Q "SET NOCOUNT ON; IF EXISTS (SELECT 1 FROM sys.databases WHERE name = 'VeterinariaDb') SELECT COUNT(*) FROM VeterinariaDb.INFORMATION_SCHEMA.TABLES ELSE SELECT 0" 2^>nul') do set "TABLE_COUNT=%%i"

if "%TABLE_COUNT%"=="0" goto :create_db

echo [OK] Base de datos 'VeterinariaDb' encontrada con estructura activa (%TABLE_COUNT% tablas).
goto :database_ready

:create_db
echo [INFO] Base de datos 'VeterinariaDb' no encontrada o sin tablas en el contenedor.
echo [INFO] Desplegando estructura inicial y tablas maestras (Script DDL v2)...
docker cp src/Veterinaria.Infrastructure/Scripts/Script_DDL_Veterinaria_v2.sql mssql2022_veterinaria:/tmp/init_db.sql
docker exec mssql2022_veterinaria /opt/mssql-tools18/bin/sqlcmd -C -S localhost -U sa -i /tmp/init_db.sql >nul
if errorlevel 1 (
    echo [ADVERTENCIA] El script DDL reporto incidencias. La aplicacion aplicara inicializaciones complementarias.
) else (
    echo [OK] Base de datos 'VeterinariaDb' y estructura creadas exitosamente en el contenedor.
)
goto :database_ready

:docker_not_running
echo [AVISO] Docker esta instalado pero Docker Desktop no parece estar iniciado.
echo - Si la base de datos corre en Docker, inicie Docker Desktop.
echo - Si utiliza SQL Server instalado de forma local en el puerto 1433, el sistema continuara.
goto :database_ready

:no_docker
echo [INFO] Docker no detectado en el PATH. Utilizando instancia local de SQL Server (localhost:1433).

:database_ready
echo.
echo ==============================================================================
echo                    EJECUTANDO APLICACION WINFORMS
echo ==============================================================================
echo.

dotnet run --project src\Veterinaria.WinForms\Veterinaria.WinForms.csproj

if errorlevel 1 (
    echo.
    echo ==============================================================================
    echo [ERROR] La aplicacion finalizo con codigo de salida: %ERRORLEVEL%
    echo ==============================================================================
    echo.
    pause
)
