using Microsoft.EntityFrameworkCore;
using Veterinaria.Domain.Comunes;
using Veterinaria.Domain.Dtos;
using Veterinaria.Domain.Entidades;
using Veterinaria.Infrastructure;

namespace Veterinaria.Controllers.Controladores;

public class AplicacionVacunaControlador(ContextoVeterinaria context)
{
    public async Task<Resultado<IEnumerable<AplicacionVacunaRespuestaDto>>> ObtenerTodosAsync()
    {
        try
        {
            var items = await ConsultaBase()
                .OrderByDescending(a => a.FechaAplicacion)
                .ToListAsync();

            return Resultado<IEnumerable<AplicacionVacunaRespuestaDto>>.Exito(items);
        }
        catch (Exception ex)
        {
            return Resultado<IEnumerable<AplicacionVacunaRespuestaDto>>.Falla($"Error al obtener aplicaciones de vacunas: {ex.Message}");
        }
    }

    public async Task<Resultado<IEnumerable<AplicacionVacunaRespuestaDto>>> ObtenerPorMascotaAsync(long idMascota)
    {
        try
        {
            if (idMascota <= 0)
                return Resultado<IEnumerable<AplicacionVacunaRespuestaDto>>.Falla("El identificador de la mascota debe ser mayor a cero.");

            var items = await ConsultaBase()
                .Where(a => a.IdMascota == idMascota)
                .OrderByDescending(a => a.FechaAplicacion)
                .ToListAsync();

            return Resultado<IEnumerable<AplicacionVacunaRespuestaDto>>.Exito(items);
        }
        catch (Exception ex)
        {
            return Resultado<IEnumerable<AplicacionVacunaRespuestaDto>>.Falla($"Error al obtener aplicaciones de la mascota: {ex.Message}");
        }
    }

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

            var vacuna = await context.Vacunas.AsNoTracking().FirstOrDefaultAsync(v => v.Id == solicitud.IdVacuna);
            if (vacuna is null)
                return Resultado<long>.Falla($"No existe la vacuna con ID {solicitud.IdVacuna}.");

            var idEspecieMascota = consulta.Mascota?.Raza?.IdEspecie ?? 0;
            if (idEspecieMascota <= 0)
                return Resultado<long>.Falla("No se pudo determinar la especie de la mascota.");

            if (vacuna.IdEspecie != idEspecieMascota)
                return Resultado<long>.Falla("La vacuna no corresponde a la especie de la mascota.");

            var descuentaStock = solicitud.PrecioUnitario.HasValue;
            if (descuentaStock && vacuna.Stock < 1)
                return Resultado<long>.Falla($"No hay stock suficiente de esta vacuna. Stock actual: {vacuna.Stock}.");

            var fechaAplicacion = solicitud.FechaAplicacion == default ? DateTime.Today : solicitud.FechaAplicacion;
            var proximaDosis = solicitud.ProximaDosis ?? (vacuna.PeriodoMesesRecomendado > 0
                ? fechaAplicacion.AddMonths(vacuna.PeriodoMesesRecomendado)
                : null);

            var errorDatos = ValidarDatos(fechaAplicacion, proximaDosis, solicitud.PrecioUnitario, solicitud.Observaciones);
            if (errorDatos is not null)
                return Resultado<long>.Falla(errorDatos);

            // Si PrecioUnitario es null (vacuna previa/externa), se preserva null; si tiene valor, se persiste dicho valor.
            var precioUnitario = solicitud.PrecioUnitario;

