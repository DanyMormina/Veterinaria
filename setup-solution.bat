@echo off
setlocal enabledelayedexpansion
title Setup Solucion Veterinaria - Arquitectura Desacoplada N-Capas .NET 10

echo ==============================================================================
echo   1. CREANDO SOLUCION Y PROYECTOS (.NET 10)
echo ==============================================================================
echo.

:: 1. Crear Solución en blanco
dotnet new sln -n Veterinaria

:: 2. Crear Proyectos por Capa
dotnet new classlib -o src/Veterinaria.Domain -f net10.0
dotnet new classlib -o src/Veterinaria.CrossCutting -f net10.0
dotnet new classlib -o src/Veterinaria.Interfaces -f net10.0
dotnet new classlib -o src/Veterinaria.Infrastructure -f net10.0
dotnet new classlib -o src/Veterinaria.Services -f net10.0
dotnet new classlib -o src/Veterinaria.Controllers -f net10.0
dotnet new winforms -o src/Veterinaria.WinForms -f net10.0

:: 3. Vincular Proyectos a la Solución
dotnet sln add src/Veterinaria.Domain/Veterinaria.Domain.csproj
dotnet sln add src/Veterinaria.CrossCutting/Veterinaria.CrossCutting.csproj
dotnet sln add src/Veterinaria.Interfaces/Veterinaria.Interfaces.csproj
dotnet sln add src/Veterinaria.Infrastructure/Veterinaria.Infrastructure.csproj
dotnet sln add src/Veterinaria.Services/Veterinaria.Services.csproj
dotnet sln add src/Veterinaria.Controllers/Veterinaria.Controllers.csproj
dotnet sln add src/Veterinaria.WinForms/Veterinaria.WinForms.csproj

echo.
echo ==============================================================================
echo   2. CONFIGURANDO REFERENCIAS (INVERSION DE DEPENDENCIAS)
echo ==============================================================================
echo.

dotnet add src/Veterinaria.CrossCutting/Veterinaria.CrossCutting.csproj reference src/Veterinaria.Domain/Veterinaria.Domain.csproj

dotnet add src/Veterinaria.Interfaces/Veterinaria.Interfaces.csproj reference src/Veterinaria.Domain/Veterinaria.Domain.csproj
dotnet add src/Veterinaria.Interfaces/Veterinaria.Interfaces.csproj reference src/Veterinaria.CrossCutting/Veterinaria.CrossCutting.csproj

dotnet add src/Veterinaria.Infrastructure/Veterinaria.Infrastructure.csproj reference src/Veterinaria.Domain/Veterinaria.Domain.csproj
dotnet add src/Veterinaria.Infrastructure/Veterinaria.Infrastructure.csproj reference src/Veterinaria.CrossCutting/Veterinaria.CrossCutting.csproj
dotnet add src/Veterinaria.Infrastructure/Veterinaria.Infrastructure.csproj reference src/Veterinaria.Interfaces/Veterinaria.Interfaces.csproj

dotnet add src/Veterinaria.Services/Veterinaria.Services.csproj reference src/Veterinaria.Domain/Veterinaria.Domain.csproj
dotnet add src/Veterinaria.Services/Veterinaria.Services.csproj reference src/Veterinaria.Interfaces/Veterinaria.Interfaces.csproj
dotnet add src/Veterinaria.Services/Veterinaria.Services.csproj reference src/Veterinaria.Infrastructure/Veterinaria.Infrastructure.csproj
dotnet add src/Veterinaria.Services/Veterinaria.Services.csproj reference src/Veterinaria.CrossCutting/Veterinaria.CrossCutting.csproj

dotnet add src/Veterinaria.Controllers/Veterinaria.Controllers.csproj reference src/Veterinaria.Domain/Veterinaria.Domain.csproj
dotnet add src/Veterinaria.Controllers/Veterinaria.Controllers.csproj reference src/Veterinaria.Interfaces/Veterinaria.Interfaces.csproj
dotnet add src/Veterinaria.Controllers/Veterinaria.Controllers.csproj reference src/Veterinaria.CrossCutting/Veterinaria.CrossCutting.csproj

dotnet add src/Veterinaria.WinForms/Veterinaria.WinForms.csproj reference src/Veterinaria.Controllers/Veterinaria.Controllers.csproj
dotnet add src/Veterinaria.WinForms/Veterinaria.WinForms.csproj reference src/Veterinaria.CrossCutting/Veterinaria.CrossCutting.csproj
dotnet add src/Veterinaria.WinForms/Veterinaria.WinForms.csproj reference src/Veterinaria.Infrastructure/Veterinaria.Infrastructure.csproj
dotnet add src/Veterinaria.WinForms/Veterinaria.WinForms.csproj reference src/Veterinaria.Services/Veterinaria.Services.csproj

