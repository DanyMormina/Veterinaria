using System.Globalization;
using System.Text;

namespace Veterinaria.WinForms.Controles;

/// <summary>
/// Permite escribir en un ComboBox para buscar dentro de su lista: mientras se escribe se muestra
/// debajo una lista con las opciones que contienen el texto (sin distinguir mayúsculas ni acentos).
/// No usa el autocompletado nativo de WinForms, que se superpone con el desplegable y puede dejar
/// el texto en blanco al elegir con el mouse.
/// </summary>
public static class ComboBuscable
{
    /// <param name="permitirVacio">
    /// Si es false, al dejar el texto vacío se vuelve a la última opción elegida (útil en filtros con "(Todos)").
    /// </param>
    public static void Habilitar(bool permitirVacio, params ComboBox[] combos)
    {
        foreach (var combo in combos)
            _ = new Buscador(combo, permitirVacio, confirmarAlSalir: true);
    }

    public static void Habilitar(params ComboBox[] combos) => Habilitar(true, combos);

    /// <summary>
    /// Solo agrega la lista de sugerencias; para combos cuyo formulario ya resuelve qué hacer al salir.
    /// </summary>
    public static void HabilitarSoloSugerencias(params ComboBox[] combos)
    {
        foreach (var combo in combos)
            _ = new Buscador(combo, permitirVacio: true, confirmarAlSalir: false);
    }

    private sealed class Buscador
    {
        private const int MaximoFilasVisibles = 8;

        private readonly ComboBox _combo;
        private readonly ListBox _lista;
        private readonly bool _permitirVacio;
        private readonly bool _confirmarAlSalir;
        private readonly List<int> _indicesCoincidentes = [];
        private string? _ultimoTextoValido;

        public Buscador(ComboBox combo, bool permitirVacio, bool confirmarAlSalir)
        {
            _combo = combo;
            _permitirVacio = permitirVacio;
            _confirmarAlSalir = confirmarAlSalir;

            _combo.DropDownStyle = ComboBoxStyle.DropDown;
            _combo.AutoCompleteMode = AutoCompleteMode.None;
            _combo.AutoCompleteSource = AutoCompleteSource.None;

            _lista = new ListBox
            {
                Visible = false,
                TabStop = false,
                IntegralHeight = false,
                BorderStyle = BorderStyle.FixedSingle,
                Font = combo.Font
            };

            _combo.TextUpdate += (_, _) => Filtrar();
            _combo.KeyDown += Combo_KeyDown;
            _combo.Leave += (_, _) => Combo_Leave();
            _combo.DropDown += (_, _) => Ocultar();
            _combo.DataSourceChanged += (_, _) =>
            {
                Ocultar();
                _ultimoTextoValido = _combo.SelectedIndex >= 0 ? _combo.Text : null;
            };
            _combo.SelectedIndexChanged += (_, _) =>
            {
                if (_combo.SelectedIndex >= 0)
                    _ultimoTextoValido = _combo.GetItemText(_combo.SelectedItem);
            };
            _combo.Disposed += (_, _) => _lista.Dispose();

            _lista.MouseMove += (_, e) =>
            {
                var indice = _lista.IndexFromPoint(e.Location);
                if (indice >= 0 && indice != _lista.SelectedIndex)
                    _lista.SelectedIndex = indice;
            };
            _lista.MouseDown += (_, e) =>
            {
                var indice = _lista.IndexFromPoint(e.Location);
                if (e.Button == MouseButtons.Left && indice >= 0)
                    Elegir(indice);
                else
                    _combo.Focus();
            };
        }

        private void Filtrar()
        {
            var texto = Normalizar(_combo.Text);
            if (texto.Length == 0)
            {
                Ocultar();
                return;
            }

            if (_combo.DroppedDown)
            {
                // Cerrar el desplegable nativo para que no quede debajo de las sugerencias
                var textoEscrito = _combo.Text;
                var cursor = _combo.SelectionStart;
                _combo.DroppedDown = false;
                _combo.Text = textoEscrito;
                _combo.SelectionStart = Math.Min(cursor, textoEscrito.Length);
                _combo.SelectionLength = 0;
            }

            _indicesCoincidentes.Clear();
            var empiezanIgual = new List<int>();
            var contienen = new List<int>();
            for (var i = 0; i < _combo.Items.Count; i++)
            {
                var item = Normalizar(_combo.GetItemText(_combo.Items[i]));
                if (item.StartsWith(texto, StringComparison.Ordinal))
                    empiezanIgual.Add(i);
                else if (item.Contains(texto, StringComparison.Ordinal))
                    contienen.Add(i);
            }
            _indicesCoincidentes.AddRange(empiezanIgual);
            _indicesCoincidentes.AddRange(contienen);

            if (_indicesCoincidentes.Count == 0)
            {
                Ocultar();
                return;
            }

            _lista.BeginUpdate();
            _lista.Items.Clear();
            foreach (var indice in _indicesCoincidentes)
                _lista.Items.Add(_combo.GetItemText(_combo.Items[indice]));
            _lista.SelectedIndex = 0;
            _lista.EndUpdate();

            Mostrar();
        }

