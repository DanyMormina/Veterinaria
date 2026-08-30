using Veterinaria.CrossCutting.Comunes;
using Veterinaria.Domain.Dtos;

namespace Veterinaria.Interfaces.Interfaces;

/// <summary>
/// Contrato para el servicio de gestión de Vacunas.
/// </summary>
public interface IVacunaService
{
    Task<Result<IEnumerable<VacunaResponseDto>>> ObtenerTodosAsync();
    Task<Result<VacunaResponseDto>> ObtenerPorIdAsync(long id);
    Task<Result<long>> CrearAsync(VacunaRequestDto request);
    Task<Result> ActualizarAsync(long id, VacunaRequestDto request);
    Task<Result> EliminarAsync(long id);
}
