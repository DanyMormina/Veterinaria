namespace Veterinaria.Domain.Dtos;

public class UsuarioRespuestaDto
{
    public long Id { get; set; }
    public long IdTipoUsuario { get; set; }
    public string NombreTipoUsuario { get; set; } = string.Empty;
    public string NombreUsuario { get; set; } = string.Empty;
    public string Nombre { get; set; } = string.Empty;
    public string Apellido { get; set; } = string.Empty;
    public string DNI { get; set; } = string.Empty;
    public string? Direccion { get; set; }
    public string? Telefono { get; set; }
    public string? CorreoElectronico { get; set; }
    public DateTime? FechaNacimiento { get; set; }
    public string? Sexo { get; set; }
    public string? Matricula { get; set; }
    public bool Activo { get; set; }
    public string NombreRol => NombreTipoUsuario; // Alias para compatibilidad de vistas
}
