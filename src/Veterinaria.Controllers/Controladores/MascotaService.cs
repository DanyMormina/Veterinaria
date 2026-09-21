using Microsoft.EntityFrameworkCore;
using Veterinaria.Domain.Comunes;
using Veterinaria.Domain.Dtos;
using Veterinaria.Infrastructure;

namespace Veterinaria.Controllers.Controladores;

/// <summary>
/// Servicio de dominio para la gestión, filtrado avanzado y búsqueda de mascotas.
/// Construye consultas dinámicas optimizadas con AsNoTracking para la interfaz de usuario.
/// </summary>
public class MascotaService(ContextoVeterinaria context) : MascotaControlador(context)
{
    /// <summary>
    /// Realiza una búsqueda dinámica combinando los criterios opcionales proporcionados por el usuario.
    /// Ejecuta el filtrado directamente sobre las entidades de la base de datos para garantizar compatibilidad con SQL Server.
    /// </summary>
    public async Task<Resultado<IEnumerable<MascotaRespuestaDto>>> BuscarMascotasAsync(
        string? nombre,
        string? nombrePropietario,
        long? idEspecie,
        long? idRaza,
        string? sexo,
        string? color,
        string? textoGeneral = null,
        bool? activo = null)
    {
        try
        {
            // Paso 1: Iniciar la consulta sobre la entidad base ignorando filtros de borrado lógico
            var consulta = Context.Mascotas
                .IgnoreQueryFilters()
                .AsNoTracking()
                .AsQueryable();

            // Paso 2: Filtrado por nombre de mascota si fue provisto
            if (!string.IsNullOrWhiteSpace(nombre))
            {
                var n = nombre.Trim();
                consulta = consulta.Where(m => m.Nombre.Contains(n));
            }

            // Paso 3: Filtrado por nombre y apellido del propietario (compatible con traducción a LIKE en SQL)
            if (!string.IsNullOrWhiteSpace(nombrePropietario))
            {
                var p = nombrePropietario.Trim();
                consulta = consulta.Where(m =>
                    m.Propietario.Nombre.Contains(p) ||
                    m.Propietario.Apellido.Contains(p) ||
                    (m.Propietario.Nombre + " " + m.Propietario.Apellido).Contains(p));
            }

            // Paso 4: Filtrado por especie (a través de la raza del animal)
            if (idEspecie.HasValue && idEspecie.Value > 0)
            {
                consulta = consulta.Where(m => m.Raza.IdEspecie == idEspecie.Value);
            }

            // Paso 5: Filtrado por raza específica
            if (idRaza.HasValue && idRaza.Value > 0)
            {
                consulta = consulta.Where(m => m.IdRaza == idRaza.Value);
            }

            // Paso 6: Filtrado por sexo biológico si no es valor comodín
            if (!string.IsNullOrWhiteSpace(sexo) && !sexo.Equals("(Todos)", StringComparison.OrdinalIgnoreCase))
            {
                var s = sexo.Trim();
                consulta = consulta.Where(m => m.Sexo == s);
            }

            // Paso 7: Filtrado por color o pelaje
            if (!string.IsNullOrWhiteSpace(color))
            {
                var c = color.Trim();
                consulta = consulta.Where(m => m.Color != null && m.Color.Contains(c));
            }

            // Paso 8: Filtrado global de texto (cuadro de búsqueda rápida)
            if (!string.IsNullOrWhiteSpace(textoGeneral))
            {
                var tg = textoGeneral.Trim();
                consulta = consulta.Where(m =>
                    m.Nombre.Contains(tg) ||
                    m.Propietario.Nombre.Contains(tg) ||
                    m.Propietario.Apellido.Contains(tg) ||
                    (m.Propietario.Nombre + " " + m.Propietario.Apellido).Contains(tg) ||
                    m.Raza.Especie.Nombre.Contains(tg) ||
                    m.Raza.Nombre.Contains(tg) ||
                    (m.Color != null && m.Color.Contains(tg)));
            }

            // Paso 9: Filtrado por estado si fue provisto
            if (activo.HasValue)
            {
                consulta = consulta.Where(m => m.Activo == activo.Value);
            }

            // Paso 10: Ordenamiento y proyección al DTO de respuesta
            var resultados = await consulta
                .OrderBy(m => m.Nombre)
                .Select(m => new MascotaRespuestaDto
                {
                    Id = m.Id,
                    IdPropietario = m.IdPropietario,
                    NombrePropietario = m.Propietario.Nombre + " " + m.Propietario.Apellido,
                    IdRaza = m.IdRaza,
                    NombreRaza = m.Raza.Nombre,
                    IdEspecie = m.Raza.IdEspecie,
                    NombreEspecie = m.Raza.Especie.Nombre,
                    Nombre = m.Nombre,
                    Sexo = m.Sexo,
                    FechaNacimiento = m.FechaNacimiento,
                    Color = m.Color,
                    Activo = m.Activo,
                    CantidadConsultas = m.Consultas.Count
                })
                .ToListAsync();

            return Resultado<IEnumerable<MascotaRespuestaDto>>.Exito(resultados);
        }
        catch (Exception ex)
        {
            return Resultado<IEnumerable<MascotaRespuestaDto>>.Falla($"Error interno al buscar mascotas: {ex.Message}");
        }
    }
}
