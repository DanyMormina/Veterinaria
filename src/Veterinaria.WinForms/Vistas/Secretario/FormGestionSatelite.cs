using Veterinaria.Controllers.Controladores;
using Veterinaria.Domain.Dtos;

namespace Veterinaria.WinForms.Vistas.Secretario;

/// <summary>
/// Modos de operación para el formulario modal de gestión satélite.
/// </summary>
public enum ModoGestion
{
    AltaEspecie,
    ModificarEspecie,
    AltaRaza,
    ModificarRaza
}

/// <summary>
/// Formulario modal polimórfico para la gestión auxiliar de Especies y Razas (Alta, Modificación y Baja lógica).
/// </summary>
public partial class FormGestionSatelite : Form
{
    private static readonly Color ColorFondoError = ColorTranslator.FromHtml("#FDECEF");

    private readonly ModoGestion _modo;
    private readonly EspecieControlador? _especieControlador;
    private readonly RazaControlador? _razaControlador;
    private readonly long? _idEspeciePreseleccionada;
    private bool _cargandoDatos = false;

    /// <summary>
    /// Constructor principal configurable con controladores inyectados o provistos.
    /// </summary>
    public FormGestionSatelite(
        ModoGestion modo,
        EspecieControlador? especieControlador = null,
        RazaControlador? razaControlador = null,
        long? idEspeciePreseleccionada = null)
    {
        _modo = modo;
        _especieControlador = especieControlador;
        _razaControlador = razaControlador;
        _idEspeciePreseleccionada = idEspeciePreseleccionada;

        InitializeComponent();
        ConfigurarEstiloSegunModo();
        ConfigurarRestriccionesTeclado();
        ConfigurarEventosLimpiezaError();
    }

    /// <summary>
    /// Configura la visibilidad y disposición de los controles según el modo activo.
    /// </summary>
    private void ConfigurarEstiloSegunModo()
    {
        bool esModoEdicion = _modo is ModoGestion.ModificarEspecie or ModoGestion.ModificarRaza;
        bool esModoRaza = _modo is ModoGestion.AltaRaza or ModoGestion.ModificarRaza;

        // Título del encabezado
        lblTitulo.Text = _modo switch
        {
            ModoGestion.AltaEspecie => "ALTA DE NUEVA ESPECIE",
            ModoGestion.ModificarEspecie => "MODIFICACIÓN / BAJA DE ESPECIE",
            ModoGestion.AltaRaza => "ALTA DE NUEVA RAZA",
            ModoGestion.ModificarRaza => "MODIFICACIÓN / BAJA DE RAZA",
            _ => "GESTIÓN SATÉLITE"
        };
        Text = lblTitulo.Text;

        // Visibilidad de controles de selección superior (Solo en modificación)
        lblSeleccion.Visible = esModoEdicion;
        cboSeleccion.Visible = esModoEdicion;

        // Visibilidad de especie padre (Solo en razas)
        lblParentEspecie.Visible = esModoRaza;
        cboParentEspecie.Visible = esModoRaza;

        // Visibilidad y reacomodo de botones inferiores
        btnGuardar.Visible = !esModoEdicion;
        btnModificar.Visible = esModoEdicion;
        btnBaja.Visible = esModoEdicion;

        if (esModoEdicion)
        {
            btnModificar.Location = new Point(176, 10);
            btnBaja.Location = new Point(282, 10);
            btnCancelar.Location = new Point(386, 10);
        }
        else
        {
            btnGuardar.Location = new Point(268, 10);
            btnCancelar.Location = new Point(386, 10);
        }

        // Reubicar dinámicamente los campos en el panel según el modo para mantener balance visual
        if (_modo == ModoGestion.AltaEspecie)
        {
            lblNombre.Location = new Point(24, 40);
            txtNombre.Location = new Point(24, 65);
        }
        else if (_modo == ModoGestion.AltaRaza)
        {
            lblParentEspecie.Location = new Point(24, 20);
            cboParentEspecie.Location = new Point(24, 45);
            lblNombre.Location = new Point(24, 90);
            txtNombre.Location = new Point(24, 115);
        }
        else if (_modo == ModoGestion.ModificarEspecie)
        {
            lblSeleccion.Location = new Point(24, 20);
            lblSeleccion.Text = "Especie a modificar:";
            cboSeleccion.Location = new Point(24, 45);
            lblNombre.Location = new Point(24, 90);
            txtNombre.Location = new Point(24, 115);
        }
        else if (_modo == ModoGestion.ModificarRaza)
        {
            lblParentEspecie.Location = new Point(24, 10);
            cboParentEspecie.Location = new Point(24, 30);
            lblSeleccion.Location = new Point(24, 68);
            lblSeleccion.Text = "Raza a modificar:";
            cboSeleccion.Location = new Point(24, 88);
            lblNombre.Location = new Point(24, 126);
            txtNombre.Location = new Point(24, 146);
        }
    }

