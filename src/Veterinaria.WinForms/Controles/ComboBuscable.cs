namespace Veterinaria.WinForms.Controles;

/// <summary>
/// Permite escribir en un ComboBox para buscar dentro de la lista (autocompletado con sugerencias),
/// sin aceptar valores que no estén en ella.
/// </summary>
public static class ComboBuscable
{
    /// <param name="permitirVacio">
    /// Si es false, al dejar el texto vacío se vuelve a la última opción elegida (útil en filtros con "(Todos)").
    /// </param>
    public static void Habilitar(bool permitirVacio, params ComboBox[] combos)
    {
        foreach (var combo in combos)
        {
            combo.DropDownStyle = ComboBoxStyle.DropDown;
            combo.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            combo.AutoCompleteSource = AutoCompleteSource.ListItems;

            var ultimoIndiceValido = -1;
            combo.SelectedIndexChanged += (_, _) =>
            {
                if (combo.SelectedIndex >= 0)
                    ultimoIndiceValido = combo.SelectedIndex;
            };
            combo.DataSourceChanged += (_, _) => ultimoIndiceValido = combo.SelectedIndex;

            combo.Leave += (_, _) => ConfirmarSeleccion(combo, permitirVacio, ultimoIndiceValido);
        }
    }

    public static void Habilitar(params ComboBox[] combos) => Habilitar(true, combos);

    private static void ConfirmarSeleccion(ComboBox combo, bool permitirVacio, int ultimoIndiceValido)
    {
        var texto = combo.Text.Trim();

        if (texto.Length == 0 && permitirVacio)
        {
            if (combo.SelectedIndex != -1)
                combo.SelectedIndex = -1;
            return;
        }

        var indice = texto.Length > 0 ? combo.FindStringExact(texto) : -1;
        if (indice < 0 && ultimoIndiceValido >= 0 && ultimoIndiceValido < combo.Items.Count)
            indice = ultimoIndiceValido;

        if (indice >= 0)
        {
            if (combo.SelectedIndex != indice)
                combo.SelectedIndex = indice;
            combo.Text = combo.GetItemText(combo.Items[indice]);
        }
        else
        {
            combo.SelectedIndex = -1;
            combo.Text = string.Empty;
        }
    }
}
