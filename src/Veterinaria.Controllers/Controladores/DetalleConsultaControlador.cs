using Microsoft.EntityFrameworkCore;
using Veterinaria.Domain.Comunes;
using Veterinaria.Domain.Dtos;
using Veterinaria.Domain.Entidades;
using Veterinaria.Infrastructure;

namespace Veterinaria.Controllers.Controladores;

public class DetalleConsultaControlador(ContextoVeterinaria context)
{
    public async Task<Resultado<IEnumerable<DetalleConsultaRespuestaDto>>> ObtenerPorConsultaAsync(long idConsulta)
    {
        try
        {
            var items = await ConsultaBase()
                .Where(d => d.IdConsulta == idConsulta)
                .ToListAsync();

            return Resultado<IEnumerable<DetalleConsultaRespuestaDto>>.Exito(items);
        }
        catch (Exception ex)
        {
            return Resultado<IEnumerable<DetalleConsultaRespuestaDto>>.Falla($"Error al obtener detalles de la consulta: {ex.Message}");
        }
    }

    public async Task<Resultado<DetalleConsultaRespuestaDto>> ObtenerPorIdAsync(long id)
    {
        try
        {
            if (id <= 0)
                return Resultado<DetalleConsultaRespuestaDto>.Falla("El identificador debe ser mayor a cero.");

            var item = await ConsultaBase().FirstOrDefaultAsync(d => d.Id == id);
            if (item is null)
                return Resultado<DetalleConsultaRespuestaDto>.Falla($"No se encontró el detalle con ID {id}.");

            return Resultado<DetalleConsultaRespuestaDto>.Exito(item);
        }
        catch (Exception ex)
        {
            return Resultado<DetalleConsultaRespuestaDto>.Falla($"Error al obtener el detalle: {ex.Message}");
        }
    }

    public async Task<Resultado<long>> CrearAsync(DetalleConsultaSolicitudDto solicitud)
    {
        try
        {
            if (solicitud.IdConsulta <= 0)
                return Resultado<long>.Falla("La consulta es obligatoria.");

            if (solicitud.IdTratamiento <= 0)
                return Resultado<long>.Falla("El tratamiento es obligatorio.");

            if (solicitud.Cantidad <= 0)
                return Resultado<long>.Falla("La cantidad debe ser mayor a cero.");

            var consultaExiste = await context.Consultas.AnyAsync(c => c.Id == solicitud.IdConsulta);
            if (!consultaExiste)
                return Resultado<long>.Falla($"No existe la consulta con ID {solicitud.IdConsulta}.");

            var tratamiento = await context.Tratamientos.FirstOrDefaultAsync(t => t.Id == solicitud.IdTratamiento);
            if (tratamiento is null)
                return Resultado<long>.Falla($"No existe el tratamiento con ID {solicitud.IdTratamiento}.");

            var precioUnitario = solicitud.PrecioUnitario > 0 ? solicitud.PrecioUnitario : tratamiento.Precio;
            var subtotal = solicitud.Subtotal > 0 ? solicitud.Subtotal : (precioUnitario * solicitud.Cantidad);

            var entidad = new DetalleConsulta
            {
                IdConsulta = solicitud.IdConsulta,
                IdTratamiento = solicitud.IdTratamiento,
                Cantidad = solicitud.Cantidad,
                PrecioUnitario = precioUnitario,
                Subtotal = subtotal,
                Indicaciones = string.IsNullOrWhiteSpace(solicitud.Indicaciones) ? null : solicitud.Indicaciones.Trim(),
                Activo = true
            };

            context.DetalleConsultas.Add(entidad);
            await context.SaveChangesAsync();
            return Resultado<long>.Exito(entidad.Id, "Detalle de tratamiento agregado a la consulta.");
        }
        catch (Exception ex)
        {
            return Resultado<long>.Falla($"Error al agregar el detalle: {ex.Message}");
        }
    }

    public async Task<Resultado> ActualizarAsync(long id, DetalleConsultaSolicitudDto solicitud)
    {
        try
        {
            if (id <= 0)
                return Resultado.Falla("El identificador debe ser mayor a cero.");

            var entidad = await context.DetalleConsultas.FirstOrDefaultAsync(d => d.Id == id);
            if (entidad is null)
                return Resultado.Falla($"No se encontró el detalle con ID {id}.");

            var precioUnitario = solicitud.PrecioUnitario > 0 ? solicitud.PrecioUnitario : entidad.PrecioUnitario;
            var subtotal = solicitud.Subtotal > 0 ? solicitud.Subtotal : (precioUnitario * solicitud.Cantidad);

            entidad.IdTratamiento = solicitud.IdTratamiento;
            entidad.Cantidad = solicitud.Cantidad;
            entidad.PrecioUnitario = precioUnitario;
            entidad.Subtotal = subtotal;
            entidad.Indicaciones = string.IsNullOrWhiteSpace(solicitud.Indicaciones) ? null : solicitud.Indicaciones.Trim();

            await context.SaveChangesAsync();
            return Resultado.Exito("Detalle de consulta actualizado exitosamente.");
        }
        catch (Exception ex)
        {
            return Resultado.Falla($"Error al actualizar el detalle: {ex.Message}");
        }
    }

    public async Task<Resultado> EliminarAsync(long id)
    {
        try
        {
            if (id <= 0)
                return Resultado.Falla("El identificador debe ser mayor a cero.");

            var entidad = await context.DetalleConsultas.FirstOrDefaultAsync(d => d.Id == id);
            if (entidad is null)
                return Resultado.Falla($"No se encontró el detalle con ID {id}.");

            context.DetalleConsultas.Remove(entidad);
            await context.SaveChangesAsync();
            return Resultado.Exito("Detalle eliminado exitosamente.");
        }
        catch (Exception ex)
        {
            return Resultado.Falla($"Error al eliminar el detalle: {ex.Message}");
        }
    }

    private IQueryable<DetalleConsultaRespuestaDto> ConsultaBase() =>
        context.DetalleConsultas
            .AsNoTracking()
            .Select(d => new DetalleConsultaRespuestaDto
            {
                Id = d.Id,
                IdConsulta = d.IdConsulta,
                IdTratamiento = d.IdTratamiento,
                TipoTratamiento = d.Tratamiento != null ? d.Tratamiento.TipoTratamiento : string.Empty,
                DescripcionTratamiento = d.Tratamiento != null ? d.Tratamiento.Descripcion : string.Empty,
                Cantidad = d.Cantidad,
                PrecioUnitario = d.PrecioUnitario,
                Subtotal = d.Subtotal,
                Indicaciones = d.Indicaciones,
                Activo = d.Activo
            });
}
