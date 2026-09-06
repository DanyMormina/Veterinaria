using Microsoft.EntityFrameworkCore;
using Veterinaria.Controllers.Seguridad;
using Veterinaria.Domain.Comunes;
using Veterinaria.Domain.Dtos;
using Veterinaria.Domain.Entidades;
using Veterinaria.Infrastructure;

namespace Veterinaria.Controllers.Controladores;

public class UsuarioController(VeterinariaDbContext context)
{
    public async Task<Result<IEnumerable<UsuarioResponseDto>>> ObtenerTodosAsync()
    {
        try
        {
            var usuarios = await context.Usuarios
                .AsNoTracking()
                .Include(u => u.TipoUsuario)
                .Select(u => Mapear(u))
                .ToListAsync();

            return Result<IEnumerable<UsuarioResponseDto>>.Ok(usuarios);
        }
        catch (Exception ex)
        {
            return Result<IEnumerable<UsuarioResponseDto>>.Falla($"Error interno al obtener usuarios: {ex.Message}");
        }
    }

    public async Task<Result<UsuarioResponseDto>> ObtenerPorIdAsync(long id)
    {
        try
        {
            if (id <= 0)
                return Result<UsuarioResponseDto>.Falla("El identificador del usuario debe ser mayor a cero.");

            var usuario = await context.Usuarios
                .AsNoTracking()
                .Include(u => u.TipoUsuario)
                .FirstOrDefaultAsync(u => u.Id == id);

            if (usuario is null)
                return Result<UsuarioResponseDto>.Falla($"No se encontró el usuario con ID {id}.");

            return Result<UsuarioResponseDto>.Ok(Mapear(usuario));
        }
        catch (Exception ex)
        {
            return Result<UsuarioResponseDto>.Falla($"Error interno al obtener el usuario: {ex.Message}");
        }
    }

