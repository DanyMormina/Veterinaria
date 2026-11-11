using System.Globalization;
using Veterinaria.Controllers.Controladores;
using Veterinaria.Domain.Comunes;
using Veterinaria.Domain.Dtos;
using Veterinaria.WinForms.Sesion;

namespace Veterinaria.WinForms.Vistas.Veterinario;
// Formulario de atenci�n cl�nica para el registro y control de vacunas en pacientes veterinarios.
// Adhiere al sistema de dise�o "Ejecutivo Rom�ntico Pastel", Result Pattern (.NET 10 / C#),
// autocompletado en tiempo real, cascadas dependientes y captura de precios hist�ricos.
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
    // Constructor por defecto requerido para el soporte del dise�ador de Windows Forms.
    public FormVacunasControles() : this(null, null, null, null)
    {
    }
    // Constructor principal con inyecci�n de dependencias de controladores cl�nicos.
    public FormVacunasControles(
        MascotaControlador? mascotaControlador,
        ConsultaControlador? consultaControlador,
        VacunaControlador? vacunaControlador,
        AplicacionVacunaControlador? aplicacionVacunaControlador)
    {
        InitializeComponent();
        Controles.ComboBuscable.Habilitar(cboMascota, cboConsulta, cboVacuna);
        _mascotaControlador = mascotaControlador;
        _consultaControlador = consultaControlador;
        _vacunaControlador = vacunaControlador;
        _aplicacionVacunaControlador = aplicacionVacunaControlador;

        ConfigurarRestriccionesTeclado();
        dgvVacunas.CellClick += (_, _) => dgvVacunas_SelectionChanged(null, EventArgs.Empty);
    }
    // Inicializa la sesi�n del veterinario actuante, estado de controles y precarga de cat�logos.
    private async void FormVacunasControles_Load(object? sender, EventArgs e)
    {
        // 1. Configuraci�n de encabezado y sesi�n cl�nica
        lblUsuarioSesion.Text = SesionActual.EstaAutenticado
            ? $"Dr./Dra. {SesionActual.NombreCompleto} | {SesionActual.Rol}"
            : "Dr./Dra. Luc�a P�rez | Veterinario";

        lblInfoEstado.Text = $"Sesi�n cl�nica activa: {SesionActual.NombreUsuario} --- {DateTime.Now:dd/MM/yyyy}";

        // 2. Establecer fechas predeterminadas
        dtpFechaAplicacion.MaxDate = DateTime.Today.AddDays(1).AddTicks(-1);
        dtpFechaAplicacion.Value = DateTime.Today;
        dtpProximaDosis.Value = DateTime.Today.AddMonths(12);

        // 3. Inicializar grilla y estado de botones
        dgvVacunas.Rows.Clear();
        btnGuardar.Enabled = true;

        // 4. Precarga as�ncrona de cat�logos base (sin seleccionar nada en el formulario)
        await CargarCatalogoMascotasAsync();
        await CargarCatalogoVacunasAsync();
        await CargarAplicacionesGrillaAsync();
        if (cboMascotas.Items.Count > 0)
            cboMascotas.SelectedIndex = -1;
        if (cboVacunas.Items.Count > 0)
            cboVacunas.SelectedIndex = -1;
        cboConsultas.DataSource = null;
    }

    #region Carga de Datos y Cat�logos
    // Carga el padr�n de mascotas activas en el selector con b�squeda y autocompletado.
    // Formato: "{NombreMascota} - {NombrePropietario} {ApellidoPropietario}".
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
    // Carga el cat�logo maestro de vacunas activas y las almacena en memoria local.
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
                lblInfoEstado.Text = "No se pudieron obtener las vacunas del cat�logo.";
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
    // Puebla el desplegable de vacunas activas, filtrando opcionalmente por la especie de la mascota seleccionada.
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
                ? $"{v.Nombre} --- Precio: {v.Precio.ToString("C2", CulturaArgentina)} --- Stock: {v.Stock}"
                : $"{v.Nombre} ({v.NombreEspecie}) --- Precio: {v.Precio.ToString("C2", CulturaArgentina)} --- Stock: {v.Stock}"
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
    // Carga las consultas cl�nicas activas asociadas a la mascota seleccionada,
    // ordenadas cronol�gicamente de la m�s reciente a la m�s antigua.
    private async Task CargarConsultasDeMascotaAsync(long idMascota)
    {
        if (_consultaControlador is null) return;

        _cargandoConsultas = true;
        try
        {
            // Consulta cl�nica ordenada por FechaHora desc
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
                var diag = string.IsNullOrWhiteSpace(c.Diagnostico) ? "(Sin diagn�stico registrado)" : c.Diagnostico.Trim();
                var diagCorto = diag.Length > 40 ? string.Concat(diag.AsSpan(0, 37), "...") : diag;

                return new ComboItemGenerico
                {
                    Id = c.Id,
                    Texto = $"{c.FechaHora:dd/MM/yyyy HH:mm} --- Diagn�stico: {diagCorto}"
                };
            }).ToList();

            cboConsultas.DataSource = items;
            cboConsultas.SelectedIndex = -1;

            if (items.Count == 0)
            {
                lblInfoEstado.Text = "La mascota no posee consultas cl�nicas activas registradas.";
            }
        }
        finally
        {
            _cargandoConsultas = false;
        }
    }
    // Consulta y puebla as�ncronamente en la grilla las aplicaciones de vacunas seg�n el contexto activo.
    // Con mascota seleccionada solo muestra lo de ese animal.
    private async Task CargarAplicacionesGrillaAsync(long? idMascotaForzada = null, long? idConsultaForzada = null)
    {
        if (_aplicacionVacunaControlador is null) return;

        _cargandoGrilla = true;
        try
        {
            var idConsulta = idConsultaForzada ?? ObtenerIdSeleccionado(cboConsultas);
            var idMascota = idMascotaForzada ?? ObtenerIdSeleccionado(cboMascotas);

            Resultado<IEnumerable<AplicacionVacunaRespuestaDto>> resultado;

            // Prioridad: mascota (todo lo del animal). Si solo hay consulta, filtrar por ella.
            if (idMascota.HasValue && idMascota.Value > 0)
            {
                resultado = await _aplicacionVacunaControlador.ObtenerPorMascotaAsync(idMascota.Value);
            }
            else if (idConsulta.HasValue && idConsulta.Value > 0)
            {
                resultado = await _aplicacionVacunaControlador.ObtenerPorConsultaAsync(idConsulta.Value);
            }
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
                    : (cboMascotas.SelectedItem as ComboItemGenerico)?.Texto.Split('-')[0].Trim() ?? "---";

                var rowIndex = dgvVacunas.Rows.Add(
                    nombreMascota,
                    a.NombreVacuna,
                    a.FechaAplicacion.ToString("dd/MM/yyyy"),
                    a.ProximaDosis?.ToString("dd/MM/yyyy") ?? "---",
                    a.Observaciones ?? string.Empty);

                dgvVacunas.Rows[rowIndex].Tag = a.Id;
            }

            lblInfoEstado.Text = $"Se cargaron {_listaAplicaciones.Count} registro(s) de vacunaci�n.";
            dgvVacunas.ClearSelection();
            dgvVacunas.CurrentCell = null;
            _idAplicacionSeleccionada = null;
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
    // Al cambiar la mascota seleccionada:
    // 1. Filtra las consultas en cascada ordenadas cronol�gicamente de la m�s reciente a la m�s antigua.
    // 2. Filtra el cat�logo de vacunas seg�n la especie del paciente.
    // 3. Actualiza la grilla de aplicaciones hist�ricas.
    private async void cboMascota_SelectedIndexChanged(object? sender, EventArgs e)
    {
        if (_cargandoMascotas || _cargandoSeleccion) return;

        var idMascota = ObtenerIdSeleccionado(cboMascotas);
        if (!idMascota.HasValue || idMascota.Value <= 0)
        {
            cboConsultas.DataSource = null;
            PoblarComboVacunas();
            btnGuardar.Enabled = true;
            await CargarAplicacionesGrillaAsync();
            dgvVacunas.ClearSelection();
            return;
        }

        // 1. Refrescar cat�logo con stock actualizado y filtrar por la especie de la mascota
        await CargarCatalogoVacunasAsync();

        // 2. Poblar consultas cl�nicas en cascada (OrderByDescending FechaHora)
        await CargarConsultasDeMascotaAsync(idMascota.Value);

        // 3. Sincronizar grilla con aplicaciones de la mascota
        await CargarAplicacionesGrillaAsync();
    }
    // Al cambiar la consulta cl�nica seleccionada, sincroniza las vacunas aplicadas a dicha consulta.
    private async void cboConsulta_SelectedIndexChanged(object? sender, EventArgs e)
    {
        if (_cargandoConsultas || _cargandoSeleccion) return;
        await CargarAplicacionesGrillaAsync();
    }
    // Al seleccionar una vacuna, actualiza autom�ticamente la fecha sugerida de la pr�xima dosis.
    private void cboVacuna_SelectedIndexChanged(object? sender, EventArgs e)
    {
        if (_cargandoVacunas || _cargandoSeleccion) return;
        ActualizarSugerenciaFechasVacuna();
        ActualizarImporteDesdeVacuna();
    }

    private void chkVacunaPrevia_CheckedChanged(object? sender, EventArgs e)
    {
        if (chkVacunaPrevia.Checked)
        {
            txtImporte.Text = "0,00";
            txtImporte.Enabled = false;
        }
        else
        {
            txtImporte.Enabled = true;
            ActualizarImporteDesdeVacuna();
        }
    }

    private void ActualizarImporteDesdeVacuna()
    {
        if (chkVacunaPrevia.Checked)
        {
            txtImporte.Text = "0,00";
            return;
        }

        var idVacuna = ObtenerIdSeleccionado(cboVacunas);
        if (!idVacuna.HasValue || idVacuna.Value <= 0)
        {
            txtImporte.Clear();
            return;
        }

        var vacuna = _todasLasVacunas.FirstOrDefault(v => v.Id == idVacuna.Value);
        txtImporte.Text = (vacuna?.Precio ?? 0m).ToString("N2", CulturaArgentina);
    }
    // Al modificar la fecha de aplicaci�n, recalcula las fechas de pr�xima dosis y pr�ximo control.
    private void dtpFechaAplicacion_ValueChanged(object? sender, EventArgs e)
    {
        if (_cargandoSeleccion) return;
        ActualizarSugerenciaFechasVacuna();
    }
    // Calcula y sugiere en los DateTimePicker de control la pr�xima fecha sumando el per�odo recomendado.
    private void ActualizarSugerenciaFechasVacuna()
    {
        var idVacuna = ObtenerIdSeleccionado(cboVacunas);
        if (!idVacuna.HasValue || idVacuna.Value <= 0) return;

        var vacuna = _todasLasVacunas.FirstOrDefault(v => v.Id == idVacuna.Value);
        if (vacuna is null || vacuna.PeriodoMesesRecomendado <= 0) return;

        var fechaCalculada = dtpFechaAplicacion.Value.AddMonths(vacuna.PeriodoMesesRecomendado);
        dtpProximaDosis.Value = fechaCalculada;
    }
    // Al hacer clic o cambiar fila en la grilla, carga la aplicaci�n en los campos para eventual edici�n.
    private async void dgvVacunas_SelectionChanged(object? sender, EventArgs e)
    {
        if (_cargandoGrilla || _cargandoSeleccion) return;

        if (dgvVacunas.SelectedRows.Count == 0)
        {
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

                    // 3. Poblar cat�logo de vacunas filtrado por la especie del paciente y seleccionar la vacuna
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
                    txtImporte.Enabled = app.PrecioUnitario.HasValue;
                    txtImporte.Text = app.PrecioUnitario.HasValue
                        ? app.PrecioUnitario.Value.ToString("N2", CulturaArgentina)
                        : "0,00";
                    txtObservaciones.Text = app.Observaciones ?? string.Empty;
                    btnGuardar.Enabled = true;
                    lblInfoEstado.Text = $"Aplicaci�n #{app.Id} ({app.NombreVacuna} - {app.NombreMascota}): solo consulta. No se puede modificar ni eliminar. Para registrar otra, complete los datos y pulse Aplicar (o Limpiar antes).";
                }
                finally
                {
                    _cargandoSeleccion = false;
                }
            }
        }
    }

    #endregion

    #region Apertura de Formularios Modales Auxiliares (Atajos de Cat�logo)
    // Abre el cuadro de di�logo modal de Alta de Vacuna (#C88A96) y refresca as�ncronamente el cat�logo al confirmar.
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

        // 3. Si se confirm� el alta, refrescar cat�logo de vacunas
        if (resultadoModal == DialogResult.OK)
        {
            await CargarCatalogoVacunasAsync();
            lblInfoEstado.Text = "Vacuna incorporada al cat�logo. Lista actualizada.";
        }
    }
    // Abre el cuadro de di�logo modal de Modificaci�n/Baja de Vacuna (#E2D9DC) y refresca cat�logo y grilla al confirmar.
    private async void btnModificarVacuna_Click(object? sender, EventArgs e)
    {
        var idVacuna = ObtenerIdSeleccionado(cboVacunas);

        // 1. Abrir formulario modal FormModificarVacuna
        using var modalModificar = new FormModificarVacuna(_vacunaControlador, idVacuna);
        var resultadoModal = modalModificar.ShowDialog(this);

        // 2. Si se confirm� modificaci�n o baja l�gica, refrescar cat�logo y grilla
        if (resultadoModal == DialogResult.OK)
        {
            await CargarCatalogoVacunasAsync();
            btnGuardar.Enabled = true;
            await CargarAplicacionesGrillaAsync();
            dgvVacunas.ClearSelection();
            lblInfoEstado.Text = "Cat�logo de vacunas y grilla cl�nica actualizados.";
        }
    }

    #endregion

    #region Acciones Principales: Aplicar, Limpiar y Volver
    // Aplica la vacuna capturando el precio hist�rico vigente en la consulta seleccionada.
    private async void btnGuardar_Click(object? sender, EventArgs e)
    {
        if (_aplicacionVacunaControlador is null) return;

        // 1. Validar selecci�n obligatoria de mascota
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

        // 2. Validar selecci�n obligatoria de consulta cl�nica
        var idConsulta = ObtenerIdSeleccionado(cboConsultas);
        if (!idConsulta.HasValue || idConsulta.Value <= 0)
        {
            MessageBox.Show(
                "Debe seleccionar una consulta cl�nica activa para registrar la aplicaci�n de la vacuna.",
                "Consulta Requerida",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning);
            cboConsultas.Focus();
            return;
        }

        // 3. Validar selecci�n de vacuna
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
                "La fecha de la pr�xima dosis no puede ser anterior a la fecha de aplicaci�n actual.",
                "Fechas Incoherentes",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning);
            dtpProximaDosis.Focus();
            return;
        }

        // 5. Capturar el importe de la vacuna (precio del cat�logo) o null si es vacuna previa / externa
        decimal? precioHistorico = null;
        if (!chkVacunaPrevia.Checked)
        {
            var textoImporte = txtImporte.Text.Trim().Replace("$", string.Empty).Trim();
            if (!decimal.TryParse(textoImporte, NumberStyles.Any, CulturaArgentina, out var importe) &&
                !decimal.TryParse(textoImporte.Replace(',', '.'), NumberStyles.Any, CultureInfo.InvariantCulture, out importe))
            {
                MessageBox.Show(
                    "El importe de la vacuna es obligatorio y debe ser un n�mero v�lido.",
                    "Importe requerido",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                txtImporte.Focus();
                return;
            }

            if (importe <= 0)
            {
                MessageBox.Show(
                    "El importe de la vacuna debe ser mayor a 0 (o marque vacuna previa / externa).",
                    "Importe inv�lido",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                txtImporte.Focus();
                return;
            }

            precioHistorico = decimal.Round(importe, 2);
        }

        // 6. Validar stock antes de pedir confirmaci�n (las previas / externas no descuentan)
        if (precioHistorico.HasValue)
        {
            var vacuna = _todasLasVacunas.FirstOrDefault(v => v.Id == idVacuna.Value);
            var stockActual = vacuna?.Stock ?? 0;
            if (stockActual < 1)
            {
                MessageBox.Show(
                    $"No hay stock suficiente de esta vacuna. Stock actual: {stockActual}.",
                    "Sin stock",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                cboVacunas.Focus();
                return;
            }
        }

        // 8. Confirmar: una vez guardado no se puede modificar ni eliminar
        var confirmar = MessageBox.Show(
            "�Confirma los datos de esta aplicaci�n de vacuna?\n\nSi confirma, el registro quedar� guardado y no habr� vuelta atr�s (no se podr� modificar ni eliminar).",
            "Confirmar aplicaci�n de vacuna",
            MessageBoxButtons.YesNo,
            MessageBoxIcon.Question,
            MessageBoxDefaultButton.Button2);
        if (confirmar != DialogResult.Yes)
            return;

        // 9. Construir DTO de solicitud
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
            // 10. Ejecutar persistencia con Result Pattern
            var resultado = await _aplicacionVacunaControlador.CrearAsync(solicitud);
            if (!resultado.EsExitoso)
            {
                MessageBox.Show(
                    resultado.Mensaje,
                    "Error al aplicar vacuna",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                btnGuardar.Enabled = true;
                return;
            }

            var detallePrecio = precioHistorico.HasValue
                ? $"Precio Unitario registrado: {precioHistorico.Value.ToString("C2", CulturaArgentina)}"
                : "Registrada como vacuna previa / externa (sin costo en consulta).";

            MessageBox.Show(
                $"Aplicaci�n de vacuna registrada exitosamente.\n{detallePrecio}",
                "Registro Exitoso",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);

            // 9. Mantener mascota y consulta; solo dejar listo para otra aplicaci�n del mismo animal
            var idMascotaActual = idMascota.Value;
            var idConsultaActual = idConsulta.Value;
            txtObservaciones.Clear();
            chkVacunaPrevia.Checked = false;
            _idAplicacionSeleccionada = null;
            btnGuardar.Enabled = true;
            await CargarCatalogoVacunasAsync();
            if (cboVacunas.Items.Count > 0)
            {
                cboVacunas.SelectedIndex = -1;
                cboVacunas.Text = string.Empty;
            }
            await CargarAplicacionesGrillaAsync(idMascotaActual, idConsultaActual);
            dgvVacunas.ClearSelection();
            lblInfoEstado.Text = $"Vacuna aplicada. Se listan solo las aplicaciones de la mascota seleccionada ({_listaAplicaciones.Count}).";
        }
        catch (Exception ex)
        {
            MessageBox.Show(
                $"Ocurri� un error inesperado al registrar la aplicaci�n: {ex.Message}",
                "Error Inesperado",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
        }
        finally
        {
            btnGuardar.Enabled = true;
        }
    }
    // Restablece integralmente todos los campos, selectores, controles de fecha,
    // estado de botones y recarga la grilla general de aplicaciones sin filtros.
    public async Task LimpiarFormularioAsync()
    {
        _cargandoMascotas = true;
        _cargandoConsultas = true;
        _cargandoVacunas = true;

        try
        {
            // 1. Deseleccionar identificador en edici�n y restaurar botones de acci�n
            _idAplicacionSeleccionada = null;
            btnGuardar.Enabled = true;

            // 2. Limpiar selector de paciente y su texto autocompletado
            cboMascotas.SelectedIndex = -1;
            cboMascotas.Text = string.Empty;

            // 3. Limpiar selector de consultas m�dicas en cascada
            _listaConsultas.Clear();
            cboConsultas.DataSource = null;
            cboConsultas.Items.Clear();
            cboConsultas.Text = string.Empty;
            cboConsultas.SelectedIndex = -1;

            // 4. Restaurar cat�logo de vacunas sin filtro de especie
            PoblarComboVacunas();
            cboVacunas.SelectedIndex = cboVacunas.Items.Count > 0 ? 0 : -1;

            // 5. Limpiar casilla de vacuna previa / externa y observaciones
            chkVacunaPrevia.Checked = false;
            txtImporte.Enabled = true;
            txtImporte.Clear();
            txtObservaciones.Clear();

            // 6. Restablecer controles de fechas predeterminadas
            dtpFechaAplicacion.Value = DateTime.Today;
            dtpProximaDosis.Value = DateTime.Today.AddMonths(12);

            // 7. Si hay vacuna seleccionada por defecto, sugerir fechas seg�n su periodicidad
            ActualizarSugerenciaFechasVacuna();
        }
        finally
        {
            _cargandoMascotas = false;
            _cargandoConsultas = false;
            _cargandoVacunas = false;
        }

        // 8. Recargar cat�logo actualizado de vacunas y todas las aplicaciones generales en la grilla
        await CargarCatalogoVacunasAsync();
        await CargarAplicacionesGrillaAsync();
        dgvVacunas.ClearSelection();

        // 9. Actualizar estado y colocar foco en el selector de pacientes
        lblInfoEstado.Text = "Formulario restablecido. Listo para registrar una nueva aplicaci�n de vacuna.";
        cboMascotas.Focus();
    }
    // Restablece todos los campos de entrada a su estado inicial y recarga el historial general (reemplaza al bot�n Cancelar).
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
                $"Ocurri� un error al restablecer el formulario: {ex.Message}",
                "Error al Limpiar",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
        }
        finally
        {
            btnLimpiar.Enabled = true;
        }
    }
    // Cierra el formulario de vacunaci�n retornando al shell principal.
    private void btnVolver_Click(object? sender, EventArgs e)
    {
        Close();
    }

    #endregion

    #region Utilidades Auxiliares
    // Configura restricciones de tipeo en tiempo real sobre los controles del formulario.
    private void ConfigurarRestriccionesTeclado()
    {
        cboMascotas.KeyPress += ValidarSoloLetras_KeyPress;
    }
    // Restringe la b�squeda en el selector de paciente exclusivamente a letras y espacios, bloqueando n�meros y caracteres especiales.
    private void ValidarSoloLetras_KeyPress(object? sender, KeyPressEventArgs e)
    {
        // 1. Permitir teclas de control del sistema (retroceso, cortar/copiar, etc.)
        if (char.IsControl(e.KeyChar))
            return;

        // 2. Permitir exclusivamente caracteres alfab�ticos (incluye vocales acentuadas �����, � y di�resis)
        if (char.IsLetter(e.KeyChar))
            return;

        // 3. Permitir espacio simple si no es al inicio y no es consecutivo
        if (e.KeyChar == ' ' && sender is ComboBox cb)
        {
            if (cb.SelectionStart > 0 && cb.Text.Length > 0 && cb.Text[cb.SelectionStart - 1] != ' ')
                return;
        }

        // 4. Bloquear cualquier n�mero (0-9), s�mbolo o car�cter especial no alfab�tico
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

        // ComboBuscable a veces deja el texto v�lido sin SelectedItem; resolver por coincidencia exacta
        var texto = combo.Text?.Trim() ?? string.Empty;
        if (texto.Length > 0)
        {
            foreach (var obj in combo.Items)
            {
                if (obj is ComboItemGenerico candidato &&
                    string.Equals(candidato.Texto, texto, StringComparison.OrdinalIgnoreCase) &&
                    candidato.Id > 0)
                    return candidato.Id;
            }
        }

        return null;
    }

    private sealed class ComboItemGenerico
    {
        public long Id { get; init; }
        public string Texto { get; init; } = string.Empty;
    }

    #endregion
}
