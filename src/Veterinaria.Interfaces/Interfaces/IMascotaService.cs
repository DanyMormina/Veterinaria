using Veterinaria.CrossCutting.Comunes;
using Veterinaria.Domain.Dtos;

namespace Veterinaria.Interfaces.Interfaces;

/// <summary>
/// Contrato para el servicio de gestión de Mascotas.
/// </summary>
public interface IMascotaService
{
    Task<Result<IEnumerable<MascotaResponseDto>>> ObtenerTodosAsync();
    Task<Result<MascotaResponseDto>> ObtenerPorIdAsync(long id);
    Task<Result<long>> CrearAsync(MascotaRequestDto request);
    Task<Result> ActualizarAsync(long id, MascotaRequestDto request);
    Task<Result> EliminarAsync(long id);
}
