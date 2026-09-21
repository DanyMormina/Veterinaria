using System.Globalization;
using Veterinaria.Controllers.Controladores;
using Veterinaria.Domain.Dtos;
using Veterinaria.WinForms.Sesion;

namespace Veterinaria.WinForms.Vistas.Veterinario;

/// <summary>
/// Formulario de aplicación y gestión de tratamientos veterinarios en consultas clínicas.
/// Adhiere al sistema de diseño "Ejecutivo Romántico Pastel", validaciones preventivas,
/// filtros predictivos en tiempo real y el Result Pattern (.NET 10 / C#).
/// </summary>
public partial class FormTratamientos : Form
{
    private readonly TratamientoControlador? _tratamientoControlador;
    private readonly ConsultaControlador? _consultaControlador;
    private readonly MascotaControlador? _mascotaControlador;

    private static readonly CultureInfo CulturaArgentina = new("es-AR");

    private List<MascotaRespuestaDto> _listaMascotas = [];
    private List<TratamientoRespuestaDto> _listaTratamientos = [];
    private List<ConsultaRespuestaDto> _listaConsultasMascota = [];

    private bool _cargandoMascotas = false;
    private bool _cargandoConsultas = false;
    private bool _cargandoTratamientos = false;

    /// <summary>
    /// Constructor por defecto para compatibilidad con el diseñador de Windows Forms.
    /// </summary>
    public FormTratamientos() : this(null, null, null)
    {
    }

    /// <summary>
    /// Constructor principal con inyección de dependencias de controladores clínicos.
    /// </summary>
    public FormTratamientos(
        TratamientoControlador? tratamientoControlador,
        ConsultaControlador? consultaControlador,
        MascotaControlador? mascotaControlador)
    {
        InitializeComponent();
        _tratamientoControlador = tratamientoControlador;
        _consultaControlador = consultaControlador;
        _mascotaControlador = mascotaControlador;

        ConfigurarRestriccionesTeclado();
    }

    /// <summary>
    /// Inicializa la sesión, limpia la grilla y precarga los catálogos clínicos.
    /// </summary>
    private async void FormTratamientos_Load(object? sender, EventArgs e)
    {
        // 1. Configuración de sesión y estado
        lblUsuarioSesion.Text = SesionActual.EstaAutenticado
            ? $"Dr./Dra. {SesionActual.NombreCompleto} | {SesionActual.Rol}"
            : "Dr./Dra. Veterinario | Atención Clínica";

        lblInfoEstado.Text = $"Sesión clínica activa: {SesionActual.NombreUsuario} — {DateTime.Now:dd/MM/yyyy}";

        // 2. Grilla arranca completamente vacía
        dgvTratamientosAplicados.Rows.Clear();
        lblTotal.Text = "Total: $ 0,00";
        txtSubtotal.Text = "$ 0,00";
        lblInfoMascota.Text = "Especie: — | Raza: — | Propietario: —";

        // 3. Cargar catálogos iniciales
        await CargarCatalogoMascotasAsync();
        await CargarCatalogoTratamientosAsync();
    }

    #region Carga de Datos y Catálogos

    /// <summary>
    /// Carga el padrón de pacientes activos en el desplegable con autocompletado y búsqueda en tiempo real.
    /// Formato: "{NombreMascota} - {NombrePropietario}".
    /// </summary>
    private async Task CargarCatalogoMascotasAsync()
    {
        if (_mascotaControlador is null) return;

        _cargandoMascotas = true;
        try
        {
            var resultado = await _mascotaControlador.ObtenerTodosAsync();
            if (!resultado.EsExitoso || resultado.Valor is null)
            {
                lblInfoEstado.Text = "No se pudieron obtener los pacientes registrados.";
                return;
            }

            _listaMascotas = resultado.Valor
                .Where(m => m.Activo)
                .OrderBy(m => m.Nombre)
                .ToList();

            cboMascotas.DataSource = null;
            cboMascotas.DisplayMember = nameof(MascotaComboItem.Texto);
            cboMascotas.ValueMember = nameof(MascotaComboItem.Id);

            var items = _listaMascotas.Select(m => new MascotaComboItem
            {
                Id = m.Id,
                Texto = $"{m.Nombre} - {m.NombrePropietario}".Trim()
            }).ToList();

            cboMascotas.DataSource = items;
            cboMascotas.SelectedIndex = -1;
            lblInfoMascota.Text = "Especie: — | Raza: — | Propietario: —";
            cboConsultas.DataSource = null;
        }
        finally
        {
            _cargandoMascotas = false;
        }
    }

