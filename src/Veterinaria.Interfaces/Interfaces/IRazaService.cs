using Veterinaria.CrossCutting.Comunes;
using Veterinaria.Domain.Dtos;

namespace Veterinaria.Interfaces.Interfaces;

/// <summary>
/// Contrato para el servicio de gestión de Razas.
/// </summary>
public interface IRazaService
{
    Task<Result<IEnumerable<RazaResponseDto>>> ObtenerTodosAsync();
    Task<Result<RazaResponseDto>> ObtenerPorIdAsync(long id);
    Task<Result<long>> CrearAsync(RazaRequestDto request);
    Task<Result> ActualizarAsync(long id, RazaRequestDto request);
    Task<Result> EliminarAsync(long id);
}
