namespace Veterinaria.WinForms.Vistas.Administrador;

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
        DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
        DataGridViewCellStyle dataGridViewCellStyle2 = new DataGridViewCellStyle();
        pnlEncabezado = new Panel();
        lblTitulo = new Label();
        lblUsuarioSesion = new Label();
        pnlContenido = new Panel();
        pnlBotones = new Panel();
        btnLimpiar = new Button();
        btnBuscar = new Button();
        grpDatos = new GroupBox();
        lbEstado = new Label();
        lblNombre = new Label();
        txtNombre = new TextBox();
        lblPropietario = new Label();
        txtPropietario = new TextBox();
        btnActivar = new Button();
        btnDesactivar = new Button();
        lblEspecie = new Label();
        cboEspecie = new ComboBox();
        lblRaza = new Label();
        cboRaza = new ComboBox();
        lblSexo = new Label();
        cboSexo = new ComboBox();
        lblColor = new Label();
        txtColor = new TextBox();
        btnVolver = new Button();
        pnlListado = new Panel();
        lblBuscar = new Label();
        txtBuscar = new TextBox();
        lblFiltroEspecie = new Label();
        cboFiltroEspecie = new ComboBox();
        lblFiltroEstado = new Label();
        cboFiltroEstado = new ComboBox();
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
        pnlEncabezado.SuspendLayout();
        pnlContenido.SuspendLayout();
        pnlBotones.SuspendLayout();
        grpDatos.SuspendLayout();
        pnlListado.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)dgvMascotas).BeginInit();
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
        lblTitulo.Size = new Size(760, 48);
        lblTitulo.TabIndex = 0;
        lblTitulo.Text = "CLÍNICA VETERINARIA — ADMINISTRADOR — GESTIÓN DE MASCOTAS";
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
        pnlContenido.Controls.Add(pnlBotones);
        pnlContenido.Controls.Add(grpDatos);
        pnlContenido.Controls.Add(btnVolver);
        pnlContenido.Controls.Add(pnlListado);
        pnlContenido.Dock = DockStyle.Fill;
        pnlContenido.Location = new Point(0, 48);
        pnlContenido.Name = "pnlContenido";
        pnlContenido.Size = new Size(1220, 672);
        pnlContenido.TabIndex = 1;
        // 
        // pnlBotones
        // 
        pnlBotones.Controls.Add(btnLimpiar);
        pnlBotones.Controls.Add(btnBuscar);
        pnlBotones.Location = new Point(12, 346);
        pnlBotones.Name = "pnlBotones";
        pnlBotones.Size = new Size(434, 61);
        pnlBotones.TabIndex = 10;
        // 
        // btnLimpiar
        // 
        btnLimpiar.BackColor = Color.FromArgb(226, 217, 220);
        btnLimpiar.Cursor = Cursors.Hand;
        btnLimpiar.FlatAppearance.BorderSize = 0;
        btnLimpiar.FlatStyle = FlatStyle.Flat;
        btnLimpiar.Font = new Font("Segoe UI", 9F);
        btnLimpiar.ForeColor = Color.FromArgb(58, 53, 59);
        btnLimpiar.Location = new Point(25, 15);
        btnLimpiar.Name = "btnLimpiar";
        btnLimpiar.Size = new Size(153, 32);
        btnLimpiar.TabIndex = 9;
        btnLimpiar.Text = "Limpiar";
        btnLimpiar.UseVisualStyleBackColor = false;
        // 
        // btnBuscar
        // 
        btnBuscar.BackColor = Color.FromArgb(152, 196, 164);
        btnBuscar.Cursor = Cursors.Hand;
        btnBuscar.FlatAppearance.BorderSize = 0;
        btnBuscar.FlatStyle = FlatStyle.Flat;
        btnBuscar.Font = new Font("Segoe UI", 9F);
        btnBuscar.ForeColor = Color.Black;
        btnBuscar.Location = new Point(197, 15);
        btnBuscar.Name = "btnBuscar";
        btnBuscar.Size = new Size(153, 32);
        btnBuscar.TabIndex = 8;
        btnBuscar.Text = "Buscar";
        btnBuscar.UseVisualStyleBackColor = false;
        btnBuscar.Click += btnBuscar_Click;
        // 
        // grpDatos
        // 
        grpDatos.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
        grpDatos.Controls.Add(lbEstado);
        grpDatos.Controls.Add(lblNombre);
        grpDatos.Controls.Add(txtNombre);
        grpDatos.Controls.Add(lblPropietario);
        grpDatos.Controls.Add(txtPropietario);
        grpDatos.Controls.Add(btnActivar);
        grpDatos.Controls.Add(btnDesactivar);
        grpDatos.Controls.Add(lblEspecie);
        grpDatos.Controls.Add(cboEspecie);
        grpDatos.Controls.Add(lblRaza);
        grpDatos.Controls.Add(cboRaza);
        grpDatos.Controls.Add(lblSexo);
        grpDatos.Controls.Add(cboSexo);
        grpDatos.Controls.Add(lblColor);
        grpDatos.Controls.Add(txtColor);
        grpDatos.Font = new Font("Segoe UI", 9F);
        grpDatos.ForeColor = Color.FromArgb(58, 53, 59);
        grpDatos.Location = new Point(12, 16);
        grpDatos.Name = "grpDatos";
        grpDatos.Size = new Size(434, 324);
        grpDatos.TabIndex = 0;
        grpDatos.TabStop = false;
        grpDatos.Text = "Datos de la mascota";
        // 
        // lbEstado
        // 
        lbEstado.AutoSize = true;
        lbEstado.Location = new Point(25, 270);
        lbEstado.Name = "lbEstado";
        lbEstado.Size = new Size(42, 15);
        lbEstado.TabIndex = 13;
        lbEstado.Text = "Estado";
        // 
        // lblNombre
        // 
        lblNombre.Location = new Point(25, 32);
        lblNombre.Name = "lblNombre";
        lblNombre.Size = new Size(84, 23);
        lblNombre.TabIndex = 0;
        lblNombre.Text = "Nombre";
        lblNombre.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // txtNombre
        // 
        txtNombre.BackColor = Color.White;
        txtNombre.BorderStyle = BorderStyle.FixedSingle;
        txtNombre.Location = new Point(124, 32);
        txtNombre.Name = "txtNombre";
        txtNombre.Size = new Size(282, 23);
        txtNombre.TabIndex = 1;
        // 
        // lblPropietario
        // 
        lblPropietario.Location = new Point(25, 70);
        lblPropietario.Name = "lblPropietario";
        lblPropietario.Size = new Size(84, 23);
        lblPropietario.TabIndex = 2;
        lblPropietario.Text = "Propietario";
        lblPropietario.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // txtPropietario
        // 
        txtPropietario.BackColor = Color.White;
        txtPropietario.BorderStyle = BorderStyle.FixedSingle;
        txtPropietario.Location = new Point(124, 68);
        txtPropietario.Name = "txtPropietario";
        txtPropietario.Size = new Size(282, 23);
        txtPropietario.TabIndex = 3;
        // 
        // btnActivar
        // 
        btnActivar.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
        btnActivar.BackColor = Color.Thistle;
        btnActivar.Cursor = Cursors.Hand;
        btnActivar.Enabled = false;
        btnActivar.FlatAppearance.BorderSize = 0;
        btnActivar.FlatStyle = FlatStyle.Flat;
        btnActivar.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
        btnActivar.ForeColor = Color.FromArgb(136, 136, 136);
        btnActivar.Location = new Point(124, 261);
        btnActivar.Name = "btnActivar";
        btnActivar.Size = new Size(127, 32);
        btnActivar.TabIndex = 8;
        btnActivar.Text = "Activar";
        btnActivar.UseVisualStyleBackColor = false;
        btnActivar.Click += btnActivar_Click;
        // 
        // btnDesactivar
        // 
        btnDesactivar.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
        btnDesactivar.BackColor = Color.MistyRose;
        btnDesactivar.Cursor = Cursors.Hand;
        btnDesactivar.Enabled = false;
        btnDesactivar.FlatAppearance.BorderSize = 0;
        btnDesactivar.FlatStyle = FlatStyle.Flat;
        btnDesactivar.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
        btnDesactivar.ForeColor = Color.FromArgb(136, 136, 136);
        btnDesactivar.Location = new Point(279, 261);
        btnDesactivar.Name = "btnDesactivar";
        btnDesactivar.Size = new Size(127, 32);
        btnDesactivar.TabIndex = 9;
        btnDesactivar.Text = "Desactivar";
        btnDesactivar.UseVisualStyleBackColor = false;
        btnDesactivar.Click += btnDesactivar_Click;
        // 
        // lblEspecie
        // 
        lblEspecie.Location = new Point(25, 108);
        lblEspecie.Name = "lblEspecie";
        lblEspecie.Size = new Size(84, 23);
        lblEspecie.TabIndex = 4;
        lblEspecie.Text = "Especie";
        lblEspecie.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // cboEspecie
        // 
        cboEspecie.BackColor = Color.White;
        cboEspecie.DropDownStyle = ComboBoxStyle.DropDownList;
        cboEspecie.FormattingEnabled = true;
        cboEspecie.Location = new Point(124, 106);
        cboEspecie.Name = "cboEspecie";
        cboEspecie.Size = new Size(282, 23);
        cboEspecie.TabIndex = 5;
        // 
        // lblRaza
        // 
        lblRaza.Location = new Point(25, 146);
        lblRaza.Name = "lblRaza";
        lblRaza.Size = new Size(84, 23);
        lblRaza.TabIndex = 6;
        lblRaza.Text = "Raza";
        lblRaza.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // cboRaza
        // 
        cboRaza.BackColor = Color.White;
        cboRaza.DropDownStyle = ComboBoxStyle.DropDownList;
        cboRaza.FormattingEnabled = true;
        cboRaza.Location = new Point(124, 144);
        cboRaza.Name = "cboRaza";
        cboRaza.Size = new Size(282, 23);
        cboRaza.TabIndex = 7;
        // 
        // lblSexo
        // 
        lblSexo.Location = new Point(25, 184);
        lblSexo.Name = "lblSexo";
        lblSexo.Size = new Size(84, 23);
        lblSexo.TabIndex = 8;
        lblSexo.Text = "Sexo";
        lblSexo.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // cboSexo
        // 
        cboSexo.BackColor = Color.White;
        cboSexo.DropDownStyle = ComboBoxStyle.DropDownList;
        cboSexo.FormattingEnabled = true;
        cboSexo.Items.AddRange(new object[] { "Macho", "Hembra" });
        cboSexo.Location = new Point(124, 182);
        cboSexo.Name = "cboSexo";
        cboSexo.Size = new Size(282, 23);
        cboSexo.TabIndex = 9;
        // 
        // lblColor
        // 
        lblColor.Location = new Point(25, 222);
        lblColor.Name = "lblColor";
        lblColor.Size = new Size(84, 23);
        lblColor.TabIndex = 10;
        lblColor.Text = "Color";
        lblColor.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // txtColor
        // 
        txtColor.BackColor = Color.White;
        txtColor.BorderStyle = BorderStyle.FixedSingle;
        txtColor.Location = new Point(124, 220);
        txtColor.Name = "txtColor";
        txtColor.Size = new Size(282, 23);
        txtColor.TabIndex = 11;
        // 
        // btnVolver
        // 
        btnVolver.BackColor = Color.FromArgb(220, 200, 204);
        btnVolver.FlatAppearance.BorderColor = Color.FromArgb(186, 162, 168);
        btnVolver.FlatStyle = FlatStyle.Flat;
        btnVolver.Font = new Font("Segoe UI", 9F);
        btnVolver.ForeColor = Color.Black;
        btnVolver.Location = new Point(41, 601);
        btnVolver.Name = "btnVolver";
        btnVolver.Size = new Size(208, 32);
        btnVolver.TabIndex = 5;
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
        pnlListado.Controls.Add(lblFiltroEspecie);
        pnlListado.Controls.Add(cboFiltroEspecie);
        pnlListado.Controls.Add(lblFiltroEstado);
        pnlListado.Controls.Add(cboFiltroEstado);
        pnlListado.Controls.Add(dgvMascotas);
        pnlListado.Location = new Point(456, 22);
        pnlListado.Name = "pnlListado";
        pnlListado.Size = new Size(748, 638);
        pnlListado.TabIndex = 2;
        // 
        // lblBuscar
        // 
        lblBuscar.Location = new Point(10, 14);
        lblBuscar.Name = "lblBuscar";
        lblBuscar.Size = new Size(46, 23);
        lblBuscar.TabIndex = 0;
        lblBuscar.Text = "Buscar";
        lblBuscar.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // txtBuscar
        // 
        txtBuscar.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
        txtBuscar.BackColor = Color.White;
        txtBuscar.BorderStyle = BorderStyle.FixedSingle;
        txtBuscar.Location = new Point(58, 14);
        txtBuscar.Name = "txtBuscar";
        txtBuscar.Size = new Size(260, 23);
        txtBuscar.TabIndex = 1;
        // 
        // lblFiltroEspecie
        // 
        lblFiltroEspecie.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        lblFiltroEspecie.Location = new Point(326, 14);
        lblFiltroEspecie.Name = "lblFiltroEspecie";
        lblFiltroEspecie.Size = new Size(50, 23);
        lblFiltroEspecie.TabIndex = 2;
        lblFiltroEspecie.Text = "Especie";
        lblFiltroEspecie.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // cboFiltroEspecie
        // 
        cboFiltroEspecie.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        cboFiltroEspecie.BackColor = Color.White;
        cboFiltroEspecie.DropDownStyle = ComboBoxStyle.DropDownList;
        cboFiltroEspecie.FormattingEnabled = true;
        cboFiltroEspecie.Location = new Point(380, 14);
        cboFiltroEspecie.Name = "cboFiltroEspecie";
        cboFiltroEspecie.Size = new Size(150, 23);
        cboFiltroEspecie.TabIndex = 3;
        // 
        // lblFiltroEstado
        // 
        lblFiltroEstado.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        lblFiltroEstado.Location = new Point(540, 14);
        lblFiltroEstado.Name = "lblFiltroEstado";
        lblFiltroEstado.Size = new Size(46, 23);
        lblFiltroEstado.TabIndex = 4;
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
        cboFiltroEstado.Location = new Point(590, 14);
        cboFiltroEstado.Name = "cboFiltroEstado";
        cboFiltroEstado.Size = new Size(142, 23);
        cboFiltroEstado.TabIndex = 5;
        // 
        // dgvMascotas
        // 
        dgvMascotas.AllowUserToAddRows = false;
        dgvMascotas.AllowUserToDeleteRows = false;
        dgvMascotas.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        dgvMascotas.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        dgvMascotas.BackgroundColor = Color.White;
        dataGridViewCellStyle1.Alignment = DataGridViewContentAlignment.MiddleLeft;
        dataGridViewCellStyle1.BackColor = SystemColors.Control;
        dataGridViewCellStyle1.Font = new Font("Segoe UI", 9F);
        dataGridViewCellStyle1.ForeColor = SystemColors.WindowText;
        dataGridViewCellStyle1.SelectionBackColor = SystemColors.Highlight;
        dataGridViewCellStyle1.SelectionForeColor = SystemColors.HighlightText;
        dataGridViewCellStyle1.WrapMode = DataGridViewTriState.True;
        dgvMascotas.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
        dgvMascotas.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
        dgvMascotas.Columns.AddRange(new DataGridViewColumn[] { colId, colNombre, colPropietario, colEspecie, colRaza, colSexo, colFechaNacimiento, colColor, colEstado });
        dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleLeft;
        dataGridViewCellStyle2.BackColor = SystemColors.Window;
        dataGridViewCellStyle2.Font = new Font("Segoe UI", 9F);
        dataGridViewCellStyle2.ForeColor = Color.FromArgb(58, 53, 59);
        dataGridViewCellStyle2.SelectionBackColor = SystemColors.Highlight;
        dataGridViewCellStyle2.SelectionForeColor = SystemColors.HighlightText;
        dataGridViewCellStyle2.WrapMode = DataGridViewTriState.False;
        dgvMascotas.DefaultCellStyle = dataGridViewCellStyle2;
        dgvMascotas.Location = new Point(10, 62);
        dgvMascotas.MultiSelect = false;
        dgvMascotas.Name = "dgvMascotas";
        dgvMascotas.ReadOnly = true;
        dgvMascotas.RowHeadersVisible = false;
        dgvMascotas.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        dgvMascotas.Size = new Size(726, 560);
        dgvMascotas.TabIndex = 7;
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
        // FormMascotas
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
        Name = "FormMascotas";
        StartPosition = FormStartPosition.CenterParent;
        Text = "CLÍNICA VETERINARIA — ADMINISTRADOR — GESTIÓN DE MASCOTAS";
        Load += FormMascotas_Load;
        pnlEncabezado.ResumeLayout(false);
        pnlContenido.ResumeLayout(false);
        pnlBotones.ResumeLayout(false);
        grpDatos.ResumeLayout(false);
        grpDatos.PerformLayout();
        pnlListado.ResumeLayout(false);
        pnlListado.PerformLayout();
        ((System.ComponentModel.ISupportInitialize)dgvMascotas).EndInit();
        ResumeLayout(false);
    }

    private Panel pnlEncabezado;
    private Label lblTitulo;
    private Label lblUsuarioSesion;
    private Panel pnlContenido;
    private GroupBox grpDatos;
    private Label lblNombre;
    private TextBox txtNombre;
    private Label lblPropietario;
    private TextBox txtPropietario;
    private Label lblEspecie;
    private ComboBox cboEspecie;
    private Label lblRaza;
    private ComboBox cboRaza;
    private Label lblSexo;
    private ComboBox cboSexo;
    private Label lblColor;
    private TextBox txtColor;
    private Button btnActivar;
    private Button btnDesactivar;
    private Button btnVolver;
    private Panel pnlListado;
    private Label lblBuscar;
    private TextBox txtBuscar;
    private Label lblFiltroEspecie;
    private ComboBox cboFiltroEspecie;
    private Label lblFiltroEstado;
    private ComboBox cboFiltroEstado;
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
    private Button btnBuscar;
    private Button btnLimpiar;
    private Label lbEstado;
    private Panel pnlBotones;
}
