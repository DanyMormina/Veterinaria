using Microsoft.EntityFrameworkCore;
using Veterinaria.Domain.Comunes;
using Veterinaria.Domain.Dtos;
using Veterinaria.Domain.Entidades;
using Veterinaria.Infrastructure;

namespace Veterinaria.Controllers.Controladores;

public class VacunaControlador(ContextoVeterinaria context)
{
    public async Task<Resultado<IEnumerable<VacunaRespuestaDto>>> ObtenerTodosAsync()
    {
        try
        {
            var vacunas = await ConsultaBase().ToListAsync();
            return Resultado<IEnumerable<VacunaRespuestaDto>>.Exito(vacunas);
        }
        catch (Exception ex)
        {
            return Resultado<IEnumerable<VacunaRespuestaDto>>.Falla($"Error interno al obtener vacunas: {ex.Message}");
        }
    }

    public async Task<Resultado<IEnumerable<VacunaRespuestaDto>>> ObtenerPorEspecieAsync(long idEspecie)
    {
        try
        {
            if (idEspecie <= 0)
                return Resultado<IEnumerable<VacunaRespuestaDto>>.Falla("El identificador de la especie debe ser mayor a cero.");

            var vacunas = await ConsultaBase()
                .Where(v => v.IdEspecie == idEspecie)
                .ToListAsync();

            return Resultado<IEnumerable<VacunaRespuestaDto>>.Exito(vacunas);
        }
        catch (Exception ex)
        {
            return Resultado<IEnumerable<VacunaRespuestaDto>>.Falla($"Error interno al obtener vacunas por especie: {ex.Message}");
        }
    }

    public async Task<Resultado<VacunaRespuestaDto>> ObtenerPorIdAsync(long id)
    {
        try
        {
            if (id <= 0)
                return Resultado<VacunaRespuestaDto>.Falla("El identificador de la vacuna debe ser mayor a cero.");

            var vacuna = await ConsultaBase().FirstOrDefaultAsync(v => v.Id == id);
            if (vacuna is null)
                return Resultado<VacunaRespuestaDto>.Falla($"No se encontró la vacuna con ID {id}.");

            return Resultado<VacunaRespuestaDto>.Exito(vacuna);
        }
        catch (Exception ex)
        {
            return Resultado<VacunaRespuestaDto>.Falla($"Error interno al obtener la vacuna: {ex.Message}");
        }
    }

    public async Task<Resultado<long>> CrearAsync(VacunaSolicitudDto solicitud)
    {
        try
        {
            var validacion = Validar(solicitud);
            if (validacion is not null)
                return Resultado<long>.Falla(validacion);

            var especieExiste = await context.Especies.AnyAsync(e => e.Id == solicitud.IdEspecie);
            if (!especieExiste)
                return Resultado<long>.Falla($"No existe una especie registrada con ID {solicitud.IdEspecie}.");

            var nombreNormalizado = solicitud.Nombre.Trim();
            var existe = await context.Vacunas.AnyAsync(v =>
                v.IdEspecie == solicitud.IdEspecie &&
                v.Nombre.ToLower() == nombreNormalizado.ToLower());

            if (existe)
                return Resultado<long>.Falla($"Ya existe la vacuna '{nombreNormalizado}' para esa especie.");

            var entidad = new Vacuna
            {
                IdEspecie = solicitud.IdEspecie,
                Nombre = nombreNormalizado,
                PeriodoMesesRecomendado = solicitud.PeriodoMesesRecomendado,
                Precio = solicitud.Precio,
                Activo = true
            };

            context.Vacunas.Add(entidad);
            await context.SaveChangesAsync();
            return Resultado<long>.Exito(entidad.Id, "Vacuna creada exitosamente.");
        }
        catch (Exception ex)
        {
            return Resultado<long>.Falla($"Error interno al crear la vacuna: {ex.Message}");
        }
    }

    public async Task<Resultado> ActualizarAsync(long id, VacunaSolicitudDto solicitud)
    {
        try
        {
            if (id <= 0)
                return Resultado.Falla("El identificador de la vacuna debe ser mayor a cero.");

            var validacion = Validar(solicitud);
            if (validacion is not null)
                return Resultado.Falla(validacion);

            var entidad = await context.Vacunas.FirstOrDefaultAsync(v => v.Id == id);
            if (entidad is null)
                return Resultado.Falla($"No se encontró la vacuna con ID {id}.");

            var especieExiste = await context.Especies.AnyAsync(e => e.Id == solicitud.IdEspecie);
            if (!especieExiste)
                return Resultado.Falla($"No existe una especie registrada con ID {solicitud.IdEspecie}.");

            var nombreNormalizado = solicitud.Nombre.Trim();
            var existeDuplicado = await context.Vacunas.AnyAsync(v =>
                v.Id != id &&
                v.IdEspecie == solicitud.IdEspecie &&
                v.Nombre.ToLower() == nombreNormalizado.ToLower());

            if (existeDuplicado)
                return Resultado.Falla($"Ya existe otra vacuna '{nombreNormalizado}' para esa especie.");

            entidad.IdEspecie = solicitud.IdEspecie;
            entidad.Nombre = nombreNormalizado;
            entidad.PeriodoMesesRecomendado = solicitud.PeriodoMesesRecomendado;
            entidad.Precio = solicitud.Precio;
            await context.SaveChangesAsync();

            return Resultado.Exito("Vacuna actualizada exitosamente.");
        }
        catch (Exception ex)
        {
            return Resultado.Falla($"Error interno al actualizar la vacuna: {ex.Message}");
        }
    }

    public async Task<Resultado> EliminarAsync(long id)
    {
        try
        {
            if (id <= 0)
                return Resultado.Falla("El identificador de la vacuna debe ser mayor a cero.");

            var entidad = await context.Vacunas.FirstOrDefaultAsync(v => v.Id == id);
            if (entidad is null)
                return Resultado.Falla($"No se encontró la vacuna con ID {id}.");

            entidad.Activo = false;
            await context.SaveChangesAsync();
            return Resultado.Exito("Vacuna eliminada exitosamente.");
        }
        catch (Exception ex)
        {
            return Resultado.Falla($"Error interno al eliminar la vacuna: {ex.Message}");
        }
    }

    private IQueryable<VacunaRespuestaDto> ConsultaBase() =>
        context.Vacunas
            .AsNoTracking()
            .Select(v => new VacunaRespuestaDto
            {
                Id = v.Id,
                IdEspecie = v.IdEspecie,
                NombreEspecie = v.Especie != null ? v.Especie.Nombre : string.Empty,
                Nombre = v.Nombre,
                PeriodoMesesRecomendado = v.PeriodoMesesRecomendado,
                Precio = v.Precio,
                Activo = v.Activo
            });

    private static string? Validar(VacunaSolicitudDto solicitud)
    {
        if (solicitud.IdEspecie <= 0)
            return "Debe indicar la especie de la vacuna.";

        if (string.IsNullOrWhiteSpace(solicitud.Nombre))
            return "El nombre de la vacuna es obligatorio.";

        if (solicitud.PeriodoMesesRecomendado <= 0)
            return "El período recomendado en meses debe ser mayor a cero.";

        if (solicitud.Precio < 0)
            return "El precio de la vacuna no puede ser negativo.";

        return null;
    }
}
