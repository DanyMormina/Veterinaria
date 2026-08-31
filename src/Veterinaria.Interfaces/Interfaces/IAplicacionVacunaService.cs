using Veterinaria.CrossCutting.Comunes;
using Veterinaria.Domain.Dtos;

namespace Veterinaria.Interfaces.Interfaces;

public interface IAplicacionVacunaService
{
    Task<Result<IEnumerable<AplicacionVacunaResponseDto>>> ObtenerPorConsultaAsync(long idConsulta);
    Task<Result<AplicacionVacunaResponseDto>> ObtenerPorIdAsync(long id);
    Task<Result<long>> CrearAsync(AplicacionVacunaRequestDto request);
    Task<Result> ActualizarAsync(long id, AplicacionVacunaRequestDto request);
    Task<Result> EliminarAsync(long id);
}
