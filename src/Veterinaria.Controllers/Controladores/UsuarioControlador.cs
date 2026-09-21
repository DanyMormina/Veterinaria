using System.Text.RegularExpressions;
using Microsoft.EntityFrameworkCore;
using Veterinaria.Controllers.Seguridad;
using Veterinaria.Domain.Comunes;
using Veterinaria.Domain.Dtos;
using Veterinaria.Domain.Entidades;
using Veterinaria.Infrastructure;

namespace Veterinaria.Controllers.Controladores;

public class UsuarioControlador(ContextoVeterinaria context)
{
    private static readonly Regex RegexNombreApellido = new(@"^[a-zA-ZáéíóúÁÉÍÓÚñÑüÜ\s]{2,100}$", RegexOptions.Compiled);
    private static readonly Regex RegexDni = new(@"^\d{7,8}$", RegexOptions.Compiled);
    private static readonly Regex RegexTelefono = new(@"^\d{6,13}$", RegexOptions.Compiled);
    private static readonly Regex RegexCorreo = new(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,63}$", RegexOptions.Compiled);

    public async Task<Resultado<IEnumerable<UsuarioRespuestaDto>>> ObtenerTodosAsync()
    {
        try
        {
            var entidades = await context.Usuarios
                .IgnoreQueryFilters()
                .AsNoTracking()
                .Include(u => u.TipoUsuario)
                .ToListAsync();

            var usuarios = entidades.Select(Mapear).ToList();

            return Resultado<IEnumerable<UsuarioRespuestaDto>>.Exito(usuarios);
        }
        catch (Exception ex)
        {
            return Resultado<IEnumerable<UsuarioRespuestaDto>>.Falla($"Error interno al obtener usuarios: {ex.Message}");
        }
    }

    public async Task<Resultado<UsuarioRespuestaDto>> ObtenerPorIdAsync(long id)
    {
        try
        {
            if (id <= 0)
                return Resultado<UsuarioRespuestaDto>.Falla("El identificador del usuario debe ser mayor a cero.");

            var usuario = await context.Usuarios
                .IgnoreQueryFilters()
                .AsNoTracking()
                .Include(u => u.TipoUsuario)
                .FirstOrDefaultAsync(u => u.Id == id);

            if (usuario is null)
                return Resultado<UsuarioRespuestaDto>.Falla($"No se encontró el usuario con ID {id}.");

            return Resultado<UsuarioRespuestaDto>.Exito(Mapear(usuario));
        }
        catch (Exception ex)
        {
            return Resultado<UsuarioRespuestaDto>.Falla($"Error interno al obtener el usuario: {ex.Message}");
        }
    }

