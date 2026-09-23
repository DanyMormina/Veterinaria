using System.Text.RegularExpressions;
using Microsoft.EntityFrameworkCore;
using Veterinaria.Domain.Comunes;
using Veterinaria.Domain.Dtos;
using Veterinaria.Domain.Entidades;
using Veterinaria.Infrastructure;

namespace Veterinaria.Controllers.Controladores;

public class PropietarioControlador(ContextoVeterinaria context)
{
    protected ContextoVeterinaria Context => context;

    private static readonly Regex RegexNombreApellido = new(@"^[a-zA-ZáéíóúÁÉÍÓÚñÑüÜ\s]{2,100}$", RegexOptions.Compiled);
    private static readonly Regex RegexDni = new(@"^\d{7,8}$", RegexOptions.Compiled);
    private static readonly Regex RegexTelefono = new(@"^\d{6,13}$", RegexOptions.Compiled);

    /// <summary>
    /// Obtiene todos los propietarios con estado activo en el sistema.
    /// Utiliza AsNoTracking para optimizar la lectura de solo datos.
    /// </summary>
    public async Task<Resultado<IEnumerable<PropietarioRespuestaDto>>> ObtenerActivosAsync()
    {
        try
        {
            var propietarios = await context.Propietarios
                .AsNoTracking()
                .Where(p => p.Activo)
                .OrderBy(p => p.Apellido)
                .ThenBy(p => p.Nombre)
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
                })
                .ToListAsync();