    /// <summary>
    /// Restringe el tipeo en txtNombre admitiendo únicamente letras, tildes y espacios simples.
    /// </summary>
    private void ConfigurarRestriccionesTeclado()
    {
        txtNombre.KeyPress += (sender, e) =>
        {
            if (char.IsControl(e.KeyChar))
                return;

            if (char.IsLetter(e.KeyChar))
                return;

            if (e.KeyChar == ' ' && sender is TextBox txt && !txt.Text.EndsWith(' '))
                return;

            e.Handled = true;
        };
    }

    private void ConfigurarEventosLimpiezaError()
    {
        txtNombre.Enter += (_, _) => txtNombre.BackColor = Color.White;
        txtNombre.TextChanged += (_, _) => txtNombre.BackColor = Color.White;

        cboSeleccion.SelectedIndexChanged += (_, _) => cboSeleccion.BackColor = Color.White;
        cboParentEspecie.SelectedIndexChanged += (_, _) => cboParentEspecie.BackColor = Color.White;

        btnGuardar.Click += async (_, _) => await GuardarAsync();
        btnModificar.Click += async (_, _) => await ModificarAsync();
        btnBaja.Click += async (_, _) => await DarDeBajaAsync();
        btnCancelar.Click += (_, _) => { DialogResult = DialogResult.Cancel; Close(); };

        cboSeleccion.SelectedIndexChanged += async (_, _) => await cboSeleccion_SelectedIndexChangedAsync();
        cboParentEspecie.SelectedIndexChanged += async (_, _) => await cboParentEspecie_SelectedIndexChangedAsync();
        Load += async (_, _) => await FormGestionSatelite_LoadAsync();
    }

    private async Task FormGestionSatelite_LoadAsync()
    {
        _cargandoDatos = true;
        try
        {
            // Cargar especies activas si es modo raza o si se edita raza
            if (_modo is ModoGestion.AltaRaza or ModoGestion.ModificarRaza)
            {
                await CargarEspeciesPadreAsync();
            }

            // Cargar combo de selección en modos de modificación
            if (_modo == ModoGestion.ModificarEspecie)
            {
                await CargarComboSeleccionEspeciesAsync();
            }
            else if (_modo == ModoGestion.ModificarRaza)
            {
                await CargarComboSeleccionRazasAsync();
            }
        }
        finally
        {
            _cargandoDatos = false;
        }
    }

    private async Task CargarEspeciesPadreAsync()
    {
        cboParentEspecie.Items.Clear();
        if (_especieControlador is null) return;

        var res = await _especieControlador.ObtenerTodosAsync();
        if (res.EsExitoso && res.Valor is not null)
        {
            foreach (var esp in res.Valor.Where(e => e.Activo).OrderBy(e => e.Nombre))
            {
                cboParentEspecie.Items.Add(new ItemCombo(esp.Id, esp.Nombre));
            }
        }

        if (_idEspeciePreseleccionada.HasValue && _idEspeciePreseleccionada.Value > 0)
        {
            SeleccionarPorId(cboParentEspecie, _idEspeciePreseleccionada.Value);
        }
        else if (cboParentEspecie.Items.Count > 0)
        {
            cboParentEspecie.SelectedIndex = 0;
        }
    }

