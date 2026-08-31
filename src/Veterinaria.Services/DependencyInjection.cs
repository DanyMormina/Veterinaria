using Microsoft.Extensions.DependencyInjection;
using Veterinaria.Interfaces.Interfaces;
using Veterinaria.Services.Servicios;

namespace Microsoft.Extensions.DependencyInjection;

/// <summary>
/// Métodos de extensión para el registro y configuración de Servicios de Negocio en el contenedor de dependencias.
/// </summary>
public static class ServiceDependencyInjectionExtensions
{
    /// <summary>
    /// Registra todos los Servicios de Aplicación (Scoped) de la solución veterinaria.
    /// </summary>
    /// <param name="services">Colección de servicios del contenedor de dependencias.</param>
    /// <returns>La misma colección para encadenamiento fluido.</returns>
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        // -------------------------------------------------------------
        // 1. Servicios de Catálogo y Satélites (Scoped)
        // -------------------------------------------------------------
        services.AddScoped<ITipoUsuarioService, TipoUsuarioService>();
        services.AddScoped<IEspecieService, EspecieService>();
        services.AddScoped<IRazaService, RazaService>();
        services.AddScoped<IVacunaService, VacunaService>();
        services.AddScoped<IMetodoPagoService, MetodoPagoService>();

        // -------------------------------------------------------------
        // 2. Servicios de Usuarios y Seguridad (Scoped)
        // -------------------------------------------------------------
        services.AddScoped<IUsuarioService, UsuarioService>();

        // -------------------------------------------------------------
        // 3. Servicios de Clientes y Pacientes (Scoped)
        // -------------------------------------------------------------
        services.AddScoped<IPropietarioService, PropietarioService>();
        services.AddScoped<IMascotaService, MascotaService>();

        // -------------------------------------------------------------
        // 4. Servicios Clínicos y Tratamientos (Scoped)
        // -------------------------------------------------------------
        services.AddScoped<IConsultaService, ConsultaService>();
        services.AddScoped<ITratamientoService, TratamientoService>();
        services.AddScoped<IDetalleConsultaService, DetalleConsultaService>();
        services.AddScoped<IAplicacionVacunaService, AplicacionVacunaService>();

        // -------------------------------------------------------------
        // 5. Servicios de Pagos y Cobranzas (Scoped)
        // -------------------------------------------------------------
        services.AddScoped<IPagoService, PagoService>();

        return services;
    }
}
