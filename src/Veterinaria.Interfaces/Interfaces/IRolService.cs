using Veterinaria.CrossCutting.Comunes;
using Veterinaria.Domain.Dtos;

namespace Veterinaria.Interfaces.Interfaces;

/// <summary>
/// Contrato para el servicio de gestión de Roles.
/// </summary>
public interface IRolService
{
    Task<Result<IEnumerable<RolResponseDto>>> ObtenerTodosAsync();
    Task<Result<RolResponseDto>> ObtenerPorIdAsync(long id);
    Task<Result<long>> CrearAsync(RolRequestDto request);
    Task<Result> ActualizarAsync(long id, RolRequestDto request);
    Task<Result> EliminarAsync(long id);
}
