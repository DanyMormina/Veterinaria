using Veterinaria.CrossCutting.Comunes;
using Veterinaria.Domain.Dtos;

namespace Veterinaria.Interfaces.Interfaces;

/// <summary>
/// Contrato para el servicio de gestión de Consultas Médicas.
/// </summary>
public interface IConsultaService
{
    Task<Result<IEnumerable<ConsultaResponseDto>>> ObtenerTodosAsync();
    Task<Result<ConsultaResponseDto>> ObtenerPorIdAsync(long id);
    Task<Result<long>> CrearAsync(ConsultaRequestDto request);
    Task<Result> ActualizarAsync(long id, ConsultaRequestDto request);
    Task<Result> EliminarAsync(long id);
}
