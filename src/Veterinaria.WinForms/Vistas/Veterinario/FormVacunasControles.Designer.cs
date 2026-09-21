namespace Veterinaria.WinForms.Vistas.Veterinario;

partial class FormVacunasControles
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
        dgvVacunas = new DataGridView();
        colMascota = new DataGridViewTextBoxColumn();
        colVacuna = new DataGridViewTextBoxColumn();
        colFechaAplicacion = new DataGridViewTextBoxColumn();
        colProximaDosis = new DataGridViewTextBoxColumn();
        colObservaciones = new DataGridViewTextBoxColumn();
        pnlBotones = new Panel();
        btnGuardar = new Button();
        btnModificar = new Button();
        btnLimpiar = new Button();
        btnVolver = new Button();
        grpAplicacion = new GroupBox();
        lblMascota = new Label();
        cboMascota = new ComboBox();
        lblConsulta = new Label();
        cboConsulta = new ComboBox();
        lblVacuna = new Label();
        cboVacuna = new ComboBox();
        btnAltaVacuna = new Button();
        btnModificarVacuna = new Button();
        lblFechaAplicacion = new Label();
        dtpFechaAplicacion = new DateTimePicker();
        chkVacunaPrevia = new CheckBox();
        lblProximaDosis = new Label();
        dtpProximaDosis = new DateTimePicker();
        lblObservaciones = new Label();
        txtObservaciones = new TextBox();
        barraEstado = new StatusStrip();
        lblInfoEstado = new ToolStripStatusLabel();
        pnlEncabezado.SuspendLayout();
        pnlContenido.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)dgvVacunas).BeginInit();
        pnlBotones.SuspendLayout();
        grpAplicacion.SuspendLayout();
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
        lblTitulo.Size = new Size(720, 50);
        lblTitulo.TabIndex = 0;
        lblTitulo.Text = "CLÍNICA VETERINARIA - ATENCIÓN CLÍNICA - VACUNACIÓN";
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
        pnlContenido.Controls.Add(dgvVacunas);
        pnlContenido.Controls.Add(pnlBotones);
        pnlContenido.Controls.Add(grpAplicacion);
        pnlContenido.Dock = DockStyle.Fill;
        pnlContenido.Location = new Point(0, 50);
        pnlContenido.Name = "pnlContenido";
        pnlContenido.Size = new Size(1100, 628);
        pnlContenido.TabIndex = 1;
        // 
        // dgvVacunas
        // 
        dgvVacunas.AllowUserToAddRows = false;
        dgvVacunas.AllowUserToDeleteRows = false;
        dgvVacunas.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        dgvVacunas.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        dgvVacunas.BackgroundColor = Color.White;
        dataGridViewCellStyle1.Alignment = DataGridViewContentAlignment.MiddleLeft;
        dataGridViewCellStyle1.BackColor = SystemColors.Control;
        dataGridViewCellStyle1.Font = new Font("Segoe UI", 9F);
        dataGridViewCellStyle1.ForeColor = Color.FromArgb(58, 53, 59);
        dataGridViewCellStyle1.SelectionBackColor = SystemColors.Highlight;
        dataGridViewCellStyle1.SelectionForeColor = SystemColors.HighlightText;
        dataGridViewCellStyle1.WrapMode = DataGridViewTriState.True;
        dgvVacunas.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
        dgvVacunas.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
        dgvVacunas.Columns.AddRange(new DataGridViewColumn[] { colMascota, colVacuna, colFechaAplicacion, colProximaDosis, colObservaciones });
        dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleLeft;
        dataGridViewCellStyle2.BackColor = SystemColors.Window;
        dataGridViewCellStyle2.Font = new Font("Segoe UI", 9F);
        dataGridViewCellStyle2.ForeColor = Color.FromArgb(58, 53, 59);
        dataGridViewCellStyle2.SelectionBackColor = SystemColors.Highlight;
        dataGridViewCellStyle2.SelectionForeColor = SystemColors.HighlightText;
        dataGridViewCellStyle2.WrapMode = DataGridViewTriState.False;
        dgvVacunas.DefaultCellStyle = dataGridViewCellStyle2;
        dgvVacunas.Location = new Point(16, 238);
        dgvVacunas.MultiSelect = false;
        dgvVacunas.Name = "dgvVacunas";
        dgvVacunas.ReadOnly = true;
        dgvVacunas.RowHeadersVisible = false;
        dgvVacunas.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        dgvVacunas.Size = new Size(1068, 374);
        dgvVacunas.TabIndex = 0;
        dgvVacunas.SelectionChanged += dgvVacunas_SelectionChanged;
        // 
        // colMascota
        // 
        colMascota.HeaderText = "Mascota";
        colMascota.Name = "colMascota";
        colMascota.ReadOnly = true;
        // 
        // colVacuna
        // 
        colVacuna.HeaderText = "Vacuna aplicada";
        colVacuna.Name = "colVacuna";
        colVacuna.ReadOnly = true;
        // 
        // colFechaAplicacion
        // 
        colFechaAplicacion.HeaderText = "Fecha de aplicación";
        colFechaAplicacion.Name = "colFechaAplicacion";
        colFechaAplicacion.ReadOnly = true;
        // 
        // colProximaDosis
        // 
        colProximaDosis.HeaderText = "Próxima dosis";
        colProximaDosis.Name = "colProximaDosis";
        colProximaDosis.ReadOnly = true;
        // 
        // colObservaciones
        // 
        colObservaciones.HeaderText = "Observaciones";
        colObservaciones.Name = "colObservaciones";
        colObservaciones.ReadOnly = true;
        // 
        // pnlBotones
        // 
        pnlBotones.Controls.Add(btnGuardar);
        pnlBotones.Controls.Add(btnModificar);
        pnlBotones.Controls.Add(btnLimpiar);
        pnlBotones.Controls.Add(btnVolver);
        pnlBotones.Location = new Point(16, 192);
        pnlBotones.Name = "pnlBotones";
        pnlBotones.Size = new Size(1068, 40);
        pnlBotones.TabIndex = 2;
        // 
        // btnGuardar
        // 
        btnGuardar.BackColor = Color.FromArgb(152, 196, 164);
        btnGuardar.FlatAppearance.BorderColor = Color.FromArgb(112, 158, 124);
        btnGuardar.FlatStyle = FlatStyle.Flat;
        btnGuardar.Font = new Font("Segoe UI", 9F);
        btnGuardar.ForeColor = Color.Black;
        btnGuardar.Location = new Point(16, 5);
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
        btnModificar.FlatAppearance.BorderColor = Color.FromArgb(112, 142, 182);
        btnModificar.FlatStyle = FlatStyle.Flat;
        btnModificar.Font = new Font("Segoe UI", 9F);
        btnModificar.ForeColor = Color.Black;
        btnModificar.Location = new Point(124, 5);
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
        btnLimpiar.Location = new Point(232, 5);
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
        btnVolver.FlatAppearance.BorderColor = Color.FromArgb(186, 162, 168);
        btnVolver.FlatStyle = FlatStyle.Flat;
        btnVolver.Font = new Font("Segoe UI", 9F);
        btnVolver.ForeColor = Color.Black;
        btnVolver.Location = new Point(340, 5);
        btnVolver.Name = "btnVolver";
        btnVolver.Size = new Size(140, 32);
        btnVolver.TabIndex = 3;
        btnVolver.Text = "Volver al panel";
        btnVolver.UseVisualStyleBackColor = false;
        btnVolver.Click += btnVolver_Click;
        // 
        // grpAplicacion
        // 
        grpAplicacion.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
        grpAplicacion.Controls.Add(lblMascota);
        grpAplicacion.Controls.Add(cboMascota);
        grpAplicacion.Controls.Add(lblConsulta);
        grpAplicacion.Controls.Add(cboConsulta);
        grpAplicacion.Controls.Add(lblVacuna);
        grpAplicacion.Controls.Add(cboVacuna);
        grpAplicacion.Controls.Add(btnAltaVacuna);
        grpAplicacion.Controls.Add(btnModificarVacuna);
        grpAplicacion.Controls.Add(lblFechaAplicacion);
        grpAplicacion.Controls.Add(dtpFechaAplicacion);
        grpAplicacion.Controls.Add(chkVacunaPrevia);
        grpAplicacion.Controls.Add(lblProximaDosis);
        grpAplicacion.Controls.Add(dtpProximaDosis);
        grpAplicacion.Controls.Add(lblObservaciones);
        grpAplicacion.Controls.Add(txtObservaciones);
        grpAplicacion.Font = new Font("Segoe UI", 9F);
        grpAplicacion.ForeColor = Color.FromArgb(58, 53, 59);
        grpAplicacion.Location = new Point(16, 12);
        grpAplicacion.Name = "grpAplicacion";
        grpAplicacion.Size = new Size(1068, 174);
        grpAplicacion.TabIndex = 0;
        grpAplicacion.TabStop = false;
        grpAplicacion.Text = "Aplicación de vacuna";
        // 
        // lblMascota
        // 
        lblMascota.Location = new Point(16, 32);
        lblMascota.Name = "lblMascota";
        lblMascota.Size = new Size(118, 23);
        lblMascota.TabIndex = 0;
        lblMascota.Text = "Mascota";
        lblMascota.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // cboMascota
        // 
        cboMascota.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
        cboMascota.AutoCompleteSource = AutoCompleteSource.ListItems;
        cboMascota.BackColor = Color.White;
        cboMascota.FormattingEnabled = true;
        cboMascota.Location = new Point(140, 32);
        cboMascota.Name = "cboMascota";
        cboMascota.Size = new Size(360, 23);
        cboMascota.TabIndex = 1;
        cboMascota.SelectedIndexChanged += cboMascota_SelectedIndexChanged;
        // 
        // lblConsulta
        // 
        lblConsulta.Location = new Point(528, 32);
        lblConsulta.Name = "lblConsulta";
        lblConsulta.Size = new Size(110, 23);
        lblConsulta.TabIndex = 2;
        lblConsulta.Text = "Consulta";
        lblConsulta.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // cboConsulta
        // 
        cboConsulta.BackColor = Color.White;
        cboConsulta.DropDownStyle = ComboBoxStyle.DropDownList;
        cboConsulta.FormattingEnabled = true;
        cboConsulta.Location = new Point(644, 32);
        cboConsulta.Name = "cboConsulta";
        cboConsulta.Size = new Size(400, 23);
        cboConsulta.TabIndex = 3;
        cboConsulta.SelectedIndexChanged += cboConsulta_SelectedIndexChanged;
        // 
        // lblVacuna
        // 
        lblVacuna.Location = new Point(16, 68);
        lblVacuna.Name = "lblVacuna";
        lblVacuna.Size = new Size(124, 23);
        lblVacuna.TabIndex = 4;
        lblVacuna.Text = "Vacuna aplicada";
        lblVacuna.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // cboVacuna
        // 
        cboVacuna.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
        cboVacuna.AutoCompleteSource = AutoCompleteSource.ListItems;
        cboVacuna.BackColor = Color.White;
        cboVacuna.FormattingEnabled = true;
        cboVacuna.Location = new Point(140, 68);
        cboVacuna.Name = "cboVacuna";
        cboVacuna.Size = new Size(210, 23);
        cboVacuna.TabIndex = 5;
        cboVacuna.SelectedIndexChanged += cboVacuna_SelectedIndexChanged;
        // 
        // btnAltaVacuna
        // 
        btnAltaVacuna.BackColor = Color.FromArgb(200, 138, 150);
        btnAltaVacuna.Cursor = Cursors.Hand;
        btnAltaVacuna.FlatAppearance.BorderSize = 0;
        btnAltaVacuna.FlatStyle = FlatStyle.Flat;
        btnAltaVacuna.Font = new Font("Segoe UI", 9F);
        btnAltaVacuna.ForeColor = Color.White;
        btnAltaVacuna.Location = new Point(356, 67);
        btnAltaVacuna.Name = "btnAltaVacuna";
        btnAltaVacuna.Size = new Size(70, 25);
        btnAltaVacuna.TabIndex = 6;
        btnAltaVacuna.Text = "Alta Vac.";
        btnAltaVacuna.UseVisualStyleBackColor = false;
        btnAltaVacuna.Click += btnAltaVacuna_Click;
        // 
        // btnModificarVacuna
        // 
        btnModificarVacuna.BackColor = Color.FromArgb(226, 217, 220);
        btnModificarVacuna.Cursor = Cursors.Hand;
        btnModificarVacuna.FlatAppearance.BorderSize = 0;
        btnModificarVacuna.FlatStyle = FlatStyle.Flat;
        btnModificarVacuna.Font = new Font("Segoe UI", 9F);
        btnModificarVacuna.ForeColor = Color.FromArgb(58, 53, 59);
        btnModificarVacuna.Location = new Point(430, 67);
        btnModificarVacuna.Name = "btnModificarVacuna";
        btnModificarVacuna.Size = new Size(70, 25);
        btnModificarVacuna.TabIndex = 7;
        btnModificarVacuna.Text = "Mod. Vac.";
        btnModificarVacuna.UseVisualStyleBackColor = false;
        btnModificarVacuna.Click += btnModificarVacuna_Click;
        // 
        // lblFechaAplicacion
        // 
        lblFechaAplicacion.Location = new Point(528, 68);
        lblFechaAplicacion.Name = "lblFechaAplicacion";
        lblFechaAplicacion.Size = new Size(110, 23);
        lblFechaAplicacion.TabIndex = 8;
        lblFechaAplicacion.Text = "Fecha aplicación";
        lblFechaAplicacion.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // dtpFechaAplicacion
        // 
        dtpFechaAplicacion.Format = DateTimePickerFormat.Short;
        dtpFechaAplicacion.Location = new Point(644, 68);
        dtpFechaAplicacion.Name = "dtpFechaAplicacion";
        dtpFechaAplicacion.Size = new Size(160, 23);
        dtpFechaAplicacion.TabIndex = 9;
        dtpFechaAplicacion.ValueChanged += dtpFechaAplicacion_ValueChanged;
        // 
        // chkVacunaPrevia
        // 
        chkVacunaPrevia.AutoSize = true;
        chkVacunaPrevia.Cursor = Cursors.Hand;
        chkVacunaPrevia.Font = new Font("Segoe UI", 9F);
        chkVacunaPrevia.ForeColor = Color.FromArgb(58, 53, 59);
        chkVacunaPrevia.Location = new Point(820, 70);
        chkVacunaPrevia.Name = "chkVacunaPrevia";
        chkVacunaPrevia.Size = new Size(206, 19);
        chkVacunaPrevia.TabIndex = 10;
        chkVacunaPrevia.Text = "Vacuna previa / Externa (sin costo)";
        chkVacunaPrevia.UseVisualStyleBackColor = true;
        // 
        // lblProximaDosis
        // 
        lblProximaDosis.Location = new Point(16, 104);
        lblProximaDosis.Name = "lblProximaDosis";
        lblProximaDosis.Size = new Size(118, 23);
        lblProximaDosis.TabIndex = 8;
        lblProximaDosis.Text = "Próxima dosis";
        lblProximaDosis.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // dtpProximaDosis
        // 
        dtpProximaDosis.Format = DateTimePickerFormat.Short;
        dtpProximaDosis.Location = new Point(140, 104);
        dtpProximaDosis.Name = "dtpProximaDosis";
        dtpProximaDosis.Size = new Size(160, 23);
        dtpProximaDosis.TabIndex = 9;
        // 
        // lblObservaciones
        // 
        lblObservaciones.Location = new Point(16, 140);
        lblObservaciones.Name = "lblObservaciones";
        lblObservaciones.Size = new Size(118, 23);
        lblObservaciones.TabIndex = 12;
        lblObservaciones.Text = "Observaciones";
        lblObservaciones.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // txtObservaciones
        // 
        txtObservaciones.BackColor = Color.White;
        txtObservaciones.BorderStyle = BorderStyle.FixedSingle;
        txtObservaciones.Location = new Point(140, 140);
        txtObservaciones.Name = "txtObservaciones";
        txtObservaciones.Size = new Size(904, 23);
        txtObservaciones.TabIndex = 13;
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
        // FormVacunasControles
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
        Name = "FormVacunasControles";
        StartPosition = FormStartPosition.CenterParent;
        Text = "CLÍNICA VETERINARIA - ATENCIÓN CLÍNICA - VACUNACIÓN";
        Load += FormVacunasControles_Load;
        pnlEncabezado.ResumeLayout(false);
        pnlContenido.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)dgvVacunas).EndInit();
        pnlBotones.ResumeLayout(false);
        grpAplicacion.ResumeLayout(false);
        grpAplicacion.PerformLayout();
        barraEstado.ResumeLayout(false);
        barraEstado.PerformLayout();
        ResumeLayout(false);
        PerformLayout();
    }

    private Panel pnlEncabezado;
    private Label lblTitulo;
    private Label lblUsuarioSesion;
    private Panel pnlContenido;
    private GroupBox grpAplicacion;
    private Label lblMascota;
    private ComboBox cboMascota;
    private Label lblConsulta;
    private ComboBox cboConsulta;
    private Label lblVacuna;
    private ComboBox cboVacuna;
    private Label lblFechaAplicacion;
    private DateTimePicker dtpFechaAplicacion;
    private Label lblProximaDosis;
    private DateTimePicker dtpProximaDosis;
    private Label lblObservaciones;
    private TextBox txtObservaciones;
    private Panel pnlBotones;
    private Button btnGuardar;
    private Button btnModificar;
    private Button btnLimpiar;
    private Button btnVolver;
    private Button btnAltaVacuna;
    private Button btnModificarVacuna;
    private ComboBox cboMascotas => cboMascota;
    private ComboBox cboConsultas => cboConsulta;
    private ComboBox cboVacunas => cboVacuna;
    private DataGridView dgvVacunas;
    private DataGridViewTextBoxColumn colMascota;
    private DataGridViewTextBoxColumn colVacuna;
    private DataGridViewTextBoxColumn colFechaAplicacion;
    private DataGridViewTextBoxColumn colProximaDosis;
    private DataGridViewTextBoxColumn colObservaciones;
    private StatusStrip barraEstado;
    private ToolStripStatusLabel lblInfoEstado;
    private CheckBox chkVacunaPrevia;
}
