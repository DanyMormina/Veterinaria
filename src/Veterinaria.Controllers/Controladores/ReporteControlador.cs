using System.Data;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Veterinaria.Domain.Comunes;
using Veterinaria.Domain.Dtos;
using Veterinaria.Infrastructure;

namespace Veterinaria.Controllers.Controladores;

/// <summary>
/// Controlador de reporting clínico y demográfico para la gestión administrativa de la veterinaria.
/// Los reportes principales se resuelven con procedimientos almacenados (dbo.sp_Reporte*)
/// creados por <see cref="ObjetosBaseDatos"/> al iniciar la aplicación.
/// </summary>
public class ReporteControlador(ContextoVeterinaria context)
{
    /// <summary>
    /// Obtiene la lista de veterinarios activos para poblar el selector de profesionales.
    /// </summary>
    public async Task<Resultado<IEnumerable<UsuarioRespuestaDto>>> ObtenerVeterinariosActivosAsync()
    {
        try
        {
            // El rol de veterinario corresponde al TipoUsuario con nombre "Veterinario"
            var veterinarios = await context.Usuarios
                .AsNoTracking()
                .Include(u => u.TipoUsuario)
                .Where(u => u.Activo && (u.TipoUsuario.Nombre == "Veterinario" || u.IdTipoUsuario == 2))
                .OrderBy(u => u.Nombre)
                .ThenBy(u => u.Apellido)
                .Select(u => new UsuarioRespuestaDto
                {
                    Id = u.Id,
                    Nombre = u.Nombre,
                    Apellido = u.Apellido,
                    DNI = u.DNI,
                    NombreUsuario = u.NombreUsuario,
                    IdTipoUsuario = u.IdTipoUsuario,
                    NombreTipoUsuario = u.TipoUsuario.Nombre,
                    Activo = u.Activo
                })
                .ToListAsync();

            return Resultado<IEnumerable<UsuarioRespuestaDto>>.Exito(veterinarios);
        }
        catch (Exception ex)
        {
            return Resultado<IEnumerable<UsuarioRespuestaDto>>.Falla($"Error al obtener profesionales veterinarios: {ex.Message}");
        }
    }

    /// <summary>
    /// Genera el reporte de Consultas Clínicas realizadas en el rango de fechas con filtros opcionales.
    /// Procedimiento: dbo.sp_ReporteConsultasClinicas.
    /// </summary>
    public async Task<Resultado<IEnumerable<ReporteConsultaClinicaDto>>> ObtenerReporteConsultasClinicasAsync(FiltroReporteDto filtro)
    {
        try
        {
            var resultados = await context.Database
                .SqlQueryRaw<ReporteConsultaClinicaDto>(
                    "EXEC dbo.sp_ReporteConsultasClinicas @FechaDesde, @FechaHasta, @IdVeterinario, @IdEspecie",
                    ParametrosRango(filtro.FechaDesde, filtro.FechaHasta, filtro.IdVeterinario, filtro.IdEspecie))
                .ToListAsync();

            return Resultado<IEnumerable<ReporteConsultaClinicaDto>>.Exito(resultados);
        }
        catch (Exception ex)
        {
            return Resultado<IEnumerable<ReporteConsultaClinicaDto>>.Falla($"Error al generar reporte de consultas clínicas: {ex.Message}");
        }
    }

    /// <summary>
    /// Reporte cruzado: cada consulta del período con la cantidad e importe de sus tratamientos y vacunas.
    /// Procedimiento: dbo.sp_ReporteConsultasDetalladas.
    /// </summary>
    public async Task<Resultado<IEnumerable<ReporteConsultaDetalladaDto>>> ObtenerReporteConsultasDetalladasAsync(FiltroReporteDto filtro)
    {
        try
        {
            var resultados = await context.Database
                .SqlQueryRaw<ReporteConsultaDetalladaDto>(
                    "EXEC dbo.sp_ReporteConsultasDetalladas @FechaDesde, @FechaHasta, @IdVeterinario, @IdEspecie",
                    ParametrosRango(filtro.FechaDesde, filtro.FechaHasta, filtro.IdVeterinario, filtro.IdEspecie))
                .ToListAsync();

            return Resultado<IEnumerable<ReporteConsultaDetalladaDto>>.Exito(resultados);
        }
        catch (Exception ex)
        {
            return Resultado<IEnumerable<ReporteConsultaDetalladaDto>>.Falla($"Error al generar reporte de consultas con tratamientos y vacunas: {ex.Message}");
        }
    }

    /// <summary>
    /// Genera el reporte de cobros (pagos) en un rango de fechas, incluyendo anulados.
    /// Procedimiento: dbo.sp_ReporteCobros.
    /// </summary>
    public async Task<Resultado<IEnumerable<ReporteCobroDto>>> ObtenerReporteCobrosAsync(FiltroReporteCobroDto filtro)
    {
        try
        {
            string? estado = null;
            if (!string.IsNullOrWhiteSpace(filtro.Estado) &&
                !filtro.Estado.Equals("(Todos)", StringComparison.OrdinalIgnoreCase))
            {
                estado = filtro.Estado.Trim();
                if (estado.Equals("Pagado", StringComparison.OrdinalIgnoreCase))
                    estado = "Completado";
            }

            var parametros = ParametrosRango(filtro.FechaDesde, filtro.FechaHasta, filtro.IdVeterinario, filtro.IdEspecie)
                .Append(new SqlParameter("@Estado", SqlDbType.NVarChar, 30) { Value = (object?)estado ?? DBNull.Value })
                .ToArray();

            var resultados = await context.Database
                .SqlQueryRaw<ReporteCobroDto>(
                    "EXEC dbo.sp_ReporteCobros @FechaDesde, @FechaHasta, @Estado, @IdVeterinario, @IdEspecie",
                    parametros)
                .ToListAsync();

            return Resultado<IEnumerable<ReporteCobroDto>>.Exito(resultados);
        }
        catch (Exception ex)
        {
            return Resultado<IEnumerable<ReporteCobroDto>>.Falla($"Error al generar reporte de cobros: {ex.Message}");
        }
    }

