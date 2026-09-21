namespace Veterinaria.WinForms.Vistas.Administrador;

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
        DataGridViewCellStyle dataGridViewCellStyle3 = new DataGridViewCellStyle();
        DataGridViewCellStyle dataGridViewCellStyle4 = new DataGridViewCellStyle();
        pnlEncabezado = new Panel();
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
        lblCorreoElectronico = new Label();
        txtCorreoElectronico = new TextBox();
        lblDireccion = new Label();
        txtDireccion = new TextBox();
        lblEstado = new Label();
        cboEstado = new ComboBox();
        grpMascotas = new GroupBox();
        dgvMascotasPropietario = new DataGridView();
        colMascotaId = new DataGridViewTextBoxColumn();
        colMascotaNombre = new DataGridViewTextBoxColumn();
        colMascotaEspecie = new DataGridViewTextBoxColumn();
        colMascotaRaza = new DataGridViewTextBoxColumn();
        colMascotaSexo = new DataGridViewTextBoxColumn();
        colMascotaEstado = new DataGridViewTextBoxColumn();
        btnBuscar = new Button();
        btnLimpiar = new Button();
        btnVolver = new Button();
        pnlListado = new Panel();
        lblBuscar = new Label();
        txtBuscar = new TextBox();
        lblFiltroEstado = new Label();
        cboFiltroEstado = new ComboBox();
        dgvPropietarios = new DataGridView();
        colId = new DataGridViewTextBoxColumn();
        colNombre = new DataGridViewTextBoxColumn();
        colApellido = new DataGridViewTextBoxColumn();
        colDni = new DataGridViewTextBoxColumn();
        colTelefono = new DataGridViewTextBoxColumn();
        colCorreoElectronico = new DataGridViewTextBoxColumn();
        colDireccion = new DataGridViewTextBoxColumn();
        colEstado = new DataGridViewTextBoxColumn();
        pnlEncabezado.SuspendLayout();
        pnlContenido.SuspendLayout();
        grpDatos.SuspendLayout();
        grpMascotas.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)dgvMascotasPropietario).BeginInit();
        pnlListado.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)dgvPropietarios).BeginInit();
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
        pnlEncabezado.Size = new Size(1220, 48);
        pnlEncabezado.TabIndex = 0;
        // 
        // lblTitulo
        // 
        lblTitulo.Dock = DockStyle.Left;
        lblTitulo.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
        lblTitulo.ForeColor = Color.White;
        lblTitulo.Location = new Point(16, 0);
        lblTitulo.Name = "lblTitulo";
        lblTitulo.Size = new Size(680, 48);
        lblTitulo.TabIndex = 0;
        lblTitulo.Text = "CLÍNICA VETERINARIA — GESTIÓN DE PROPIETARIOS";
        lblTitulo.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // lblUsuarioSesion
        // 
        lblUsuarioSesion.Dock = DockStyle.Right;
        lblUsuarioSesion.Font = new Font("Segoe UI", 9.75F);
        lblUsuarioSesion.ForeColor = Color.FromArgb(250, 244, 244);
        lblUsuarioSesion.Location = new Point(804, 0);
        lblUsuarioSesion.Name = "lblUsuarioSesion";
        lblUsuarioSesion.Size = new Size(400, 48);
        lblUsuarioSesion.TabIndex = 1;
        lblUsuarioSesion.Text = "Usuario: Admin";
        lblUsuarioSesion.TextAlign = ContentAlignment.MiddleRight;
        // 
        // pnlContenido
        // 
        pnlContenido.BackColor = Color.FromArgb(250, 244, 244);
        pnlContenido.Controls.Add(grpDatos);
        pnlContenido.Controls.Add(grpMascotas);
        pnlContenido.Controls.Add(btnBuscar);
        pnlContenido.Controls.Add(btnLimpiar);
        pnlContenido.Controls.Add(btnVolver);
        pnlContenido.Controls.Add(pnlListado);
        pnlContenido.Dock = DockStyle.Fill;
        pnlContenido.Location = new Point(0, 48);
        pnlContenido.Name = "pnlContenido";
        pnlContenido.Size = new Size(1220, 672);
        pnlContenido.TabIndex = 1;
        // 
        // grpDatos
        // 
        grpDatos.Controls.Add(lblNombre);
        grpDatos.Controls.Add(txtNombre);
        grpDatos.Controls.Add(lblApellido);
        grpDatos.Controls.Add(txtApellido);
        grpDatos.Controls.Add(lblDni);
        grpDatos.Controls.Add(txtDni);
        grpDatos.Controls.Add(lblTelefono);
        grpDatos.Controls.Add(txtTelefono);
        grpDatos.Controls.Add(lblCorreoElectronico);
        grpDatos.Controls.Add(txtCorreoElectronico);
        grpDatos.Controls.Add(lblDireccion);
        grpDatos.Controls.Add(txtDireccion);
        grpDatos.Controls.Add(lblEstado);
        grpDatos.Controls.Add(cboEstado);
        grpDatos.Font = new Font("Segoe UI", 9F);
        grpDatos.ForeColor = Color.FromArgb(58, 53, 59);
        grpDatos.Location = new Point(16, 16);
        grpDatos.Name = "grpDatos";
        grpDatos.Size = new Size(410, 290);
        grpDatos.TabIndex = 0;
        grpDatos.TabStop = false;
        grpDatos.Text = "Datos del propietario";
        // 
        // lblNombre
        // 
        lblNombre.Location = new Point(16, 28);
        lblNombre.Name = "lblNombre";
        lblNombre.Size = new Size(106, 23);
        lblNombre.TabIndex = 0;
        lblNombre.Text = "Nombre";
        lblNombre.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // txtNombre
        // 
        txtNombre.BackColor = Color.White;
        txtNombre.BorderStyle = BorderStyle.FixedSingle;
        txtNombre.Location = new Point(128, 28);
        txtNombre.MaxLength = 100;
        txtNombre.Name = "txtNombre";
        txtNombre.Size = new Size(266, 23);
        txtNombre.TabIndex = 1;
        // 
        // lblApellido
        // 
        lblApellido.Location = new Point(16, 64);
        lblApellido.Name = "lblApellido";
        lblApellido.Size = new Size(106, 23);
        lblApellido.TabIndex = 2;
        lblApellido.Text = "Apellido";
        lblApellido.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // txtApellido
        // 
        txtApellido.BackColor = Color.White;
        txtApellido.BorderStyle = BorderStyle.FixedSingle;
        txtApellido.Location = new Point(128, 64);
        txtApellido.MaxLength = 100;
        txtApellido.Name = "txtApellido";
        txtApellido.Size = new Size(266, 23);
        txtApellido.TabIndex = 3;
        // 
        // lblDni
        // 
        lblDni.Location = new Point(16, 100);
        lblDni.Name = "lblDni";
        lblDni.Size = new Size(106, 23);
        lblDni.TabIndex = 4;
        lblDni.Text = "DNI";
        lblDni.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // txtDni
        // 
        txtDni.BackColor = Color.White;
        txtDni.BorderStyle = BorderStyle.FixedSingle;
        txtDni.Location = new Point(128, 100);
        txtDni.MaxLength = 8;
        txtDni.Name = "txtDni";
        txtDni.Size = new Size(266, 23);
        txtDni.TabIndex = 5;
        // 
        // lblTelefono
        // 
        lblTelefono.Location = new Point(16, 136);
        lblTelefono.Name = "lblTelefono";
        lblTelefono.Size = new Size(106, 23);
        lblTelefono.TabIndex = 6;
        lblTelefono.Text = "Teléfono";
        lblTelefono.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // txtTelefono
        // 
        txtTelefono.BackColor = Color.White;
        txtTelefono.BorderStyle = BorderStyle.FixedSingle;
        txtTelefono.Location = new Point(128, 136);
        txtTelefono.MaxLength = 13;
        txtTelefono.Name = "txtTelefono";
        txtTelefono.Size = new Size(266, 23);
        txtTelefono.TabIndex = 7;
        // 
        // lblCorreoElectronico
        // 
        lblCorreoElectronico.Location = new Point(16, 172);
        lblCorreoElectronico.Name = "lblCorreoElectronico";
        lblCorreoElectronico.Size = new Size(106, 23);
        lblCorreoElectronico.TabIndex = 8;
        lblCorreoElectronico.Text = "Correo";
        lblCorreoElectronico.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // txtCorreoElectronico
        // 
        txtCorreoElectronico.BackColor = Color.White;
        txtCorreoElectronico.BorderStyle = BorderStyle.FixedSingle;
        txtCorreoElectronico.Location = new Point(128, 172);
        txtCorreoElectronico.MaxLength = 150;
        txtCorreoElectronico.Name = "txtCorreoElectronico";
        txtCorreoElectronico.Size = new Size(266, 23);
        txtCorreoElectronico.TabIndex = 9;
        // 
        // lblDireccion
        // 
        lblDireccion.Location = new Point(16, 208);
        lblDireccion.Name = "lblDireccion";
        lblDireccion.Size = new Size(106, 23);
        lblDireccion.TabIndex = 10;
        lblDireccion.Text = "Dirección";
        lblDireccion.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // txtDireccion
        // 
        txtDireccion.BackColor = Color.White;
        txtDireccion.BorderStyle = BorderStyle.FixedSingle;
        txtDireccion.Location = new Point(128, 208);
        txtDireccion.MaxLength = 200;
        txtDireccion.Name = "txtDireccion";
        txtDireccion.Size = new Size(266, 23);
        txtDireccion.TabIndex = 11;
        // 
        // lblEstado
        // 
        lblEstado.Location = new Point(16, 244);
        lblEstado.Name = "lblEstado";
        lblEstado.Size = new Size(106, 23);
        lblEstado.TabIndex = 12;
        lblEstado.Text = "Estado";
        lblEstado.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // cboEstado
        // 
        cboEstado.BackColor = Color.White;
        cboEstado.DropDownStyle = ComboBoxStyle.DropDownList;
        cboEstado.FormattingEnabled = true;
        cboEstado.Items.AddRange(new object[] { "Activo", "Inactivo" });
        cboEstado.Location = new Point(128, 244);
        cboEstado.Name = "cboEstado";
        cboEstado.Size = new Size(266, 23);
        cboEstado.TabIndex = 13;
        // 
        // grpMascotas
        // 
        grpMascotas.Controls.Add(dgvMascotasPropietario);
        grpMascotas.Font = new Font("Segoe UI", 9F);
        grpMascotas.ForeColor = Color.FromArgb(58, 53, 59);
        grpMascotas.Location = new Point(16, 314);
        grpMascotas.Name = "grpMascotas";
        grpMascotas.Size = new Size(410, 295);
        grpMascotas.TabIndex = 1;
        grpMascotas.TabStop = false;
        grpMascotas.Text = "Mascotas registradas del propietario";
        // 
        // dgvMascotasPropietario
        // 
        dgvMascotasPropietario.AllowUserToAddRows = false;
        dgvMascotasPropietario.AllowUserToDeleteRows = false;
        dgvMascotasPropietario.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        dgvMascotasPropietario.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        dgvMascotasPropietario.BackgroundColor = Color.White;
        dgvMascotasPropietario.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
        dgvMascotasPropietario.Columns.AddRange(new DataGridViewColumn[] { colMascotaId, colMascotaNombre, colMascotaEspecie, colMascotaRaza, colMascotaSexo, colMascotaEstado });
        dgvMascotasPropietario.Location = new Point(10, 24);
        dgvMascotasPropietario.MultiSelect = false;
        dgvMascotasPropietario.Name = "dgvMascotasPropietario";
        dgvMascotasPropietario.ReadOnly = true;
        dgvMascotasPropietario.RowHeadersVisible = false;
        dgvMascotasPropietario.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        dgvMascotasPropietario.Size = new Size(390, 258);
        dgvMascotasPropietario.TabIndex = 0;
        // 
        // colMascotaId
        // 
        colMascotaId.HeaderText = "ID";
        colMascotaId.Name = "colMascotaId";
        colMascotaId.ReadOnly = true;
        colMascotaId.Visible = false;
        // 
        // colMascotaNombre
        // 
        colMascotaNombre.FillWeight = 85F;
        colMascotaNombre.HeaderText = "Nombre";
        colMascotaNombre.MinimumWidth = 65;
        colMascotaNombre.Name = "colMascotaNombre";
        colMascotaNombre.ReadOnly = true;
        // 
        // colMascotaEspecie
        // 
        colMascotaEspecie.FillWeight = 75F;
        colMascotaEspecie.HeaderText = "Especie";
        colMascotaEspecie.MinimumWidth = 60;
        colMascotaEspecie.Name = "colMascotaEspecie";
        colMascotaEspecie.ReadOnly = true;
        // 
        // colMascotaRaza
        // 
        colMascotaRaza.FillWeight = 80F;
        colMascotaRaza.HeaderText = "Raza";
        colMascotaRaza.MinimumWidth = 65;
        colMascotaRaza.Name = "colMascotaRaza";
        colMascotaRaza.ReadOnly = true;
        // 
        // colMascotaSexo
        // 
        colMascotaSexo.FillWeight = 60F;
        colMascotaSexo.HeaderText = "Sexo";
        colMascotaSexo.MinimumWidth = 50;
        colMascotaSexo.Name = "colMascotaSexo";
        colMascotaSexo.ReadOnly = true;
        // 
        // colMascotaEstado
        // 
        colMascotaEstado.FillWeight = 60F;
        colMascotaEstado.HeaderText = "Estado";
        colMascotaEstado.MinimumWidth = 50;
        colMascotaEstado.Name = "colMascotaEstado";
        colMascotaEstado.ReadOnly = true;
        // 
        // btnBuscar
        // 
        btnBuscar.BackColor = Color.FromArgb(200, 138, 150);
        btnBuscar.Cursor = Cursors.Hand;
        btnBuscar.FlatAppearance.BorderSize = 0;
        btnBuscar.FlatStyle = FlatStyle.Flat;
        btnBuscar.Font = new Font("Segoe UI", 9F);
        btnBuscar.ForeColor = Color.Black;
        btnBuscar.Location = new Point(16, 624);
        btnBuscar.Name = "btnBuscar";
        btnBuscar.Size = new Size(125, 32);
        btnBuscar.TabIndex = 4;
        btnBuscar.Text = "Buscar";
        btnBuscar.UseVisualStyleBackColor = false;
        btnBuscar.Click += btnBuscar_Click;
        // 
        // btnLimpiar
        // 
        btnLimpiar.BackColor = Color.FromArgb(226, 217, 220);
        btnLimpiar.Cursor = Cursors.Hand;
        btnLimpiar.FlatAppearance.BorderSize = 0;
        btnLimpiar.FlatStyle = FlatStyle.Flat;
        btnLimpiar.Font = new Font("Segoe UI", 9F);
        btnLimpiar.ForeColor = Color.FromArgb(58, 53, 59);
        btnLimpiar.Location = new Point(151, 624);
        btnLimpiar.Name = "btnLimpiar";
        btnLimpiar.Size = new Size(125, 32);
        btnLimpiar.TabIndex = 5;
        btnLimpiar.Text = "Limpiar";
        btnLimpiar.UseVisualStyleBackColor = false;
        btnLimpiar.Click += btnLimpiar_Click;
        // 
        // btnVolver
        // 
        btnVolver.BackColor = Color.FromArgb(220, 200, 204);
        btnVolver.Cursor = Cursors.Hand;
        btnVolver.FlatAppearance.BorderSize = 0;
        btnVolver.FlatStyle = FlatStyle.Flat;
        btnVolver.Font = new Font("Segoe UI", 9F);
        btnVolver.ForeColor = Color.Black;
        btnVolver.Location = new Point(286, 624);
        btnVolver.Name = "btnVolver";
        btnVolver.Size = new Size(140, 32);
        btnVolver.TabIndex = 6;
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
        pnlListado.Controls.Add(lblFiltroEstado);
        pnlListado.Controls.Add(cboFiltroEstado);
        pnlListado.Controls.Add(dgvPropietarios);
        pnlListado.Location = new Point(444, 16);
        pnlListado.Name = "pnlListado";
        pnlListado.Size = new Size(760, 638);
        pnlListado.TabIndex = 2;
        // 
        // lblBuscar
        // 
        lblBuscar.Location = new Point(19, 14);
        lblBuscar.Name = "lblBuscar";
        lblBuscar.Size = new Size(48, 23);
        lblBuscar.TabIndex = 0;
        lblBuscar.Text = "Buscar";
        lblBuscar.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // txtBuscar
        // 
        txtBuscar.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
        txtBuscar.BackColor = Color.White;
        txtBuscar.BorderStyle = BorderStyle.FixedSingle;
        txtBuscar.Location = new Point(69, 14);
        txtBuscar.Name = "txtBuscar";
        txtBuscar.Size = new Size(475, 23);
        txtBuscar.TabIndex = 1;
        // 
        // lblFiltroEstado
        // 
        lblFiltroEstado.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        lblFiltroEstado.Location = new Point(555, 14);
        lblFiltroEstado.Name = "lblFiltroEstado";
        lblFiltroEstado.Size = new Size(48, 23);
        lblFiltroEstado.TabIndex = 2;
        lblFiltroEstado.Text = "Estado";
        lblFiltroEstado.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // cboFiltroEstado
        // 
        cboFiltroEstado.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        cboFiltroEstado.BackColor = Color.White;
        cboFiltroEstado.DropDownStyle = ComboBoxStyle.DropDownList;
        cboFiltroEstado.FormattingEnabled = true;
        cboFiltroEstado.Items.AddRange(new object[] { "(Todos)", "Solo Activos", "Solo Inactivos" });
        cboFiltroEstado.Location = new Point(608, 14);
        cboFiltroEstado.Name = "cboFiltroEstado";
        cboFiltroEstado.Size = new Size(130, 23);
        cboFiltroEstado.TabIndex = 3;
        // 
        // dgvPropietarios
        // 
        dgvPropietarios.AllowUserToAddRows = false;
        dgvPropietarios.AllowUserToDeleteRows = false;
        dgvPropietarios.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        dgvPropietarios.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        dgvPropietarios.BackgroundColor = Color.White;
        dataGridViewCellStyle3.Alignment = DataGridViewContentAlignment.MiddleLeft;
        dataGridViewCellStyle3.BackColor = SystemColors.Control;
        dataGridViewCellStyle3.Font = new Font("Segoe UI", 9F);
        dataGridViewCellStyle3.ForeColor = SystemColors.WindowText;
        dataGridViewCellStyle3.SelectionBackColor = SystemColors.Highlight;
        dataGridViewCellStyle3.SelectionForeColor = SystemColors.HighlightText;
        dataGridViewCellStyle3.WrapMode = DataGridViewTriState.True;
        dgvPropietarios.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle3;
        dgvPropietarios.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
        dgvPropietarios.Columns.AddRange(new DataGridViewColumn[] { colId, colNombre, colApellido, colDni, colTelefono, colCorreoElectronico, colDireccion, colEstado });
        dataGridViewCellStyle4.Alignment = DataGridViewContentAlignment.MiddleLeft;
        dataGridViewCellStyle4.BackColor = SystemColors.Window;
        dataGridViewCellStyle4.Font = new Font("Segoe UI", 9F);
        dataGridViewCellStyle4.ForeColor = Color.FromArgb(58, 53, 59);
        dataGridViewCellStyle4.SelectionBackColor = SystemColors.Highlight;
        dataGridViewCellStyle4.SelectionForeColor = SystemColors.HighlightText;
        dataGridViewCellStyle4.WrapMode = DataGridViewTriState.False;
        dgvPropietarios.DefaultCellStyle = dataGridViewCellStyle4;
        dgvPropietarios.Location = new Point(19, 48);
        dgvPropietarios.MultiSelect = false;
        dgvPropietarios.Name = "dgvPropietarios";
        dgvPropietarios.ReadOnly = true;
        dgvPropietarios.RowHeadersVisible = false;
        dgvPropietarios.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        dgvPropietarios.Size = new Size(720, 575);
        dgvPropietarios.TabIndex = 4;
        // 
        // colId
        // 
        colId.HeaderText = "ID";
        colId.Name = "colId";
        colId.ReadOnly = true;
        colId.Visible = false;
        // 
        // colNombre
        // 
        colNombre.FillWeight = 85F;
        colNombre.HeaderText = "Nombre";
        colNombre.MinimumWidth = 75;
        colNombre.Name = "colNombre";
        colNombre.ReadOnly = true;
        // 
        // colApellido
        // 
        colApellido.FillWeight = 85F;
        colApellido.HeaderText = "Apellido";
        colApellido.MinimumWidth = 75;
        colApellido.Name = "colApellido";
        colApellido.ReadOnly = true;
        // 
        // colDni
        // 
        colDni.FillWeight = 70F;
        colDni.HeaderText = "DNI";
        colDni.MinimumWidth = 65;
        colDni.Name = "colDni";
        colDni.ReadOnly = true;
        // 
        // colTelefono
        // 
        colTelefono.FillWeight = 80F;
        colTelefono.HeaderText = "Teléfono";
        colTelefono.MinimumWidth = 75;
        colTelefono.Name = "colTelefono";
        colTelefono.ReadOnly = true;
        // 
        // colCorreoElectronico
        // 
        colCorreoElectronico.FillWeight = 110F;
        colCorreoElectronico.HeaderText = "Correo";
        colCorreoElectronico.MinimumWidth = 95;
        colCorreoElectronico.Name = "colCorreoElectronico";
        colCorreoElectronico.ReadOnly = true;
        // 
        // colDireccion
        // 
        colDireccion.HeaderText = "Dirección";
        colDireccion.MinimumWidth = 85;
        colDireccion.Name = "colDireccion";
        colDireccion.ReadOnly = true;
        // 
        // colEstado
        // 
        colEstado.FillWeight = 65F;
        colEstado.HeaderText = "Estado";
        colEstado.MinimumWidth = 60;
        colEstado.Name = "colEstado";
        colEstado.ReadOnly = true;
        // 
        // FormPropietarios
        // 
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        BackColor = Color.FromArgb(250, 244, 244);
        ClientSize = new Size(1220, 720);
        Controls.Add(pnlContenido);
        Controls.Add(pnlEncabezado);
        Font = new Font("Segoe UI", 9F);
        ForeColor = Color.FromArgb(58, 53, 59);
        MinimumSize = new Size(1220, 720);
        Name = "FormPropietarios";
        StartPosition = FormStartPosition.CenterParent;
        Text = "CLÍNICA VETERINARIA — GESTIÓN DE PROPIETARIOS";
        Load += FormPropietarios_Load;
        pnlEncabezado.ResumeLayout(false);
        pnlContenido.ResumeLayout(false);
        grpDatos.ResumeLayout(false);
        grpDatos.PerformLayout();
        grpMascotas.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)dgvMascotasPropietario).EndInit();
        pnlListado.ResumeLayout(false);
        pnlListado.PerformLayout();
        ((System.ComponentModel.ISupportInitialize)dgvPropietarios).EndInit();
        ResumeLayout(false);
    }

    private Panel pnlEncabezado;
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
    private Label lblCorreoElectronico;
    private TextBox txtCorreoElectronico;
    private Label lblDireccion;
    private TextBox txtDireccion;
    private Label lblEstado;
    private ComboBox cboEstado;
    private GroupBox grpMascotas;
    private DataGridView dgvMascotasPropietario;
    private DataGridViewTextBoxColumn colMascotaId;
    private DataGridViewTextBoxColumn colMascotaNombre;
    private DataGridViewTextBoxColumn colMascotaEspecie;
    private DataGridViewTextBoxColumn colMascotaRaza;
    private DataGridViewTextBoxColumn colMascotaSexo;
    private DataGridViewTextBoxColumn colMascotaEstado;
    private Button btnBuscar;
    private Button btnLimpiar;
    private Button btnVolver;
    private Panel pnlListado;
    private Label lblBuscar;
    private TextBox txtBuscar;
    private Label lblFiltroEstado;
    private ComboBox cboFiltroEstado;
    private DataGridView dgvPropietarios;
    private DataGridViewTextBoxColumn colId;
    private DataGridViewTextBoxColumn colNombre;
    private DataGridViewTextBoxColumn colApellido;
    private DataGridViewTextBoxColumn colDni;
    private DataGridViewTextBoxColumn colTelefono;
    private DataGridViewTextBoxColumn colCorreoElectronico;
    private DataGridViewTextBoxColumn colDireccion;
    private DataGridViewTextBoxColumn colEstado;
}

