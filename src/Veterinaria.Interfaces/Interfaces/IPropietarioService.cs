using Veterinaria.CrossCutting.Comunes;
using Veterinaria.Domain.Dtos;

namespace Veterinaria.Interfaces.Interfaces;

/// <summary>
/// Contrato para el servicio de gestión de Propietarios.
/// </summary>
public interface IPropietarioService
{
    Task<Result<IEnumerable<PropietarioResponseDto>>> ObtenerTodosAsync();
    Task<Result<PropietarioResponseDto>> ObtenerPorIdAsync(long id);
    Task<Result<long>> CrearAsync(PropietarioRequestDto request);
    Task<Result> ActualizarAsync(long id, PropietarioRequestDto request);
    Task<Result> EliminarAsync(long id);
}
