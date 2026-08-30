using Microsoft.EntityFrameworkCore;
using Veterinaria.CrossCutting.Comunes;
using Veterinaria.Domain.Dtos;
using Veterinaria.Domain.Entidades;
using Veterinaria.Infrastructure;
using Veterinaria.Interfaces.Interfaces;

namespace Veterinaria.Services.Servicios;

/// <summary>
/// Servicio para la gestión de Consultas Clínicas Veterinarias.
/// </summary>
public class ConsultaService(VeterinariaDbContext context) : IConsultaService
{
    public async Task<Result<IEnumerable<ConsultaResponseDto>>> ObtenerTodosAsync()
    {
        var consultas = await context.Consultas
            .AsNoTracking()
            .Include(c => c.Mascota)
                .ThenInclude(m => m.Propietario)
            .Include(c => c.Veterinario)
            .Include(c => c.Tratamientos)
            .Select(c => new ConsultaResponseDto
            {
                Id = c.Id,
                IdMascota = c.IdMascota,
                NombreMascota = c.Mascota != null ? c.Mascota.Nombre : string.Empty,
                NombrePropietario = c.Mascota != null && c.Mascota.Propietario != null
                    ? $"{c.Mascota.Propietario.Nombre} {c.Mascota.Propietario.Apellido}".Trim()
                    : string.Empty,
                IdVeterinario = c.IdVeterinario,
                NombreVeterinario = c.Veterinario != null
                    ? $"{c.Veterinario.Nombre} {c.Veterinario.Apellido}".Trim()
                    : string.Empty,
                FechaHora = c.FechaHora,
                PesoKg = c.PesoKg,
                Temperatura = c.Temperatura,
                Diagnostico = c.Diagnostico,
                Observaciones = c.Observaciones,
                Activo = c.Activo,
                CantidadTratamientos = c.Tratamientos.Count
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
            .Include(c => c.Veterinario)
            .Include(c => c.Tratamientos)
            .Where(c => c.Id == id)
            .Select(c => new ConsultaResponseDto
            {
                Id = c.Id,
                IdMascota = c.IdMascota,
                NombreMascota = c.Mascota != null ? c.Mascota.Nombre : string.Empty,
                NombrePropietario = c.Mascota != null && c.Mascota.Propietario != null
                    ? $"{c.Mascota.Propietario.Nombre} {c.Mascota.Propietario.Apellido}".Trim()
                    : string.Empty,
                IdVeterinario = c.IdVeterinario,
                NombreVeterinario = c.Veterinario != null
                    ? $"{c.Veterinario.Nombre} {c.Veterinario.Apellido}".Trim()
                    : string.Empty,
                FechaHora = c.FechaHora,
                PesoKg = c.PesoKg,
                Temperatura = c.Temperatura,
                Diagnostico = c.Diagnostico,
                Observaciones = c.Observaciones,
                Activo = c.Activo,
                CantidadTratamientos = c.Tratamientos.Count
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

        if (request.IdVeterinario <= 0)
            return Result<long>.Falla("El identificador del veterinario debe ser mayor a cero.");

        if (string.IsNullOrWhiteSpace(request.Diagnostico))
            return Result<long>.Falla("El diagnóstico clínico es obligatorio.");

        if (request.PesoKg <= 0)
            return Result<long>.Falla("El peso de la mascota en kg debe ser mayor a cero.");

        if (request.Temperatura <= 0)
            return Result<long>.Falla("La temperatura corporal debe ser mayor a cero.");

        var mascotaExiste = await context.Mascotas.AnyAsync(m => m.Id == request.IdMascota);
        if (!mascotaExiste)
            return Result<long>.Falla($"No existe una mascota registrada con ID {request.IdMascota}.");

        var veterinarioExiste = await context.Usuarios.AnyAsync(u => u.Id == request.IdVeterinario);
        if (!veterinarioExiste)
            return Result<long>.Falla($"No existe un veterinario registrado con ID {request.IdVeterinario}.");

        var entidad = new Consulta
        {
            IdMascota = request.IdMascota,
            IdVeterinario = request.IdVeterinario,
            FechaHora = request.FechaHora == default ? DateTime.Now : request.FechaHora,
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

        if (request.IdVeterinario <= 0)
            return Result.Falla("El identificador del veterinario debe ser mayor a cero.");

        if (string.IsNullOrWhiteSpace(request.Diagnostico))
            return Result.Falla("El diagnóstico clínico es obligatorio.");

        if (request.PesoKg <= 0)
            return Result.Falla("El peso de la mascota en kg debe ser mayor a cero.");

        if (request.Temperatura <= 0)
            return Result.Falla("La temperatura corporal debe ser mayor a cero.");

        var entidad = await context.Consultas.FirstOrDefaultAsync(c => c.Id == id);
        if (entidad is null)
            return Result.Falla($"No se encontró la consulta con ID {id}.");

        var mascotaExiste = await context.Mascotas.AnyAsync(m => m.Id == request.IdMascota);
        if (!mascotaExiste)
            return Result.Falla($"No existe una mascota registrada con ID {request.IdMascota}.");

        var veterinarioExiste = await context.Usuarios.AnyAsync(u => u.Id == request.IdVeterinario);
        if (!veterinarioExiste)
            return Result.Falla($"No existe un veterinario registrado con ID {request.IdVeterinario}.");

        entidad.IdMascota = request.IdMascota;
        entidad.IdVeterinario = request.IdVeterinario;
        entidad.FechaHora = request.FechaHora == default ? entidad.FechaHora : request.FechaHora;
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
