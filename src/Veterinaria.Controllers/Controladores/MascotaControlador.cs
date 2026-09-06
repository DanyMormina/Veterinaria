using Microsoft.EntityFrameworkCore;
using Veterinaria.Domain.Comunes;
using Veterinaria.Domain.Dtos;
using Veterinaria.Domain.Entidades;
using Veterinaria.Infrastructure;

namespace Veterinaria.Controllers.Controladores;

public class MascotaControlador(ContextoVeterinaria context)
{
    public async Task<Resultado<IEnumerable<MascotaRespuestaDto>>> ObtenerTodosAsync()
    {
        try
        {
            var mascotas = await ConsultaBase().ToListAsync();
            return Resultado<IEnumerable<MascotaRespuestaDto>>.Exito(mascotas);
        }
        catch (Exception ex)
        {
            return Resultado<IEnumerable<MascotaRespuestaDto>>.Falla($"Error interno al obtener mascotas: {ex.Message}");
        }
    }

    public async Task<Resultado<MascotaRespuestaDto>> ObtenerPorIdAsync(long id)
    {
        try
        {
            if (id <= 0)
                return Resultado<MascotaRespuestaDto>.Falla("El identificador de la mascota debe ser mayor a cero.");

            var mascota = await ConsultaBase().FirstOrDefaultAsync(m => m.Id == id);
            if (mascota is null)
                return Resultado<MascotaRespuestaDto>.Falla($"No se encontró la mascota con ID {id}.");

            return Resultado<MascotaRespuestaDto>.Exito(mascota);
        }
        catch (Exception ex)
        {
            return Resultado<MascotaRespuestaDto>.Falla($"Error interno al obtener la mascota: {ex.Message}");
        }
    }

    public async Task<Resultado<IEnumerable<MascotaRespuestaDto>>> ObtenerPorPropietarioAsync(long idPropietario)
    {
        try
        {
            if (idPropietario <= 0)
                return Resultado<IEnumerable<MascotaRespuestaDto>>.Falla("El identificador del propietario debe ser mayor a cero.");

            var mascotas = await ConsultaBase()
                .Where(m => m.IdPropietario == idPropietario)
                .ToListAsync();

            return Resultado<IEnumerable<MascotaRespuestaDto>>.Exito(mascotas);
        }
        catch (Exception ex)
        {
            return Resultado<IEnumerable<MascotaRespuestaDto>>.Falla($"Error interno al obtener mascotas del propietario: {ex.Message}");
        }
    }

    public async Task<Resultado<long>> CrearAsync(MascotaSolicitudDto solicitud)
    {
        try
        {
            var validacion = Validar(solicitud);
            if (validacion is not null)
                return Resultado<long>.Falla(validacion);

            var propietarioExiste = await context.Propietarios.AnyAsync(p => p.Id == solicitud.IdPropietario);
            if (!propietarioExiste)
                return Resultado<long>.Falla($"No existe el propietario con ID {solicitud.IdPropietario}.");

            var razaExiste = await context.Razas.AnyAsync(r => r.Id == solicitud.IdRaza);
            if (!razaExiste)
                return Resultado<long>.Falla($"No existe la raza con ID {solicitud.IdRaza}.");

            var entidad = new Mascota
            {
                IdPropietario = solicitud.IdPropietario,
                IdRaza = solicitud.IdRaza,
                Nombre = solicitud.Nombre.Trim(),
                Sexo = solicitud.Sexo?.Trim() ?? string.Empty,
                FechaNacimiento = solicitud.FechaNacimiento,
                Color = solicitud.Color?.Trim(),
                Activo = true
            };

            context.Mascotas.Add(entidad);
            await context.SaveChangesAsync();
            return Resultado<long>.Exito(entidad.Id, "Mascota registrada exitosamente.");
        }
        catch (Exception ex)
        {
            return Resultado<long>.Falla($"Error interno al registrar la mascota: {ex.Message}");
        }
    }

    public async Task<Resultado> ActualizarAsync(long id, MascotaSolicitudDto solicitud)
    {
        try
        {
            if (id <= 0)
                return Resultado.Falla("El identificador de la mascota debe ser mayor a cero.");

            var validacion = Validar(solicitud);
            if (validacion is not null)
                return Resultado.Falla(validacion);

            var entidad = await context.Mascotas.FirstOrDefaultAsync(m => m.Id == id);
            if (entidad is null)
                return Resultado.Falla($"No se encontró la mascota con ID {id}.");

            var propietarioExiste = await context.Propietarios.AnyAsync(p => p.Id == solicitud.IdPropietario);
            if (!propietarioExiste)
                return Resultado.Falla($"No existe el propietario con ID {solicitud.IdPropietario}.");

            var razaExiste = await context.Razas.AnyAsync(r => r.Id == solicitud.IdRaza);
            if (!razaExiste)
                return Resultado.Falla($"No existe la raza con ID {solicitud.IdRaza}.");

            entidad.IdPropietario = solicitud.IdPropietario;
            entidad.IdRaza = solicitud.IdRaza;
            entidad.Nombre = solicitud.Nombre.Trim();
            entidad.Sexo = solicitud.Sexo?.Trim() ?? string.Empty;
            entidad.FechaNacimiento = solicitud.FechaNacimiento;
            entidad.Color = solicitud.Color?.Trim();

            await context.SaveChangesAsync();
            return Resultado.Exito("Mascota actualizada exitosamente.");
        }
        catch (Exception ex)
        {
            return Resultado.Falla($"Error interno al actualizar la mascota: {ex.Message}");
        }
    }

    public async Task<Resultado> EliminarAsync(long id)
    {
        try
        {
            if (id <= 0)
                return Resultado.Falla("El identificador de la mascota debe ser mayor a cero.");

            var entidad = await context.Mascotas.FirstOrDefaultAsync(m => m.Id == id);
            if (entidad is null)
                return Resultado.Falla($"No se encontró la mascota con ID {id}.");

            entidad.Activo = false;
            await context.SaveChangesAsync();
            return Resultado.Exito("Mascota eliminada exitosamente.");
        }
        catch (Exception ex)
        {
            return Resultado.Falla($"Error interno al eliminar la mascota: {ex.Message}");
        }
    }

    private IQueryable<MascotaRespuestaDto> ConsultaBase() =>
        context.Mascotas
            .AsNoTracking()
            .Select(m => new MascotaRespuestaDto
            {
                Id = m.Id,
                IdPropietario = m.IdPropietario,
                NombrePropietario = $"{m.Propietario.Nombre} {m.Propietario.Apellido}",
                IdRaza = m.IdRaza,
                NombreRaza = m.Raza.Nombre,
                IdEspecie = m.Raza.IdEspecie,
                NombreEspecie = m.Raza.Especie.Nombre,
                Nombre = m.Nombre,
                Sexo = m.Sexo,
                FechaNacimiento = m.FechaNacimiento,
                Color = m.Color,
                Activo = m.Activo,
                CantidadConsultas = m.Consultas.Count
            });

    private static string? Validar(MascotaSolicitudDto solicitud)
    {
        if (string.IsNullOrWhiteSpace(solicitud.Nombre))
            return "El nombre de la mascota es obligatorio.";

        if (solicitud.IdPropietario <= 0)
            return "Debe asociar un propietario válido.";

        if (solicitud.IdRaza <= 0)
            return "Debe seleccionar una raza válida.";

        return null;
    }
}
