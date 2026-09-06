namespace Veterinaria.Domain.Dtos;

/// <summary>
/// DTO para la creación y actualización de un Propietario.
/// </summary>
public class PropietarioSolicitudDto
{
    public string DNI { get; set; } = string.Empty;
    public string Nombre { get; set; } = string.Empty;
    public string Apellido { get; set; } = string.Empty;
    public string? Telefono { get; set; }
    public string? CorreoElectronico { get; set; }
    public string? Direccion { get; set; }
}