    /// <summary>
    /// Carga el catálogo de tratamientos activos con autocompletado predictivo.
    /// Formato: "{Nombre/Descripcion} - Precio: {Precio:C2}".
    /// </summary>
    public async Task CargarCatalogoTratamientosAsync()
    {
        if (_tratamientoControlador is null) return;

        _cargandoTratamientos = true;
        var idTratamientoActual = ObtenerIdSeleccionado(cboTratamientos);

        try
        {
            var resultado = await _tratamientoControlador.ObtenerTodosAsync();
            if (!resultado.EsExitoso || resultado.Valor is null)
            {
                lblInfoEstado.Text = "No se pudieron obtener los tratamientos activos.";
                return;
            }

            _listaTratamientos = resultado.Valor
                .Where(t => t.Activo)
                .OrderBy(t => t.Descripcion)
                .ToList();

            cboTratamientos.DataSource = null;
            cboTratamientos.DisplayMember = nameof(TratamientoComboItem.Texto);
            cboTratamientos.ValueMember = nameof(TratamientoComboItem.Id);

            var items = _listaTratamientos.Select(t => new TratamientoComboItem
            {
                Id = t.Id,
                Texto = $"{t.Descripcion} - Precio: {t.Precio.ToString("C2", CulturaArgentina)}",
                Precio = t.Precio
            }).ToList();

            cboTratamientos.DataSource = items;

            // Mantener selección previa si sigue existiendo y activa
            if (idTratamientoActual.HasValue && items.Any(i => i.Id == idTratamientoActual.Value))
            {
                cboTratamientos.SelectedValue = idTratamientoActual.Value;
            }
            else
            {
                cboTratamientos.SelectedIndex = items.Count > 0 ? 0 : -1;
            }

            RecalcularSubtotal();
        }
        finally
        {
            _cargandoTratamientos = false;
        }
    }

    /// <summary>
    /// Carga las consultas de la mascota seleccionada ordenadas de la más reciente a la más antigua.
    /// Formato: "{FechaHora:dd/MM/yyyy HH:mm} - Diagnóstico: {DiagnosticoCorto}".
    /// </summary>
    private async Task CargarConsultasDeMascotaAsync(long idMascota)
    {
        if (_consultaControlador is null) return;

        _cargandoConsultas = true;
        try
        {
            var resultado = await _consultaControlador.ObtenerPorMascotaAsync(idMascota);
            if (!resultado.EsExitoso || resultado.Valor is null)
            {
                cboConsultas.DataSource = null;
                dgvTratamientosAplicados.Rows.Clear();
                lblTotal.Text = "Total: $ 0,00";
                return;
            }

            _listaConsultasMascota = resultado.Valor
                .OrderByDescending(c => c.FechaHora)
                .ToList();

            cboConsultas.DataSource = null;
            cboConsultas.DisplayMember = nameof(ConsultaComboItem.Texto);
            cboConsultas.ValueMember = nameof(ConsultaComboItem.Id);

            var items = _listaConsultasMascota.Select(c =>
            {
                var diag = string.IsNullOrWhiteSpace(c.Diagnostico) ? "(Sin diagnóstico registrado)" : c.Diagnostico.Trim();
                var diagCorto = diag.Length > 40 ? string.Concat(diag.AsSpan(0, 37), "...") : diag;

                return new ConsultaComboItem
                {
                    Id = c.Id,
                    Texto = $"{c.FechaHora:dd/MM/yyyy HH:mm} — Diagnóstico: {diagCorto}"
                };
            }).ToList();

            cboConsultas.DataSource = items;

            if (items.Count > 0)
            {
                cboConsultas.SelectedIndex = 0;
                await CargarTratamientosConsultaAsync(items[0].Id);
            }
            else
            {
                dgvTratamientosAplicados.Rows.Clear();
                lblTotal.Text = "Total: $ 0,00";
                lblInfoEstado.Text = "La mascota seleccionada no posee consultas clínicas activas registradas.";
            }
        }
        finally
        {
            _cargandoConsultas = false;
        }
    }

