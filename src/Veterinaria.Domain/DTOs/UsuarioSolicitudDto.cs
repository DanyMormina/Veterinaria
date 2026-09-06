namespace Veterinaria.Domain.Dtos;

public class UsuarioSolicitudDto
{
    public long IdTipoUsuario { get; set; }
    public string NombreUsuario { get; set; } = string.Empty;
    public string Contrasena { get; set; } = string.Empty;
    public string Nombre { get; set; } = string.Empty;
    public string Apellido { get; set; } = string.Empty;
    public string DNI { get; set; } = string.Empty;
    public string? Matricula { get; set; }
}
