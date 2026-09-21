namespace Veterinaria.WinForms.Vistas.Administrador
{
    partial class FormUsuarios
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
            DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle2 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle3 = new DataGridViewCellStyle();
            pnlEncabezado = new Panel();
            lblTitulo = new Label();
            lblUsuarioSesion = new Label();
            pnlContenido = new Panel();
            pnlListado = new Panel();
            lblBuscar = new Label();
            txtBuscar = new TextBox();
            lblFiltroRol = new Label();
            cboFiltroRol = new ComboBox();
            lblFiltroEstado = new Label();
            cboFiltroEstado = new ComboBox();
            dgvUsuarios = new DataGridView();
            colId = new DataGridViewTextBoxColumn();
            colNombre = new DataGridViewTextBoxColumn();
            colApellido = new DataGridViewTextBoxColumn();
            colRol = new DataGridViewTextBoxColumn();
            colDni = new DataGridViewTextBoxColumn();
            colTelefono = new DataGridViewTextBoxColumn();
            colCorreoElectronico = new DataGridViewTextBoxColumn();
            colDireccion = new DataGridViewTextBoxColumn();
            colEstado = new DataGridViewTextBoxColumn();
            pnlBotones = new Panel();
            btnGuardar = new Button();
            btnModificar = new Button();
            btnLimpiar = new Button();
            grpDatos = new GroupBox();
            lblNombre = new Label();
            txtNombre = new TextBox();
            lblApellido = new Label();
            txtApellido = new TextBox();
            lblDni = new Label();
            txtDni = new TextBox();
            lblDireccion = new Label();
            txtDireccion = new TextBox();
            lblTelefono = new Label();
            txtTelefono = new TextBox();
            lblCorreoElectronico = new Label();
            txtCorreoElectronico = new TextBox();
            lblUsuario = new Label();
            txtUsuario = new TextBox();
            lblContrasena = new Label();
            txtContrasena = new TextBox();
            lblFechaNacimiento = new Label();
            dtpFechaNacimiento = new DateTimePicker();
            lblSexo = new Label();
            rbHombre = new RadioButton();
            rbMujer = new RadioButton();
            lblPerfil = new Label();
            cboPerfil = new ComboBox();
            lblEstado = new Label();
            cboEstado = new ComboBox();
            lblErrorValidacion = new Label();
            btnVolver = new Button();
            pnlEncabezado.SuspendLayout();
            pnlContenido.SuspendLayout();
            pnlListado.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvUsuarios).BeginInit();
            pnlBotones.SuspendLayout();
            grpDatos.SuspendLayout();
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
            pnlEncabezado.TabIndex = 1;
            // 
            // lblTitulo
            // 
            lblTitulo.Dock = DockStyle.Fill;
            lblTitulo.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            lblTitulo.ForeColor = Color.White;
            lblTitulo.Location = new Point(16, 0);
            lblTitulo.Name = "lblTitulo";
            lblTitulo.Size = new Size(908, 48);
            lblTitulo.TabIndex = 0;
            lblTitulo.Text = "CLÍNICA VETERINARIA — GESTIÓN DE USUARIOS";
            lblTitulo.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lblUsuarioSesion
            // 
            lblUsuarioSesion.Dock = DockStyle.Right;
            lblUsuarioSesion.Font = new Font("Segoe UI", 9F);
            lblUsuarioSesion.ForeColor = Color.FromArgb(250, 244, 244);
            lblUsuarioSesion.Location = new Point(924, 0);
            lblUsuarioSesion.Name = "lblUsuarioSesion";
            lblUsuarioSesion.Size = new Size(280, 48);
            lblUsuarioSesion.TabIndex = 1;
            lblUsuarioSesion.Text = "Usuario: Admin";
            lblUsuarioSesion.TextAlign = ContentAlignment.MiddleRight;
            // 
            // pnlContenido
            // 
            pnlContenido.BackColor = Color.FromArgb(250, 244, 244);
            pnlContenido.Controls.Add(pnlListado);
            pnlContenido.Controls.Add(pnlBotones);
            pnlContenido.Controls.Add(grpDatos);
            pnlContenido.Controls.Add(btnVolver);
            pnlContenido.Dock = DockStyle.Fill;
            pnlContenido.Location = new Point(0, 48);
            pnlContenido.Name = "pnlContenido";
            pnlContenido.Padding = new Padding(12);
            pnlContenido.Size = new Size(1220, 672);
            pnlContenido.TabIndex = 0;
            // 
            // pnlListado
            // 
            pnlListado.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            pnlListado.BorderStyle = BorderStyle.FixedSingle;
            pnlListado.Controls.Add(lblBuscar);
            pnlListado.Controls.Add(txtBuscar);
            pnlListado.Controls.Add(lblFiltroRol);
            pnlListado.Controls.Add(cboFiltroRol);
            pnlListado.Controls.Add(lblFiltroEstado);
            pnlListado.Controls.Add(cboFiltroEstado);
            pnlListado.Controls.Add(dgvUsuarios);
            pnlListado.Location = new Point(434, 12);
            pnlListado.Name = "pnlListado";
            pnlListado.Size = new Size(1782, 1206);
            pnlListado.TabIndex = 0;
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
            txtBuscar.BackColor = Color.White;
            txtBuscar.BorderStyle = BorderStyle.FixedSingle;
            txtBuscar.Location = new Point(69, 14);
            txtBuscar.Name = "txtBuscar";
            txtBuscar.Size = new Size(359, 23);
            txtBuscar.TabIndex = 1;
            // 
            // lblFiltroRol
            // 
            lblFiltroRol.Location = new Point(434, 14);
            lblFiltroRol.Name = "lblFiltroRol";
            lblFiltroRol.Size = new Size(30, 23);
            lblFiltroRol.TabIndex = 2;
            lblFiltroRol.Text = "Rol";
            lblFiltroRol.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // cboFiltroRol
            // 
            cboFiltroRol.BackColor = Color.White;
            cboFiltroRol.DropDownStyle = ComboBoxStyle.DropDownList;
            cboFiltroRol.FormattingEnabled = true;
            cboFiltroRol.Location = new Point(466, 14);
            cboFiltroRol.Name = "cboFiltroRol";
            cboFiltroRol.Size = new Size(130, 23);
            cboFiltroRol.TabIndex = 3;
            // 
            // lblFiltroEstado
            // 
            lblFiltroEstado.Location = new Point(602, 14);
            lblFiltroEstado.Name = "lblFiltroEstado";
            lblFiltroEstado.Size = new Size(46, 23);
            lblFiltroEstado.TabIndex = 4;
            lblFiltroEstado.Text = "Estado";
            lblFiltroEstado.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // cboFiltroEstado
            // 
            cboFiltroEstado.BackColor = Color.White;
            cboFiltroEstado.DropDownStyle = ComboBoxStyle.DropDownList;
            cboFiltroEstado.FormattingEnabled = true;
            cboFiltroEstado.Location = new Point(654, 14);
            cboFiltroEstado.Name = "cboFiltroEstado";
            cboFiltroEstado.Size = new Size(115, 23);
            cboFiltroEstado.TabIndex = 5;
            // 
            // dgvUsuarios
            // 
            dgvUsuarios.AllowUserToAddRows = false;
            dgvUsuarios.AllowUserToDeleteRows = false;
            dgvUsuarios.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            dgvUsuarios.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvUsuarios.BackgroundColor = Color.White;
            dataGridViewCellStyle1.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = SystemColors.Control;
            dataGridViewCellStyle1.Font = new Font("Segoe UI", 9F);
            dataGridViewCellStyle1.ForeColor = SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = DataGridViewTriState.True;
            dgvUsuarios.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            dgvUsuarios.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvUsuarios.Columns.AddRange(new DataGridViewColumn[] { colId, colNombre, colApellido, colRol, colDni, colTelefono, colCorreoElectronico, colDireccion, colEstado });
            dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = SystemColors.Window;
            dataGridViewCellStyle2.Font = new Font("Segoe UI", 9F);
            dataGridViewCellStyle2.ForeColor = Color.FromArgb(58, 53, 59);
            dataGridViewCellStyle2.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = DataGridViewTriState.False;
            dgvUsuarios.DefaultCellStyle = dataGridViewCellStyle2;
            dgvUsuarios.Location = new Point(19, 48);
            dgvUsuarios.MultiSelect = false;
            dgvUsuarios.Name = "dgvUsuarios";
            dgvUsuarios.ReadOnly = true;
            dataGridViewCellStyle3.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = SystemColors.Control;
            dataGridViewCellStyle3.Font = new Font("Segoe UI", 9F);
            dataGridViewCellStyle3.ForeColor = SystemColors.WindowText;
            dataGridViewCellStyle3.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = DataGridViewTriState.True;
            dgvUsuarios.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
            dgvUsuarios.RowHeadersVisible = false;
            dgvUsuarios.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvUsuarios.Size = new Size(748, 595);
            dgvUsuarios.TabIndex = 0;
            dgvUsuarios.CellClick += dgvUsuarios_CellClick;
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
            // colRol
            // 
            colRol.FillWeight = 80F;
            colRol.HeaderText = "Rol";
            colRol.MinimumWidth = 70;
            colRol.Name = "colRol";
            colRol.ReadOnly = true;
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
            // pnlBotones
            // 
            pnlBotones.Controls.Add(btnGuardar);
            pnlBotones.Controls.Add(btnModificar);
            pnlBotones.Controls.Add(btnLimpiar);
            pnlBotones.Location = new Point(12, 502);
            pnlBotones.Name = "pnlBotones";
            pnlBotones.Size = new Size(410, 68);
            pnlBotones.TabIndex = 1;
            // 
            // btnGuardar
            // 
            btnGuardar.BackColor = Color.FromArgb(152, 196, 164);
            btnGuardar.Cursor = Cursors.Hand;
            btnGuardar.FlatAppearance.BorderSize = 0;
            btnGuardar.FlatStyle = FlatStyle.Flat;
            btnGuardar.Font = new Font("Segoe UI", 9F);
            btnGuardar.ForeColor = Color.Black;
            btnGuardar.Location = new Point(10, 18);
            btnGuardar.Name = "btnGuardar";
            btnGuardar.Size = new Size(120, 32);
            btnGuardar.TabIndex = 1;
            btnGuardar.Text = "Guardar";
            btnGuardar.UseVisualStyleBackColor = false;
            btnGuardar.Click += btnGuardar_Click;
            // 
            // btnModificar
            // 
            btnModificar.BackColor = Color.FromArgb(148, 176, 214);
            btnModificar.Cursor = Cursors.Hand;
            btnModificar.FlatAppearance.BorderSize = 0;
            btnModificar.FlatStyle = FlatStyle.Flat;
            btnModificar.Font = new Font("Segoe UI", 9F);
            btnModificar.ForeColor = Color.Black;
            btnModificar.Location = new Point(144, 18);
            btnModificar.Name = "btnModificar";
            btnModificar.Size = new Size(120, 32);
            btnModificar.TabIndex = 2;
            btnModificar.Text = "Modificar";
            btnModificar.UseVisualStyleBackColor = false;
            btnModificar.Click += btnModificar_Click;
            // 
            // btnLimpiar
            // 
            btnLimpiar.BackColor = Color.FromArgb(226, 217, 220);
            btnLimpiar.Cursor = Cursors.Hand;
            btnLimpiar.FlatAppearance.BorderSize = 0;
            btnLimpiar.FlatStyle = FlatStyle.Flat;
            btnLimpiar.Font = new Font("Segoe UI", 9F);
            btnLimpiar.ForeColor = Color.FromArgb(58, 53, 59);
            btnLimpiar.Location = new Point(278, 18);
            btnLimpiar.Name = "btnLimpiar";
            btnLimpiar.Size = new Size(120, 32);
            btnLimpiar.TabIndex = 3;
            btnLimpiar.Text = "Limpiar";
            btnLimpiar.UseVisualStyleBackColor = false;
            btnLimpiar.Click += btnLimpiar_Click;
            // 
            // grpDatos
            // 
            grpDatos.Controls.Add(lblNombre);
            grpDatos.Controls.Add(txtNombre);
            grpDatos.Controls.Add(lblApellido);
            grpDatos.Controls.Add(txtApellido);
            grpDatos.Controls.Add(lblDni);
            grpDatos.Controls.Add(txtDni);
            grpDatos.Controls.Add(lblDireccion);
            grpDatos.Controls.Add(txtDireccion);
            grpDatos.Controls.Add(lblTelefono);
            grpDatos.Controls.Add(txtTelefono);
            grpDatos.Controls.Add(lblCorreoElectronico);
            grpDatos.Controls.Add(txtCorreoElectronico);
            grpDatos.Controls.Add(lblUsuario);
            grpDatos.Controls.Add(txtUsuario);
            grpDatos.Controls.Add(lblContrasena);
            grpDatos.Controls.Add(txtContrasena);
            grpDatos.Controls.Add(lblFechaNacimiento);
            grpDatos.Controls.Add(dtpFechaNacimiento);
            grpDatos.Controls.Add(lblSexo);
            grpDatos.Controls.Add(rbHombre);
            grpDatos.Controls.Add(rbMujer);
            grpDatos.Controls.Add(lblPerfil);
            grpDatos.Controls.Add(cboPerfil);
            grpDatos.Controls.Add(lblEstado);
            grpDatos.Controls.Add(cboEstado);
            grpDatos.Controls.Add(lblErrorValidacion);
            grpDatos.Font = new Font("Segoe UI", 9F);
            grpDatos.ForeColor = Color.FromArgb(58, 53, 59);
            grpDatos.Location = new Point(12, 12);
            grpDatos.Name = "grpDatos";
            grpDatos.Size = new Size(410, 482);
            grpDatos.TabIndex = 2;
            grpDatos.TabStop = false;
            grpDatos.Text = "Datos del usuario";
            // 
            // lblNombre
            // 
            lblNombre.Location = new Point(14, 28);
            lblNombre.Name = "lblNombre";
            lblNombre.Size = new Size(132, 23);
            lblNombre.TabIndex = 0;
            lblNombre.Text = "Nombre";
            lblNombre.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // txtNombre
            // 
            txtNombre.BackColor = Color.White;
            txtNombre.BorderStyle = BorderStyle.FixedSingle;
            txtNombre.Location = new Point(154, 28);
            txtNombre.MaxLength = 100;
            txtNombre.Name = "txtNombre";
            txtNombre.Size = new Size(240, 23);
            txtNombre.TabIndex = 1;
            // 
            // lblApellido
            // 
            lblApellido.Location = new Point(14, 64);
            lblApellido.Name = "lblApellido";
            lblApellido.Size = new Size(132, 23);
            lblApellido.TabIndex = 2;
            lblApellido.Text = "Apellido";
            lblApellido.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // txtApellido
            // 
            txtApellido.BackColor = Color.White;
            txtApellido.BorderStyle = BorderStyle.FixedSingle;
            txtApellido.Location = new Point(154, 64);
            txtApellido.MaxLength = 100;
            txtApellido.Name = "txtApellido";
            txtApellido.Size = new Size(240, 23);
            txtApellido.TabIndex = 3;
            // 
            // lblDni
            // 
            lblDni.Location = new Point(14, 100);
            lblDni.Name = "lblDni";
            lblDni.Size = new Size(132, 23);
            lblDni.TabIndex = 4;
            lblDni.Text = "DNI";
            lblDni.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // txtDni
            // 
            txtDni.BackColor = Color.White;
            txtDni.BorderStyle = BorderStyle.FixedSingle;
            txtDni.Location = new Point(154, 100);
            txtDni.MaxLength = 20;
            txtDni.Name = "txtDni";
            txtDni.Size = new Size(240, 23);
            txtDni.TabIndex = 5;
            // 
            // lblDireccion
            // 
            lblDireccion.Location = new Point(14, 136);
            lblDireccion.Name = "lblDireccion";
            lblDireccion.Size = new Size(132, 23);
            lblDireccion.TabIndex = 6;
            lblDireccion.Text = "Dirección";
            lblDireccion.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // txtDireccion
            // 
            txtDireccion.BackColor = Color.White;
            txtDireccion.BorderStyle = BorderStyle.FixedSingle;
            txtDireccion.Location = new Point(154, 136);
            txtDireccion.MaxLength = 200;
            txtDireccion.Name = "txtDireccion";
            txtDireccion.Size = new Size(240, 23);
            txtDireccion.TabIndex = 7;
            // 
            // lblTelefono
            // 
            lblTelefono.Location = new Point(14, 172);
            lblTelefono.Name = "lblTelefono";
            lblTelefono.Size = new Size(132, 23);
            lblTelefono.TabIndex = 8;
            lblTelefono.Text = "Teléfono";
            lblTelefono.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // txtTelefono
            // 
            txtTelefono.BackColor = Color.White;
            txtTelefono.BorderStyle = BorderStyle.FixedSingle;
            txtTelefono.Location = new Point(154, 172);
            txtTelefono.MaxLength = 13;
            txtTelefono.Name = "txtTelefono";
            txtTelefono.Size = new Size(240, 23);
            txtTelefono.TabIndex = 9;
            // 
            // lblCorreoElectronico
            // 
            lblCorreoElectronico.Location = new Point(14, 208);
            lblCorreoElectronico.Name = "lblCorreoElectronico";
            lblCorreoElectronico.Size = new Size(132, 23);
            lblCorreoElectronico.TabIndex = 10;
            lblCorreoElectronico.Text = "Correo";
            lblCorreoElectronico.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // txtCorreoElectronico
            // 
            txtCorreoElectronico.BackColor = Color.White;
            txtCorreoElectronico.BorderStyle = BorderStyle.FixedSingle;
            txtCorreoElectronico.Location = new Point(154, 208);
            txtCorreoElectronico.MaxLength = 100;
            txtCorreoElectronico.Name = "txtCorreoElectronico";
            txtCorreoElectronico.Size = new Size(240, 23);
            txtCorreoElectronico.TabIndex = 11;
            // 
            // lblUsuario
            // 
            lblUsuario.Location = new Point(14, 244);
            lblUsuario.Name = "lblUsuario";
            lblUsuario.Size = new Size(132, 23);
            lblUsuario.TabIndex = 12;
            lblUsuario.Text = "Usuario";
            lblUsuario.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // txtUsuario
            // 
            txtUsuario.BackColor = Color.White;
            txtUsuario.BorderStyle = BorderStyle.FixedSingle;
            txtUsuario.Location = new Point(154, 244);
            txtUsuario.MaxLength = 50;
            txtUsuario.Name = "txtUsuario";
            txtUsuario.Size = new Size(240, 23);
            txtUsuario.TabIndex = 13;
            // 
            // lblContrasena
            // 
            lblContrasena.Location = new Point(14, 280);
            lblContrasena.Name = "lblContrasena";
            lblContrasena.Size = new Size(132, 23);
            lblContrasena.TabIndex = 14;
            lblContrasena.Text = "Contraseña";
            lblContrasena.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // txtContrasena
            // 
            txtContrasena.BackColor = Color.White;
            txtContrasena.BorderStyle = BorderStyle.FixedSingle;
            txtContrasena.Location = new Point(154, 280);
            txtContrasena.MaxLength = 100;
            txtContrasena.Name = "txtContrasena";
            txtContrasena.Size = new Size(240, 23);
            txtContrasena.TabIndex = 15;
            txtContrasena.UseSystemPasswordChar = true;
            // 
            // lblFechaNacimiento
            // 
            lblFechaNacimiento.Location = new Point(14, 316);
            lblFechaNacimiento.Name = "lblFechaNacimiento";
            lblFechaNacimiento.Size = new Size(132, 23);
            lblFechaNacimiento.TabIndex = 16;
            lblFechaNacimiento.Text = "Nacimiento";
            lblFechaNacimiento.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // dtpFechaNacimiento
            // 
            dtpFechaNacimiento.CalendarMonthBackground = Color.White;
            dtpFechaNacimiento.CustomFormat = "dd/MM/yyyy";
            dtpFechaNacimiento.Format = DateTimePickerFormat.Custom;
            dtpFechaNacimiento.Location = new Point(154, 316);
            dtpFechaNacimiento.Name = "dtpFechaNacimiento";
            dtpFechaNacimiento.Size = new Size(240, 23);
            dtpFechaNacimiento.TabIndex = 17;
            // 
            // lblSexo
            // 
            lblSexo.Location = new Point(14, 352);
            lblSexo.Name = "lblSexo";
            lblSexo.Size = new Size(132, 23);
            lblSexo.TabIndex = 18;
            lblSexo.Text = "Sexo";
            lblSexo.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // rbHombre
            // 
            rbHombre.Location = new Point(154, 352);
            rbHombre.Name = "rbHombre";
            rbHombre.Size = new Size(80, 23);
            rbHombre.TabIndex = 19;
            rbHombre.Text = "Hombre";
            // 
            // rbMujer
            // 
            rbMujer.Location = new Point(244, 352);
            rbMujer.Name = "rbMujer";
            rbMujer.Size = new Size(70, 23);
            rbMujer.TabIndex = 20;
            rbMujer.Text = "Mujer";
            // 
            // lblPerfil
            // 
            lblPerfil.Location = new Point(14, 388);
            lblPerfil.Name = "lblPerfil";
            lblPerfil.Size = new Size(132, 23);
            lblPerfil.TabIndex = 21;
            lblPerfil.Text = "Perfil";
            lblPerfil.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // cboPerfil
            // 
            cboPerfil.BackColor = Color.White;
            cboPerfil.DropDownStyle = ComboBoxStyle.DropDownList;
            cboPerfil.FormattingEnabled = true;
            cboPerfil.Location = new Point(154, 388);
            cboPerfil.Name = "cboPerfil";
            cboPerfil.Size = new Size(240, 23);
            cboPerfil.TabIndex = 22;
            // 
            // lblEstado
            // 
            lblEstado.Location = new Point(14, 420);
            lblEstado.Name = "lblEstado";
            lblEstado.Size = new Size(132, 23);
            lblEstado.TabIndex = 23;
            lblEstado.Text = "Estado";
            lblEstado.TextAlign = ContentAlignment.MiddleLeft;
            lblEstado.Visible = false;
            // 
            // cboEstado
            // 
            cboEstado.BackColor = Color.White;
            cboEstado.DropDownStyle = ComboBoxStyle.DropDownList;
            cboEstado.FormattingEnabled = true;
            cboEstado.Location = new Point(154, 420);
            cboEstado.Name = "cboEstado";
            cboEstado.Size = new Size(240, 23);
            cboEstado.TabIndex = 24;
            cboEstado.Visible = false;
            // 
            // lblErrorValidacion
            // 
            lblErrorValidacion.ForeColor = Color.FromArgb(178, 34, 34);
            lblErrorValidacion.Location = new Point(14, 448);
            lblErrorValidacion.Name = "lblErrorValidacion";
            lblErrorValidacion.Size = new Size(380, 22);
            lblErrorValidacion.TabIndex = 25;
            lblErrorValidacion.TextAlign = ContentAlignment.MiddleLeft;
            lblErrorValidacion.Visible = false;
            // 
            // btnVolver
            // 
            btnVolver.BackColor = Color.FromArgb(220, 200, 204);
            btnVolver.Cursor = Cursors.Hand;
            btnVolver.FlatAppearance.BorderSize = 0;
            btnVolver.FlatStyle = FlatStyle.Flat;
            btnVolver.Font = new Font("Segoe UI", 9F);
            btnVolver.ForeColor = Color.Black;
            btnVolver.Location = new Point(16, 625);
            btnVolver.Name = "btnVolver";
            btnVolver.Size = new Size(202, 32);
            btnVolver.TabIndex = 4;
            btnVolver.Text = "Volver al panel";
            btnVolver.UseVisualStyleBackColor = false;
            btnVolver.Click += btnVolver_Click;
            // 
            // FormUsuarios
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
            Name = "FormUsuarios";
            StartPosition = FormStartPosition.CenterParent;
            Text = "CLÍNICA VETERINARIA — GESTIÓN DE USUARIOS";
            Load += FormUsuarios_Load;
            pnlEncabezado.ResumeLayout(false);
            pnlContenido.ResumeLayout(false);
            pnlListado.ResumeLayout(false);
            pnlListado.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dgvUsuarios).EndInit();
            pnlBotones.ResumeLayout(false);
            grpDatos.ResumeLayout(false);
            grpDatos.PerformLayout();
            ResumeLayout(false);
        }

        private Panel pnlEncabezado;
        private Label lblTitulo;
        private Label lblUsuarioSesion;
        private Panel pnlContenido;
        private GroupBox grpDatos;
        private Label lblNombre;
        private TextBox txtNombre;
        private Label lblApellido;
        private TextBox txtApellido;
        private Label lblDni;
        private TextBox txtDni;
        private Label lblDireccion;
        private TextBox txtDireccion;
        private Label lblTelefono;
        private TextBox txtTelefono;
        private Label lblCorreoElectronico;
        private TextBox txtCorreoElectronico;
        private Label lblUsuario;
        private TextBox txtUsuario;
        private Label lblContrasena;
        private TextBox txtContrasena;
        private Label lblFechaNacimiento;
        private DateTimePicker dtpFechaNacimiento;
        private Label lblSexo;
        private RadioButton rbHombre;
        private RadioButton rbMujer;
        private Label lblPerfil;
        private ComboBox cboPerfil;
        private Label lblErrorValidacion;
        private Panel pnlBotones;
        private Button btnGuardar;
        private Button btnModificar;
        private Button btnLimpiar;
        private Button btnVolver;
        private Panel pnlListado;
        private Label lblBuscar;
        private TextBox txtBuscar;
        private Label lblFiltroRol;
        private ComboBox cboFiltroRol;
        private Label lblFiltroEstado;
        private ComboBox cboFiltroEstado;
        private Label lblEstado;
        private ComboBox cboEstado;
        private DataGridView dgvUsuarios;
        private DataGridViewTextBoxColumn colId;
        private DataGridViewTextBoxColumn colNombre;
        private DataGridViewTextBoxColumn colApellido;
        private DataGridViewTextBoxColumn colRol;
        private DataGridViewTextBoxColumn colDni;
        private DataGridViewTextBoxColumn colTelefono;
        private DataGridViewTextBoxColumn colCorreoElectronico;
        private DataGridViewTextBoxColumn colDireccion;
        private DataGridViewTextBoxColumn colEstado;
    }
}
