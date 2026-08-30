using Veterinaria.CrossCutting.Comunes;
using Veterinaria.Domain.Dtos;
using Veterinaria.Interfaces.Interfaces;

namespace Veterinaria.Controllers.Controladores;

/// <summary>
/// Controlador para la gestión de Mascotas.
/// </summary>
public class MascotaController(IMascotaService mascotaService)
{
    public async Task<Result<IEnumerable<MascotaResponseDto>>> ObtenerTodosAsync()
    {
        try
        {
            return await mascotaService.ObtenerTodosAsync();
        }
        catch (Exception ex)
        {
            return Result<IEnumerable<MascotaResponseDto>>.Falla($"Error interno al obtener mascotas: {ex.Message}");
        }
    }

    public async Task<Result<MascotaResponseDto>> ObtenerPorIdAsync(long id)
    {
        try
        {
            return await mascotaService.ObtenerPorIdAsync(id);
        }
        catch (Exception ex)
        {
            return Result<MascotaResponseDto>.Falla($"Error interno al obtener la mascota: {ex.Message}");
        }
    }

    public async Task<Result<long>> CrearAsync(MascotaRequestDto request)
    {
        try
        {
            return await mascotaService.CrearAsync(request);
        }
        catch (Exception ex)
        {
            return Result<long>.Falla($"Error interno al registrar la mascota: {ex.Message}");
        }
    }

    public async Task<Result> ActualizarAsync(long id, MascotaRequestDto request)
    {
        try
        {
            return await mascotaService.ActualizarAsync(id, request);
        }
        catch (Exception ex)
        {
            return Result.Falla($"Error interno al actualizar la mascota: {ex.Message}");
        }
    }

    public async Task<Result> EliminarAsync(long id)
    {
        try
        {
            return await mascotaService.EliminarAsync(id);
        }
        catch (Exception ex)
        {
            return Result.Falla($"Error interno al eliminar la mascota: {ex.Message}");
        }
    }
}
