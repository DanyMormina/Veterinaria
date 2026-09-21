namespace Veterinaria.WinForms.Vistas.Veterinario;

partial class FormConsultas
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
        lblMascota = new Label();
        cboMascota = new ComboBox();
        lblPropietario = new Label();
        cboPropietario = new ComboBox();
        lblMotivo = new Label();
        txtMotivo = new TextBox();
        lblPeso = new Label();
        txtPeso = new TextBox();
        lblTemperatura = new Label();
        txtTemperatura = new TextBox();
        lblProximoControl = new Label();
        dtpProximoControl = new DateTimePicker();
        lblDiagnostico = new Label();
        txtDiagnostico = new TextBox();
        lblObservaciones = new Label();
        txtObservaciones = new TextBox();
        pnlBotones = new Panel();
        btnGuardar = new Button();
        btnModificar = new Button();
        btnLimpiar = new Button();
        btnVolver = new Button();
        grpHistorial = new GroupBox();
        dgvConsultas = new DataGridView();
        colId = new DataGridViewTextBoxColumn();
        colFecha = new DataGridViewTextBoxColumn();
        colMascota = new DataGridViewTextBoxColumn();
        colPropietario = new DataGridViewTextBoxColumn();
        colMotivo = new DataGridViewTextBoxColumn();
        colDiagnostico = new DataGridViewTextBoxColumn();
        colProximoControl = new DataGridViewTextBoxColumn();
        barraEstado = new StatusStrip();
        lblInfoEstado = new ToolStripStatusLabel();
        pnlEncabezado.SuspendLayout();
        pnlContenido.SuspendLayout();
        grpDatos.SuspendLayout();
        pnlBotones.SuspendLayout();
        grpHistorial.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)dgvConsultas).BeginInit();
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
        lblTitulo.Text = "CLÍNICA VETERINARIA — ATENCIÓN CLÍNICA — CONSULTAS";
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
        // grpDatos
        // 
        grpDatos.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
        grpDatos.Controls.Add(lblMascota);
        grpDatos.Controls.Add(cboMascota);
        grpDatos.Controls.Add(lblPropietario);
        grpDatos.Controls.Add(cboPropietario);
        grpDatos.Controls.Add(lblMotivo);
        grpDatos.Controls.Add(txtMotivo);
        grpDatos.Controls.Add(lblPeso);
        grpDatos.Controls.Add(txtPeso);
        grpDatos.Controls.Add(lblTemperatura);
        grpDatos.Controls.Add(txtTemperatura);
        grpDatos.Controls.Add(lblProximoControl);
        grpDatos.Controls.Add(dtpProximoControl);
        grpDatos.Controls.Add(lblDiagnostico);
        grpDatos.Controls.Add(txtDiagnostico);
        grpDatos.Controls.Add(lblObservaciones);
        grpDatos.Controls.Add(txtObservaciones);
        grpDatos.Font = new Font("Segoe UI", 9F);
        grpDatos.ForeColor = Color.FromArgb(58, 53, 59);
        grpDatos.Location = new Point(16, 12);
        grpDatos.Name = "grpDatos";
        grpDatos.Size = new Size(1068, 248);
        grpDatos.TabIndex = 0;
        grpDatos.TabStop = false;
        grpDatos.Text = "Datos de la consulta";
        // 
        // lblMascota
        // 
        lblMascota.Location = new Point(16, 32);
        lblMascota.Name = "lblMascota";
        lblMascota.Size = new Size(110, 23);
        lblMascota.Text = "Mascota";
        lblMascota.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // cboMascota
        // 
        cboMascota.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
        cboMascota.AutoCompleteSource = AutoCompleteSource.ListItems;
        cboMascota.BackColor = Color.White;
        cboMascota.DropDownStyle = ComboBoxStyle.DropDown;
        cboMascota.FormattingEnabled = true;
        cboMascota.Location = new Point(132, 32);
        cboMascota.Name = "cboMascota";
        cboMascota.Size = new Size(360, 23);
        // 
        // lblPropietario
        // 
        lblPropietario.Location = new Point(528, 32);
        lblPropietario.Name = "lblPropietario";
        lblPropietario.Size = new Size(110, 23);
        lblPropietario.Text = "Propietario";
        lblPropietario.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // cboPropietario
        // 
        cboPropietario.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
        cboPropietario.AutoCompleteSource = AutoCompleteSource.ListItems;
        cboPropietario.BackColor = Color.White;
        cboPropietario.DropDownStyle = ComboBoxStyle.DropDown;
        cboPropietario.FormattingEnabled = true;
        cboPropietario.Location = new Point(644, 32);
        cboPropietario.Name = "cboPropietario";
        cboPropietario.Size = new Size(400, 23);
        // 
        // lblMotivo
        // 
        lblMotivo.Location = new Point(16, 68);
        lblMotivo.Name = "lblMotivo";
        lblMotivo.Size = new Size(110, 23);
        lblMotivo.Text = "Motivo";
        lblMotivo.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // txtMotivo
        // 
        txtMotivo.BackColor = Color.White;
        txtMotivo.BorderStyle = BorderStyle.FixedSingle;
        txtMotivo.Location = new Point(132, 68);
        txtMotivo.Name = "txtMotivo";
        txtMotivo.Size = new Size(912, 23);
        // 
        // lblPeso
        // 
        lblPeso.Location = new Point(16, 104);
        lblPeso.Name = "lblPeso";
        lblPeso.Size = new Size(110, 23);
        lblPeso.Text = "Peso";
        lblPeso.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // txtPeso
        // 
        txtPeso.BackColor = Color.White;
        txtPeso.BorderStyle = BorderStyle.FixedSingle;
        txtPeso.Location = new Point(132, 104);
        txtPeso.Name = "txtPeso";
        txtPeso.Size = new Size(160, 23);
        // 
        // lblTemperatura
        // 
        lblTemperatura.Location = new Point(312, 104);
        lblTemperatura.Name = "lblTemperatura";
        lblTemperatura.Size = new Size(90, 23);
        lblTemperatura.Text = "Temperatura";
        lblTemperatura.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // txtTemperatura
        // 
        txtTemperatura.BackColor = Color.White;
        txtTemperatura.BorderStyle = BorderStyle.FixedSingle;
        txtTemperatura.Location = new Point(408, 104);
        txtTemperatura.Name = "txtTemperatura";
        txtTemperatura.Size = new Size(84, 23);
        // 
        // lblProximoControl
        // 
        lblProximoControl.Location = new Point(528, 104);
        lblProximoControl.Name = "lblProximoControl";
        lblProximoControl.Size = new Size(110, 23);
        lblProximoControl.Text = "Próximo control";
        lblProximoControl.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // dtpProximoControl
        // 
        dtpProximoControl.Format = DateTimePickerFormat.Short;
        dtpProximoControl.Location = new Point(644, 104);
        dtpProximoControl.Name = "dtpProximoControl";
        dtpProximoControl.Size = new Size(160, 23);
        // 
        // lblDiagnostico
        // 
        lblDiagnostico.Location = new Point(16, 140);
        lblDiagnostico.Name = "lblDiagnostico";
        lblDiagnostico.Size = new Size(110, 23);
        lblDiagnostico.Text = "Diagnóstico";
        lblDiagnostico.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // txtDiagnostico
        // 
        txtDiagnostico.BackColor = Color.White;
        txtDiagnostico.BorderStyle = BorderStyle.FixedSingle;
        txtDiagnostico.Location = new Point(132, 140);
        txtDiagnostico.Name = "txtDiagnostico";
        txtDiagnostico.Size = new Size(912, 23);
        // 
        // lblObservaciones
        // 
        lblObservaciones.Location = new Point(16, 176);
        lblObservaciones.Name = "lblObservaciones";
        lblObservaciones.Size = new Size(110, 23);
        lblObservaciones.Text = "Observaciones";
        lblObservaciones.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // txtObservaciones
        // 
        txtObservaciones.BackColor = Color.White;
        txtObservaciones.BorderStyle = BorderStyle.FixedSingle;
        txtObservaciones.Location = new Point(132, 176);
        txtObservaciones.Multiline = true;
        txtObservaciones.Name = "txtObservaciones";
        txtObservaciones.Size = new Size(912, 56);
        // 
        // 
        // pnlBotones
        // 
        // pnlBotones
        // 
        pnlBotones.Anchor = AnchorStyles.Top | AnchorStyles.Left;
        pnlBotones.Controls.Add(btnGuardar);
        pnlBotones.Controls.Add(btnModificar);
        pnlBotones.Controls.Add(btnLimpiar);
        pnlBotones.Controls.Add(btnVolver);
        pnlBotones.Location = new Point(16, 268);
        pnlBotones.Name = "pnlBotones";
        pnlBotones.Size = new Size(1068, 40);
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
        btnGuardar.Location = new Point(0, 4);
        btnGuardar.Name = "btnGuardar";
        btnGuardar.Size = new Size(100, 32);
        btnGuardar.TabIndex = 0;
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
        btnModificar.Location = new Point(108, 4);
        btnModificar.Name = "btnModificar";
        btnModificar.Size = new Size(100, 32);
        btnModificar.TabIndex = 1;
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
        btnLimpiar.Location = new Point(216, 4);
        btnLimpiar.Name = "btnLimpiar";
        btnLimpiar.Size = new Size(100, 32);
        btnLimpiar.TabIndex = 2;
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
        btnVolver.Location = new Point(324, 4);
        btnVolver.Name = "btnVolver";
        btnVolver.Size = new Size(140, 32);
        btnVolver.TabIndex = 3;
        btnVolver.Text = "Volver al panel";
        btnVolver.UseVisualStyleBackColor = false;
        btnVolver.Click += btnVolver_Click;
        // 
        // grpHistorial
        // 
        grpHistorial.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        grpHistorial.Controls.Add(dgvConsultas);
        grpHistorial.Font = new Font("Segoe UI", 9F);
        grpHistorial.ForeColor = Color.FromArgb(58, 53, 59);
        grpHistorial.Location = new Point(16, 316);
        grpHistorial.Name = "grpHistorial";
        grpHistorial.Size = new Size(1068, 296);
        grpHistorial.TabIndex = 2;
        grpHistorial.TabStop = false;
        grpHistorial.Text = "Historial de consultas";
        // 
        // dgvConsultas
        // 
        dgvConsultas.AllowUserToAddRows = false;
        dgvConsultas.AllowUserToDeleteRows = false;
        dgvConsultas.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        dgvConsultas.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        dgvConsultas.BackgroundColor = Color.White;
        dgvConsultas.BorderStyle = BorderStyle.FixedSingle;
        dgvConsultas.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
        dgvConsultas.Columns.AddRange(new DataGridViewColumn[] { colId, colFecha, colMascota, colPropietario, colMotivo, colDiagnostico, colProximoControl });
        dgvConsultas.EnableHeadersVisualStyles = false;
        dgvConsultas.ColumnHeadersDefaultCellStyle.BackColor = Color.White;
        dgvConsultas.ColumnHeadersDefaultCellStyle.ForeColor = Color.Black;
        dgvConsultas.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
        dgvConsultas.DefaultCellStyle.SelectionBackColor = Color.FromArgb(226, 217, 220);
        dgvConsultas.DefaultCellStyle.SelectionForeColor = Color.FromArgb(58, 53, 59);
        dgvConsultas.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(250, 244, 244);
        dgvConsultas.Location = new Point(16, 28);
        dgvConsultas.MultiSelect = false;
        dgvConsultas.Name = "dgvConsultas";
        dgvConsultas.ReadOnly = true;
        dgvConsultas.RowHeadersVisible = false;
        dgvConsultas.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        dgvConsultas.Size = new Size(1036, 252);
        dgvConsultas.TabIndex = 0;
        dgvConsultas.CellClick += dgvConsultas_CellClick;
        // 
        // colId
        // 
        colId.HeaderText = "ID";
        colId.Name = "colId";
        colId.Visible = false;
        // 
        // colFecha
        // 
        colFecha.HeaderText = "Fecha";
        colFecha.Name = "colFecha";
        colMascota.HeaderText = "Mascota";
        colMascota.Name = "colMascota";
        colPropietario.HeaderText = "Propietario";
        colPropietario.Name = "colPropietario";
        colMotivo.HeaderText = "Motivo";
        colMotivo.Name = "colMotivo";
        colDiagnostico.HeaderText = "Diagnóstico";
        colDiagnostico.Name = "colDiagnostico";
        colProximoControl.HeaderText = "Próximo control";
        colProximoControl.Name = "colProximoControl";
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
        // FormConsultas
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
        Name = "FormConsultas";
        StartPosition = FormStartPosition.CenterParent;
        Text = "CLÍNICA VETERINARIA — ATENCIÓN CLÍNICA — CONSULTAS";
        Load += FormConsultas_Load;
        pnlEncabezado.ResumeLayout(false);
        pnlContenido.ResumeLayout(false);
        grpDatos.ResumeLayout(false);
        grpDatos.PerformLayout();
        pnlBotones.ResumeLayout(false);
        grpHistorial.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)dgvConsultas).EndInit();
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
    private Label lblMascota;
    private ComboBox cboMascota;
    private Label lblPropietario;
    private ComboBox cboPropietario;
    private Label lblMotivo;
    private TextBox txtMotivo;
    private Label lblPeso;
    private TextBox txtPeso;
    private Label lblTemperatura;
    private TextBox txtTemperatura;
    private Label lblProximoControl;
    private DateTimePicker dtpProximoControl;
    private Label lblDiagnostico;
    private TextBox txtDiagnostico;
    private Label lblObservaciones;
    private TextBox txtObservaciones;
    private Panel pnlBotones;
    private Button btnGuardar;
    private Button btnModificar;
    private Button btnLimpiar;
    private Button btnVolver;
    private GroupBox grpHistorial;
    private DataGridView dgvConsultas;
    private DataGridViewTextBoxColumn colId;
    private DataGridViewTextBoxColumn colFecha;
    private DataGridViewTextBoxColumn colMascota;
    private DataGridViewTextBoxColumn colPropietario;
    private DataGridViewTextBoxColumn colMotivo;
    private DataGridViewTextBoxColumn colDiagnostico;
    private DataGridViewTextBoxColumn colProximoControl;
    private StatusStrip barraEstado;
    private ToolStripStatusLabel lblInfoEstado;
}
