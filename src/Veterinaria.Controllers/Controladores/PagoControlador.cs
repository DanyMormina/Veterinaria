using Microsoft.EntityFrameworkCore;
using Veterinaria.Domain.Comunes;
using Veterinaria.Domain.Dtos;
using Veterinaria.Domain.Entidades;
using Veterinaria.Infrastructure;

namespace Veterinaria.Controllers.Controladores;

public class PagoControlador(ContextoVeterinaria context)
{
    public async Task<Resultado<IEnumerable<PagoRespuestaDto>>> ObtenerPorConsultaAsync(long idConsulta)
    {
        try
        {
            var items = await ConsultaBase()
                .Where(p => p.IdConsulta == idConsulta)
                .ToListAsync();

            return Resultado<IEnumerable<PagoRespuestaDto>>.Exito(items);
        }
        catch (Exception ex)
        {
            return Resultado<IEnumerable<PagoRespuestaDto>>.Falla($"Error al obtener pagos de la consulta: {ex.Message}");
        }
    }

    public async Task<Resultado<IEnumerable<PagoRespuestaDto>>> ObtenerTodosAsync()
    {
        try
        {
            var items = await ConsultaBase()
                .OrderByDescending(p => p.Fecha)
                .ThenByDescending(p => p.Id)
                .ToListAsync();
            return Resultado<IEnumerable<PagoRespuestaDto>>.Exito(items);
        }
        catch (Exception ex)
        {
            return Resultado<IEnumerable<PagoRespuestaDto>>.Falla($"Error al obtener pagos: {ex.Message}");
        }
    }

    public async Task<Resultado<PagoRespuestaDto>> ObtenerPorIdAsync(long id)
    {
        try
        {
            if (id <= 0)
                return Resultado<PagoRespuestaDto>.Falla("El identificador debe ser mayor a cero.");

            var item = await ConsultaBase().FirstOrDefaultAsync(p => p.Id == id);
            if (item is null)
                return Resultado<PagoRespuestaDto>.Falla($"No se encontró el pago con ID {id}.");

            return Resultado<PagoRespuestaDto>.Exito(item);
        }
        catch (Exception ex)
        {
            return Resultado<PagoRespuestaDto>.Falla($"Error al obtener el pago: {ex.Message}");
        }
    }

    public async Task<Resultado<long>> RegistrarPagoAsync(PagoSolicitudDto solicitud)
    {
        try
        {
            var error = Validar(solicitud);
            if (error is not null)
                return Resultado<long>.Falla(error);

            var consultaExiste = await context.Consultas.AnyAsync(c => c.Id == solicitud.IdConsulta);
            if (!consultaExiste)
                return Resultado<long>.Falla($"No existe la consulta con ID {solicitud.IdConsulta}.");

            var metodoExiste = await context.MetodosPago.AnyAsync(m => m.Id == solicitud.IdMetodoPago);
            if (!metodoExiste)
                return Resultado<long>.Falla($"No existe el método de pago con ID {solicitud.IdMetodoPago}.");

            var estado = NormalizarEstado(solicitud.Estado);
            var entidad = new Pago
            {
                IdConsulta = solicitud.IdConsulta,
                IdMetodoPago = solicitud.IdMetodoPago,
                Fecha = solicitud.Fecha == default ? DateTime.Now : solicitud.Fecha,
                Importe = solicitud.Importe,
                Estado = estado,
                Cuotas = solicitud.Cuotas,
                Activo = !string.Equals(estado, "Anulado", StringComparison.OrdinalIgnoreCase)
            };

            context.Pagos.Add(entidad);
            await context.SaveChangesAsync();
            return Resultado<long>.Exito(entidad.Id, "Pago registrado exitosamente.");
        }
        catch (Exception ex)
        {
            return Resultado<long>.Falla($"Error al registrar el pago: {ex.Message}");
        }
    }

