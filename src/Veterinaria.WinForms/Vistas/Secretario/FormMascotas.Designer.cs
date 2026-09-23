namespace Veterinaria.WinForms.Vistas.Secretario;

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
        DataGridViewCellStyle dataGridViewCellStyle3 = new DataGridViewCellStyle();
        DataGridViewCellStyle dataGridViewCellStyle4 = new DataGridViewCellStyle();
        pnlEncabezado = new Panel();
        lblTitulo = new Label();
        lblUsuarioSesion = new Label();
        pnlContenido = new Panel();
        pnlBotones = new Panel();
        btnGuardar = new Button();
        btnModificar = new Button();
        btnLimpiar = new Button();
        btnVolver = new Button();
        grpDatos = new GroupBox();
        lblNombre = new Label();
        txtNombre = new TextBox();
        lblPropietario = new Label();
        cboPropietario = new ComboBox();
        btnNuevoPropietario = new Button();
        lblEspecie = new Label();
        cboEspecie = new ComboBox();
        lblRaza = new Label();
        cboRaza = new ComboBox();
        btnAltaEspecie = new Button();
        btnModEspecie = new Button();
        btnAltaRaza = new Button();
        btnModRaza = new Button();
        lblSexo = new Label();
        pnlSexo = new Panel();
        rbMacho = new RadioButton();
        rbHembra = new RadioButton();
        lblFechaNacimiento = new Label();
        dtpFechaNacimiento = new DateTimePicker();
        lblColor = new Label();
        txtColor = new TextBox();
        pnlListado = new Panel();
        lblBuscar = new Label();
        txtBuscar = new TextBox();
        btnBuscar = new Button();
        lblFiltroEspecie = new Label();
        cboFiltroEspecie = new ComboBox();
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
        lblTotalRegistros = new Label();
        btnActivar = new Button();
        btnDesactivar = new Button();
        barraEstado = new StatusStrip();
        lblInfoEstado = new ToolStripStatusLabel();
        pnlEncabezado.SuspendLayout();
        pnlContenido.SuspendLayout();
        pnlBotones.SuspendLayout();
        grpDatos.SuspendLayout();
        pnlSexo.SuspendLayout();
        pnlListado.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)dgvMascotas).BeginInit();
        barraEstado.SuspendLayout();
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
        pnlEncabezado.Size = new Size(1100, 50);
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
        lblTitulo.Text = "CLÍNICA VETERINARIA — RECEPCIÓN — GESTIÓN DE MASCOTAS";
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
        lblUsuarioSesion.Text = "Recepción: Secretario";
        lblUsuarioSesion.TextAlign = ContentAlignment.MiddleRight;
        // 
        // pnlContenido
        // 
        pnlContenido.BackColor = Color.FromArgb(250, 244, 244);
        pnlContenido.Controls.Add(pnlBotones);
        pnlContenido.Controls.Add(grpDatos);
        pnlContenido.Controls.Add(pnlListado);
        pnlContenido.Dock = DockStyle.Fill;
        pnlContenido.Location = new Point(0, 50);
        pnlContenido.Name = "pnlContenido";
        pnlContenido.Size = new Size(1100, 628);
        pnlContenido.TabIndex = 1;
        // 
        // pnlBotones
        // 
        pnlBotones.Controls.Add(btnGuardar);
        pnlBotones.Controls.Add(btnModificar);
        pnlBotones.Controls.Add(btnLimpiar);
        pnlBotones.Controls.Add(btnVolver);
        pnlBotones.Location = new Point(16, 172);
        pnlBotones.Name = "pnlBotones";
        pnlBotones.Size = new Size(1068, 40);
        pnlBotones.TabIndex = 3;
        // 
        // btnGuardar
        // 
        btnGuardar.BackColor = Color.FromArgb(152, 196, 164);
        btnGuardar.Cursor = Cursors.Hand;
        btnGuardar.FlatAppearance.BorderSize = 0;
        btnGuardar.FlatStyle = FlatStyle.Flat;
        btnGuardar.Font = new Font("Segoe UI", 9F);
        btnGuardar.ForeColor = Color.Black;
        btnGuardar.Location = new Point(11, 3);
        btnGuardar.Name = "btnGuardar";
        btnGuardar.Size = new Size(100, 32);
        btnGuardar.TabIndex = 1;
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
        btnModificar.Location = new Point(119, 3);
        btnModificar.Name = "btnModificar";
        btnModificar.Size = new Size(100, 32);
        btnModificar.TabIndex = 2;
        btnModificar.Text = "Modificar";
        btnModificar.UseVisualStyleBackColor = false;
        // 
        // btnLimpiar
        // 
        btnLimpiar.BackColor = Color.FromArgb(226, 217, 220);
        btnLimpiar.Cursor = Cursors.Hand;
        btnLimpiar.FlatAppearance.BorderSize = 0;
        btnLimpiar.FlatStyle = FlatStyle.Flat;
        btnLimpiar.Font = new Font("Segoe UI", 9F);
        btnLimpiar.ForeColor = Color.FromArgb(58, 53, 59);
        btnLimpiar.Location = new Point(227, 3);
        btnLimpiar.Name = "btnLimpiar";
        btnLimpiar.Size = new Size(100, 32);
        btnLimpiar.TabIndex = 3;
        btnLimpiar.Text = "Limpiar";
        btnLimpiar.UseVisualStyleBackColor = false;
        // 
        // btnVolver
        // 
        btnVolver.BackColor = Color.FromArgb(220, 200, 204);
        btnVolver.FlatAppearance.BorderColor = Color.FromArgb(186, 162, 168);
        btnVolver.FlatStyle = FlatStyle.Flat;
        btnVolver.Font = new Font("Segoe UI", 9F);
        btnVolver.ForeColor = Color.Black;
        btnVolver.Location = new Point(335, 3);
        btnVolver.Name = "btnVolver";
        btnVolver.Size = new Size(140, 32);
        btnVolver.TabIndex = 4;
        btnVolver.Text = "Volver al panel";
        btnVolver.UseVisualStyleBackColor = false;
        btnVolver.Click += btnVolver_Click;
        // 
        // grpDatos
        // 
        grpDatos.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
        grpDatos.Controls.Add(lblNombre);
        grpDatos.Controls.Add(txtNombre);
        grpDatos.Controls.Add(lblPropietario);
        grpDatos.Controls.Add(cboPropietario);
        grpDatos.Controls.Add(btnNuevoPropietario);
        grpDatos.Controls.Add(lblEspecie);
        grpDatos.Controls.Add(cboEspecie);
        grpDatos.Controls.Add(lblRaza);
        grpDatos.Controls.Add(cboRaza);
        grpDatos.Controls.Add(btnAltaEspecie);
        grpDatos.Controls.Add(btnModEspecie);
        grpDatos.Controls.Add(btnAltaRaza);
        grpDatos.Controls.Add(btnModRaza);
        grpDatos.Controls.Add(lblSexo);
        grpDatos.Controls.Add(pnlSexo);
        grpDatos.Controls.Add(lblFechaNacimiento);
        grpDatos.Controls.Add(dtpFechaNacimiento);
        grpDatos.Controls.Add(lblColor);
        grpDatos.Controls.Add(txtColor);
        grpDatos.Font = new Font("Segoe UI", 9F);
        grpDatos.ForeColor = Color.FromArgb(58, 53, 59);
        grpDatos.Location = new Point(16, 16);
        grpDatos.Name = "grpDatos";
        grpDatos.Size = new Size(1068, 150);
        grpDatos.TabIndex = 0;
        grpDatos.TabStop = false;
        grpDatos.Text = "Datos de la mascota";
        grpDatos.Enter += grpDatos_Enter;
        // 
        // lblNombre
        // 
        lblNombre.Location = new Point(12, 26);
        lblNombre.Name = "lblNombre";
        lblNombre.Size = new Size(65, 23);
        lblNombre.TabIndex = 0;
        lblNombre.Text = "Nombre";
        lblNombre.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // txtNombre
        // 
        txtNombre.BackColor = Color.White;
        txtNombre.BorderStyle = BorderStyle.FixedSingle;
        txtNombre.Font = new Font("Segoe UI", 9F);
        txtNombre.ForeColor = Color.FromArgb(58, 53, 59);
        txtNombre.Location = new Point(99, 27);
        txtNombre.Name = "txtNombre";
        txtNombre.Size = new Size(377, 23);
        txtNombre.TabIndex = 1;
        // 
        // lblPropietario
        // 
        lblPropietario.Location = new Point(500, 26);
        lblPropietario.Name = "lblPropietario";
        lblPropietario.Size = new Size(90, 23);
        lblPropietario.TabIndex = 2;
        lblPropietario.Text = "Propietario";
        lblPropietario.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // cboPropietario
        // 
        cboPropietario.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
        cboPropietario.AutoCompleteSource = AutoCompleteSource.ListItems;
        cboPropietario.BackColor = Color.White;
        cboPropietario.Font = new Font("Segoe UI", 9F);
        cboPropietario.ForeColor = Color.FromArgb(58, 53, 59);
        cboPropietario.FormattingEnabled = true;
        cboPropietario.Location = new Point(594, 26);
        cboPropietario.Name = "cboPropietario";
        cboPropietario.Size = new Size(320, 23);
        cboPropietario.TabIndex = 3;
        // 
        // btnNuevoPropietario
        // 
        btnNuevoPropietario.BackColor = Color.FromArgb(200, 138, 150);
        btnNuevoPropietario.Cursor = Cursors.Hand;
        btnNuevoPropietario.FlatAppearance.BorderSize = 0;
        btnNuevoPropietario.FlatStyle = FlatStyle.Flat;
        btnNuevoPropietario.Font = new Font("Segoe UI", 8.25F, FontStyle.Bold);
        btnNuevoPropietario.ForeColor = Color.White;
        btnNuevoPropietario.Location = new Point(922, 25);
        btnNuevoPropietario.Name = "btnNuevoPropietario";
        btnNuevoPropietario.Size = new Size(118, 27);
        btnNuevoPropietario.TabIndex = 4;
        btnNuevoPropietario.Text = "+ Propietario";
        btnNuevoPropietario.UseVisualStyleBackColor = false;
        // 
        // lblEspecie
        // 
        lblEspecie.Location = new Point(12, 66);
        lblEspecie.Name = "lblEspecie";
        lblEspecie.Size = new Size(65, 23);
        lblEspecie.TabIndex = 5;
        lblEspecie.Text = "Especie";
        lblEspecie.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // cboEspecie
        // 
        cboEspecie.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
        cboEspecie.AutoCompleteSource = AutoCompleteSource.ListItems;
        cboEspecie.BackColor = Color.White;
        cboEspecie.DropDownStyle = ComboBoxStyle.DropDownList;
        cboEspecie.Font = new Font("Segoe UI", 9F);
        cboEspecie.ForeColor = Color.FromArgb(58, 53, 59);
        cboEspecie.FormattingEnabled = true;
        cboEspecie.Location = new Point(99, 65);
        cboEspecie.Name = "cboEspecie";
        cboEspecie.Size = new Size(160, 23);
        cboEspecie.TabIndex = 6;
        // 
        // lblRaza
        // 
        lblRaza.Location = new Point(279, 66);
        lblRaza.Name = "lblRaza";
        lblRaza.Size = new Size(44, 23);
        lblRaza.TabIndex = 7;
        lblRaza.Text = "Raza";
        lblRaza.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // cboRaza
        // 
        cboRaza.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
        cboRaza.AutoCompleteSource = AutoCompleteSource.ListItems;
        cboRaza.BackColor = Color.White;
        cboRaza.DropDownStyle = ComboBoxStyle.DropDownList;
        cboRaza.Font = new Font("Segoe UI", 9F);
        cboRaza.ForeColor = Color.FromArgb(58, 53, 59);
        cboRaza.FormattingEnabled = true;
        cboRaza.Location = new Point(331, 66);
        cboRaza.Name = "cboRaza";
        cboRaza.Size = new Size(145, 23);
        cboRaza.TabIndex = 8;
        // 
        // btnAltaEspecie
        // 
        btnAltaEspecie.BackColor = Color.FromArgb(200, 138, 150);
        btnAltaEspecie.Cursor = Cursors.Hand;
        btnAltaEspecie.FlatAppearance.BorderSize = 0;
        btnAltaEspecie.FlatStyle = FlatStyle.Flat;
        btnAltaEspecie.Font = new Font("Segoe UI", 8.25F, FontStyle.Bold);
        btnAltaEspecie.ForeColor = Color.White;
        btnAltaEspecie.Location = new Point(99, 104);
        btnAltaEspecie.Name = "btnAltaEspecie";
        btnAltaEspecie.Size = new Size(78, 24);
        btnAltaEspecie.TabIndex = 9;
        btnAltaEspecie.Text = "Alta Esp.";
        btnAltaEspecie.UseVisualStyleBackColor = false;
        // 
        // btnModEspecie
        // 
        btnModEspecie.BackColor = Color.FromArgb(226, 217, 220);
        btnModEspecie.Cursor = Cursors.Hand;
        btnModEspecie.FlatAppearance.BorderSize = 0;
        btnModEspecie.FlatStyle = FlatStyle.Flat;
        btnModEspecie.Font = new Font("Segoe UI", 8.25F);
        btnModEspecie.ForeColor = Color.FromArgb(58, 53, 59);
        btnModEspecie.Location = new Point(181, 104);
        btnModEspecie.Name = "btnModEspecie";
        btnModEspecie.Size = new Size(78, 24);
        btnModEspecie.TabIndex = 10;
        btnModEspecie.Text = "Mod. Esp.";
        btnModEspecie.UseVisualStyleBackColor = false;
        // 
        // btnAltaRaza
        // 
        btnAltaRaza.BackColor = Color.FromArgb(200, 138, 150);
        btnAltaRaza.Cursor = Cursors.Hand;
        btnAltaRaza.FlatAppearance.BorderSize = 0;
        btnAltaRaza.FlatStyle = FlatStyle.Flat;
        btnAltaRaza.Font = new Font("Segoe UI", 8.25F, FontStyle.Bold);
        btnAltaRaza.ForeColor = Color.White;
        btnAltaRaza.Location = new Point(315, 104);
        btnAltaRaza.Name = "btnAltaRaza";
        btnAltaRaza.Size = new Size(78, 24);
        btnAltaRaza.TabIndex = 11;
        btnAltaRaza.Text = "Alta Raza";
        btnAltaRaza.UseVisualStyleBackColor = false;
        // 
        // btnModRaza
        // 
        btnModRaza.BackColor = Color.FromArgb(226, 217, 220);
        btnModRaza.Cursor = Cursors.Hand;
        btnModRaza.FlatAppearance.BorderSize = 0;
        btnModRaza.FlatStyle = FlatStyle.Flat;
        btnModRaza.Font = new Font("Segoe UI", 8.25F);
        btnModRaza.ForeColor = Color.FromArgb(58, 53, 59);
        btnModRaza.Location = new Point(397, 104);
        btnModRaza.Name = "btnModRaza";
        btnModRaza.Size = new Size(78, 24);
        btnModRaza.TabIndex = 12;
        btnModRaza.Text = "Mod. Raza";
        btnModRaza.UseVisualStyleBackColor = false;
        // 
        // lblSexo
        // 
        lblSexo.Location = new Point(500, 66);
        lblSexo.Name = "lblSexo";
        lblSexo.Size = new Size(90, 23);
        lblSexo.TabIndex = 13;
        lblSexo.Text = "Sexo";
        lblSexo.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // pnlSexo
        // 
        pnlSexo.BackColor = Color.Transparent;
        pnlSexo.Controls.Add(rbMacho);
        pnlSexo.Controls.Add(rbHembra);
        pnlSexo.Location = new Point(594, 66);
        pnlSexo.Name = "pnlSexo";
        pnlSexo.Size = new Size(157, 25);
        pnlSexo.TabIndex = 14;
        // 
        // rbMacho
        // 
        rbMacho.AutoSize = true;
        rbMacho.Cursor = Cursors.Hand;
        rbMacho.ForeColor = Color.FromArgb(58, 53, 59);
        rbMacho.Location = new Point(4, 3);
        rbMacho.Name = "rbMacho";
        rbMacho.Size = new Size(62, 19);
        rbMacho.TabIndex = 0;
        rbMacho.TabStop = true;
        rbMacho.Text = "Macho";
        rbMacho.UseVisualStyleBackColor = true;
        // 
        // rbHembra
        // 
        rbHembra.AutoSize = true;
        rbHembra.Cursor = Cursors.Hand;
        rbHembra.ForeColor = Color.FromArgb(58, 53, 59);
        rbHembra.Location = new Point(80, 3);
        rbHembra.Name = "rbHembra";
        rbHembra.Size = new Size(68, 19);
        rbHembra.TabIndex = 1;
        rbHembra.TabStop = true;
        rbHembra.Text = "Hembra";
        rbHembra.UseVisualStyleBackColor = true;
        // 
        // lblFechaNacimiento
        // 
        lblFechaNacimiento.Location = new Point(796, 105);
        lblFechaNacimiento.Name = "lblFechaNacimiento";
        lblFechaNacimiento.Size = new Size(118, 23);
        lblFechaNacimiento.TabIndex = 15;
        lblFechaNacimiento.Text = "Fecha de nacimiento";
        lblFechaNacimiento.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // dtpFechaNacimiento
        // 
        dtpFechaNacimiento.Font = new Font("Segoe UI", 9F);
        dtpFechaNacimiento.Format = DateTimePickerFormat.Short;
        dtpFechaNacimiento.Location = new Point(920, 103);
        dtpFechaNacimiento.Name = "dtpFechaNacimiento";
        dtpFechaNacimiento.Size = new Size(103, 23);
        dtpFechaNacimiento.TabIndex = 16;
        // 
        // lblColor
        // 
        lblColor.Location = new Point(498, 104);
        lblColor.Name = "lblColor";
        lblColor.Size = new Size(90, 23);
        lblColor.TabIndex = 17;
        lblColor.Text = "Color";
        lblColor.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // txtColor
        // 
        txtColor.BackColor = Color.White;
        txtColor.BorderStyle = BorderStyle.FixedSingle;
        txtColor.Font = new Font("Segoe UI", 9F);
        txtColor.ForeColor = Color.FromArgb(58, 53, 59);
        txtColor.Location = new Point(594, 104);
        txtColor.Name = "txtColor";
        txtColor.Size = new Size(157, 23);
        txtColor.TabIndex = 18;
        txtColor.TextChanged += txtColor_TextChanged;
        // 
        // pnlListado
        // 
        pnlListado.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        pnlListado.BackColor = Color.White;
        pnlListado.BorderStyle = BorderStyle.FixedSingle;
        pnlListado.Controls.Add(lblBuscar);
        pnlListado.Controls.Add(txtBuscar);
        pnlListado.Controls.Add(btnBuscar);
        pnlListado.Controls.Add(lblFiltroEspecie);
        pnlListado.Controls.Add(cboFiltroEspecie);
        pnlListado.Controls.Add(dgvMascotas);
        pnlListado.Controls.Add(lblTotalRegistros);
        pnlListado.Controls.Add(btnActivar);
        pnlListado.Controls.Add(btnDesactivar);
        pnlListado.Location = new Point(16, 218);
        pnlListado.Name = "pnlListado";
        pnlListado.Size = new Size(1072, 399);
        pnlListado.TabIndex = 2;
        // 
        // lblBuscar
        // 
        lblBuscar.Location = new Point(10, 14);
        lblBuscar.Name = "lblBuscar";
        lblBuscar.Size = new Size(52, 23);
        lblBuscar.TabIndex = 0;
        lblBuscar.Text = "Buscar";
        lblBuscar.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // txtBuscar
        // 
        txtBuscar.BackColor = Color.White;
        txtBuscar.BorderStyle = BorderStyle.FixedSingle;
        txtBuscar.Location = new Point(64, 14);
        txtBuscar.Name = "txtBuscar";
        txtBuscar.PlaceholderText = "Nombre de la mascota...";
        txtBuscar.Size = new Size(350, 23);
        txtBuscar.TabIndex = 1;
        // 
        // btnBuscar
        // 
        btnBuscar.BackColor = Color.FromArgb(226, 217, 220);
        btnBuscar.Cursor = Cursors.Hand;
        btnBuscar.FlatAppearance.BorderColor = Color.FromArgb(186, 162, 168);
        btnBuscar.FlatStyle = FlatStyle.Flat;
        btnBuscar.ForeColor = Color.FromArgb(58, 53, 59);
        btnBuscar.Location = new Point(420, 12);
        btnBuscar.Name = "btnBuscar";
        btnBuscar.Size = new Size(78, 27);
        btnBuscar.TabIndex = 2;
        btnBuscar.Text = "Buscar";
        btnBuscar.UseVisualStyleBackColor = false;
        // 
        // lblFiltroEspecie
        // 
        lblFiltroEspecie.Location = new Point(514, 14);
        lblFiltroEspecie.Name = "lblFiltroEspecie";
        lblFiltroEspecie.Size = new Size(52, 23);
        lblFiltroEspecie.TabIndex = 3;
        lblFiltroEspecie.Text = "Especie";
        lblFiltroEspecie.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // cboFiltroEspecie
        // 
        cboFiltroEspecie.BackColor = Color.White;
        cboFiltroEspecie.DropDownStyle = ComboBoxStyle.DropDownList;
        cboFiltroEspecie.FormattingEnabled = true;
        cboFiltroEspecie.Location = new Point(570, 14);
        cboFiltroEspecie.Name = "cboFiltroEspecie";
        cboFiltroEspecie.Size = new Size(180, 23);
        cboFiltroEspecie.TabIndex = 4;
        // 
        // dgvMascotas
        // 
        dgvMascotas.AllowUserToAddRows = false;
        dgvMascotas.AllowUserToDeleteRows = false;
        dgvMascotas.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        dgvMascotas.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        dgvMascotas.BackgroundColor = Color.White;
        dataGridViewCellStyle3.Alignment = DataGridViewContentAlignment.MiddleLeft;
        dataGridViewCellStyle3.BackColor = SystemColors.Control;
        dataGridViewCellStyle3.Font = new Font("Segoe UI", 9F);
        dataGridViewCellStyle3.ForeColor = SystemColors.WindowText;
        dataGridViewCellStyle3.SelectionBackColor = SystemColors.Highlight;
        dataGridViewCellStyle3.SelectionForeColor = SystemColors.HighlightText;
        dataGridViewCellStyle3.WrapMode = DataGridViewTriState.True;
        dgvMascotas.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle3;
        dgvMascotas.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
        dgvMascotas.Columns.AddRange(new DataGridViewColumn[] { colId, colNombre, colPropietario, colEspecie, colRaza, colSexo, colFechaNacimiento, colColor, colEstado });
        dataGridViewCellStyle4.Alignment = DataGridViewContentAlignment.MiddleLeft;
        dataGridViewCellStyle4.BackColor = SystemColors.Window;
        dataGridViewCellStyle4.Font = new Font("Segoe UI", 9F);
        dataGridViewCellStyle4.ForeColor = Color.FromArgb(58, 53, 59);
        dataGridViewCellStyle4.SelectionBackColor = SystemColors.Highlight;
        dataGridViewCellStyle4.SelectionForeColor = SystemColors.HighlightText;
        dataGridViewCellStyle4.WrapMode = DataGridViewTriState.False;
        dgvMascotas.DefaultCellStyle = dataGridViewCellStyle4;
        dgvMascotas.Location = new Point(10, 48);
        dgvMascotas.MultiSelect = false;
        dgvMascotas.Name = "dgvMascotas";
        dgvMascotas.ReadOnly = true;
        dgvMascotas.RowHeadersVisible = false;
        dgvMascotas.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        dgvMascotas.Size = new Size(1050, 311);
        dgvMascotas.TabIndex = 7;
        dgvMascotas.RowsAdded += dgvMascotas_RowsAdded;
        dgvMascotas.RowsRemoved += dgvMascotas_RowsRemoved;
        // 
        // colId
        // 
        colId.HeaderText = "ID";
        colId.Name = "colId";
        colId.ReadOnly = true;
        // 
        // colNombre
        // 
        colNombre.HeaderText = "Nombre";
        colNombre.Name = "colNombre";
        colNombre.ReadOnly = true;
        // 
        // colPropietario
        // 
        colPropietario.HeaderText = "Propietario";
        colPropietario.Name = "colPropietario";
        colPropietario.ReadOnly = true;
        // 
        // colEspecie
        // 
        colEspecie.HeaderText = "Especie";
        colEspecie.Name = "colEspecie";
        colEspecie.ReadOnly = true;
        // 
        // colRaza
        // 
        colRaza.HeaderText = "Raza";
        colRaza.Name = "colRaza";
        colRaza.ReadOnly = true;
        // 
        // colSexo
        // 
        colSexo.HeaderText = "Sexo";
        colSexo.Name = "colSexo";
        colSexo.ReadOnly = true;
        // 
        // colFechaNacimiento
        // 
        colFechaNacimiento.HeaderText = "Fecha de nacimiento";
        colFechaNacimiento.Name = "colFechaNacimiento";
        colFechaNacimiento.ReadOnly = true;
        // 
        // colColor
        // 
        colColor.HeaderText = "Color";
        colColor.Name = "colColor";
        colColor.ReadOnly = true;
        // 
        // colEstado
        // 
        colEstado.HeaderText = "Estado";
        colEstado.Name = "colEstado";
        colEstado.ReadOnly = true;
        // 
        // lblTotalRegistros
        // 
        lblTotalRegistros.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
        lblTotalRegistros.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
        lblTotalRegistros.ForeColor = Color.FromArgb(58, 53, 59);
        lblTotalRegistros.Location = new Point(10, 367);
        lblTotalRegistros.Name = "lblTotalRegistros";
        lblTotalRegistros.Size = new Size(300, 23);
        lblTotalRegistros.TabIndex = 8;
        lblTotalRegistros.Text = "Total de registros: 0";
        lblTotalRegistros.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // btnActivar
        // 
        btnActivar.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
        btnActivar.BackColor = Color.FromArgb(226, 217, 220);
        btnActivar.Cursor = Cursors.Hand;
        btnActivar.Enabled = false;
        btnActivar.FlatAppearance.BorderSize = 0;
        btnActivar.FlatStyle = FlatStyle.Flat;
        btnActivar.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
        btnActivar.ForeColor = Color.FromArgb(136, 136, 136);
        btnActivar.Location = new Point(840, 363);
        btnActivar.Name = "btnActivar";
        btnActivar.Size = new Size(105, 30);
        btnActivar.TabIndex = 9;
        btnActivar.Text = "Activar";
        btnActivar.UseVisualStyleBackColor = false;
        // 
        // btnDesactivar
        // 
        btnDesactivar.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
        btnDesactivar.BackColor = Color.FromArgb(226, 217, 220);
        btnDesactivar.Cursor = Cursors.Hand;
        btnDesactivar.Enabled = false;
        btnDesactivar.FlatAppearance.BorderSize = 0;
        btnDesactivar.FlatStyle = FlatStyle.Flat;
        btnDesactivar.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
        btnDesactivar.ForeColor = Color.FromArgb(136, 136, 136);
        btnDesactivar.Location = new Point(955, 363);
        btnDesactivar.Name = "btnDesactivar";
        btnDesactivar.Size = new Size(105, 30);
        btnDesactivar.TabIndex = 10;
        btnDesactivar.Text = "Desactivar";
        btnDesactivar.UseVisualStyleBackColor = false;
        // 
        // barraEstado
        // 
        barraEstado.BackColor = Color.FromArgb(249, 240, 242);
        barraEstado.Items.AddRange(new ToolStripItem[] { lblInfoEstado });
        barraEstado.Location = new Point(0, 678);
        barraEstado.Name = "barraEstado";
        barraEstado.Size = new Size(1100, 22);
        barraEstado.TabIndex = 2;
        // 
        // lblInfoEstado
        // 
        lblInfoEstado.Font = new Font("Segoe UI", 8.25F);
        lblInfoEstado.ForeColor = Color.FromArgb(58, 53, 59);
        lblInfoEstado.Name = "lblInfoEstado";
        lblInfoEstado.Size = new Size(197, 17);
        lblInfoEstado.Text = "Módulo de gestión de mascotas listo";
        // 
        // FormMascotas
        // 
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        BackColor = Color.FromArgb(250, 244, 244);
        ClientSize = new Size(1100, 700);
        Controls.Add(pnlContenido);
        Controls.Add(barraEstado);
        Controls.Add(pnlEncabezado);
        Font = new Font("Segoe UI", 9F);
        ForeColor = Color.FromArgb(58, 53, 59);
        MinimumSize = new Size(1100, 700);
        Name = "FormMascotas";
        StartPosition = FormStartPosition.CenterParent;
        Text = "CLÍNICA VETERINARIA — RECEPCIÓN — GESTIÓN DE MASCOTAS";
        Load += FormMascotas_Load;
        pnlEncabezado.ResumeLayout(false);
        pnlContenido.ResumeLayout(false);
        pnlBotones.ResumeLayout(false);
        grpDatos.ResumeLayout(false);
        grpDatos.PerformLayout();
        pnlSexo.ResumeLayout(false);
        pnlSexo.PerformLayout();
        pnlListado.ResumeLayout(false);
        pnlListado.PerformLayout();
        ((System.ComponentModel.ISupportInitialize)dgvMascotas).EndInit();
        barraEstado.ResumeLayout(false);
        barraEstado.PerformLayout();
        ResumeLayout(false);
        PerformLayout();
    }

    private Panel pnlEncabezado;
    private Label lblTitulo;
    private Label lblUsuarioSesion;
    private Panel pnlContenido;
    private GroupBox grpDatos;
    private Panel pnlListado;
    private Label lblBuscar;
    private TextBox txtBuscar;
    private Button btnBuscar;
    private Label lblFiltroEspecie;
    private ComboBox cboFiltroEspecie;
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
    private Label lblTotalRegistros;
    private StatusStrip barraEstado;
    private ToolStripStatusLabel lblInfoEstado;
    private Label lblNombre;
    private TextBox txtNombre;
    private Label lblPropietario;
    private ComboBox cboPropietario;
    private Button btnNuevoPropietario;
    private Label lblEspecie;
    private ComboBox cboEspecie;
    private Label lblRaza;
    private ComboBox cboRaza;
    private Button btnAltaEspecie;
    private Button btnModEspecie;
    private Button btnAltaRaza;
    private Button btnModRaza;
    private Label lblSexo;
    private Panel pnlSexo;
    private RadioButton rbMacho;
    private RadioButton rbHembra;
    private Label lblFechaNacimiento;
    private DateTimePicker dtpFechaNacimiento;
    private Label lblColor;
    private TextBox txtColor;
    private Panel pnlBotones;
    private Button btnGuardar;
    private Button btnModificar;
    private Button btnLimpiar;
    private Button btnVolver;
    private Button btnActivar;
    private Button btnDesactivar;
}
