namespace Veterinaria.WinForms.Views.Auth;

partial class FormLogin
{
    private System.ComponentModel.IContainer components = null;
    private System.Windows.Forms.Label lblTitulo;
    private System.Windows.Forms.Button btnCerrar;
    private System.Windows.Forms.TextBox txtUsuario;
    private System.Windows.Forms.TextBox txtPassword;
    private System.Windows.Forms.Button btnLogin;
    private System.Windows.Forms.Label lblError;
    private System.Windows.Forms.Panel pnlUsuario;
    private System.Windows.Forms.Panel pnlPassword;

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
        pnlPassword = new Panel();
        txtPassword = new TextBox();
        lblError = new Label();
        btnLogin = new Button();
        pnlUsuario.SuspendLayout();
        pnlPassword.SuspendLayout();
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
        // pnlPassword
        // 
        pnlPassword.BackColor = Color.White;
        pnlPassword.BorderStyle = BorderStyle.FixedSingle;
        pnlPassword.Controls.Add(txtPassword);
        pnlPassword.Location = new Point(16, 70);
        pnlPassword.Name = "pnlPassword";
        pnlPassword.Padding = new Padding(4, 2, 4, 2);
        pnlPassword.Size = new Size(168, 26);
        pnlPassword.TabIndex = 2;
        // 
        // txtPassword
        // 
        txtPassword.BorderStyle = BorderStyle.None;
        txtPassword.Dock = DockStyle.Fill;
        txtPassword.Font = new Font("Segoe UI", 9F);
        txtPassword.ForeColor = Color.FromArgb(142, 130, 138);
        txtPassword.Location = new Point(4, 2);
        txtPassword.Name = "txtPassword";
        txtPassword.Size = new Size(158, 16);
        txtPassword.TabIndex = 0;
        txtPassword.Text = "Contraseña";
        txtPassword.Enter += txtPassword_Enter;
        txtPassword.KeyDown += txtCampos_KeyDown;
        txtPassword.Leave += txtPassword_Leave;
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
        // btnLogin
        // 
        btnLogin.BackColor = Color.FromArgb(200, 138, 150);
        btnLogin.Cursor = Cursors.Hand;
        btnLogin.FlatAppearance.BorderSize = 0;
        btnLogin.FlatStyle = FlatStyle.Flat;
        btnLogin.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
        btnLogin.ForeColor = Color.White;
        btnLogin.Location = new Point(16, 152);
        btnLogin.Name = "btnLogin";
        btnLogin.Size = new Size(168, 32);
        btnLogin.TabIndex = 4;
        btnLogin.Text = "INGRESAR";
        btnLogin.UseVisualStyleBackColor = false;
        btnLogin.Click += btnLogin_Click;
        // 
        // FormLogin
        // 
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        BackColor = Color.FromArgb(250, 244, 244);
        ClientSize = new Size(200, 200);
        Controls.Add(lblTitulo);
        Controls.Add(btnCerrar);
        Controls.Add(pnlUsuario);
        Controls.Add(pnlPassword);
        Controls.Add(lblError);
        Controls.Add(btnLogin);
        FormBorderStyle = FormBorderStyle.None;
        KeyPreview = true;
        MaximizeBox = false;
        MinimizeBox = false;
        Name = "FormLogin";
        StartPosition = FormStartPosition.CenterScreen;
        Text = "Acceso";
        MouseDown += FormLogin_MouseDown;
        pnlUsuario.ResumeLayout(false);
        pnlUsuario.PerformLayout();
        pnlPassword.ResumeLayout(false);
        pnlPassword.PerformLayout();
        ResumeLayout(false);
    }
}
