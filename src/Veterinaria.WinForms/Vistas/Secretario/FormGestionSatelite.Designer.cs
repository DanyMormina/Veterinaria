namespace Veterinaria.WinForms.Vistas.Secretario;

partial class FormGestionSatelite
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
        lblSeleccion = new Label();
        cboSeleccion = new ComboBox();
        lblParentEspecie = new Label();
        cboParentEspecie = new ComboBox();
        lblNombre = new Label();
        txtNombre = new TextBox();
        pnlBotones = new Panel();
        btnGuardar = new Button();
        btnModificar = new Button();
        btnBaja = new Button();
        btnCancelar = new Button();
        pnlEncabezado.SuspendLayout();
        pnlContenido.SuspendLayout();
        pnlBotones.SuspendLayout();
        SuspendLayout();
        // 
        // pnlEncabezado
        // 
        pnlEncabezado.BackColor = Color.FromArgb(200, 138, 150);
        pnlEncabezado.Controls.Add(lblTitulo);
        pnlEncabezado.Dock = DockStyle.Top;
        pnlEncabezado.Location = new Point(0, 0);
        pnlEncabezado.Name = "pnlEncabezado";
        pnlEncabezado.Padding = new Padding(16, 0, 16, 0);
        pnlEncabezado.Size = new Size(520, 45);
        pnlEncabezado.TabIndex = 0;
        // 
        // lblTitulo
        // 
        lblTitulo.Dock = DockStyle.Fill;
        lblTitulo.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
        lblTitulo.ForeColor = Color.White;
        lblTitulo.Location = new Point(16, 0);
        lblTitulo.Name = "lblTitulo";
        lblTitulo.Size = new Size(488, 45);
        lblTitulo.TabIndex = 0;
        lblTitulo.Text = "GESTIÓN SATÉLITE";
        lblTitulo.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // pnlContenido
        // 
        pnlContenido.BackColor = Color.FromArgb(250, 244, 244);
        pnlContenido.Controls.Add(lblSeleccion);
        pnlContenido.Controls.Add(cboSeleccion);
        pnlContenido.Controls.Add(lblParentEspecie);
        pnlContenido.Controls.Add(cboParentEspecie);
        pnlContenido.Controls.Add(lblNombre);
        pnlContenido.Controls.Add(txtNombre);
        pnlContenido.Dock = DockStyle.Fill;
        pnlContenido.Location = new Point(0, 45);
        pnlContenido.Name = "pnlContenido";
        pnlContenido.Padding = new Padding(24, 16, 24, 16);
        pnlContenido.Size = new Size(520, 225);
        pnlContenido.TabIndex = 1;
        // 
        // lblSeleccion
        // 
        lblSeleccion.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
        lblSeleccion.ForeColor = Color.FromArgb(58, 53, 59);
        lblSeleccion.Location = new Point(24, 12);
        lblSeleccion.Name = "lblSeleccion";
        lblSeleccion.Size = new Size(472, 18);
        lblSeleccion.TabIndex = 0;
        lblSeleccion.Text = "Seleccionar para editar:";
        // 
        // cboSeleccion
        // 
        cboSeleccion.BackColor = Color.White;
        cboSeleccion.DropDownStyle = ComboBoxStyle.DropDownList;
        cboSeleccion.Font = new Font("Segoe UI", 9.5F);
        cboSeleccion.ForeColor = Color.FromArgb(58, 53, 59);
        cboSeleccion.FormattingEnabled = true;
        cboSeleccion.Location = new Point(24, 32);
        cboSeleccion.Name = "cboSeleccion";
        cboSeleccion.Size = new Size(472, 25);
        cboSeleccion.TabIndex = 1;
        // 
        // lblParentEspecie
        // 
        lblParentEspecie.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
        lblParentEspecie.ForeColor = Color.FromArgb(58, 53, 59);
        lblParentEspecie.Location = new Point(24, 70);
        lblParentEspecie.Name = "lblParentEspecie";
        lblParentEspecie.Size = new Size(472, 18);
        lblParentEspecie.TabIndex = 2;
        lblParentEspecie.Text = "Especie asociada:";
        // 
        // cboParentEspecie
        // 
        cboParentEspecie.BackColor = Color.White;
        cboParentEspecie.DropDownStyle = ComboBoxStyle.DropDownList;
        cboParentEspecie.Font = new Font("Segoe UI", 9.5F);
        cboParentEspecie.ForeColor = Color.FromArgb(58, 53, 59);
        cboParentEspecie.FormattingEnabled = true;
        cboParentEspecie.Location = new Point(24, 90);
        cboParentEspecie.Name = "cboParentEspecie";
        cboParentEspecie.Size = new Size(472, 25);
        cboParentEspecie.TabIndex = 3;
        // 
        // lblNombre
        // 
        lblNombre.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
        lblNombre.ForeColor = Color.FromArgb(58, 53, 59);
        lblNombre.Location = new Point(24, 128);
        lblNombre.Name = "lblNombre";
        lblNombre.Size = new Size(472, 18);
        lblNombre.TabIndex = 4;
        lblNombre.Text = "Nombre:";
        // 
        // txtNombre
        // 
        txtNombre.BackColor = Color.White;
        txtNombre.BorderStyle = BorderStyle.FixedSingle;
        txtNombre.Font = new Font("Segoe UI", 9.5F);
        txtNombre.ForeColor = Color.FromArgb(58, 53, 59);
        txtNombre.Location = new Point(24, 148);
        txtNombre.Name = "txtNombre";
        txtNombre.Size = new Size(472, 25);
        txtNombre.TabIndex = 5;
        // 
        // pnlBotones
        // 
        pnlBotones.BackColor = Color.FromArgb(250, 244, 244);
        pnlBotones.Controls.Add(btnGuardar);
        pnlBotones.Controls.Add(btnModificar);
        pnlBotones.Controls.Add(btnBaja);
        pnlBotones.Controls.Add(btnCancelar);
        pnlBotones.Dock = DockStyle.Bottom;
        pnlBotones.Location = new Point(0, 270);
        pnlBotones.Name = "pnlBotones";
        pnlBotones.Padding = new Padding(24, 8, 24, 16);
        pnlBotones.Size = new Size(520, 56);
        pnlBotones.TabIndex = 2;
        // 
        // btnGuardar
        // 
        btnGuardar.BackColor = Color.FromArgb(152, 196, 164);
        btnGuardar.Cursor = Cursors.Hand;
        btnGuardar.FlatAppearance.BorderSize = 0;
        btnGuardar.FlatStyle = FlatStyle.Flat;
        btnGuardar.Font = new Font("Segoe UI", 9F);
        btnGuardar.ForeColor = Color.Black;
        btnGuardar.Location = new Point(268, 10);
        btnGuardar.Name = "btnGuardar";
        btnGuardar.Size = new Size(110, 32);
        btnGuardar.TabIndex = 0;
        btnGuardar.Text = "Guardar";
        btnGuardar.UseVisualStyleBackColor = false;
        // 
        // btnModificar
        // 
        btnModificar.BackColor = Color.FromArgb(148, 176, 214);
        btnModificar.Cursor = Cursors.Hand;
        btnModificar.FlatAppearance.BorderSize = 0;
        btnModificar.FlatStyle = FlatStyle.Flat;
        btnModificar.Font = new Font("Segoe UI", 9F);
        btnModificar.ForeColor = Color.Black;
        btnModificar.Location = new Point(176, 10);
        btnModificar.Name = "btnModificar";
        btnModificar.Size = new Size(100, 32);
        btnModificar.TabIndex = 1;
        btnModificar.Text = "Modificar";
        btnModificar.UseVisualStyleBackColor = false;
        // 
        // btnBaja
        // 
        btnBaja.BackColor = Color.FromArgb(230, 160, 165);
        btnBaja.Cursor = Cursors.Hand;
        btnBaja.FlatAppearance.BorderSize = 0;
        btnBaja.FlatStyle = FlatStyle.Flat;
        btnBaja.Font = new Font("Segoe UI", 9F);
        btnBaja.ForeColor = Color.Black;
        btnBaja.Location = new Point(282, 10);
        btnBaja.Name = "btnBaja";
        btnBaja.Size = new Size(96, 32);
        btnBaja.TabIndex = 2;
        btnBaja.Text = "Dar de Baja";
        btnBaja.UseVisualStyleBackColor = false;
        // 
        // btnCancelar
        // 
        btnCancelar.BackColor = Color.FromArgb(226, 217, 220);
        btnCancelar.Cursor = Cursors.Hand;
        btnCancelar.FlatAppearance.BorderSize = 0;
        btnCancelar.FlatStyle = FlatStyle.Flat;
        btnCancelar.Font = new Font("Segoe UI", 9F);
        btnCancelar.ForeColor = Color.FromArgb(58, 53, 59);
        btnCancelar.Location = new Point(386, 10);
        btnCancelar.Name = "btnCancelar";
        btnCancelar.Size = new Size(110, 32);
        btnCancelar.TabIndex = 3;
        btnCancelar.Text = "Cancelar";
        btnCancelar.UseVisualStyleBackColor = false;
        // 
        // FormGestionSatelite
        // 
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        BackColor = Color.FromArgb(250, 244, 244);
        ClientSize = new Size(520, 326);
        Controls.Add(pnlContenido);
        Controls.Add(pnlBotones);
        Controls.Add(pnlEncabezado);
        Font = new Font("Segoe UI", 9F);
        ForeColor = Color.FromArgb(58, 53, 59);
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;
        MinimizeBox = false;
        Name = "FormGestionSatelite";
        StartPosition = FormStartPosition.CenterParent;
        Text = "Gestión Satélite";
        pnlEncabezado.ResumeLayout(false);
        pnlContenido.ResumeLayout(false);
        pnlBotones.ResumeLayout(false);
        ResumeLayout(false);
    }

    private Panel pnlEncabezado;
    private Label lblTitulo;
    private Panel pnlContenido;
    private Label lblSeleccion;
    private ComboBox cboSeleccion;
    private Label lblParentEspecie;
    private ComboBox cboParentEspecie;
    private Label lblNombre;
    private TextBox txtNombre;
    private Panel pnlBotones;
    private Button btnGuardar;
    private Button btnModificar;
    private Button btnBaja;
    private Button btnCancelar;
}
