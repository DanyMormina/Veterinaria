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

            entidad.IdMascota = solicitud.IdMascota;
            entidad.IdUsuario = solicitud.IdUsuario;
            entidad.FechaHora = solicitud.FechaHora == default ? entidad.FechaHora : solicitud.FechaHora;
            entidad.Motivo = string.IsNullOrWhiteSpace(solicitud.Motivo) ? null : solicitud.Motivo.Trim();
            entidad.PesoKg = solicitud.PesoKg;
            entidad.Temperatura = solicitud.Temperatura;
            entidad.Diagnostico = solicitud.Diagnostico.Trim();
            entidad.Observaciones = string.IsNullOrWhiteSpace(solicitud.Observaciones) ? null : solicitud.Observaciones.Trim();

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

        return null;
    }
}
