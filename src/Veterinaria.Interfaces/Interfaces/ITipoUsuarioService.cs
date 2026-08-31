using Veterinaria.CrossCutting.Comunes;
using Veterinaria.Domain.Dtos;

namespace Veterinaria.Interfaces.Interfaces;

public interface ITipoUsuarioService
{
    Task<Result<IEnumerable<TipoUsuarioResponseDto>>> ObtenerTodosAsync();
    Task<Result<TipoUsuarioResponseDto>> ObtenerPorIdAsync(long id);
    Task<Result<long>> CrearAsync(TipoUsuarioRequestDto request);
    Task<Result> ActualizarAsync(long id, TipoUsuarioRequestDto request);
    Task<Result> EliminarAsync(long id);
}
