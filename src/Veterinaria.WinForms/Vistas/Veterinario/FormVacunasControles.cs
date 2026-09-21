using System.Globalization;
using Veterinaria.Controllers.Controladores;
using Veterinaria.Domain.Comunes;
using Veterinaria.Domain.Dtos;
using Veterinaria.WinForms.Sesion;

namespace Veterinaria.WinForms.Vistas.Veterinario;

/// <summary>
/// Formulario de atención clínica para el registro y control de vacunas en pacientes veterinarios.
/// Adhiere al sistema de diseño "Ejecutivo Romántico Pastel", Result Pattern (.NET 10 / C#),
/// autocompletado en tiempo real, cascadas dependientes y captura de precios históricos.
/// </summary>
public partial class FormVacunasControles : Form
{
    private readonly MascotaControlador? _mascotaControlador;
    private readonly ConsultaControlador? _consultaControlador;
    private readonly VacunaControlador? _vacunaControlador;
    private readonly AplicacionVacunaControlador? _aplicacionVacunaControlador;

    private static readonly CultureInfo CulturaArgentina = new("es-AR");

    private List<MascotaRespuestaDto> _listaMascotas = [];
    private List<VacunaRespuestaDto> _todasLasVacunas = [];
    private List<ConsultaRespuestaDto> _listaConsultas = [];
    private List<AplicacionVacunaRespuestaDto> _listaAplicaciones = [];

    private bool _cargandoMascotas = false;
    private bool _cargandoConsultas = false;
    private bool _cargandoVacunas = false;
    private bool _cargandoGrilla = false;
    private bool _cargandoSeleccion = false;
    private long? _idAplicacionSeleccionada = null;

    /// <summary>
    /// Constructor por defecto requerido para el soporte del diseñador de Windows Forms.
    /// </summary>
    public FormVacunasControles() : this(null, null, null, null)
    {
    }

    /// <summary>
    /// Constructor principal con inyección de dependencias de controladores clínicos.
    /// </summary>
    public FormVacunasControles(
        MascotaControlador? mascotaControlador,
        ConsultaControlador? consultaControlador,
        VacunaControlador? vacunaControlador,
        AplicacionVacunaControlador? aplicacionVacunaControlador)
    {
        InitializeComponent();
        _mascotaControlador = mascotaControlador;
        _consultaControlador = consultaControlador;
        _vacunaControlador = vacunaControlador;
        _aplicacionVacunaControlador = aplicacionVacunaControlador;

        ConfigurarRestriccionesTeclado();
        dgvVacunas.CellClick += (_, _) => dgvVacunas_SelectionChanged(null, EventArgs.Empty);
    }

    /// <summary>
    /// Inicializa la sesión del veterinario actuante, estado de controles y precarga de catálogos.
    /// </summary>
    private async void FormVacunasControles_Load(object? sender, EventArgs e)
    {
        // 1. Configuración de encabezado y sesión clínica
        lblUsuarioSesion.Text = SesionActual.EstaAutenticado
            ? $"Dr./Dra. {SesionActual.NombreCompleto} | {SesionActual.Rol}"
            : "Dr./Dra. Lucía Pérez | Veterinario";

        lblInfoEstado.Text = $"Sesión clínica activa: {SesionActual.NombreUsuario} — {DateTime.Now:dd/MM/yyyy}";

        // 2. Establecer fechas predeterminadas
        dtpFechaAplicacion.Value = DateTime.Today;
        dtpProximaDosis.Value = DateTime.Today.AddMonths(12);

        // 3. Inicializar grilla y estado de botones
        dgvVacunas.Rows.Clear();
        btnGuardar.Enabled = true;
        btnModificar.Enabled = false;

        // 4. Precarga asíncrona de catálogos base
        await CargarCatalogoMascotasAsync();
        await CargarCatalogoVacunasAsync();
        await CargarAplicacionesGrillaAsync();
    }

