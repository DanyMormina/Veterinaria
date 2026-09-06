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
        pnlEncabezado = new Panel();
        lblTitulo = new Label();
        lblUsuarioSesion = new Label();
        pnlContenido = new Panel();
        grpAplicacion = new GroupBox();
        lblMascota = new Label();
        cboMascota = new ComboBox();
        lblConsulta = new Label();
        cboConsulta = new ComboBox();
        lblVacuna = new Label();
        cboVacuna = new ComboBox();
        lblFechaAplicacion = new Label();
        dtpFechaAplicacion = new DateTimePicker();
        lblProximaDosis = new Label();
        dtpProximaDosis = new DateTimePicker();
        lblObservaciones = new Label();
        txtObservaciones = new TextBox();
        grpControles = new GroupBox();
        lblMascotaControl = new Label();
        cboMascotaControl = new ComboBox();
        lblFechaRecomendada = new Label();
        dtpFechaRecomendada = new DateTimePicker();
        lblMotivoControl = new Label();
        txtMotivoControl = new TextBox();
        pnlBotones = new Panel();
        btnNuevo = new Button();
        btnGuardar = new Button();
        btnModificar = new Button();
        btnCancelar = new Button();
        btnVolver = new Button();
        dgvVacunas = new DataGridView();
        colMascota = new DataGridViewTextBoxColumn();
        colVacuna = new DataGridViewTextBoxColumn();
        colFechaAplicacion = new DataGridViewTextBoxColumn();
        colProximaDosis = new DataGridViewTextBoxColumn();
        colProximoControl = new DataGridViewTextBoxColumn();
        colObservaciones = new DataGridViewTextBoxColumn();
        barraEstado = new StatusStrip();
        lblInfoEstado = new ToolStripStatusLabel();
        pnlEncabezado.SuspendLayout();
        pnlContenido.SuspendLayout();
        grpAplicacion.SuspendLayout();
        grpControles.SuspendLayout();
        pnlBotones.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)dgvVacunas).BeginInit();
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
        lblTitulo.Text = "CLÍNICA VETERINARIA — ATENCIÓN CLÍNICA — CONTROLES";
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
        pnlContenido.Controls.Add(grpControles);
        pnlContenido.Controls.Add(grpAplicacion);
        pnlContenido.Dock = DockStyle.Fill;
        pnlContenido.Location = new Point(0, 50);
        pnlContenido.Name = "pnlContenido";
        pnlContenido.Size = new Size(1100, 628);
        pnlContenido.TabIndex = 1;
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
        grpAplicacion.Controls.Add(lblFechaAplicacion);
        grpAplicacion.Controls.Add(dtpFechaAplicacion);
        grpAplicacion.Controls.Add(lblProximaDosis);
        grpAplicacion.Controls.Add(dtpProximaDosis);
        grpAplicacion.Controls.Add(lblFechaRecomendada);
        grpAplicacion.Controls.Add(dtpFechaRecomendada);
        grpAplicacion.Controls.Add(lblObservaciones);
        grpAplicacion.Controls.Add(txtObservaciones);
        grpAplicacion.Font = new Font("Segoe UI", 9F);
        grpAplicacion.ForeColor = Color.FromArgb(58, 53, 59);
        grpAplicacion.Location = new Point(16, 12);
        grpAplicacion.Name = "grpAplicacion";
        grpAplicacion.Size = new Size(1068, 196);
        grpAplicacion.TabIndex = 0;
        grpAplicacion.TabStop = false;
        grpAplicacion.Text = "Aplicación de vacuna";
        // 
        // lblMascota
        // 
        lblMascota.Location = new Point(16, 32);
        lblMascota.Name = "lblMascota";
        lblMascota.Size = new Size(118, 23);
        lblMascota.Text = "Mascota";
        lblMascota.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // cboMascota
        // 
        cboMascota.BackColor = Color.White;
        cboMascota.DropDownStyle = ComboBoxStyle.DropDownList;
        cboMascota.FormattingEnabled = true;
        cboMascota.Location = new Point(140, 32);
        cboMascota.Name = "cboMascota";
        cboMascota.Size = new Size(360, 23);
        // 
        // lblConsulta
        // 
        lblConsulta.Location = new Point(528, 32);
        lblConsulta.Name = "lblConsulta";
        lblConsulta.Size = new Size(110, 23);
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
        // 
        // lblVacuna
        // 
        lblVacuna.Location = new Point(16, 68);
        lblVacuna.Name = "lblVacuna";
        lblVacuna.Size = new Size(124, 23);
        lblVacuna.Text = "Vacuna aplicada";
        lblVacuna.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // cboVacuna
        // 
        cboVacuna.BackColor = Color.White;
        cboVacuna.DropDownStyle = ComboBoxStyle.DropDownList;
        cboVacuna.FormattingEnabled = true;
        cboVacuna.Location = new Point(140, 68);
        cboVacuna.Name = "cboVacuna";
        cboVacuna.Size = new Size(360, 23);
        // 
        // lblFechaAplicacion
        // 
        lblFechaAplicacion.Location = new Point(528, 68);
        lblFechaAplicacion.Name = "lblFechaAplicacion";
        lblFechaAplicacion.Size = new Size(110, 23);
        lblFechaAplicacion.Text = "Fecha de aplicación";
        lblFechaAplicacion.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // dtpFechaAplicacion
        // 
        dtpFechaAplicacion.Format = DateTimePickerFormat.Short;
        dtpFechaAplicacion.Location = new Point(644, 68);
        dtpFechaAplicacion.Name = "dtpFechaAplicacion";
        dtpFechaAplicacion.Size = new Size(160, 23);
        // 
        // lblProximaDosis
        // 
        lblProximaDosis.Location = new Point(16, 104);
        lblProximaDosis.Name = "lblProximaDosis";
        lblProximaDosis.Size = new Size(118, 23);
        lblProximaDosis.Text = "Próxima dosis";
        lblProximaDosis.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // dtpProximaDosis
        // 
        dtpProximaDosis.Format = DateTimePickerFormat.Short;
        dtpProximaDosis.Location = new Point(140, 104);
        dtpProximaDosis.Name = "dtpProximaDosis";
        dtpProximaDosis.Size = new Size(160, 23);
        // 
        // lblFechaRecomendada
        // 
        lblFechaRecomendada.Location = new Point(328, 104);
        lblFechaRecomendada.Name = "lblFechaRecomendada";
        lblFechaRecomendada.Size = new Size(230, 23);
        lblFechaRecomendada.Text = "Fecha recomendada del próximo control";
        lblFechaRecomendada.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // dtpFechaRecomendada
        // 
        dtpFechaRecomendada.Format = DateTimePickerFormat.Short;
        dtpFechaRecomendada.Location = new Point(564, 104);
        dtpFechaRecomendada.Name = "dtpFechaRecomendada";
        dtpFechaRecomendada.Size = new Size(160, 23);
        // 
        // lblObservaciones
        // 
        lblObservaciones.Location = new Point(16, 140);
        lblObservaciones.Name = "lblObservaciones";
        lblObservaciones.Size = new Size(118, 23);
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
        // 
        // grpControles
        // 
        grpControles.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
        grpControles.Controls.Add(lblMascotaControl);
        grpControles.Controls.Add(cboMascotaControl);
        grpControles.Controls.Add(lblMotivoControl);
        grpControles.Controls.Add(txtMotivoControl);
        grpControles.Font = new Font("Segoe UI", 9F);
        grpControles.ForeColor = Color.FromArgb(58, 53, 59);
        grpControles.Location = new Point(16, 176);
        grpControles.Name = "grpControles";
        grpControles.Size = new Size(1068, 82);
        grpControles.TabIndex = 1;
        grpControles.TabStop = false;
        grpControles.Text = "Próximos controles";
        grpControles.Visible = false;
        // 
        // lblMascotaControl
        // 
        lblMascotaControl.Location = new Point(16, 36);
        lblMascotaControl.Name = "lblMascotaControl";
        lblMascotaControl.Size = new Size(118, 23);
        lblMascotaControl.Text = "Mascota";
        lblMascotaControl.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // cboMascotaControl
        // 
        cboMascotaControl.BackColor = Color.White;
        cboMascotaControl.DropDownStyle = ComboBoxStyle.DropDownList;
        cboMascotaControl.FormattingEnabled = true;
        cboMascotaControl.Location = new Point(140, 36);
        cboMascotaControl.Name = "cboMascotaControl";
        cboMascotaControl.Size = new Size(220, 23);
        // 
        // lblMotivoControl
        // 
        lblMotivoControl.Location = new Point(660, 36);
        lblMotivoControl.Name = "lblMotivoControl";
        lblMotivoControl.Size = new Size(140, 23);
        lblMotivoControl.Text = "Motivo u observaciones";
        lblMotivoControl.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // txtMotivoControl
        // 
        txtMotivoControl.BackColor = Color.White;
        txtMotivoControl.BorderStyle = BorderStyle.FixedSingle;
        txtMotivoControl.Location = new Point(806, 36);
        txtMotivoControl.Name = "txtMotivoControl";
        txtMotivoControl.Size = new Size(238, 23);
        // 
        // pnlBotones
        // 
        pnlBotones.Controls.Add(btnNuevo);
        pnlBotones.Controls.Add(btnGuardar);
        pnlBotones.Controls.Add(btnModificar);
        pnlBotones.Controls.Add(btnCancelar);
        pnlBotones.Controls.Add(btnVolver);
        pnlBotones.Location = new Point(16, 220);
        pnlBotones.Name = "pnlBotones";
        pnlBotones.Size = new Size(1068, 40);
        pnlBotones.TabIndex = 2;
        // 
        // btnNuevo
        // 
        btnNuevo.BackColor = Color.FromArgb(230, 196, 202);
        btnNuevo.FlatAppearance.BorderColor = Color.FromArgb(186, 162, 168);
        btnNuevo.FlatStyle = FlatStyle.Flat;
        btnNuevo.ForeColor = Color.FromArgb(58, 53, 59);
        btnNuevo.Location = new Point(0, 4);
        btnNuevo.Name = "btnNuevo";
        btnNuevo.Size = new Size(100, 32);
        btnNuevo.Text = "Nuevo";
        btnNuevo.UseVisualStyleBackColor = false;
        // 
        // btnGuardar
        // 
        btnGuardar.BackColor = Color.FromArgb(152, 196, 164);
        btnGuardar.FlatAppearance.BorderColor = Color.FromArgb(112, 158, 124);
        btnGuardar.FlatStyle = FlatStyle.Flat;
        btnGuardar.ForeColor = Color.FromArgb(58, 53, 59);
        btnGuardar.Location = new Point(108, 4);
        btnGuardar.Name = "btnGuardar";
        btnGuardar.Size = new Size(100, 32);
        btnGuardar.Text = "Guardar";
        btnGuardar.UseVisualStyleBackColor = false;
        // 
        // btnModificar
        // 
        btnModificar.BackColor = Color.FromArgb(148, 176, 214);
        btnModificar.FlatAppearance.BorderColor = Color.FromArgb(112, 142, 182);
        btnModificar.FlatStyle = FlatStyle.Flat;
        btnModificar.ForeColor = Color.FromArgb(58, 53, 59);
        btnModificar.Location = new Point(216, 4);
        btnModificar.Name = "btnModificar";
        btnModificar.Size = new Size(100, 32);
        btnModificar.Text = "Modificar";
        btnModificar.UseVisualStyleBackColor = false;
        // 
        // btnCancelar
        // 
        btnCancelar.BackColor = Color.FromArgb(220, 150, 154);
        btnCancelar.FlatAppearance.BorderColor = Color.FromArgb(186, 118, 122);
        btnCancelar.FlatStyle = FlatStyle.Flat;
        btnCancelar.ForeColor = Color.FromArgb(58, 53, 59);
        btnCancelar.Location = new Point(324, 4);
        btnCancelar.Name = "btnCancelar";
        btnCancelar.Size = new Size(100, 32);
        btnCancelar.Text = "Cancelar";
        btnCancelar.UseVisualStyleBackColor = false;
        // 
        // btnVolver
        // 
        btnVolver.BackColor = Color.FromArgb(230, 196, 202);
        btnVolver.FlatAppearance.BorderColor = Color.FromArgb(186, 162, 168);
        btnVolver.FlatStyle = FlatStyle.Flat;
        btnVolver.ForeColor = Color.FromArgb(58, 53, 59);
        btnVolver.Location = new Point(432, 4);
        btnVolver.Name = "btnVolver";
        btnVolver.Size = new Size(140, 32);
        btnVolver.Text = "Volver al panel";
        btnVolver.UseVisualStyleBackColor = false;
        btnVolver.Click += btnVolver_Click;
        // 
        // dgvVacunas
        // 
        dgvVacunas.AllowUserToAddRows = false;
        dgvVacunas.AllowUserToDeleteRows = false;
        dgvVacunas.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        dgvVacunas.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        dgvVacunas.BackgroundColor = Color.White;
        dgvVacunas.BorderStyle = BorderStyle.FixedSingle;
        dgvVacunas.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
        dgvVacunas.Columns.AddRange(new DataGridViewColumn[] { colMascota, colVacuna, colFechaAplicacion, colProximaDosis, colProximoControl, colObservaciones });
        dgvVacunas.Location = new Point(16, 268);
        dgvVacunas.MultiSelect = false;
        dgvVacunas.Name = "dgvVacunas";
        dgvVacunas.ReadOnly = true;
        dgvVacunas.RowHeadersVisible = false;
        dgvVacunas.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        dgvVacunas.Size = new Size(1068, 344);
        // 
        // columnas
        // 
        colMascota.HeaderText = "Mascota";
        colMascota.Name = "colMascota";
        colVacuna.HeaderText = "Vacuna aplicada";
        colVacuna.Name = "colVacuna";
        colFechaAplicacion.HeaderText = "Fecha de aplicación";
        colFechaAplicacion.Name = "colFechaAplicacion";
        colProximaDosis.HeaderText = "Próxima dosis";
        colProximaDosis.Name = "colProximaDosis";
        colProximoControl.HeaderText = "Próximo control";
        colProximoControl.Name = "colProximoControl";
        colObservaciones.HeaderText = "Observaciones";
        colObservaciones.Name = "colObservaciones";
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
        Text = "CLÍNICA VETERINARIA — ATENCIÓN CLÍNICA — CONTROLES";
        Load += FormVacunasControles_Load;
        pnlEncabezado.ResumeLayout(false);
        pnlContenido.ResumeLayout(false);
        grpAplicacion.ResumeLayout(false);
        grpAplicacion.PerformLayout();
        grpControles.ResumeLayout(false);
        grpControles.PerformLayout();
        pnlBotones.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)dgvVacunas).EndInit();
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
    private GroupBox grpControles;
    private Label lblMascotaControl;
    private ComboBox cboMascotaControl;
    private Label lblFechaRecomendada;
    private DateTimePicker dtpFechaRecomendada;
    private Label lblMotivoControl;
    private TextBox txtMotivoControl;
    private Panel pnlBotones;
    private Button btnNuevo;
    private Button btnGuardar;
    private Button btnModificar;
    private Button btnCancelar;
    private Button btnVolver;
    private DataGridView dgvVacunas;
    private DataGridViewTextBoxColumn colMascota;
    private DataGridViewTextBoxColumn colVacuna;
    private DataGridViewTextBoxColumn colFechaAplicacion;
    private DataGridViewTextBoxColumn colProximaDosis;
    private DataGridViewTextBoxColumn colProximoControl;
    private DataGridViewTextBoxColumn colObservaciones;
    private StatusStrip barraEstado;
    private ToolStripStatusLabel lblInfoEstado;
}
