using Microsoft.EntityFrameworkCore;
using Veterinaria.Controllers.Seguridad;
using Veterinaria.Domain.Comunes;
using Veterinaria.Domain.Dtos;
using Veterinaria.Domain.Entidades;
using Veterinaria.Infrastructure;

namespace Veterinaria.Controllers.Controladores;

public class UsuarioControlador(ContextoVeterinaria context)
{
    public async Task<Resultado<IEnumerable<UsuarioRespuestaDto>>> ObtenerTodosAsync()
    {
        try
        {
            var usuarios = await context.Usuarios
                .AsNoTracking()
                .Include(u => u.TipoUsuario)
                .Select(u => Mapear(u))
                .ToListAsync();

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
            if (solicitud.IdTipoUsuario <= 0)
                return Resultado<long>.Falla("El identificador del tipo de usuario debe ser mayor a cero.");

            if (string.IsNullOrWhiteSpace(solicitud.NombreUsuario))
                return Resultado<long>.Falla("El nombre de usuario es obligatorio.");

            if (string.IsNullOrWhiteSpace(solicitud.Contrasena))
                return Resultado<long>.Falla("La contraseña es obligatoria.");

            if (string.IsNullOrWhiteSpace(solicitud.Nombre))
                return Resultado<long>.Falla("El nombre del usuario es obligatorio.");

            if (string.IsNullOrWhiteSpace(solicitud.Apellido))
                return Resultado<long>.Falla("El apellido del usuario es obligatorio.");

            if (string.IsNullOrWhiteSpace(solicitud.DNI))
                return Resultado<long>.Falla("El DNI del usuario es obligatorio.");

            var tipoExiste = await context.TiposUsuario.AnyAsync(r => r.Id == solicitud.IdTipoUsuario);
            if (!tipoExiste)
                return Resultado<long>.Falla($"No existe un tipo de usuario registrado con ID {solicitud.IdTipoUsuario}.");

            var nombreUsuarioNormalizado = solicitud.NombreUsuario.Trim();
            var existeNombreUsuario = await context.Usuarios
                .AnyAsync(u => u.NombreUsuario.ToLower() == nombreUsuarioNormalizado.ToLower());

            if (existeNombreUsuario)
                return Resultado<long>.Falla($"El nombre de usuario '{nombreUsuarioNormalizado}' ya está en uso.");

            var entidad = new Usuario
            {
                IdTipoUsuario = solicitud.IdTipoUsuario,
                NombreUsuario = nombreUsuarioNormalizado,
                HashContrasena = HasheadorContrasena.Hashear(solicitud.Contrasena),
                Nombre = solicitud.Nombre.Trim(),
                Apellido = solicitud.Apellido.Trim(),
                DNI = solicitud.DNI.Trim(),
                Matricula = string.IsNullOrWhiteSpace(solicitud.Matricula) ? null : solicitud.Matricula.Trim(),
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
            if (id <= 0)
                return Resultado.Falla("El identificador del usuario debe ser mayor a cero.");

            if (solicitud.IdTipoUsuario <= 0)
                return Resultado.Falla("El identificador del tipo de usuario debe ser mayor a cero.");

            if (string.IsNullOrWhiteSpace(solicitud.NombreUsuario))
                return Resultado.Falla("El nombre de usuario es obligatorio.");

            if (string.IsNullOrWhiteSpace(solicitud.Nombre))
                return Resultado.Falla("El nombre del usuario es obligatorio.");

            if (string.IsNullOrWhiteSpace(solicitud.Apellido))
                return Resultado.Falla("El apellido del usuario es obligatorio.");

            if (string.IsNullOrWhiteSpace(solicitud.DNI))
                return Resultado.Falla("El DNI del usuario es obligatorio.");

            var entidad = await context.Usuarios.FirstOrDefaultAsync(u => u.Id == id);
            if (entidad is null)
                return Resultado.Falla($"No se encontró el usuario con ID {id}.");

            var tipoExiste = await context.TiposUsuario.AnyAsync(r => r.Id == solicitud.IdTipoUsuario);
            if (!tipoExiste)
                return Resultado.Falla($"No existe un tipo de usuario registrado con ID {solicitud.IdTipoUsuario}.");

            var nombreUsuarioNormalizado = solicitud.NombreUsuario.Trim();
            var existeNombreUsuario = await context.Usuarios
                .AnyAsync(u => u.Id != id && u.NombreUsuario.ToLower() == nombreUsuarioNormalizado.ToLower());

            if (existeNombreUsuario)
                return Resultado.Falla($"El nombre de usuario '{nombreUsuarioNormalizado}' ya está en uso por otro usuario.");

            entidad.IdTipoUsuario = solicitud.IdTipoUsuario;
            entidad.NombreUsuario = nombreUsuarioNormalizado;
            entidad.Nombre = solicitud.Nombre.Trim();
            entidad.Apellido = solicitud.Apellido.Trim();
            entidad.DNI = solicitud.DNI.Trim();
            entidad.Matricula = string.IsNullOrWhiteSpace(solicitud.Matricula) ? null : solicitud.Matricula.Trim();

            if (!string.IsNullOrWhiteSpace(solicitud.Contrasena))
                entidad.HashContrasena = HasheadorContrasena.Hashear(solicitud.Contrasena);

            await context.SaveChangesAsync();
            return Resultado.Exito("Usuario actualizado exitosamente.");
        }
        catch (Exception ex)
        {
            return Resultado.Falla($"Error interno al actualizar el usuario: {ex.Message}");
        }
    }

    public async Task<Resultado> EliminarAsync(long id)
    {
        try
        {
            if (id <= 0)
                return Resultado.Falla("El identificador del usuario debe ser mayor a cero.");

            var entidad = await context.Usuarios.FirstOrDefaultAsync(u => u.Id == id);
            if (entidad is null)
                return Resultado.Falla($"No se encontró el usuario con ID {id}.");

            entidad.Activo = false;
            await context.SaveChangesAsync();
            return Resultado.Exito("Usuario eliminado exitosamente.");
        }
        catch (Exception ex)
        {
            return Resultado.Falla($"Error interno al eliminar el usuario: {ex.Message}");
        }
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
        Matricula = usuario.Matricula,
        Activo = usuario.Activo
    };
}
