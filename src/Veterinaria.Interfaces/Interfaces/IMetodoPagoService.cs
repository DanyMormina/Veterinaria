using Veterinaria.CrossCutting.Comunes;
using Veterinaria.Domain.Dtos;

namespace Veterinaria.Interfaces.Interfaces;

/// <summary>
/// Contrato para el servicio de gestión de Métodos de Pago.
/// </summary>
public interface IMetodoPagoService
{
    Task<Result<IEnumerable<MetodoPagoResponseDto>>> ObtenerTodosAsync();
    Task<Result<MetodoPagoResponseDto>> ObtenerPorIdAsync(long id);
    Task<Result<long>> CrearAsync(MetodoPagoRequestDto request);
    Task<Result> ActualizarAsync(long id, MetodoPagoRequestDto request);
    Task<Result> EliminarAsync(long id);
}
