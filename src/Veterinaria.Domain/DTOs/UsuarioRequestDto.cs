namespace Veterinaria.Domain.Dtos;

public class UsuarioRequestDto
{
    public long IdTipoUsuario { get; set; }
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string Nombre { get; set; } = string.Empty;
    public string Apellido { get; set; } = string.Empty;
    public string DNI { get; set; } = string.Empty;
    public string? Matricula { get; set; }
}
