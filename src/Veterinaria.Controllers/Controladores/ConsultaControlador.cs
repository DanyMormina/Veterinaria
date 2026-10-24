using Microsoft.EntityFrameworkCore;
using Veterinaria.Domain.Comunes;
using Veterinaria.Domain.Dtos;
using Veterinaria.Domain.Entidades;
using Veterinaria.Infrastructure;

namespace Veterinaria.Controllers.Controladores;

public class ConsultaControlador(ContextoVeterinaria context)
{
    public async Task<Resultado<IEnumerable<ConsultaRespuestaDto>>> ObtenerTodosAsync()
    {
        try
        {
            var consultas = await ConsultaBase().ToListAsync();
            return Resultado<IEnumerable<ConsultaRespuestaDto>>.Exito(consultas);
        }
        catch (Exception ex)
        {
            return Resultado<IEnumerable<ConsultaRespuestaDto>>.Falla($"Error interno al obtener consultas: {ex.Message}");
        }
    }

    /// <summary>
    /// Obtiene las consultas activas asociadas a una mascota, ordenadas cronológicamente de la más reciente a la más antigua.
    /// </summary>
    public async Task<Resultado<IEnumerable<ConsultaRespuestaDto>>> ObtenerPorMascotaAsync(long idMascota)
    {
        return await ObtenerHistorialPorMascotaIdAsync(idMascota);
    }

    /// <summary>
    /// Obtiene el historial clínico completo de una mascota incluyendo tratamientos aplicados,
    /// ordenado cronológicamente de la consulta más reciente a la más antigua.
    /// </summary>
    public async Task<Resultado<IEnumerable<ConsultaRespuestaDto>>> ObtenerHistorialPorMascotaIdAsync(long idMascota)
    {
        try
        {
            if (idMascota <= 0)
                return Resultado<IEnumerable<ConsultaRespuestaDto>>.Falla("El identificador de la mascota debe ser mayor a cero.");

            var consultas = await context.Consultas
                .AsNoTracking()
                .Include(c => c.Mascota).ThenInclude(m => m.Propietario)
                .Include(c => c.Usuario)
                .Include(c => c.DetallesConsulta).ThenInclude(d => d.Tratamiento)
                .Where(c => c.IdMascota == idMascota && c.Activo)
                .OrderByDescending(c => c.FechaHora)
                .ToListAsync();

            var dtos = consultas.Select(c => new ConsultaRespuestaDto
            {
                Id = c.Id,
                IdMascota = c.IdMascota,
                NombreMascota = c.Mascota != null ? c.Mascota.Nombre : string.Empty,
                NombrePropietario = c.Mascota != null && c.Mascota.Propietario != null
                    ? $"{c.Mascota.Propietario.Nombre} {c.Mascota.Propietario.Apellido}".Trim()
                    : string.Empty,
                IdUsuario = c.IdUsuario,
                NombreUsuario = c.Usuario != null
                    ? $"{c.Usuario.Nombre} {c.Usuario.Apellido}".Trim()
                    : string.Empty,
                FechaHora = c.FechaHora,
                Motivo = c.Motivo,
                PesoKg = c.PesoKg,
                Temperatura = c.Temperatura,
                Diagnostico = c.Diagnostico,
                Observaciones = c.Observaciones,
                Importe = c.Importe,
                Activo = c.Activo,
                CantidadTratamientos = c.DetallesConsulta.Count,
                CantidadVacunas = c.AplicacionesVacuna.Count,
                CantidadPagos = c.Pagos.Count,
                Tratamiento = c.DetallesConsulta.Any()
                    ? string.Join(", ", c.DetallesConsulta.Where(d => d.Tratamiento != null).Select(d => d.Tratamiento.Descripcion))
                    : (c.Observaciones ?? "-"),
                ProximoControl = null
            }).ToList();

            return Resultado<IEnumerable<ConsultaRespuestaDto>>.Exito(dtos);
        }
        catch (Exception ex)
        {
            return Resultado<IEnumerable<ConsultaRespuestaDto>>.Falla($"Error interno al obtener consultas de la mascota: {ex.Message}");
        }
    }

    public async Task<Resultado<ConsultaRespuestaDto>> ObtenerPorIdAsync(long id)
    {
        try
        {
            if (id <= 0)
                return Resultado<ConsultaRespuestaDto>.Falla("El identificador de la consulta debe ser mayor a cero.");

            var consulta = await ConsultaBase().FirstOrDefaultAsync(c => c.Id == id);
            if (consulta is null)
                return Resultado<ConsultaRespuestaDto>.Falla($"No se encontró la consulta con ID {id}.");

            return Resultado<ConsultaRespuestaDto>.Exito(consulta);
        }
        catch (Exception ex)
        {
            return Resultado<ConsultaRespuestaDto>.Falla($"Error interno al obtener la consulta: {ex.Message}");
        }
    }