            return Resultado<IEnumerable<PropietarioRespuestaDto>>.Exito(propietarios);
        }
        catch (Exception ex)
        {
            return Resultado<IEnumerable<PropietarioRespuestaDto>>.Falla($"Error interno al obtener propietarios activos: {ex.Message}");
        }
    }

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
            // 1. Validación de campos obligatorios y formato
            var errorValidacion = ValidarDatosSolicitud(solicitud);
            if (errorValidacion is not null)
                return Resultado<long>.Falla(errorValidacion);

            var dniNormalizado = solicitud.DNI.Trim();
            var telefonoNormalizado = string.IsNullOrWhiteSpace(solicitud.Telefono) ? null : solicitud.Telefono.Trim();
            var correoNormalizado = string.IsNullOrWhiteSpace(solicitud.CorreoElectronico) ? null : solicitud.CorreoElectronico.Trim();

            // 2. Comprobar unicidad en base de datos ignorando filtros de borrado lógico
            var conflictoUnicidad = await ValidarUnicidadContactoAsync(
                idExcluir: null,
                dniNormalizado,
                telefonoNormalizado,
                correoNormalizado);

            if (conflictoUnicidad is not null)
                return Resultado<long>.Falla(conflictoUnicidad);

            var entidad = new Propietario
            {
                DNI = dniNormalizado,
                Nombre = solicitud.Nombre.Trim(),
                Apellido = solicitud.Apellido.Trim(),
                Telefono = telefonoNormalizado,
                CorreoElectronico = correoNormalizado,
                Direccion = solicitud.Direccion?.Trim(),
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

            // 1. Validación de campos obligatorios y formato
            var errorValidacion = ValidarDatosSolicitud(solicitud);
            if (errorValidacion is not null)
                return Resultado.Falla(errorValidacion);

            var entidad = await context.Propietarios.FirstOrDefaultAsync(p => p.Id == id);
            if (entidad is null)
                return Resultado.Falla($"No se encontró el propietario con ID {id}.");

            var dniNormalizado = solicitud.DNI.Trim();
            var telefonoNormalizado = string.IsNullOrWhiteSpace(solicitud.Telefono) ? null : solicitud.Telefono.Trim();
            var correoNormalizado = string.IsNullOrWhiteSpace(solicitud.CorreoElectronico) ? null : solicitud.CorreoElectronico.Trim();

            // 2. Comprobar unicidad en base de datos excluyendo el registro actual e ignorando soft-deleted
            var conflictoUnicidad = await ValidarUnicidadContactoAsync(
                id,
                dniNormalizado,
                telefonoNormalizado,
                correoNormalizado);

            if (conflictoUnicidad is not null)
                return Resultado.Falla(conflictoUnicidad);

            entidad.DNI = dniNormalizado;
            entidad.Nombre = solicitud.Nombre.Trim();
            entidad.Apellido = solicitud.Apellido.Trim();
            entidad.Telefono = telefonoNormalizado;
            entidad.CorreoElectronico = correoNormalizado;
            entidad.Direccion = solicitud.Direccion?.Trim();

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
        return await CambiarEstadoAsync(id, false);
    }

    /// <summary>
    /// Modifica el estado de activación (Activo = true/false) de un propietario.
    /// Utiliza IgnoreQueryFilters para permitir encontrar propietarios desactivados y reactivarlos.
    /// </summary>
    public async Task<Resultado> CambiarEstadoAsync(long id, bool activo)
    {
        try
        {
            if (id <= 0)
                return Resultado.Falla("El identificador del propietario debe ser mayor a cero.");

            // Buscar el propietario ignorando el filtro global de soft delete
            var entidad = await context.Propietarios.IgnoreQueryFilters().FirstOrDefaultAsync(p => p.Id == id);
            if (entidad is null)
                return Resultado.Falla($"No se encontró el propietario con ID {id}.");

            entidad.Activo = activo;
            await context.SaveChangesAsync();

            var accion = activo ? "activado" : "desactivado";
            return Resultado.Exito($"Propietario {accion} exitosamente.");
        }
        catch (Exception ex)
        {
            return Resultado.Falla($"Error interno al cambiar el estado del propietario: {ex.Message}");
        }
    }

    /// <summary>
    /// Valida la consistencia, obligatoriedad y restricciones de formato de la solicitud de propietario.
    /// Retorna un mensaje descriptivo de error en caso de fallo, o null si los datos son válidos.
    /// </summary>
    private static string? ValidarDatosSolicitud(PropietarioSolicitudDto solicitud)
    {
        // 1. Nombre: obligatorio, 2 a 100 caracteres, solo letras, tildes y espacios
        var nombre = solicitud.Nombre?.Trim() ?? string.Empty;
        if (string.IsNullOrWhiteSpace(nombre) || nombre.Length < 2 || nombre.Length > 100)
            return "El nombre es obligatorio y debe tener entre 2 y 100 caracteres.";

        if (!RegexNombreApellido.IsMatch(nombre))
            return "El nombre solo debe contener letras, tildes y espacios.";

        // 2. Apellido: obligatorio, 2 a 100 caracteres, solo letras, tildes y espacios
        var apellido = solicitud.Apellido?.Trim() ?? string.Empty;
        if (string.IsNullOrWhiteSpace(apellido) || apellido.Length < 2 || apellido.Length > 100)
            return "El apellido es obligatorio y debe tener entre 2 y 100 caracteres.";

        if (!RegexNombreApellido.IsMatch(apellido))
            return "El apellido solo debe contener letras, tildes y espacios.";

        // 3. DNI: obligatorio, numérico, entre 7 y 8 dígitos exactos
        var dni = solicitud.DNI?.Trim() ?? string.Empty;
        if (string.IsNullOrWhiteSpace(dni) || !RegexDni.IsMatch(dni))
            return "El DNI es obligatorio y debe contener entre 7 y 8 dígitos numéricos.";

        // 4. Dirección: obligatoria, longitud 3 a 200 caracteres
        var direccion = solicitud.Direccion?.Trim() ?? string.Empty;
        if (string.IsNullOrWhiteSpace(direccion) || direccion.Length < 3 || direccion.Length > 200)
            return "La dirección es obligatoria y debe tener entre 3 y 200 caracteres.";

        // 5. Teléfono: obligatorio, numérico, entre 6 y 13 dígitos
        var telefono = solicitud.Telefono?.Trim() ?? string.Empty;
        if (string.IsNullOrWhiteSpace(telefono) || !RegexTelefono.IsMatch(telefono))
            return "El teléfono es obligatorio y debe contener entre 6 y 13 dígitos numéricos.";

        // 6. Correo Electrónico: obligatorio y formato válido
        var errorCorreo = UsuarioControlador.ValidarFormatoCorreo(solicitud.CorreoElectronico);
        if (errorCorreo is not null)
            return errorCorreo;

        return null;
    }

    /// <summary>
    /// Comprueba la unicidad de DNI, teléfono y correo electrónico en la base de datos ignorando filtros de borrado lógico (soft-deleted).
    /// </summary>
    protected async Task<string?> ValidarUnicidadContactoAsync(
        long? idExcluir,
        string dni,
        string? telefono,
        string? correoElectronico)
    {
        var propietarios = context.Propietarios.IgnoreQueryFilters().AsQueryable();

        // 1. Unicidad de DNI en toda la tabla (activos e inactivos)
        var existeDni = await propietarios.AnyAsync(p =>
            (!idExcluir.HasValue || p.Id != idExcluir.Value) &&
            p.DNI.ToLower() == dni.ToLower());

        if (existeDni)
            return $"Ya existe un propietario registrado con el DNI '{dni}'.";

        // 2. Unicidad de Teléfono en toda la tabla
        if (!string.IsNullOrWhiteSpace(telefono))
        {
            var existeTelefono = await propietarios.AnyAsync(p =>
                (!idExcluir.HasValue || p.Id != idExcluir.Value) &&
                p.Telefono != null &&
                p.Telefono.ToLower() == telefono.ToLower());

            if (existeTelefono)
                return $"Ya existe un propietario registrado con el teléfono '{telefono}'.";
        }

        // 3. Unicidad de Correo Electrónico en toda la tabla
        if (!string.IsNullOrWhiteSpace(correoElectronico))
        {
            var existeCorreo = await propietarios.AnyAsync(p =>
                (!idExcluir.HasValue || p.Id != idExcluir.Value) &&
                p.CorreoElectronico != null &&
                p.CorreoElectronico.ToLower() == correoElectronico.ToLower());

            if (existeCorreo)
                return $"Ya existe un propietario registrado con el correo electrónico '{correoElectronico}'.";
        }

        return null;
    }

    private IQueryable<PropietarioRespuestaDto> ConsultaBase() =>
        context.Propietarios
            .IgnoreQueryFilters()
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
