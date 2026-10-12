namespace Veterinaria.WinForms.Vistas.Administrador;

partial class FormConfirmarRestore
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
        lblAdvertenciaIcono = new Label();
        lblAdvertenciaTitulo = new Label();
        lblAdvertenciaTexto = new Label();
        lblArchivo = new Label();
        lblArchivoNombre = new Label();
        lblPasswordPrompt = new Label();
        txtPasswordConfirmacion = new TextBox();
        pnlBotones = new Panel();
        btnConfirmar = new Button();
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
        lblTitulo.Text = "AUTORIZACIÓN REQUERIDA — RESTAURAR BASE DE DATOS";
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
        pnlContenido.Size = new Size(520, 316);
        pnlContenido.TabIndex = 1;
        // 
        // pnlTarjeta
        // 
        pnlTarjeta.BackColor = Color.White;
        pnlTarjeta.Controls.Add(lblAdvertenciaIcono);
        pnlTarjeta.Controls.Add(lblAdvertenciaTitulo);
        pnlTarjeta.Controls.Add(lblAdvertenciaTexto);
        pnlTarjeta.Controls.Add(lblArchivo);
        pnlTarjeta.Controls.Add(lblArchivoNombre);
        pnlTarjeta.Controls.Add(lblPasswordPrompt);
        pnlTarjeta.Controls.Add(txtPasswordConfirmacion);
        pnlTarjeta.Dock = DockStyle.Top;
        pnlTarjeta.Location = new Point(16, 16);
        pnlTarjeta.Name = "pnlTarjeta";
        pnlTarjeta.Padding = new Padding(16);
        pnlTarjeta.Size = new Size(488, 230);
        pnlTarjeta.TabIndex = 0;
        // 
        // lblAdvertenciaIcono
        // 
        lblAdvertenciaIcono.Font = new Font("Segoe UI", 16F);
        lblAdvertenciaIcono.ForeColor = Color.FromArgb(184, 93, 105);
        lblAdvertenciaIcono.Location = new Point(14, 12);
        lblAdvertenciaIcono.Name = "lblAdvertenciaIcono";
        lblAdvertenciaIcono.Size = new Size(32, 30);
        lblAdvertenciaIcono.TabIndex = 0;
        lblAdvertenciaIcono.Text = "⚠";
        lblAdvertenciaIcono.TextAlign = ContentAlignment.MiddleCenter;
        // 
        // lblAdvertenciaTitulo
        // 
        lblAdvertenciaTitulo.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
        lblAdvertenciaTitulo.ForeColor = Color.FromArgb(184, 93, 105);
        lblAdvertenciaTitulo.Location = new Point(50, 14);
        lblAdvertenciaTitulo.Name = "lblAdvertenciaTitulo";
        lblAdvertenciaTitulo.Size = new Size(418, 26);
        lblAdvertenciaTitulo.TabIndex = 1;
        lblAdvertenciaTitulo.Text = "ADVERTENCIA CRÍTICA DE SOBRESCRITURA";
        lblAdvertenciaTitulo.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // lblAdvertenciaTexto
        // 
        lblAdvertenciaTexto.Font = new Font("Segoe UI", 9F);
        lblAdvertenciaTexto.ForeColor = Color.FromArgb(58, 53, 59);
        lblAdvertenciaTexto.Location = new Point(16, 44);
        lblAdvertenciaTexto.Name = "lblAdvertenciaTexto";
        lblAdvertenciaTexto.Size = new Size(452, 40);
        lblAdvertenciaTexto.TabIndex = 2;
        lblAdvertenciaTexto.Text = "Esta acción sobrescribirá todos los datos actuales con la copia seleccionada. Toda información posterior a este respaldo se perderá.";
        // 
        // lblArchivo
        // 
        lblArchivo.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
        lblArchivo.ForeColor = Color.FromArgb(58, 53, 59);
        lblArchivo.Location = new Point(16, 90);
        lblArchivo.Name = "lblArchivo";
        lblArchivo.Size = new Size(130, 22);
        lblArchivo.TabIndex = 3;
        lblArchivo.Text = "Copia a restaurar:";
        // 
        // lblArchivoNombre
        // 
        lblArchivoNombre.AutoEllipsis = true;
        lblArchivoNombre.BackColor = Color.FromArgb(250, 244, 244);
        lblArchivoNombre.BorderStyle = BorderStyle.FixedSingle;
        lblArchivoNombre.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
        lblArchivoNombre.ForeColor = Color.FromArgb(58, 53, 59);
        lblArchivoNombre.Location = new Point(16, 114);
        lblArchivoNombre.Name = "lblArchivoNombre";
        lblArchivoNombre.Padding = new Padding(6, 0, 6, 0);
        lblArchivoNombre.Size = new Size(452, 26);
        lblArchivoNombre.TabIndex = 4;
        lblArchivoNombre.Text = "Archivo.bak";
        lblArchivoNombre.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // lblPasswordPrompt
        // 
        lblPasswordPrompt.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
        lblPasswordPrompt.ForeColor = Color.FromArgb(58, 53, 59);
        lblPasswordPrompt.Location = new Point(16, 148);
        lblPasswordPrompt.Name = "lblPasswordPrompt";
        lblPasswordPrompt.Size = new Size(452, 22);
        lblPasswordPrompt.TabIndex = 5;
        lblPasswordPrompt.Text = "Ingrese su contraseña de administrador para confirmar:";
        // 
        // txtPasswordConfirmacion
        // 
        txtPasswordConfirmacion.BackColor = Color.White;
        txtPasswordConfirmacion.BorderStyle = BorderStyle.FixedSingle;
        txtPasswordConfirmacion.Font = new Font("Segoe UI", 10F);
        txtPasswordConfirmacion.ForeColor = Color.FromArgb(58, 53, 59);
        txtPasswordConfirmacion.Location = new Point(16, 174);
        txtPasswordConfirmacion.Name = "txtPasswordConfirmacion";
        txtPasswordConfirmacion.Size = new Size(452, 25);
        txtPasswordConfirmacion.TabIndex = 6;
        txtPasswordConfirmacion.UseSystemPasswordChar = true;
        // 
        // pnlBotones
        // 
        pnlBotones.Controls.Add(btnConfirmar);
        pnlBotones.Controls.Add(btnCancelar);
        pnlBotones.Dock = DockStyle.Bottom;
        pnlBotones.Location = new Point(16, 260);
        pnlBotones.Name = "pnlBotones";
        pnlBotones.Size = new Size(488, 40);
        pnlBotones.TabIndex = 1;
        // 
        // btnConfirmar
        // 
        btnConfirmar.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        btnConfirmar.BackColor = Color.FromArgb(184, 93, 105);
        btnConfirmar.Cursor = Cursors.Hand;
        btnConfirmar.FlatAppearance.BorderSize = 0;
        btnConfirmar.FlatStyle = FlatStyle.Flat;
        btnConfirmar.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
        btnConfirmar.ForeColor = Color.White;
        btnConfirmar.Location = new Point(280, 4);
        btnConfirmar.Name = "btnConfirmar";
        btnConfirmar.Size = new Size(208, 32);
        btnConfirmar.TabIndex = 0;
        btnConfirmar.Text = "Confirmar Restauración";
        btnConfirmar.UseVisualStyleBackColor = false;
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
        // FormConfirmarRestore
        // 
        AcceptButton = btnConfirmar;
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        BackColor = Color.FromArgb(250, 244, 244);
        CancelButton = btnCancelar;
        ClientSize = new Size(520, 360);
        Controls.Add(pnlContenido);
        Controls.Add(pnlEncabezado);
        Font = new Font("Segoe UI", 9F);
        ForeColor = Color.FromArgb(58, 53, 59);
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;
        MinimizeBox = false;
        Name = "FormConfirmarRestore";
        StartPosition = FormStartPosition.CenterParent;
        Text = "Confirmar Restauración de Base de Datos";
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
    private Label lblAdvertenciaIcono;
    private Label lblAdvertenciaTitulo;
    private Label lblAdvertenciaTexto;
    private Label lblArchivo;
    private Label lblArchivoNombre;
    private Label lblPasswordPrompt;
    private TextBox txtPasswordConfirmacion;
    private Panel pnlBotones;
    private Button btnConfirmar;
    private Button btnCancelar;
}
