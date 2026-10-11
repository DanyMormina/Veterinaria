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
        System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormAdminPrincipal));
        pnlEncabezado = new Panel();
        lblTitulo = new Label();
        lblUsuarioSesion = new Label();
        btnCerrarSesion = new Button();
        barraEstado = new StatusStrip();
        lblInfoEstado = new ToolStripStatusLabel();
        BTUSUARIOS = new Button();
        picUsuarios = new PictureBox();
        BTPROPIETARIOS = new Button();
        picPropietarios = new PictureBox();
        BTMASCOTAS = new Button();
        picMascotas = new PictureBox();
        BTREPORTES = new Button();
        picReportes = new PictureBox();
        pnlContenido = new Panel();
        BTBACKUP = new Button();
        pictureBox1 = new PictureBox();
        pnlEncabezado.SuspendLayout();
        barraEstado.SuspendLayout();
        BTUSUARIOS.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)picUsuarios).BeginInit();
        BTPROPIETARIOS.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)picPropietarios).BeginInit();
        BTMASCOTAS.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)picMascotas).BeginInit();
        BTREPORTES.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)picReportes).BeginInit();
        pnlContenido.SuspendLayout();
        BTBACKUP.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
        SuspendLayout();
        // 
        // pnlEncabezado
        // 
        pnlEncabezado.BackColor = Color.FromArgb(200, 138, 150);
        pnlEncabezado.Controls.Add(lblTitulo);
        pnlEncabezado.Controls.Add(lblUsuarioSesion);
        pnlEncabezado.Controls.Add(btnCerrarSesion);
        pnlEncabezado.Dock = DockStyle.Top;
        pnlEncabezado.Location = new Point(0, 0);
        pnlEncabezado.Name = "pnlEncabezado";
        pnlEncabezado.Padding = new Padding(16, 0, 16, 0);
        pnlEncabezado.Size = new Size(1220, 50);
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
        lblUsuarioSesion.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        lblUsuarioSesion.Font = new Font("Segoe UI", 9.75F);
        lblUsuarioSesion.ForeColor = Color.FromArgb(250, 244, 244);
        lblUsuarioSesion.Location = new Point(640, 0);
        lblUsuarioSesion.Name = "lblUsuarioSesion";
        lblUsuarioSesion.Size = new Size(420, 50);
        lblUsuarioSesion.TabIndex = 1;
        lblUsuarioSesion.Text = "Usuario: Admin";
        lblUsuarioSesion.TextAlign = ContentAlignment.MiddleRight;
        // 
        // btnCerrarSesion
        // 
        btnCerrarSesion.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        btnCerrarSesion.BackColor = Color.FromArgb(180, 110, 120);
        btnCerrarSesion.Cursor = Cursors.Hand;
        btnCerrarSesion.FlatAppearance.BorderColor = Color.FromArgb(160, 90, 100);
        btnCerrarSesion.FlatStyle = FlatStyle.Flat;
        btnCerrarSesion.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
        btnCerrarSesion.ForeColor = Color.White;
        btnCerrarSesion.Location = new Point(1072, 10);
        btnCerrarSesion.Name = "btnCerrarSesion";
        btnCerrarSesion.Size = new Size(124, 30);
        btnCerrarSesion.TabIndex = 2;
        btnCerrarSesion.Text = "Cerrar sesión";
        btnCerrarSesion.UseVisualStyleBackColor = false;
        btnCerrarSesion.Click += btnCerrarSesion_Click;
        // 
        // barraEstado
        // 
        barraEstado.BackColor = Color.FromArgb(249, 240, 242);
        barraEstado.Items.AddRange(new ToolStripItem[] { lblInfoEstado });
        barraEstado.Location = new Point(0, 698);
        barraEstado.Name = "barraEstado";
        barraEstado.Size = new Size(1220, 22);
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
        BTUSUARIOS.Controls.Add(picUsuarios);
        BTUSUARIOS.Font = new Font("Segoe UI", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
        BTUSUARIOS.ImageAlign = ContentAlignment.TopCenter;
        BTUSUARIOS.Location = new Point(25, 158);
        BTUSUARIOS.Name = "BTUSUARIOS";
        BTUSUARIOS.Padding = new Padding(8, 8, 8, 12);
        BTUSUARIOS.Size = new Size(230, 290);
        BTUSUARIOS.TabIndex = 0;
        BTUSUARIOS.Text = "USUARIOS Y PERFILES";
        BTUSUARIOS.TextAlign = ContentAlignment.BottomCenter;
        BTUSUARIOS.UseVisualStyleBackColor = true;
        BTUSUARIOS.Click += BTUSUARIOS_Click;
        // 
        // picUsuarios
        // 
        picUsuarios.BackColor = Color.Transparent;
        picUsuarios.Image = (Image)resources.GetObject("picUsuarios.Image");
        picUsuarios.Location = new Point(40, 12);
        picUsuarios.Name = "picUsuarios";
        picUsuarios.Size = new Size(150, 190);
        picUsuarios.SizeMode = PictureBoxSizeMode.Zoom;
        picUsuarios.TabIndex = 0;
        picUsuarios.TabStop = false;
        picUsuarios.Click += BTUSUARIOS_Click;
        // 
        // BTPROPIETARIOS
        // 
        BTPROPIETARIOS.Controls.Add(picPropietarios);
        BTPROPIETARIOS.Font = new Font("Segoe UI", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
        BTPROPIETARIOS.ImageAlign = ContentAlignment.TopCenter;
        BTPROPIETARIOS.Location = new Point(261, 158);
        BTPROPIETARIOS.Name = "BTPROPIETARIOS";
        BTPROPIETARIOS.Padding = new Padding(8, 8, 8, 12);
        BTPROPIETARIOS.Size = new Size(230, 290);
        BTPROPIETARIOS.TabIndex = 0;
        BTPROPIETARIOS.Text = "CONSULTAR PROPIETARIOS";
        BTPROPIETARIOS.TextAlign = ContentAlignment.BottomCenter;
        BTPROPIETARIOS.UseVisualStyleBackColor = true;
        BTPROPIETARIOS.Click += button1_Click;
        // 
        // picPropietarios
        // 
        picPropietarios.BackColor = Color.Transparent;
        picPropietarios.Image = (Image)resources.GetObject("picPropietarios.Image");
        picPropietarios.Location = new Point(40, 12);
        picPropietarios.Name = "picPropietarios";
        picPropietarios.Size = new Size(150, 190);
        picPropietarios.SizeMode = PictureBoxSizeMode.Zoom;
        picPropietarios.TabIndex = 0;
        picPropietarios.TabStop = false;
        picPropietarios.Click += button1_Click;
        // 
        // BTMASCOTAS
        // 
        BTMASCOTAS.Controls.Add(picMascotas);
        BTMASCOTAS.Font = new Font("Segoe UI", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
        BTMASCOTAS.ImageAlign = ContentAlignment.TopCenter;
        BTMASCOTAS.Location = new Point(497, 158);
        BTMASCOTAS.Name = "BTMASCOTAS";
        BTMASCOTAS.Padding = new Padding(8, 8, 8, 12);
        BTMASCOTAS.Size = new Size(230, 290);
        BTMASCOTAS.TabIndex = 1;
        BTMASCOTAS.Text = "CONSULTAR MASCOTAS";
        BTMASCOTAS.TextAlign = ContentAlignment.BottomCenter;
        BTMASCOTAS.UseVisualStyleBackColor = true;
        BTMASCOTAS.Click += BTMASCOTAS_Click;
        // 
        // picMascotas
        // 
        picMascotas.BackColor = Color.Transparent;
        picMascotas.Image = (Image)resources.GetObject("picMascotas.Image");
        picMascotas.Location = new Point(40, 12);
        picMascotas.Name = "picMascotas";
        picMascotas.Size = new Size(150, 190);
        picMascotas.SizeMode = PictureBoxSizeMode.Zoom;
        picMascotas.TabIndex = 0;
        picMascotas.TabStop = false;
        picMascotas.Click += BTMASCOTAS_Click;
        // 
        // BTREPORTES
        // 
        BTREPORTES.Controls.Add(picReportes);
        BTREPORTES.Font = new Font("Segoe UI", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
        BTREPORTES.ImageAlign = ContentAlignment.TopCenter;
        BTREPORTES.Location = new Point(733, 158);
        BTREPORTES.Name = "BTREPORTES";
        BTREPORTES.Padding = new Padding(8, 8, 8, 12);
        BTREPORTES.Size = new Size(230, 290);
        BTREPORTES.TabIndex = 2;
        BTREPORTES.Text = "REPORTES";
        BTREPORTES.TextAlign = ContentAlignment.BottomCenter;
        BTREPORTES.UseVisualStyleBackColor = true;
        BTREPORTES.Click += BTREPORTES_Click;
        // 
        // picReportes
        // 
        picReportes.BackColor = Color.Transparent;
        picReportes.Image = (Image)resources.GetObject("picReportes.Image");
        picReportes.Location = new Point(40, 12);
        picReportes.Name = "picReportes";
        picReportes.Size = new Size(150, 190);
        picReportes.SizeMode = PictureBoxSizeMode.Zoom;
        picReportes.TabIndex = 0;
        picReportes.TabStop = false;
        picReportes.Click += BTREPORTES_Click;
        // 
        // pnlContenido
        // 
        pnlContenido.BackColor = Color.FromArgb(250, 244, 244);
        pnlContenido.Controls.Add(BTBACKUP);
        pnlContenido.Controls.Add(BTREPORTES);
        pnlContenido.Controls.Add(BTMASCOTAS);
        pnlContenido.Controls.Add(BTPROPIETARIOS);
        pnlContenido.Controls.Add(BTUSUARIOS);
        pnlContenido.Dock = DockStyle.Fill;
        pnlContenido.Location = new Point(0, 50);
        pnlContenido.Name = "pnlContenido";
        pnlContenido.Size = new Size(1220, 648);
        pnlContenido.TabIndex = 1;
        pnlContenido.Paint += pnlContenido_Paint;
        // 
        // BTBACKUP
        // 
        BTBACKUP.Controls.Add(pictureBox1);
        BTBACKUP.Font = new Font("Segoe UI", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
        BTBACKUP.ImageAlign = ContentAlignment.TopCenter;
        BTBACKUP.Location = new Point(969, 158);
        BTBACKUP.Name = "BTBACKUP";
        BTBACKUP.Padding = new Padding(8, 8, 8, 12);
        BTBACKUP.Size = new Size(230, 290);
        BTBACKUP.TabIndex = 3;
        BTBACKUP.Text = "BACKUP";
        BTBACKUP.TextAlign = ContentAlignment.BottomCenter;
        BTBACKUP.UseVisualStyleBackColor = true;
        BTBACKUP.Click += BTBACKUP_Click;
        // 
        // pictureBox1
        // 
        pictureBox1.BackColor = Color.Transparent;
        pictureBox1.Image = (Image)resources.GetObject("pictureBox1.Image");
        pictureBox1.Location = new Point(40, 12);
        pictureBox1.Name = "pictureBox1";
        pictureBox1.Size = new Size(150, 190);
        pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
        pictureBox1.TabIndex = 0;
        pictureBox1.TabStop = false;
        pictureBox1.Click += BTBACKUP_Click;
        // 
        // FormAdminPrincipal
        // 
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        BackColor = Color.FromArgb(250, 244, 244);
        ClientSize = new Size(1220, 720);
        Controls.Add(pnlContenido);
        Controls.Add(barraEstado);
        Controls.Add(pnlEncabezado);
        Font = new Font("Segoe UI", 9F);
        ForeColor = Color.FromArgb(58, 53, 59);
        MinimumSize = new Size(1220, 720);
        Name = "FormAdminPrincipal";
        StartPosition = FormStartPosition.CenterScreen;
        Text = "Veterinaria - Panel Administrador";
        Load += FormAdminPrincipal_Load;
        pnlEncabezado.ResumeLayout(false);
        barraEstado.ResumeLayout(false);
        barraEstado.PerformLayout();
        BTUSUARIOS.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)picUsuarios).EndInit();
        BTPROPIETARIOS.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)picPropietarios).EndInit();
        BTMASCOTAS.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)picMascotas).EndInit();
        BTREPORTES.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)picReportes).EndInit();
        pnlContenido.ResumeLayout(false);
        BTBACKUP.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
        ResumeLayout(false);
        PerformLayout();
    }

    private Button BTUSUARIOS;
    private Button BTPROPIETARIOS;
    private Button BTMASCOTAS;
    private Button BTREPORTES;
    private Button btnCerrarSesion;
    private PictureBox picReportes;
    private PictureBox picPropietarios;
    private PictureBox picUsuarios;
    private PictureBox picMascotas;
    private Panel pnlContenido;
    private Button BTBACKUP;
    private PictureBox pictureBox1;
}
