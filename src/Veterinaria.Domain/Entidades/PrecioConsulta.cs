namespace Veterinaria.Domain.Entidades;

/// <summary>
/// Precio fijo de la consulta clínica según la especie de la mascota. Se modifica solo desde la base de datos.
/// </summary>
public class PrecioConsulta
{
    public long Id { get; set; }
    public long IdEspecie { get; set; }
    public decimal Precio { get; set; }

    public Especie Especie { get; set; } = null!;
}
