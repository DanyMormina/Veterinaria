using Veterinaria.Domain.Comunes;

namespace Veterinaria.Domain.Entidades
{
    public class Usuario : Auditable
    {
        public long IdRol { get; set; }
        public string Username { get; set; }
        public string PasswordHash { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string DNI { get; set; }
        public string? Matricula { get; set; }
        public Rol Rol { get; set; } = null!;
    }
}