namespace Veterinaria.WinForms.Views.Admin;

partial class FormAdminPrincipal
{
    private System.ComponentModel.IContainer components = null;
    private System.Windows.Forms.Panel pnlHeader;
    private System.Windows.Forms.Label lblTitulo;
    private System.Windows.Forms.Label lblUsuarioSesion;
    private System.Windows.Forms.StatusStrip statusStrip;
    private System.Windows.Forms.ToolStripStatusLabel lblStatusInfo;

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
        System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormAdminPrincipal));
        pnlHeader = new Panel();
        lblTitulo = new Label();
        lblUsuarioSesion = new Label();
        statusStrip = new StatusStrip();
        lblStatusInfo = new ToolStripStatusLabel();
        BTUSUARIOS = new Button();
        BTPROPIETARIOS = new Button();
        BTMASCOTAS = new Button();
        BTREPORTES = new Button();
        pnlContenido = new Panel();
        pnlHeader.SuspendLayout();
        statusStrip.SuspendLayout();
        pnlContenido.SuspendLayout();
        SuspendLayout();
        // 
        // pnlHeader
        // 
        pnlHeader.BackColor = Color.FromArgb(200, 138, 150);
        pnlHeader.Controls.Add(lblTitulo);
        pnlHeader.Controls.Add(lblUsuarioSesion);
        pnlHeader.Dock = DockStyle.Top;
        pnlHeader.Location = new Point(0, 0);
        pnlHeader.Name = "pnlHeader";
        pnlHeader.Padding = new Padding(16, 0, 16, 0);
        pnlHeader.Size = new Size(1100, 50);
        pnlHeader.TabIndex = 0;
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
        // statusStrip
        // 
        statusStrip.BackColor = Color.FromArgb(249, 240, 242);
        statusStrip.Items.AddRange(new ToolStripItem[] { lblStatusInfo });
        statusStrip.Location = new Point(0, 678);
        statusStrip.Name = "statusStrip";
        statusStrip.Size = new Size(1100, 22);
        statusStrip.TabIndex = 2;
        // 
        // lblStatusInfo
        // 
        lblStatusInfo.Font = new Font("Segoe UI", 8.25F);
        lblStatusInfo.ForeColor = Color.FromArgb(58, 53, 59);
        lblStatusInfo.Name = "lblStatusInfo";
        lblStatusInfo.Size = new Size(134, 17);
        lblStatusInfo.Text = "Sistema listo para operar";
        // 
        // BTUSUARIOS
        // 
        BTUSUARIOS.Font = new Font("Segoe UI", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
        BTUSUARIOS.Image = (Image)resources.GetObject("BTUSUARIOS.Image");
        BTUSUARIOS.ImageAlign = ContentAlignment.TopCenter;
        BTUSUARIOS.Location = new Point(64, 143);
        BTUSUARIOS.Name = "BTUSUARIOS";
        BTUSUARIOS.Size = new Size(214, 277);
        BTUSUARIOS.TabIndex = 0;
        BTUSUARIOS.Text = "USUARIOS";
        BTUSUARIOS.TextAlign = ContentAlignment.BottomCenter;
        BTUSUARIOS.UseVisualStyleBackColor = true;
        BTUSUARIOS.Click += BTUSUARIOS_Click;
        // 
        // BTPROPIETARIOS
        // 
        BTPROPIETARIOS.Font = new Font("Segoe UI", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
        BTPROPIETARIOS.Image = (Image)resources.GetObject("BTPROPIETARIOS.Image");
        BTPROPIETARIOS.ImageAlign = ContentAlignment.TopCenter;
        BTPROPIETARIOS.Location = new Point(319, 143);
        BTPROPIETARIOS.Name = "BTPROPIETARIOS";
        BTPROPIETARIOS.Size = new Size(214, 277);
        BTPROPIETARIOS.TabIndex = 0;
        BTPROPIETARIOS.Text = "PROPIETARIOS";
        BTPROPIETARIOS.TextAlign = ContentAlignment.BottomCenter;
        BTPROPIETARIOS.UseVisualStyleBackColor = true;
        BTPROPIETARIOS.Click += button1_Click;
        // 
        // BTMASCOTAS
        // 
        BTMASCOTAS.Font = new Font("Segoe UI", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
        BTMASCOTAS.Image = (Image)resources.GetObject("BTMASCOTAS.Image");
        BTMASCOTAS.ImageAlign = ContentAlignment.TopCenter;
        BTMASCOTAS.Location = new Point(568, 143);
        BTMASCOTAS.Name = "BTMASCOTAS";
        BTMASCOTAS.Size = new Size(214, 277);
        BTMASCOTAS.TabIndex = 1;
        BTMASCOTAS.Text = "MASCOTAS";
        BTMASCOTAS.TextAlign = ContentAlignment.BottomCenter;
        BTMASCOTAS.UseVisualStyleBackColor = true;
        BTMASCOTAS.Click += BTMASCOTAS_Click;
        // 
        // BTREPORTES
        // 
        BTREPORTES.Font = new Font("Segoe UI", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
        BTREPORTES.Image = (Image)resources.GetObject("BTREPORTES.Image");
        BTREPORTES.ImageAlign = ContentAlignment.TopCenter;
        BTREPORTES.Location = new Point(811, 143);
        BTREPORTES.Name = "BTREPORTES";
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
        // 
        // FormAdminPrincipal
        // 
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        BackColor = Color.FromArgb(250, 244, 244);
        ClientSize = new Size(1100, 700);
        Controls.Add(pnlContenido);
        Controls.Add(statusStrip);
        Controls.Add(pnlHeader);
        Font = new Font("Segoe UI", 9F);
        ForeColor = Color.FromArgb(58, 53, 59);
        MinimumSize = new Size(800, 500);
        Name = "FormAdminPrincipal";
        StartPosition = FormStartPosition.CenterScreen;
        Text = "Veterinaria - Panel Administrador";
        Load += FormAdminPrincipal_Load;
        pnlHeader.ResumeLayout(false);
        statusStrip.ResumeLayout(false);
        statusStrip.PerformLayout();
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
