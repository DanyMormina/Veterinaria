using Veterinaria.CrossCutting.Comunes;
using Veterinaria.Domain.Dtos;

namespace Veterinaria.Interfaces.Interfaces;

public interface IDetalleConsultaService
{
    Task<Result<IEnumerable<DetalleConsultaResponseDto>>> ObtenerPorConsultaAsync(long idConsulta);
    Task<Result<DetalleConsultaResponseDto>> ObtenerPorIdAsync(long id);
    Task<Result<long>> CrearAsync(DetalleConsultaRequestDto request);
    Task<Result> ActualizarAsync(long id, DetalleConsultaRequestDto request);
    Task<Result> EliminarAsync(long id);
}
