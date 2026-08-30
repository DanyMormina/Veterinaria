using Veterinaria.CrossCutting.Comunes;
using Veterinaria.Domain.Dtos;
using Veterinaria.Interfaces.Interfaces;

namespace Veterinaria.Controllers.Controladores;

/// <summary>
/// Controlador para la gestión de Consultas Clínicas.
/// </summary>
public class ConsultaController(IConsultaService consultaService)
{
    public async Task<Result<IEnumerable<ConsultaResponseDto>>> ObtenerTodosAsync()
    {
        try
        {
            return await consultaService.ObtenerTodosAsync();
        }
        catch (Exception ex)
        {
            return Result<IEnumerable<ConsultaResponseDto>>.Falla($"Error interno al obtener consultas: {ex.Message}");
        }
    }

    public async Task<Result<ConsultaResponseDto>> ObtenerPorIdAsync(long id)
    {
        try
        {
            return await consultaService.ObtenerPorIdAsync(id);
        }
        catch (Exception ex)
        {
            return Result<ConsultaResponseDto>.Falla($"Error interno al obtener la consulta: {ex.Message}");
        }
    }

    public async Task<Result<long>> CrearAsync(ConsultaRequestDto request)
    {
        try
        {
            return await consultaService.CrearAsync(request);
        }
        catch (Exception ex)
        {
            return Result<long>.Falla($"Error interno al registrar la consulta: {ex.Message}");
        }
    }

    public async Task<Result> ActualizarAsync(long id, ConsultaRequestDto request)
    {
        try
        {
            return await consultaService.ActualizarAsync(id, request);
        }
        catch (Exception ex)
        {
            return Result.Falla($"Error interno al actualizar la consulta: {ex.Message}");
        }
    }

    public async Task<Result> EliminarAsync(long id)
    {
        try
        {
            return await consultaService.EliminarAsync(id);
        }
        catch (Exception ex)
        {
            return Result.Falla($"Error interno al eliminar la consulta: {ex.Message}");
        }
    }
}
