using Veterinaria.WinForms.Session;

namespace Veterinaria.WinForms.Views.Veterinario;

/// <summary>
/// Formulario principal de shell para el rol Veterinario (Atención clínica).
/// </summary>
public partial class FormVeterinarioPrincipal : Form
{
    private readonly IServiceProvider _serviceProvider;

    public FormVeterinarioPrincipal(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
        InitializeComponent();
        CargarImagenesModulos();
    }

    private void FormVeterinarioPrincipal_Load(object? sender, EventArgs e)
    {
        lblUsuarioSesion.Text = SesionActual.EstaAutenticado
            ? $"Dr./Dra. {SesionActual.NombreCompleto} | {SesionActual.Rol}"
            : "Médico: Veterinario";

        lblStatusInfo.Text = $"Sesión clínica activa: {SesionActual.Username} - {DateTime.Now:dd/MM/yyyy}";
    }

    private void button1_Click(object sender, EventArgs e)
    {

    }

    private void BTCONSULTAS_Click(object sender, EventArgs e)
    {
        using var vista = new FormConsultas();
        vista.ShowDialog(this);
    }

    private void BTFICHAMEDICA_Click(object sender, EventArgs e)
    {
        using var vista = new FormFichaMedica();
        vista.ShowDialog(this);
    }

    private void BTTRATAMIENTOS_Click(object sender, EventArgs e)
    {
        using var vista = new FormTratamientos();
        vista.ShowDialog(this);
    }

    private void BTVACUNAS_Click(object sender, EventArgs e)
    {
        using var vista = new FormVacunasControles();
        vista.ShowDialog(this);
    }

    private void CargarImagenesModulos()
    {
        BTCONSULTAS.Image = CargarImagenModulo("vet-consultas.png");
        BTFICHAMEDICA.Image = CargarImagenModulo("vet-ficha.png");
        BTTRATAMIENTOS.Image = CargarImagenModulo("vet-tratamientos.png");
        BTVACUNAS.Image = CargarImagenModulo("vet-vacunas.png");
    }

    private static Image? CargarImagenModulo(string archivo)
    {
        var ruta = Path.Combine(AppContext.BaseDirectory, "Resources", archivo);
        if (!File.Exists(ruta))
        {
            return null;
        }

        using var original = Image.FromFile(ruta);
        return new Bitmap(original, new Size(186, 186));
    }
}