    /// <summary>
    /// Consulta y puebla asíncronamente los tratamientos aplicados a la consulta clínica activa.
    /// Recalcula el total acumulado de la consulta.
    /// </summary>
    private async Task CargarTratamientosConsultaAsync(long idConsulta)
    {
        if (_tratamientoControlador is null) return;

        try
        {
            // 1. Obtener tratamientos aplicados a la consulta específica
            var resultado = await _tratamientoControlador.ObtenerTratamientosAplicadosPorConsultaAsync(idConsulta);
            dgvTratamientosAplicados.Rows.Clear();

            if (!resultado.EsExitoso || resultado.Valor is null)
            {
                lblTotal.Text = "Total: $ 0,00";
                return;
            }

            var detalles = resultado.Valor.ToList();
            decimal totalAcumulado = 0;

            // 2. Poblar filas en la grilla
            foreach (var d in detalles)
            {
                totalAcumulado += d.Subtotal;
                dgvTratamientosAplicados.Rows.Add(
                    d.DescripcionTratamiento,
                    d.TipoTratamiento,
                    d.Cantidad,
                    d.PrecioUnitario.ToString("C2", CulturaArgentina),
                    d.Subtotal.ToString("C2", CulturaArgentina),
                    d.Indicaciones ?? string.Empty);
            }

            // 3. Actualizar etiqueta de total ejecutivo
            lblTotal.Text = $"Total: {totalAcumulado.ToString("C2", CulturaArgentina)}";
            lblInfoEstado.Text = $"Consulta cargada: {detalles.Count} tratamiento(s) aplicado(s). Total: {totalAcumulado.ToString("C2", CulturaArgentina)}";
        }
        catch (Exception ex)
        {
            lblInfoEstado.Text = $"Error al cargar tratamientos aplicados: {ex.Message}";
        }
    }

    #endregion

    #region Eventos Interactivos de Controles

    /// <summary>
    /// Actualiza la ficha resumen de la mascota y recarga sus consultas clínicas en cascada.
    /// </summary>
    private async void cboMascotas_SelectedIndexChanged(object? sender, EventArgs e)
    {
        if (_cargandoMascotas) return;

        var idMascota = ObtenerIdSeleccionado(cboMascotas);
        if (!idMascota.HasValue || idMascota.Value <= 0)
        {
            lblInfoMascota.Text = "Especie: — | Raza: — | Propietario: —";
            cboConsultas.DataSource = null;
            dgvTratamientosAplicados.Rows.Clear();
            lblTotal.Text = "Total: $ 0,00";
            return;
        }

        var mascota = _listaMascotas.FirstOrDefault(m => m.Id == idMascota.Value);
        if (mascota is not null)
        {
            lblInfoMascota.Text = $"Especie: {mascota.NombreEspecie} | Raza: {mascota.NombreRaza} | Propietario: {mascota.NombrePropietario}";
        }
        else
        {
            lblInfoMascota.Text = "Especie: — | Raza: — | Propietario: —";
        }

        await CargarConsultasDeMascotaAsync(idMascota.Value);
    }

    /// <summary>
    /// Al cambiar la consulta seleccionada, actualiza la grilla con los tratamientos de esa consulta.
    /// </summary>
    private async void cboConsultas_SelectedIndexChanged(object? sender, EventArgs e)
    {
        if (_cargandoConsultas) return;

        var idConsulta = ObtenerIdSeleccionado(cboConsultas);
        if (idConsulta.HasValue && idConsulta.Value > 0)
        {
            await CargarTratamientosConsultaAsync(idConsulta.Value);
        }
        else
        {
            dgvTratamientosAplicados.Rows.Clear();
            lblTotal.Text = "Total: $ 0,00";
        }
    }

    /// <summary>
    /// Al cambiar el tratamiento seleccionado, recalcula dinámicamente el Subtotal en tiempo real.
    /// </summary>
    private void cboTratamientos_SelectedIndexChanged(object? sender, EventArgs e)
    {
        if (_cargandoTratamientos) return;
        RecalcularSubtotal();
    }

    /// <summary>
    /// Al variar la cantidad, recalcula el Subtotal en tiempo real.
    /// </summary>
    private void numCantidad_ValueChanged(object? sender, EventArgs e)
    {
        RecalcularSubtotal();
    }

    /// <summary>
    /// Calcula de forma reactiva Subtotal = Cantidad * PrecioUnitario.
    /// </summary>
    private void RecalcularSubtotal()
    {
        var cantidad = (int)numCantidad.Value;
        var idTratamiento = ObtenerIdSeleccionado(cboTratamientos);

        if (idTratamiento.HasValue && idTratamiento.Value > 0)
        {
            var item = _listaTratamientos.FirstOrDefault(t => t.Id == idTratamiento.Value);
            var precio = item?.Precio ?? 0;
            var subtotal = cantidad * precio;
            txtSubtotal.Text = subtotal.ToString("C2", CulturaArgentina);
        }
        else
        {
            txtSubtotal.Text = "$ 0,00";
        }
    }

    #endregion

    #region Apertura de Formularios Modales Auxiliares

