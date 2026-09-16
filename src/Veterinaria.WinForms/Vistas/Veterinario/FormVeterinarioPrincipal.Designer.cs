namespace Veterinaria.WinForms.Vistas.Veterinario;

partial class FormVeterinarioPrincipal
{
    private PictureBox picConsultas;
    private PictureBox picFichaMedica;
    private PictureBox picTratamientos;
    private PictureBox picVacunas;
    private System.ComponentModel.IContainer components = null;
    private System.Windows.Forms.Panel pnlEncabezado;
    private System.Windows.Forms.Label lblTitulo;
    private System.Windows.Forms.Label lblUsuarioSesion;
    private System.Windows.Forms.Panel pnlContenido;
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
        System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormVeterinarioPrincipal));
        pnlEncabezado = new Panel();
        lblTitulo = new Label();
        lblUsuarioSesion = new Label();
        pnlContenido = new Panel();
        BTVACUNAS = new Button();
        picVacunas = new PictureBox();
        BTTRATAMIENTOS = new Button();
        picTratamientos = new PictureBox();
        BTFICHAMEDICA = new Button();
        picFichaMedica = new PictureBox();
        BTCONSULTAS = new Button();
        picConsultas = new PictureBox();
        barraEstado = new StatusStrip();
        lblInfoEstado = new ToolStripStatusLabel();
        pnlEncabezado.SuspendLayout();
        pnlContenido.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)picVacunas).BeginInit();
        ((System.ComponentModel.ISupportInitialize)picTratamientos).BeginInit();
        ((System.ComponentModel.ISupportInitialize)picFichaMedica).BeginInit();
        ((System.ComponentModel.ISupportInitialize)picConsultas).BeginInit();
        barraEstado.SuspendLayout();
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
        lblTitulo.Size = new Size(398, 50);
        lblTitulo.TabIndex = 0;
        lblTitulo.Text = "CLÍNICA VETERINARIA — ATENCIÓN CLÍNICA";
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
        lblUsuarioSesion.Text = "Médico: Veterinario";
        lblUsuarioSesion.TextAlign = ContentAlignment.MiddleRight;
        // 
        // pnlContenido
        // 
        pnlContenido.BackColor = Color.FromArgb(250, 244, 244);
        pnlContenido.Controls.Add(picVacunas);
        pnlContenido.Controls.Add(picTratamientos);
        pnlContenido.Controls.Add(picConsultas);
        pnlContenido.Controls.Add(picFichaMedica);
        pnlContenido.Controls.Add(BTVACUNAS);
        pnlContenido.Controls.Add(BTTRATAMIENTOS);
        pnlContenido.Controls.Add(BTFICHAMEDICA);
        pnlContenido.Controls.Add(BTCONSULTAS);
        pnlContenido.Dock = DockStyle.Fill;
        pnlContenido.Location = new Point(0, 50);
        pnlContenido.Name = "pnlContenido";
        pnlContenido.Size = new Size(1100, 628);
        pnlContenido.TabIndex = 1;
        // 
        // BTVACUNAS
        // 
        BTVACUNAS.BackColor = Color.White;
        BTVACUNAS.FlatAppearance.BorderColor = Color.FromArgb(210, 186, 190);
        BTVACUNAS.FlatStyle = FlatStyle.Flat;
        BTVACUNAS.Font = new Font("Segoe UI", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
        BTVACUNAS.ImageAlign = ContentAlignment.TopCenter;
        BTVACUNAS.Location = new Point(811, 143);
        BTVACUNAS.Name = "BTVACUNAS";
        BTVACUNAS.Padding = new Padding(12, 8, 12, 10);
        BTVACUNAS.Size = new Size(214, 277);
        BTVACUNAS.TabIndex = 3;
        BTVACUNAS.Text = "CONTROLES";
        BTVACUNAS.TextAlign = ContentAlignment.BottomCenter;
        BTVACUNAS.TextImageRelation = TextImageRelation.ImageAboveText;
        BTVACUNAS.UseVisualStyleBackColor = false;
        BTVACUNAS.Click += BTVACUNAS_Click;
        // 
        // picVacunas
        // 
        picVacunas.BackColor = Color.Transparent;
        picVacunas.Image = (Image)resources.GetObject("picVacunas.Image");
        picVacunas.Location = new Point(854, 162);
        picVacunas.Name = "picVacunas";
        picVacunas.Size = new Size(125, 186);
        picVacunas.SizeMode = PictureBoxSizeMode.Zoom;
        picVacunas.TabIndex = 0;
        picVacunas.TabStop = false;
        picVacunas.Click += BTVACUNAS_Click;
        // 
        // BTTRATAMIENTOS
        // 
        BTTRATAMIENTOS.BackColor = Color.White;
        BTTRATAMIENTOS.FlatAppearance.BorderColor = Color.FromArgb(210, 186, 190);
        BTTRATAMIENTOS.FlatStyle = FlatStyle.Flat;
        BTTRATAMIENTOS.Font = new Font("Segoe UI", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
        BTTRATAMIENTOS.ImageAlign = ContentAlignment.TopCenter;
        BTTRATAMIENTOS.Location = new Point(568, 143);
        BTTRATAMIENTOS.Name = "BTTRATAMIENTOS";
        BTTRATAMIENTOS.Padding = new Padding(12, 8, 12, 10);
        BTTRATAMIENTOS.Size = new Size(214, 277);
        BTTRATAMIENTOS.TabIndex = 2;
        BTTRATAMIENTOS.Text = "TRATAMIENTOS";
        BTTRATAMIENTOS.TextAlign = ContentAlignment.BottomCenter;
        BTTRATAMIENTOS.TextImageRelation = TextImageRelation.ImageAboveText;
        BTTRATAMIENTOS.UseVisualStyleBackColor = false;
        BTTRATAMIENTOS.Click += BTTRATAMIENTOS_Click;
        // 
        // picTratamientos
        // 
        picTratamientos.BackColor = Color.Transparent;
        picTratamientos.Image = (Image)resources.GetObject("picTratamientos.Image");
        picTratamientos.Location = new Point(615, 162);
        picTratamientos.Name = "picTratamientos";
        picTratamientos.Size = new Size(128, 186);
        picTratamientos.SizeMode = PictureBoxSizeMode.Zoom;
        picTratamientos.TabIndex = 0;
        picTratamientos.TabStop = false;
        picTratamientos.Click += BTTRATAMIENTOS_Click;
        // 
        // BTFICHAMEDICA
        // 
        BTFICHAMEDICA.BackColor = Color.White;
        BTFICHAMEDICA.FlatAppearance.BorderColor = Color.FromArgb(210, 186, 190);
        BTFICHAMEDICA.FlatStyle = FlatStyle.Flat;
        BTFICHAMEDICA.Font = new Font("Segoe UI", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
        BTFICHAMEDICA.ImageAlign = ContentAlignment.TopCenter;
        BTFICHAMEDICA.Location = new Point(319, 143);
        BTFICHAMEDICA.Name = "BTFICHAMEDICA";
        BTFICHAMEDICA.Padding = new Padding(12, 8, 12, 10);
        BTFICHAMEDICA.Size = new Size(214, 277);
        BTFICHAMEDICA.TabIndex = 1;
        BTFICHAMEDICA.Text = "HISTORIAL CLÍNICO";
        BTFICHAMEDICA.TextAlign = ContentAlignment.BottomCenter;
        BTFICHAMEDICA.TextImageRelation = TextImageRelation.ImageAboveText;
        BTFICHAMEDICA.UseVisualStyleBackColor = false;
        BTFICHAMEDICA.Click += BTFICHAMEDICA_Click;
        // 
        // picFichaMedica
        // 
        picFichaMedica.BackColor = Color.Transparent;
        picFichaMedica.Image = (Image)resources.GetObject("picFichaMedica.Image");
        picFichaMedica.Location = new Point(367, 162);
        picFichaMedica.Name = "picFichaMedica";
        picFichaMedica.Size = new Size(129, 186);
        picFichaMedica.SizeMode = PictureBoxSizeMode.Zoom;
        picFichaMedica.TabIndex = 0;
        picFichaMedica.TabStop = false;
        picFichaMedica.Click += BTFICHAMEDICA_Click;
        // 
        // BTCONSULTAS
        // 
        BTCONSULTAS.BackColor = Color.White;
        BTCONSULTAS.FlatAppearance.BorderColor = Color.FromArgb(210, 186, 190);
        BTCONSULTAS.FlatStyle = FlatStyle.Flat;
        BTCONSULTAS.Font = new Font("Segoe UI", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
        BTCONSULTAS.ImageAlign = ContentAlignment.TopCenter;
        BTCONSULTAS.Location = new Point(64, 143);
        BTCONSULTAS.Name = "BTCONSULTAS";
        BTCONSULTAS.Padding = new Padding(12, 8, 12, 10);
        BTCONSULTAS.Size = new Size(214, 277);
        BTCONSULTAS.TabIndex = 0;
        BTCONSULTAS.Text = "CONSULTAS";
        BTCONSULTAS.TextAlign = ContentAlignment.BottomCenter;
        BTCONSULTAS.TextImageRelation = TextImageRelation.ImageAboveText;
        BTCONSULTAS.UseVisualStyleBackColor = false;
        BTCONSULTAS.Click += BTCONSULTAS_Click;
        // 
        // picConsultas
        // 
        picConsultas.BackColor = Color.Transparent;
        picConsultas.Image = (Image)resources.GetObject("picConsultas.Image");
        picConsultas.Location = new Point(107, 162);
        picConsultas.Name = "picConsultas";
        picConsultas.Size = new Size(125, 186);
        picConsultas.SizeMode = PictureBoxSizeMode.Zoom;
        picConsultas.TabIndex = 0;
        picConsultas.TabStop = false;
        picConsultas.Click += BTCONSULTAS_Click;
        // 
        // barraEstado
        // 
        barraEstado.BackColor = Color.FromArgb(249, 240, 242);
        barraEstado.ImageScalingSize = new Size(20, 20);
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
        lblInfoEstado.Size = new Size(109, 17);
        lblInfoEstado.Text = "Módulo clínico listo";
        // 
        // FormVeterinarioPrincipal
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
        MinimumSize = new Size(800, 498);
        Name = "FormVeterinarioPrincipal";
        StartPosition = FormStartPosition.CenterScreen;
        Text = "Veterinaria - Panel Veterinario";
        Load += FormVeterinarioPrincipal_Load;
        pnlEncabezado.ResumeLayout(false);
        pnlContenido.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)picVacunas).EndInit();
        ((System.ComponentModel.ISupportInitialize)picTratamientos).EndInit();
        ((System.ComponentModel.ISupportInitialize)picFichaMedica).EndInit();
        ((System.ComponentModel.ISupportInitialize)picConsultas).EndInit();
        barraEstado.ResumeLayout(false);
        barraEstado.PerformLayout();
        ResumeLayout(false);
        PerformLayout();
    }

    private Button BTCONSULTAS;
    private Button BTFICHAMEDICA;
    private Button BTTRATAMIENTOS;
    private Button BTVACUNAS;
}