    private async Task CargarComboSeleccionEspeciesAsync()
    {
        cboSeleccion.Items.Clear();
        if (_especieControlador is null) return;

        var res = await _especieControlador.ObtenerTodosAsync();
        if (res.EsExitoso && res.Valor is not null)
        {
            foreach (var esp in res.Valor.Where(e => e.Activo).OrderBy(e => e.Nombre))
            {
                cboSeleccion.Items.Add(new ItemCombo(esp.Id, esp.Nombre));
            }
        }

        if (cboSeleccion.Items.Count > 0)
        {
            if (_idEspeciePreseleccionada.HasValue && _idEspeciePreseleccionada.Value > 0)
                SeleccionarPorId(cboSeleccion, _idEspeciePreseleccionada.Value);
            else
                cboSeleccion.SelectedIndex = 0;
        }
    }

    private async Task CargarComboSeleccionRazasAsync()
    {
        cboSeleccion.Items.Clear();
        txtNombre.Clear();
        var especiePadre = cboParentEspecie.SelectedItem as ItemCombo;
        if (especiePadre?.Id is null || _razaControlador is null) return;

        var res = await _razaControlador.ObtenerPorEspecieAsync(especiePadre.Id.Value);
        if (res.EsExitoso && res.Valor is not null)
        {
            foreach (var r in res.Valor.Where(x => x.Activo).OrderBy(x => x.Nombre))
            {
                cboSeleccion.Items.Add(new ItemCombo(r.Id, r.Nombre));
            }
        }

        if (cboSeleccion.Items.Count > 0)
        {
            cboSeleccion.SelectedIndex = 0;
        }
    }

    private async Task cboParentEspecie_SelectedIndexChangedAsync()
    {
        if (_cargandoDatos) return;

        if (_modo == ModoGestion.ModificarRaza)
        {
            await CargarComboSeleccionRazasAsync();
        }
    }

    private async Task cboSeleccion_SelectedIndexChangedAsync()
    {
        if (_cargandoDatos) return;

        var seleccionado = cboSeleccion.SelectedItem as ItemCombo;
        if (seleccionado is null)
        {
            txtNombre.Clear();
            return;
        }

        txtNombre.Text = seleccionado.Texto;
    }

