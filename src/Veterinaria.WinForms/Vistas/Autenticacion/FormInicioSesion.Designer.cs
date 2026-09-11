namespace Veterinaria.WinForms.Vistas.Autenticacion;

partial class FormInicioSesion
{
    private System.ComponentModel.IContainer components = null;
    private System.Windows.Forms.Panel pnlSeparadorVertical;
    private System.Windows.Forms.Label lblTitulo;
    private System.Windows.Forms.Panel pnlDecoradorTitulo;
    private System.Windows.Forms.Panel pnlLineaIzq;
    private System.Windows.Forms.Label lblIconoPata;
    private System.Windows.Forms.Panel pnlLineaDer;
    private System.Windows.Forms.Label lblUsuario;
    private System.Windows.Forms.Panel pnlUsuario;
    private System.Windows.Forms.TextBox txtUsuario;
    private System.Windows.Forms.Label lblContrasena;
    private System.Windows.Forms.Panel pnlContrasena;
    private System.Windows.Forms.TextBox txtContrasena;
    private System.Windows.Forms.CheckBox chkRecordarUsuario;
    private System.Windows.Forms.Label lblError;

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
        System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormInicioSesion));
        pnlSeparadorVertical = new Panel();
        lblTitulo = new Label();
        pnlDecoradorTitulo = new Panel();
        pnlLineaDer = new Panel();
        lblIconoPata = new Label();
        pnlLineaIzq = new Panel();
        lblUsuario = new Label();
        pnlUsuario = new Panel();
        txtUsuario = new TextBox();
        lblContrasena = new Label();
        pnlContrasena = new Panel();
        txtContrasena = new TextBox();
        chkRecordarUsuario = new CheckBox();
        lblError = new Label();
        BTINGRESAR = new Button();
        BTSALIR = new Button();
        picLogo = new PictureBox();
        pnlDecoradorTitulo.SuspendLayout();
        pnlUsuario.SuspendLayout();
        pnlContrasena.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)picLogo).BeginInit();
        SuspendLayout();
        // 
        // pnlSeparadorVertical
        // 
        pnlSeparadorVertical.BackColor = Color.FromArgb(235, 192, 202);
        pnlSeparadorVertical.Location = new Point(365, 35);
        pnlSeparadorVertical.Name = "pnlSeparadorVertical";
        pnlSeparadorVertical.Size = new Size(1, 370);
        pnlSeparadorVertical.TabIndex = 1;
        // 
        // lblTitulo
        // 
        lblTitulo.Font = new Font("Georgia", 18F);
        lblTitulo.ForeColor = Color.FromArgb(45, 40, 46);
        lblTitulo.Location = new Point(390, 35);
        lblTitulo.Name = "lblTitulo";
        lblTitulo.Size = new Size(410, 34);
        lblTitulo.TabIndex = 2;
        lblTitulo.Text = "Sistema de Gestión Veterinaria";
        lblTitulo.TextAlign = ContentAlignment.MiddleCenter;
        // 
        // pnlDecoradorTitulo
        // 
        pnlDecoradorTitulo.Controls.Add(pnlLineaDer);
        pnlDecoradorTitulo.Controls.Add(lblIconoPata);
        pnlDecoradorTitulo.Controls.Add(pnlLineaIzq);
        pnlDecoradorTitulo.Location = new Point(390, 72);
        pnlDecoradorTitulo.Name = "pnlDecoradorTitulo";
        pnlDecoradorTitulo.Size = new Size(410, 22);
        pnlDecoradorTitulo.TabIndex = 3;
        // 
        // pnlLineaDer
        // 
        pnlLineaDer.BackColor = Color.FromArgb(235, 192, 202);
        pnlLineaDer.Location = new Point(230, 10);
        pnlLineaDer.Name = "pnlLineaDer";
        pnlLineaDer.Size = new Size(165, 1);
        pnlLineaDer.TabIndex = 2;
        // 
        // lblIconoPata
        // 
        lblIconoPata.Font = new Font("Segoe UI", 10F);
        lblIconoPata.ForeColor = Color.FromArgb(212, 139, 152);
        lblIconoPata.Location = new Point(186, 0);
        lblIconoPata.Name = "lblIconoPata";
        lblIconoPata.Size = new Size(38, 20);
        lblIconoPata.TabIndex = 1;
        lblIconoPata.Text = "🐾";
        lblIconoPata.TextAlign = ContentAlignment.MiddleCenter;
        // 
        // pnlLineaIzq
        // 
        pnlLineaIzq.BackColor = Color.FromArgb(235, 192, 202);
        pnlLineaIzq.Location = new Point(15, 10);
        pnlLineaIzq.Name = "pnlLineaIzq";
        pnlLineaIzq.Size = new Size(165, 1);
        pnlLineaIzq.TabIndex = 0;
        // 
        // lblUsuario
        // 
        lblUsuario.Font = new Font("Segoe UI", 10.5F, FontStyle.Bold);
        lblUsuario.ForeColor = Color.FromArgb(45, 40, 46);
        lblUsuario.Location = new Point(400, 110);
        lblUsuario.Name = "lblUsuario";
        lblUsuario.Size = new Size(390, 20);
        lblUsuario.TabIndex = 4;
        lblUsuario.Text = "Usuario:";
        lblUsuario.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // pnlUsuario
        // 
        pnlUsuario.BackColor = Color.White;
        pnlUsuario.BorderStyle = BorderStyle.FixedSingle;
        pnlUsuario.Controls.Add(txtUsuario);
        pnlUsuario.Location = new Point(400, 134);
        pnlUsuario.Name = "pnlUsuario";
        pnlUsuario.Padding = new Padding(10, 8, 10, 8);
        pnlUsuario.Size = new Size(390, 38);
        pnlUsuario.TabIndex = 5;
        // 
        // txtUsuario
        // 
        txtUsuario.BorderStyle = BorderStyle.None;
        txtUsuario.Dock = DockStyle.Fill;
        txtUsuario.Font = new Font("Segoe UI", 10.5F);
        txtUsuario.ForeColor = Color.FromArgb(160, 140, 148);
        txtUsuario.Location = new Point(10, 8);
        txtUsuario.Name = "txtUsuario";
        txtUsuario.Size = new Size(368, 19);
        txtUsuario.TabIndex = 0;
        txtUsuario.Text = "Ingrese su usuario";
        txtUsuario.Enter += txtUsuario_Enter;
        txtUsuario.KeyDown += txtCampos_KeyDown;
        txtUsuario.Leave += txtUsuario_Leave;
        // 
        // lblContrasena
        // 
        lblContrasena.Font = new Font("Segoe UI", 10.5F, FontStyle.Bold);
        lblContrasena.ForeColor = Color.FromArgb(45, 40, 46);
        lblContrasena.Location = new Point(400, 186);
        lblContrasena.Name = "lblContrasena";
        lblContrasena.Size = new Size(390, 20);
        lblContrasena.TabIndex = 6;
        lblContrasena.Text = "Contraseña:";
        lblContrasena.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // pnlContrasena
        // 
        pnlContrasena.BackColor = Color.White;
        pnlContrasena.BorderStyle = BorderStyle.FixedSingle;
        pnlContrasena.Controls.Add(txtContrasena);
        pnlContrasena.Location = new Point(400, 210);
        pnlContrasena.Name = "pnlContrasena";
        pnlContrasena.Padding = new Padding(10, 8, 10, 8);
        pnlContrasena.Size = new Size(390, 38);
        pnlContrasena.TabIndex = 7;
        // 
        // txtContrasena
        // 
        txtContrasena.BorderStyle = BorderStyle.None;
        txtContrasena.Dock = DockStyle.Fill;
        txtContrasena.Font = new Font("Segoe UI", 10.5F);
        txtContrasena.ForeColor = Color.FromArgb(160, 140, 148);
        txtContrasena.Location = new Point(10, 8);
        txtContrasena.Name = "txtContrasena";
        txtContrasena.Size = new Size(368, 19);
        txtContrasena.TabIndex = 0;
        txtContrasena.Text = "Ingrese su contraseña";
        txtContrasena.Enter += txtContrasena_Enter;
        txtContrasena.KeyDown += txtCampos_KeyDown;
        txtContrasena.Leave += txtContrasena_Leave;
        // 
        // chkRecordarUsuario
        // 
        chkRecordarUsuario.AutoSize = true;
        chkRecordarUsuario.Cursor = Cursors.Hand;
        chkRecordarUsuario.FlatStyle = FlatStyle.Flat;
        chkRecordarUsuario.Font = new Font("Segoe UI", 9.5F);
        chkRecordarUsuario.ForeColor = Color.FromArgb(45, 40, 46);
        chkRecordarUsuario.Location = new Point(400, 260);
        chkRecordarUsuario.Name = "chkRecordarUsuario";
        chkRecordarUsuario.Size = new Size(125, 21);
        chkRecordarUsuario.TabIndex = 8;
        chkRecordarUsuario.Text = "Recordar usuario";
        chkRecordarUsuario.UseVisualStyleBackColor = true;
        // 
        // lblError
        // 
        lblError.Font = new Font("Segoe UI", 8.5F, FontStyle.Bold);
        lblError.ForeColor = Color.FromArgb(184, 93, 105);
        lblError.Location = new Point(400, 288);
        lblError.Name = "lblError";
        lblError.Size = new Size(390, 38);
        lblError.TabIndex = 9;
        lblError.TextAlign = ContentAlignment.MiddleLeft;
        lblError.Visible = false;
        // 
        // BTINGRESAR
        // 
        BTINGRESAR.BackColor = Color.FromArgb(230, 196, 202);
        BTINGRESAR.FlatAppearance.BorderColor = Color.FromArgb(186, 162, 168);
        BTINGRESAR.FlatStyle = FlatStyle.Flat;
        BTINGRESAR.ForeColor = Color.FromArgb(58, 53, 59);
        BTINGRESAR.Location = new Point(454, 329);
        BTINGRESAR.Name = "BTINGRESAR";
        BTINGRESAR.Size = new Size(131, 44);
        BTINGRESAR.TabIndex = 10;
        BTINGRESAR.Text = "INGRESAR";
        BTINGRESAR.UseVisualStyleBackColor = false;
        // 
        // BTSALIR
        // 
        BTSALIR.BackColor = Color.PapayaWhip;
        BTSALIR.FlatAppearance.BorderColor = Color.FromArgb(186, 162, 168);
        BTSALIR.FlatStyle = FlatStyle.Flat;
        BTSALIR.ForeColor = Color.FromArgb(58, 53, 59);
        BTSALIR.Location = new Point(603, 329);
        BTSALIR.Name = "BTSALIR";
        BTSALIR.Size = new Size(131, 44);
        BTSALIR.TabIndex = 11;
        BTSALIR.Text = "SALIR";
        BTSALIR.UseVisualStyleBackColor = false;
        // 
        // picLogo
        // 
        picLogo.BackColor = Color.White;
        picLogo.BorderStyle = BorderStyle.FixedSingle;
        picLogo.ErrorImage = (Image)resources.GetObject("picLogo.ErrorImage");
        picLogo.Location = new Point(35, 40);
        picLogo.Name = "picLogo";
        picLogo.Size = new Size(300, 360);
        picLogo.SizeMode = PictureBoxSizeMode.Zoom;
        picLogo.TabIndex = 0;
        picLogo.TabStop = false;
        // 
        // FormInicioSesion
        // 
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        BackColor = Color.FromArgb(254, 243, 246);
        ClientSize = new Size(830, 440);
        Controls.Add(BTSALIR);
        Controls.Add(BTINGRESAR);
        Controls.Add(picLogo);
        Controls.Add(pnlSeparadorVertical);
        Controls.Add(lblTitulo);
        Controls.Add(pnlDecoradorTitulo);
        Controls.Add(lblUsuario);
        Controls.Add(pnlUsuario);
        Controls.Add(lblContrasena);
        Controls.Add(pnlContrasena);
        Controls.Add(chkRecordarUsuario);
        Controls.Add(lblError);
        FormBorderStyle = FormBorderStyle.FixedSingle;
        KeyPreview = true;
        MaximizeBox = false;
        Name = "FormInicioSesion";
        StartPosition = FormStartPosition.CenterScreen;
        Text = "Inicio de sesión";
        pnlDecoradorTitulo.ResumeLayout(false);
        pnlUsuario.ResumeLayout(false);
        pnlUsuario.PerformLayout();
        pnlContrasena.ResumeLayout(false);
        pnlContrasena.PerformLayout();
        ((System.ComponentModel.ISupportInitialize)picLogo).EndInit();
        ResumeLayout(false);
        PerformLayout();
    }

    private Button BTINGRESAR;
    private Button BTSALIR;
    private PictureBox picLogo;
}
