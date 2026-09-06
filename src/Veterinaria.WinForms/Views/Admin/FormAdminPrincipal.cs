using Veterinaria.WinForms.Session;

namespace Veterinaria.WinForms.Views.Admin;

/// <summary>
/// Formulario principal de shell para el rol Administrador.
/// </summary>
public partial class FormAdminPrincipal : Form
{
    private readonly IServiceProvider _serviceProvider;

    public FormAdminPrincipal(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
        InitializeComponent();
        RestaurarImagenesCompactas();
    }

    private void RestaurarImagenesCompactas()
    {
        BTUSUARIOS.Image = CompactarImagenTarjeta(BTUSUARIOS.Image);
        BTPROPIETARIOS.Image = CompactarImagenTarjeta(BTPROPIETARIOS.Image);
        BTMASCOTAS.Image = CompactarImagenTarjeta(BTMASCOTAS.Image);
        BTREPORTES.Image = CompactarImagenTarjeta(BTREPORTES.Image);
    }

    private static Image? CompactarImagenTarjeta(Image? original)
    {
        if (original is null)
        {
            return null;
        }

        const int altoMaximo = 186;
        var alto = Math.Min(altoMaximo, original.Height);
        var ancho = Math.Max(1, (int)Math.Round(original.Width * (alto / (double)original.Height)));
        var compacta = new Bitmap(original, new Size(ancho, alto));
        original.Dispose();
        return compacta;
    }


    private void FormAdminPrincipal_Load(object? sender, EventArgs e)
    {
        lblUsuarioSesion.Text = SesionActual.EstaAutenticado
            ? $"Usuario: {SesionActual.NombreCompleto} | Rol: {SesionActual.Rol}"
            : "Usuario: Administrador";

        lblStatusInfo.Text = $"Conectado como {SesionActual.Username} ({SesionActual.Rol}) - {DateTime.Now:dd/MM/yyyy}";
    }

    private void button1_Click(object sender, EventArgs e)
    {
        FormPropietarios vistaPropietarios = new FormPropietarios();
        vistaPropietarios.ShowDialog();
    }

    private void BTUSUARIOS_Click(object sender, EventArgs e)
    {
        // Como ambos formularios están en la carpeta Admin, Visual Studio los conecta directamente
        FormUsuarios vistaUsuarios = new FormUsuarios();

        vistaUsuarios.ShowDialog();
    }

    private void BTMASCOTAS_Click(object sender, EventArgs e)
    {
        FormMascotas vistaMascotas = new FormMascotas();
        vistaMascotas.ShowDialog();
    }

    private void BTREPORTES_Click(object sender, EventArgs e)
    {
        FormReportes vistaReportes = new FormReportes();
        vistaReportes.ShowDialog();
    }
}
