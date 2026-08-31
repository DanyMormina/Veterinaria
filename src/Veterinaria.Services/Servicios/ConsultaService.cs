using Microsoft.EntityFrameworkCore;
using Veterinaria.CrossCutting.Comunes;
using Veterinaria.Domain.Dtos;
using Veterinaria.Domain.Entidades;
using Veterinaria.Infrastructure;
using Veterinaria.Interfaces.Interfaces;

namespace Veterinaria.Services.Servicios;

/// <summary>
/// Servicio para la gestión de Consultas Clínicas Veterinarias según DER v2.
/// </summary>
public class ConsultaService(VeterinariaDbContext context) : IConsultaService
{
    public async Task<Result<IEnumerable<ConsultaResponseDto>>> ObtenerTodosAsync()
    {
        var consultas = await context.Consultas
            .AsNoTracking()
            .Include(c => c.Mascota)
                .ThenInclude(m => m.Propietario)
            .Include(c => c.Usuario)
            .Include(c => c.DetallesConsulta)
            .Include(c => c.AplicacionesVacuna)
            .Include(c => c.Pagos)
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
            })
            .ToListAsync();

        return Result<IEnumerable<ConsultaResponseDto>>.Ok(consultas);
    }

    public async Task<Result<ConsultaResponseDto>> ObtenerPorIdAsync(long id)
    {
        if (id <= 0)
            return Result<ConsultaResponseDto>.Falla("El identificador de la consulta debe ser mayor a cero.");

        var consulta = await context.Consultas
            .AsNoTracking()
            .Include(c => c.Mascota)
                .ThenInclude(m => m.Propietario)
            .Include(c => c.Usuario)
            .Include(c => c.DetallesConsulta)
            .Include(c => c.AplicacionesVacuna)
            .Include(c => c.Pagos)
            .Where(c => c.Id == id)
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
            })
            .FirstOrDefaultAsync();

        if (consulta is null)
            return Result<ConsultaResponseDto>.Falla($"No se encontró la consulta con ID {id}.");

        return Result<ConsultaResponseDto>.Ok(consulta);
    }

    public async Task<Result<long>> CrearAsync(ConsultaRequestDto request)
    {
        if (request.IdMascota <= 0)
            return Result<long>.Falla("El identificador de la mascota debe ser mayor a cero.");

        if (request.IdUsuario <= 0)
            return Result<long>.Falla("El identificador del profesional/usuario debe ser mayor a cero.");

        if (string.IsNullOrWhiteSpace(request.Diagnostico))
            return Result<long>.Falla("El diagnóstico clínico es obligatorio.");

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

    public async Task<Result> ActualizarAsync(long id, ConsultaRequestDto request)
    {
        if (id <= 0)
            return Result.Falla("El identificador de la consulta debe ser mayor a cero.");

        if (request.IdMascota <= 0)
            return Result.Falla("El identificador de la mascota debe ser mayor a cero.");

        if (request.IdUsuario <= 0)
            return Result.Falla("El identificador del usuario/profesional debe ser mayor a cero.");

        if (string.IsNullOrWhiteSpace(request.Diagnostico))
            return Result.Falla("El diagnóstico clínico es obligatorio.");

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

    public async Task<Result> EliminarAsync(long id)
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
}
