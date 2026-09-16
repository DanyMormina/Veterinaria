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
            pnlEncabezado = new Panel();
            lblTitulo = new Label();
            lblUsuarioSesion = new Label();
            pnlContenido = new Panel();
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
            pnlBotones = new Panel();
            btnNuevo = new Button();
            btnGuardar = new Button();
            btnModificar = new Button();
            btnCancelar = new Button();
            btnDesactivar = new Button();
            btnVolver = new Button();
            pnlListado = new Panel();
            lblBuscar = new Label();
            txtBuscar = new TextBox();
            btnBuscar = new Button();
            btnActivos = new Button();
            btnInactivos = new Button();
            dgvUsuarios = new DataGridView();
            colId = new DataGridViewTextBoxColumn();
            colNombre = new DataGridViewTextBoxColumn();
            colApellido = new DataGridViewTextBoxColumn();
            colDni = new DataGridViewTextBoxColumn();
            colCorreoElectronico = new DataGridViewTextBoxColumn();
            colDireccion = new DataGridViewTextBoxColumn();
            colEstado = new DataGridViewTextBoxColumn();
            pnlEncabezado.SuspendLayout();
            pnlContenido.SuspendLayout();
            grpDatos.SuspendLayout();
            pnlBotones.SuspendLayout();
            pnlListado.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvUsuarios).BeginInit();
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
            pnlEncabezado.Size = new Size(1180, 50);
            pnlEncabezado.TabIndex = 0;
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
            lblTitulo.Text = "CLÍNICA VETERINARIA — ADMINISTRADOR — GESTIÓN DE USUARIOS";
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
            pnlContenido.Padding = new Padding(16);
            pnlContenido.Size = new Size(1180, 630);
            pnlContenido.TabIndex = 1;
            // 
            // grpDatos
            // 
            grpDatos.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
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
            grpDatos.Font = new Font("Segoe UI", 9F);
            grpDatos.ForeColor = Color.FromArgb(58, 53, 59);
            grpDatos.Location = new Point(16, 12);
            grpDatos.Name = "grpDatos";
            grpDatos.Size = new Size(380, 510);
            grpDatos.TabIndex = 0;
            grpDatos.TabStop = false;
            grpDatos.Text = "Datos del usuario";
            // 
            // lblNombre
            // 
            lblNombre.Location = new Point(16, 32);
            lblNombre.Name = "lblNombre";
            lblNombre.Size = new Size(118, 23);
            lblNombre.Text = "Nombre";
            lblNombre.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // txtNombre
            // 
            txtNombre.BackColor = Color.White;
            txtNombre.BorderStyle = BorderStyle.FixedSingle;
            txtNombre.Location = new Point(130, 32);
            txtNombre.Name = "txtNombre";
            txtNombre.Size = new Size(230, 23);
            // 
            // lblApellido
            // 
            lblApellido.Location = new Point(16, 68);
            lblApellido.Name = "lblApellido";
            lblApellido.Size = new Size(118, 23);
            lblApellido.Text = "Apellido";
            lblApellido.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // txtApellido
            // 
            txtApellido.BackColor = Color.White;
            txtApellido.BorderStyle = BorderStyle.FixedSingle;
            txtApellido.Location = new Point(130, 68);
            txtApellido.Name = "txtApellido";
            txtApellido.Size = new Size(230, 23);
            // 
            // lblDni
            // 
            lblDni.Location = new Point(16, 104);
            lblDni.Name = "lblDni";
            lblDni.Size = new Size(118, 23);
            lblDni.Text = "DNI";
            lblDni.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // txtDni
            // 
            txtDni.BackColor = Color.White;
            txtDni.BorderStyle = BorderStyle.FixedSingle;
            txtDni.Location = new Point(130, 104);
            txtDni.Name = "txtDni";
            txtDni.Size = new Size(230, 23);
            // 
            // lblDireccion
            // 
            lblDireccion.Location = new Point(16, 140);
            lblDireccion.Name = "lblDireccion";
            lblDireccion.Size = new Size(118, 23);
            lblDireccion.Text = "Dirección";
            lblDireccion.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // txtDireccion
            // 
            txtDireccion.BackColor = Color.White;
            txtDireccion.BorderStyle = BorderStyle.FixedSingle;
            txtDireccion.Location = new Point(130, 140);
            txtDireccion.Name = "txtDireccion";
            txtDireccion.Size = new Size(230, 23);
            // 
            // lblTelefono
            // 
            lblTelefono.Location = new Point(16, 176);
            lblTelefono.Name = "lblTelefono";
            lblTelefono.Size = new Size(118, 23);
            lblTelefono.Text = "Teléfono";
            lblTelefono.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // txtTelefono
            // 
            txtTelefono.BackColor = Color.White;
            txtTelefono.BorderStyle = BorderStyle.FixedSingle;
            txtTelefono.Location = new Point(130, 176);
            txtTelefono.Name = "txtTelefono";
            txtTelefono.Size = new Size(230, 23);
            // 
            // lblCorreoElectronico
            // 
            lblCorreoElectronico.Location = new Point(16, 212);
            lblCorreoElectronico.Name = "lblCorreoElectronico";
            lblCorreoElectronico.Size = new Size(118, 23);
            lblCorreoElectronico.Text = "Correo";
            lblCorreoElectronico.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // txtCorreoElectronico
            // 
            txtCorreoElectronico.BackColor = Color.White;
            txtCorreoElectronico.BorderStyle = BorderStyle.FixedSingle;
            txtCorreoElectronico.Location = new Point(130, 212);
            txtCorreoElectronico.Name = "txtCorreoElectronico";
            txtCorreoElectronico.Size = new Size(230, 23);
            // 
            // lblUsuario
            // 
            lblUsuario.Location = new Point(16, 248);
            lblUsuario.Name = "lblUsuario";
            lblUsuario.Size = new Size(118, 23);
            lblUsuario.Text = "Usuario";
            lblUsuario.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // txtUsuario
            // 
            txtUsuario.BackColor = Color.White;
            txtUsuario.BorderStyle = BorderStyle.FixedSingle;
            txtUsuario.Location = new Point(130, 248);
            txtUsuario.Name = "txtUsuario";
            txtUsuario.Size = new Size(230, 23);
            // 
            // lblContrasena
            // 
            lblContrasena.Location = new Point(16, 284);
            lblContrasena.Name = "lblContrasena";
            lblContrasena.Size = new Size(118, 23);
            lblContrasena.Text = "Contraseña";
            lblContrasena.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // txtContrasena
            // 
            txtContrasena.BackColor = Color.White;
            txtContrasena.BorderStyle = BorderStyle.FixedSingle;
            txtContrasena.Location = new Point(130, 284);
            txtContrasena.Name = "txtContrasena";
            txtContrasena.Size = new Size(230, 23);
            txtContrasena.UseSystemPasswordChar = true;
            // 
            // lblFechaNacimiento
            // 
            lblFechaNacimiento.Location = new Point(16, 320);
            lblFechaNacimiento.Name = "lblFechaNacimiento";
            lblFechaNacimiento.Size = new Size(118, 23);
            lblFechaNacimiento.Text = "Fecha de nacimiento";
            lblFechaNacimiento.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // dtpFechaNacimiento
            // 
            dtpFechaNacimiento.CalendarMonthBackground = Color.White;
            dtpFechaNacimiento.Format = DateTimePickerFormat.Short;
            dtpFechaNacimiento.Location = new Point(130, 320);
            dtpFechaNacimiento.Name = "dtpFechaNacimiento";
            dtpFechaNacimiento.Size = new Size(230, 23);
            // 
            // lblSexo
            // 
            lblSexo.Location = new Point(16, 356);
            lblSexo.Name = "lblSexo";
            lblSexo.Size = new Size(118, 23);
            lblSexo.Text = "Sexo";
            lblSexo.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // rbHombre
            // 
            rbHombre.Location = new Point(130, 356);
            rbHombre.Name = "rbHombre";
            rbHombre.Size = new Size(90, 23);
            rbHombre.Text = "Hombre";
            // 
            // rbMujer
            // 
            rbMujer.Location = new Point(226, 356);
            rbMujer.Name = "rbMujer";
            rbMujer.Size = new Size(90, 23);
            rbMujer.Text = "Mujer";
            // 
            // lblPerfil
            // 
            lblPerfil.Location = new Point(16, 392);
            lblPerfil.Name = "lblPerfil";
            lblPerfil.Size = new Size(118, 23);
            lblPerfil.Text = "Perfil";
            lblPerfil.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // cboPerfil
            // 
            cboPerfil.BackColor = Color.White;
            cboPerfil.DropDownStyle = ComboBoxStyle.DropDownList;
            cboPerfil.FormattingEnabled = true;
            cboPerfil.Items.AddRange(new object[] { "Administrador", "Secretario", "Veterinario" });
            cboPerfil.Location = new Point(130, 392);
            cboPerfil.Name = "cboPerfil";
            cboPerfil.Size = new Size(230, 23);
            // 
            // pnlBotones
            // 
            pnlBotones.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            pnlBotones.Controls.Add(btnNuevo);
            pnlBotones.Controls.Add(btnGuardar);
            pnlBotones.Controls.Add(btnModificar);
            pnlBotones.Controls.Add(btnCancelar);
            pnlBotones.Controls.Add(btnDesactivar);
            pnlBotones.Controls.Add(btnVolver);
            pnlBotones.Location = new Point(16, 530);
            pnlBotones.Name = "pnlBotones";
            pnlBotones.Size = new Size(380, 84);
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
            btnNuevo.Size = new Size(88, 32);
            btnNuevo.Text = "Nuevo";
            btnNuevo.UseVisualStyleBackColor = false;
            btnNuevo.Click += btnNuevo_Click;
            // 
            // btnGuardar
            // 
            btnGuardar.BackColor = Color.FromArgb(72, 148, 96);
            btnGuardar.FlatAppearance.BorderColor = Color.FromArgb(48, 118, 74);
            btnGuardar.FlatStyle = FlatStyle.Flat;
            btnGuardar.ForeColor = Color.White;
            btnGuardar.Location = new Point(96, 4);
            btnGuardar.Name = "btnGuardar";
            btnGuardar.Size = new Size(88, 32);
            btnGuardar.Text = "Guardar";
            btnGuardar.UseVisualStyleBackColor = false;
            btnGuardar.Click += btnGuardar_Click;
            // 
            // btnModificar
            // 
            btnModificar.BackColor = Color.FromArgb(64, 126, 186);
            btnModificar.FlatAppearance.BorderColor = Color.FromArgb(44, 102, 158);
            btnModificar.FlatStyle = FlatStyle.Flat;
            btnModificar.ForeColor = Color.White;
            btnModificar.Location = new Point(192, 4);
            btnModificar.Name = "btnModificar";
            btnModificar.Size = new Size(88, 32);
            btnModificar.Text = "Modificar";
            btnModificar.UseVisualStyleBackColor = false;
            btnModificar.Click += btnModificar_Click;
            // 
            // btnCancelar
            // 
            btnCancelar.BackColor = Color.FromArgb(196, 78, 86);
            btnCancelar.FlatAppearance.BorderColor = Color.FromArgb(164, 54, 62);
            btnCancelar.FlatStyle = FlatStyle.Flat;
            btnCancelar.ForeColor = Color.White;
            btnCancelar.Location = new Point(288, 4);
            btnCancelar.Name = "btnCancelar";
            btnCancelar.Size = new Size(88, 32);
            btnCancelar.Text = "Cancelar";
            btnCancelar.UseVisualStyleBackColor = false;
            btnCancelar.Click += btnCancelar_Click;
            // 
            // btnDesactivar
            // 
            btnDesactivar.BackColor = Color.FromArgb(196, 78, 86);
            btnDesactivar.FlatAppearance.BorderColor = Color.FromArgb(164, 54, 62);
            btnDesactivar.FlatStyle = FlatStyle.Flat;
            btnDesactivar.ForeColor = Color.White;
            btnDesactivar.Location = new Point(192, 44);
            btnDesactivar.Name = "btnDesactivar";
            btnDesactivar.Size = new Size(184, 32);
            btnDesactivar.Text = "Desactivar";
            btnDesactivar.UseVisualStyleBackColor = false;
            btnDesactivar.Click += btnDesactivar_Click;
            // 
            // btnVolver
            // 
            btnVolver.BackColor = Color.FromArgb(220, 200, 204);
            btnVolver.FlatAppearance.BorderColor = Color.FromArgb(186, 162, 168);
            btnVolver.FlatStyle = FlatStyle.Flat;
            btnVolver.ForeColor = Color.FromArgb(58, 53, 59);
            btnVolver.Location = new Point(0, 44);
            btnVolver.Name = "btnVolver";
            btnVolver.Size = new Size(184, 32);
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
            pnlListado.Controls.Add(dgvUsuarios);
            pnlListado.Location = new Point(412, 12);
            pnlListado.Name = "pnlListado";
            pnlListado.Size = new Size(752, 602);
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
            txtBuscar.Size = new Size(354, 23);
            // 
            // btnBuscar
            // 
            btnBuscar.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnBuscar.BackColor = Color.FromArgb(220, 200, 204);
            btnBuscar.FlatAppearance.BorderColor = Color.FromArgb(186, 162, 168);
            btnBuscar.FlatStyle = FlatStyle.Flat;
            btnBuscar.ForeColor = Color.FromArgb(58, 53, 59);
            btnBuscar.Location = new Point(430, 10);
            btnBuscar.Name = "btnBuscar";
            btnBuscar.Size = new Size(90, 27);
            btnBuscar.Text = "Buscar";
            btnBuscar.UseVisualStyleBackColor = false;
            btnBuscar.Click += btnBuscar_Click;
            // 
            // btnActivos
            // 
            btnActivos.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnActivos.BackColor = Color.FromArgb(220, 200, 204);
            btnActivos.FlatAppearance.BorderColor = Color.FromArgb(186, 162, 168);
            btnActivos.FlatStyle = FlatStyle.Flat;
            btnActivos.ForeColor = Color.FromArgb(58, 53, 59);
            btnActivos.Location = new Point(526, 10);
            btnActivos.Name = "btnActivos";
            btnActivos.Size = new Size(100, 27);
            btnActivos.Text = "Activos";
            btnActivos.UseVisualStyleBackColor = false;
            btnActivos.Click += btnActivos_Click;
            // 
            // btnInactivos
            // 
            btnInactivos.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnInactivos.BackColor = Color.FromArgb(220, 200, 204);
            btnInactivos.FlatAppearance.BorderColor = Color.FromArgb(186, 162, 168);
            btnInactivos.FlatStyle = FlatStyle.Flat;
            btnInactivos.ForeColor = Color.FromArgb(58, 53, 59);
            btnInactivos.Location = new Point(632, 10);
            btnInactivos.Name = "btnInactivos";
            btnInactivos.Size = new Size(106, 27);
            btnInactivos.Text = "Inactivos";
            btnInactivos.UseVisualStyleBackColor = false;
            btnInactivos.Click += btnInactivos_Click;
            // 
            // dgvUsuarios
            // 
            dgvUsuarios.AllowUserToAddRows = false;
            dgvUsuarios.AllowUserToDeleteRows = false;
            dgvUsuarios.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            dgvUsuarios.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvUsuarios.BackgroundColor = Color.White;
            dgvUsuarios.BorderStyle = BorderStyle.FixedSingle;
            dgvUsuarios.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvUsuarios.Columns.AddRange(new DataGridViewColumn[] { colId, colNombre, colApellido, colDni, colCorreoElectronico, colDireccion, colEstado });
            dgvUsuarios.Location = new Point(10, 48);
            dgvUsuarios.MultiSelect = false;
            dgvUsuarios.Name = "dgvUsuarios";
            dgvUsuarios.ReadOnly = true;
            dgvUsuarios.RowHeadersVisible = false;
            dgvUsuarios.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvUsuarios.Size = new Size(730, 542);
            dgvUsuarios.CellClick += dgvUsuarios_CellClick;
            // 
            // columnas
            // 
            colId.HeaderText = "ID";
            colId.Name = "colId";
            colId.Visible = false;
            colNombre.FillWeight = 90F;
            colNombre.HeaderText = "Nombre";
            colNombre.Name = "colNombre";
            colApellido.FillWeight = 90F;
            colApellido.HeaderText = "Apellido";
            colApellido.Name = "colApellido";
            colDni.FillWeight = 70F;
            colDni.HeaderText = "DNI";
            colDni.Name = "colDni";
            colCorreoElectronico.FillWeight = 130F;
            colCorreoElectronico.HeaderText = "Correo";
            colCorreoElectronico.Name = "colCorreoElectronico";
            colDireccion.FillWeight = 140F;
            colDireccion.HeaderText = "Dirección";
            colDireccion.Name = "colDireccion";
            colEstado.FillWeight = 60F;
            colEstado.HeaderText = "Estado";
            colEstado.Name = "colEstado";
            // 
            // FormUsuarios
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(250, 244, 244);
            ClientSize = new Size(1180, 680);
            Controls.Add(pnlContenido);
            Controls.Add(pnlEncabezado);
            Font = new Font("Segoe UI", 9F);
            ForeColor = Color.FromArgb(58, 53, 59);
            MinimumSize = new Size(1100, 650);
            Name = "FormUsuarios";
            StartPosition = FormStartPosition.CenterParent;
            Text = "CLÍNICA VETERINARIA — ADMINISTRADOR — GESTIÓN DE USUARIOS";
            Load += FormUsuarios_Load;
            pnlEncabezado.ResumeLayout(false);
            pnlContenido.ResumeLayout(false);
            grpDatos.ResumeLayout(false);
            grpDatos.PerformLayout();
            pnlBotones.ResumeLayout(false);
            pnlListado.ResumeLayout(false);
            pnlListado.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dgvUsuarios).EndInit();
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
        private Panel pnlBotones;
        private Button btnNuevo;
        private Button btnGuardar;
        private Button btnModificar;
        private Button btnCancelar;
        private Button btnDesactivar;
        private Button btnVolver;
        private Panel pnlListado;
        private Label lblBuscar;
        private TextBox txtBuscar;
        private Button btnBuscar;
        private Button btnActivos;
        private Button btnInactivos;
        private DataGridView dgvUsuarios;
        private DataGridViewTextBoxColumn colId;
        private DataGridViewTextBoxColumn colNombre;
        private DataGridViewTextBoxColumn colApellido;
        private DataGridViewTextBoxColumn colDni;
        private DataGridViewTextBoxColumn colCorreoElectronico;
        private DataGridViewTextBoxColumn colDireccion;
        private DataGridViewTextBoxColumn colEstado;
    }
}
