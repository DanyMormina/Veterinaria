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

            var consulta = await context.Consultas.FirstOrDefaultAsync(c => c.Id == solicitud.IdConsulta);
            if (consulta is null)
                return Resultado<long>.Falla($"No existe la consulta con ID {solicitud.IdConsulta}.");

            if (await ConsultaYaCobradaAsync(solicitud.IdConsulta))
                return Resultado<long>.Falla($"La consulta #{solicitud.IdConsulta} ya fue cobrada. No se puede volver a cobrar.");

            var metodo = await context.MetodosPago.FirstOrDefaultAsync(m => m.Id == solicitud.IdMetodoPago);
            if (metodo is null)
                return Resultado<long>.Falla($"No existe el método de pago con ID {solicitud.IdMetodoPago}.");

            var errorReglas = ValidarFechaYCuotas(solicitud, consulta.FechaHora, metodo.Nombre);
            if (errorReglas is not null)
                return Resultado<long>.Falla(errorReglas);

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

            if (await ConsultaYaCobradaAsync(solicitud.IdConsulta, id))
                return Resultado.Falla($"La consulta #{solicitud.IdConsulta} ya tiene otro cobro registrado.");

            var metodo = await context.MetodosPago.FirstOrDefaultAsync(m => m.Id == solicitud.IdMetodoPago);
            if (metodo is null)
                return Resultado.Falla($"No existe el método de pago con ID {solicitud.IdMetodoPago}.");

            var consulta = await context.Consultas.IgnoreQueryFilters().FirstOrDefaultAsync(c => c.Id == solicitud.IdConsulta);
            if (consulta is null)
                return Resultado.Falla($"No existe la consulta con ID {solicitud.IdConsulta}.");

            var errorReglas = ValidarFechaYCuotas(solicitud, consulta.FechaHora, metodo.Nombre);
            if (errorReglas is not null)
                return Resultado.Falla(errorReglas);

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

    /// <summary>
    /// Indica si la consulta ya tiene un cobro vigente (no anulado), opcionalmente excluyendo un pago.
    /// </summary>
    public async Task<bool> ConsultaYaCobradaAsync(long idConsulta, long? idPagoExcluido = null) =>
        await context.Pagos
            .IgnoreQueryFilters()
            .AnyAsync(p => p.IdConsulta == idConsulta &&
                           p.Activo &&
                           p.Estado != "Anulado" &&
                           (!idPagoExcluido.HasValue || p.Id != idPagoExcluido.Value));

    private static string? Validar(PagoSolicitudDto solicitud)
    {
        if (solicitud.IdConsulta <= 0)
            return "La consulta asociada es obligatoria.";

        if (solicitud.IdMetodoPago <= 0)
            return "El método de pago es obligatorio.";

        if (solicitud.Importe <= 0)
            return "El importe debe ser mayor a cero.";

        var estado = NormalizarEstado(solicitud.Estado);
        if (!EstadosPermitidos.Contains(estado))
            return "El estado del pago debe ser Completado, Pendiente o Anulado.";

        return null;
    }

    private static readonly string[] EstadosPermitidos = ["Completado", "Pendiente", "Anulado"];
    private static readonly int[] CuotasPermitidas = [1, 3, 6, 9, 12];

    private static string? ValidarFechaYCuotas(PagoSolicitudDto solicitud, DateTime fechaConsulta, string nombreMetodo)
    {
        if (solicitud.Fecha != default)
        {
            if (solicitud.Fecha.Date > DateTime.Today)
                return "La fecha del pago no puede ser posterior a hoy.";

            if (solicitud.Fecha.Date < fechaConsulta.Date)
                return "La fecha del pago no puede ser anterior a la fecha de la consulta.";
        }

        var esCredito = nombreMetodo.Contains("Crédito", StringComparison.OrdinalIgnoreCase) ||
                        nombreMetodo.Contains("Credito", StringComparison.OrdinalIgnoreCase);

        if (!esCredito && solicitud.Cuotas.HasValue)
            return "Las cuotas solo aplican a pagos con tarjeta de crédito.";

        if (esCredito && solicitud.Cuotas.HasValue && !CuotasPermitidas.Contains(solicitud.Cuotas.Value))
            return "Las cuotas deben ser 1, 3, 6, 9 o 12.";

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
