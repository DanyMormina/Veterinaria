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
        DataGridViewCellStyle dataGridViewCellStyle5 = new DataGridViewCellStyle();
        DataGridViewCellStyle dataGridViewCellStyle6 = new DataGridViewCellStyle();
        pnlEncabezado = new Panel();
        lblTitulo = new Label();
        lblUsuarioSesion = new Label();
        pnlContenido = new Panel();
        grpDatos = new GroupBox();
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
        lblTotalRegistros = new Label();
        barraEstado = new StatusStrip();
        lblInfoEstado = new ToolStripStatusLabel();
        lblBuscarMascota = new Label();
        txtBuscarMascota = new TextBox();
        lblNombre = new Label();
        txtNombre = new TextBox();
        lblPropietario = new Label();
        txtPropietario = new TextBox();
        lblEspecie = new Label();
        txtEspecie = new TextBox();
        lblRaza = new Label();
        txtRaza = new TextBox();
        lblSexo = new Label();
        txtSexo = new TextBox();
        lblFechaNacimiento = new Label();
        dtpFechaNacimiento = new DateTimePicker();
        lblColor = new Label();
        txtColor = new TextBox();
        btnCancelar = new Button();
        btnModificar = new Button();
        btnGuardar = new Button();
        pnlBotones = new Panel();
        pnlEncabezado.SuspendLayout();
        pnlContenido.SuspendLayout();
        grpDatos.SuspendLayout();
        pnlListado.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)dgvMascotas).BeginInit();
        barraEstado.SuspendLayout();
        pnlBotones.SuspendLayout();
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
        // grpDatos
        // 
        grpDatos.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
        grpDatos.Controls.Add(lblBuscarMascota);
        grpDatos.Controls.Add(txtBuscarMascota);
        grpDatos.Controls.Add(lblNombre);
        grpDatos.Controls.Add(txtNombre);
        grpDatos.Controls.Add(lblPropietario);
        grpDatos.Controls.Add(txtPropietario);
        grpDatos.Controls.Add(lblEspecie);
        grpDatos.Controls.Add(txtEspecie);
        grpDatos.Controls.Add(lblRaza);
        grpDatos.Controls.Add(txtRaza);
        grpDatos.Controls.Add(lblSexo);
        grpDatos.Controls.Add(txtSexo);
        grpDatos.Controls.Add(lblFechaNacimiento);
        grpDatos.Controls.Add(dtpFechaNacimiento);
        grpDatos.Controls.Add(lblColor);
        grpDatos.Controls.Add(txtColor);
        grpDatos.Font = new Font("Segoe UI", 9F);
        grpDatos.ForeColor = Color.FromArgb(58, 53, 59);
        grpDatos.Location = new Point(16, 16);
        grpDatos.Name = "grpDatos";
        grpDatos.Size = new Size(1068, 189);
        grpDatos.TabIndex = 0;
        grpDatos.TabStop = false;
        grpDatos.Text = "Datos de la mascota";
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
        pnlListado.Controls.Add(btnActivas);
        pnlListado.Controls.Add(btnInactivas);
        pnlListado.Controls.Add(dgvMascotas);
        pnlListado.Controls.Add(lblTotalRegistros);
        pnlListado.Location = new Point(16, 257);
        pnlListado.Name = "pnlListado";
        pnlListado.Size = new Size(1072, 360);
        pnlListado.TabIndex = 2;
        // 
        // lblBuscar
        // 
        lblBuscar.Location = new Point(10, 12);
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
        txtBuscar.Location = new Point(64, 12);
        txtBuscar.Name = "txtBuscar";
        txtBuscar.Size = new Size(150, 23);
        txtBuscar.TabIndex = 1;
        // 
        // btnBuscar
        // 
        btnBuscar.BackColor = Color.FromArgb(226, 217, 220);
        btnBuscar.Cursor = Cursors.Hand;
        btnBuscar.FlatAppearance.BorderColor = Color.FromArgb(186, 162, 168);
        btnBuscar.FlatStyle = FlatStyle.Flat;
        btnBuscar.ForeColor = Color.FromArgb(58, 53, 59);
        btnBuscar.Location = new Point(220, 10);
        btnBuscar.Name = "btnBuscar";
        btnBuscar.Size = new Size(78, 27);
        btnBuscar.TabIndex = 2;
        btnBuscar.Text = "Buscar";
        btnBuscar.UseVisualStyleBackColor = false;
        // 
        // lblFiltroEspecie
        // 
        lblFiltroEspecie.Location = new Point(10, 46);
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
        cboFiltroEspecie.Location = new Point(64, 46);
        cboFiltroEspecie.Name = "cboFiltroEspecie";
        cboFiltroEspecie.Size = new Size(150, 23);
        cboFiltroEspecie.TabIndex = 4;
        // 
        // btnActivas
        // 
        btnActivas.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        btnActivas.BackColor = Color.FromArgb(226, 217, 220);
        btnActivas.Cursor = Cursors.Hand;
        btnActivas.FlatAppearance.BorderColor = Color.FromArgb(186, 162, 168);
        btnActivas.FlatStyle = FlatStyle.Flat;
        btnActivas.ForeColor = Color.FromArgb(58, 53, 59);
        btnActivas.Location = new Point(840, 10);
        btnActivas.Name = "btnActivas";
        btnActivas.Size = new Size(104, 27);
        btnActivas.TabIndex = 5;
        btnActivas.Text = "Activas";
        btnActivas.UseVisualStyleBackColor = false;
        // 
        // btnInactivas
        // 
        btnInactivas.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        btnInactivas.BackColor = Color.FromArgb(226, 217, 220);
        btnInactivas.Cursor = Cursors.Hand;
        btnInactivas.FlatAppearance.BorderColor = Color.FromArgb(186, 162, 168);
        btnInactivas.FlatStyle = FlatStyle.Flat;
        btnInactivas.ForeColor = Color.FromArgb(58, 53, 59);
        btnInactivas.Location = new Point(950, 10);
        btnInactivas.Name = "btnInactivas";
        btnInactivas.Size = new Size(110, 27);
        btnInactivas.TabIndex = 6;
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
        dataGridViewCellStyle5.Alignment = DataGridViewContentAlignment.MiddleLeft;
        dataGridViewCellStyle5.BackColor = SystemColors.Control;
        dataGridViewCellStyle5.Font = new Font("Segoe UI", 9F);
        dataGridViewCellStyle5.ForeColor = SystemColors.WindowText;
        dataGridViewCellStyle5.SelectionBackColor = SystemColors.Highlight;
        dataGridViewCellStyle5.SelectionForeColor = SystemColors.HighlightText;
        dataGridViewCellStyle5.WrapMode = DataGridViewTriState.True;
        dgvMascotas.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle5;
        dgvMascotas.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
        dgvMascotas.Columns.AddRange(new DataGridViewColumn[] { colId, colNombre, colPropietario, colEspecie, colRaza, colSexo, colFechaNacimiento, colColor, colEstado });
        dataGridViewCellStyle6.Alignment = DataGridViewContentAlignment.MiddleLeft;
        dataGridViewCellStyle6.BackColor = SystemColors.Window;
        dataGridViewCellStyle6.Font = new Font("Segoe UI", 9F);
        dataGridViewCellStyle6.ForeColor = Color.FromArgb(58, 53, 59);
        dataGridViewCellStyle6.SelectionBackColor = SystemColors.Highlight;
        dataGridViewCellStyle6.SelectionForeColor = SystemColors.HighlightText;
        dataGridViewCellStyle6.WrapMode = DataGridViewTriState.False;
        dgvMascotas.DefaultCellStyle = dataGridViewCellStyle6;
        dgvMascotas.Location = new Point(10, 80);
        dgvMascotas.MultiSelect = false;
        dgvMascotas.Name = "dgvMascotas";
        dgvMascotas.ReadOnly = true;
        dgvMascotas.RowHeadersVisible = false;
        dgvMascotas.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        dgvMascotas.Size = new Size(1050, 240);
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
        lblTotalRegistros.Location = new Point(10, 328);
        lblTotalRegistros.Name = "lblTotalRegistros";
        lblTotalRegistros.Size = new Size(300, 23);
        lblTotalRegistros.TabIndex = 8;
        lblTotalRegistros.Text = "Total de registros: 0";
        lblTotalRegistros.TextAlign = ContentAlignment.MiddleLeft;
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
        // lblBuscarMascota
        // 
        lblBuscarMascota.Location = new Point(12, 34);
        lblBuscarMascota.Name = "lblBuscarMascota";
        lblBuscarMascota.Size = new Size(118, 23);
        lblBuscarMascota.TabIndex = 16;
        lblBuscarMascota.Text = "Buscar mascota";
        lblBuscarMascota.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // txtBuscarMascota
        // 
        txtBuscarMascota.BackColor = Color.White;
        txtBuscarMascota.BorderStyle = BorderStyle.FixedSingle;
        txtBuscarMascota.Location = new Point(136, 34);
        txtBuscarMascota.Name = "txtBuscarMascota";
        txtBuscarMascota.Size = new Size(904, 23);
        txtBuscarMascota.TabIndex = 17;
        // 
        // lblNombre
        // 
        lblNombre.Location = new Point(12, 74);
        lblNombre.Name = "lblNombre";
        lblNombre.Size = new Size(118, 23);
        lblNombre.TabIndex = 18;
        lblNombre.Text = "Nombre";
        lblNombre.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // txtNombre
        // 
        txtNombre.BackColor = Color.White;
        txtNombre.BorderStyle = BorderStyle.FixedSingle;
        txtNombre.Location = new Point(136, 74);
        txtNombre.Name = "txtNombre";
        txtNombre.Size = new Size(360, 23);
        txtNombre.TabIndex = 19;
        // 
        // lblPropietario
        // 
        lblPropietario.Location = new Point(524, 74);
        lblPropietario.Name = "lblPropietario";
        lblPropietario.Size = new Size(110, 23);
        lblPropietario.TabIndex = 20;
        lblPropietario.Text = "Propietario";
        lblPropietario.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // txtPropietario
        // 
        txtPropietario.BackColor = Color.White;
        txtPropietario.BorderStyle = BorderStyle.FixedSingle;
        txtPropietario.Location = new Point(640, 74);
        txtPropietario.Name = "txtPropietario";
        txtPropietario.Size = new Size(400, 23);
        txtPropietario.TabIndex = 21;
        // 
        // lblEspecie
        // 
        lblEspecie.Location = new Point(12, 110);
        lblEspecie.Name = "lblEspecie";
        lblEspecie.Size = new Size(118, 23);
        lblEspecie.TabIndex = 22;
        lblEspecie.Text = "Especie";
        lblEspecie.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // txtEspecie
        // 
        txtEspecie.BackColor = Color.White;
        txtEspecie.BorderStyle = BorderStyle.FixedSingle;
        txtEspecie.Location = new Point(136, 110);
        txtEspecie.Name = "txtEspecie";
        txtEspecie.Size = new Size(160, 23);
        txtEspecie.TabIndex = 23;
        // 
        // lblRaza
        // 
        lblRaza.Location = new Point(316, 110);
        lblRaza.Name = "lblRaza";
        lblRaza.Size = new Size(50, 23);
        lblRaza.TabIndex = 24;
        lblRaza.Text = "Raza";
        lblRaza.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // txtRaza
        // 
        txtRaza.BackColor = Color.White;
        txtRaza.BorderStyle = BorderStyle.FixedSingle;
        txtRaza.Location = new Point(372, 110);
        txtRaza.Name = "txtRaza";
        txtRaza.Size = new Size(124, 23);
        txtRaza.TabIndex = 25;
        // 
        // lblSexo
        // 
        lblSexo.Location = new Point(524, 110);
        lblSexo.Name = "lblSexo";
        lblSexo.Size = new Size(110, 23);
        lblSexo.TabIndex = 26;
        lblSexo.Text = "Sexo";
        lblSexo.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // txtSexo
        // 
        txtSexo.BackColor = Color.White;
        txtSexo.BorderStyle = BorderStyle.FixedSingle;
        txtSexo.Location = new Point(640, 110);
        txtSexo.Name = "txtSexo";
        txtSexo.Size = new Size(160, 23);
        txtSexo.TabIndex = 27;
        // 
        // lblFechaNacimiento
        // 
        lblFechaNacimiento.Location = new Point(12, 146);
        lblFechaNacimiento.Name = "lblFechaNacimiento";
        lblFechaNacimiento.Size = new Size(118, 23);
        lblFechaNacimiento.TabIndex = 28;
        lblFechaNacimiento.Text = "Fecha de nacimiento";
        lblFechaNacimiento.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // dtpFechaNacimiento
        // 
        dtpFechaNacimiento.Format = DateTimePickerFormat.Short;
        dtpFechaNacimiento.Location = new Point(136, 146);
        dtpFechaNacimiento.Name = "dtpFechaNacimiento";
        dtpFechaNacimiento.Size = new Size(160, 23);
        dtpFechaNacimiento.TabIndex = 29;
        // 
        // lblColor
        // 
        lblColor.Location = new Point(524, 146);
        lblColor.Name = "lblColor";
        lblColor.Size = new Size(110, 23);
        lblColor.TabIndex = 30;
        lblColor.Text = "Color";
        lblColor.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // txtColor
        // 
        txtColor.BackColor = Color.White;
        txtColor.BorderStyle = BorderStyle.FixedSingle;
        txtColor.Location = new Point(640, 146);
        txtColor.Name = "txtColor";
        txtColor.Size = new Size(160, 23);
        txtColor.TabIndex = 31;
        // 
        // btnCancelar
        // 
        btnCancelar.BackColor = Color.FromArgb(220, 150, 154);
        btnCancelar.FlatAppearance.BorderColor = Color.FromArgb(186, 118, 122);
        btnCancelar.FlatStyle = FlatStyle.Flat;
        btnCancelar.ForeColor = Color.FromArgb(58, 53, 59);
        btnCancelar.Location = new Point(223, 5);
        btnCancelar.Name = "btnCancelar";
        btnCancelar.Size = new Size(100, 32);
        btnCancelar.TabIndex = 3;
        btnCancelar.Text = "Cancelar";
        btnCancelar.UseVisualStyleBackColor = false;
        // 
        // btnModificar
        // 
        btnModificar.BackColor = Color.FromArgb(148, 176, 214);
        btnModificar.FlatAppearance.BorderColor = Color.FromArgb(112, 142, 182);
        btnModificar.FlatStyle = FlatStyle.Flat;
        btnModificar.ForeColor = Color.FromArgb(58, 53, 59);
        btnModificar.Location = new Point(115, 5);
        btnModificar.Name = "btnModificar";
        btnModificar.Size = new Size(100, 32);
        btnModificar.TabIndex = 2;
        btnModificar.Text = "Modificar";
        btnModificar.UseVisualStyleBackColor = false;
        // 
        // btnGuardar
        // 
        btnGuardar.BackColor = Color.FromArgb(152, 196, 164);
        btnGuardar.FlatAppearance.BorderColor = Color.FromArgb(112, 158, 124);
        btnGuardar.FlatStyle = FlatStyle.Flat;
        btnGuardar.ForeColor = Color.FromArgb(58, 53, 59);
        btnGuardar.Location = new Point(7, 5);
        btnGuardar.Name = "btnGuardar";
        btnGuardar.Size = new Size(100, 32);
        btnGuardar.TabIndex = 1;
        btnGuardar.Text = "Guardar";
        btnGuardar.UseVisualStyleBackColor = false;
        // 
        // pnlBotones
        // 
        pnlBotones.Controls.Add(btnGuardar);
        pnlBotones.Controls.Add(btnModificar);
        pnlBotones.Controls.Add(btnCancelar);
        pnlBotones.Location = new Point(16, 211);
        pnlBotones.Name = "pnlBotones";
        pnlBotones.Size = new Size(1068, 40);
        pnlBotones.TabIndex = 3;
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
        grpDatos.ResumeLayout(false);
        grpDatos.PerformLayout();
        pnlListado.ResumeLayout(false);
        pnlListado.PerformLayout();
        ((System.ComponentModel.ISupportInitialize)dgvMascotas).EndInit();
        barraEstado.ResumeLayout(false);
        barraEstado.PerformLayout();
        pnlBotones.ResumeLayout(false);
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
    private Label lblTotalRegistros;
    private StatusStrip barraEstado;
    private ToolStripStatusLabel lblInfoEstado;
    private Label lblBuscarMascota;
    private TextBox txtBuscarMascota;
    private Label lblNombre;
    private TextBox txtNombre;
    private Label lblPropietario;
    private TextBox txtPropietario;
    private Label lblEspecie;
    private TextBox txtEspecie;
    private Label lblRaza;
    private TextBox txtRaza;
    private Label lblSexo;
    private TextBox txtSexo;
    private Label lblFechaNacimiento;
    private DateTimePicker dtpFechaNacimiento;
    private Label lblColor;
    private TextBox txtColor;
    private Panel pnlBotones;
    private Button btnGuardar;
    private Button btnModificar;
    private Button btnCancelar;
}
