using Microsoft.Extensions.DependencyInjection;
using Veterinaria.Controllers.Controladores;
using Veterinaria.Controllers.Sistema;
using Veterinaria.Controllers.Seguridad;
using Veterinaria.Interfaces.Services.Seguridad;
using Veterinaria.Interfaces.Services.Sistema;
using Veterinaria.Services.Seguridad;
using Veterinaria.Services.Sistema;

namespace Microsoft.Extensions.DependencyInjection;
// Métodos de extensión para el registro de Controladores en el contenedor de Inyección de Dependencias.
public static class ExtensionesInyeccionControladores
{
    // Registra todos los Controladores (Transient) de la solución.
    public static IServiceCollection AgregarControladoresAplicacion(this IServiceCollection services)
    {
        services.AddTransient<TipoUsuarioControlador>();
        services.AddTransient<EspecieControlador>();
        services.AddTransient<RazaControlador>();
        services.AddTransient<VacunaControlador>();
        services.AddTransient<VacunaController>();
        services.AddTransient<MetodoPagoControlador>();

        services.AddTransient<UsuarioControlador>();
        services.AddTransient<IUsuarioService, UsuarioService>();
        services.AddTransient<UsuarioService>();
        services.AddTransient<UsuarioController>();

        services.AddTransient<PropietarioControlador>();
        services.AddTransient<PropietarioService>();
        services.AddTransient<MascotaControlador>();
        services.AddTransient<MascotaService>();

        services.AddTransient<ConsultaControlador>();
        services.AddTransient<ConsultaController>();
        services.AddTransient<TratamientoControlador>();
        services.AddTransient<DetalleConsultaControlador>();
        services.AddTransient<AplicacionVacunaControlador>();

        services.AddTransient<PagoControlador>();
        services.AddTransient<ReporteControlador>();

        // Módulo de Copias de Seguridad (Backup)
        services.AddTransient<IBackupService, BackupService>();
        services.AddTransient<BackupController>();

        return services;
    }
}
