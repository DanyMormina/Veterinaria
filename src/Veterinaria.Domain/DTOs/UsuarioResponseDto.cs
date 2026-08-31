namespace Veterinaria.Domain.Dtos;

public class UsuarioResponseDto
{
    public long Id { get; set; }
    public long IdTipoUsuario { get; set; }
    public string NombreTipoUsuario { get; set; } = string.Empty;
    public string Username { get; set; } = string.Empty;
    public string Nombre { get; set; } = string.Empty;
    public string Apellido { get; set; } = string.Empty;
    public string DNI { get; set; } = string.Empty;
    public string? Matricula { get; set; }
    public bool Activo { get; set; }
    public string NombreRol => NombreTipoUsuario; // Alias para compatibilidad de vistas
}
