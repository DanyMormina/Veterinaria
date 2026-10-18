namespace Veterinaria.Domain.Comunes;

public class EntidadAuditable : ISoftDeletable
{
    public long Id { get; set; }
    public bool Activo { get; set; }
}