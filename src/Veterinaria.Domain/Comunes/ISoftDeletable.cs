namespace Veterinaria.Domain.Comunes;
// Contrato para entidades que admiten borrado lógico (Soft Delete).
public interface ISoftDeletable
{
    bool Activo { get; set; }
}