    public async Task<Resultado<UsuarioRespuestaDto>> ObtenerPorNombreUsuarioAsync(string nombreUsuario)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(nombreUsuario))
                return Resultado<UsuarioRespuestaDto>.Falla("El nombre de usuario no puede estar vacío.");

            var nombreUsuarioNormalizado = nombreUsuario.Trim();
            var usuario = await context.Usuarios
                .AsNoTracking()
                .Include(u => u.TipoUsuario)
                .FirstOrDefaultAsync(u => u.NombreUsuario.ToLower() == nombreUsuarioNormalizado.ToLower());

            if (usuario is null)
                return Resultado<UsuarioRespuestaDto>.Falla($"No se encontró el usuario con nombreUsuario '{nombreUsuarioNormalizado}'.");

            return Resultado<UsuarioRespuestaDto>.Exito(Mapear(usuario));
        }
        catch (Exception ex)
        {
            return Resultado<UsuarioRespuestaDto>.Falla($"Error interno al obtener usuario por nombreUsuario: {ex.Message}");
        }
    }

    public async Task<Resultado<UsuarioRespuestaDto>> AutenticarAsync(string nombreUsuario, string contrasena)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(nombreUsuario) || string.IsNullOrWhiteSpace(contrasena))
                return Resultado<UsuarioRespuestaDto>.Falla("Debe ingresar usuario y contraseña.");

            var nombreUsuarioNormalizado = nombreUsuario.Trim();
            var usuario = await context.Usuarios
                .AsNoTracking()
                .Include(u => u.TipoUsuario)
                .FirstOrDefaultAsync(u => u.NombreUsuario.ToLower() == nombreUsuarioNormalizado.ToLower());

            if (usuario is null)
                return Resultado<UsuarioRespuestaDto>.Falla("Credenciales inválidas.");

            if (!usuario.Activo)
                return Resultado<UsuarioRespuestaDto>.Falla("El usuario se encuentra inactivo en el sistema.");

            if (!HasheadorContrasena.Verificar(contrasena, usuario.HashContrasena))
                return Resultado<UsuarioRespuestaDto>.Falla("Credenciales inválidas.");

            return Resultado<UsuarioRespuestaDto>.Exito(Mapear(usuario), "Autenticación exitosa.");
        }
        catch (Exception ex)
        {
            return Resultado<UsuarioRespuestaDto>.Falla($"Error interno al autenticar usuario: {ex.Message}");
        }
    }

    public async Task<Resultado<long>> CrearAsync(UsuarioSolicitudDto solicitud)
    {
        try
        {
            // 1. Asegurar la consistencia de columnas en la base de datos
            await InicializadorDatos.AsegurarColumnasUsuarioAsync(context);

            // 2. Validación de campos obligatorios y formato
            var errorValidacion = ValidarDatosSolicitud(solicitud, esAlta: true);
            if (errorValidacion is not null)
                return Resultado<long>.Falla(errorValidacion);

            // 3. Validar existencia del rol/tipo de usuario
            var tipoExiste = await context.TiposUsuario.AnyAsync(r => r.Id == solicitud.IdTipoUsuario && r.Activo);
            if (!tipoExiste)
                return Resultado<long>.Falla($"No existe un rol activo registrado con ID {solicitud.IdTipoUsuario}.");

            var nombreUsuarioNormalizado = solicitud.NombreUsuario.Trim();
            var dniNormalizado = solicitud.DNI.Trim();
            var telefonoNormalizado = NormalizarTexto(solicitud.Telefono);
            var correoNormalizado = NormalizarTexto(solicitud.CorreoElectronico);
            var direccionNormalizada = NormalizarTexto(solicitud.Direccion);
            var sexoNormalizado = NormalizarTexto(solicitud.Sexo);

            // 4. Validar unicidad del nombre de usuario en la base de datos
            var existeNombreUsuario = await context.Usuarios
                .IgnoreQueryFilters()
                .AnyAsync(u => u.NombreUsuario.ToLower() == nombreUsuarioNormalizado.ToLower());

            if (existeNombreUsuario)
                return Resultado<long>.Falla($"El nombre de usuario '{nombreUsuarioNormalizado}' ya está en uso.");

            // 5. Validar unicidad de DNI y Correo Electrónico
            var conflictoUnicidad = await ValidarUnicidadContactoAsync(
                idExcluir: null,
                dniNormalizado,
                telefonoNormalizado,
                correoNormalizado);

            if (conflictoUnicidad is not null)
                return Resultado<long>.Falla(conflictoUnicidad);

            // 6. Instanciar y persistir nueva entidad de usuario con contraseña hasheada
            var entidad = new Usuario
            {
                IdTipoUsuario = solicitud.IdTipoUsuario,
                NombreUsuario = nombreUsuarioNormalizado,
                HashContrasena = HasheadorContrasena.Hashear(solicitud.Contrasena),
                Nombre = solicitud.Nombre.Trim(),
                Apellido = solicitud.Apellido.Trim(),
                DNI = dniNormalizado,
                Direccion = direccionNormalizada,
                Telefono = telefonoNormalizado,
                CorreoElectronico = correoNormalizado,
                FechaNacimiento = solicitud.FechaNacimiento,
                Sexo = sexoNormalizado,
                Matricula = NormalizarTexto(solicitud.Matricula),
                Activo = true
            };

            context.Usuarios.Add(entidad);
            await context.SaveChangesAsync();

            return Resultado<long>.Exito(entidad.Id, "Usuario creado exitosamente.");
        }
        catch (Exception ex)
        {
            return Resultado<long>.Falla($"Error interno al crear el usuario: {ex.Message}");
        }
    }

    public async Task<Resultado> ActualizarAsync(long id, UsuarioSolicitudDto solicitud)
    {
        try
        {
            // 1. Asegurar consistencia de base de datos
            await InicializadorDatos.AsegurarColumnasUsuarioAsync(context);

            if (id <= 0)
                return Resultado.Falla("El identificador del usuario debe ser mayor a cero.");

            // 2. Validación de campos de la solicitud
            var errorValidacion = ValidarDatosSolicitud(solicitud, esAlta: false);
            if (errorValidacion is not null)
                return Resultado.Falla(errorValidacion);

            // 3. Buscar entidad existente
            var entidad = await context.Usuarios
                .IgnoreQueryFilters()
                .Include(u => u.TipoUsuario)
                .FirstOrDefaultAsync(u => u.Id == id);

            if (entidad is null)
                return Resultado.Falla($"No se encontró el usuario con ID {id}.");

            // 4. Validar existencia del rol seleccionado
            var nuevoTipo = await context.TiposUsuario.FirstOrDefaultAsync(r => r.Id == solicitud.IdTipoUsuario && r.Activo);
            if (nuevoTipo is null)
                return Resultado.Falla($"No existe un rol activo registrado con ID {solicitud.IdTipoUsuario}.");

            // 5. Protección del Último Administrador: Impedir cambiar el rol si es el único Administrador activo
            var esAdminActual = entidad.TipoUsuario != null &&
                                entidad.TipoUsuario.Nombre.Equals("Administrador", StringComparison.OrdinalIgnoreCase);
            var seraAdmin = nuevoTipo.Nombre.Equals("Administrador", StringComparison.OrdinalIgnoreCase);

            if (entidad.Activo && esAdminActual && !seraAdmin)
            {
                var otrosAdmins = await context.Usuarios
                    .CountAsync(u => u.Activo && u.Id != id && u.TipoUsuario.Nombre.ToLower() == "administrador");

                if (otrosAdmins < 1)
                    return Resultado.Falla("No es posible cambiar el rol del único Administrador activo del sistema.");
            }

            var nombreUsuarioNormalizado = solicitud.NombreUsuario.Trim();
            var dniNormalizado = solicitud.DNI.Trim();
            var telefonoNormalizado = NormalizarTexto(solicitud.Telefono);
            var correoNormalizado = NormalizarTexto(solicitud.CorreoElectronico);
            var direccionNormalizada = NormalizarTexto(solicitud.Direccion);
            var sexoNormalizado = NormalizarTexto(solicitud.Sexo);

            // 6. Validar unicidad de nombre de usuario excluyendo el actual
            var existeNombreUsuario = await context.Usuarios
                .IgnoreQueryFilters()
                .AnyAsync(u => u.Id != id && u.NombreUsuario.ToLower() == nombreUsuarioNormalizado.ToLower());

            if (existeNombreUsuario)
                return Resultado.Falla($"El nombre de usuario '{nombreUsuarioNormalizado}' ya está en uso por otro usuario.");

            // 7. Validar unicidad de DNI y correo
            var conflictoUnicidad = await ValidarUnicidadContactoAsync(
                id,
                dniNormalizado,
                telefonoNormalizado,
                correoNormalizado);

            if (conflictoUnicidad is not null)
                return Resultado.Falla(conflictoUnicidad);

            // 8. Actualizar propiedades
            entidad.IdTipoUsuario = solicitud.IdTipoUsuario;
            entidad.NombreUsuario = nombreUsuarioNormalizado;
            entidad.Nombre = solicitud.Nombre.Trim();
            entidad.Apellido = solicitud.Apellido.Trim();
            entidad.DNI = dniNormalizado;
            entidad.Direccion = direccionNormalizada;
            entidad.Telefono = telefonoNormalizado;
            entidad.CorreoElectronico = correoNormalizado;
            entidad.FechaNacimiento = solicitud.FechaNacimiento;
            entidad.Sexo = sexoNormalizado;
            entidad.Matricula = NormalizarTexto(solicitud.Matricula);

            if (solicitud.Activo.HasValue)
            {
                if (!solicitud.Activo.Value && entidad.Activo)
                {
                    var esAdmin = entidad.TipoUsuario != null &&
                                  entidad.TipoUsuario.Nombre.Equals("Administrador", StringComparison.OrdinalIgnoreCase);

                    if (esAdmin)
                    {
                        var adminsActivos = await context.Usuarios
                            .CountAsync(u => u.Activo && u.TipoUsuario.Nombre.ToLower() == "administrador");

                        if (adminsActivos <= 1)
                            return Resultado.Falla("No es posible desactivar al único Administrador activo del sistema.");
                    }
                }

                entidad.Activo = solicitud.Activo.Value;
            }

            var cambioContrasena = !string.IsNullOrWhiteSpace(solicitud.Contrasena);
            if (cambioContrasena)
            {
                entidad.HashContrasena = HasheadorContrasena.Hashear(solicitud.Contrasena);
            }

            // 9. Marcar modificaciones explícitas en EF Core
            context.Entry(entidad).State = EntityState.Modified;
            context.Entry(entidad).Property(u => u.HashContrasena).IsModified = cambioContrasena;

            await context.SaveChangesAsync();
            return Resultado.Exito("Usuario actualizado exitosamente.");
        }
        catch (Exception ex)
        {
            return Resultado.Falla($"Error interno al actualizar el usuario: {ex.Message}");
        }
    }

    public async Task<Resultado> CambiarEstadoAsync(long id, bool activo, long? idUsuarioSesion = null)
    {
        try
        {
            if (id <= 0)
                return Resultado.Falla("El identificador del usuario debe ser mayor a cero.");

            var entidad = await context.Usuarios
                .IgnoreQueryFilters()
                .Include(u => u.TipoUsuario)
                .FirstOrDefaultAsync(u => u.Id == id);

            if (entidad is null)
                return Resultado.Falla($"No se encontró el usuario con ID {id}.");

            if (entidad.Activo == activo)
                return Resultado.Falla(activo ? "El usuario ya se encuentra activo." : "El usuario ya se encuentra inactivo.");

            // Si se va a desactivar, aplicar protecciones defensivas
            if (!activo)
            {
                if (idUsuarioSesion.HasValue && id == idUsuarioSesion.Value)
                    return Resultado.Falla("No puede desactivar su propia cuenta de usuario en sesión actual.");

                var esAdmin = entidad.TipoUsuario != null &&
                              entidad.TipoUsuario.Nombre.Equals("Administrador", StringComparison.OrdinalIgnoreCase);

                if (esAdmin)
                {
                    var adminsActivos = await context.Usuarios
                        .CountAsync(u => u.Activo && u.TipoUsuario.Nombre.ToLower() == "administrador");

                    if (adminsActivos <= 1)
                        return Resultado.Falla("No es posible desactivar al único Administrador activo del sistema.");
                }
            }

            entidad.Activo = activo;
            await context.SaveChangesAsync();

            return Resultado.Exito(activo ? "Usuario reactivado exitosamente." : "Usuario desactivado exitosamente.");
        }
        catch (Exception ex)
        {
            return Resultado.Falla($"Error interno al cambiar el estado del usuario: {ex.Message}");
        }
    }

    public async Task<Resultado> EliminarAsync(long id, long? idUsuarioSesion = null)
    {
        return await CambiarEstadoAsync(id, activo: false, idUsuarioSesion);
    }

    private static string? ValidarDatosSolicitud(UsuarioSolicitudDto solicitud, bool esAlta)
    {
        if (solicitud.IdTipoUsuario <= 0)
            return "Debe seleccionar un rol / perfil válido.";

        if (string.IsNullOrWhiteSpace(solicitud.Nombre) || solicitud.Nombre.Trim().Length < 2)
            return "El nombre es obligatorio y debe tener al menos 2 caracteres.";

        if (!RegexNombreApellido.IsMatch(solicitud.Nombre.Trim()))
            return "El nombre solo debe contener letras, tildes y espacios.";

        if (string.IsNullOrWhiteSpace(solicitud.Apellido) || solicitud.Apellido.Trim().Length < 2)
            return "El apellido es obligatorio y debe tener al menos 2 caracteres.";

        if (!RegexNombreApellido.IsMatch(solicitud.Apellido.Trim()))
            return "El apellido solo debe contener letras, tildes y espacios.";

        var dni = solicitud.DNI?.Trim() ?? string.Empty;
        if (string.IsNullOrWhiteSpace(dni) || !RegexDni.IsMatch(dni))
            return "El DNI es obligatorio y debe contener entre 7 y 8 dígitos numéricos.";

        var direccion = solicitud.Direccion?.Trim() ?? string.Empty;
        if (string.IsNullOrWhiteSpace(direccion) || direccion.Length < 3)
            return "La dirección es obligatoria y debe tener al menos 3 caracteres.";

        var telefono = solicitud.Telefono?.Trim() ?? string.Empty;
        if (string.IsNullOrWhiteSpace(telefono) || !RegexTelefono.IsMatch(telefono))
            return "El teléfono es obligatorio y debe contener entre 6 y 13 dígitos numéricos.";

        var errorCorreo = ValidarFormatoCorreo(solicitud.CorreoElectronico);
        if (errorCorreo is not null)
            return errorCorreo;

        var usuario = solicitud.NombreUsuario?.Trim() ?? string.Empty;
        if (string.IsNullOrWhiteSpace(usuario) || usuario.Length < 4 || usuario.Any(char.IsWhiteSpace))
            return "El nombre de usuario es obligatorio, debe tener al menos 4 caracteres y no contener espacios.";

        if (esAlta)
        {
            if (string.IsNullOrWhiteSpace(solicitud.Contrasena) || solicitud.Contrasena.Trim().Length < 6)
                return "La contraseña es obligatoria y debe tener al menos 6 caracteres.";
        }
        else
        {
            if (!string.IsNullOrWhiteSpace(solicitud.Contrasena) && solicitud.Contrasena.Trim().Length < 6)
                return "La nueva contraseña debe tener al menos 6 caracteres.";
        }

        if (solicitud.FechaNacimiento.HasValue)
        {
            var fechaMax = DateTime.Today.AddYears(-18);
            if (solicitud.FechaNacimiento.Value.Date > fechaMax)
                return "El usuario debe tener al menos 18 años de edad para trabajar legalmente.";
        }
        else
        {
            return "La fecha de nacimiento es obligatoria.";
        }

        var sexo = solicitud.Sexo?.Trim();
        if (string.IsNullOrWhiteSpace(sexo) || (!sexo.Equals("Hombre", StringComparison.OrdinalIgnoreCase) && !sexo.Equals("Mujer", StringComparison.OrdinalIgnoreCase)))
            return "Debe seleccionar el sexo del usuario (Hombre o Mujer).";

        return null;
    }

    public static string? ValidarFormatoCorreo(string? correo)
    {
        if (string.IsNullOrWhiteSpace(correo))
            return "El correo electrónico es obligatorio.";

        var correoNormalizado = correo.Trim();
        if (!RegexCorreo.IsMatch(correoNormalizado))
            return "Ingrese un correo electrónico válido (ejemplo: usuario@dominio.com).";

        var partes = correoNormalizado.Split('@');
        if (partes.Length != 2)
            return "El formato del correo electrónico es inválido.";

        var dominio = partes[1].ToLowerInvariant();

        if (dominio.StartsWith("gmail.") && dominio != "gmail.com")
            return "El correo de Gmail debe finalizar en '@gmail.com'.";

        if (dominio.StartsWith("hotmail.") && dominio != "hotmail.com" && dominio != "hotmail.es" && dominio != "hotmail.com.ar")
            return "El correo de Hotmail debe finalizar en '@hotmail.com', '@hotmail.es' o '@hotmail.com.ar'.";

        if (dominio.StartsWith("outlook.") && dominio != "outlook.com" && dominio != "outlook.es" && dominio != "outlook.com.ar")
            return "El correo de Outlook debe finalizar en '@outlook.com', '@outlook.es' o '@outlook.com.ar'.";

        if (dominio.StartsWith("yahoo.") && dominio != "yahoo.com" && dominio != "yahoo.es" && dominio != "yahoo.com.ar")
            return "El correo de Yahoo debe finalizar en '@yahoo.com', '@yahoo.es' o '@yahoo.com.ar'.";

        return null;
    }

    private async Task<string?> ValidarUnicidadContactoAsync(
        long? idExcluir,
        string dni,
        string? telefono,
        string? correoElectronico)
    {
        var usuarios = context.Usuarios.IgnoreQueryFilters().AsQueryable();

        var existeDni = await usuarios.AnyAsync(u =>
            (!idExcluir.HasValue || u.Id != idExcluir.Value) &&
            u.DNI.ToLower() == dni.ToLower());

        if (existeDni)
            return $"Ya existe un usuario registrado con el DNI '{dni}'.";

        if (!string.IsNullOrWhiteSpace(telefono))
        {
            var existeTelefono = await usuarios.AnyAsync(u =>
                (!idExcluir.HasValue || u.Id != idExcluir.Value) &&
                u.Telefono != null &&
                u.Telefono.ToLower() == telefono.ToLower());

            if (existeTelefono)
                return $"Ya existe un usuario registrado con el teléfono '{telefono}'.";
        }

        if (!string.IsNullOrWhiteSpace(correoElectronico))
        {
            var existeCorreo = await usuarios.AnyAsync(u =>
                (!idExcluir.HasValue || u.Id != idExcluir.Value) &&
                u.CorreoElectronico != null &&
                u.CorreoElectronico.ToLower() == correoElectronico.ToLower());

            if (existeCorreo)
                return $"Ya existe un usuario registrado con el correo electrónico '{correoElectronico}'.";
        }

        return null;
    }

    private static UsuarioRespuestaDto Mapear(Usuario usuario) => new()
    {
        Id = usuario.Id,
        IdTipoUsuario = usuario.IdTipoUsuario,
        NombreTipoUsuario = usuario.TipoUsuario != null ? usuario.TipoUsuario.Nombre : string.Empty,
        NombreUsuario = usuario.NombreUsuario,
        Nombre = usuario.Nombre,
        Apellido = usuario.Apellido,
        DNI = usuario.DNI,
        Direccion = usuario.Direccion,
        Telefono = usuario.Telefono,
        CorreoElectronico = usuario.CorreoElectronico,
        FechaNacimiento = usuario.FechaNacimiento,
        Sexo = usuario.Sexo,
        Matricula = usuario.Matricula,
        Activo = usuario.Activo
    };

    private static string? NormalizarTexto(string? valor) =>
        string.IsNullOrWhiteSpace(valor) ? null : valor.Trim();
}
