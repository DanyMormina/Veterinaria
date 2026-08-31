using Veterinaria.CrossCutting.Comunes;
using Veterinaria.Domain.Dtos;

namespace Veterinaria.Interfaces.Interfaces;

public interface IPagoService
{
    Task<Result<IEnumerable<PagoResponseDto>>> ObtenerPorConsultaAsync(long idConsulta);
    Task<Result<IEnumerable<PagoResponseDto>>> ObtenerTodosAsync();
    Task<Result<PagoResponseDto>> ObtenerPorIdAsync(long id);
    Task<Result<long>> RegistrarPagoAsync(PagoRequestDto request);
    Task<Result> AnularPagoAsync(long id);
}