    #region Carga de Datos y Catálogos

    /// <summary>
    /// Carga el padrón de mascotas activas en el selector con búsqueda y autocompletado.
    /// Formato: "{NombreMascota} - {NombrePropietario} {ApellidoPropietario}".
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
            cboMascotas.DisplayMember = nameof(ComboItemGenerico.Texto);
            cboMascotas.ValueMember = nameof(ComboItemGenerico.Id);

            var items = _listaMascotas.Select(m => new ComboItemGenerico
            {
                Id = m.Id,
                Texto = $"{m.Nombre} - {m.NombrePropietario}".Trim()
            }).ToList();

            cboMascotas.DataSource = items;
            cboMascotas.SelectedIndex = -1;
            cboConsultas.DataSource = null;
        }
        finally
        {
            _cargandoMascotas = false;
        }
    }

    /// <summary>
    /// Carga el catálogo maestro de vacunas activas y las almacena en memoria local.
    /// </summary>
    public async Task CargarCatalogoVacunasAsync()
    {
        if (_vacunaControlador is null) return;

        _cargandoVacunas = true;
        var idVacunaActual = ObtenerIdSeleccionado(cboVacunas);

        try
        {
            var resultado = await _vacunaControlador.ObtenerTodosAsync();
            if (!resultado.EsExitoso || resultado.Valor is null)
            {
                lblInfoEstado.Text = "No se pudieron obtener las vacunas del catálogo.";
                return;
            }

            _todasLasVacunas = resultado.Valor
                .Where(v => v.Activo)
                .OrderBy(v => v.Nombre)
                .ToList();

            // Refrescar selector respetando filtro de especie si hay mascota seleccionada
            PoblarComboVacunas(idVacunaActual);
        }
        finally
        {
            _cargandoVacunas = false;
        }
    }

    /// <summary>
    /// Puebla el desplegable de vacunas activas, filtrando opcionalmente por la especie de la mascota seleccionada.
    /// </summary>
    private void PoblarComboVacunas(long? idVacunaASeleccionar = null)
    {
        var idMascota = ObtenerIdSeleccionado(cboMascotas);
        MascotaRespuestaDto? mascota = null;

        if (idMascota.HasValue && idMascota.Value > 0)
        {
            mascota = _listaMascotas.FirstOrDefault(m => m.Id == idMascota.Value);
        }

        // Filtrar vacunas para la especie de la mascota seleccionada
        var vacunasFiltradas = mascota is not null && mascota.IdEspecie > 0
            ? _todasLasVacunas.Where(v => v.IdEspecie == mascota.IdEspecie || v.IdEspecie == 0).ToList()
            : _todasLasVacunas;

        cboVacunas.DataSource = null;
        cboVacunas.DisplayMember = nameof(ComboItemGenerico.Texto);
        cboVacunas.ValueMember = nameof(ComboItemGenerico.Id);

        var items = vacunasFiltradas.Select(v => new ComboItemGenerico
        {
            Id = v.Id,
            Texto = string.IsNullOrWhiteSpace(v.NombreEspecie)
                ? $"{v.Nombre} — Precio: {v.Precio.ToString("C2", CulturaArgentina)}"
                : $"{v.Nombre} ({v.NombreEspecie}) — Precio: {v.Precio.ToString("C2", CulturaArgentina)}"
        }).ToList();

        cboVacunas.DataSource = items;

        if (idVacunaASeleccionar.HasValue && items.Any(i => i.Id == idVacunaASeleccionar.Value))
        {
            cboVacunas.SelectedValue = idVacunaASeleccionar.Value;
        }
        else
        {
            cboVacunas.SelectedIndex = items.Count > 0 ? 0 : -1;
        }

        ActualizarSugerenciaFechasVacuna();
    }

    /// <summary>
    /// Carga las consultas clínicas activas asociadas a la mascota seleccionada,
    /// ordenadas cronológicamente de la más reciente a la más antigua.
    /// </summary>
    private async Task CargarConsultasDeMascotaAsync(long idMascota)
    {
        if (_consultaControlador is null) return;

        _cargandoConsultas = true;
        try
        {
            // Consulta clínica ordenada por FechaHora desc
            var resultado = await _consultaControlador.ObtenerPorMascotaAsync(idMascota);
            if (!resultado.EsExitoso || resultado.Valor is null)
            {
                cboConsultas.DataSource = null;
                return;
            }

            _listaConsultas = resultado.Valor
                .OrderByDescending(c => c.FechaHora)
                .ToList();

            cboConsultas.DataSource = null;
            cboConsultas.DisplayMember = nameof(ComboItemGenerico.Texto);
            cboConsultas.ValueMember = nameof(ComboItemGenerico.Id);

            var items = _listaConsultas.Select(c =>
            {
                var diag = string.IsNullOrWhiteSpace(c.Diagnostico) ? "(Sin diagnóstico registrado)" : c.Diagnostico.Trim();
                var diagCorto = diag.Length > 40 ? string.Concat(diag.AsSpan(0, 37), "...") : diag;

                return new ComboItemGenerico
                {
                    Id = c.Id,
                    Texto = $"{c.FechaHora:dd/MM/yyyy HH:mm} — Diagnóstico: {diagCorto}"
                };
            }).ToList();

            cboConsultas.DataSource = items;

            if (items.Count > 0)
            {
                cboConsultas.SelectedIndex = 0;
            }
            else
            {
                lblInfoEstado.Text = "La mascota no posee consultas clínicas activas registradas.";
            }
        }
        finally
        {
            _cargandoConsultas = false;
        }
    }

    /// <summary>
    /// Consulta y puebla asíncronamente en la grilla las aplicaciones de vacunas según el contexto activo.
    /// Retiene los estilos y columnas preexistentes sin congelamiento visual.
    /// </summary>
    private async Task CargarAplicacionesGrillaAsync()
    {
        if (_aplicacionVacunaControlador is null) return;

        _cargandoGrilla = true;
        try
        {
            var idConsulta = ObtenerIdSeleccionado(cboConsultas);
            var idMascota = ObtenerIdSeleccionado(cboMascotas);

            Resultado<IEnumerable<AplicacionVacunaRespuestaDto>> resultado;

            // 1. Si hay una consulta seleccionada, obtener aplicaciones de esa consulta
            if (idConsulta.HasValue && idConsulta.Value > 0)
            {
                resultado = await _aplicacionVacunaControlador.ObtenerPorConsultaAsync(idConsulta.Value);
            }
            // 2. Si no hay consulta pero sí mascota, obtener aplicaciones históricas de esa mascota
            else if (idMascota.HasValue && idMascota.Value > 0)
            {
                resultado = await _aplicacionVacunaControlador.ObtenerPorMascotaAsync(idMascota.Value);
            }
            // 3. De lo contrario, cargar las aplicaciones generales registradas
            else
            {
                resultado = await _aplicacionVacunaControlador.ObtenerTodosAsync();
            }

            dgvVacunas.Rows.Clear();

            if (!resultado.EsExitoso || resultado.Valor is null)
            {
                return;
            }

            _listaAplicaciones = resultado.Valor.ToList();

            // 4. Poblar filas en la grilla conservando la estructura de columnas existente
            foreach (var a in _listaAplicaciones)
            {
                var nombreMascota = !string.IsNullOrWhiteSpace(a.NombreMascota)
                    ? a.NombreMascota
                    : (cboMascotas.SelectedItem as ComboItemGenerico)?.Texto.Split('-')[0].Trim() ?? "—";

                var rowIndex = dgvVacunas.Rows.Add(
                    nombreMascota,
                    a.NombreVacuna,
                    a.FechaAplicacion.ToString("dd/MM/yyyy"),
                    a.ProximaDosis?.ToString("dd/MM/yyyy") ?? "—",
                    a.Observaciones ?? string.Empty);

                dgvVacunas.Rows[rowIndex].Tag = a.Id;
            }

            lblInfoEstado.Text = $"Se cargaron {_listaAplicaciones.Count} registro(s) de vacunación.";
        }
        catch (Exception ex)
        {
            lblInfoEstado.Text = $"Error al sincronizar grilla de vacunas: {ex.Message}";
        }
        finally
        {
            _cargandoGrilla = false;
        }
    }

    #endregion

    #region Eventos Interactivos de Controles

    /// <summary>
    /// Al cambiar la mascota seleccionada:
    /// 1. Filtra las consultas en cascada ordenadas cronológicamente de la más reciente a la más antigua.
    /// 2. Filtra el catálogo de vacunas según la especie del paciente.
    /// 3. Actualiza la grilla de aplicaciones históricas.
    /// </summary>
    private async void cboMascota_SelectedIndexChanged(object? sender, EventArgs e)
    {
        if (_cargandoMascotas || _cargandoSeleccion) return;

        var idMascota = ObtenerIdSeleccionado(cboMascotas);
        if (!idMascota.HasValue || idMascota.Value <= 0)
        {
            cboConsultas.DataSource = null;
            PoblarComboVacunas();
            await CargarAplicacionesGrillaAsync();
            dgvVacunas.ClearSelection();
            return;
        }

        // 1. Filtrar vacunas correspondientes a la especie de la mascota
        PoblarComboVacunas();

        // 2. Poblar consultas clínicas en cascada (OrderByDescending FechaHora)
        await CargarConsultasDeMascotaAsync(idMascota.Value);

        // 3. Sincronizar grilla con aplicaciones de la mascota
        await CargarAplicacionesGrillaAsync();
    }

    /// <summary>
    /// Al cambiar la consulta clínica seleccionada, sincroniza las vacunas aplicadas a dicha consulta.
    /// </summary>
    private async void cboConsulta_SelectedIndexChanged(object? sender, EventArgs e)
    {
        if (_cargandoConsultas || _cargandoSeleccion) return;
        await CargarAplicacionesGrillaAsync();
    }

    /// <summary>
    /// Al seleccionar una vacuna, actualiza automáticamente la fecha sugerida de la próxima dosis.
    /// </summary>
    private void cboVacuna_SelectedIndexChanged(object? sender, EventArgs e)
    {
        if (_cargandoVacunas || _cargandoSeleccion) return;
        ActualizarSugerenciaFechasVacuna();
    }

    /// <summary>
    /// Al modificar la fecha de aplicación, recalcula las fechas de próxima dosis y próximo control.
    /// </summary>
    private void dtpFechaAplicacion_ValueChanged(object? sender, EventArgs e)
    {
        if (_cargandoSeleccion) return;
        ActualizarSugerenciaFechasVacuna();
    }

    /// <summary>
    /// Calcula y sugiere en los DateTimePicker de control la próxima fecha sumando el período recomendado.
    /// </summary>
    private void ActualizarSugerenciaFechasVacuna()
    {
        var idVacuna = ObtenerIdSeleccionado(cboVacunas);
        if (!idVacuna.HasValue || idVacuna.Value <= 0) return;

        var vacuna = _todasLasVacunas.FirstOrDefault(v => v.Id == idVacuna.Value);
        if (vacuna is null || vacuna.PeriodoMesesRecomendado <= 0) return;

        var fechaCalculada = dtpFechaAplicacion.Value.AddMonths(vacuna.PeriodoMesesRecomendado);
        dtpProximaDosis.Value = fechaCalculada;
    }

    /// <summary>
    /// Al hacer clic o cambiar fila en la grilla, carga la aplicación en los campos para eventual edición.
    /// </summary>
    private async void dgvVacunas_SelectionChanged(object? sender, EventArgs e)
    {
        if (_cargandoGrilla || _cargandoSeleccion) return;

        if (dgvVacunas.SelectedRows.Count == 0)
        {
            btnModificar.Enabled = false;
            return;
        }

        var fila = dgvVacunas.SelectedRows[0];
        if (fila.Tag is long idAplicacion)
        {
            _idAplicacionSeleccionada = idAplicacion;
            var app = _listaAplicaciones.FirstOrDefault(a => a.Id == idAplicacion);
            if (app is not null)
            {
                _cargandoSeleccion = true;
                try
                {
                    // 1. Cargar y seleccionar la mascota en los selectores
                    if (app.IdMascota > 0)
                    {
                        var itemMascota = cboMascotas.Items.Cast<ComboItemGenerico>()
                            .FirstOrDefault(i => i.Id == app.IdMascota);
                        if (itemMascota is not null)
                        {
                            cboMascotas.SelectedItem = itemMascota;
                        }
                        else
                        {
                            cboMascotas.SelectedValue = app.IdMascota;
                        }

                        // 2. Cargar consultas de la mascota y seleccionar la correspondiente
                        await CargarConsultasDeMascotaAsync(app.IdMascota);
                        if (app.IdConsulta > 0)
                        {
                            var itemConsulta = cboConsultas.Items.Cast<ComboItemGenerico>()
                                .FirstOrDefault(i => i.Id == app.IdConsulta);
                            if (itemConsulta is not null)
                            {
                                cboConsultas.SelectedItem = itemConsulta;
                            }
                            else
                            {
                                cboConsultas.SelectedValue = app.IdConsulta;
                            }
                        }
                    }

                    // 3. Poblar catálogo de vacunas filtrado por la especie del paciente y seleccionar la vacuna
                    PoblarComboVacunas(app.IdVacuna);
                    var itemVacuna = cboVacunas.Items.Cast<ComboItemGenerico>()
                        .FirstOrDefault(i => i.Id == app.IdVacuna);
                    if (itemVacuna is not null)
                    {
                        cboVacunas.SelectedItem = itemVacuna;
                    }
                    else
                    {
                        cboVacunas.SelectedValue = app.IdVacuna;
                    }

                    // 4. Fechas, casilla de vacuna externa y observaciones
                    dtpFechaAplicacion.Value = app.FechaAplicacion;
                    if (app.ProximaDosis.HasValue)
                    {
                        dtpProximaDosis.Value = app.ProximaDosis.Value;
                    }

                    chkVacunaPrevia.Checked = !app.PrecioUnitario.HasValue;
                    txtObservaciones.Text = app.Observaciones ?? string.Empty;

                    btnModificar.Enabled = true;
                    btnGuardar.Enabled = false;
                    lblInfoEstado.Text = $"Aplicación #{app.Id} seleccionada ({app.NombreVacuna} - {app.NombreMascota}). Modifique los datos y presione 'Modificar' o 'Limpiar' para cancelar.";
                }
                finally
                {
                    _cargandoSeleccion = false;
                }
            }
        }
    }

    #endregion

    #region Apertura de Formularios Modales Auxiliares (Atajos de Catálogo)

    /// <summary>
    /// Abre el cuadro de diálogo modal de Alta de Vacuna (#C88A96) y refresca asíncronamente el catálogo al confirmar.
    /// </summary>
    private async void btnAltaVacuna_Click(object? sender, EventArgs e)
    {
        // 1. Determinar especie sugerida de la mascota actualmente seleccionada
        var idMascota = ObtenerIdSeleccionado(cboMascotas);
        long? idEspecieSugerida = null;
        if (idMascota.HasValue && idMascota.Value > 0)
        {
            var mascota = _listaMascotas.FirstOrDefault(m => m.Id == idMascota.Value);
            idEspecieSugerida = mascota?.IdEspecie;
        }

        // 2. Abrir formulario modal FormAltaVacuna
        using var modalAlta = new FormAltaVacuna(_vacunaControlador, null, idEspecieSugerida);
        var resultadoModal = modalAlta.ShowDialog(this);

        // 3. Si se confirmó el alta, refrescar catálogo de vacunas
        if (resultadoModal == DialogResult.OK)
        {
            await CargarCatalogoVacunasAsync();
            lblInfoEstado.Text = "Vacuna incorporada al catálogo. Lista actualizada.";
        }
    }

    /// <summary>
    /// Abre el cuadro de diálogo modal de Modificación/Baja de Vacuna (#E2D9DC) y refresca catálogo y grilla al confirmar.
    /// </summary>
    private async void btnModificarVacuna_Click(object? sender, EventArgs e)
    {
        var idVacuna = ObtenerIdSeleccionado(cboVacunas);

        // 1. Abrir formulario modal FormModificarVacuna
        using var modalModificar = new FormModificarVacuna(_vacunaControlador, idVacuna);
        var resultadoModal = modalModificar.ShowDialog(this);

        // 2. Si se confirmó modificación o baja lógica, refrescar catálogo y grilla
        if (resultadoModal == DialogResult.OK)
        {
            await CargarCatalogoVacunasAsync();
            await CargarAplicacionesGrillaAsync();
            lblInfoEstado.Text = "Catálogo de vacunas y grilla clínica actualizados.";
        }
    }

    #endregion

    #region Acciones Principales: Guardar, Modificar, Limpiar y Volver

    /// <summary>
    /// Registra la aplicación de la vacuna capturando el precio histórico vigente en la consulta seleccionada.
    /// </summary>
    private async void btnGuardar_Click(object? sender, EventArgs e)
    {
        if (_aplicacionVacunaControlador is null) return;

        // 1. Validar selección obligatoria de mascota
        var idMascota = ObtenerIdSeleccionado(cboMascotas);
        if (!idMascota.HasValue || idMascota.Value <= 0)
        {
            MessageBox.Show(
                "Debe seleccionar una mascota / paciente registrado.",
                "Paciente Requerido",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning);
            cboMascotas.Focus();
            return;
        }

        // 2. Validar selección obligatoria de consulta clínica
        var idConsulta = ObtenerIdSeleccionado(cboConsultas);
        if (!idConsulta.HasValue || idConsulta.Value <= 0)
        {
            MessageBox.Show(
                "Debe seleccionar una consulta clínica activa para registrar la aplicación de la vacuna.",
                "Consulta Requerida",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning);
            cboConsultas.Focus();
            return;
        }

        // 3. Validar selección de vacuna
        var idVacuna = ObtenerIdSeleccionado(cboVacunas);
        if (!idVacuna.HasValue || idVacuna.Value <= 0)
        {
            MessageBox.Show(
                "Debe seleccionar la vacuna que se ha aplicado al paciente.",
                "Vacuna Requerida",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning);
            cboVacunas.Focus();
            return;
        }

        // 4. Validar coherencia temporal de fechas
        if (dtpProximaDosis.Value.Date < dtpFechaAplicacion.Value.Date)
        {
            MessageBox.Show(
                "La fecha de la próxima dosis no puede ser anterior a la fecha de aplicación actual.",
                "Fechas Incoherentes",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning);
            dtpProximaDosis.Focus();
            return;
        }

        // 5. Capturar el precio unitario vigente de la vacuna o null si es vacuna previa / externa
        var vacunaSeleccionada = _todasLasVacunas.FirstOrDefault(v => v.Id == idVacuna.Value);
        decimal? precioHistorico = chkVacunaPrevia.Checked
            ? null
            : (vacunaSeleccionada?.Precio ?? 0.00m);

        // 6. Construir DTO de solicitud
        var solicitud = new AplicacionVacunaSolicitudDto
        {
            IdConsulta = idConsulta.Value,
            IdVacuna = idVacuna.Value,
            FechaAplicacion = dtpFechaAplicacion.Value.Date,
            ProximaDosis = dtpProximaDosis.Value.Date,
            PrecioUnitario = precioHistorico,
            Observaciones = string.IsNullOrWhiteSpace(txtObservaciones.Text) ? null : txtObservaciones.Text.Trim()
        };

        btnGuardar.Enabled = false;

        try
        {
            // 7. Ejecutar persistencia con Result Pattern
            var resultado = await _aplicacionVacunaControlador.CrearAsync(solicitud);
            if (!resultado.EsExitoso)
            {
                MessageBox.Show(
                    resultado.Mensaje,
                    "Error al Guardar Aplicación",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                btnGuardar.Enabled = true;
                return;
            }

            var detallePrecio = precioHistorico.HasValue
                ? $"Precio Unitario registrado: {precioHistorico.Value.ToString("C2", CulturaArgentina)}"
                : "Registrada como vacuna previa / externa (sin costo en consulta).";

            MessageBox.Show(
                $"Aplicación de vacuna registrada exitosamente.\n{detallePrecio}",
                "Registro Exitoso",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);

            // 8. Limpiar observaciones, casilla previa y actualizar grilla de forma asíncrona
            txtObservaciones.Clear();
            chkVacunaPrevia.Checked = false;
            _idAplicacionSeleccionada = null;
            await CargarAplicacionesGrillaAsync();
        }
        catch (Exception ex)
        {
            MessageBox.Show(
                $"Ocurrió un error inesperado al registrar la aplicación: {ex.Message}",
                "Error Inesperado",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
        }
        finally
        {
            btnGuardar.Enabled = true;
        }
    }

    /// <summary>
    /// Modifica los datos de la aplicación de vacuna seleccionada en la grilla.
    /// </summary>
    private async void btnModificar_Click(object? sender, EventArgs e)
    {
        if (_aplicacionVacunaControlador is null) return;

        if (!_idAplicacionSeleccionada.HasValue || _idAplicacionSeleccionada.Value <= 0)
        {
            MessageBox.Show(
                "Debe seleccionar una aplicación de vacuna de la grilla para modificar.",
                "Selección Requerida",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning);
            return;
        }

        var idVacuna = ObtenerIdSeleccionado(cboVacunas);
        if (!idVacuna.HasValue || idVacuna.Value <= 0)
        {
            MessageBox.Show(
                "Debe seleccionar una vacuna válida.",
                "Vacuna Requerida",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning);
            return;
        }

        decimal? precioModificado = chkVacunaPrevia.Checked
            ? null
            : (_todasLasVacunas.FirstOrDefault(v => v.Id == idVacuna.Value)?.Precio ?? 0.00m);

        var idConsulta = ObtenerIdSeleccionado(cboConsultas);

        var solicitud = new AplicacionVacunaSolicitudDto
        {
            IdConsulta = idConsulta ?? 0,
            IdVacuna = idVacuna.Value,
            FechaAplicacion = dtpFechaAplicacion.Value.Date,
            ProximaDosis = dtpProximaDosis.Value.Date,
            PrecioUnitario = precioModificado,
            Observaciones = string.IsNullOrWhiteSpace(txtObservaciones.Text) ? null : txtObservaciones.Text.Trim()
        };

        btnModificar.Enabled = false;

        try
        {
            var resultado = await _aplicacionVacunaControlador.ActualizarAsync(_idAplicacionSeleccionada.Value, solicitud);
            if (!resultado.EsExitoso)
            {
                MessageBox.Show(
                    resultado.Mensaje,
                    "Error al Modificar",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                btnModificar.Enabled = true;
                return;
            }

            MessageBox.Show(
                "Aplicación de vacuna actualizada exitosamente.",
                "Operación Exitosa",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);

            txtObservaciones.Clear();
            chkVacunaPrevia.Checked = false;
            _idAplicacionSeleccionada = null;
            btnModificar.Enabled = false;
            btnGuardar.Enabled = true;
            await CargarAplicacionesGrillaAsync();
            dgvVacunas.ClearSelection();
        }
        catch (Exception ex)
        {
            MessageBox.Show(
                $"Ocurrió un error al actualizar: {ex.Message}",
                "Error Inesperado",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
            btnModificar.Enabled = true;
        }
    }

    /// <summary>
    /// Restablece integralmente todos los campos, selectores, controles de fecha,
    /// estado de botones y recarga la grilla general de aplicaciones sin filtros.
    /// </summary>
    public async Task LimpiarFormularioAsync()
    {
        _cargandoMascotas = true;
        _cargandoConsultas = true;
        _cargandoVacunas = true;

        try
        {
            // 1. Deseleccionar identificador en edición y restaurar botones de acción
            _idAplicacionSeleccionada = null;
            btnModificar.Enabled = false;
            btnGuardar.Enabled = true;

            // 2. Limpiar selector de paciente y su texto autocompletado
            cboMascotas.SelectedIndex = -1;
            cboMascotas.Text = string.Empty;

            // 3. Limpiar selector de consultas médicas en cascada
            _listaConsultas.Clear();
            cboConsultas.DataSource = null;
            cboConsultas.Items.Clear();
            cboConsultas.Text = string.Empty;
            cboConsultas.SelectedIndex = -1;

            // 4. Restaurar catálogo de vacunas sin filtro de especie
            PoblarComboVacunas();
            cboVacunas.SelectedIndex = cboVacunas.Items.Count > 0 ? 0 : -1;

            // 5. Limpiar casilla de vacuna previa / externa y observaciones
            chkVacunaPrevia.Checked = false;
            txtObservaciones.Clear();

            // 6. Restablecer controles de fechas predeterminadas
            dtpFechaAplicacion.Value = DateTime.Today;
            dtpProximaDosis.Value = DateTime.Today.AddMonths(12);

            // 7. Si hay vacuna seleccionada por defecto, sugerir fechas según su periodicidad
            ActualizarSugerenciaFechasVacuna();
        }
        finally
        {
            _cargandoMascotas = false;
            _cargandoConsultas = false;
            _cargandoVacunas = false;
        }

        // 8. Recargar todas las aplicaciones generales en la grilla y deseleccionar filas
        await CargarAplicacionesGrillaAsync();
        dgvVacunas.ClearSelection();

        // 9. Actualizar estado y colocar foco en el selector de pacientes
        lblInfoEstado.Text = "Formulario restablecido. Listo para registrar una nueva aplicación de vacuna.";
        cboMascotas.Focus();
    }

    /// <summary>
    /// Restablece todos los campos de entrada a su estado inicial y recarga el historial general (reemplaza al botón Cancelar).
    /// </summary>
    private async void btnLimpiar_Click(object? sender, EventArgs e)
    {
        btnLimpiar.Enabled = false;
        try
        {
            await LimpiarFormularioAsync();
        }
        catch (Exception ex)
        {
            MessageBox.Show(
                $"Ocurrió un error al restablecer el formulario: {ex.Message}",
                "Error al Limpiar",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
        }
        finally
        {
            btnLimpiar.Enabled = true;
        }
    }

    /// <summary>
    /// Cierra el formulario de vacunación retornando al shell principal.
    /// </summary>
    private void btnVolver_Click(object? sender, EventArgs e)
    {
        Close();
    }

    #endregion

    #region Utilidades Auxiliares

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

    private static long? ObtenerIdSeleccionado(ComboBox combo)
    {
        if (combo.SelectedValue is long idLong && idLong > 0)
            return idLong;

        if (combo.SelectedValue is int idInt && idInt > 0)
            return idInt;

        if (combo.SelectedItem is ComboItemGenerico item && item.Id > 0)
            return item.Id;

        return null;
    }

    private sealed class ComboItemGenerico
    {
        public long Id { get; init; }
        public string Texto { get; init; } = string.Empty;
    }

    #endregion
}
