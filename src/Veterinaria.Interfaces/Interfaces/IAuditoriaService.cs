using Veterinaria.CrossCutting.Comunes;
using Veterinaria.Domain.Dtos;

namespace Veterinaria.Interfaces.Interfaces;

/// <summary>
/// Contrato para el servicio de Auditoría del sistema.
/// </summary>
public interface IAuditoriaService
{
    Task<Result<IEnumerable<AuditoriaResponseDto>>> ObtenerTodosAsync();
    Task<Result<AuditoriaResponseDto>> ObtenerPorIdAsync(long id);
    Task<Result<long>> CrearAsync(AuditoriaRequestDto request);
    Task<Result> ActualizarAsync(long id, AuditoriaRequestDto request);
    Task<Result> EliminarAsync(long id);
}