        private void Mostrar()
        {
            var contenedor = _combo.FindForm() ?? _combo.Parent;
            if (contenedor is null || _combo.Parent is null)
                return;

            if (_lista.Parent != contenedor)
                contenedor.Controls.Add(_lista);

            var filas = Math.Min(_lista.Items.Count, MaximoFilasVisibles);
            var alto = filas * _lista.ItemHeight + 4;
            var ancho = Math.Max(_combo.Width, _combo.DropDownWidth);

            var arriba = contenedor.PointToClient(_combo.Parent.PointToScreen(_combo.Location));
            var y = arriba.Y + _combo.Height;
            if (y + alto > contenedor.ClientSize.Height && arriba.Y - alto >= 0)
                y = arriba.Y - alto;

            _lista.Bounds = new Rectangle(arriba.X, y, ancho, alto);
            _lista.Visible = true;
            _lista.BringToFront();
        }

        private void Ocultar()
        {
            if (_lista.Visible)
                _lista.Visible = false;
        }

        private void Elegir(int posicionEnLista)
        {
            if (posicionEnLista < 0 || posicionEnLista >= _indicesCoincidentes.Count)
                return;

            var indice = _indicesCoincidentes[posicionEnLista];
            Ocultar();

            if (_combo.SelectedIndex != indice)
                _combo.SelectedIndex = indice;

            if (indice < _combo.Items.Count)
                _combo.Text = _combo.GetItemText(_combo.Items[indice]);

            if (!_combo.Focused)
                _combo.Focus();
            _combo.SelectionStart = _combo.Text.Length;
            _combo.SelectionLength = 0;
        }

        private void Combo_KeyDown(object? sender, KeyEventArgs e)
        {
            if (!_lista.Visible)
                return;

            switch (e.KeyCode)
            {
                case Keys.Down:
                    _lista.SelectedIndex = Math.Min(_lista.SelectedIndex + 1, _lista.Items.Count - 1);
                    e.Handled = e.SuppressKeyPress = true;
                    break;
                case Keys.Up:
                    _lista.SelectedIndex = Math.Max(_lista.SelectedIndex - 1, 0);
                    e.Handled = e.SuppressKeyPress = true;
                    break;
                case Keys.Enter:
                    Elegir(_lista.SelectedIndex);
                    e.Handled = e.SuppressKeyPress = true;
                    break;
                case Keys.Tab:
                    Elegir(_lista.SelectedIndex);
                    break;
                case Keys.Escape:
                    Ocultar();
                    e.Handled = e.SuppressKeyPress = true;
                    break;
            }
        }

        private void Combo_Leave()
        {
            // Al hacer clic en una sugerencia el foco pasa un instante a la lista; la elección la resuelve MouseDown
            if (_lista.Visible && _lista.ClientRectangle.Contains(_lista.PointToClient(Cursor.Position)))
                return;

            Ocultar();
            if (_confirmarAlSalir)
                ConfirmarSeleccion();
        }

        private void ConfirmarSeleccion()
        {
            var texto = Normalizar(_combo.Text);

            if (texto.Length == 0 && _permitirVacio)
            {
                if (_combo.SelectedIndex != -1)
                    _combo.SelectedIndex = -1;
                return;
            }

            var indice = -1;
            if (texto.Length > 0)
            {
                var coincidencias = new List<int>();
                for (var i = 0; i < _combo.Items.Count; i++)
                {
                    var item = Normalizar(_combo.GetItemText(_combo.Items[i]));
                    if (item == texto)
                    {
                        indice = i;
                        break;
                    }
                    if (item.Contains(texto, StringComparison.Ordinal))
                        coincidencias.Add(i);
                }

                if (indice < 0 && coincidencias.Count == 1)
                    indice = coincidencias[0];
            }

            if (indice < 0 && _ultimoTextoValido is not null)
                indice = _combo.FindStringExact(_ultimoTextoValido);

            if (indice >= 0)
            {
                if (_combo.SelectedIndex != indice)
                    _combo.SelectedIndex = indice;
                _combo.Text = _combo.GetItemText(_combo.Items[indice]);
            }
            else
            {
                if (_combo.SelectedIndex != -1)
                    _combo.SelectedIndex = -1;
                _combo.Text = string.Empty;
            }
        }

        private static string Normalizar(string? texto)
        {
            if (string.IsNullOrWhiteSpace(texto))
                return string.Empty;

            var descompuesto = texto.Trim().Normalize(NormalizationForm.FormD);
            var sb = new StringBuilder(descompuesto.Length);
            foreach (var c in descompuesto)
            {
                if (CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark)
                    sb.Append(c);
            }
            return sb.ToString().Normalize(NormalizationForm.FormC).ToLowerInvariant();
        }
    }
}
