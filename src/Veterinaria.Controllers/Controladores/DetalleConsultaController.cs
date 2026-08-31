using Veterinaria.CrossCutting.Comunes;
using Veterinaria.Domain.Dtos;
using Veterinaria.Interfaces.Interfaces;

namespace Veterinaria.Controllers.Controladores;

public class DetalleConsultaController(IDetalleConsultaService detalleConsultaService)
{
    public async Task<Result<IEnumerable<DetalleConsultaResponseDto>>> ObtenerPorConsultaAsync(long idConsulta)
    {
        try
        {
            return await detalleConsultaService.ObtenerPorConsultaAsync(idConsulta);
        }
        catch (Exception ex)
        {
            return Result<IEnumerable<DetalleConsultaResponseDto>>.Falla($"Error al obtener los detalles de la consulta: {ex.Message}");
        }
    }

    public async Task<Result<DetalleConsultaResponseDto>> ObtenerPorIdAsync(long id)
    {
        try
        {
            return await detalleConsultaService.ObtenerPorIdAsync(id);
        }
        catch (Exception ex)
        {
            return Result<DetalleConsultaResponseDto>.Falla($"Error al obtener el detalle: {ex.Message}");
        }
    }

    public async Task<Result<long>> CrearAsync(DetalleConsultaRequestDto request)
    {
        try
        {
            return await detalleConsultaService.CrearAsync(request);
        }
        catch (Exception ex)
        {
            return Result<long>.Falla($"Error al agregar detalle a la consulta: {ex.Message}");
        }
    }

    public async Task<Result> ActualizarAsync(long id, DetalleConsultaRequestDto request)
    {
        try
        {
            return await detalleConsultaService.ActualizarAsync(id, request);
        }
        catch (Exception ex)
        {
            return Result.Falla($"Error al actualizar el detalle: {ex.Message}");
        }
    }

    public async Task<Result> EliminarAsync(long id)
    {
        try
        {
            return await detalleConsultaService.EliminarAsync(id);
        }
        catch (Exception ex)
        {
            return Result.Falla($"Error al eliminar el detalle: {ex.Message}");
        }
    }
}