    public async Task<Resultado> ActualizarAsync(long id, PagoSolicitudDto solicitud)
    {
        try
        {
            if (id <= 0)
                return Resultado.Falla("El identificador debe ser mayor a cero.");

            var error = Validar(solicitud);
            if (error is not null)
                return Resultado.Falla(error);

            var entidad = await context.Pagos.IgnoreQueryFilters().FirstOrDefaultAsync(p => p.Id == id);
            if (entidad is null)
                return Resultado.Falla($"No se encontró el pago con ID {id}.");

            var metodoExiste = await context.MetodosPago.AnyAsync(m => m.Id == solicitud.IdMetodoPago);
            if (!metodoExiste)
                return Resultado.Falla($"No existe el método de pago con ID {solicitud.IdMetodoPago}.");

            var estado = NormalizarEstado(solicitud.Estado);
            entidad.IdConsulta = solicitud.IdConsulta;
            entidad.IdMetodoPago = solicitud.IdMetodoPago;
            entidad.Fecha = solicitud.Fecha == default ? entidad.Fecha : solicitud.Fecha;
            entidad.Importe = solicitud.Importe;
            entidad.Estado = estado;
            entidad.Cuotas = solicitud.Cuotas;
            entidad.Activo = !string.Equals(estado, "Anulado", StringComparison.OrdinalIgnoreCase);

            if (!context.ChangeTracker.HasChanges())
                return Resultado.Falla("No se detectaron cambios para guardar.");

            await context.SaveChangesAsync();
            return Resultado.Exito("Pago actualizado exitosamente.");
        }
        catch (Exception ex)
        {
            return Resultado.Falla($"Error al actualizar el pago: {ex.Message}");
        }
    }

    public async Task<Resultado> AnularPagoAsync(long id)
    {
        try
        {
            if (id <= 0)
                return Resultado.Falla("El identificador debe ser mayor a cero.");

            var entidad = await context.Pagos.IgnoreQueryFilters().FirstOrDefaultAsync(p => p.Id == id);
            if (entidad is null)
                return Resultado.Falla($"No se encontró el pago con ID {id}.");

            entidad.Estado = "Anulado";
            entidad.Activo = false;

            if (!context.ChangeTracker.HasChanges())
                return Resultado.Falla("No se detectaron cambios para guardar.");

            await context.SaveChangesAsync();
            return Resultado.Exito("Pago anulado exitosamente.");
        }
        catch (Exception ex)
        {
            return Resultado.Falla($"Error al anular el pago: {ex.Message}");
        }
    }

    private static string? Validar(PagoSolicitudDto solicitud)
    {
        if (solicitud.IdConsulta <= 0)
            return "La consulta asociada es obligatoria.";

        if (solicitud.IdMetodoPago <= 0)
            return "El método de pago es obligatorio.";

        if (solicitud.Importe <= 0)
            return "El importe debe ser mayor a cero.";

        return null;
    }

    private static string NormalizarEstado(string? estado)
    {
        var valor = string.IsNullOrWhiteSpace(estado) ? "Completado" : estado.Trim();
        if (valor.Equals("Pagado", StringComparison.OrdinalIgnoreCase))
            return "Completado";
        return valor;
    }

    private IQueryable<PagoRespuestaDto> ConsultaBase() =>
        context.Pagos
            .IgnoreQueryFilters()
            .AsNoTracking()
            .Select(p => new PagoRespuestaDto
            {
                Id = p.Id,
                IdConsulta = p.IdConsulta,
                DescripcionConsulta = "Consulta #" + p.IdConsulta + " - " + p.Consulta.Mascota.Nombre +
                                      " (" + p.Consulta.Mascota.Propietario.Apellido + ")",
                IdMetodoPago = p.IdMetodoPago,
                NombreMetodoPago = p.MetodoPago != null ? p.MetodoPago.Nombre : string.Empty,
                Fecha = p.Fecha,
                Importe = p.Importe,
                Cuotas = p.Cuotas,
                Estado = p.Estado,
                Activo = p.Activo
            });
}
