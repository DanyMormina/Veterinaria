using Microsoft.EntityFrameworkCore;
using Veterinaria.Domain.Comunes;
using Veterinaria.Domain.Dtos;
using Veterinaria.Domain.Entidades;
using Veterinaria.Infrastructure;

namespace Veterinaria.Controllers.Controladores;

public class AplicacionVacunaController(VeterinariaDbContext context)
{
    public async Task<Result<IEnumerable<AplicacionVacunaResponseDto>>> ObtenerPorConsultaAsync(long idConsulta)
    {
        try
        {
            var items = await ConsultaBase()
                .Where(a => a.IdConsulta == idConsulta)
                .ToListAsync();

            return Result<IEnumerable<AplicacionVacunaResponseDto>>.Ok(items);
        }
        catch (Exception ex)
        {
            return Result<IEnumerable<AplicacionVacunaResponseDto>>.Falla($"Error al obtener aplicaciones de vacunas: {ex.Message}");
        }
    }

    public async Task<Result<AplicacionVacunaResponseDto>> ObtenerPorIdAsync(long id)
    {
        try
        {
            if (id <= 0)
                return Result<AplicacionVacunaResponseDto>.Falla("El identificador debe ser mayor a cero.");

            var item = await ConsultaBase().FirstOrDefaultAsync(a => a.Id == id);
            if (item is null)
                return Result<AplicacionVacunaResponseDto>.Falla($"No se encontró la aplicación de vacuna con ID {id}.");

            return Result<AplicacionVacunaResponseDto>.Ok(item);
        }
        catch (Exception ex)
        {
            return Result<AplicacionVacunaResponseDto>.Falla($"Error al obtener el registro: {ex.Message}");
        }
    }

    public async Task<Result<long>> CrearAsync(AplicacionVacunaRequestDto request)
    {
        try
        {
            if (request.IdConsulta <= 0)
                return Result<long>.Falla("La consulta es obligatoria.");

            if (request.IdVacuna <= 0)
                return Result<long>.Falla("La vacuna es obligatoria.");

            var consulta = await context.Consultas
                .Include(c => c.Mascota)
                    .ThenInclude(m => m.Raza)
                .FirstOrDefaultAsync(c => c.Id == request.IdConsulta);

            if (consulta is null)
                return Result<long>.Falla($"No existe la consulta con ID {request.IdConsulta}.");

            var vacuna = await context.Vacunas.FirstOrDefaultAsync(v => v.Id == request.IdVacuna);
            if (vacuna is null)
                return Result<long>.Falla($"No existe la vacuna con ID {request.IdVacuna}.");

            var idEspecieMascota = consulta.Mascota?.Raza?.IdEspecie ?? 0;
            if (idEspecieMascota <= 0)
                return Result<long>.Falla("No se pudo determinar la especie de la mascota.");

            if (vacuna.IdEspecie != idEspecieMascota)
                return Result<long>.Falla("La vacuna no corresponde a la especie de la mascota.");

            var fechaAplicacion = request.FechaAplicacion == default ? DateTime.Today : request.FechaAplicacion;
            var proximaDosis = request.ProximaDosis ?? (vacuna.PeriodoMesesRecomendado > 0
                ? fechaAplicacion.AddMonths(vacuna.PeriodoMesesRecomendado)
                : null);

            var entidad = new AplicacionVacuna
            {
                IdConsulta = request.IdConsulta,
                IdVacuna = request.IdVacuna,
                FechaAplicacion = fechaAplicacion,
                ProximaDosis = proximaDosis,
                Observaciones = string.IsNullOrWhiteSpace(request.Observaciones) ? null : request.Observaciones.Trim()
            };

            context.AplicacionesVacuna.Add(entidad);
            await context.SaveChangesAsync();
            return Result<long>.Ok(entidad.Id, "Aplicación de vacuna registrada exitosamente.");
        }
        catch (Exception ex)
        {
            return Result<long>.Falla($"Error al registrar la aplicación de vacuna: {ex.Message}");
        }
    }

    public async Task<Result> ActualizarAsync(long id, AplicacionVacunaRequestDto request)
    {
        try
        {
            if (id <= 0)
                return Result.Falla("El identificador debe ser mayor a cero.");

            var entidad = await context.AplicacionesVacuna.FirstOrDefaultAsync(a => a.Id == id);
            if (entidad is null)
                return Result.Falla($"No se encontró el registro con ID {id}.");

            if (request.IdVacuna > 0)
            {
                var vacuna = await context.Vacunas.FirstOrDefaultAsync(v => v.Id == request.IdVacuna);
                if (vacuna is null)
                    return Result.Falla($"No existe la vacuna con ID {request.IdVacuna}.");

                var idEspecieMascota = await context.Consultas
                    .Where(c => c.Id == entidad.IdConsulta)
                    .Select(c => c.Mascota.Raza.IdEspecie)
                    .FirstOrDefaultAsync();

                if (vacuna.IdEspecie != idEspecieMascota)
                    return Result.Falla("La vacuna no corresponde a la especie de la mascota.");

                entidad.IdVacuna = request.IdVacuna;
            }

            entidad.FechaAplicacion = request.FechaAplicacion == default ? entidad.FechaAplicacion : request.FechaAplicacion;
            entidad.ProximaDosis = request.ProximaDosis ?? entidad.ProximaDosis;
            entidad.Observaciones = string.IsNullOrWhiteSpace(request.Observaciones) ? null : request.Observaciones.Trim();

            await context.SaveChangesAsync();
            return Result.Ok("Aplicación de vacuna actualizada exitosamente.");
        }
        catch (Exception ex)
        {
            return Result.Falla($"Error al actualizar la aplicación de vacuna: {ex.Message}");
        }
    }

    public async Task<Result> EliminarAsync(long id)
    {
        try
        {
            if (id <= 0)
                return Result.Falla("El identificador debe ser mayor a cero.");

            var entidad = await context.AplicacionesVacuna.FirstOrDefaultAsync(a => a.Id == id);
            if (entidad is null)
                return Result.Falla($"No se encontró el registro con ID {id}.");

            context.AplicacionesVacuna.Remove(entidad);
            await context.SaveChangesAsync();
            return Result.Ok("Aplicación de vacuna eliminada exitosamente.");
        }
        catch (Exception ex)
        {
            return Result.Falla($"Error al eliminar la aplicación de vacuna: {ex.Message}");
        }
    }

    private IQueryable<AplicacionVacunaResponseDto> ConsultaBase() =>
        context.AplicacionesVacuna
            .AsNoTracking()
            .Select(a => new AplicacionVacunaResponseDto
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
