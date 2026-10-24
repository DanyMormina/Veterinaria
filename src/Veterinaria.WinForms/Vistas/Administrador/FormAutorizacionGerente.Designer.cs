namespace Veterinaria.WinForms.Vistas.Administrador;

partial class FormAutorizacionGerente
{
    private System.ComponentModel.IContainer components = null;

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
        pnlContenido = new Panel();
        pnlTarjeta = new Panel();
        lblIcono = new Label();
        lblSubtitulo = new Label();
        lblTexto = new Label();
        lblPasswordPrompt = new Label();
        txtPasswordGerente = new TextBox();
        pnlBotones = new Panel();
        btnAutorizar = new Button();
        btnCancelar = new Button();
        pnlEncabezado.SuspendLayout();
        pnlContenido.SuspendLayout();
        pnlTarjeta.SuspendLayout();
        pnlBotones.SuspendLayout();
        SuspendLayout();
        // 
        // pnlEncabezado
        // 
        pnlEncabezado.BackColor = Color.FromArgb(184, 93, 105);
        pnlEncabezado.Controls.Add(lblTitulo);
        pnlEncabezado.Dock = DockStyle.Top;
        pnlEncabezado.Location = new Point(0, 0);
        pnlEncabezado.Name = "pnlEncabezado";
        pnlEncabezado.Padding = new Padding(16, 0, 16, 0);
        pnlEncabezado.Size = new Size(520, 44);
        pnlEncabezado.TabIndex = 0;
        // 
        // lblTitulo
        // 
        lblTitulo.Dock = DockStyle.Fill;
        lblTitulo.Font = new Font("Segoe UI", 10.5F, FontStyle.Bold);
        lblTitulo.ForeColor = Color.White;
        lblTitulo.Location = new Point(16, 0);
        lblTitulo.Name = "lblTitulo";
        lblTitulo.Size = new Size(488, 44);
        lblTitulo.TabIndex = 0;
        lblTitulo.Text = "AUTORIZACIÓN DEL GERENTE — RESTAURAR BASE DE DATOS";
        lblTitulo.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // pnlContenido
        // 
        pnlContenido.BackColor = Color.FromArgb(250, 244, 244);
        pnlContenido.Controls.Add(pnlTarjeta);
        pnlContenido.Controls.Add(pnlBotones);
        pnlContenido.Dock = DockStyle.Fill;
        pnlContenido.Location = new Point(0, 44);
        pnlContenido.Name = "pnlContenido";
        pnlContenido.Padding = new Padding(16);
        pnlContenido.Size = new Size(520, 226);
        pnlContenido.TabIndex = 1;
        // 
        // pnlTarjeta
        // 
        pnlTarjeta.BackColor = Color.White;
        pnlTarjeta.Controls.Add(lblIcono);
        pnlTarjeta.Controls.Add(lblSubtitulo);
        pnlTarjeta.Controls.Add(lblTexto);
        pnlTarjeta.Controls.Add(lblPasswordPrompt);
        pnlTarjeta.Controls.Add(txtPasswordGerente);
        pnlTarjeta.Dock = DockStyle.Top;
        pnlTarjeta.Location = new Point(16, 16);
        pnlTarjeta.Name = "pnlTarjeta";
        pnlTarjeta.Padding = new Padding(16);
        pnlTarjeta.Size = new Size(488, 150);
        pnlTarjeta.TabIndex = 0;
        // 
        // lblIcono
        // 
        lblIcono.Font = new Font("Segoe UI", 16F);
        lblIcono.ForeColor = Color.FromArgb(184, 93, 105);
        lblIcono.Location = new Point(14, 12);
        lblIcono.Name = "lblIcono";
        lblIcono.Size = new Size(32, 30);
        lblIcono.TabIndex = 0;
        lblIcono.Text = "🔒";
        lblIcono.TextAlign = ContentAlignment.MiddleCenter;
        // 
        // lblSubtitulo
        // 
        lblSubtitulo.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
        lblSubtitulo.ForeColor = Color.FromArgb(184, 93, 105);
        lblSubtitulo.Location = new Point(50, 14);
        lblSubtitulo.Name = "lblSubtitulo";
        lblSubtitulo.Size = new Size(418, 26);
        lblSubtitulo.TabIndex = 1;
        lblSubtitulo.Text = "SEGUNDA AUTORIZACIÓN REQUERIDA";
        lblSubtitulo.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // lblTexto
        // 
        lblTexto.Font = new Font("Segoe UI", 9F);
        lblTexto.ForeColor = Color.FromArgb(58, 53, 59);
        lblTexto.Location = new Point(16, 44);
        lblTexto.Name = "lblTexto";
        lblTexto.Size = new Size(452, 36);
        lblTexto.TabIndex = 2;
        lblTexto.Text = "La contraseña del administrador fue verificada. Para continuar, el gerente debe autorizar la restauración.";
        // 
        // lblPasswordPrompt
        // 
        lblPasswordPrompt.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
        lblPasswordPrompt.ForeColor = Color.FromArgb(58, 53, 59);
        lblPasswordPrompt.Location = new Point(16, 84);
        lblPasswordPrompt.Name = "lblPasswordPrompt";
        lblPasswordPrompt.Size = new Size(452, 22);
        lblPasswordPrompt.TabIndex = 3;
        lblPasswordPrompt.Text = "Contraseña del gerente:";
        // 
        // txtPasswordGerente
        // 
        txtPasswordGerente.BackColor = Color.White;
        txtPasswordGerente.BorderStyle = BorderStyle.FixedSingle;
        txtPasswordGerente.Font = new Font("Segoe UI", 10F);
        txtPasswordGerente.ForeColor = Color.FromArgb(58, 53, 59);
        txtPasswordGerente.Location = new Point(16, 110);
        txtPasswordGerente.MaxLength = 100;
        txtPasswordGerente.Name = "txtPasswordGerente";
        txtPasswordGerente.Size = new Size(452, 25);
        txtPasswordGerente.TabIndex = 4;
        txtPasswordGerente.UseSystemPasswordChar = true;
        // 
        // pnlBotones
        // 
        pnlBotones.Controls.Add(btnAutorizar);
        pnlBotones.Controls.Add(btnCancelar);
        pnlBotones.Dock = DockStyle.Bottom;
        pnlBotones.Location = new Point(16, 170);
        pnlBotones.Name = "pnlBotones";
        pnlBotones.Size = new Size(488, 40);
        pnlBotones.TabIndex = 1;
        // 
        // btnAutorizar
        // 
        btnAutorizar.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        btnAutorizar.BackColor = Color.FromArgb(184, 93, 105);
        btnAutorizar.Cursor = Cursors.Hand;
        btnAutorizar.FlatAppearance.BorderSize = 0;
        btnAutorizar.FlatStyle = FlatStyle.Flat;
        btnAutorizar.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
        btnAutorizar.ForeColor = Color.White;
        btnAutorizar.Location = new Point(280, 4);
        btnAutorizar.Name = "btnAutorizar";
        btnAutorizar.Size = new Size(208, 32);
        btnAutorizar.TabIndex = 0;
        btnAutorizar.Text = "Autorizar Restauración";
        btnAutorizar.UseVisualStyleBackColor = false;
        // 
        // btnCancelar
        // 
        btnCancelar.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        btnCancelar.BackColor = Color.FromArgb(226, 217, 220);
        btnCancelar.Cursor = Cursors.Hand;
        btnCancelar.FlatAppearance.BorderSize = 0;
        btnCancelar.FlatStyle = FlatStyle.Flat;
        btnCancelar.Font = new Font("Segoe UI", 9.5F);
        btnCancelar.ForeColor = Color.FromArgb(58, 53, 59);
        btnCancelar.Location = new Point(160, 4);
        btnCancelar.Name = "btnCancelar";
        btnCancelar.Size = new Size(110, 32);
        btnCancelar.TabIndex = 1;
        btnCancelar.Text = "Cancelar";
        btnCancelar.UseVisualStyleBackColor = false;
        // 
        // FormAutorizacionGerente
        // 
        AcceptButton = btnAutorizar;
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        BackColor = Color.FromArgb(250, 244, 244);
        CancelButton = btnCancelar;
        ClientSize = new Size(520, 270);
        Controls.Add(pnlContenido);
        Controls.Add(pnlEncabezado);
        Font = new Font("Segoe UI", 9F);
        ForeColor = Color.FromArgb(58, 53, 59);
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;
        MinimizeBox = false;
        Name = "FormAutorizacionGerente";
        ShowInTaskbar = false;
        StartPosition = FormStartPosition.CenterParent;
        Text = "Autorización del Gerente";
        pnlEncabezado.ResumeLayout(false);
        pnlContenido.ResumeLayout(false);
        pnlTarjeta.ResumeLayout(false);
        pnlTarjeta.PerformLayout();
        pnlBotones.ResumeLayout(false);
        ResumeLayout(false);
    }

    private Panel pnlEncabezado;
    private Label lblTitulo;
    private Panel pnlContenido;
    private Panel pnlTarjeta;
    private Label lblIcono;
    private Label lblSubtitulo;
    private Label lblTexto;
    private Label lblPasswordPrompt;
    private TextBox txtPasswordGerente;
    private Panel pnlBotones;
    private Button btnAutorizar;
    private Button btnCancelar;
}