    private async Task GuardarAsync()
    {
        var nombre = txtNombre.Text.Trim();
        if (string.IsNullOrWhiteSpace(nombre) || nombre.Length < 2)
        {
            txtNombre.BackColor = ColorFondoError;
            MessageBox.Show("El nombre debe tener al menos 2 caracteres.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            txtNombre.Focus();
            return;
        }

        if (_modo == ModoGestion.AltaEspecie)
        {
            if (_especieControlador is null) return;
            var res = await _especieControlador.CrearAsync(new EspecieSolicitudDto { Nombre = nombre });
            if (res.EsExitoso)
            {
                MessageBox.Show(res.Mensaje ?? "Especie registrada correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                DialogResult = DialogResult.OK;
                Close();
            }
            else
            {
                txtNombre.BackColor = ColorFondoError;
                MessageBox.Show(res.Mensaje, "Error al registrar especie", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        else if (_modo == ModoGestion.AltaRaza)
        {
            var espPadre = cboParentEspecie.SelectedItem as ItemCombo;
            if (espPadre?.Id is null || espPadre.Id <= 0)
            {
                cboParentEspecie.BackColor = ColorFondoError;
                MessageBox.Show("Debe seleccionar una especie válida para asociar la raza.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cboParentEspecie.Focus();
                return;
            }

            if (_razaControlador is null) return;
            var res = await _razaControlador.CrearAsync(new RazaSolicitudDto { Nombre = nombre, IdEspecie = espPadre.Id.Value });
            if (res.EsExitoso)
            {
                MessageBox.Show(res.Mensaje ?? "Raza registrada correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                DialogResult = DialogResult.OK;
                Close();
            }
            else
            {
                txtNombre.BackColor = ColorFondoError;
                MessageBox.Show(res.Mensaje, "Error al registrar raza", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
    }

    private async Task ModificarAsync()
    {
        var seleccionado = cboSeleccion.SelectedItem as ItemCombo;
        if (seleccionado?.Id is null || seleccionado.Id <= 0)
        {
            cboSeleccion.BackColor = ColorFondoError;
            MessageBox.Show("Debe seleccionar un registro para modificar.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            cboSeleccion.Focus();
            return;
        }

        var nombre = txtNombre.Text.Trim();
        if (string.IsNullOrWhiteSpace(nombre) || nombre.Length < 2)
        {
            txtNombre.BackColor = ColorFondoError;
            MessageBox.Show("El nombre debe tener al menos 2 caracteres.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            txtNombre.Focus();
            return;
        }

        if (_modo == ModoGestion.ModificarEspecie)
        {
            if (_especieControlador is null) return;
            var res = await _especieControlador.ActualizarAsync(seleccionado.Id.Value, new EspecieSolicitudDto { Nombre = nombre });
            if (res.EsExitoso)
            {
                MessageBox.Show(res.Mensaje ?? "Especie actualizada correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                DialogResult = DialogResult.OK;
                Close();
            }
            else
            {
                txtNombre.BackColor = ColorFondoError;
                MessageBox.Show(res.Mensaje, "Error al actualizar especie", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        else if (_modo == ModoGestion.ModificarRaza)
        {
            var espPadre = cboParentEspecie.SelectedItem as ItemCombo;
            if (espPadre?.Id is null || espPadre.Id <= 0)
            {
                cboParentEspecie.BackColor = ColorFondoError;
                MessageBox.Show("Debe seleccionar una especie asociada.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cboParentEspecie.Focus();
                return;
            }

            if (_razaControlador is null) return;
            var res = await _razaControlador.ActualizarAsync(seleccionado.Id.Value, new RazaSolicitudDto { Nombre = nombre, IdEspecie = espPadre.Id.Value });
            if (res.EsExitoso)
            {
                MessageBox.Show(res.Mensaje ?? "Raza actualizada correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                DialogResult = DialogResult.OK;
                Close();
            }
            else
            {
                txtNombre.BackColor = ColorFondoError;
                MessageBox.Show(res.Mensaje, "Error al actualizar raza", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
    }

    private async Task DarDeBajaAsync()
    {
        var seleccionado = cboSeleccion.SelectedItem as ItemCombo;
        if (seleccionado?.Id is null || seleccionado.Id <= 0)
        {
            cboSeleccion.BackColor = ColorFondoError;
            MessageBox.Show("Debe seleccionar un registro para dar de baja.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            cboSeleccion.Focus();
            return;
        }

        var tipoTexto = _modo == ModoGestion.ModificarEspecie ? "la especie" : "la raza";
        var confirmacion = MessageBox.Show(
            $"¿Está seguro de que desea dar de baja {tipoTexto} '{seleccionado.Texto}'?",
            "Confirmar Baja Lógica",
            MessageBoxButtons.YesNo,
            MessageBoxIcon.Question);

        if (confirmacion != DialogResult.Yes)
            return;

        if (_modo == ModoGestion.ModificarEspecie)
        {
            if (_especieControlador is null) return;
            var res = await _especieControlador.EliminarAsync(seleccionado.Id.Value);
            if (res.EsExitoso)
            {
                MessageBox.Show(res.Mensaje ?? "Especie dada de baja exitosamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                DialogResult = DialogResult.OK;
                Close();
            }
            else
            {
                MessageBox.Show(res.Mensaje, "Error al dar de baja", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        else if (_modo == ModoGestion.ModificarRaza)
        {
            if (_razaControlador is null) return;
            var res = await _razaControlador.EliminarAsync(seleccionado.Id.Value);
            if (res.EsExitoso)
            {
                MessageBox.Show(res.Mensaje ?? "Raza dada de baja exitosamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                DialogResult = DialogResult.OK;
                Close();
            }
            else
            {
                MessageBox.Show(res.Mensaje, "Error al dar de baja", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
    }

    private static void SeleccionarPorId(ComboBox cbo, long id)
    {
        for (int i = 0; i < cbo.Items.Count; i++)
        {
            if (cbo.Items[i] is ItemCombo item && item.Id == id)
            {
                cbo.SelectedIndex = i;
                return;
            }
        }
    }

    /// <summary>
    /// Elemento auxiliar para vincular identificador y texto en los desplegables.
    /// </summary>
    public sealed record ItemCombo(long? Id, string Texto)
    {
        public override string ToString() => Texto;
    }
}