            var strategy = context.Database.CreateExecutionStrategy();
            return await strategy.ExecuteAsync(async () =>
            {
                using var transaction = await context.Database.BeginTransactionAsync();
                try
                {
                    if (descuentaStock)
                    {
                        // Descuento atómico condicional en el motor de base de datos
                        var filasAfectadas = await context.Vacunas
                            .Where(v => v.Id == solicitud.IdVacuna && v.Stock >= 1)
                            .ExecuteUpdateAsync(s => s.SetProperty(v => v.Stock, v => v.Stock - 1));

                        if (filasAfectadas == 0)
                        {
                            await transaction.RollbackAsync();
                            return Resultado<long>.Falla("No hay stock suficiente de esta vacuna (puede haber sido consumida simultáneamente por otro usuario).");
                        }
                    }

                    var entidad = new AplicacionVacuna
                    {
                        IdConsulta = solicitud.IdConsulta,
                        IdVacuna = solicitud.IdVacuna,
                        FechaAplicacion = fechaAplicacion,
                        ProximaDosis = proximaDosis,
                        PrecioUnitario = precioUnitario,
                        Observaciones = string.IsNullOrWhiteSpace(solicitud.Observaciones) ? null : solicitud.Observaciones.Trim()
                    };

                    context.AplicacionesVacuna.Add(entidad);
                    await context.SaveChangesAsync();
                    await transaction.CommitAsync();

                    return Resultado<long>.Exito(entidad.Id, "Aplicación de vacuna registrada exitosamente.");
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    return Resultado<long>.Falla($"Error al registrar la aplicación de vacuna: {ex.Message}");
                }
            });
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

            var errorDatos = ValidarDatos(
                solicitud.FechaAplicacion == default ? entidad.FechaAplicacion : solicitud.FechaAplicacion,
                solicitud.ProximaDosis ?? entidad.ProximaDosis,
                solicitud.PrecioUnitario,
                solicitud.Observaciones);
            if (errorDatos is not null)
                return Resultado.Falla(errorDatos);

            var idVacunaAnterior = entidad.IdVacuna;
            var consumiaStock = entidad.PrecioUnitario.HasValue;
            var consumeStock = solicitud.PrecioUnitario.HasValue;
            var idVacunaNueva = solicitud.IdVacuna > 0 ? solicitud.IdVacuna : idVacunaAnterior;

