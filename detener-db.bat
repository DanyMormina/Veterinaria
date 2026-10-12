@echo off
echo Deteniendo contenedor de base de datos SQL Server (Veterinaria)...
docker compose down
if %ERRORLEVEL% equ 0 (
    echo Contenedor detenido exitosamente.
) else (
    echo Error al detener el contenedor.
)
pause
