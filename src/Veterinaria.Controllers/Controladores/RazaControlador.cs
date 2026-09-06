using Microsoft.EntityFrameworkCore;
using Veterinaria.Domain.Comunes;
using Veterinaria.Domain.Dtos;
using Veterinaria.Domain.Entidades;
using Veterinaria.Infrastructure;

namespace Veterinaria.Controllers.Controladores;

public class RazaControlador(ContextoVeterinaria context)
{
    public async Task<Resultado<IEnumerable<RazaRespuestaDto>>> ObtenerTodosAsync()
    {
        try
        {
            var razas = await ConsultaBase().ToListAsync();
            return Resultado<IEnumerable<RazaRespuestaDto>>.Exito(razas);
        }
        catch (Exception ex)
        {
            return Resultado<IEnumerable<RazaRespuestaDto>>.Falla($"Error interno al obtener razas: {ex.Message}");
        }
    }

    public async Task<Resultado<RazaRespuestaDto>> ObtenerPorIdAsync(long id)
    {
        try
        {
            if (id <= 0)
                return Resultado<RazaRespuestaDto>.Falla("El identificador de la raza debe ser mayor a cero.");

            var raza = await ConsultaBase().FirstOrDefaultAsync(r => r.Id == id);
            if (raza is null)
                return Resultado<RazaRespuestaDto>.Falla($"No se encontró la raza con ID {id}.");

            return Resultado<RazaRespuestaDto>.Exito(raza);
        }
        catch (Exception ex)
        {
            return Resultado<RazaRespuestaDto>.Falla($"Error interno al obtener la raza: {ex.Message}");
        }
    }

    public async Task<Resultado<long>> CrearAsync(RazaSolicitudDto solicitud)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(solicitud.Nombre))
                return Resultado<long>.Falla("El nombre de la raza es obligatorio.");

            if (solicitud.IdEspecie <= 0)
                return Resultado<long>.Falla("El identificador de la especie debe ser mayor a cero.");

            var especieExiste = await context.Especies.AnyAsync(e => e.Id == solicitud.IdEspecie);
            if (!especieExiste)
                return Resultado<long>.Falla($"No existe una especie registrada con ID {solicitud.IdEspecie}.");

            var nombreNormalizado = solicitud.Nombre.Trim();
            var existeDuplicado = await context.Razas
                .AnyAsync(r => r.IdEspecie == solicitud.IdEspecie && r.Nombre.ToLower() == nombreNormalizado.ToLower());

            if (existeDuplicado)
                return Resultado<long>.Falla($"Ya existe una raza con el nombre '{nombreNormalizado}' para la especie seleccionada.");

            var entidad = new Raza
            {
                Nombre = nombreNormalizado,
                IdEspecie = solicitud.IdEspecie,
                Activo = true
            };

            context.Razas.Add(entidad);
            await context.SaveChangesAsync();
            return Resultado<long>.Exito(entidad.Id, "Raza creada exitosamente.");
        }
        catch (Exception ex)
        {
            return Resultado<long>.Falla($"Error interno al crear la raza: {ex.Message}");
        }
    }

    public async Task<Resultado> ActualizarAsync(long id, RazaSolicitudDto solicitud)
    {
        try
        {
            if (id <= 0)
                return Resultado.Falla("El identificador de la raza debe ser mayor a cero.");

            if (string.IsNullOrWhiteSpace(solicitud.Nombre))
                return Resultado.Falla("El nombre de la raza es obligatorio.");

            if (solicitud.IdEspecie <= 0)
                return Resultado.Falla("El identificador de la especie debe ser mayor a cero.");

            var entidad = await context.Razas.FirstOrDefaultAsync(r => r.Id == id);
            if (entidad is null)
                return Resultado.Falla($"No se encontró la raza con ID {id}.");

            var especieExiste = await context.Especies.AnyAsync(e => e.Id == solicitud.IdEspecie);
            if (!especieExiste)
                return Resultado.Falla($"No existe una especie registrada con ID {solicitud.IdEspecie}.");

            var nombreNormalizado = solicitud.Nombre.Trim();
            var existeDuplicado = await context.Razas
                .AnyAsync(r => r.Id != id && r.IdEspecie == solicitud.IdEspecie && r.Nombre.ToLower() == nombreNormalizado.ToLower());

            if (existeDuplicado)
                return Resultado.Falla($"Ya existe otra raza con el nombre '{nombreNormalizado}' para la especie seleccionada.");

            entidad.Nombre = nombreNormalizado;
            entidad.IdEspecie = solicitud.IdEspecie;
            await context.SaveChangesAsync();
            return Resultado.Exito("Raza actualizada exitosamente.");
        }
        catch (Exception ex)
        {
            return Resultado.Falla($"Error interno al actualizar la raza: {ex.Message}");
        }
    }

    public async Task<Resultado> EliminarAsync(long id)
    {
        try
        {
            if (id <= 0)
                return Resultado.Falla("El identificador de la raza debe ser mayor a cero.");

            var entidad = await context.Razas.FirstOrDefaultAsync(r => r.Id == id);
            if (entidad is null)
                return Resultado.Falla($"No se encontró la raza con ID {id}.");

            entidad.Activo = false;
            await context.SaveChangesAsync();
            return Resultado.Exito("Raza eliminada exitosamente.");
        }
        catch (Exception ex)
        {
            return Resultado.Falla($"Error interno al eliminar la raza: {ex.Message}");
        }
    }

    private IQueryable<RazaRespuestaDto> ConsultaBase() =>
        context.Razas
            .AsNoTracking()
            .Select(r => new RazaRespuestaDto
            {
                Id = r.Id,
                Nombre = r.Nombre,
                IdEspecie = r.IdEspecie,
                NombreEspecie = r.Especie != null ? r.Especie.Nombre : string.Empty,
                Activo = r.Activo
            });
}
