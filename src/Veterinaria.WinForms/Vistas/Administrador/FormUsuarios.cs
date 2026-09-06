using Veterinaria.WinForms.Sesion;

namespace Veterinaria.WinForms.Vistas.Administrador
{
    public partial class FormUsuarios : Form
    {
        public FormUsuarios()
        {
            InitializeComponent();
        }

        private void FormUsuarios_Load(object? sender, EventArgs e)
        {
            lblUsuarioSesion.Text = SesionActual.EstaAutenticado
                ? $"Usuario: {SesionActual.NombreCompleto} | Rol: {SesionActual.Rol}"
                : "Usuario: Administrador";
        }

        private void btnVolver_Click(object? sender, EventArgs e)
        {
            Close();
        }
    }
}
