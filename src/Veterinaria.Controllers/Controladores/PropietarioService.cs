using Microsoft.EntityFrameworkCore;
using Veterinaria.Domain.Comunes;
using Veterinaria.Domain.Dtos;
using Veterinaria.Infrastructure;

namespace Veterinaria.Controllers.Controladores;

/// <summary>
/// Servicio de dominio para la gestión, validación de unicidad y búsqueda dinámica multi-campo de propietarios.
/// Reutiliza y expone las operaciones de negocio con validaciones estrictas y control de unicidad.
/// </summary>
public class PropietarioService(ContextoVeterinaria context) : PropietarioControlador(context)
{
    /// <summary>
    /// Realiza una búsqueda dinámica multi-campo asíncrona de propietarios utilizando AsNoTracking().
    /// Si el término está vacío, recarga todos los propietarios activos.
    /// Si se proporciona un término, filtra de forma insensible a mayúsculas/minúsculas en DNI, Nombre, Apellido,
    /// Teléfono, Correo Electrónico o Dirección.
    /// </summary>
    /// <param name="termino">Término global de búsqueda.</param>
    /// <param name="soloActivos">Indica si se deben limitar los resultados exclusivamente a propietarios activos.</param>
    /// <returns>Resultado con la colección de propietarios coincidentes.</returns>
    public async Task<Resultado<IEnumerable<PropietarioRespuestaDto>>> BuscarPropietariosAsync(string? termino, bool soloActivos = true)
    {
        try
        {
            // Paso 1: Configurar consulta optimizada de solo lectura sin seguimiento de entidades
            var consulta = Context.Propietarios
                .AsNoTracking()
                .AsQueryable();

            // Paso 2: Aplicar filtro de estado activo si corresponde
            if (soloActivos)
            {
                consulta = consulta.Where(p => p.Activo);
            }

            // Paso 3: Filtrar dinámicamente si se suministró un término de búsqueda
            if (!string.IsNullOrWhiteSpace(termino))
            {
                var term = termino.Trim();
                consulta = consulta.Where(p =>
                    p.DNI.Contains(term) ||
                    p.Nombre.Contains(term) ||
                    p.Apellido.Contains(term) ||
                    (p.Telefono != null && p.Telefono.Contains(term)) ||
                    (p.CorreoElectronico != null && p.CorreoElectronico.Contains(term)) ||
                    (p.Direccion != null && p.Direccion.Contains(term)));
            }

            // Paso 4: Ordenar alfabéticamente y proyectar al DTO de respuesta
            var resultados = await consulta
                .OrderBy(p => p.Apellido)
                .ThenBy(p => p.Nombre)
                .Select(p => new PropietarioRespuestaDto
                {
                    Id = p.Id,
                    DNI = p.DNI,
                    Nombre = p.Nombre,
                    Apellido = p.Apellido,
                    Telefono = p.Telefono,
                    CorreoElectronico = p.CorreoElectronico,
                    Direccion = p.Direccion,
                    Activo = p.Activo,
                    CantidadMascotas = p.Mascotas.Count
                })
                .ToListAsync();

            return Resultado<IEnumerable<PropietarioRespuestaDto>>.Exito(resultados);
        }
        catch (Exception ex)
        {
            return Resultado<IEnumerable<PropietarioRespuestaDto>>.Falla($"Error interno al realizar la búsqueda de propietarios: {ex.Message}");
        }
    }

    /// <summary>
    /// Valida la unicidad en base de datos para DNI, Teléfono y Correo Electrónico frente a registros activos e inactivos.
    /// Utiliza IgnoreQueryFilters para garantizar que no existan colisiones con registros soft-deleted.
    /// </summary>
    /// <param name="idExcluir">Identificador del propietario a excluir en caso de modificación.</param>
    /// <param name="dni">DNI a verificar.</param>
    /// <param name="telefono">Teléfono a verificar.</param>
    /// <param name="correoElectronico">Correo electrónico a verificar.</param>
    /// <returns>Resultado exitoso o con mensaje descriptivo de conflicto de unicidad.</returns>
    public async Task<Resultado> ValidarUnicidadAsync(
        long? idExcluir,
        string dni,
        string? telefono,
        string? correoElectronico)
    {
        // Paso 1: Normalizar entradas de texto
        var dniNormalizado = dni.Trim();
        var telNormalizado = string.IsNullOrWhiteSpace(telefono) ? null : telefono.Trim();
        var correoNormalizado = string.IsNullOrWhiteSpace(correoElectronico) ? null : correoElectronico.Trim();

        // Paso 2: Ejecutar validación contra registros activos e inactivos
        var mensajeConflicto = await ValidarUnicidadContactoAsync(
            idExcluir,
            dniNormalizado,
            telNormalizado,
            correoNormalizado);

        if (mensajeConflicto is not null)
        {
            return Resultado.Falla(mensajeConflicto);
        }

        return Resultado.Exito();
    }
}
