using Microsoft.EntityFrameworkCore;
using Veterinaria.Domain.Comunes;
using Veterinaria.Domain.Dtos;
using Veterinaria.Domain.Entidades;
using Veterinaria.Infrastructure;

namespace Veterinaria.Controllers.Controladores;

public class ConsultaController(VeterinariaDbContext context)
{
    public async Task<Result<IEnumerable<ConsultaResponseDto>>> ObtenerTodosAsync()
    {
        try
        {
            var consultas = await ConsultaBase().ToListAsync();
            return Result<IEnumerable<ConsultaResponseDto>>.Ok(consultas);
        }
        catch (Exception ex)
        {
            return Result<IEnumerable<ConsultaResponseDto>>.Falla($"Error interno al obtener consultas: {ex.Message}");
        }
    }

    public async Task<Result<ConsultaResponseDto>> ObtenerPorIdAsync(long id)
    {
        try
        {
            if (id <= 0)
                return Result<ConsultaResponseDto>.Falla("El identificador de la consulta debe ser mayor a cero.");

            var consulta = await ConsultaBase().FirstOrDefaultAsync(c => c.Id == id);
            if (consulta is null)
                return Result<ConsultaResponseDto>.Falla($"No se encontró la consulta con ID {id}.");

            return Result<ConsultaResponseDto>.Ok(consulta);
        }
        catch (Exception ex)
        {
            return Result<ConsultaResponseDto>.Falla($"Error interno al obtener la consulta: {ex.Message}");
        }
    }

    public async Task<Result<long>> CrearAsync(ConsultaRequestDto request)
    {
        try
        {
            var validacion = Validar(request);
            if (validacion is not null)
                return Result<long>.Falla(validacion);

            var mascotaExiste = await context.Mascotas.AnyAsync(m => m.Id == request.IdMascota);
            if (!mascotaExiste)
                return Result<long>.Falla($"No existe una mascota registrada con ID {request.IdMascota}.");

            var usuarioExiste = await context.Usuarios.AnyAsync(u => u.Id == request.IdUsuario);
            if (!usuarioExiste)
                return Result<long>.Falla($"No existe un usuario registrado con ID {request.IdUsuario}.");

            var entidad = new Consulta
            {
                IdMascota = request.IdMascota,
                IdUsuario = request.IdUsuario,
                FechaHora = request.FechaHora == default ? DateTime.Now : request.FechaHora,
                Motivo = string.IsNullOrWhiteSpace(request.Motivo) ? null : request.Motivo.Trim(),
                PesoKg = request.PesoKg,
                Temperatura = request.Temperatura,
                Diagnostico = request.Diagnostico.Trim(),
                Observaciones = string.IsNullOrWhiteSpace(request.Observaciones) ? null : request.Observaciones.Trim(),
                Activo = true
            };

            context.Consultas.Add(entidad);
            await context.SaveChangesAsync();
            return Result<long>.Ok(entidad.Id, "Consulta registrada exitosamente.");
        }
        catch (Exception ex)
        {
            return Result<long>.Falla($"Error interno al registrar la consulta: {ex.Message}");
        }
    }

    public async Task<Result> ActualizarAsync(long id, ConsultaRequestDto request)
    {
        try
        {
            if (id <= 0)
                return Result.Falla("El identificador de la consulta debe ser mayor a cero.");

            var validacion = Validar(request);
            if (validacion is not null)
                return Result.Falla(validacion);

            var entidad = await context.Consultas.FirstOrDefaultAsync(c => c.Id == id);
            if (entidad is null)
                return Result.Falla($"No se encontró la consulta con ID {id}.");

            var mascotaExiste = await context.Mascotas.AnyAsync(m => m.Id == request.IdMascota);
            if (!mascotaExiste)
                return Result.Falla($"No existe una mascota registrada con ID {request.IdMascota}.");

            var usuarioExiste = await context.Usuarios.AnyAsync(u => u.Id == request.IdUsuario);
            if (!usuarioExiste)
                return Result.Falla($"No existe un usuario registrado con ID {request.IdUsuario}.");

            entidad.IdMascota = request.IdMascota;
            entidad.IdUsuario = request.IdUsuario;
            entidad.FechaHora = request.FechaHora == default ? entidad.FechaHora : request.FechaHora;
            entidad.Motivo = string.IsNullOrWhiteSpace(request.Motivo) ? null : request.Motivo.Trim();
            entidad.PesoKg = request.PesoKg;
            entidad.Temperatura = request.Temperatura;
            entidad.Diagnostico = request.Diagnostico.Trim();
            entidad.Observaciones = string.IsNullOrWhiteSpace(request.Observaciones) ? null : request.Observaciones.Trim();

            await context.SaveChangesAsync();
            return Result.Ok("Consulta actualizada exitosamente.");
        }
        catch (Exception ex)
        {
            return Result.Falla($"Error interno al actualizar la consulta: {ex.Message}");
        }
    }

    public async Task<Result> EliminarAsync(long id)
    {
        try
        {
            if (id <= 0)
                return Result.Falla("El identificador de la consulta debe ser mayor a cero.");

            var entidad = await context.Consultas.FirstOrDefaultAsync(c => c.Id == id);
            if (entidad is null)
                return Result.Falla($"No se encontró la consulta con ID {id}.");

            entidad.Activo = false;
            await context.SaveChangesAsync();
            return Result.Ok("Consulta eliminada exitosamente.");
        }
        catch (Exception ex)
        {
            return Result.Falla($"Error interno al eliminar la consulta: {ex.Message}");
        }
    }

    private IQueryable<ConsultaResponseDto> ConsultaBase() =>
        context.Consultas
            .AsNoTracking()
            .Select(c => new ConsultaResponseDto
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

    private static string? Validar(ConsultaRequestDto request)
    {
        if (request.IdMascota <= 0)
            return "El identificador de la mascota debe ser mayor a cero.";

        if (request.IdUsuario <= 0)
            return "El identificador del profesional/usuario debe ser mayor a cero.";

        if (string.IsNullOrWhiteSpace(request.Diagnostico))
            return "El diagnóstico clínico es obligatorio.";

        return null;
    }
}
