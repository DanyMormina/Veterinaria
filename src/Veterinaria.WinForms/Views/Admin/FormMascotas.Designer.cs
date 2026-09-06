namespace Veterinaria.WinForms.Views.Admin;

partial class FormMascotas
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
        lblNombre = new Label();
        txtNombre = new TextBox();
        lblPropietario = new Label();
        cboPropietario = new ComboBox();
        lblEspecie = new Label();
        cboEspecie = new ComboBox();
        lblRaza = new Label();
        cboRaza = new ComboBox();
        lblSexo = new Label();
        cboSexo = new ComboBox();
        lblFechaNacimiento = new Label();
        dtpFechaNacimiento = new DateTimePicker();
        lblColor = new Label();
        txtColor = new TextBox();
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
        lblFiltroEspecie = new Label();
        cboFiltroEspecie = new ComboBox();
        btnActivas = new Button();
        btnInactivas = new Button();
        dgvMascotas = new DataGridView();
        colId = new DataGridViewTextBoxColumn();
        colNombre = new DataGridViewTextBoxColumn();
        colPropietario = new DataGridViewTextBoxColumn();
        colEspecie = new DataGridViewTextBoxColumn();
        colRaza = new DataGridViewTextBoxColumn();
        colSexo = new DataGridViewTextBoxColumn();
        colFechaNacimiento = new DataGridViewTextBoxColumn();
        colColor = new DataGridViewTextBoxColumn();
        colEstado = new DataGridViewTextBoxColumn();
        pnlHeader.SuspendLayout();
        pnlContenido.SuspendLayout();
        grpDatos.SuspendLayout();
        pnlBotones.SuspendLayout();
        pnlListado.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)dgvMascotas).BeginInit();
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
        lblTitulo.Text = "CLÍNICA VETERINARIA — ADMINISTRADOR — GESTIÓN DE MASCOTAS";
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
        grpDatos.Controls.Add(lblNombre);
        grpDatos.Controls.Add(txtNombre);
        grpDatos.Controls.Add(lblPropietario);
        grpDatos.Controls.Add(cboPropietario);
        grpDatos.Controls.Add(lblEspecie);
        grpDatos.Controls.Add(cboEspecie);
        grpDatos.Controls.Add(lblRaza);
        grpDatos.Controls.Add(cboRaza);
        grpDatos.Controls.Add(lblSexo);
        grpDatos.Controls.Add(cboSexo);
        grpDatos.Controls.Add(lblFechaNacimiento);
        grpDatos.Controls.Add(dtpFechaNacimiento);
        grpDatos.Controls.Add(lblColor);
        grpDatos.Controls.Add(txtColor);
        grpDatos.Font = new Font("Segoe UI", 9F);
        grpDatos.ForeColor = Color.FromArgb(58, 53, 59);
        grpDatos.Location = new Point(16, 16);
        grpDatos.Name = "grpDatos";
        grpDatos.Size = new Size(428, 528);
        grpDatos.TabIndex = 0;
        grpDatos.TabStop = false;
        grpDatos.Text = "Datos de la mascota";
        // 
        // lblNombre
        // 
        lblNombre.Location = new Point(16, 40);
        lblNombre.Name = "lblNombre";
        lblNombre.Size = new Size(118, 23);
        lblNombre.Text = "Nombre";
        lblNombre.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // txtNombre
        // 
        txtNombre.BackColor = Color.White;
        txtNombre.BorderStyle = BorderStyle.FixedSingle;
        txtNombre.Location = new Point(140, 40);
        txtNombre.Name = "txtNombre";
        txtNombre.Size = new Size(266, 23);
        // 
        // lblPropietario
        // 
        lblPropietario.Location = new Point(16, 84);
        lblPropietario.Name = "lblPropietario";
        lblPropietario.Size = new Size(118, 23);
        lblPropietario.Text = "Propietario";
        lblPropietario.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // cboPropietario
        // 
        cboPropietario.BackColor = Color.White;
        cboPropietario.DropDownStyle = ComboBoxStyle.DropDownList;
        cboPropietario.FormattingEnabled = true;
        cboPropietario.Location = new Point(140, 84);
        cboPropietario.Name = "cboPropietario";
        cboPropietario.Size = new Size(266, 23);
        // 
        // lblEspecie
        // 
        lblEspecie.Location = new Point(16, 128);
        lblEspecie.Name = "lblEspecie";
        lblEspecie.Size = new Size(118, 23);
        lblEspecie.Text = "Especie";
        lblEspecie.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // cboEspecie
        // 
        cboEspecie.BackColor = Color.White;
        cboEspecie.DropDownStyle = ComboBoxStyle.DropDownList;
        cboEspecie.FormattingEnabled = true;
        cboEspecie.Location = new Point(140, 128);
        cboEspecie.Name = "cboEspecie";
        cboEspecie.Size = new Size(266, 23);
        // 
        // lblRaza
        // 
        lblRaza.Location = new Point(16, 172);
        lblRaza.Name = "lblRaza";
        lblRaza.Size = new Size(118, 23);
        lblRaza.Text = "Raza";
        lblRaza.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // cboRaza
        // 
        cboRaza.BackColor = Color.White;
        cboRaza.DropDownStyle = ComboBoxStyle.DropDownList;
        cboRaza.FormattingEnabled = true;
        cboRaza.Location = new Point(140, 172);
        cboRaza.Name = "cboRaza";
        cboRaza.Size = new Size(266, 23);
        // 
        // lblSexo
        // 
        lblSexo.Location = new Point(16, 216);
        lblSexo.Name = "lblSexo";
        lblSexo.Size = new Size(118, 23);
        lblSexo.Text = "Sexo";
        lblSexo.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // cboSexo
        // 
        cboSexo.BackColor = Color.White;
        cboSexo.DropDownStyle = ComboBoxStyle.DropDownList;
        cboSexo.FormattingEnabled = true;
        cboSexo.Items.AddRange(new object[] { "Macho", "Hembra" });
        cboSexo.Location = new Point(140, 216);
        cboSexo.Name = "cboSexo";
        cboSexo.Size = new Size(266, 23);
        // 
        // lblFechaNacimiento
        // 
        lblFechaNacimiento.Location = new Point(16, 260);
        lblFechaNacimiento.Name = "lblFechaNacimiento";
        lblFechaNacimiento.Size = new Size(118, 23);
        lblFechaNacimiento.Text = "Fecha de nacimiento";
        lblFechaNacimiento.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // dtpFechaNacimiento
        // 
        dtpFechaNacimiento.Format = DateTimePickerFormat.Short;
        dtpFechaNacimiento.Location = new Point(140, 260);
        dtpFechaNacimiento.Name = "dtpFechaNacimiento";
        dtpFechaNacimiento.Size = new Size(266, 23);
        // 
        // lblColor
        // 
        lblColor.Location = new Point(16, 304);
        lblColor.Name = "lblColor";
        lblColor.Size = new Size(118, 23);
        lblColor.Text = "Color";
        lblColor.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // txtColor
        // 
        txtColor.BackColor = Color.White;
        txtColor.BorderStyle = BorderStyle.FixedSingle;
        txtColor.Location = new Point(140, 304);
        txtColor.Name = "txtColor";
        txtColor.Size = new Size(266, 23);
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
        pnlListado.Controls.Add(lblFiltroEspecie);
        pnlListado.Controls.Add(cboFiltroEspecie);
        pnlListado.Controls.Add(btnActivas);
        pnlListado.Controls.Add(btnInactivas);
        pnlListado.Controls.Add(dgvMascotas);
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
        txtBuscar.BackColor = Color.White;
        txtBuscar.BorderStyle = BorderStyle.FixedSingle;
        txtBuscar.Location = new Point(64, 12);
        txtBuscar.Name = "txtBuscar";
        txtBuscar.Size = new Size(150, 23);
        // 
        // btnBuscar
        // 
        btnBuscar.BackColor = Color.FromArgb(220, 200, 204);
        btnBuscar.FlatAppearance.BorderColor = Color.FromArgb(186, 162, 168);
        btnBuscar.FlatStyle = FlatStyle.Flat;
        btnBuscar.ForeColor = Color.FromArgb(58, 53, 59);
        btnBuscar.Location = new Point(220, 10);
        btnBuscar.Name = "btnBuscar";
        btnBuscar.Size = new Size(78, 27);
        btnBuscar.Text = "Buscar";
        btnBuscar.UseVisualStyleBackColor = false;
        // 
        // lblFiltroEspecie
        // 
        lblFiltroEspecie.Location = new Point(10, 46);
        lblFiltroEspecie.Name = "lblFiltroEspecie";
        lblFiltroEspecie.Size = new Size(52, 23);
        lblFiltroEspecie.Text = "Especie";
        lblFiltroEspecie.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // cboFiltroEspecie
        // 
        cboFiltroEspecie.BackColor = Color.White;
        cboFiltroEspecie.DropDownStyle = ComboBoxStyle.DropDownList;
        cboFiltroEspecie.FormattingEnabled = true;
        cboFiltroEspecie.Location = new Point(64, 46);
        cboFiltroEspecie.Name = "cboFiltroEspecie";
        cboFiltroEspecie.Size = new Size(150, 23);
        // 
        // btnActivas
        // 
        btnActivas.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        btnActivas.BackColor = Color.FromArgb(220, 200, 204);
        btnActivas.FlatAppearance.BorderColor = Color.FromArgb(186, 162, 168);
        btnActivas.FlatStyle = FlatStyle.Flat;
        btnActivas.ForeColor = Color.FromArgb(58, 53, 59);
        btnActivas.Location = new Point(396, 10);
        btnActivas.Name = "btnActivas";
        btnActivas.Size = new Size(104, 27);
        btnActivas.Text = "Activas";
        btnActivas.UseVisualStyleBackColor = false;
        // 
        // btnInactivas
        // 
        btnInactivas.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        btnInactivas.BackColor = Color.FromArgb(220, 200, 204);
        btnInactivas.FlatAppearance.BorderColor = Color.FromArgb(186, 162, 168);
        btnInactivas.FlatStyle = FlatStyle.Flat;
        btnInactivas.ForeColor = Color.FromArgb(58, 53, 59);
        btnInactivas.Location = new Point(506, 10);
        btnInactivas.Name = "btnInactivas";
        btnInactivas.Size = new Size(110, 27);
        btnInactivas.Text = "Inactivas";
        btnInactivas.UseVisualStyleBackColor = false;
        // 
        // dgvMascotas
        // 
        dgvMascotas.AllowUserToAddRows = false;
        dgvMascotas.AllowUserToDeleteRows = false;
        dgvMascotas.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        dgvMascotas.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        dgvMascotas.BackgroundColor = Color.White;
        dgvMascotas.BorderStyle = BorderStyle.FixedSingle;
        dgvMascotas.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
        dgvMascotas.Columns.AddRange(new DataGridViewColumn[] { colId, colNombre, colPropietario, colEspecie, colRaza, colSexo, colFechaNacimiento, colColor, colEstado });
        dgvMascotas.Location = new Point(10, 80);
        dgvMascotas.MultiSelect = false;
        dgvMascotas.Name = "dgvMascotas";
        dgvMascotas.ReadOnly = true;
        dgvMascotas.RowHeadersVisible = false;
        dgvMascotas.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        dgvMascotas.Size = new Size(606, 526);
        // 
        // columnas
        // 
        colId.HeaderText = "ID";
        colId.Name = "colId";
        colNombre.HeaderText = "Nombre";
        colNombre.Name = "colNombre";
        colPropietario.HeaderText = "Propietario";
        colPropietario.Name = "colPropietario";
        colEspecie.HeaderText = "Especie";
        colEspecie.Name = "colEspecie";
        colRaza.HeaderText = "Raza";
        colRaza.Name = "colRaza";
        colSexo.HeaderText = "Sexo";
        colSexo.Name = "colSexo";
        colFechaNacimiento.HeaderText = "Fecha de nacimiento";
        colFechaNacimiento.Name = "colFechaNacimiento";
        colColor.HeaderText = "Color";
        colColor.Name = "colColor";
        colEstado.HeaderText = "Estado";
        colEstado.Name = "colEstado";
        // 
        // FormMascotas
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
        Name = "FormMascotas";
        StartPosition = FormStartPosition.CenterParent;
        Text = "CLÍNICA VETERINARIA — ADMINISTRADOR — GESTIÓN DE MASCOTAS";
        Load += FormMascotas_Load;
        pnlHeader.ResumeLayout(false);
        pnlContenido.ResumeLayout(false);
        grpDatos.ResumeLayout(false);
        grpDatos.PerformLayout();
        pnlBotones.ResumeLayout(false);
        pnlListado.ResumeLayout(false);
        pnlListado.PerformLayout();
        ((System.ComponentModel.ISupportInitialize)dgvMascotas).EndInit();
        ResumeLayout(false);
    }

    private Panel pnlHeader;
    private Label lblTitulo;
    private Label lblUsuarioSesion;
    private Panel pnlContenido;
    private GroupBox grpDatos;
    private Label lblNombre;
    private TextBox txtNombre;
    private Label lblPropietario;
    private ComboBox cboPropietario;
    private Label lblEspecie;
    private ComboBox cboEspecie;
    private Label lblRaza;
    private ComboBox cboRaza;
    private Label lblSexo;
    private ComboBox cboSexo;
    private Label lblFechaNacimiento;
    private DateTimePicker dtpFechaNacimiento;
    private Label lblColor;
    private TextBox txtColor;
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
    private Label lblFiltroEspecie;
    private ComboBox cboFiltroEspecie;
    private Button btnActivas;
    private Button btnInactivas;
    private DataGridView dgvMascotas;
    private DataGridViewTextBoxColumn colId;
    private DataGridViewTextBoxColumn colNombre;
    private DataGridViewTextBoxColumn colPropietario;
    private DataGridViewTextBoxColumn colEspecie;
    private DataGridViewTextBoxColumn colRaza;
    private DataGridViewTextBoxColumn colSexo;
    private DataGridViewTextBoxColumn colFechaNacimiento;
    private DataGridViewTextBoxColumn colColor;
    private DataGridViewTextBoxColumn colEstado;
}
