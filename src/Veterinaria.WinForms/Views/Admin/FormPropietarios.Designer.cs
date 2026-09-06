namespace Veterinaria.WinForms.Views.Admin;

partial class FormPropietarios
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
        pnlHeader = new Panel();
        lblTitulo = new Label();
        lblUsuarioSesion = new Label();
        pnlContenido = new Panel();
        grpDatos = new GroupBox();
        lblDni = new Label();
        txtDni = new TextBox();
        lblNombre = new Label();
        txtNombre = new TextBox();
        lblApellido = new Label();
        txtApellido = new TextBox();
        lblTelefono = new Label();
        txtTelefono = new TextBox();
        lblEmail = new Label();
        txtEmail = new TextBox();
        lblDireccion = new Label();
        txtDireccion = new TextBox();
        pnlBotones = new Panel();
        btnNuevo = new Button();
        btnGuardar = new Button();
        btnModificar = new Button();
        btnCancelar = new Button();
        btnVolver = new Button();
        pnlListado = new Panel();
        lblBuscar = new Label();
        txtBuscar = new TextBox();
        btnBuscar = new Button();
        btnActivos = new Button();
        btnInactivos = new Button();
        dgvPropietarios = new DataGridView();
        colId = new DataGridViewTextBoxColumn();
        colDni = new DataGridViewTextBoxColumn();
        colNombre = new DataGridViewTextBoxColumn();
        colApellido = new DataGridViewTextBoxColumn();
        colTelefono = new DataGridViewTextBoxColumn();
        colEmail = new DataGridViewTextBoxColumn();
        colDireccion = new DataGridViewTextBoxColumn();
        colEstado = new DataGridViewTextBoxColumn();
        pnlHeader.SuspendLayout();
        pnlContenido.SuspendLayout();
        grpDatos.SuspendLayout();
        pnlBotones.SuspendLayout();
        pnlListado.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)dgvPropietarios).BeginInit();
        SuspendLayout();
        // 
        // pnlHeader
        // 
        pnlHeader.BackColor = Color.FromArgb(200, 138, 150);
        pnlHeader.Controls.Add(lblTitulo);
        pnlHeader.Controls.Add(lblUsuarioSesion);
        pnlHeader.Dock = DockStyle.Top;
        pnlHeader.Location = new Point(0, 0);
        pnlHeader.Name = "pnlHeader";
        pnlHeader.Padding = new Padding(16, 0, 16, 0);
        pnlHeader.Size = new Size(1100, 50);
        pnlHeader.TabIndex = 0;
        // 
        // lblTitulo
        // 
        lblTitulo.Dock = DockStyle.Left;
        lblTitulo.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
        lblTitulo.ForeColor = Color.White;
        lblTitulo.Location = new Point(16, 0);
        lblTitulo.Name = "lblTitulo";
        lblTitulo.Size = new Size(680, 50);
        lblTitulo.TabIndex = 0;
        lblTitulo.Text = "CLÍNICA VETERINARIA — ADMINISTRADOR — GESTIÓN DE PROPIETARIOS";
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
        lblUsuarioSesion.Text = "Usuario: Admin";
        lblUsuarioSesion.TextAlign = ContentAlignment.MiddleRight;
        // 
        // pnlContenido
        // 
        pnlContenido.BackColor = Color.FromArgb(250, 244, 244);
        pnlContenido.Controls.Add(grpDatos);
        pnlContenido.Controls.Add(pnlBotones);
        pnlContenido.Controls.Add(pnlListado);
        pnlContenido.Dock = DockStyle.Fill;
        pnlContenido.Location = new Point(0, 50);
        pnlContenido.Name = "pnlContenido";
        pnlContenido.Size = new Size(1100, 650);
        pnlContenido.TabIndex = 1;
        // 
        // grpDatos
        // 
        grpDatos.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
        grpDatos.Controls.Add(lblDni);
        grpDatos.Controls.Add(txtDni);
        grpDatos.Controls.Add(lblNombre);
        grpDatos.Controls.Add(txtNombre);
        grpDatos.Controls.Add(lblApellido);
        grpDatos.Controls.Add(txtApellido);
        grpDatos.Controls.Add(lblTelefono);
        grpDatos.Controls.Add(txtTelefono);
        grpDatos.Controls.Add(lblEmail);
        grpDatos.Controls.Add(txtEmail);
        grpDatos.Controls.Add(lblDireccion);
        grpDatos.Controls.Add(txtDireccion);
        grpDatos.Font = new Font("Segoe UI", 9F);
        grpDatos.ForeColor = Color.FromArgb(58, 53, 59);
        grpDatos.Location = new Point(16, 16);
        grpDatos.Name = "grpDatos";
        grpDatos.Size = new Size(428, 528);
        grpDatos.TabIndex = 0;
        grpDatos.TabStop = false;
        grpDatos.Text = "Datos del propietario";
        // 
        // lblDni
        // 
        lblDni.Location = new Point(16, 40);
        lblDni.Name = "lblDni";
        lblDni.Size = new Size(118, 23);
        lblDni.Text = "DNI";
        lblDni.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // txtDni
        // 
        txtDni.BackColor = Color.White;
        txtDni.BorderStyle = BorderStyle.FixedSingle;
        txtDni.Location = new Point(140, 40);
        txtDni.Name = "txtDni";
        txtDni.Size = new Size(266, 23);
        // 
        // lblNombre
        // 
        lblNombre.Location = new Point(16, 84);
        lblNombre.Name = "lblNombre";
        lblNombre.Size = new Size(118, 23);
        lblNombre.Text = "Nombre";
        lblNombre.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // txtNombre
        // 
        txtNombre.BackColor = Color.White;
        txtNombre.BorderStyle = BorderStyle.FixedSingle;
        txtNombre.Location = new Point(140, 84);
        txtNombre.Name = "txtNombre";
        txtNombre.Size = new Size(266, 23);
        // 
        // lblApellido
        // 
        lblApellido.Location = new Point(16, 128);
        lblApellido.Name = "lblApellido";
        lblApellido.Size = new Size(118, 23);
        lblApellido.Text = "Apellido";
        lblApellido.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // txtApellido
        // 
        txtApellido.BackColor = Color.White;
        txtApellido.BorderStyle = BorderStyle.FixedSingle;
        txtApellido.Location = new Point(140, 128);
        txtApellido.Name = "txtApellido";
        txtApellido.Size = new Size(266, 23);
        // 
        // lblTelefono
        // 
        lblTelefono.Location = new Point(16, 172);
        lblTelefono.Name = "lblTelefono";
        lblTelefono.Size = new Size(118, 23);
        lblTelefono.Text = "Teléfono";
        lblTelefono.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // txtTelefono
        // 
        txtTelefono.BackColor = Color.White;
        txtTelefono.BorderStyle = BorderStyle.FixedSingle;
        txtTelefono.Location = new Point(140, 172);
        txtTelefono.Name = "txtTelefono";
        txtTelefono.Size = new Size(266, 23);
        // 
        // lblEmail
        // 
        lblEmail.Location = new Point(16, 216);
        lblEmail.Name = "lblEmail";
        lblEmail.Size = new Size(118, 23);
        lblEmail.Text = "Email";
        lblEmail.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // txtEmail
        // 
        txtEmail.BackColor = Color.White;
        txtEmail.BorderStyle = BorderStyle.FixedSingle;
        txtEmail.Location = new Point(140, 216);
        txtEmail.Name = "txtEmail";
        txtEmail.Size = new Size(266, 23);
        // 
        // lblDireccion
        // 
        lblDireccion.Location = new Point(16, 260);
        lblDireccion.Name = "lblDireccion";
        lblDireccion.Size = new Size(118, 23);
        lblDireccion.Text = "Dirección";
        lblDireccion.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // txtDireccion
        // 
        txtDireccion.BackColor = Color.White;
        txtDireccion.BorderStyle = BorderStyle.FixedSingle;
        txtDireccion.Location = new Point(140, 260);
        txtDireccion.Name = "txtDireccion";
        txtDireccion.Size = new Size(266, 23);
        // 
        // pnlBotones
        // 
        pnlBotones.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
        pnlBotones.Controls.Add(btnNuevo);
        pnlBotones.Controls.Add(btnGuardar);
        pnlBotones.Controls.Add(btnModificar);
        pnlBotones.Controls.Add(btnCancelar);
        pnlBotones.Controls.Add(btnVolver);
        pnlBotones.Location = new Point(16, 550);
        pnlBotones.Name = "pnlBotones";
        pnlBotones.Size = new Size(428, 84);
        pnlBotones.TabIndex = 1;
        // 
        // btnNuevo
        // 
        btnNuevo.BackColor = Color.FromArgb(230, 196, 202);
        btnNuevo.FlatAppearance.BorderColor = Color.FromArgb(186, 162, 168);
        btnNuevo.FlatStyle = FlatStyle.Flat;
        btnNuevo.ForeColor = Color.FromArgb(58, 53, 59);
        btnNuevo.Location = new Point(0, 4);
        btnNuevo.Name = "btnNuevo";
        btnNuevo.Size = new Size(100, 32);
        btnNuevo.Text = "Nuevo";
        btnNuevo.UseVisualStyleBackColor = false;
        // 
        // btnGuardar
        // 
        btnGuardar.BackColor = Color.FromArgb(72, 148, 96);
        btnGuardar.FlatAppearance.BorderColor = Color.FromArgb(48, 118, 74);
        btnGuardar.FlatStyle = FlatStyle.Flat;
        btnGuardar.ForeColor = Color.White;
        btnGuardar.Location = new Point(108, 4);
        btnGuardar.Name = "btnGuardar";
        btnGuardar.Size = new Size(100, 32);
        btnGuardar.Text = "Guardar";
        btnGuardar.UseVisualStyleBackColor = false;
        // 
        // btnModificar
        // 
        btnModificar.BackColor = Color.FromArgb(64, 126, 186);
        btnModificar.FlatAppearance.BorderColor = Color.FromArgb(44, 102, 158);
        btnModificar.FlatStyle = FlatStyle.Flat;
        btnModificar.ForeColor = Color.White;
        btnModificar.Location = new Point(216, 4);
        btnModificar.Name = "btnModificar";
        btnModificar.Size = new Size(100, 32);
        btnModificar.Text = "Modificar";
        btnModificar.UseVisualStyleBackColor = false;
        // 
        // btnCancelar
        // 
        btnCancelar.BackColor = Color.FromArgb(196, 78, 86);
        btnCancelar.FlatAppearance.BorderColor = Color.FromArgb(164, 54, 62);
        btnCancelar.FlatStyle = FlatStyle.Flat;
        btnCancelar.ForeColor = Color.White;
        btnCancelar.Location = new Point(324, 4);
        btnCancelar.Name = "btnCancelar";
        btnCancelar.Size = new Size(100, 32);
        btnCancelar.Text = "Cancelar";
        btnCancelar.UseVisualStyleBackColor = false;
        // 
        // btnVolver
        // 
        btnVolver.BackColor = Color.FromArgb(220, 200, 204);
        btnVolver.FlatAppearance.BorderColor = Color.FromArgb(186, 162, 168);
        btnVolver.FlatStyle = FlatStyle.Flat;
        btnVolver.ForeColor = Color.FromArgb(58, 53, 59);
        btnVolver.Location = new Point(0, 44);
        btnVolver.Name = "btnVolver";
        btnVolver.Size = new Size(208, 32);
        btnVolver.Text = "Volver al panel";
        btnVolver.UseVisualStyleBackColor = false;
        btnVolver.Click += btnVolver_Click;
        // 
        // pnlListado
        // 
        pnlListado.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        pnlListado.BorderStyle = BorderStyle.FixedSingle;
        pnlListado.Controls.Add(lblBuscar);
        pnlListado.Controls.Add(txtBuscar);
        pnlListado.Controls.Add(btnBuscar);
        pnlListado.Controls.Add(btnActivos);
        pnlListado.Controls.Add(btnInactivos);
        pnlListado.Controls.Add(dgvPropietarios);
        pnlListado.Location = new Point(456, 16);
        pnlListado.Name = "pnlListado";
        pnlListado.Size = new Size(628, 618);
        pnlListado.TabIndex = 2;
        // 
        // lblBuscar
        // 
        lblBuscar.Location = new Point(10, 12);
        lblBuscar.Name = "lblBuscar";
        lblBuscar.Size = new Size(52, 23);
        lblBuscar.Text = "Buscar";
        lblBuscar.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // txtBuscar
        // 
        txtBuscar.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
        txtBuscar.BackColor = Color.White;
        txtBuscar.BorderStyle = BorderStyle.FixedSingle;
        txtBuscar.Location = new Point(64, 12);
        txtBuscar.Name = "txtBuscar";
        txtBuscar.Size = new Size(230, 23);
        // 
        // btnBuscar
        // 
        btnBuscar.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        btnBuscar.BackColor = Color.FromArgb(220, 200, 204);
        btnBuscar.FlatAppearance.BorderColor = Color.FromArgb(186, 162, 168);
        btnBuscar.FlatStyle = FlatStyle.Flat;
        btnBuscar.ForeColor = Color.FromArgb(58, 53, 59);
        btnBuscar.Location = new Point(300, 10);
        btnBuscar.Name = "btnBuscar";
        btnBuscar.Size = new Size(90, 27);
        btnBuscar.Text = "Buscar";
        btnBuscar.UseVisualStyleBackColor = false;
        // 
        // btnActivos
        // 
        btnActivos.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        btnActivos.BackColor = Color.FromArgb(220, 200, 204);
        btnActivos.FlatAppearance.BorderColor = Color.FromArgb(186, 162, 168);
        btnActivos.FlatStyle = FlatStyle.Flat;
        btnActivos.ForeColor = Color.FromArgb(58, 53, 59);
        btnActivos.Location = new Point(396, 10);
        btnActivos.Name = "btnActivos";
        btnActivos.Size = new Size(104, 27);
        btnActivos.Text = "Activos";
        btnActivos.UseVisualStyleBackColor = false;
        // 
        // btnInactivos
        // 
        btnInactivos.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        btnInactivos.BackColor = Color.FromArgb(220, 200, 204);
        btnInactivos.FlatAppearance.BorderColor = Color.FromArgb(186, 162, 168);
        btnInactivos.FlatStyle = FlatStyle.Flat;
        btnInactivos.ForeColor = Color.FromArgb(58, 53, 59);
        btnInactivos.Location = new Point(506, 10);
        btnInactivos.Name = "btnInactivos";
        btnInactivos.Size = new Size(110, 27);
        btnInactivos.Text = "Inactivos";
        btnInactivos.UseVisualStyleBackColor = false;
        // 
        // dgvPropietarios
        // 
        dgvPropietarios.AllowUserToAddRows = false;
        dgvPropietarios.AllowUserToDeleteRows = false;
        dgvPropietarios.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        dgvPropietarios.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        dgvPropietarios.BackgroundColor = Color.White;
        dgvPropietarios.BorderStyle = BorderStyle.FixedSingle;
        dgvPropietarios.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
        dgvPropietarios.Columns.AddRange(new DataGridViewColumn[] { colId, colDni, colNombre, colApellido, colTelefono, colEmail, colDireccion, colEstado });
        dgvPropietarios.Location = new Point(10, 48);
        dgvPropietarios.MultiSelect = false;
        dgvPropietarios.Name = "dgvPropietarios";
        dgvPropietarios.ReadOnly = true;
        dgvPropietarios.RowHeadersVisible = false;
        dgvPropietarios.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        dgvPropietarios.Size = new Size(606, 558);
        // 
        // columnas
        // 
        colId.HeaderText = "ID";
        colId.Name = "colId";
        colDni.HeaderText = "DNI";
        colDni.Name = "colDni";
        colNombre.HeaderText = "Nombre";
        colNombre.Name = "colNombre";
        colApellido.HeaderText = "Apellido";
        colApellido.Name = "colApellido";
        colTelefono.HeaderText = "Teléfono";
        colTelefono.Name = "colTelefono";
        colEmail.HeaderText = "Email";
        colEmail.Name = "colEmail";
        colDireccion.HeaderText = "Dirección";
        colDireccion.Name = "colDireccion";
        colEstado.HeaderText = "Estado";
        colEstado.Name = "colEstado";
        // 
        // FormPropietarios
        // 
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        BackColor = Color.FromArgb(250, 244, 244);
        ClientSize = new Size(1100, 700);
        Controls.Add(pnlContenido);
        Controls.Add(pnlHeader);
        Font = new Font("Segoe UI", 9F);
        ForeColor = Color.FromArgb(58, 53, 59);
        MinimumSize = new Size(1100, 700);
        Name = "FormPropietarios";
        StartPosition = FormStartPosition.CenterParent;
        Text = "CLÍNICA VETERINARIA — ADMINISTRADOR — GESTIÓN DE PROPIETARIOS";
        Load += FormPropietarios_Load;
        pnlHeader.ResumeLayout(false);
        pnlContenido.ResumeLayout(false);
        grpDatos.ResumeLayout(false);
        grpDatos.PerformLayout();
        pnlBotones.ResumeLayout(false);
        pnlListado.ResumeLayout(false);
        pnlListado.PerformLayout();
        ((System.ComponentModel.ISupportInitialize)dgvPropietarios).EndInit();
        ResumeLayout(false);
    }

    private Panel pnlHeader;
    private Label lblTitulo;
    private Label lblUsuarioSesion;
    private Panel pnlContenido;
    private GroupBox grpDatos;
    private Label lblDni;
    private TextBox txtDni;
    private Label lblNombre;
    private TextBox txtNombre;
    private Label lblApellido;
    private TextBox txtApellido;
    private Label lblTelefono;
    private TextBox txtTelefono;
    private Label lblEmail;
    private TextBox txtEmail;
    private Label lblDireccion;
    private TextBox txtDireccion;
    private Panel pnlBotones;
    private Button btnNuevo;
    private Button btnGuardar;
    private Button btnModificar;
    private Button btnCancelar;
    private Button btnVolver;
    private Panel pnlListado;
    private Label lblBuscar;
    private TextBox txtBuscar;
    private Button btnBuscar;
    private Button btnActivos;
    private Button btnInactivos;
    private DataGridView dgvPropietarios;
    private DataGridViewTextBoxColumn colId;
    private DataGridViewTextBoxColumn colDni;
    private DataGridViewTextBoxColumn colNombre;
    private DataGridViewTextBoxColumn colApellido;
    private DataGridViewTextBoxColumn colTelefono;
    private DataGridViewTextBoxColumn colEmail;
    private DataGridViewTextBoxColumn colDireccion;
    private DataGridViewTextBoxColumn colEstado;
}
