using Veterinaria.Domain.Comunes;

namespace Veterinaria.Domain.Entidades;

public class Usuario : EntidadAuditable
{
    public long IdTipoUsuario { get; set; }
    public string NombreUsuario { get; set; } = string.Empty;
    public string HashContrasena { get; set; } = string.Empty;
    public string Nombre { get; set; } = string.Empty;
    public string Apellido { get; set; } = string.Empty;
    public string DNI { get; set; } = string.Empty;
    public string? Direccion { get; set; }
    public string? Telefono { get; set; }
    public string? CorreoElectronico { get; set; }
    public DateTime? FechaNacimiento { get; set; }
    public string? Sexo { get; set; }
    public string? Matricula { get; set; }

    public TipoUsuario TipoUsuario { get; set; } = null!;
    public ICollection<Consulta> Consultas { get; set; } = [];
}