    public async Task<Resultado<long>> CrearAsync(ConsultaSolicitudDto solicitud)
    {
        try
        {
            var validacion = Validar(solicitud);
            if (validacion is not null)
                return Resultado<long>.Falla(validacion);

            var mascotaExiste = await context.Mascotas.AnyAsync(m => m.Id == solicitud.IdMascota);
            if (!mascotaExiste)
                return Resultado<long>.Falla($"No existe una mascota registrada con ID {solicitud.IdMascota}.");

            var usuarioExiste = await context.Usuarios.AnyAsync(u => u.Id == solicitud.IdUsuario);
            if (!usuarioExiste)
                return Resultado<long>.Falla($"No existe un usuario registrado con ID {solicitud.IdUsuario}.");

            var precio = await ObtenerPrecioPorMascotaAsync(solicitud.IdMascota);
            if (precio is null)
                return Resultado<long>.Falla("No hay un precio de consulta cargado para la especie de esta mascota.");

            var entidad = new Consulta
            {
                IdMascota = solicitud.IdMascota,
                IdUsuario = solicitud.IdUsuario,
                FechaHora = solicitud.FechaHora == default ? DateTime.Now : solicitud.FechaHora,
                Motivo = string.IsNullOrWhiteSpace(solicitud.Motivo) ? null : solicitud.Motivo.Trim(),
                PesoKg = solicitud.PesoKg,
                Temperatura = solicitud.Temperatura,
                Diagnostico = solicitud.Diagnostico.Trim(),
                Observaciones = string.IsNullOrWhiteSpace(solicitud.Observaciones) ? null : solicitud.Observaciones.Trim(),
                Importe = precio.Value,
                Activo = true
            };

            context.Consultas.Add(entidad);
            await context.SaveChangesAsync();
            return Resultado<long>.Exito(entidad.Id, "Consulta registrada exitosamente.");
        }
        catch (Exception ex)
        {
            return Resultado<long>.Falla($"Error interno al registrar la consulta: {ex.Message}");
        }
    }

    public async Task<Resultado> ActualizarAsync(long id, ConsultaSolicitudDto solicitud)
    {
        try
        {
            if (id <= 0)
                return Resultado.Falla("El identificador de la consulta debe ser mayor a cero.");

            var validacion = Validar(solicitud);
            if (validacion is not null)
                return Resultado.Falla(validacion);

            var entidad = await context.Consultas.FirstOrDefaultAsync(c => c.Id == id);
            if (entidad is null)
                return Resultado.Falla($"No se encontró la consulta con ID {id}.");

            var mascotaExiste = await context.Mascotas.AnyAsync(m => m.Id == solicitud.IdMascota);
            if (!mascotaExiste)
                return Resultado.Falla($"No existe una mascota registrada con ID {solicitud.IdMascota}.");

            var usuarioExiste = await context.Usuarios.AnyAsync(u => u.Id == solicitud.IdUsuario);
            if (!usuarioExiste)
                return Resultado.Falla($"No existe un usuario registrado con ID {solicitud.IdUsuario}.");

            // El importe queda fijado al registrar la consulta; solo se recalcula si cambia la mascota.
            if (entidad.IdMascota != solicitud.IdMascota)
            {
                var precio = await ObtenerPrecioPorMascotaAsync(solicitud.IdMascota);
                if (precio is null)
                    return Resultado.Falla("No hay un precio de consulta cargado para la especie de esta mascota.");
                entidad.Importe = precio.Value;
            }

            entidad.IdMascota = solicitud.IdMascota;
            entidad.IdUsuario = solicitud.IdUsuario;
            entidad.FechaHora = solicitud.FechaHora == default ? entidad.FechaHora : solicitud.FechaHora;
            entidad.Motivo = string.IsNullOrWhiteSpace(solicitud.Motivo) ? null : solicitud.Motivo.Trim();
            entidad.PesoKg = solicitud.PesoKg;
            entidad.Temperatura = solicitud.Temperatura;
            entidad.Diagnostico = solicitud.Diagnostico.Trim();
            entidad.Observaciones = string.IsNullOrWhiteSpace(solicitud.Observaciones) ? null : solicitud.Observaciones.Trim();

            if (!context.ChangeTracker.HasChanges())
                return Resultado.Falla("No se detectaron cambios para guardar.");

            await context.SaveChangesAsync();
            return Resultado.Exito("Consulta actualizada exitosamente.");
        }
        catch (Exception ex)
        {
            return Resultado.Falla($"Error interno al actualizar la consulta: {ex.Message}");
        }
    }

