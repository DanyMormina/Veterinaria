using Microsoft.Extensions.DependencyInjection;
using Veterinaria.Controllers.Controladores;

namespace Microsoft.Extensions.DependencyInjection;

/// <summary>
/// Métodos de extensión para el registro de Controladores en el contenedor de Inyección de Dependencias.
/// </summary>
public static class ExtensionesInyeccionControladores
{
    /// <summary>
    /// Registra todos los Controladores (Transient) de la solución.
    /// </summary>
    /// <param name="services">Colección de servicios del contenedor de dependencias.</param>
    /// <returns>La misma colección para encadenamiento fluido.</returns>
    public static IServiceCollection AgregarControladoresAplicacion(this IServiceCollection services)
    {
        services.AddTransient<TipoUsuarioControlador>();
        services.AddTransient<EspecieControlador>();
        services.AddTransient<RazaControlador>();
        services.AddTransient<VacunaControlador>();
        services.AddTransient<MetodoPagoControlador>();

        services.AddTransient<UsuarioControlador>();

        services.AddTransient<PropietarioControlador>();
        services.AddTransient<MascotaControlador>();

        services.AddTransient<ConsultaControlador>();
        services.AddTransient<TratamientoControlador>();
        services.AddTransient<DetalleConsultaControlador>();
        services.AddTransient<AplicacionVacunaControlador>();

        services.AddTransient<PagoControlador>();

        return services;
    }
}
