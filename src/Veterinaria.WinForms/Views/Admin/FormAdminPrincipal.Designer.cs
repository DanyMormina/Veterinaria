namespace Veterinaria.WinForms.Views.Admin;

partial class FormAdminPrincipal
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
        pnlHeader = new System.Windows.Forms.Panel();
        lblTitulo = new System.Windows.Forms.Label();
        lblUsuarioSesion = new System.Windows.Forms.Label();
        pnlContenido = new System.Windows.Forms.Panel();
        statusStrip = new System.Windows.Forms.StatusStrip();
        lblStatusInfo = new System.Windows.Forms.ToolStripStatusLabel();
        pnlHeader.SuspendLayout();
        statusStrip.SuspendLayout();
        SuspendLayout();
        // 
        // pnlHeader
        // 
        pnlHeader.BackColor = System.Drawing.Color.FromArgb(200, 138, 150);
        pnlHeader.Controls.Add(lblTitulo);
        pnlHeader.Controls.Add(lblUsuarioSesion);
        pnlHeader.Dock = System.Windows.Forms.DockStyle.Top;
        pnlHeader.Location = new System.Drawing.Point(0, 0);
        pnlHeader.Name = "pnlHeader";
        pnlHeader.Padding = new System.Windows.Forms.Padding(16, 0, 16, 0);
        pnlHeader.Size = new System.Drawing.Size(1100, 50);
        pnlHeader.TabIndex = 0;
        // 
        // lblTitulo
        // 
        lblTitulo.Dock = System.Windows.Forms.DockStyle.Left;
        lblTitulo.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
        lblTitulo.ForeColor = System.Drawing.Color.White;
        lblTitulo.Location = new System.Drawing.Point(16, 0);
        lblTitulo.Name = "lblTitulo";
        lblTitulo.Size = new System.Drawing.Size(350, 50);
        lblTitulo.TabIndex = 0;
        lblTitulo.Text = "CLÍNICA VETERINARIA — ADMINISTRADOR";
        lblTitulo.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
        // 
        // lblUsuarioSesion
        // 
        lblUsuarioSesion.Dock = System.Windows.Forms.DockStyle.Right;
        lblUsuarioSesion.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
        lblUsuarioSesion.ForeColor = System.Drawing.Color.FromArgb(250, 244, 244);
        lblUsuarioSesion.Location = new System.Drawing.Point(684, 0);
        lblUsuarioSesion.Name = "lblUsuarioSesion";
        lblUsuarioSesion.Size = new System.Drawing.Size(400, 50);
        lblUsuarioSesion.TabIndex = 1;
        lblUsuarioSesion.Text = "Usuario: Admin";
        lblUsuarioSesion.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
        // 
        // pnlContenido
        // 
        pnlContenido.BackColor = System.Drawing.Color.FromArgb(250, 244, 244);
        pnlContenido.Dock = System.Windows.Forms.DockStyle.Fill;
        pnlContenido.Location = new System.Drawing.Point(0, 50);
        pnlContenido.Name = "pnlContenido";
        pnlContenido.Size = new System.Drawing.Size(1100, 628);
        pnlContenido.TabIndex = 1;
        // 
        // statusStrip
        // 
        statusStrip.BackColor = System.Drawing.Color.FromArgb(249, 240, 242);
        statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { lblStatusInfo });
        statusStrip.Location = new System.Drawing.Point(0, 678);
        statusStrip.Name = "statusStrip";
        statusStrip.Size = new System.Drawing.Size(1100, 22);
        statusStrip.TabIndex = 2;
        // 
        // lblStatusInfo
        // 
        lblStatusInfo.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
        lblStatusInfo.ForeColor = System.Drawing.Color.FromArgb(58, 53, 59);
        lblStatusInfo.Name = "lblStatusInfo";
        lblStatusInfo.Size = new System.Drawing.Size(125, 17);
        lblStatusInfo.Text = "Sistema listo para operar";
        // 
        // FormAdminPrincipal
        // 
        AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
        AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        BackColor = System.Drawing.Color.FromArgb(250, 244, 244);
        ClientSize = new System.Drawing.Size(1100, 700);
        Controls.Add(pnlContenido);
        Controls.Add(statusStrip);
        Controls.Add(pnlHeader);
        Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
        ForeColor = System.Drawing.Color.FromArgb(58, 53, 59);
        MinimumSize = new System.Drawing.Size(800, 500);
        Name = "FormAdminPrincipal";
        StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
        Text = "Veterinaria - Panel Administrador";
        Load += FormAdminPrincipal_Load;
        pnlHeader.ResumeLayout(false);
        statusStrip.ResumeLayout(false);
        statusStrip.PerformLayout();
        ResumeLayout(false);
        PerformLayout();
    }
}
