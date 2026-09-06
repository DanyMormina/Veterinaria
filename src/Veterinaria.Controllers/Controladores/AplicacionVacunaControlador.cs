using Microsoft.EntityFrameworkCore;
using Veterinaria.Domain.Comunes;
using Veterinaria.Domain.Dtos;
using Veterinaria.Domain.Entidades;
using Veterinaria.Infrastructure;

namespace Veterinaria.Controllers.Controladores;

public class AplicacionVacunaControlador(ContextoVeterinaria context)
{
    public async Task<Resultado<IEnumerable<AplicacionVacunaRespuestaDto>>> ObtenerPorConsultaAsync(long idConsulta)
    {
        try
        {
            var items = await ConsultaBase()
                .Where(a => a.IdConsulta == idConsulta)
                .ToListAsync();

            return Resultado<IEnumerable<AplicacionVacunaRespuestaDto>>.Exito(items);
        }
        catch (Exception ex)
        {
            return Resultado<IEnumerable<AplicacionVacunaRespuestaDto>>.Falla($"Error al obtener aplicaciones de vacunas: {ex.Message}");
        }
    }

    public async Task<Resultado<AplicacionVacunaRespuestaDto>> ObtenerPorIdAsync(long id)
    {
        try
        {
            if (id <= 0)
                return Resultado<AplicacionVacunaRespuestaDto>.Falla("El identificador debe ser mayor a cero.");

            var item = await ConsultaBase().FirstOrDefaultAsync(a => a.Id == id);
            if (item is null)
                return Resultado<AplicacionVacunaRespuestaDto>.Falla($"No se encontró la aplicación de vacuna con ID {id}.");

            return Resultado<AplicacionVacunaRespuestaDto>.Exito(item);
        }
        catch (Exception ex)
        {
            return Resultado<AplicacionVacunaRespuestaDto>.Falla($"Error al obtener el registro: {ex.Message}");
        }
    }

    public async Task<Resultado<long>> CrearAsync(AplicacionVacunaSolicitudDto solicitud)
    {
        try
        {
            if (solicitud.IdConsulta <= 0)
                return Resultado<long>.Falla("La consulta es obligatoria.");

            if (solicitud.IdVacuna <= 0)
                return Resultado<long>.Falla("La vacuna es obligatoria.");

            var consulta = await context.Consultas
                .Include(c => c.Mascota)
                    .ThenInclude(m => m.Raza)
                .FirstOrDefaultAsync(c => c.Id == solicitud.IdConsulta);

            if (consulta is null)
                return Resultado<long>.Falla($"No existe la consulta con ID {solicitud.IdConsulta}.");

            var vacuna = await context.Vacunas.FirstOrDefaultAsync(v => v.Id == solicitud.IdVacuna);
            if (vacuna is null)
                return Resultado<long>.Falla($"No existe la vacuna con ID {solicitud.IdVacuna}.");

            var idEspecieMascota = consulta.Mascota?.Raza?.IdEspecie ?? 0;
            if (idEspecieMascota <= 0)
                return Resultado<long>.Falla("No se pudo determinar la especie de la mascota.");

            if (vacuna.IdEspecie != idEspecieMascota)
                return Resultado<long>.Falla("La vacuna no corresponde a la especie de la mascota.");

            var fechaAplicacion = solicitud.FechaAplicacion == default ? DateTime.Today : solicitud.FechaAplicacion;
            var proximaDosis = solicitud.ProximaDosis ?? (vacuna.PeriodoMesesRecomendado > 0
                ? fechaAplicacion.AddMonths(vacuna.PeriodoMesesRecomendado)
                : null);

            var entidad = new AplicacionVacuna
            {
                IdConsulta = solicitud.IdConsulta,
                IdVacuna = solicitud.IdVacuna,
                FechaAplicacion = fechaAplicacion,
                ProximaDosis = proximaDosis,
                Observaciones = string.IsNullOrWhiteSpace(solicitud.Observaciones) ? null : solicitud.Observaciones.Trim()
            };

            context.AplicacionesVacuna.Add(entidad);
            await context.SaveChangesAsync();
            return Resultado<long>.Exito(entidad.Id, "Aplicación de vacuna registrada exitosamente.");
        }
        catch (Exception ex)
        {
            return Resultado<long>.Falla($"Error al registrar la aplicación de vacuna: {ex.Message}");
        }
    }

    public async Task<Resultado> ActualizarAsync(long id, AplicacionVacunaSolicitudDto solicitud)
    {
        try
        {
            if (id <= 0)
                return Resultado.Falla("El identificador debe ser mayor a cero.");

            var entidad = await context.AplicacionesVacuna.FirstOrDefaultAsync(a => a.Id == id);
            if (entidad is null)
                return Resultado.Falla($"No se encontró el registro con ID {id}.");

            if (solicitud.IdVacuna > 0)
            {
                var vacuna = await context.Vacunas.FirstOrDefaultAsync(v => v.Id == solicitud.IdVacuna);
                if (vacuna is null)
                    return Resultado.Falla($"No existe la vacuna con ID {solicitud.IdVacuna}.");

                var idEspecieMascota = await context.Consultas
                    .Where(c => c.Id == entidad.IdConsulta)
                    .Select(c => c.Mascota.Raza.IdEspecie)
                    .FirstOrDefaultAsync();

                if (vacuna.IdEspecie != idEspecieMascota)
                    return Resultado.Falla("La vacuna no corresponde a la especie de la mascota.");

                entidad.IdVacuna = solicitud.IdVacuna;
            }

            entidad.FechaAplicacion = solicitud.FechaAplicacion == default ? entidad.FechaAplicacion : solicitud.FechaAplicacion;
            entidad.ProximaDosis = solicitud.ProximaDosis ?? entidad.ProximaDosis;
            entidad.Observaciones = string.IsNullOrWhiteSpace(solicitud.Observaciones) ? null : solicitud.Observaciones.Trim();

            await context.SaveChangesAsync();
            return Resultado.Exito("Aplicación de vacuna actualizada exitosamente.");
        }
        catch (Exception ex)
        {
            return Resultado.Falla($"Error al actualizar la aplicación de vacuna: {ex.Message}");
        }
    }

    public async Task<Resultado> EliminarAsync(long id)
    {
        try
        {
            if (id <= 0)
                return Resultado.Falla("El identificador debe ser mayor a cero.");

            var entidad = await context.AplicacionesVacuna.FirstOrDefaultAsync(a => a.Id == id);
            if (entidad is null)
                return Resultado.Falla($"No se encontró el registro con ID {id}.");

            context.AplicacionesVacuna.Remove(entidad);
            await context.SaveChangesAsync();
            return Resultado.Exito("Aplicación de vacuna eliminada exitosamente.");
        }
        catch (Exception ex)
        {
            return Resultado.Falla($"Error al eliminar la aplicación de vacuna: {ex.Message}");
        }
    }

    private IQueryable<AplicacionVacunaRespuestaDto> ConsultaBase() =>
        context.AplicacionesVacuna
            .AsNoTracking()
            .Select(a => new AplicacionVacunaRespuestaDto
            {
                Id = a.Id,
                IdConsulta = a.IdConsulta,
                IdVacuna = a.IdVacuna,
                NombreVacuna = a.Vacuna != null ? a.Vacuna.Nombre : string.Empty,
                FechaAplicacion = a.FechaAplicacion,
                ProximaDosis = a.ProximaDosis,
                Observaciones = a.Observaciones,
                Precio = a.Vacuna != null ? a.Vacuna.Precio : 0
            });
}
