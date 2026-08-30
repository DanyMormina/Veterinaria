using Veterinaria.CrossCutting.Comunes;
using Veterinaria.Domain.Dtos;

namespace Veterinaria.Interfaces.Interfaces;

/// <summary>
/// Contrato para el servicio de gestión de Detalles de Factura.
/// </summary>
public interface IDetalleFacturaService
{
    Task<Result<IEnumerable<DetalleFacturaResponseDto>>> ObtenerTodosAsync();
    Task<Result<DetalleFacturaResponseDto>> ObtenerPorIdAsync(long id);
    Task<Result<long>> CrearAsync(DetalleFacturaRequestDto request);
    Task<Result> ActualizarAsync(long id, DetalleFacturaRequestDto request);
    Task<Result> EliminarAsync(long id);
}
