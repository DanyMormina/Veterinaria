namespace Veterinaria.WinForms.Views.Veterinario;

partial class FormVeterinarioPrincipal
{
    private System.ComponentModel.IContainer components = null;
    private System.Windows.Forms.Panel pnlHeader;
    private System.Windows.Forms.Label lblTitulo;
    private System.Windows.Forms.Label lblUsuarioSesion;
    private System.Windows.Forms.Panel pnlContenido;
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
        pnlHeader = new Panel();
        lblTitulo = new Label();
        lblUsuarioSesion = new Label();
        pnlContenido = new Panel();
        statusStrip = new StatusStrip();
        lblStatusInfo = new ToolStripStatusLabel();
        BTCONSULTAS = new Button();
        BTFICHAMEDICA = new Button();
        BTTRATAMIENTOS = new Button();
        BTVACUNAS = new Button();
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
        // BTCONSULTAS
        // 
        BTCONSULTAS.BackColor = Color.White;
        BTCONSULTAS.FlatAppearance.BorderColor = Color.FromArgb(210, 186, 190);
        BTCONSULTAS.FlatStyle = FlatStyle.Flat;
        BTCONSULTAS.Font = new Font("Segoe UI", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
        BTCONSULTAS.ImageAlign = ContentAlignment.TopCenter;
        BTCONSULTAS.Location = new Point(64, 143);
        BTCONSULTAS.Name = "BTCONSULTAS";
        BTCONSULTAS.Size = new Size(214, 277);
        BTCONSULTAS.TabIndex = 0;
        BTCONSULTAS.Text = "CONSULTAS";
        BTCONSULTAS.TextAlign = ContentAlignment.BottomCenter;
        BTCONSULTAS.UseVisualStyleBackColor = false;
        BTCONSULTAS.Click += BTCONSULTAS_Click;
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
        BTFICHAMEDICA.Size = new Size(214, 277);
        BTFICHAMEDICA.TabIndex = 1;
        BTFICHAMEDICA.Text = "FICHA MÉDICA";
        BTFICHAMEDICA.TextAlign = ContentAlignment.BottomCenter;
        BTFICHAMEDICA.UseVisualStyleBackColor = false;
        BTFICHAMEDICA.Click += BTFICHAMEDICA_Click;
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
        BTTRATAMIENTOS.Size = new Size(214, 277);
        BTTRATAMIENTOS.TabIndex = 2;
        BTTRATAMIENTOS.Text = "TRATAMIENTOS";
        BTTRATAMIENTOS.TextAlign = ContentAlignment.BottomCenter;
        BTTRATAMIENTOS.UseVisualStyleBackColor = false;
        BTTRATAMIENTOS.Click += BTTRATAMIENTOS_Click;
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
        BTVACUNAS.Size = new Size(214, 277);
        BTVACUNAS.TabIndex = 3;
        BTVACUNAS.Text = "VACUNAS Y CONTROLES";
        BTVACUNAS.TextAlign = ContentAlignment.BottomCenter;
        BTVACUNAS.UseVisualStyleBackColor = false;
        BTVACUNAS.Click += BTVACUNAS_Click;
        // 
        // pnlContenido
        // 
        pnlContenido.BackColor = Color.FromArgb(250, 244, 244);
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
        // statusStrip
        // 
        statusStrip.BackColor = Color.FromArgb(249, 240, 242);
        statusStrip.ImageScalingSize = new Size(20, 20);
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
        lblStatusInfo.Size = new Size(109, 17);
        lblStatusInfo.Text = "Módulo clínico listo";
        // 
        // FormVeterinarioPrincipal
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
        MinimumSize = new Size(800, 498);
        Name = "FormVeterinarioPrincipal";
        StartPosition = FormStartPosition.CenterScreen;
        Text = "Veterinaria - Panel Veterinario";
        Load += FormVeterinarioPrincipal_Load;
        pnlHeader.ResumeLayout(false);
        statusStrip.ResumeLayout(false);
        statusStrip.PerformLayout();
        pnlContenido.ResumeLayout(false);
        ResumeLayout(false);
        PerformLayout();
    }

    private Button BTCONSULTAS;
    private Button BTFICHAMEDICA;
    private Button BTTRATAMIENTOS;
    private Button BTVACUNAS;
}
