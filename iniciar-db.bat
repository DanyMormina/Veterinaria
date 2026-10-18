@echo off
echo Iniciando contenedor de base de datos SQL Server (Veterinaria)...
docker compose up -d
if %ERRORLEVEL% equ 0 (
    echo Contenedor iniciado exitosamente.
) else (
    echo Error al iniciar el contenedor.
)
pause
