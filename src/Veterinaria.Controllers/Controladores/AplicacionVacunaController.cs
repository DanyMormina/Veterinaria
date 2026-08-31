using Veterinaria.CrossCutting.Comunes;
using Veterinaria.Domain.Dtos;
using Veterinaria.Interfaces.Interfaces;

namespace Veterinaria.Controllers.Controladores;

public class AplicacionVacunaController(IAplicacionVacunaService aplicacionVacunaService)
{
    public async Task<Result<IEnumerable<AplicacionVacunaResponseDto>>> ObtenerPorConsultaAsync(long idConsulta)
    {
        try
        {
            return await aplicacionVacunaService.ObtenerPorConsultaAsync(idConsulta);
        }
        catch (Exception ex)
        {
            return Result<IEnumerable<AplicacionVacunaResponseDto>>.Falla($"Error al obtener aplicaciones de vacunas: {ex.Message}");
        }
    }

    public async Task<Result<AplicacionVacunaResponseDto>> ObtenerPorIdAsync(long id)
    {
        try
        {
            return await aplicacionVacunaService.ObtenerPorIdAsync(id);
        }
        catch (Exception ex)
        {
            return Result<AplicacionVacunaResponseDto>.Falla($"Error al obtener el registro: {ex.Message}");
        }
    }

    public async Task<Result<long>> CrearAsync(AplicacionVacunaRequestDto request)
    {
        try
        {
            return await aplicacionVacunaService.CrearAsync(request);
        }
        catch (Exception ex)
        {
            return Result<long>.Falla($"Error al registrar la aplicación de vacuna: {ex.Message}");
        }
    }

    public async Task<Result> ActualizarAsync(long id, AplicacionVacunaRequestDto request)
    {
        try
        {
            return await aplicacionVacunaService.ActualizarAsync(id, request);
        }
        catch (Exception ex)
        {
            return Result.Falla($"Error al actualizar la aplicación de vacuna: {ex.Message}");
        }
    }

    public async Task<Result> EliminarAsync(long id)
    {
        try
        {
            return await aplicacionVacunaService.EliminarAsync(id);
        }
        catch (Exception ex)
        {
            return Result.Falla($"Error al eliminar la aplicación de vacuna: {ex.Message}");
        }
    }
}
