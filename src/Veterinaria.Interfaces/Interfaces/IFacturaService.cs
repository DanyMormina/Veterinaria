using Veterinaria.CrossCutting.Comunes;
using Veterinaria.Domain.Dtos;

namespace Veterinaria.Interfaces.Interfaces;

/// <summary>
/// Contrato para el servicio de gestión de Facturas.
/// </summary>
public interface IFacturaService
{
    Task<Result<IEnumerable<FacturaResponseDto>>> ObtenerTodosAsync();
    Task<Result<FacturaResponseDto>> ObtenerPorIdAsync(long id);
    Task<Result<long>> CrearAsync(FacturaRequestDto request);
    Task<Result> ActualizarAsync(long id, FacturaRequestDto request);
    Task<Result> EliminarAsync(long id);
}