    public async Task<Result<UsuarioResponseDto>> ObtenerPorUsernameAsync(string username)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(username))
                return Result<UsuarioResponseDto>.Falla("El nombre de usuario no puede estar vacío.");

            var usernameNormalizado = username.Trim();
            var usuario = await context.Usuarios
                .AsNoTracking()
                .Include(u => u.TipoUsuario)
                .FirstOrDefaultAsync(u => u.Username.ToLower() == usernameNormalizado.ToLower());

            if (usuario is null)
                return Result<UsuarioResponseDto>.Falla($"No se encontró el usuario con username '{usernameNormalizado}'.");

            return Result<UsuarioResponseDto>.Ok(Mapear(usuario));
        }
        catch (Exception ex)
        {
            return Result<UsuarioResponseDto>.Falla($"Error interno al obtener usuario por username: {ex.Message}");
        }
    }

    public async Task<Result<UsuarioResponseDto>> AutenticarAsync(string username, string password)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
                return Result<UsuarioResponseDto>.Falla("Debe ingresar usuario y contraseña.");

            var usernameNormalizado = username.Trim();
            var usuario = await context.Usuarios
                .AsNoTracking()
                .Include(u => u.TipoUsuario)
                .FirstOrDefaultAsync(u => u.Username.ToLower() == usernameNormalizado.ToLower());

            if (usuario is null)
                return Result<UsuarioResponseDto>.Falla("Credenciales inválidas.");

            if (!usuario.Activo)
                return Result<UsuarioResponseDto>.Falla("El usuario se encuentra inactivo en el sistema.");

            if (!PasswordHasher.Verificar(password, usuario.PasswordHash))
                return Result<UsuarioResponseDto>.Falla("Credenciales inválidas.");

            return Result<UsuarioResponseDto>.Ok(Mapear(usuario), "Autenticación exitosa.");
        }
        catch (Exception ex)
        {
            return Result<UsuarioResponseDto>.Falla($"Error interno al autenticar usuario: {ex.Message}");
        }
    }

    public async Task<Result<long>> CrearAsync(UsuarioRequestDto request)
    {
        try
        {
            if (request.IdTipoUsuario <= 0)
                return Result<long>.Falla("El identificador del tipo de usuario debe ser mayor a cero.");

            if (string.IsNullOrWhiteSpace(request.Username))
                return Result<long>.Falla("El nombre de usuario es obligatorio.");

            if (string.IsNullOrWhiteSpace(request.Password))
                return Result<long>.Falla("La contraseña es obligatoria.");

            if (string.IsNullOrWhiteSpace(request.Nombre))
                return Result<long>.Falla("El nombre del usuario es obligatorio.");

            if (string.IsNullOrWhiteSpace(request.Apellido))
                return Result<long>.Falla("El apellido del usuario es obligatorio.");

            if (string.IsNullOrWhiteSpace(request.DNI))
                return Result<long>.Falla("El DNI del usuario es obligatorio.");

            var tipoExiste = await context.TiposUsuario.AnyAsync(r => r.Id == request.IdTipoUsuario);
            if (!tipoExiste)
                return Result<long>.Falla($"No existe un tipo de usuario registrado con ID {request.IdTipoUsuario}.");

            var usernameNormalizado = request.Username.Trim();
            var existeUsername = await context.Usuarios
                .AnyAsync(u => u.Username.ToLower() == usernameNormalizado.ToLower());

            if (existeUsername)
                return Result<long>.Falla($"El nombre de usuario '{usernameNormalizado}' ya está en uso.");

            var entidad = new Usuario
            {
                IdTipoUsuario = request.IdTipoUsuario,
                Username = usernameNormalizado,
                PasswordHash = PasswordHasher.Hash(request.Password),
                Nombre = request.Nombre.Trim(),
                Apellido = request.Apellido.Trim(),
                DNI = request.DNI.Trim(),
                Matricula = string.IsNullOrWhiteSpace(request.Matricula) ? null : request.Matricula.Trim(),
                Activo = true
            };

            context.Usuarios.Add(entidad);
            await context.SaveChangesAsync();

            return Result<long>.Ok(entidad.Id, "Usuario creado exitosamente.");
        }
        catch (Exception ex)
        {
            return Result<long>.Falla($"Error interno al crear el usuario: {ex.Message}");
        }
    }

    public async Task<Result> ActualizarAsync(long id, UsuarioRequestDto request)
    {
        try
        {
            if (id <= 0)
                return Result.Falla("El identificador del usuario debe ser mayor a cero.");

            if (request.IdTipoUsuario <= 0)
                return Result.Falla("El identificador del tipo de usuario debe ser mayor a cero.");

            if (string.IsNullOrWhiteSpace(request.Username))
                return Result.Falla("El nombre de usuario es obligatorio.");

            if (string.IsNullOrWhiteSpace(request.Nombre))
                return Result.Falla("El nombre del usuario es obligatorio.");

            if (string.IsNullOrWhiteSpace(request.Apellido))
                return Result.Falla("El apellido del usuario es obligatorio.");

            if (string.IsNullOrWhiteSpace(request.DNI))
                return Result.Falla("El DNI del usuario es obligatorio.");

            var entidad = await context.Usuarios.FirstOrDefaultAsync(u => u.Id == id);
            if (entidad is null)
                return Result.Falla($"No se encontró el usuario con ID {id}.");

            var tipoExiste = await context.TiposUsuario.AnyAsync(r => r.Id == request.IdTipoUsuario);
            if (!tipoExiste)
                return Result.Falla($"No existe un tipo de usuario registrado con ID {request.IdTipoUsuario}.");

            var usernameNormalizado = request.Username.Trim();
            var existeUsername = await context.Usuarios
                .AnyAsync(u => u.Id != id && u.Username.ToLower() == usernameNormalizado.ToLower());

            if (existeUsername)
                return Result.Falla($"El nombre de usuario '{usernameNormalizado}' ya está en uso por otro usuario.");

            entidad.IdTipoUsuario = request.IdTipoUsuario;
            entidad.Username = usernameNormalizado;
            entidad.Nombre = request.Nombre.Trim();
            entidad.Apellido = request.Apellido.Trim();
            entidad.DNI = request.DNI.Trim();
            entidad.Matricula = string.IsNullOrWhiteSpace(request.Matricula) ? null : request.Matricula.Trim();

            if (!string.IsNullOrWhiteSpace(request.Password))
                entidad.PasswordHash = PasswordHasher.Hash(request.Password);

            await context.SaveChangesAsync();
            return Result.Ok("Usuario actualizado exitosamente.");
        }
        catch (Exception ex)
        {
            return Result.Falla($"Error interno al actualizar el usuario: {ex.Message}");
        }
    }

    public async Task<Result> EliminarAsync(long id)
    {
        try
        {
            if (id <= 0)
                return Result.Falla("El identificador del usuario debe ser mayor a cero.");

            var entidad = await context.Usuarios.FirstOrDefaultAsync(u => u.Id == id);
            if (entidad is null)
                return Result.Falla($"No se encontró el usuario con ID {id}.");

            entidad.Activo = false;
            await context.SaveChangesAsync();
            return Result.Ok("Usuario eliminado exitosamente.");
        }
        catch (Exception ex)
        {
            return Result.Falla($"Error interno al eliminar el usuario: {ex.Message}");
        }
    }

    private static UsuarioResponseDto Mapear(Usuario usuario) => new()
    {
        Id = usuario.Id,
        IdTipoUsuario = usuario.IdTipoUsuario,
        NombreTipoUsuario = usuario.TipoUsuario != null ? usuario.TipoUsuario.Nombre : string.Empty,
        Username = usuario.Username,
        Nombre = usuario.Nombre,
        Apellido = usuario.Apellido,
        DNI = usuario.DNI,
        Matricula = usuario.Matricula,
        Activo = usuario.Activo
    };
}
