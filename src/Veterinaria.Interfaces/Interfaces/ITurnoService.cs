using Veterinaria.CrossCutting.Comunes;
using Veterinaria.Domain.Dtos;

namespace Veterinaria.Interfaces.Interfaces;

/// <summary>
/// Contrato para el servicio de gestión de Turnos.
/// </summary>
public interface ITurnoService
{
    Task<Result<IEnumerable<TurnoResponseDto>>> ObtenerTodosAsync();
    Task<Result<TurnoResponseDto>> ObtenerPorIdAsync(long id);
    Task<Result<long>> CrearAsync(TurnoRequestDto request);
    Task<Result> ActualizarAsync(long id, TurnoRequestDto request);
    Task<Result> EliminarAsync(long id);
}
