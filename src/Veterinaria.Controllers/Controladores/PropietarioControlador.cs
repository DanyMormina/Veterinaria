using Microsoft.EntityFrameworkCore;
using Veterinaria.Domain.Comunes;
using Veterinaria.Domain.Dtos;
using Veterinaria.Domain.Entidades;
using Veterinaria.Infrastructure;

namespace Veterinaria.Controllers.Controladores;

public class PropietarioControlador(ContextoVeterinaria context)
{
    public async Task<Resultado<IEnumerable<PropietarioRespuestaDto>>> ObtenerTodosAsync()
    {
        try
        {
            var propietarios = await ConsultaBase().ToListAsync();
            return Resultado<IEnumerable<PropietarioRespuestaDto>>.Exito(propietarios);
        }
        catch (Exception ex)
        {
            return Resultado<IEnumerable<PropietarioRespuestaDto>>.Falla($"Error interno al obtener propietarios: {ex.Message}");
        }
    }

    public async Task<Resultado<PropietarioRespuestaDto>> ObtenerPorIdAsync(long id)
    {
        try
        {
            if (id <= 0)
                return Resultado<PropietarioRespuestaDto>.Falla("El identificador del propietario debe ser mayor a cero.");

            var propietario = await ConsultaBase().FirstOrDefaultAsync(p => p.Id == id);
            if (propietario is null)
                return Resultado<PropietarioRespuestaDto>.Falla($"No se encontró el propietario con ID {id}.");

            return Resultado<PropietarioRespuestaDto>.Exito(propietario);
        }
        catch (Exception ex)
        {
            return Resultado<PropietarioRespuestaDto>.Falla($"Error interno al obtener el propietario: {ex.Message}");
        }
    }

    public async Task<Resultado<long>> CrearAsync(PropietarioSolicitudDto solicitud)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(solicitud.DNI))
                return Resultado<long>.Falla("El DNI del propietario es obligatorio.");

            if (string.IsNullOrWhiteSpace(solicitud.Nombre))
                return Resultado<long>.Falla("El nombre del propietario es obligatorio.");

            if (string.IsNullOrWhiteSpace(solicitud.Apellido))
                return Resultado<long>.Falla("El apellido del propietario es obligatorio.");

            var dniNormalizado = solicitud.DNI.Trim();
            var existeDni = await context.Propietarios.AnyAsync(p => p.DNI.ToLower() == dniNormalizado.ToLower());
            if (existeDni)
                return Resultado<long>.Falla($"Ya existe un propietario registrado con el DNI '{dniNormalizado}'.");

            var entidad = new Propietario
            {
                DNI = dniNormalizado,
                Nombre = solicitud.Nombre.Trim(),
                Apellido = solicitud.Apellido.Trim(),
                Telefono = string.IsNullOrWhiteSpace(solicitud.Telefono) ? null : solicitud.Telefono.Trim(),
                CorreoElectronico = string.IsNullOrWhiteSpace(solicitud.CorreoElectronico) ? null : solicitud.CorreoElectronico.Trim(),
                Direccion = string.IsNullOrWhiteSpace(solicitud.Direccion) ? null : solicitud.Direccion.Trim(),
                Activo = true
            };

            context.Propietarios.Add(entidad);
            await context.SaveChangesAsync();
            return Resultado<long>.Exito(entidad.Id, "Propietario registrado exitosamente.");
        }
        catch (Exception ex)
        {
            return Resultado<long>.Falla($"Error interno al registrar el propietario: {ex.Message}");
        }
    }

    public async Task<Resultado> ActualizarAsync(long id, PropietarioSolicitudDto solicitud)
    {
        try
        {
            if (id <= 0)
                return Resultado.Falla("El identificador del propietario debe ser mayor a cero.");

            if (string.IsNullOrWhiteSpace(solicitud.DNI))
                return Resultado.Falla("El DNI del propietario es obligatorio.");

            if (string.IsNullOrWhiteSpace(solicitud.Nombre))
                return Resultado.Falla("El nombre del propietario es obligatorio.");

            if (string.IsNullOrWhiteSpace(solicitud.Apellido))
                return Resultado.Falla("El apellido del propietario es obligatorio.");

            var entidad = await context.Propietarios.FirstOrDefaultAsync(p => p.Id == id);
            if (entidad is null)
                return Resultado.Falla($"No se encontró el propietario con ID {id}.");

            var dniNormalizado = solicitud.DNI.Trim();
            var existeDni = await context.Propietarios.AnyAsync(p => p.Id != id && p.DNI.ToLower() == dniNormalizado.ToLower());
            if (existeDni)
                return Resultado.Falla($"Ya existe otro propietario registrado con el DNI '{dniNormalizado}'.");

            entidad.DNI = dniNormalizado;
            entidad.Nombre = solicitud.Nombre.Trim();
            entidad.Apellido = solicitud.Apellido.Trim();
            entidad.Telefono = string.IsNullOrWhiteSpace(solicitud.Telefono) ? null : solicitud.Telefono.Trim();
            entidad.CorreoElectronico = string.IsNullOrWhiteSpace(solicitud.CorreoElectronico) ? null : solicitud.CorreoElectronico.Trim();
            entidad.Direccion = string.IsNullOrWhiteSpace(solicitud.Direccion) ? null : solicitud.Direccion.Trim();

            await context.SaveChangesAsync();
            return Resultado.Exito("Propietario actualizado exitosamente.");
        }
        catch (Exception ex)
        {
            return Resultado.Falla($"Error interno al actualizar el propietario: {ex.Message}");
        }
    }

    public async Task<Resultado> EliminarAsync(long id)
    {
        try
        {
            if (id <= 0)
                return Resultado.Falla("El identificador del propietario debe ser mayor a cero.");

            var entidad = await context.Propietarios.FirstOrDefaultAsync(p => p.Id == id);
            if (entidad is null)
                return Resultado.Falla($"No se encontró el propietario con ID {id}.");

            entidad.Activo = false;
            await context.SaveChangesAsync();
            return Resultado.Exito("Propietario eliminado exitosamente.");
        }
        catch (Exception ex)
        {
            return Resultado.Falla($"Error interno al eliminar el propietario: {ex.Message}");
        }
    }

    private IQueryable<PropietarioRespuestaDto> ConsultaBase() =>
        context.Propietarios
            .AsNoTracking()
            .Select(p => new PropietarioRespuestaDto
            {
                Id = p.Id,
                DNI = p.DNI,
                Nombre = p.Nombre,
                Apellido = p.Apellido,
                Telefono = p.Telefono,
                CorreoElectronico = p.CorreoElectronico,
                Direccion = p.Direccion,
                Activo = p.Activo,
                CantidadMascotas = p.Mascotas.Count
            });
}
