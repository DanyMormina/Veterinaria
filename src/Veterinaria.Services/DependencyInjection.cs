using Microsoft.Extensions.DependencyInjection;
using Veterinaria.Controllers.Controladores;
using Veterinaria.Interfaces.Interfaces;
using Veterinaria.Services.Servicios;

namespace Microsoft.Extensions.DependencyInjection;

/// <summary>
/// Métodos de extensión para el registro y configuración del contenedor de Inyección de Dependencias.
/// </summary>
public static class DependencyInjectionExtensions
{
    /// <summary>
    /// Registra todos los Servicios de Aplicación (Scoped) y Controladores (Transient) de la solución veterinaria (DER v2).
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
        // 2. Servicios de Seguridad y Logs (Scoped)
        // -------------------------------------------------------------
        services.AddScoped<IUsuarioService, UsuarioService>();
        services.AddScoped<ISesionService, SesionService>();
        services.AddScoped<IAuditoriaService, AuditoriaService>();

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

        // -------------------------------------------------------------
        // Controladores (Transient)
        // -------------------------------------------------------------
        services.AddTransient<TipoUsuarioController>();
        services.AddTransient<EspecieController>();
        services.AddTransient<RazaController>();
        services.AddTransient<VacunaController>();
        services.AddTransient<MetodoPagoController>();

        services.AddTransient<UsuarioController>();
        services.AddTransient<SesionController>();
        services.AddTransient<AuditoriaController>();

        services.AddTransient<PropietarioController>();
        services.AddTransient<MascotaController>();

        services.AddTransient<ConsultaController>();
        services.AddTransient<TratamientoController>();
        services.AddTransient<DetalleConsultaController>();
        services.AddTransient<AplicacionVacunaController>();

        services.AddTransient<PagoController>();

        return services;
    }
}
