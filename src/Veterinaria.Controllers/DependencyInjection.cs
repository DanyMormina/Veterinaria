using Microsoft.Extensions.DependencyInjection;
using Veterinaria.Controllers.Controladores;

namespace Microsoft.Extensions.DependencyInjection;

/// <summary>
/// Métodos de extensión para el registro de Controladores en el contenedor de Inyección de Dependencias.
/// </summary>
public static class ControllerDependencyInjectionExtensions
{
    /// <summary>
    /// Registra todos los Controladores (Transient) de la solución.
    /// </summary>
    /// <param name="services">Colección de servicios del contenedor de dependencias.</param>
    /// <returns>La misma colección para encadenamiento fluido.</returns>
    public static IServiceCollection AddApplicationControllers(this IServiceCollection services)
    {
        services.AddTransient<TipoUsuarioController>();
        services.AddTransient<EspecieController>();
        services.AddTransient<RazaController>();
        services.AddTransient<VacunaController>();
        services.AddTransient<MetodoPagoController>();

        services.AddTransient<UsuarioController>();

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
