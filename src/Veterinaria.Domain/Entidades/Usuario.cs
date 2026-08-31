using Veterinaria.Domain.Comunes;

namespace Veterinaria.Domain.Entidades;

public class Usuario : Auditable
{
    public long IdTipoUsuario { get; set; }
    public string Username { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public string Nombre { get; set; } = string.Empty;
    public string Apellido { get; set; } = string.Empty;
    public string DNI { get; set; } = string.Empty;
    public string? Matricula { get; set; }

    public TipoUsuario TipoUsuario { get; set; } = null!;
    public ICollection<Consulta> Consultas { get; set; } = [];
}