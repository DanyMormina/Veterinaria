namespace Veterinaria.Domain.Dtos;

/// <summary>
/// DTO para la creación y actualización de un Usuario.
/// </summary>
public class UsuarioRequestDto
{
    public long IdRol { get; set; }
    public string Username { get; set; } = string.Empty;
    public string? Password { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string Apellido { get; set; } = string.Empty;
    public string DNI { get; set; } = string.Empty;
    public string? Matricula { get; set; }
}
