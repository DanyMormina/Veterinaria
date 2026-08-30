using Veterinaria.CrossCutting.Comunes;
using Veterinaria.Domain.Dtos;

namespace Veterinaria.Interfaces.Interfaces;

/// <summary>
/// Contrato para el servicio de gestión de Especies.
/// </summary>
public interface IEspecieService
{
    Task<Result<IEnumerable<EspecieResponseDto>>> ObtenerTodosAsync();
    Task<Result<EspecieResponseDto>> ObtenerPorIdAsync(long id);
    Task<Result<long>> CrearAsync(EspecieRequestDto request);
    Task<Result> ActualizarAsync(long id, EspecieRequestDto request);
    Task<Result> EliminarAsync(long id);
}
