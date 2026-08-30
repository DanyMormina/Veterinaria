using System.Diagnostics;
using Microsoft.EntityFrameworkCore;
using Veterinaria.CrossCutting.Comunes;
using Veterinaria.CrossCutting.Security;
using Veterinaria.Domain.Dtos;
using Veterinaria.Domain.Entidades;
using Veterinaria.Infrastructure;
using Veterinaria.Interfaces.Interfaces;

namespace Veterinaria.Services.Servicios;

/// <summary>
/// Servicio para la gestión de Usuarios y Credenciales del sistema.
/// </summary>
public class UsuarioService(VeterinariaDbContext context) : IUsuarioService
{
    public async Task<Result<IEnumerable<UsuarioResponseDto>>> ObtenerTodosAsync()
    {
        var usuarios = await context.Usuarios
            .AsNoTracking()
            .Include(u => u.Rol)
            .Select(u => new UsuarioResponseDto
            {
                Id = u.Id,
                IdRol = u.IdRol,
                NombreRol = u.Rol != null ? u.Rol.Nombre : string.Empty,
                Username = u.Username,
                Nombre = u.Nombre,
                Apellido = u.Apellido,
                DNI = u.DNI,
                Matricula = u.Matricula,
                Activo = u.Activo
            })
            .ToListAsync();

        return Result<IEnumerable<UsuarioResponseDto>>.Ok(usuarios);
    }

    public async Task<Result<UsuarioResponseDto>> ObtenerPorIdAsync(long id)
    {
        if (id <= 0)
            return Result<UsuarioResponseDto>.Falla("El identificador del usuario debe ser mayor a cero.");

        var usuario = await context.Usuarios
            .AsNoTracking()
            .Include(u => u.Rol)
            .Where(u => u.Id == id)
            .Select(u => new UsuarioResponseDto
            {
                Id = u.Id,
                IdRol = u.IdRol,
                NombreRol = u.Rol != null ? u.Rol.Nombre : string.Empty,
                Username = u.Username,
                Nombre = u.Nombre,
                Apellido = u.Apellido,
                DNI = u.DNI,
                Matricula = u.Matricula,
                Activo = u.Activo
            })
            .FirstOrDefaultAsync();

        if (usuario is null)
            return Result<UsuarioResponseDto>.Falla($"No se encontró el usuario con ID {id}.");

        return Result<UsuarioResponseDto>.Ok(usuario);
    }

    public async Task<Result<UsuarioResponseDto>> ObtenerPorUsernameAsync(string username)
    {
        if (string.IsNullOrWhiteSpace(username))
            return Result<UsuarioResponseDto>.Falla("El nombre de usuario no puede estar vacío.");

        var usernameNormalizado = username.Trim();
        var usuario = await context.Usuarios
            .AsNoTracking()
            .Include(u => u.Rol)
            .Where(u => u.Username.ToLower() == usernameNormalizado.ToLower())
            .Select(u => new UsuarioResponseDto
            {
                Id = u.Id,
                IdRol = u.IdRol,
                NombreRol = u.Rol != null ? u.Rol.Nombre : string.Empty,
                Username = u.Username,
                Nombre = u.Nombre,
                Apellido = u.Apellido,
                DNI = u.DNI,
                Matricula = u.Matricula,
                Activo = u.Activo
            })
            .FirstOrDefaultAsync();

        if (usuario is null)
            return Result<UsuarioResponseDto>.Falla($"No se encontró el usuario con username '{usernameNormalizado}'.");

        return Result<UsuarioResponseDto>.Ok(usuario);
    }

    public async Task<Result<UsuarioResponseDto>> AutenticarAsync(string username, string password)
    {
        if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            return Result<UsuarioResponseDto>.Falla("Debe ingresar usuario y contraseña.");

        var usernameNormalizado = username.Trim();

        try
        {
            var usuario = await context.Usuarios
                .AsNoTracking()
                .Include(u => u.Rol)
                .FirstOrDefaultAsync(u => u.Username.ToLower() == usernameNormalizado.ToLower());

            if (usuario is null)
                return Result<UsuarioResponseDto>.Falla("Credenciales inválidas.");

            if (!usuario.Activo)
                return Result<UsuarioResponseDto>.Falla("El usuario se encuentra inactivo en el sistema.");

            if (!PasswordHasher.Verificar(password, usuario.PasswordHash))
                return Result<UsuarioResponseDto>.Falla("Credenciales inválidas.");

            var response = new UsuarioResponseDto
            {
                Id = usuario.Id,
                IdRol = usuario.IdRol,
                NombreRol = usuario.Rol != null ? usuario.Rol.Nombre : string.Empty,
                Username = usuario.Username,
                Nombre = usuario.Nombre,
                Apellido = usuario.Apellido,
                DNI = usuario.DNI,
                Matricula = usuario.Matricula,
                Activo = usuario.Activo
            };

            return Result<UsuarioResponseDto>.Ok(response, "Autenticación exitosa.");
        }
        catch (Exception ex)
        {
            return Result<UsuarioResponseDto>.Falla($"Error interno al autenticar: {ex.Message}");
        }
    }

    public async Task<Result<long>> CrearAsync(UsuarioRequestDto request)
    {
        if (request.IdRol <= 0)
            return Result<long>.Falla("El identificador del rol debe ser mayor a cero.");

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

        var rolExiste = await context.Roles.AnyAsync(r => r.Id == request.IdRol);
        if (!rolExiste)
            return Result<long>.Falla($"No existe un rol registrado con ID {request.IdRol}.");

        var usernameNormalizado = request.Username.Trim();
        var existeUsername = await context.Usuarios
            .AnyAsync(u => u.Username.ToLower() == usernameNormalizado.ToLower());

        if (existeUsername)
            return Result<long>.Falla($"El nombre de usuario '{usernameNormalizado}' ya está en uso.");

        var entidad = new Usuario
        {
            IdRol = request.IdRol,
            Username = usernameNormalizado,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password.Trim()),
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

    public async Task<Result> ActualizarAsync(long id, UsuarioRequestDto request)
    {
        if (id <= 0)
            return Result.Falla("El identificador del usuario debe ser mayor a cero.");

        if (request.IdRol <= 0)
            return Result.Falla("El identificador del rol debe ser mayor a cero.");

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

        var rolExiste = await context.Roles.AnyAsync(r => r.Id == request.IdRol);
        if (!rolExiste)
            return Result.Falla($"No existe un rol registrado con ID {request.IdRol}.");

        var usernameNormalizado = request.Username.Trim();
        var existeUsername = await context.Usuarios
            .AnyAsync(u => u.Id != id && u.Username.ToLower() == usernameNormalizado.ToLower());

        if (existeUsername)
            return Result.Falla($"El nombre de usuario '{usernameNormalizado}' ya está en uso por otro usuario.");

        entidad.IdRol = request.IdRol;
        entidad.Username = usernameNormalizado;
        entidad.Nombre = request.Nombre.Trim();
        entidad.Apellido = request.Apellido.Trim();
        entidad.DNI = request.DNI.Trim();
        entidad.Matricula = string.IsNullOrWhiteSpace(request.Matricula) ? null : request.Matricula.Trim();

        if (!string.IsNullOrWhiteSpace(request.Password))
        {
            entidad.PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password.Trim());
        }

        await context.SaveChangesAsync();

        return Result.Ok("Usuario actualizado exitosamente.");
    }

    public async Task<Result> EliminarAsync(long id)
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
}