    /// <summary>
    /// Abre el diálogo modal de Alta de Tratamiento (#C88A96) y refresca el catálogo al confirmarse.
    /// </summary>
    private async void btnAltaTratamiento_Click(object? sender, EventArgs e)
    {
        using var modalAlta = new FormAltaTratamiento(_tratamientoControlador);
        var resultadoModal = modalAlta.ShowDialog(this);

        if (resultadoModal == DialogResult.OK)
        {
            await CargarCatalogoTratamientosAsync();
            lblInfoEstado.Text = "Catálogo de tratamientos actualizado exitosamente.";
        }
    }

    /// <summary>
    /// Abre el diálogo modal de Modificación/Baja (#E2D9DC) y refresca el catálogo al confirmarse.
    /// </summary>
    private async void btnModificarTratamiento_Click(object? sender, EventArgs e)
    {
        var idTratamiento = ObtenerIdSeleccionado(cboTratamientos);
        using var modalModificar = new FormModificarTratamiento(_tratamientoControlador, idTratamiento);
        var resultadoModal = modalModificar.ShowDialog(this);

        if (resultadoModal == DialogResult.OK)
        {
            await CargarCatalogoTratamientosAsync();

            // Refrescar grilla si hay una consulta activa seleccionada
            var idConsulta = ObtenerIdSeleccionado(cboConsultas);
            if (idConsulta.HasValue && idConsulta.Value > 0)
            {
                await CargarTratamientosConsultaAsync(idConsulta.Value);
            }

            lblInfoEstado.Text = "Catálogo y tratamientos aplicados actualizados exitosamente.";
        }
    }

    #endregion

    #region Acción Principal: Aplicar Tratamiento