echo.
echo ==============================================================================
echo   3. INSTALANDO PAQUETES NUGET REQUERIDOS
echo ==============================================================================
echo.

dotnet add src/Veterinaria.CrossCutting/Veterinaria.CrossCutting.csproj package BCrypt.Net-Next
dotnet add src/Veterinaria.Infrastructure/Veterinaria.Infrastructure.csproj package Microsoft.EntityFrameworkCore.SqlServer
dotnet add src/Veterinaria.Infrastructure/Veterinaria.Infrastructure.csproj package Microsoft.EntityFrameworkCore.Tools
dotnet add src/Veterinaria.WinForms/Veterinaria.WinForms.csproj package Microsoft.Extensions.DependencyInjection

echo.
echo ==============================================================================
echo   4. CREANDO JERARQUIA DE CARPETAS SEGREGADAS POR ENTIDAD
echo ==============================================================================
echo.

:: Dominio
mkdir src\Veterinaria.Domain\Entities
mkdir src\Veterinaria.Domain\Enums
mkdir src\Veterinaria.Domain\DTOs
mkdir src\Veterinaria.Domain\Common

:: CrossCutting
mkdir src\Veterinaria.CrossCutting\Common
mkdir src\Veterinaria.CrossCutting\Session
mkdir src\Veterinaria.CrossCutting\Security

:: Interfaces Segregadas por Entidad
mkdir src\Veterinaria.Interfaces\Services\Propietarios
mkdir src\Veterinaria.Interfaces\Services\Mascotas
mkdir src\Veterinaria.Interfaces\Services\Turnos
mkdir src\Veterinaria.Interfaces\Services\Consultas
mkdir src\Veterinaria.Interfaces\Services\Tratamientos
mkdir src\Veterinaria.Interfaces\Services\Catalogos
mkdir src\Veterinaria.Interfaces\Services\Facturacion
mkdir src\Veterinaria.Interfaces\Services\Seguridad
mkdir src\Veterinaria.Interfaces\Services\Sistema

:: Servicios Aislados por Entidad
mkdir src\Veterinaria.Services\Propietarios
mkdir src\Veterinaria.Services\Mascotas
mkdir src\Veterinaria.Services\Turnos
mkdir src\Veterinaria.Services\Consultas
mkdir src\Veterinaria.Services\Tratamientos
mkdir src\Veterinaria.Services\Catalogos
mkdir src\Veterinaria.Services\Facturacion
mkdir src\Veterinaria.Services\Seguridad
mkdir src\Veterinaria.Services\Sistema

:: Controladores Aislados por Entidad
mkdir src\Veterinaria.Controllers\Propietarios
mkdir src\Veterinaria.Controllers\Mascotas
mkdir src\Veterinaria.Controllers\Turnos
mkdir src\Veterinaria.Controllers\Consultas
mkdir src\Veterinaria.Controllers\Tratamientos
mkdir src\Veterinaria.Controllers\Catalogos
mkdir src\Veterinaria.Controllers\Facturacion
mkdir src\Veterinaria.Controllers\Seguridad
mkdir src\Veterinaria.Controllers\Sistema

:: Infraestructura (EF Core y Servicios)
mkdir src\Veterinaria.Infrastructure\Context
mkdir src\Veterinaria.Infrastructure\Configurations
mkdir src\Veterinaria.Infrastructure\Migrations
mkdir src\Veterinaria.Infrastructure\Seeding
mkdir src\Veterinaria.Infrastructure\Services

:: Presentación (WinForms Vistas)
mkdir src\Veterinaria.WinForms\Views\Auth
mkdir src\Veterinaria.WinForms\Views\Propietarios
mkdir src\Veterinaria.WinForms\Views\Mascotas
mkdir src\Veterinaria.WinForms\Views\Turnos
mkdir src\Veterinaria.WinForms\Views\Consultas
mkdir src\Veterinaria.WinForms\Views\Facturacion
mkdir src\Veterinaria.WinForms\Views\Admin
mkdir src\Veterinaria.WinForms\Views\Reportes

echo.
echo ==============================================================================
echo   ENTORNO Y ESTRUCTURA DE PROYECTOS CONFIGURADOS CON EXITO
echo ==============================================================================
pause