    /// <summary>
    /// Genera el reporte consolidado de Vacunas y Tratamientos clínicos aplicados en el período.
    /// Procedimiento: dbo.sp_ReporteVacunasTratamientos.
    /// </summary>
    public async Task<Resultado<IEnumerable<ReporteVacunaTratamientoDto>>> ObtenerReporteVacunasTratamientosAsync(FiltroReporteDto filtro)
    {
        try
        {
            var resultados = await context.Database
                .SqlQueryRaw<ReporteVacunaTratamientoDto>(
                    "EXEC dbo.sp_ReporteVacunasTratamientos @FechaDesde, @FechaHasta, @IdVeterinario, @IdEspecie",
                    ParametrosRango(filtro.FechaDesde, filtro.FechaHasta, filtro.IdVeterinario, filtro.IdEspecie))
                .ToListAsync();

            return Resultado<IEnumerable<ReporteVacunaTratamientoDto>>.Exito(resultados);
        }
        catch (Exception ex)
        {
            return Resultado<IEnumerable<ReporteVacunaTratamientoDto>>.Falla($"Error al generar reporte de vacunas y tratamientos: {ex.Message}");
        }
    }

    /// <summary>
    /// Genera el reporte preventivo de Próximos Controles Sanitarios a vencer en el rango indicado.
    /// </summary>
    public async Task<Resultado<IEnumerable<ReporteProximoControlDto>>> ObtenerReporteProximosControlesAsync(FiltroReporteDto filtro)
    {
        try
        {
            var fechaFin = filtro.FechaHasta.Date.AddDays(1).AddTicks(-1);

            var query = context.AplicacionesVacuna
                .AsNoTracking()
                .Where(v => v.ProximaDosis.HasValue &&
                            v.ProximaDosis.Value >= filtro.FechaDesde.Date &&
                            v.ProximaDosis.Value <= fechaFin);

            if (filtro.IdEspecie.HasValue && filtro.IdEspecie.Value > 0)
            {
                query = query.Where(v => v.Consulta.Mascota.Raza.IdEspecie == filtro.IdEspecie.Value);
            }

            var resultados = await query
                .OrderBy(v => v.ProximaDosis!.Value)
                .Select(v => new ReporteProximoControlDto
                {
                    FechaProximoControl = v.ProximaDosis!.Value,
                    Mascota = v.Consulta.Mascota.Nombre,
                    Especie = v.Consulta.Mascota.Raza.Especie.Nombre,
                    Propietario = v.Consulta.Mascota.Propietario.Nombre + " " + v.Consulta.Mascota.Propietario.Apellido,
                    Telefono = string.IsNullOrWhiteSpace(v.Consulta.Mascota.Propietario.Telefono)
                        ? "-"
                        : v.Consulta.Mascota.Propietario.Telefono,
                    TratamientoAsociado = "Refuerzo Vacuna " + v.Vacuna.Nombre
                })
                .ToListAsync();

            return Resultado<IEnumerable<ReporteProximoControlDto>>.Exito(resultados);
        }
        catch (Exception ex)
        {
            return Resultado<IEnumerable<ReporteProximoControlDto>>.Falla($"Error al generar reporte de próximos controles: {ex.Message}");
        }
    }

    /// <summary>
    /// Genera el Censo Demográfico de Pacientes (Mascotas registradas activas) con filtro de especie.
    /// Procedimiento: dbo.sp_ReporteCensoMascotas.
    /// </summary>
    public async Task<Resultado<IEnumerable<ReporteCensoMascotaDto>>> ObtenerReporteCensoMascotasAsync(FiltroReporteDto filtro)
    {
        try
        {
            var resultados = await context.Database
                .SqlQueryRaw<ReporteCensoMascotaDto>(
                    "EXEC dbo.sp_ReporteCensoMascotas @IdEspecie",
                    ParametroId("@IdEspecie", filtro.IdEspecie))
                .ToListAsync();

            return Resultado<IEnumerable<ReporteCensoMascotaDto>>.Exito(resultados);
        }
        catch (Exception ex)
        {
            return Resultado<IEnumerable<ReporteCensoMascotaDto>>.Falla($"Error al generar censo de pacientes: {ex.Message}");
        }
    }

    private static SqlParameter[] ParametrosRango(DateTime desde, DateTime hasta, long? idVeterinario, long? idEspecie) =>
    [
        new SqlParameter("@FechaDesde", SqlDbType.Date) { Value = desde.Date },
        new SqlParameter("@FechaHasta", SqlDbType.Date) { Value = hasta.Date },
        ParametroId("@IdVeterinario", idVeterinario),
        ParametroId("@IdEspecie", idEspecie)
    ];

    private static SqlParameter ParametroId(string nombre, long? id) =>
        new(nombre, SqlDbType.BigInt) { Value = id is > 0 ? id.Value : DBNull.Value as object };
}