    /// <summary>
    /// Registra la aplicación del tratamiento a la consulta clínica activa previa validación.
    /// </summary>
    private async void btnAplicar_Click(object? sender, EventArgs e)
    {
        // 1. Validaciones preventivas de selección
        var idConsulta = ObtenerIdSeleccionado(cboConsultas);
        if (!idConsulta.HasValue || idConsulta.Value <= 0)
        {
            MessageBox.Show(
                "Debe seleccionar una consulta clínica válida para aplicar el tratamiento.",
                "Consulta Requerida",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning);
            cboConsultas.Focus();
            return;
        }

        var idTratamiento = ObtenerIdSeleccionado(cboTratamientos);
        if (!idTratamiento.HasValue || idTratamiento.Value <= 0)
        {
            MessageBox.Show(
                "Debe seleccionar un tratamiento activo del catálogo.",
                "Tratamiento Requerido",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning);
            cboTratamientos.Focus();
            return;
        }

        var cantidad = (int)numCantidad.Value;
        if (cantidad <= 0)
        {
            MessageBox.Show(
                "La cantidad del tratamiento debe ser mayor o igual a 1.",
                "Cantidad Inválida",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning);
            numCantidad.Focus();
            return;
        }

        if (_tratamientoControlador is null)
        {
            MessageBox.Show(
                "No se pudo acceder al controlador de tratamientos clínicos.",
                "Error del Sistema",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
            return;
        }

        // Obtener datos de precio unitario del catálogo
        var tratamiento = _listaTratamientos.FirstOrDefault(t => t.Id == idTratamiento.Value);
        var precioUnitario = tratamiento?.Precio ?? 0;
        var subtotal = cantidad * precioUnitario;
        var indicaciones = string.IsNullOrWhiteSpace(txtIndicaciones.Text) ? null : txtIndicaciones.Text.Trim();

        // 2. Construir DTO de aplicación
        var solicitud = new DetalleConsultaSolicitudDto
        {
            IdConsulta = idConsulta.Value,
            IdTratamiento = idTratamiento.Value,
            Cantidad = cantidad,
            PrecioUnitario = precioUnitario,
            Subtotal = subtotal,
            Indicaciones = indicaciones
        };

        btnAplicar.Enabled = false;

        try
        {
            // 3. Crear registro de tratamiento mediante TratamientoController.CrearAsync
            var resultado = await _tratamientoControlador.CrearAsync(solicitud);

            if (!resultado.EsExitoso)
            {
                MessageBox.Show(
                    resultado.Mensaje,
                    "Error al Aplicar Tratamiento",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                btnAplicar.Enabled = true;
                return;
            }

            // 4. Recargar la grilla con los detalles actualizados de la consulta activa y totalizar
            await CargarTratamientosConsultaAsync(idConsulta.Value);

            // 5. Restablecer campos de detalle aplicado
            numCantidad.Value = 1;
            txtIndicaciones.Clear();
            RecalcularSubtotal();

            lblInfoEstado.Text = $"Tratamiento aplicado con éxito a la consulta clínica. Id: {resultado.Valor}";
        }
        catch (Exception ex)
        {
            MessageBox.Show(
                $"Ocurrió un error inesperado al aplicar el tratamiento: {ex.Message}",
                "Error Inesperado",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
        }
        finally
        {
            btnAplicar.Enabled = true;
        }
    }

    /// <summary>
    /// Botón Limpiar: Restablece completamente el estado de la vista, limpiando selecciones
    /// de paciente, consultas, tratamientos, detalles y la grilla de tratamientos aplicados.
    /// </summary>
    private void btnLimpiar_Click(object? sender, EventArgs e)
    {
        RestablecerFormulario();
    }

    /// <summary>
    /// Restablece todos los controles y estados del formulario a sus valores iniciales predeterminados.
    /// </summary>
    public void RestablecerFormulario()
    {
        // 1. Limpiar selección y ficha resumen de paciente
        _cargandoMascotas = true;
        cboMascotas.SelectedIndex = -1;
        cboMascotas.Text = string.Empty;
        _cargandoMascotas = false;
        lblInfoMascota.Text = "Especie: — | Raza: — | Propietario: —";

        // 2. Limpiar consultas asociadas
        _cargandoConsultas = true;
        cboConsultas.DataSource = null;
        _listaConsultasMascota.Clear();
        _cargandoConsultas = false;

        // 3. Restablecer tratamiento activo del catálogo
        _cargandoTratamientos = true;
        cboTratamientos.SelectedIndex = -1;
        cboTratamientos.Text = string.Empty;
        _cargandoTratamientos = false;

        // 4. Restablecer campos de detalle aplicado
        numCantidad.Value = 1;
        txtIndicaciones.Clear();
        txtSubtotal.Text = "$ 0,00";

        // 5. Vaciar grilla de tratamientos aplicados y totalizador acumulado
        dgvTratamientosAplicados.Rows.Clear();
        lblTotal.Text = "Total: $ 0,00";

        // 6. Mensaje de estado y enfoque inicial en el buscador de mascotas
        lblInfoEstado.Text = "Formulario restablecido. Seleccione un paciente para comenzar.";
        cboMascotas.Focus();
    }

    #endregion

    #region Navegación y Helpers

    /// <summary>
    /// Configura restricciones de tipeo en tiempo real sobre los controles del formulario.
    /// </summary>
    private void ConfigurarRestriccionesTeclado()
    {
        cboMascotas.KeyPress += ValidarSoloLetras_KeyPress;
    }

    /// <summary>
    /// Restringe la búsqueda en el selector de paciente exclusivamente a letras y espacios, bloqueando números y caracteres especiales.
    /// </summary>
    private void ValidarSoloLetras_KeyPress(object? sender, KeyPressEventArgs e)
    {
        // 1. Permitir teclas de control del sistema (retroceso, cortar/copiar, etc.)
        if (char.IsControl(e.KeyChar))
            return;

        // 2. Permitir exclusivamente caracteres alfabéticos (incluye vocales acentuadas áéíóú, ñ y diéresis)
        if (char.IsLetter(e.KeyChar))
            return;

        // 3. Permitir espacio simple si no es al inicio y no es consecutivo
        if (e.KeyChar == ' ' && sender is ComboBox cb)
        {
            if (cb.SelectionStart > 0 && cb.Text.Length > 0 && cb.Text[cb.SelectionStart - 1] != ' ')
                return;
        }

        // 4. Bloquear cualquier número (0-9), símbolo o carácter especial no alfabético
        e.Handled = true;
    }

    private void btnVolver_Click(object? sender, EventArgs e)
    {
        Close();
    }

    private static long? ObtenerIdSeleccionado(ComboBox combo)
    {
        if (combo.SelectedValue is long id)
            return id;

        if (combo.SelectedValue is int idInt)
            return idInt;

        if (combo.SelectedItem is MascotaComboItem mascotaItem)
            return mascotaItem.Id;

        if (combo.SelectedItem is TratamientoComboItem tratamientoItem)
            return tratamientoItem.Id;

        if (combo.SelectedItem is ConsultaComboItem consultaItem)
            return consultaItem.Id;

        return null;
    }

    private sealed class MascotaComboItem
    {
        public long Id { get; init; }
        public string Texto { get; init; } = string.Empty;
    }

    private sealed class TratamientoComboItem
    {
        public long Id { get; init; }
        public string Texto { get; init; } = string.Empty;
        public decimal Precio { get; init; }
    }

    private sealed class ConsultaComboItem
    {
        public long Id { get; init; }
        public string Texto { get; init; } = string.Empty;
    }

    #endregion
}
