using Microsoft.EntityFrameworkCore;
using Veterinaria.CrossCutting.Comunes;
using Veterinaria.Domain.Dtos;
using Veterinaria.Domain.Entidades;
using Veterinaria.Infrastructure;
using Veterinaria.Interfaces.Interfaces;

namespace Veterinaria.Services.Servicios;

public class AplicacionVacunaService(VeterinariaDbContext context) : IAplicacionVacunaService
{
    public async Task<Result<IEnumerable<AplicacionVacunaResponseDto>>> ObtenerPorConsultaAsync(long idConsulta)
    {
        var items = await context.AplicacionesVacuna
            .AsNoTracking()
            .Include(a => a.Vacuna)
            .Where(a => a.IdConsulta == idConsulta)
            .Select(a => new AplicacionVacunaResponseDto
            {
                Id = a.Id,
                IdConsulta = a.IdConsulta,
                IdVacuna = a.IdVacuna,
                NombreVacuna = a.Vacuna != null ? a.Vacuna.Nombre : string.Empty,
                FechaAplicacion = a.FechaAplicacion,
                ProximaDosis = a.ProximaDosis,
                Observaciones = a.Observaciones,
                PrecioAplicado = a.PrecioAplicado,
                Activo = a.Activo
            })
            .ToListAsync();

        return Result<IEnumerable<AplicacionVacunaResponseDto>>.Ok(items);
    }

    public async Task<Result<AplicacionVacunaResponseDto>> ObtenerPorIdAsync(long id)
    {
        if (id <= 0)
            return Result<AplicacionVacunaResponseDto>.Falla("El identificador debe ser mayor a cero.");

        var item = await context.AplicacionesVacuna
            .AsNoTracking()
            .Include(a => a.Vacuna)
            .Where(a => a.Id == id)
            .Select(a => new AplicacionVacunaResponseDto
            {
                Id = a.Id,
                IdConsulta = a.IdConsulta,
                IdVacuna = a.IdVacuna,
                NombreVacuna = a.Vacuna != null ? a.Vacuna.Nombre : string.Empty,
                FechaAplicacion = a.FechaAplicacion,
                ProximaDosis = a.ProximaDosis,
                Observaciones = a.Observaciones,
                PrecioAplicado = a.PrecioAplicado,
                Activo = a.Activo
            })
            .FirstOrDefaultAsync();

        if (item is null)
            return Result<AplicacionVacunaResponseDto>.Falla($"No se encontró la aplicación de vacuna con ID {id}.");

        return Result<AplicacionVacunaResponseDto>.Ok(item);
    }

    public async Task<Result<long>> CrearAsync(AplicacionVacunaRequestDto request)
    {
        if (request.IdConsulta <= 0)
            return Result<long>.Falla("La consulta es obligatoria.");

        if (request.IdVacuna <= 0)
            return Result<long>.Falla("La vacuna es obligatoria.");

        var consultaExiste = await context.Consultas.AnyAsync(c => c.Id == request.IdConsulta);
        if (!consultaExiste)
            return Result<long>.Falla($"No existe la consulta con ID {request.IdConsulta}.");

        var vacuna = await context.Vacunas.FirstOrDefaultAsync(v => v.Id == request.IdVacuna);
        if (vacuna is null)
            return Result<long>.Falla($"No existe la vacuna con ID {request.IdVacuna}.");

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
            Observaciones = string.IsNullOrWhiteSpace(request.Observaciones) ? null : request.Observaciones.Trim(),
            PrecioAplicado = request.PrecioAplicado,
            Activo = true
        };

        context.AplicacionesVacuna.Add(entidad);
        await context.SaveChangesAsync();

        return Result<long>.Ok(entidad.Id, "Aplicación de vacuna registrada exitosamente.");
    }

    public async Task<Result> ActualizarAsync(long id, AplicacionVacunaRequestDto request)
    {
        if (id <= 0)
            return Result.Falla("El identificador debe ser mayor a cero.");

        var entidad = await context.AplicacionesVacuna.FirstOrDefaultAsync(a => a.Id == id);
        if (entidad is null)
            return Result.Falla($"No se encontró el registro con ID {id}.");

        entidad.IdVacuna = request.IdVacuna;
        entidad.FechaAplicacion = request.FechaAplicacion == default ? entidad.FechaAplicacion : request.FechaAplicacion;
        entidad.ProximaDosis = request.ProximaDosis ?? entidad.ProximaDosis;
        entidad.Observaciones = string.IsNullOrWhiteSpace(request.Observaciones) ? null : request.Observaciones.Trim();
        entidad.PrecioAplicado = request.PrecioAplicado;

        await context.SaveChangesAsync();

        return Result.Ok("Aplicación de vacuna actualizada exitosamente.");
    }

    public async Task<Result> EliminarAsync(long id)
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
}
