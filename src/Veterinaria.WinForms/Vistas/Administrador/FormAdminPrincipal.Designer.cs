namespace Veterinaria.WinForms.Vistas.Administrador;

partial class FormAdminPrincipal
{
    private System.ComponentModel.IContainer components = null;
    private System.Windows.Forms.Panel pnlEncabezado;
    private System.Windows.Forms.Label lblTitulo;
    private System.Windows.Forms.Label lblUsuarioSesion;
    private System.Windows.Forms.StatusStrip barraEstado;
    private System.Windows.Forms.ToolStripStatusLabel lblInfoEstado;

    protected override void Dispose(bool disposing)
    {
        if (disposing && (components != null))
        {
            components.Dispose();
        }
        base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
        pnlEncabezado = new Panel();
        lblTitulo = new Label();
        lblUsuarioSesion = new Label();
        barraEstado = new StatusStrip();
        lblInfoEstado = new ToolStripStatusLabel();
        BTUSUARIOS = new Button();
        BTPROPIETARIOS = new Button();
        BTMASCOTAS = new Button();
        BTREPORTES = new Button();
        pnlContenido = new Panel();
        pnlEncabezado.SuspendLayout();
        barraEstado.SuspendLayout();
        pnlContenido.SuspendLayout();
        SuspendLayout();
        // 
        // pnlEncabezado
        // 
        pnlEncabezado.BackColor = Color.FromArgb(200, 138, 150);
        pnlEncabezado.Controls.Add(lblTitulo);
        pnlEncabezado.Controls.Add(lblUsuarioSesion);
        pnlEncabezado.Dock = DockStyle.Top;
        pnlEncabezado.Location = new Point(0, 0);
        pnlEncabezado.Name = "pnlEncabezado";
        pnlEncabezado.Padding = new Padding(16, 0, 16, 0);
        pnlEncabezado.Size = new Size(1100, 50);
        pnlEncabezado.TabIndex = 0;
        // 
        // lblTitulo
        // 
        lblTitulo.Dock = DockStyle.Left;
        lblTitulo.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
        lblTitulo.ForeColor = Color.White;
        lblTitulo.Location = new Point(16, 0);
        lblTitulo.Name = "lblTitulo";
        lblTitulo.Size = new Size(350, 50);
        lblTitulo.TabIndex = 0;
        lblTitulo.Text = "CLÍNICA VETERINARIA — ADMINISTRADOR";
        lblTitulo.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // lblUsuarioSesion
        // 
        lblUsuarioSesion.Dock = DockStyle.Right;
        lblUsuarioSesion.Font = new Font("Segoe UI", 9.75F);
        lblUsuarioSesion.ForeColor = Color.FromArgb(250, 244, 244);
        lblUsuarioSesion.Location = new Point(684, 0);
        lblUsuarioSesion.Name = "lblUsuarioSesion";
        lblUsuarioSesion.Size = new Size(400, 50);
        lblUsuarioSesion.TabIndex = 1;
        lblUsuarioSesion.Text = "Usuario: Admin";
        lblUsuarioSesion.TextAlign = ContentAlignment.MiddleRight;
        // 
        // barraEstado
        // 
        barraEstado.BackColor = Color.FromArgb(249, 240, 242);
        barraEstado.Items.AddRange(new ToolStripItem[] { lblInfoEstado });
        barraEstado.Location = new Point(0, 678);
        barraEstado.Name = "barraEstado";
        barraEstado.Size = new Size(1100, 22);
        barraEstado.TabIndex = 2;
        // 
        // lblInfoEstado
        // 
        lblInfoEstado.Font = new Font("Segoe UI", 8.25F);
        lblInfoEstado.ForeColor = Color.FromArgb(58, 53, 59);
        lblInfoEstado.Name = "lblInfoEstado";
        lblInfoEstado.Size = new Size(134, 17);
        lblInfoEstado.Text = "Sistema listo para operar";
        // 
        // BTUSUARIOS
        // 
        BTUSUARIOS.Font = new Font("Segoe UI", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
        BTUSUARIOS.ImageAlign = ContentAlignment.TopCenter;
        BTUSUARIOS.Location = new Point(64, 143);
        BTUSUARIOS.Name = "BTUSUARIOS";
        BTUSUARIOS.Padding = new Padding(8, 8, 8, 12);
        BTUSUARIOS.Size = new Size(214, 277);
        BTUSUARIOS.TabIndex = 0;
        BTUSUARIOS.Text = "USUARIOS Y PERFILES";
        BTUSUARIOS.TextAlign = ContentAlignment.BottomCenter;
        BTUSUARIOS.UseVisualStyleBackColor = true;
        BTUSUARIOS.Click += BTUSUARIOS_Click;
        // 
        // BTPROPIETARIOS
        // 
        BTPROPIETARIOS.Font = new Font("Segoe UI", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
        BTPROPIETARIOS.ImageAlign = ContentAlignment.TopCenter;
        BTPROPIETARIOS.Location = new Point(319, 143);
        BTPROPIETARIOS.Name = "BTPROPIETARIOS";
        BTPROPIETARIOS.Padding = new Padding(8, 8, 8, 12);
        BTPROPIETARIOS.Size = new Size(214, 277);
        BTPROPIETARIOS.TabIndex = 0;
        BTPROPIETARIOS.Text = "CONSULTAR PROPIETARIOS";
        BTPROPIETARIOS.TextAlign = ContentAlignment.BottomCenter;
        BTPROPIETARIOS.UseVisualStyleBackColor = true;
        BTPROPIETARIOS.Click += button1_Click;
        // 
        // BTMASCOTAS
        // 
        BTMASCOTAS.Font = new Font("Segoe UI", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
        BTMASCOTAS.ImageAlign = ContentAlignment.TopCenter;
        BTMASCOTAS.Location = new Point(568, 143);
        BTMASCOTAS.Name = "BTMASCOTAS";
        BTMASCOTAS.Padding = new Padding(8, 8, 8, 12);
        BTMASCOTAS.Size = new Size(214, 277);
        BTMASCOTAS.TabIndex = 1;
        BTMASCOTAS.Text = "CONSULTAR MASCOTAS";
        BTMASCOTAS.TextAlign = ContentAlignment.BottomCenter;
        BTMASCOTAS.UseVisualStyleBackColor = true;
        BTMASCOTAS.Click += BTMASCOTAS_Click;
        // 
        // BTREPORTES
        // 
        BTREPORTES.Font = new Font("Segoe UI", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
        BTREPORTES.ImageAlign = ContentAlignment.TopCenter;
        BTREPORTES.Location = new Point(811, 143);
        BTREPORTES.Name = "BTREPORTES";
        BTREPORTES.Padding = new Padding(8, 8, 8, 12);
        BTREPORTES.Size = new Size(214, 277);
        BTREPORTES.TabIndex = 2;
        BTREPORTES.Text = "REPORTES";
        BTREPORTES.TextAlign = ContentAlignment.BottomCenter;
        BTREPORTES.UseVisualStyleBackColor = true;
        BTREPORTES.Click += BTREPORTES_Click;
        // 
        // pnlContenido
        // 
        pnlContenido.BackColor = Color.FromArgb(250, 244, 244);
        pnlContenido.Controls.Add(BTREPORTES);
        pnlContenido.Controls.Add(BTMASCOTAS);
        pnlContenido.Controls.Add(BTPROPIETARIOS);
        pnlContenido.Controls.Add(BTUSUARIOS);
        pnlContenido.Dock = DockStyle.Fill;
        pnlContenido.Location = new Point(0, 50);
        pnlContenido.Name = "pnlContenido";
        pnlContenido.Size = new Size(1100, 628);
        pnlContenido.TabIndex = 1;
        pnlContenido.Paint += pnlContenido_Paint;
        // 
        // FormAdminPrincipal
        // 
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        BackColor = Color.FromArgb(250, 244, 244);
        ClientSize = new Size(1100, 700);
        Controls.Add(pnlContenido);
        Controls.Add(barraEstado);
        Controls.Add(pnlEncabezado);
        Font = new Font("Segoe UI", 9F);
        ForeColor = Color.FromArgb(58, 53, 59);
        MinimumSize = new Size(800, 500);
        Name = "FormAdminPrincipal";
        StartPosition = FormStartPosition.CenterScreen;
        Text = "Veterinaria - Panel Administrador";
        Load += FormAdminPrincipal_Load;
        pnlEncabezado.ResumeLayout(false);
        barraEstado.ResumeLayout(false);
        barraEstado.PerformLayout();
        pnlContenido.ResumeLayout(false);
        ResumeLayout(false);
        PerformLayout();
    }

    private Button BTUSUARIOS;
    private Button BTPROPIETARIOS;
    private Button BTMASCOTAS;
    private Button BTREPORTES;
    private Panel pnlContenido;
}
