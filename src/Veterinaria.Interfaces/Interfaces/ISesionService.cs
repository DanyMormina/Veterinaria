using Veterinaria.CrossCutting.Comunes;
using Veterinaria.Domain.Dtos;

namespace Veterinaria.Interfaces.Interfaces;

/// <summary>
/// Contrato para el servicio de gestión de Sesiones.
/// </summary>
public interface ISesionService
{
    Task<Result<IEnumerable<SesionResponseDto>>> ObtenerTodosAsync();
    Task<Result<SesionResponseDto>> ObtenerPorIdAsync(long id);
    Task<Result<long>> CrearAsync(SesionRequestDto request);
    Task<Result> ActualizarAsync(long id, SesionRequestDto request);
    Task<Result> EliminarAsync(long id);
}
