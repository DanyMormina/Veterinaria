namespace Veterinaria.WinForms.Vistas.Veterinario;

partial class FormFichaMedica
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
        grpHistorial = new GroupBox();
        dgvHistorialClinico = new DataGridView();
        colFecha = new DataGridViewTextBoxColumn();
        colMotivo = new DataGridViewTextBoxColumn();
        colDiagnostico = new DataGridViewTextBoxColumn();
        colPeso = new DataGridViewTextBoxColumn();
        colTemperatura = new DataGridViewTextBoxColumn();
        colTratamiento = new DataGridViewTextBoxColumn();
        colProximoControl = new DataGridViewTextBoxColumn();
        pnlBotones = new Panel();
        btnVerHistorial = new Button();
        btnImprimirFicha = new Button();
        btnLimpiar = new Button();
        btnVolver = new Button();
        grpDatos = new GroupBox();
        lblBuscarMascota = new Label();
        pnlContenedorBusqueda = new Panel();
        cboBuscarMascota = new ComboBox();
        lblNombre = new Label();
        txtNombre = new TextBox();
        lblPropietario = new Label();
        txtPropietario = new TextBox();
        lblEspecie = new Label();
        txtEspecie = new TextBox();
        lblRaza = new Label();
        txtRaza = new TextBox();
        lblSexo = new Label();
        rbMacho = new RadioButton();
        rbHembra = new RadioButton();
        lblFechaNacimiento = new Label();
        dtpFechaNacimiento = new DateTimePicker();
        lblColor = new Label();
        txtColor = new TextBox();
        barraEstado = new StatusStrip();
        lblInfoEstado = new ToolStripStatusLabel();
        pnlEncabezado.SuspendLayout();
        pnlContenido.SuspendLayout();
        grpHistorial.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)dgvHistorialClinico).BeginInit();
        pnlBotones.SuspendLayout();
        grpDatos.SuspendLayout();
        pnlContenedorBusqueda.SuspendLayout();
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
        lblTitulo.Text = "CLÍNICA VETERINARIA — ATENCIÓN CLÍNICA — HISTORIAL CLÍNICO";
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
        lblUsuarioSesion.Text = "Dr./Dra. Lucía Pérez | Veterinario";
        lblUsuarioSesion.TextAlign = ContentAlignment.MiddleRight;
        // 
        // pnlContenido
        // 
        pnlContenido.BackColor = Color.FromArgb(250, 244, 244);
        pnlContenido.Controls.Add(grpHistorial);
        pnlContenido.Controls.Add(pnlBotones);
        pnlContenido.Controls.Add(grpDatos);
        pnlContenido.Dock = DockStyle.Fill;
        pnlContenido.Location = new Point(0, 50);
        pnlContenido.Name = "pnlContenido";
        pnlContenido.Size = new Size(1100, 628);
        pnlContenido.TabIndex = 1;
        // 
        // grpHistorial
        // 
        grpHistorial.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        grpHistorial.Controls.Add(dgvHistorialClinico);
        grpHistorial.Font = new Font("Segoe UI", 9F);
        grpHistorial.ForeColor = Color.FromArgb(58, 53, 59);
        grpHistorial.Location = new Point(16, 264);
        grpHistorial.Name = "grpHistorial";
        grpHistorial.Size = new Size(1068, 348);
        grpHistorial.TabIndex = 2;
        grpHistorial.TabStop = false;
        grpHistorial.Text = "Historial clínico";
        // 
        // dgvHistorialClinico
        // 
        dgvHistorialClinico.AllowUserToAddRows = false;
        dgvHistorialClinico.AllowUserToDeleteRows = false;
        dgvHistorialClinico.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        dgvHistorialClinico.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        dgvHistorialClinico.BackgroundColor = Color.White;
        dgvHistorialClinico.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
        dgvHistorialClinico.Columns.AddRange(new DataGridViewColumn[] { colFecha, colMotivo, colDiagnostico, colPeso, colTemperatura, colTratamiento, colProximoControl });
        dgvHistorialClinico.Location = new Point(16, 28);
        dgvHistorialClinico.MultiSelect = false;
        dgvHistorialClinico.Name = "dgvHistorialClinico";
        dgvHistorialClinico.ReadOnly = true;
        dgvHistorialClinico.RowHeadersVisible = false;
        dgvHistorialClinico.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        dgvHistorialClinico.Size = new Size(1036, 304);
        dgvHistorialClinico.TabIndex = 0;
        // 
        // colFecha
        // 
        colFecha.HeaderText = "Fecha";
        colFecha.Name = "colFecha";
        colFecha.ReadOnly = true;
        // 
        // colMotivo
        // 
        colMotivo.HeaderText = "Motivo";
        colMotivo.Name = "colMotivo";
        colMotivo.ReadOnly = true;
        // 
        // colDiagnostico
        // 
        colDiagnostico.HeaderText = "Diagnóstico";
        colDiagnostico.Name = "colDiagnostico";
        colDiagnostico.ReadOnly = true;
        // 
        // colPeso
        // 
        colPeso.HeaderText = "Peso";
        colPeso.Name = "colPeso";
        colPeso.ReadOnly = true;
        // 
        // colTemperatura
        // 
        colTemperatura.HeaderText = "Temperatura";
        colTemperatura.Name = "colTemperatura";
        colTemperatura.ReadOnly = true;
        // 
        // colTratamiento
        // 
        colTratamiento.HeaderText = "Tratamiento";
        colTratamiento.Name = "colTratamiento";
        colTratamiento.ReadOnly = true;
        // 
        // colProximoControl
        // 
        colProximoControl.HeaderText = "Próximo control";
        colProximoControl.Name = "colProximoControl";
        colProximoControl.ReadOnly = true;
        // 
        // pnlBotones
        // 
        pnlBotones.Controls.Add(btnVerHistorial);
        pnlBotones.Controls.Add(btnImprimirFicha);
        pnlBotones.Controls.Add(btnLimpiar);
        pnlBotones.Controls.Add(btnVolver);
        pnlBotones.Location = new Point(16, 216);
        pnlBotones.Name = "pnlBotones";
        pnlBotones.Size = new Size(1068, 40);
        pnlBotones.TabIndex = 1;
        // 
        // btnVerHistorial
        // 
        btnVerHistorial.BackColor = Color.FromArgb(230, 196, 202);
        btnVerHistorial.Cursor = Cursors.Hand;
        btnVerHistorial.FlatAppearance.BorderColor = Color.FromArgb(186, 162, 168);
        btnVerHistorial.FlatStyle = FlatStyle.Flat;
        btnVerHistorial.Font = new Font("Segoe UI", 9F);
        btnVerHistorial.ForeColor = Color.Black;
        btnVerHistorial.Location = new Point(16, 3);
        btnVerHistorial.Name = "btnVerHistorial";
        btnVerHistorial.Size = new Size(140, 32);
        btnVerHistorial.TabIndex = 0;
        btnVerHistorial.Text = "Ver historial";
        btnVerHistorial.UseVisualStyleBackColor = false;
        btnVerHistorial.Click += btnVerHistorial_Click;
        // 
        // btnImprimirFicha
        // 
        btnImprimirFicha.BackColor = Color.Thistle;
        btnImprimirFicha.Cursor = Cursors.Hand;
        btnImprimirFicha.FlatAppearance.BorderColor = Color.FromArgb(186, 162, 168);
        btnImprimirFicha.FlatStyle = FlatStyle.Flat;
        btnImprimirFicha.Font = new Font("Segoe UI", 9F);
        btnImprimirFicha.ForeColor = Color.Black;
        btnImprimirFicha.Location = new Point(164, 3);
        btnImprimirFicha.Name = "btnImprimirFicha";
        btnImprimirFicha.Size = new Size(140, 32);
        btnImprimirFicha.TabIndex = 1;
        btnImprimirFicha.Text = "Imprimir ficha";
        btnImprimirFicha.UseVisualStyleBackColor = false;
        btnImprimirFicha.Click += btnImprimirFicha_Click;
        // 
        // btnLimpiar
        // 
        btnLimpiar.BackColor = Color.FromArgb(226, 217, 220);
        btnLimpiar.Cursor = Cursors.Hand;
        btnLimpiar.FlatAppearance.BorderSize = 0;
        btnLimpiar.FlatStyle = FlatStyle.Flat;
        btnLimpiar.Font = new Font("Segoe UI", 9F);
        btnLimpiar.ForeColor = Color.Black;
        btnLimpiar.Location = new Point(312, 3);
        btnLimpiar.Name = "btnLimpiar";
        btnLimpiar.Size = new Size(140, 32);
        btnLimpiar.TabIndex = 2;
        btnLimpiar.Text = "Limpiar";
        btnLimpiar.UseVisualStyleBackColor = false;
        btnLimpiar.Click += btnLimpiar_Click;
        // 
        // btnVolver
        // 
        btnVolver.BackColor = Color.FromArgb(220, 200, 204);
        btnVolver.FlatAppearance.BorderColor = Color.FromArgb(186, 162, 168);
        btnVolver.FlatStyle = FlatStyle.Flat;
        btnVolver.Font = new Font("Segoe UI", 9F);
        btnVolver.ForeColor = Color.Black;
        btnVolver.Location = new Point(460, 3);
        btnVolver.Name = "btnVolver";
        btnVolver.Size = new Size(140, 32);
        btnVolver.TabIndex = 3;
        btnVolver.Text = "Volver al panel";
        btnVolver.UseVisualStyleBackColor = false;
        btnVolver.Click += btnVolver_Click;
        // 
        // grpDatos
        // 
        grpDatos.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
        grpDatos.Controls.Add(lblBuscarMascota);
        grpDatos.Controls.Add(pnlContenedorBusqueda);
        grpDatos.Controls.Add(lblNombre);
        grpDatos.Controls.Add(txtNombre);
        grpDatos.Controls.Add(lblPropietario);
        grpDatos.Controls.Add(txtPropietario);
        grpDatos.Controls.Add(lblEspecie);
        grpDatos.Controls.Add(txtEspecie);
        grpDatos.Controls.Add(lblRaza);
        grpDatos.Controls.Add(txtRaza);
        grpDatos.Controls.Add(lblSexo);
        grpDatos.Controls.Add(rbMacho);
        grpDatos.Controls.Add(rbHembra);
        grpDatos.Controls.Add(lblFechaNacimiento);
        grpDatos.Controls.Add(dtpFechaNacimiento);
        grpDatos.Controls.Add(lblColor);
        grpDatos.Controls.Add(txtColor);
        grpDatos.Font = new Font("Segoe UI", 9F);
        grpDatos.ForeColor = Color.FromArgb(58, 53, 59);
        grpDatos.Location = new Point(16, 12);
        grpDatos.Name = "grpDatos";
        grpDatos.Size = new Size(1068, 190);
        grpDatos.TabIndex = 0;
        grpDatos.TabStop = false;
        grpDatos.Text = "Datos de la mascota";
        // 
        // lblBuscarMascota
        // 
        lblBuscarMascota.Location = new Point(16, 32);
        lblBuscarMascota.Name = "lblBuscarMascota";
        lblBuscarMascota.Size = new Size(118, 23);
        lblBuscarMascota.TabIndex = 0;
        lblBuscarMascota.Text = "Buscar mascota";
        lblBuscarMascota.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // pnlContenedorBusqueda
        // 
        pnlContenedorBusqueda.BackColor = Color.FromArgb(226, 217, 220);
        pnlContenedorBusqueda.Controls.Add(cboBuscarMascota);
        pnlContenedorBusqueda.Location = new Point(139, 31);
        pnlContenedorBusqueda.Name = "pnlContenedorBusqueda";
        pnlContenedorBusqueda.Padding = new Padding(1);
        pnlContenedorBusqueda.Size = new Size(906, 25);
        pnlContenedorBusqueda.TabIndex = 1;
        // 
        // cboBuscarMascota
        // 
        cboBuscarMascota.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
        cboBuscarMascota.AutoCompleteSource = AutoCompleteSource.ListItems;
        cboBuscarMascota.BackColor = Color.White;
        cboBuscarMascota.Dock = DockStyle.Fill;
        cboBuscarMascota.FlatStyle = FlatStyle.Flat;
        cboBuscarMascota.Font = new Font("Segoe UI", 9F);
        cboBuscarMascota.ForeColor = Color.FromArgb(58, 53, 59);
        cboBuscarMascota.FormattingEnabled = true;
        cboBuscarMascota.Location = new Point(1, 1);
        cboBuscarMascota.Name = "cboBuscarMascota";
        cboBuscarMascota.Size = new Size(904, 23);
        cboBuscarMascota.TabIndex = 0;
        // 
        // lblNombre
        // 
        lblNombre.Location = new Point(16, 72);
        lblNombre.Name = "lblNombre";
        lblNombre.Size = new Size(118, 23);
        lblNombre.TabIndex = 2;
        lblNombre.Text = "Nombre";
        lblNombre.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // txtNombre
        // 
        txtNombre.BackColor = Color.White;
        txtNombre.BorderStyle = BorderStyle.FixedSingle;
        txtNombre.Location = new Point(140, 72);
        txtNombre.Name = "txtNombre";
        txtNombre.ReadOnly = true;
        txtNombre.Size = new Size(360, 23);
        txtNombre.TabIndex = 3;
        // 
        // lblPropietario
        // 
        lblPropietario.Location = new Point(528, 72);
        lblPropietario.Name = "lblPropietario";
        lblPropietario.Size = new Size(110, 23);
        lblPropietario.TabIndex = 4;
        lblPropietario.Text = "Propietario";
        lblPropietario.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // txtPropietario
        // 
        txtPropietario.BackColor = Color.White;
        txtPropietario.BorderStyle = BorderStyle.FixedSingle;
        txtPropietario.Location = new Point(652, 72);
        txtPropietario.Name = "txtPropietario";
        txtPropietario.ReadOnly = true;
        txtPropietario.Size = new Size(392, 23);
        txtPropietario.TabIndex = 5;
        // 
        // lblEspecie
        // 
        lblEspecie.Location = new Point(16, 108);
        lblEspecie.Name = "lblEspecie";
        lblEspecie.Size = new Size(118, 23);
        lblEspecie.TabIndex = 6;
        lblEspecie.Text = "Especie";
        lblEspecie.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // txtEspecie
        // 
        txtEspecie.BackColor = Color.White;
        txtEspecie.BorderStyle = BorderStyle.FixedSingle;
        txtEspecie.Location = new Point(140, 108);
        txtEspecie.Name = "txtEspecie";
        txtEspecie.ReadOnly = true;
        txtEspecie.Size = new Size(160, 23);
        txtEspecie.TabIndex = 7;
        // 
        // lblRaza
        // 
        lblRaza.Location = new Point(320, 108);
        lblRaza.Name = "lblRaza";
        lblRaza.Size = new Size(50, 23);
        lblRaza.TabIndex = 8;
        lblRaza.Text = "Raza";
        lblRaza.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // txtRaza
        // 
        txtRaza.BackColor = Color.White;
        txtRaza.BorderStyle = BorderStyle.FixedSingle;
        txtRaza.Location = new Point(376, 108);
        txtRaza.Name = "txtRaza";
        txtRaza.ReadOnly = true;
        txtRaza.Size = new Size(124, 23);
        txtRaza.TabIndex = 9;
        // 
        // lblSexo
        // 
        lblSexo.Location = new Point(528, 108);
        lblSexo.Name = "lblSexo";
        lblSexo.Size = new Size(110, 23);
        lblSexo.TabIndex = 10;
        lblSexo.Text = "Sexo";
        lblSexo.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // rbMacho
        // 
        rbMacho.AutoSize = true;
        rbMacho.Cursor = Cursors.Hand;
        rbMacho.Enabled = false;
        rbMacho.Font = new Font("Segoe UI", 9F);
        rbMacho.ForeColor = Color.FromArgb(58, 53, 59);
        rbMacho.Location = new Point(644, 110);
        rbMacho.Name = "rbMacho";
        rbMacho.Size = new Size(62, 19);
        rbMacho.TabIndex = 6;
        rbMacho.TabStop = true;
        rbMacho.Text = "Macho";
        rbMacho.UseVisualStyleBackColor = true;
        // 
        // rbHembra
        // 
        rbHembra.AutoSize = true;
        rbHembra.Cursor = Cursors.Hand;
        rbHembra.Enabled = false;
        rbHembra.Font = new Font("Segoe UI", 9F);
        rbHembra.ForeColor = Color.FromArgb(58, 53, 59);
        rbHembra.Location = new Point(720, 110);
        rbHembra.Name = "rbHembra";
        rbHembra.Size = new Size(68, 19);
        rbHembra.TabIndex = 7;
        rbHembra.TabStop = true;
        rbHembra.Text = "Hembra";
        rbHembra.UseVisualStyleBackColor = true;
        // 
        // lblFechaNacimiento
        // 
        lblFechaNacimiento.Location = new Point(16, 155);
        lblFechaNacimiento.Name = "lblFechaNacimiento";
        lblFechaNacimiento.Size = new Size(118, 23);
        lblFechaNacimiento.TabIndex = 11;
        lblFechaNacimiento.Text = "Fecha de nacimiento";
        lblFechaNacimiento.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // dtpFechaNacimiento
        // 
        dtpFechaNacimiento.Enabled = false;
        dtpFechaNacimiento.Format = DateTimePickerFormat.Short;
        dtpFechaNacimiento.Location = new Point(140, 151);
        dtpFechaNacimiento.Name = "dtpFechaNacimiento";
        dtpFechaNacimiento.Size = new Size(160, 23);
        dtpFechaNacimiento.TabIndex = 12;
        // 
        // lblColor
        // 
        lblColor.Location = new Point(825, 110);
        lblColor.Name = "lblColor";
        lblColor.Size = new Size(50, 23);
        lblColor.TabIndex = 13;
        lblColor.Text = "Color";
        lblColor.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // txtColor
        // 
        txtColor.BackColor = Color.White;
        txtColor.BorderStyle = BorderStyle.FixedSingle;
        txtColor.Location = new Point(881, 110);
        txtColor.Name = "txtColor";
        txtColor.ReadOnly = true;
        txtColor.Size = new Size(124, 23);
        txtColor.TabIndex = 14;
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
        lblInfoEstado.Size = new Size(109, 17);
        lblInfoEstado.Text = "Módulo clínico listo";
        // 
        // FormFichaMedica
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
        Name = "FormFichaMedica";
        StartPosition = FormStartPosition.CenterParent;
        Text = "CLÍNICA VETERINARIA — ATENCIÓN CLÍNICA — HISTORIAL CLÍNICO";
        Load += FormFichaMedica_Load;
        pnlEncabezado.ResumeLayout(false);
        pnlContenido.ResumeLayout(false);
        grpHistorial.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)dgvHistorialClinico).EndInit();
        pnlBotones.ResumeLayout(false);
        grpDatos.ResumeLayout(false);
        grpDatos.PerformLayout();
        pnlContenedorBusqueda.ResumeLayout(false);
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
    private Label lblBuscarMascota;
    private Panel pnlContenedorBusqueda;
    private ComboBox cboBuscarMascota;
    private Label lblNombre;
    private TextBox txtNombre;
    private Label lblPropietario;
    private TextBox txtPropietario;
    private Label lblEspecie;
    private TextBox txtEspecie;
    private Label lblRaza;
    private TextBox txtRaza;
    private Label lblSexo;
    private RadioButton rbMacho;
    private RadioButton rbHembra;
    private Label lblFechaNacimiento;
    private DateTimePicker dtpFechaNacimiento;
    private Label lblColor;
    private TextBox txtColor;
    private Panel pnlBotones;
    private Button btnVerHistorial;
    private Button btnImprimirFicha;
    private Button btnLimpiar;
    private Button btnVolver;
    private GroupBox grpHistorial;
    private DataGridView dgvHistorialClinico;
    private DataGridViewTextBoxColumn colFecha;
    private DataGridViewTextBoxColumn colMotivo;
    private DataGridViewTextBoxColumn colDiagnostico;
    private DataGridViewTextBoxColumn colPeso;
    private DataGridViewTextBoxColumn colTemperatura;
    private DataGridViewTextBoxColumn colTratamiento;
    private DataGridViewTextBoxColumn colProximoControl;
    private StatusStrip barraEstado;
    private ToolStripStatusLabel lblInfoEstado;
}