            var strategy = context.Database.CreateExecutionStrategy();
            return await strategy.ExecuteAsync(async () =>
            {
                using var transaction = await context.Database.BeginTransactionAsync();
                try
                {
                    if (idVacunaNueva != idVacunaAnterior || consumiaStock != consumeStock)
                    {
                        var vacunaNueva = await context.Vacunas.AsNoTracking().FirstOrDefaultAsync(v => v.Id == idVacunaNueva);
                        if (vacunaNueva is null)
                        {
                            await transaction.RollbackAsync();
                            return Resultado.Falla($"No existe la vacuna con ID {idVacunaNueva}.");
                        }

                        var idEspecieMascota = await context.Consultas
                            .Where(c => c.Id == entidad.IdConsulta)
                            .Select(c => c.Mascota.Raza.IdEspecie)
                            .FirstOrDefaultAsync();

                        if (vacunaNueva.IdEspecie != idEspecieMascota)
                        {
                            await transaction.RollbackAsync();
                            return Resultado.Falla("La vacuna no corresponde a la especie de la mascota.");
                        }

                        if (idVacunaAnterior == idVacunaNueva)
                        {
                            if (!consumiaStock && consumeStock)
                            {
                                var filas = await context.Vacunas
                                    .Where(v => v.Id == idVacunaNueva && v.Stock >= 1)
                                    .ExecuteUpdateAsync(s => s.SetProperty(v => v.Stock, v => v.Stock - 1));

                                if (filas == 0)
                                {
                                    await transaction.RollbackAsync();
                                    return Resultado.Falla("No hay stock suficiente de esta vacuna (puede haber sido consumida simultáneamente por otro usuario).");
                                }
                            }
                            else if (consumiaStock && !consumeStock)
                            {
                                await context.Vacunas
                                    .Where(v => v.Id == idVacunaNueva)
                                    .ExecuteUpdateAsync(s => s.SetProperty(v => v.Stock, v => v.Stock + 1));
                            }
                        }
                        else
                        {
                            if (consumiaStock)
                            {
                                await context.Vacunas
                                    .Where(v => v.Id == idVacunaAnterior)
                                    .ExecuteUpdateAsync(s => s.SetProperty(v => v.Stock, v => v.Stock + 1));
                            }

                            if (consumeStock)
                            {
                                var filas = await context.Vacunas
                                    .Where(v => v.Id == idVacunaNueva && v.Stock >= 1)
                                    .ExecuteUpdateAsync(s => s.SetProperty(v => v.Stock, v => v.Stock - 1));

                                if (filas == 0)
                                {
                                    await transaction.RollbackAsync();
                                    return Resultado.Falla("No hay stock suficiente de la nueva vacuna seleccionada (puede haber sido consumida simultáneamente por otro usuario).");
                                }
                            }
                        }

                        entidad.IdVacuna = idVacunaNueva;
                    }

                    entidad.PrecioUnitario = solicitud.PrecioUnitario;
                    entidad.FechaAplicacion = solicitud.FechaAplicacion == default ? entidad.FechaAplicacion : solicitud.FechaAplicacion;
                    entidad.ProximaDosis = solicitud.ProximaDosis ?? entidad.ProximaDosis;
                    entidad.Observaciones = string.IsNullOrWhiteSpace(solicitud.Observaciones) ? null : solicitud.Observaciones.Trim();

                    await context.SaveChangesAsync();
                    await transaction.CommitAsync();

                    return Resultado.Exito("Aplicación de vacuna actualizada exitosamente.");
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    return Resultado.Falla($"Error al actualizar la aplicación de vacuna: {ex.Message}");
                }
            });
        }
        catch (Exception ex)
        {
            return Resultado.Falla($"Error interno al actualizar la aplicación de vacuna: {ex.Message}");
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

            var strategy = context.Database.CreateExecutionStrategy();
            return await strategy.ExecuteAsync(async () =>
            {
                using var transaction = await context.Database.BeginTransactionAsync();
                try
                {
                    if (entidad.PrecioUnitario.HasValue)
                    {
                        await context.Vacunas
                            .Where(v => v.Id == entidad.IdVacuna)
                            .ExecuteUpdateAsync(s => s.SetProperty(v => v.Stock, v => v.Stock + 1));
                    }

                    context.AplicacionesVacuna.Remove(entidad);
                    await context.SaveChangesAsync();
                    await transaction.CommitAsync();

                    return Resultado.Exito("Aplicación de vacuna eliminada exitosamente.");
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    return Resultado.Falla($"Error al eliminar la aplicación de vacuna: {ex.Message}");
                }
            });
        }
        catch (Exception ex)
        {
            return Resultado.Falla($"Error interno al eliminar la aplicación de vacuna: {ex.Message}");
        }
    }

    private static string? ValidarDatos(DateTime fechaAplicacion, DateTime? proximaDosis, decimal? precioUnitario, string? observaciones)
    {
        if (fechaAplicacion.Date > DateTime.Today)
            return "La fecha de aplicación no puede ser posterior a hoy.";

        if (proximaDosis.HasValue && proximaDosis.Value.Date < fechaAplicacion.Date)
            return "La fecha de la próxima dosis no puede ser anterior a la fecha de aplicación.";

        if (precioUnitario is < 0)
            return "El importe de la vacuna no puede ser negativo.";

        if (observaciones?.Trim().Length > 250)
            return "Las observaciones no pueden superar los 250 caracteres.";

        return null;
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
                IdMascota = a.Consulta != null ? a.Consulta.IdMascota : 0,
                NombreMascota = a.Consulta != null && a.Consulta.Mascota != null ? a.Consulta.Mascota.Nombre : string.Empty,
                FechaAplicacion = a.FechaAplicacion,
                ProximaDosis = a.ProximaDosis,
                Observaciones = a.Observaciones,
                PrecioUnitario = a.PrecioUnitario,
                Precio = a.PrecioUnitario
            });
}
