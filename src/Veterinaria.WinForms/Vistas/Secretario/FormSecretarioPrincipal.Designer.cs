namespace Veterinaria.WinForms.Vistas.Secretario;

partial class FormSecretarioPrincipal
{
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
        pnlEncabezado = new System.Windows.Forms.Panel();
        lblTitulo = new System.Windows.Forms.Label();
        lblUsuarioSesion = new System.Windows.Forms.Label();
        pnlContenido = new System.Windows.Forms.Panel();
        barraEstado = new System.Windows.Forms.StatusStrip();
        lblInfoEstado = new System.Windows.Forms.ToolStripStatusLabel();
        pnlEncabezado.SuspendLayout();
        barraEstado.SuspendLayout();
        SuspendLayout();
        // 
        // pnlEncabezado
        // 
        pnlEncabezado.BackColor = System.Drawing.Color.FromArgb(200, 138, 150);
        pnlEncabezado.Controls.Add(lblTitulo);
        pnlEncabezado.Controls.Add(lblUsuarioSesion);
        pnlEncabezado.Dock = System.Windows.Forms.DockStyle.Top;
        pnlEncabezado.Location = new System.Drawing.Point(0, 0);
        pnlEncabezado.Name = "pnlEncabezado";
        pnlEncabezado.Padding = new System.Windows.Forms.Padding(16, 0, 16, 0);
        pnlEncabezado.Size = new System.Drawing.Size(1100, 50);
        pnlEncabezado.TabIndex = 0;
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
        lblTitulo.Text = "CLÍNICA VETERINARIA — RECEPCIÓN";
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
        lblUsuarioSesion.Text = "Recepción: Secretario";
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
        // barraEstado
        // 
        barraEstado.BackColor = System.Drawing.Color.FromArgb(249, 240, 242);
        barraEstado.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { lblInfoEstado });
        barraEstado.Location = new System.Drawing.Point(0, 678);
        barraEstado.Name = "barraEstado";
        barraEstado.Size = new System.Drawing.Size(1100, 22);
        barraEstado.TabIndex = 2;
        // 
        // lblInfoEstado
        // 
        lblInfoEstado.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
        lblInfoEstado.ForeColor = System.Drawing.Color.FromArgb(58, 53, 59);
        lblInfoEstado.Name = "lblInfoEstado";
        lblInfoEstado.Size = new System.Drawing.Size(142, 17);
        lblInfoEstado.Text = "Módulo de recepción listo";
        // 
        // FormSecretarioPrincipal
        // 
        AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
        AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        BackColor = System.Drawing.Color.FromArgb(250, 244, 244);
        ClientSize = new System.Drawing.Size(1100, 700);
        Controls.Add(pnlContenido);
        Controls.Add(barraEstado);
        Controls.Add(pnlEncabezado);
        Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
        ForeColor = System.Drawing.Color.FromArgb(58, 53, 59);
        MinimumSize = new System.Drawing.Size(800, 500);
        Name = "FormSecretarioPrincipal";
        StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
        Text = "Veterinaria - Panel Secretario";
        Load += FormSecretarioPrincipal_Load;
        pnlEncabezado.ResumeLayout(false);
        barraEstado.ResumeLayout(false);
        barraEstado.PerformLayout();
        ResumeLayout(false);
        PerformLayout();
    }
}
