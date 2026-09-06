namespace Veterinaria.WinForms.Vistas.Autenticacion;

partial class FormInicioSesion
{
    private System.ComponentModel.IContainer components = null;
    private System.Windows.Forms.Label lblTitulo;
    private System.Windows.Forms.Button btnCerrar;
    private System.Windows.Forms.TextBox txtUsuario;
    private System.Windows.Forms.TextBox txtContrasena;
    private System.Windows.Forms.Button btnIngresar;
    private System.Windows.Forms.Label lblError;
    private System.Windows.Forms.Panel pnlUsuario;
    private System.Windows.Forms.Panel pnlContrasena;

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
        lblTitulo = new Label();
        btnCerrar = new Button();
        pnlUsuario = new Panel();
        txtUsuario = new TextBox();
        pnlContrasena = new Panel();
        txtContrasena = new TextBox();
        lblError = new Label();
        btnIngresar = new Button();
        pnlUsuario.SuspendLayout();
        pnlContrasena.SuspendLayout();
        SuspendLayout();
        // 
        // lblTitulo
        // 
        lblTitulo.Font = new Font("Segoe UI", 9.75F, FontStyle.Bold);
        lblTitulo.ForeColor = Color.FromArgb(58, 53, 59);
        lblTitulo.Location = new Point(12, 10);
        lblTitulo.Name = "lblTitulo";
        lblTitulo.Size = new Size(150, 20);
        lblTitulo.TabIndex = 0;
        lblTitulo.Text = "VETERINARIA";
        lblTitulo.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // btnCerrar
        // 
        btnCerrar.FlatAppearance.BorderSize = 0;
        btnCerrar.FlatStyle = FlatStyle.Flat;
        btnCerrar.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
        btnCerrar.ForeColor = Color.FromArgb(142, 130, 138);
        btnCerrar.Location = new Point(172, 6);
        btnCerrar.Name = "btnCerrar";
        btnCerrar.Size = new Size(22, 22);
        btnCerrar.TabIndex = 6;
        btnCerrar.Text = "×";
        btnCerrar.UseVisualStyleBackColor = true;
        btnCerrar.Click += btnCerrar_Click;
        // 
        // pnlUsuario
        // 
        pnlUsuario.BackColor = Color.White;
        pnlUsuario.BorderStyle = BorderStyle.FixedSingle;
        pnlUsuario.Controls.Add(txtUsuario);
        pnlUsuario.Location = new Point(16, 38);
        pnlUsuario.Name = "pnlUsuario";
        pnlUsuario.Padding = new Padding(4, 2, 4, 2);
        pnlUsuario.Size = new Size(168, 26);
        pnlUsuario.TabIndex = 1;
        // 
        // txtUsuario
        // 
        txtUsuario.BorderStyle = BorderStyle.None;
        txtUsuario.Dock = DockStyle.Fill;
        txtUsuario.Font = new Font("Segoe UI", 9F);
        txtUsuario.ForeColor = Color.FromArgb(142, 130, 138);
        txtUsuario.Location = new Point(4, 2);
        txtUsuario.Name = "txtUsuario";
        txtUsuario.Size = new Size(158, 16);
        txtUsuario.TabIndex = 0;
        txtUsuario.Text = "Usuario";
        txtUsuario.Enter += txtUsuario_Enter;
        txtUsuario.KeyDown += txtCampos_KeyDown;
        txtUsuario.Leave += txtUsuario_Leave;
        // 
        // pnlContrasena
        // 
        pnlContrasena.BackColor = Color.White;
        pnlContrasena.BorderStyle = BorderStyle.FixedSingle;
        pnlContrasena.Controls.Add(txtContrasena);
        pnlContrasena.Location = new Point(16, 70);
        pnlContrasena.Name = "pnlContrasena";
        pnlContrasena.Padding = new Padding(4, 2, 4, 2);
        pnlContrasena.Size = new Size(168, 26);
        pnlContrasena.TabIndex = 2;
        // 
        // txtContrasena
        // 
        txtContrasena.BorderStyle = BorderStyle.None;
        txtContrasena.Dock = DockStyle.Fill;
        txtContrasena.Font = new Font("Segoe UI", 9F);
        txtContrasena.ForeColor = Color.FromArgb(142, 130, 138);
        txtContrasena.Location = new Point(4, 2);
        txtContrasena.Name = "txtContrasena";
        txtContrasena.Size = new Size(158, 16);
        txtContrasena.TabIndex = 0;
        txtContrasena.Text = "Contraseña";
        txtContrasena.Enter += txtContrasena_Enter;
        txtContrasena.KeyDown += txtCampos_KeyDown;
        txtContrasena.Leave += txtContrasena_Leave;
        // 
        // lblError
        // 
        lblError.Font = new Font("Segoe UI", 7F);
        lblError.ForeColor = Color.FromArgb(192, 57, 43);
        lblError.Location = new Point(16, 100);
        lblError.Name = "lblError";
        lblError.Size = new Size(168, 48);
        lblError.TabIndex = 3;
        lblError.TextAlign = ContentAlignment.MiddleCenter;
        lblError.Visible = false;
        // 
        // btnIngresar
        // 
        btnIngresar.BackColor = Color.FromArgb(200, 138, 150);
        btnIngresar.Cursor = Cursors.Hand;
        btnIngresar.FlatAppearance.BorderSize = 0;
        btnIngresar.FlatStyle = FlatStyle.Flat;
        btnIngresar.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
        btnIngresar.ForeColor = Color.White;
        btnIngresar.Location = new Point(16, 152);
        btnIngresar.Name = "btnIngresar";
        btnIngresar.Size = new Size(168, 32);
        btnIngresar.TabIndex = 4;
        btnIngresar.Text = "INGRESAR";
        btnIngresar.UseVisualStyleBackColor = false;
        btnIngresar.Click += btnIngresar_Click;
        // 
        // FormInicioSesion
        // 
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        BackColor = Color.FromArgb(250, 244, 244);
        ClientSize = new Size(200, 200);
        Controls.Add(lblTitulo);
        Controls.Add(btnCerrar);
        Controls.Add(pnlUsuario);
        Controls.Add(pnlContrasena);
        Controls.Add(lblError);
        Controls.Add(btnIngresar);
        FormBorderStyle = FormBorderStyle.None;
        KeyPreview = true;
        MaximizeBox = false;
        MinimizeBox = false;
        Name = "FormInicioSesion";
        StartPosition = FormStartPosition.CenterScreen;
        Text = "Acceso";
        MouseDown += FormInicioSesion_MouseDown;
        pnlUsuario.ResumeLayout(false);
        pnlUsuario.PerformLayout();
        pnlContrasena.ResumeLayout(false);
        pnlContrasena.PerformLayout();
        ResumeLayout(false);
    }
}
