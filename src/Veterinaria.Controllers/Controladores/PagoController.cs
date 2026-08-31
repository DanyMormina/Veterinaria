using Veterinaria.CrossCutting.Comunes;
using Veterinaria.Domain.Dtos;
using Veterinaria.Interfaces.Interfaces;

namespace Veterinaria.Controllers.Controladores;

public class PagoController(IPagoService pagoService)
{
    public async Task<Result<IEnumerable<PagoResponseDto>>> ObtenerPorConsultaAsync(long idConsulta)
    {
        try
        {
            return await pagoService.ObtenerPorConsultaAsync(idConsulta);
        }
        catch (Exception ex)
        {
            return Result<IEnumerable<PagoResponseDto>>.Falla($"Error al obtener pagos de la consulta: {ex.Message}");
        }
    }

    public async Task<Result<IEnumerable<PagoResponseDto>>> ObtenerTodosAsync()
    {
        try
        {
            return await pagoService.ObtenerTodosAsync();
        }
        catch (Exception ex)
        {
            return Result<IEnumerable<PagoResponseDto>>.Falla($"Error al obtener pagos: {ex.Message}");
        }
    }

    public async Task<Result<PagoResponseDto>> ObtenerPorIdAsync(long id)
    {
        try
        {
            return await pagoService.ObtenerPorIdAsync(id);
        }
        catch (Exception ex)
        {
            return Result<PagoResponseDto>.Falla($"Error al obtener el pago: {ex.Message}");
        }
    }

    public async Task<Result<long>> RegistrarPagoAsync(PagoRequestDto request)
    {
        try
        {
            return await pagoService.RegistrarPagoAsync(request);
        }
        catch (Exception ex)
        {
            return Result<long>.Falla($"Error al registrar el pago: {ex.Message}");
        }
    }

    public async Task<Result> AnularPagoAsync(long id)
    {
        try
        {
            return await pagoService.AnularPagoAsync(id);
        }
        catch (Exception ex)
        {
            return Result.Falla($"Error al anular el pago: {ex.Message}");
        }
    }
}
