namespace Veterinaria.Domain.Comunes;

/// <summary>
/// Contrato para entidades que admiten borrado lógico (Soft Delete).
/// </summary>
public interface ISoftDeletable
{
    bool Activo { get; set; }
}
