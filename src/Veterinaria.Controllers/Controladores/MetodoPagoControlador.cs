using Microsoft.EntityFrameworkCore;
using Veterinaria.Domain.Comunes;
using Veterinaria.Domain.Dtos;
using Veterinaria.Domain.Entidades;
using Veterinaria.Infrastructure;

namespace Veterinaria.Controllers.Controladores;

public class MetodoPagoControlador(ContextoVeterinaria context)
{
    public async Task<Resultado<IEnumerable<MetodoPagoRespuestaDto>>> ObtenerTodosAsync()
    {
        try
        {
            var metodos = await context.MetodosPago
                .AsNoTracking()
                .Select(m => new MetodoPagoRespuestaDto { Id = m.Id, Nombre = m.Nombre, Activo = m.Activo })
                .ToListAsync();

            return Resultado<IEnumerable<MetodoPagoRespuestaDto>>.Exito(metodos);
        }
        catch (Exception ex)
        {
            return Resultado<IEnumerable<MetodoPagoRespuestaDto>>.Falla($"Error interno al obtener métodos de pago: {ex.Message}");
        }
    }

    public async Task<Resultado<MetodoPagoRespuestaDto>> ObtenerPorIdAsync(long id)
    {
        try
        {
            if (id <= 0)
                return Resultado<MetodoPagoRespuestaDto>.Falla("El identificador del método de pago debe ser mayor a cero.");

            var metodo = await context.MetodosPago
                .AsNoTracking()
                .Where(m => m.Id == id)
                .Select(m => new MetodoPagoRespuestaDto { Id = m.Id, Nombre = m.Nombre, Activo = m.Activo })
                .FirstOrDefaultAsync();

            if (metodo is null)
                return Resultado<MetodoPagoRespuestaDto>.Falla($"No se encontró el método de pago con ID {id}.");

            return Resultado<MetodoPagoRespuestaDto>.Exito(metodo);
        }
        catch (Exception ex)
        {
            return Resultado<MetodoPagoRespuestaDto>.Falla($"Error interno al obtener el método de pago: {ex.Message}");
        }
    }

    public async Task<Resultado<long>> CrearAsync(MetodoPagoSolicitudDto solicitud)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(solicitud.Nombre))
                return Resultado<long>.Falla("El nombre del método de pago es obligatorio.");

            var nombreNormalizado = solicitud.Nombre.Trim();
            var existe = await context.MetodosPago.AnyAsync(m => m.Nombre.ToLower() == nombreNormalizado.ToLower());
            if (existe)
                return Resultado<long>.Falla($"Ya existe un método de pago registrado con el nombre '{nombreNormalizado}'.");

            var entidad = new MetodoPago { Nombre = nombreNormalizado, Activo = true };
            context.MetodosPago.Add(entidad);
            await context.SaveChangesAsync();
            return Resultado<long>.Exito(entidad.Id, "Método de pago creado exitosamente.");
        }
        catch (Exception ex)
        {
            return Resultado<long>.Falla($"Error interno al crear el método de pago: {ex.Message}");
        }
    }

    public async Task<Resultado> ActualizarAsync(long id, MetodoPagoSolicitudDto solicitud)
    {
        try
        {
            if (id <= 0)
                return Resultado.Falla("El identificador del método de pago debe ser mayor a cero.");

            if (string.IsNullOrWhiteSpace(solicitud.Nombre))
                return Resultado.Falla("El nombre del método de pago es obligatorio.");

            var entidad = await context.MetodosPago.FirstOrDefaultAsync(m => m.Id == id);
            if (entidad is null)
                return Resultado.Falla($"No se encontró el método de pago con ID {id}.");

            var nombreNormalizado = solicitud.Nombre.Trim();
            var existeDuplicado = await context.MetodosPago.AnyAsync(m => m.Id != id && m.Nombre.ToLower() == nombreNormalizado.ToLower());
            if (existeDuplicado)
                return Resultado.Falla($"Ya existe otro método de pago registrado con el nombre '{nombreNormalizado}'.");

            entidad.Nombre = nombreNormalizado;
            await context.SaveChangesAsync();
            return Resultado.Exito("Método de pago actualizado exitosamente.");
        }
        catch (Exception ex)
        {
            return Resultado.Falla($"Error interno al actualizar el método de pago: {ex.Message}");
        }
    }

    public async Task<Resultado> EliminarAsync(long id)
    {
        try
        {
            if (id <= 0)
                return Resultado.Falla("El identificador del método de pago debe ser mayor a cero.");

            var entidad = await context.MetodosPago.FirstOrDefaultAsync(m => m.Id == id);
            if (entidad is null)
                return Resultado.Falla($"No se encontró el método de pago con ID {id}.");

            entidad.Activo = false;
            await context.SaveChangesAsync();
            return Resultado.Exito("Método de pago eliminado exitosamente.");
        }
        catch (Exception ex)
        {
            return Resultado.Falla($"Error interno al eliminar el método de pago: {ex.Message}");
        }
    }
}
