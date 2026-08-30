namespace Veterinaria.Domain.Dtos;

/// <summary>
/// DTO de respuesta para la entidad Usuario.
/// </summary>
public class UsuarioResponseDto
{
    public long Id { get; set; }
    public long IdRol { get; set; }
    public string NombreRol { get; set; } = string.Empty;
    public string Username { get; set; } = string.Empty;
    public string Nombre { get; set; } = string.Empty;
    public string Apellido { get; set; } = string.Empty;
    public string DNI { get; set; } = string.Empty;
    public string? Matricula { get; set; }
    public bool Activo { get; set; }
}
