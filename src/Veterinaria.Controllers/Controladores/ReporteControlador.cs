using Microsoft.EntityFrameworkCore;
using Veterinaria.Domain.Comunes;
using Veterinaria.Domain.Dtos;
using Veterinaria.Infrastructure;

namespace Veterinaria.Controllers.Controladores;

/// <summary>
/// Controlador de reporting clínico y demográfico para la gestión administrativa de la veterinaria.
/// Implementa consultas optimizadas con AsNoTracking() bajo el Result Pattern.
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
    /// </summary>
    public async Task<Resultado<IEnumerable<ReporteConsultaClinicaDto>>> ObtenerReporteConsultasClinicasAsync(FiltroReporteDto filtro)
    {
        try
        {
            var fechaFin = filtro.FechaHasta.Date.AddDays(1).AddTicks(-1);

            var query = context.Consultas
                .AsNoTracking()
                .Where(c => c.FechaHora >= filtro.FechaDesde.Date && c.FechaHora <= fechaFin);

            if (filtro.IdVeterinario.HasValue && filtro.IdVeterinario.Value > 0)
            {
                query = query.Where(c => c.IdUsuario == filtro.IdVeterinario.Value);
            }

            if (filtro.IdEspecie.HasValue && filtro.IdEspecie.Value > 0)
            {
                query = query.Where(c => c.Mascota.Raza.IdEspecie == filtro.IdEspecie.Value);
            }

            var resultados = await query
                .OrderByDescending(c => c.FechaHora)
                .Select(c => new ReporteConsultaClinicaDto
                {
                    FechaHora = c.FechaHora,
                    Mascota = c.Mascota.Nombre,
                    Especie = c.Mascota.Raza.Especie.Nombre,
                    Raza = c.Mascota.Raza.Nombre,
                    Propietario = c.Mascota.Propietario.Nombre + " " + c.Mascota.Propietario.Apellido,
                    Veterinario = c.Usuario.Nombre + " " + c.Usuario.Apellido,
                    Diagnostico = c.Diagnostico
                })
                .ToListAsync();

            return Resultado<IEnumerable<ReporteConsultaClinicaDto>>.Exito(resultados);
        }
        catch (Exception ex)
        {
            return Resultado<IEnumerable<ReporteConsultaClinicaDto>>.Falla($"Error al generar reporte de consultas clínicas: {ex.Message}");
        }
    }

    /// <summary>
    /// Genera el reporte consolidado de Vacunas y Tratamientos clínicos aplicados en el período.
    /// </summary>
    public async Task<Resultado<IEnumerable<ReporteVacunaTratamientoDto>>> ObtenerReporteVacunasTratamientosAsync(FiltroReporteDto filtro)
    {
        try
        {
            var fechaFin = filtro.FechaHasta.Date.AddDays(1).AddTicks(-1);

            // 1. Tratamientos clínicos desde DetalleConsulta
            var queryTratamientos = context.DetalleConsultas
                .AsNoTracking()
                .Where(d => d.Consulta.FechaHora >= filtro.FechaDesde.Date && d.Consulta.FechaHora <= fechaFin);

            if (filtro.IdVeterinario.HasValue && filtro.IdVeterinario.Value > 0)
            {
                queryTratamientos = queryTratamientos.Where(d => d.Consulta.IdUsuario == filtro.IdVeterinario.Value);
            }

            if (filtro.IdEspecie.HasValue && filtro.IdEspecie.Value > 0)
            {
                queryTratamientos = queryTratamientos.Where(d => d.Consulta.Mascota.Raza.IdEspecie == filtro.IdEspecie.Value);
            }

            var tratamientos = await queryTratamientos
                .Select(d => new ReporteVacunaTratamientoDto
                {
                    Fecha = d.Consulta.FechaHora,
                    Mascota = d.Consulta.Mascota.Nombre,
                    Especie = d.Consulta.Mascota.Raza.Especie.Nombre,
                    TratamientoVacuna = d.Tratamiento.Descripcion,
                    Tipo = string.IsNullOrWhiteSpace(d.Tratamiento.TipoTratamiento) ? "Tratamiento" : d.Tratamiento.TipoTratamiento,
                    Dosis = string.IsNullOrWhiteSpace(d.Tratamiento.Dosis) ? "-" : d.Tratamiento.Dosis,
                    Veterinario = d.Consulta.Usuario.Nombre + " " + d.Consulta.Usuario.Apellido
                })
                .ToListAsync();

            // 2. Aplicaciones de Vacunas desde AplicacionesVacuna
            var queryVacunas = context.AplicacionesVacuna
                .AsNoTracking()
                .Where(v => v.FechaAplicacion >= filtro.FechaDesde.Date && v.FechaAplicacion <= fechaFin);

            if (filtro.IdVeterinario.HasValue && filtro.IdVeterinario.Value > 0)
            {
                queryVacunas = queryVacunas.Where(v => v.Consulta.IdUsuario == filtro.IdVeterinario.Value);
            }

            if (filtro.IdEspecie.HasValue && filtro.IdEspecie.Value > 0)
            {
                queryVacunas = queryVacunas.Where(v => v.Consulta.Mascota.Raza.IdEspecie == filtro.IdEspecie.Value);
            }

            var vacunas = await queryVacunas
                .Select(v => new ReporteVacunaTratamientoDto
                {
                    Fecha = v.FechaAplicacion,
                    Mascota = v.Consulta.Mascota.Nombre,
                    Especie = v.Consulta.Mascota.Raza.Especie.Nombre,
                    TratamientoVacuna = v.Vacuna.Nombre,
                    Tipo = "Vacunación",
                    Dosis = v.ProximaDosis.HasValue
                        ? "Próx. refuerzo: " + v.ProximaDosis.Value.ToString("dd/MM/yyyy")
                        : "Dosis completa",
                    Veterinario = v.Consulta.Usuario.Nombre + " " + v.Consulta.Usuario.Apellido
                })
                .ToListAsync();

            // Consolidación en memoria ordenada por fecha descendente
            var resultadoConsolidado = tratamientos
                .Concat(vacunas)
                .OrderByDescending(r => r.Fecha)
                .ToList();

            return Resultado<IEnumerable<ReporteVacunaTratamientoDto>>.Exito(resultadoConsolidado);
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
    /// </summary>
    public async Task<Resultado<IEnumerable<ReporteCensoMascotaDto>>> ObtenerReporteCensoMascotasAsync(FiltroReporteDto filtro)
    {
        try
        {
            var query = context.Mascotas
                .AsNoTracking()
                .Where(m => m.Activo);

            if (filtro.IdEspecie.HasValue && filtro.IdEspecie.Value > 0)
            {
                query = query.Where(m => m.Raza.IdEspecie == filtro.IdEspecie.Value);
            }

            var resultados = await query
                .OrderBy(m => m.Nombre)
                .Select(m => new ReporteCensoMascotaDto
                {
                    Id = m.Id,
                    Nombre = m.Nombre,
                    Especie = m.Raza.Especie.Nombre,
                    Raza = m.Raza.Nombre,
                    Sexo = m.Sexo,
                    FechaNacimiento = m.FechaNacimiento,
                    Color = string.IsNullOrWhiteSpace(m.Color) ? "-" : m.Color,
                    Propietario = m.Propietario.Nombre + " " + m.Propietario.Apellido
                })
                .ToListAsync();

            return Resultado<IEnumerable<ReporteCensoMascotaDto>>.Exito(resultados);
        }
        catch (Exception ex)
        {
            return Resultado<IEnumerable<ReporteCensoMascotaDto>>.Falla($"Error al generar censo de pacientes: {ex.Message}");
        }
    }
}
