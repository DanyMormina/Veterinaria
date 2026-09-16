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
            lblErrorValidacion = new Label();
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
            colTelefono = new DataGridViewTextBoxColumn();
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
            // pnlEncabezado — alto fijo 48
            // 
            pnlEncabezado.BackColor = Color.FromArgb(200, 138, 150);
            pnlEncabezado.Controls.Add(lblTitulo);
            pnlEncabezado.Controls.Add(lblUsuarioSesion);
            pnlEncabezado.Dock = DockStyle.Top;
            pnlEncabezado.Name = "pnlEncabezado";
            pnlEncabezado.Padding = new Padding(16, 0, 16, 0);
            pnlEncabezado.Size = new Size(1220, 48);
            // 
            // lblTitulo
            // 
            lblTitulo.Dock = DockStyle.Fill;
            lblTitulo.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            lblTitulo.ForeColor = Color.White;
            lblTitulo.Name = "lblTitulo";
            lblTitulo.Text = "CLÍNICA VETERINARIA — GESTIÓN DE USUARIOS";
            lblTitulo.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lblUsuarioSesion
            // 
            lblUsuarioSesion.Dock = DockStyle.Right;
            lblUsuarioSesion.Font = new Font("Segoe UI", 9F);
            lblUsuarioSesion.ForeColor = Color.FromArgb(250, 244, 244);
            lblUsuarioSesion.Name = "lblUsuarioSesion";
            lblUsuarioSesion.Size = new Size(280, 48);
            lblUsuarioSesion.Text = "Usuario: Admin";
            lblUsuarioSesion.TextAlign = ContentAlignment.MiddleRight;
            // 
            // pnlContenido
            // 
            pnlContenido.BackColor = Color.FromArgb(250, 244, 244);
            pnlContenido.Controls.Add(dgvUsuarios);
            pnlContenido.Controls.Add(pnlListado);
            pnlContenido.Controls.Add(pnlBotones);
            pnlContenido.Controls.Add(grpDatos);
            pnlContenido.Dock = DockStyle.Fill;
            pnlContenido.Name = "pnlContenido";
            pnlContenido.Padding = new Padding(12);
            // 
            // grpDatos
            // Columna etiquetas: X=14 W=132
            // Columna campos:    X=154 W=240
            // Ancho total: 14+132+8+240+14 = 408 → usamos 410
            // Filas cada 36px desde Y=28
            // 
            grpDatos.Anchor = AnchorStyles.Top | AnchorStyles.Left;
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
            grpDatos.Controls.Add(lblErrorValidacion);
            grpDatos.Font = new Font("Segoe UI", 9F);
            grpDatos.ForeColor = Color.FromArgb(58, 53, 59);
            grpDatos.Location = new Point(12, 12);
            grpDatos.Name = "grpDatos";
            grpDatos.Size = new Size(410, 448);
            grpDatos.TabStop = false;
            grpDatos.Text = "Datos del usuario";
            // 
            // Fila 0 — Nombre (Y=28)
            // 
            lblNombre.Location = new Point(14, 28);
            lblNombre.Name = "lblNombre";
            lblNombre.Size = new Size(132, 23);
            lblNombre.Text = "Nombre";
            lblNombre.TextAlign = ContentAlignment.MiddleLeft;
            txtNombre.BackColor = Color.White;
            txtNombre.BorderStyle = BorderStyle.FixedSingle;
            txtNombre.Location = new Point(154, 28);
            txtNombre.Name = "txtNombre";
            txtNombre.Size = new Size(240, 23);
            // 
            // Fila 1 — Apellido (Y=64)
            // 
            lblApellido.Location = new Point(14, 64);
            lblApellido.Name = "lblApellido";
            lblApellido.Size = new Size(132, 23);
            lblApellido.Text = "Apellido";
            lblApellido.TextAlign = ContentAlignment.MiddleLeft;
            txtApellido.BackColor = Color.White;
            txtApellido.BorderStyle = BorderStyle.FixedSingle;
            txtApellido.Location = new Point(154, 64);
            txtApellido.Name = "txtApellido";
            txtApellido.Size = new Size(240, 23);
            // 
            // Fila 2 — DNI (Y=100)
            // 
            lblDni.Location = new Point(14, 100);
            lblDni.Name = "lblDni";
            lblDni.Size = new Size(132, 23);
            lblDni.Text = "DNI";
            lblDni.TextAlign = ContentAlignment.MiddleLeft;
            txtDni.BackColor = Color.White;
            txtDni.BorderStyle = BorderStyle.FixedSingle;
            txtDni.Location = new Point(154, 100);
            txtDni.Name = "txtDni";
            txtDni.Size = new Size(240, 23);
            // 
            // Fila 3 — Dirección (Y=136)
            // 
            lblDireccion.Location = new Point(14, 136);
            lblDireccion.Name = "lblDireccion";
            lblDireccion.Size = new Size(132, 23);
            lblDireccion.Text = "Dirección";
            lblDireccion.TextAlign = ContentAlignment.MiddleLeft;
            txtDireccion.BackColor = Color.White;
            txtDireccion.BorderStyle = BorderStyle.FixedSingle;
            txtDireccion.Location = new Point(154, 136);
            txtDireccion.Name = "txtDireccion";
            txtDireccion.Size = new Size(240, 23);
            // 
            // Fila 4 — Teléfono (Y=172)
            // 
            lblTelefono.Location = new Point(14, 172);
            lblTelefono.Name = "lblTelefono";
            lblTelefono.Size = new Size(132, 23);
            lblTelefono.Text = "Teléfono";
            lblTelefono.TextAlign = ContentAlignment.MiddleLeft;
            txtTelefono.BackColor = Color.White;
            txtTelefono.BorderStyle = BorderStyle.FixedSingle;
            txtTelefono.Location = new Point(154, 172);
            txtTelefono.Name = "txtTelefono";
            txtTelefono.Size = new Size(240, 23);
            // 
            // Fila 5 — Correo (Y=208)
            // 
            lblCorreoElectronico.Location = new Point(14, 208);
            lblCorreoElectronico.Name = "lblCorreoElectronico";
            lblCorreoElectronico.Size = new Size(132, 23);
            lblCorreoElectronico.Text = "Correo";
            lblCorreoElectronico.TextAlign = ContentAlignment.MiddleLeft;
            txtCorreoElectronico.BackColor = Color.White;
            txtCorreoElectronico.BorderStyle = BorderStyle.FixedSingle;
            txtCorreoElectronico.Location = new Point(154, 208);
            txtCorreoElectronico.Name = "txtCorreoElectronico";
            txtCorreoElectronico.Size = new Size(240, 23);
            // 
            // Fila 6 — Usuario (Y=244)
            // 
            lblUsuario.Location = new Point(14, 244);
            lblUsuario.Name = "lblUsuario";
            lblUsuario.Size = new Size(132, 23);
            lblUsuario.Text = "Usuario";
            lblUsuario.TextAlign = ContentAlignment.MiddleLeft;
            txtUsuario.BackColor = Color.White;
            txtUsuario.BorderStyle = BorderStyle.FixedSingle;
            txtUsuario.Location = new Point(154, 244);
            txtUsuario.Name = "txtUsuario";
            txtUsuario.Size = new Size(240, 23);
            // 
            // Fila 7 — Contraseña (Y=280)
            // 
            lblContrasena.Location = new Point(14, 280);
            lblContrasena.Name = "lblContrasena";
            lblContrasena.Size = new Size(132, 23);
            lblContrasena.Text = "Contraseña";
            lblContrasena.TextAlign = ContentAlignment.MiddleLeft;
            txtContrasena.BackColor = Color.White;
            txtContrasena.BorderStyle = BorderStyle.FixedSingle;
            txtContrasena.Location = new Point(154, 280);
            txtContrasena.Name = "txtContrasena";
            txtContrasena.Size = new Size(240, 23);
            txtContrasena.UseSystemPasswordChar = true;
            // 
            // Fila 8 — Nacimiento (Y=316) — texto corto para no invadir el campo
            // 
            lblFechaNacimiento.Location = new Point(14, 316);
            lblFechaNacimiento.Name = "lblFechaNacimiento";
            lblFechaNacimiento.Size = new Size(132, 23);
            lblFechaNacimiento.Text = "Nacimiento";
            lblFechaNacimiento.TextAlign = ContentAlignment.MiddleLeft;
            dtpFechaNacimiento.CalendarMonthBackground = Color.White;
            dtpFechaNacimiento.Format = DateTimePickerFormat.Short;
            dtpFechaNacimiento.Location = new Point(154, 316);
            dtpFechaNacimiento.Name = "dtpFechaNacimiento";
            dtpFechaNacimiento.Size = new Size(240, 23);
            // 
            // Fila 9 — Sexo (Y=352)
            // 
            lblSexo.Location = new Point(14, 352);
            lblSexo.Name = "lblSexo";
            lblSexo.Size = new Size(132, 23);
            lblSexo.Text = "Sexo";
            lblSexo.TextAlign = ContentAlignment.MiddleLeft;
            rbHombre.Location = new Point(154, 352);
            rbHombre.Name = "rbHombre";
            rbHombre.Size = new Size(80, 23);
            rbHombre.Text = "Hombre";
            rbMujer.Location = new Point(244, 352);
            rbMujer.Name = "rbMujer";
            rbMujer.Size = new Size(70, 23);
            rbMujer.Text = "Mujer";
            // 
            // Fila 10 — Perfil (Y=388)
            // 
            lblPerfil.Location = new Point(14, 388);
            lblPerfil.Name = "lblPerfil";
            lblPerfil.Size = new Size(132, 23);
            lblPerfil.Text = "Perfil";
            lblPerfil.TextAlign = ContentAlignment.MiddleLeft;
            cboPerfil.BackColor = Color.White;
            cboPerfil.DropDownStyle = ComboBoxStyle.DropDownList;
            cboPerfil.FormattingEnabled = true;
            cboPerfil.Location = new Point(154, 388);
            cboPerfil.Name = "cboPerfil";
            cboPerfil.Size = new Size(240, 23);
            // 
            // lblErrorValidacion — mensaje en rojo debajo de los campos
            // 
            lblErrorValidacion.ForeColor = Color.FromArgb(178, 34, 34);
            lblErrorValidacion.Location = new Point(14, 420);
            lblErrorValidacion.Name = "lblErrorValidacion";
            lblErrorValidacion.Size = new Size(380, 22);
            lblErrorValidacion.Text = "";
            lblErrorValidacion.TextAlign = ContentAlignment.MiddleLeft;
            lblErrorValidacion.Visible = false;
            // 
            // pnlBotones — debajo del groupbox, anclado arriba (no bottom) para no aplastar
            // 
            pnlBotones.Anchor = AnchorStyles.Top | AnchorStyles.Left;
            pnlBotones.Controls.Add(btnNuevo);
            pnlBotones.Controls.Add(btnGuardar);
            pnlBotones.Controls.Add(btnModificar);
            pnlBotones.Controls.Add(btnCancelar);
            pnlBotones.Controls.Add(btnVolver);
            pnlBotones.Controls.Add(btnDesactivar);
            pnlBotones.Location = new Point(12, 468);
            pnlBotones.Name = "pnlBotones";
            pnlBotones.Size = new Size(410, 78);
            // 
            // Botones fila 1: 4 x 98 con gap 6 → 4*98+3*6 = 410
            // 
            btnNuevo.BackColor = Color.FromArgb(230, 196, 202);
            btnNuevo.FlatAppearance.BorderColor = Color.FromArgb(186, 162, 168);
            btnNuevo.FlatStyle = FlatStyle.Flat;
            btnNuevo.ForeColor = Color.FromArgb(58, 53, 59);
            btnNuevo.Location = new Point(0, 4);
            btnNuevo.Name = "btnNuevo";
            btnNuevo.Size = new Size(98, 32);
            btnNuevo.Text = "Nuevo";
            btnNuevo.UseVisualStyleBackColor = false;
            btnNuevo.Click += btnNuevo_Click;

            btnGuardar.BackColor = Color.FromArgb(72, 148, 96);
            btnGuardar.FlatAppearance.BorderColor = Color.FromArgb(48, 118, 74);
            btnGuardar.FlatStyle = FlatStyle.Flat;
            btnGuardar.ForeColor = Color.White;
            btnGuardar.Location = new Point(104, 4);
            btnGuardar.Name = "btnGuardar";
            btnGuardar.Size = new Size(98, 32);
            btnGuardar.Text = "Guardar";
            btnGuardar.UseVisualStyleBackColor = false;
            btnGuardar.Click += btnGuardar_Click;

            btnModificar.BackColor = Color.FromArgb(64, 126, 186);
            btnModificar.FlatAppearance.BorderColor = Color.FromArgb(44, 102, 158);
            btnModificar.FlatStyle = FlatStyle.Flat;
            btnModificar.ForeColor = Color.White;
            btnModificar.Location = new Point(208, 4);
            btnModificar.Name = "btnModificar";
            btnModificar.Size = new Size(98, 32);
            btnModificar.Text = "Modificar";
            btnModificar.UseVisualStyleBackColor = false;
            btnModificar.Click += btnModificar_Click;

            btnCancelar.BackColor = Color.FromArgb(196, 78, 86);
            btnCancelar.FlatAppearance.BorderColor = Color.FromArgb(164, 54, 62);
            btnCancelar.FlatStyle = FlatStyle.Flat;
            btnCancelar.ForeColor = Color.White;
            btnCancelar.Location = new Point(312, 4);
            btnCancelar.Name = "btnCancelar";
            btnCancelar.Size = new Size(98, 32);
            btnCancelar.Text = "Cancelar";
            btnCancelar.UseVisualStyleBackColor = false;
            btnCancelar.Click += btnCancelar_Click;
            // 
            // Botones fila 2: 2 x 202 con gap 6 → 410
            // 
            btnVolver.BackColor = Color.FromArgb(220, 200, 204);
            btnVolver.FlatAppearance.BorderColor = Color.FromArgb(186, 162, 168);
            btnVolver.FlatStyle = FlatStyle.Flat;
            btnVolver.ForeColor = Color.FromArgb(58, 53, 59);
            btnVolver.Location = new Point(0, 42);
            btnVolver.Name = "btnVolver";
            btnVolver.Size = new Size(202, 32);
            btnVolver.Text = "Volver al panel";
            btnVolver.UseVisualStyleBackColor = false;
            btnVolver.Click += btnVolver_Click;

            btnDesactivar.BackColor = Color.FromArgb(196, 78, 86);
            btnDesactivar.FlatAppearance.BorderColor = Color.FromArgb(164, 54, 62);
            btnDesactivar.FlatStyle = FlatStyle.Flat;
            btnDesactivar.ForeColor = Color.White;
            btnDesactivar.Location = new Point(208, 42);
            btnDesactivar.Name = "btnDesactivar";
            btnDesactivar.Size = new Size(202, 32);
            btnDesactivar.Text = "Desactivar";
            btnDesactivar.UseVisualStyleBackColor = false;
            btnDesactivar.Click += btnDesactivar_Click;
            // 
            // pnlListado — a la derecha del formulario
            // X = 12+410+12 = 434
            // 
            pnlListado.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            pnlListado.BorderStyle = BorderStyle.FixedSingle;
            pnlListado.Controls.Add(lblBuscar);
            pnlListado.Controls.Add(txtBuscar);
            pnlListado.Controls.Add(btnBuscar);
            pnlListado.Controls.Add(btnActivos);
            pnlListado.Controls.Add(btnInactivos);
            pnlListado.Controls.Add(dgvUsuarios);
            pnlListado.Location = new Point(434, 12);
            pnlListado.Name = "pnlListado";
            pnlListado.Size = new Size(762, 634);
            // 
            // Barra de búsqueda
            // 
            lblBuscar.Location = new Point(10, 12);
            lblBuscar.Name = "lblBuscar";
            lblBuscar.Size = new Size(50, 23);
            lblBuscar.Text = "Buscar";
            lblBuscar.TextAlign = ContentAlignment.MiddleLeft;

            txtBuscar.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            txtBuscar.BackColor = Color.White;
            txtBuscar.BorderStyle = BorderStyle.FixedSingle;
            txtBuscar.Location = new Point(64, 12);
            txtBuscar.Name = "txtBuscar";
            txtBuscar.Size = new Size(360, 23);

            btnBuscar.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnBuscar.BackColor = Color.FromArgb(220, 200, 204);
            btnBuscar.FlatAppearance.BorderColor = Color.FromArgb(186, 162, 168);
            btnBuscar.FlatStyle = FlatStyle.Flat;
            btnBuscar.ForeColor = Color.FromArgb(58, 53, 59);
            btnBuscar.Location = new Point(436, 10);
            btnBuscar.Name = "btnBuscar";
            btnBuscar.Size = new Size(90, 27);
            btnBuscar.Text = "Buscar";
            btnBuscar.UseVisualStyleBackColor = false;
            btnBuscar.Click += btnBuscar_Click;

            btnActivos.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnActivos.BackColor = Color.FromArgb(220, 200, 204);
            btnActivos.FlatAppearance.BorderColor = Color.FromArgb(186, 162, 168);
            btnActivos.FlatStyle = FlatStyle.Flat;
            btnActivos.ForeColor = Color.FromArgb(58, 53, 59);
            btnActivos.Location = new Point(532, 10);
            btnActivos.Name = "btnActivos";
            btnActivos.Size = new Size(100, 27);
            btnActivos.Text = "Activos";
            btnActivos.UseVisualStyleBackColor = false;
            btnActivos.Click += btnActivos_Click;

            btnInactivos.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnInactivos.BackColor = Color.FromArgb(220, 200, 204);
            btnInactivos.FlatAppearance.BorderColor = Color.FromArgb(186, 162, 168);
            btnInactivos.FlatStyle = FlatStyle.Flat;
            btnInactivos.ForeColor = Color.FromArgb(58, 53, 59);
            btnInactivos.Location = new Point(638, 10);
            btnInactivos.Name = "btnInactivos";
            btnInactivos.Size = new Size(110, 27);
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
            dgvUsuarios.Columns.AddRange(new DataGridViewColumn[]
            {
                colId, colNombre, colApellido, colDni, colTelefono, colCorreoElectronico, colDireccion, colEstado
            });
            dgvUsuarios.Location = new Point(10, 46);
            dgvUsuarios.MultiSelect = false;
            dgvUsuarios.Name = "dgvUsuarios";
            dgvUsuarios.ReadOnly = true;
            dgvUsuarios.RowHeadersVisible = false;
            dgvUsuarios.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvUsuarios.Size = new Size(740, 576);
            dgvUsuarios.CellClick += dgvUsuarios_CellClick;

            colId.HeaderText = "ID";
            colId.Name = "colId";
            colId.Visible = false;

            colNombre.FillWeight = 90F;
            colNombre.HeaderText = "Nombre";
            colNombre.MinimumWidth = 80;
            colNombre.Name = "colNombre";

            colApellido.FillWeight = 90F;
            colApellido.HeaderText = "Apellido";
            colApellido.MinimumWidth = 80;
            colApellido.Name = "colApellido";

            colDni.FillWeight = 70F;
            colDni.HeaderText = "DNI";
            colDni.MinimumWidth = 70;
            colDni.Name = "colDni";

            colTelefono.FillWeight = 85F;
            colTelefono.HeaderText = "Teléfono";
            colTelefono.MinimumWidth = 80;
            colTelefono.Name = "colTelefono";

            colCorreoElectronico.FillWeight = 120F;
            colCorreoElectronico.HeaderText = "Correo";
            colCorreoElectronico.MinimumWidth = 100;
            colCorreoElectronico.Name = "colCorreoElectronico";

            colDireccion.FillWeight = 120F;
            colDireccion.HeaderText = "Dirección";
            colDireccion.MinimumWidth = 100;
            colDireccion.Name = "colDireccion";

            colEstado.FillWeight = 60F;
            colEstado.HeaderText = "Estado";
            colEstado.MinimumWidth = 60;
            colEstado.Name = "colEstado";
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
            MinimumSize = new Size(1100, 680);
            Name = "FormUsuarios";
            StartPosition = FormStartPosition.CenterParent;
            Text = "CLÍNICA VETERINARIA — GESTIÓN DE USUARIOS";
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
        private Label lblErrorValidacion;
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
        private DataGridViewTextBoxColumn colTelefono;
        private DataGridViewTextBoxColumn colCorreoElectronico;
        private DataGridViewTextBoxColumn colDireccion;
        private DataGridViewTextBoxColumn colEstado;
    }
}
