using Veterinaria.CrossCutting.Comunes;
using Veterinaria.Domain.Dtos;

namespace Veterinaria.Interfaces.Interfaces;

/// <summary>
/// Contrato para el servicio de gestión de Tratamientos.
/// </summary>
public interface ITratamientoService
{
    Task<Result<IEnumerable<TratamientoResponseDto>>> ObtenerTodosAsync();
    Task<Result<TratamientoResponseDto>> ObtenerPorIdAsync(long id);
    Task<Result<long>> CrearAsync(TratamientoRequestDto request);
    Task<Result> ActualizarAsync(long id, TratamientoRequestDto request);
    Task<Result> EliminarAsync(long id);
}
