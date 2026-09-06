using Microsoft.EntityFrameworkCore;
using Veterinaria.Domain.Comunes;
using Veterinaria.Domain.Dtos;
using Veterinaria.Domain.Entidades;
using Veterinaria.Infrastructure;

namespace Veterinaria.Controllers.Controladores;

public class EspecieControlador(ContextoVeterinaria context)
{
    public async Task<Resultado<IEnumerable<EspecieRespuestaDto>>> ObtenerTodosAsync()
    {
        try
        {
            var especies = await context.Especies
                .AsNoTracking()
                .Select(e => new EspecieRespuestaDto { Id = e.Id, Nombre = e.Nombre, Activo = e.Activo })
                .ToListAsync();

            return Resultado<IEnumerable<EspecieRespuestaDto>>.Exito(especies);
        }
        catch (Exception ex)
        {
            return Resultado<IEnumerable<EspecieRespuestaDto>>.Falla($"Error interno al obtener especies: {ex.Message}");
        }
    }

    public async Task<Resultado<EspecieRespuestaDto>> ObtenerPorIdAsync(long id)
    {
        try
        {
            if (id <= 0)
                return Resultado<EspecieRespuestaDto>.Falla("El identificador de la especie debe ser mayor a cero.");

            var especie = await context.Especies
                .AsNoTracking()
                .Where(e => e.Id == id)
                .Select(e => new EspecieRespuestaDto { Id = e.Id, Nombre = e.Nombre, Activo = e.Activo })
                .FirstOrDefaultAsync();

            if (especie is null)
                return Resultado<EspecieRespuestaDto>.Falla($"No se encontró la especie con ID {id}.");

            return Resultado<EspecieRespuestaDto>.Exito(especie);
        }
        catch (Exception ex)
        {
            return Resultado<EspecieRespuestaDto>.Falla($"Error interno al obtener la especie: {ex.Message}");
        }
    }

    public async Task<Resultado<long>> CrearAsync(EspecieSolicitudDto solicitud)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(solicitud.Nombre))
                return Resultado<long>.Falla("El nombre de la especie es obligatorio.");

            var nombreNormalizado = solicitud.Nombre.Trim();
            var existe = await context.Especies.AnyAsync(e => e.Nombre.ToLower() == nombreNormalizado.ToLower());
            if (existe)
                return Resultado<long>.Falla($"Ya existe una especie registrada con el nombre '{nombreNormalizado}'.");

            var entidad = new Especie { Nombre = nombreNormalizado, Activo = true };
            context.Especies.Add(entidad);
            await context.SaveChangesAsync();
            return Resultado<long>.Exito(entidad.Id, "Especie creada exitosamente.");
        }
        catch (Exception ex)
        {
            return Resultado<long>.Falla($"Error interno al crear la especie: {ex.Message}");
        }
    }

    public async Task<Resultado> ActualizarAsync(long id, EspecieSolicitudDto solicitud)
    {
        try
        {
            if (id <= 0)
                return Resultado.Falla("El identificador de la especie debe ser mayor a cero.");

            if (string.IsNullOrWhiteSpace(solicitud.Nombre))
                return Resultado.Falla("El nombre de la especie es obligatorio.");

            var entidad = await context.Especies.FirstOrDefaultAsync(e => e.Id == id);
            if (entidad is null)
                return Resultado.Falla($"No se encontró la especie con ID {id}.");

            var nombreNormalizado = solicitud.Nombre.Trim();
            var existeDuplicado = await context.Especies.AnyAsync(e => e.Id != id && e.Nombre.ToLower() == nombreNormalizado.ToLower());
            if (existeDuplicado)
                return Resultado.Falla($"Ya existe otra especie registrada con el nombre '{nombreNormalizado}'.");

            entidad.Nombre = nombreNormalizado;
            await context.SaveChangesAsync();
            return Resultado.Exito("Especie actualizada exitosamente.");
        }
        catch (Exception ex)
        {
            return Resultado.Falla($"Error interno al actualizar la especie: {ex.Message}");
        }
    }

    public async Task<Resultado> EliminarAsync(long id)
    {
        try
        {
            if (id <= 0)
                return Resultado.Falla("El identificador de la especie debe ser mayor a cero.");

            var entidad = await context.Especies.FirstOrDefaultAsync(e => e.Id == id);
            if (entidad is null)
                return Resultado.Falla($"No se encontró la especie con ID {id}.");

            entidad.Activo = false;
            await context.SaveChangesAsync();
            return Resultado.Exito("Especie eliminada exitosamente.");
        }
        catch (Exception ex)
        {
            return Resultado.Falla($"Error interno al eliminar la especie: {ex.Message}");
        }
    }
}
