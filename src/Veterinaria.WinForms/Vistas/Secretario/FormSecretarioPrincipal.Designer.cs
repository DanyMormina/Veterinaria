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

    private System.Windows.Forms.Button BTPROPIETARIOS;
    private System.Windows.Forms.Button BTMASCOTAS;

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
        pnlContenido = new Panel();
        BTMASCOTAS = new Button();
        BTPROPIETARIOS = new Button();
        barraEstado = new StatusStrip();
        lblInfoEstado = new ToolStripStatusLabel();
        pnlEncabezado.SuspendLayout();
        pnlContenido.SuspendLayout();
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
        lblTitulo.Size = new Size(350, 50);
        lblTitulo.TabIndex = 0;
        lblTitulo.Text = "CLÍNICA VETERINARIA — RECEPCIÓN";
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
        lblUsuarioSesion.Text = "Recepción: Secretario";
        lblUsuarioSesion.TextAlign = ContentAlignment.MiddleRight;
        // 
        // pnlContenido
        // 
        pnlContenido.BackColor = Color.FromArgb(250, 244, 244);
        pnlContenido.Controls.Add(BTMASCOTAS);
        pnlContenido.Controls.Add(BTPROPIETARIOS);
        pnlContenido.Dock = DockStyle.Fill;
        pnlContenido.Location = new Point(0, 50);
        pnlContenido.Name = "pnlContenido";
        pnlContenido.Size = new Size(1100, 628);
        pnlContenido.TabIndex = 1;
        // 
        // BTMASCOTAS
        // 
        BTMASCOTAS.BackColor = Color.White;
        BTMASCOTAS.Cursor = Cursors.Hand;
        BTMASCOTAS.FlatAppearance.BorderColor = Color.FromArgb(210, 186, 190);
        BTMASCOTAS.FlatStyle = FlatStyle.Flat;
        BTMASCOTAS.Font = new Font("Segoe UI", 9.75F, FontStyle.Bold);
        BTMASCOTAS.ForeColor = Color.FromArgb(58, 53, 59);
        BTMASCOTAS.ImageAlign = ContentAlignment.TopCenter;
        BTMASCOTAS.Location = new Point(313, 136);
        BTMASCOTAS.Name = "BTMASCOTAS";
        BTMASCOTAS.Padding = new Padding(12, 8, 12, 10);
        BTMASCOTAS.Size = new Size(214, 277);
        BTMASCOTAS.TabIndex = 1;
        BTMASCOTAS.Text = "MASCOTAS";
        BTMASCOTAS.TextAlign = ContentAlignment.BottomCenter;
        BTMASCOTAS.TextImageRelation = TextImageRelation.ImageAboveText;
        BTMASCOTAS.UseVisualStyleBackColor = false;
        BTMASCOTAS.Click += BTMASCOTAS_Click;
        // 
        // BTPROPIETARIOS
        // 
        BTPROPIETARIOS.BackColor = Color.White;
        BTPROPIETARIOS.Cursor = Cursors.Hand;
        BTPROPIETARIOS.FlatAppearance.BorderColor = Color.FromArgb(210, 186, 190);
        BTPROPIETARIOS.FlatStyle = FlatStyle.Flat;
        BTPROPIETARIOS.Font = new Font("Segoe UI", 9.75F, FontStyle.Bold);
        BTPROPIETARIOS.ForeColor = Color.FromArgb(58, 53, 59);
        BTPROPIETARIOS.ImageAlign = ContentAlignment.TopCenter;
        BTPROPIETARIOS.Location = new Point(578, 136);
        BTPROPIETARIOS.Name = "BTPROPIETARIOS";
        BTPROPIETARIOS.Padding = new Padding(12, 8, 12, 10);
        BTPROPIETARIOS.Size = new Size(214, 277);
        BTPROPIETARIOS.TabIndex = 0;
        BTPROPIETARIOS.Text = "PROPIETARIOS";
        BTPROPIETARIOS.TextAlign = ContentAlignment.BottomCenter;
        BTPROPIETARIOS.TextImageRelation = TextImageRelation.ImageAboveText;
        BTPROPIETARIOS.UseVisualStyleBackColor = false;
        BTPROPIETARIOS.Click += BTPROPIETARIOS_Click;
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
        lblInfoEstado.Size = new Size(142, 17);
        lblInfoEstado.Text = "Módulo de recepción listo";
        // 
        // FormSecretarioPrincipal
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
        Name = "FormSecretarioPrincipal";
        StartPosition = FormStartPosition.CenterScreen;
        Text = "Veterinaria - Panel Secretario";
        Load += FormSecretarioPrincipal_Load;
        pnlEncabezado.ResumeLayout(false);
        pnlContenido.ResumeLayout(false);
        barraEstado.ResumeLayout(false);
        barraEstado.PerformLayout();
        ResumeLayout(false);
        PerformLayout();
    }
}
