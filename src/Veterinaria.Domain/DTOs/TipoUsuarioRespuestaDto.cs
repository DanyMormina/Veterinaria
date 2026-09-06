namespace Veterinaria.Domain.Dtos;

public class TipoUsuarioRespuestaDto
{
    public long Id { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public bool Activo { get; set; }
}
