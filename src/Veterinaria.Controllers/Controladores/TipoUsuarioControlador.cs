using Microsoft.EntityFrameworkCore;
using Veterinaria.Domain.Comunes;
using Veterinaria.Domain.Dtos;
using Veterinaria.Domain.Entidades;
using Veterinaria.Infrastructure;

namespace Veterinaria.Controllers.Controladores;

public class TipoUsuarioControlador(ContextoVeterinaria context)
{
    public async Task<Resultado<IEnumerable<TipoUsuarioRespuestaDto>>> ObtenerTodosAsync()
    {
        try
        {
            var list = await context.TiposUsuario
                .AsNoTracking()
                .Select(t => new TipoUsuarioRespuestaDto { Id = t.Id, Nombre = t.Nombre, Activo = t.Activo })
                .ToListAsync();

            return Resultado<IEnumerable<TipoUsuarioRespuestaDto>>.Exito(list);
        }
        catch (Exception ex)
        {
            return Resultado<IEnumerable<TipoUsuarioRespuestaDto>>.Falla($"Error interno al obtener tipos de usuario: {ex.Message}");
        }
    }

    public async Task<Resultado<TipoUsuarioRespuestaDto>> ObtenerPorIdAsync(long id)
    {
        try
        {
            if (id <= 0)
                return Resultado<TipoUsuarioRespuestaDto>.Falla("El identificador debe ser mayor a cero.");

            var item = await context.TiposUsuario
                .AsNoTracking()
                .Where(t => t.Id == id)
                .Select(t => new TipoUsuarioRespuestaDto { Id = t.Id, Nombre = t.Nombre, Activo = t.Activo })
                .FirstOrDefaultAsync();

            if (item is null)
                return Resultado<TipoUsuarioRespuestaDto>.Falla($"No se encontró el tipo de usuario con ID {id}.");

            return Resultado<TipoUsuarioRespuestaDto>.Exito(item);
        }
        catch (Exception ex)
        {
            return Resultado<TipoUsuarioRespuestaDto>.Falla($"Error interno al obtener el tipo de usuario: {ex.Message}");
        }
    }

    public async Task<Resultado<long>> CrearAsync(TipoUsuarioSolicitudDto solicitud)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(solicitud.Nombre))
                return Resultado<long>.Falla("El nombre del tipo de usuario es obligatorio.");

            var nombreNormalizado = solicitud.Nombre.Trim();
            var existe = await context.TiposUsuario.AnyAsync(t => t.Nombre.ToLower() == nombreNormalizado.ToLower());
            if (existe)
                return Resultado<long>.Falla($"Ya existe un tipo de usuario con el nombre '{nombreNormalizado}'.");

            var entidad = new TipoUsuario { Nombre = nombreNormalizado, Activo = true };
            context.TiposUsuario.Add(entidad);
            await context.SaveChangesAsync();
            return Resultado<long>.Exito(entidad.Id, "Tipo de usuario registrado exitosamente.");
        }
        catch (Exception ex)
        {
            return Resultado<long>.Falla($"Error interno al crear el tipo de usuario: {ex.Message}");
        }
    }

    public async Task<Resultado> ActualizarAsync(long id, TipoUsuarioSolicitudDto solicitud)
    {
        try
        {
            if (id <= 0)
                return Resultado.Falla("El identificador debe ser mayor a cero.");

            if (string.IsNullOrWhiteSpace(solicitud.Nombre))
                return Resultado.Falla("El nombre del tipo de usuario es obligatorio.");

            var entidad = await context.TiposUsuario.FirstOrDefaultAsync(t => t.Id == id);
            if (entidad is null)
                return Resultado.Falla($"No se encontró el tipo de usuario con ID {id}.");

            var nombreNormalizado = solicitud.Nombre.Trim();
            var existe = await context.TiposUsuario.AnyAsync(t => t.Id != id && t.Nombre.ToLower() == nombreNormalizado.ToLower());
            if (existe)
                return Resultado.Falla($"Ya existe otro tipo de usuario con el nombre '{nombreNormalizado}'.");

            entidad.Nombre = nombreNormalizado;
            await context.SaveChangesAsync();
            return Resultado.Exito("Tipo de usuario actualizado exitosamente.");
        }
        catch (Exception ex)
        {
            return Resultado.Falla($"Error interno al actualizar el tipo de usuario: {ex.Message}");
        }
    }

    public async Task<Resultado> EliminarAsync(long id)
    {
        try
        {
            if (id <= 0)
                return Resultado.Falla("El identificador debe ser mayor a cero.");

            var entidad = await context.TiposUsuario.FirstOrDefaultAsync(t => t.Id == id);
            if (entidad is null)
                return Resultado.Falla($"No se encontró el tipo de usuario con ID {id}.");

            entidad.Activo = false;
            await context.SaveChangesAsync();
            return Resultado.Exito("Tipo de usuario eliminado exitosamente.");
        }
        catch (Exception ex)
        {
            return Resultado.Falla($"Error interno al eliminar el tipo de usuario: {ex.Message}");
        }
    }
}
