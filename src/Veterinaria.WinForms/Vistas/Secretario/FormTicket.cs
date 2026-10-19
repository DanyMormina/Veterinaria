using System.Drawing.Printing;

namespace Veterinaria.WinForms.Vistas.Secretario;

/// <summary>
/// Muestra el ticket emitido al registrar un cobro y permite imprimirlo.
/// </summary>
public class FormTicket : Form
{
    private readonly string _ticket;
    private readonly Font _fuenteTicket = new("Consolas", 9F);
    private string[] _lineasPendientes = [];

    public FormTicket(string ticket)
    {
        _ticket = ticket;

        Text = "Ticket de cobro";
        StartPosition = FormStartPosition.CenterParent;
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;
        MinimizeBox = false;
        ClientSize = new Size(480, 600);
        BackColor = Color.FromArgb(250, 244, 244);
        Font = new Font("Segoe UI", 9F);

        var txtTicket = new TextBox
        {
            Multiline = true,
            ReadOnly = true,
            ScrollBars = ScrollBars.Vertical,
            Font = _fuenteTicket,
            BackColor = Color.White,
            ForeColor = Color.FromArgb(58, 53, 59),
            Location = new Point(12, 12),
            Size = new Size(456, 530),
            Text = ticket.Replace("\r\n", "\n").Replace("\n", Environment.NewLine)
        };

        var btnImprimir = new Button
        {
            Text = "Imprimir",
            BackColor = Color.FromArgb(152, 196, 164),
            FlatStyle = FlatStyle.Flat,
            ForeColor = Color.Black,
            Location = new Point(260, 554),
            Size = new Size(100, 32),
            Cursor = Cursors.Hand
        };
        btnImprimir.FlatAppearance.BorderColor = Color.FromArgb(112, 158, 124);
        btnImprimir.Click += (_, _) => Imprimir();

        var btnCerrar = new Button
        {
            Text = "Cerrar",
            BackColor = Color.MistyRose,
            FlatStyle = FlatStyle.Flat,
            ForeColor = Color.Black,
            Location = new Point(368, 554),
            Size = new Size(100, 32),
            Cursor = Cursors.Hand,
            DialogResult = DialogResult.OK
        };
        btnCerrar.FlatAppearance.BorderColor = Color.FromArgb(184, 93, 105);

        Controls.Add(txtTicket);
        Controls.Add(btnImprimir);
        Controls.Add(btnCerrar);
        AcceptButton = btnCerrar;
        CancelButton = btnCerrar;

        Shown += (_, _) => { txtTicket.SelectionStart = 0; btnImprimir.Focus(); };
    }

    private void Imprimir()
    {
        using var documento = new PrintDocument { DocumentName = "Ticket de cobro" };
        documento.BeginPrint += (_, _) => _lineasPendientes = _ticket.Replace("\r\n", "\n").Split('\n');
        documento.PrintPage += ImprimirPagina;

        using var dialogo = new PrintDialog { Document = documento, UseEXDialog = true };
        if (dialogo.ShowDialog(this) != DialogResult.OK)
            return;

        try
        {
            documento.Print();
        }
        catch (Exception ex)
        {
            MessageBox.Show($"No se pudo imprimir el ticket: {ex.Message}", "Error de impresión",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
    }

    private void ImprimirPagina(object? sender, PrintPageEventArgs e)
    {
        if (e.Graphics is null)
            return;

        var alto = _fuenteTicket.GetHeight(e.Graphics);
        var y = (float)e.MarginBounds.Top;
        var indice = 0;

        while (indice < _lineasPendientes.Length && y + alto <= e.MarginBounds.Bottom)
        {
            e.Graphics.DrawString(_lineasPendientes[indice], _fuenteTicket, Brushes.Black, e.MarginBounds.Left, y);
            y += alto;
            indice++;
        }

        _lineasPendientes = _lineasPendientes[indice..];
        e.HasMorePages = _lineasPendientes.Length > 0;
    }

    protected override void Dispose(bool disposing)
    {
        if (disposing)
            _fuenteTicket.Dispose();
        base.Dispose(disposing);
    }
}