    public async Task<Resultado> EliminarAsync(long id)
    {
        try
        {
            if (id <= 0)
                return Resultado.Falla("El identificador de la consulta debe ser mayor a cero.");

            var entidad = await context.Consultas.FirstOrDefaultAsync(c => c.Id == id);
            if (entidad is null)
                return Resultado.Falla($"No se encontró la consulta con ID {id}.");

            entidad.Activo = false;
            await context.SaveChangesAsync();
            return Resultado.Exito("Consulta eliminada exitosamente.");
        }
        catch (Exception ex)
        {
            return Resultado.Falla($"Error interno al eliminar la consulta: {ex.Message}");
        }
    }

    private IQueryable<ConsultaRespuestaDto> ConsultaBase() =>
        context.Consultas
            .AsNoTracking()
            .Select(c => new ConsultaRespuestaDto
            {
                Id = c.Id,
                IdMascota = c.IdMascota,
                NombreMascota = c.Mascota != null ? c.Mascota.Nombre : string.Empty,
                NombrePropietario = c.Mascota != null && c.Mascota.Propietario != null
                    ? $"{c.Mascota.Propietario.Nombre} {c.Mascota.Propietario.Apellido}".Trim()
                    : string.Empty,
                IdUsuario = c.IdUsuario,
                NombreUsuario = c.Usuario != null
                    ? $"{c.Usuario.Nombre} {c.Usuario.Apellido}".Trim()
                    : string.Empty,
                FechaHora = c.FechaHora,
                Motivo = c.Motivo,
                PesoKg = c.PesoKg,
                Temperatura = c.Temperatura,
                Diagnostico = c.Diagnostico,
                Observaciones = c.Observaciones,
                Importe = c.Importe,
                Activo = c.Activo,
                CantidadTratamientos = c.DetallesConsulta.Count,
                CantidadVacunas = c.AplicacionesVacuna.Count,
                CantidadPagos = c.Pagos.Count
            });

    private static string? Validar(ConsultaSolicitudDto solicitud)
    {
        if (solicitud.IdMascota <= 0)
            return "El identificador de la mascota debe ser mayor a cero.";

        if (solicitud.IdUsuario <= 0)
            return "El identificador del profesional/usuario debe ser mayor a cero.";

        if (string.IsNullOrWhiteSpace(solicitud.Diagnostico))
            return "El diagnóstico clínico es obligatorio.";

        if (solicitud.Motivo?.Trim().Length > 250)
            return "El motivo de la consulta no puede superar los 250 caracteres.";

        if (solicitud.PesoKg.HasValue && (solicitud.PesoKg.Value <= 0 || solicitud.PesoKg.Value > 150))
            return "El peso debe ser mayor a 0 y no superar los 150 kg.";

        if (solicitud.Temperatura.HasValue && (solicitud.Temperatura.Value < 34 || solicitud.Temperatura.Value > 44))
            return "La temperatura debe estar entre 34 y 44 °C.";

        return null;
    }

    /// <summary>
    /// Devuelve el precio fijo de la consulta según la especie de la mascota.
    /// </summary>
    public async Task<Resultado<decimal>> ObtenerPrecioConsultaAsync(long idMascota)
    {
        try
        {
            var precio = await ObtenerPrecioPorMascotaAsync(idMascota);
            return precio is null
                ? Resultado<decimal>.Falla("No hay un precio de consulta cargado para la especie de esta mascota.")
                : Resultado<decimal>.Exito(precio.Value);
        }
        catch (Exception ex)
        {
            return Resultado<decimal>.Falla($"Error interno al obtener el precio de la consulta: {ex.Message}");
        }
    }

    private async Task<decimal?> ObtenerPrecioPorMascotaAsync(long idMascota)
    {
        var idEspecie = await context.Mascotas
            .IgnoreQueryFilters()
            .Where(m => m.Id == idMascota)
            .Select(m => (long?)m.Raza.IdEspecie)
            .FirstOrDefaultAsync();
        if (idEspecie is null)
            return null;

        return await context.PreciosConsulta
            .Where(p => p.IdEspecie == idEspecie.Value)
            .Select(p => (decimal?)p.Precio)
            .FirstOrDefaultAsync();
    }
}

/// <summary>
/// Alias de compatibilidad para ConsultaControlador.
/// </summary>
public class ConsultaController(ContextoVeterinaria context) : ConsultaControlador(context);

