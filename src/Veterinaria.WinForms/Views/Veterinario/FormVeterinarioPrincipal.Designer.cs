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
        pnlHeader.SuspendLayout();
        statusStrip.SuspendLayout();
        SuspendLayout();
        // 
        // pnlHeader
        // 
        pnlHeader.BackColor = Color.FromArgb(200, 138, 150);
        pnlHeader.Controls.Add(lblTitulo);
        pnlHeader.Controls.Add(lblUsuarioSesion);
        pnlHeader.Dock = DockStyle.Top;
        pnlHeader.Location = new Point(0, 0);
        pnlHeader.Margin = new Padding(3, 4, 3, 4);
        pnlHeader.Name = "pnlHeader";
        pnlHeader.Padding = new Padding(18, 0, 18, 0);
        pnlHeader.Size = new Size(1257, 67);
        pnlHeader.TabIndex = 0;
        // 
        // lblTitulo
        // 
        lblTitulo.Dock = DockStyle.Left;
        lblTitulo.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
        lblTitulo.ForeColor = Color.White;
        lblTitulo.Location = new Point(18, 0);
        lblTitulo.Name = "lblTitulo";
        lblTitulo.Size = new Size(455, 67);
        lblTitulo.TabIndex = 0;
        lblTitulo.Text = "CLÍNICA VETERINARIA — ATENCIÓN CLÍNICA";
        lblTitulo.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // lblUsuarioSesion
        // 
        lblUsuarioSesion.Dock = DockStyle.Right;
        lblUsuarioSesion.Font = new Font("Segoe UI", 9.75F);
        lblUsuarioSesion.ForeColor = Color.FromArgb(250, 244, 244);
        lblUsuarioSesion.Location = new Point(782, 0);
        lblUsuarioSesion.Name = "lblUsuarioSesion";
        lblUsuarioSesion.Size = new Size(457, 67);
        lblUsuarioSesion.TabIndex = 1;
        lblUsuarioSesion.Text = "Médico: Veterinario";
        lblUsuarioSesion.TextAlign = ContentAlignment.MiddleRight;
        // 
        // pnlContenido
        // 
        pnlContenido.BackColor = Color.FromArgb(250, 244, 244);
        pnlContenido.Dock = DockStyle.Fill;
        pnlContenido.Location = new Point(0, 67);
        pnlContenido.Margin = new Padding(3, 4, 3, 4);
        pnlContenido.Name = "pnlContenido";
        pnlContenido.Size = new Size(1257, 841);
        pnlContenido.TabIndex = 1;
        // 
        // statusStrip
        // 
        statusStrip.BackColor = Color.FromArgb(249, 240, 242);
        statusStrip.ImageScalingSize = new Size(20, 20);
        statusStrip.Items.AddRange(new ToolStripItem[] { lblStatusInfo });
        statusStrip.Location = new Point(0, 908);
        statusStrip.Name = "statusStrip";
        statusStrip.Padding = new Padding(1, 0, 16, 0);
        statusStrip.Size = new Size(1257, 25);
        statusStrip.TabIndex = 2;
        // 
        // lblStatusInfo
        // 
        lblStatusInfo.Font = new Font("Segoe UI", 8.25F);
        lblStatusInfo.ForeColor = Color.FromArgb(58, 53, 59);
        lblStatusInfo.Name = "lblStatusInfo";
        lblStatusInfo.Size = new Size(127, 19);
        lblStatusInfo.Text = "Módulo clínico listo";
        // 
        // FormVeterinarioPrincipal
        // 
        AutoScaleDimensions = new SizeF(8F, 20F);
        AutoScaleMode = AutoScaleMode.Font;
        BackColor = Color.FromArgb(250, 244, 244);
        ClientSize = new Size(1257, 933);
        Controls.Add(pnlContenido);
        Controls.Add(statusStrip);
        Controls.Add(pnlHeader);
        Font = new Font("Segoe UI", 9F);
        ForeColor = Color.FromArgb(58, 53, 59);
        Margin = new Padding(3, 4, 3, 4);
        MinimumSize = new Size(912, 651);
        Name = "FormVeterinarioPrincipal";
        StartPosition = FormStartPosition.CenterScreen;
        Text = "Veterinaria - Panel Veterinario";
        Load += FormVeterinarioPrincipal_Load;
        pnlHeader.ResumeLayout(false);
        statusStrip.ResumeLayout(false);
        statusStrip.PerformLayout();
        ResumeLayout(false);
        PerformLayout();
    }
}
