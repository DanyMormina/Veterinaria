namespace Veterinaria.WinForms.Views.Veterinario;

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
        pnlHeader = new Panel();
        lblTitulo = new Label();
        lblUsuarioSesion = new Label();
        pnlContenido = new Panel();
        grpDatos = new GroupBox();
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
        pnlBotones = new Panel();
        btnVerHistorial = new Button();
        btnImprimirFicha = new Button();
        btnVolver = new Button();
        grpHistorial = new GroupBox();
        dgvHistorial = new DataGridView();
        colFecha = new DataGridViewTextBoxColumn();
        colMotivo = new DataGridViewTextBoxColumn();
        colDiagnostico = new DataGridViewTextBoxColumn();
        colPeso = new DataGridViewTextBoxColumn();
        colTemperatura = new DataGridViewTextBoxColumn();
        colTratamiento = new DataGridViewTextBoxColumn();
        colProximoControl = new DataGridViewTextBoxColumn();
        statusStrip = new StatusStrip();
        lblStatusInfo = new ToolStripStatusLabel();
        pnlHeader.SuspendLayout();
        pnlContenido.SuspendLayout();
        grpDatos.SuspendLayout();
        pnlBotones.SuspendLayout();
        grpHistorial.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)dgvHistorial).BeginInit();
        statusStrip.SuspendLayout();
        SuspendLayout();
        // 
        // pnlHeader
        // 
        pnlHeader.BackColor = Color.FromArgb(200, 138, 150);
        pnlHeader.Controls.Add(lblTitulo);
        pnlHeader.Controls.Add(lblUsuarioSesion);
        pnlHeader.Dock = DockStyle.Top;
        pnlHeader.Location = new Point(0, 0);
        pnlHeader.Name = "pnlHeader";
        pnlHeader.Padding = new Padding(16, 0, 16, 0);
        pnlHeader.Size = new Size(1100, 50);
        pnlHeader.TabIndex = 0;
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
        lblTitulo.Text = "CLÍNICA VETERINARIA — ATENCIÓN CLÍNICA — FICHA MÉDICA";
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
        grpDatos.Location = new Point(16, 12);
        grpDatos.Name = "grpDatos";
        grpDatos.Size = new Size(1068, 196);
        grpDatos.TabIndex = 0;
        grpDatos.TabStop = false;
        grpDatos.Text = "Datos de la mascota";
        // 
        // lblBuscarMascota
        // 
        lblBuscarMascota.Location = new Point(16, 32);
        lblBuscarMascota.Name = "lblBuscarMascota";
        lblBuscarMascota.Size = new Size(118, 23);
        lblBuscarMascota.Text = "Buscar mascota";
        lblBuscarMascota.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // txtBuscarMascota
        // 
        txtBuscarMascota.BackColor = Color.White;
        txtBuscarMascota.BorderStyle = BorderStyle.FixedSingle;
        txtBuscarMascota.Location = new Point(140, 32);
        txtBuscarMascota.Name = "txtBuscarMascota";
        txtBuscarMascota.Size = new Size(904, 23);
        // 
        // lblNombre
        // 
        lblNombre.Location = new Point(16, 72);
        lblNombre.Name = "lblNombre";
        lblNombre.Size = new Size(118, 23);
        lblNombre.Text = "Nombre";
        lblNombre.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // txtNombre
        // 
        txtNombre.BackColor = Color.White;
        txtNombre.BorderStyle = BorderStyle.FixedSingle;
        txtNombre.Location = new Point(140, 72);
        txtNombre.Name = "txtNombre";
        txtNombre.Size = new Size(360, 23);
        // 
        // lblPropietario
        // 
        lblPropietario.Location = new Point(528, 72);
        lblPropietario.Name = "lblPropietario";
        lblPropietario.Size = new Size(110, 23);
        lblPropietario.Text = "Propietario";
        lblPropietario.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // txtPropietario
        // 
        txtPropietario.BackColor = Color.White;
        txtPropietario.BorderStyle = BorderStyle.FixedSingle;
        txtPropietario.Location = new Point(644, 72);
        txtPropietario.Name = "txtPropietario";
        txtPropietario.Size = new Size(400, 23);
        // 
        // lblEspecie
        // 
        lblEspecie.Location = new Point(16, 108);
        lblEspecie.Name = "lblEspecie";
        lblEspecie.Size = new Size(118, 23);
        lblEspecie.Text = "Especie";
        lblEspecie.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // txtEspecie
        // 
        txtEspecie.BackColor = Color.White;
        txtEspecie.BorderStyle = BorderStyle.FixedSingle;
        txtEspecie.Location = new Point(140, 108);
        txtEspecie.Name = "txtEspecie";
        txtEspecie.Size = new Size(160, 23);
        // 
        // lblRaza
        // 
        lblRaza.Location = new Point(320, 108);
        lblRaza.Name = "lblRaza";
        lblRaza.Size = new Size(50, 23);
        lblRaza.Text = "Raza";
        lblRaza.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // txtRaza
        // 
        txtRaza.BackColor = Color.White;
        txtRaza.BorderStyle = BorderStyle.FixedSingle;
        txtRaza.Location = new Point(376, 108);
        txtRaza.Name = "txtRaza";
        txtRaza.Size = new Size(124, 23);
        // 
        // lblSexo
        // 
        lblSexo.Location = new Point(528, 108);
        lblSexo.Name = "lblSexo";
        lblSexo.Size = new Size(110, 23);
        lblSexo.Text = "Sexo";
        lblSexo.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // txtSexo
        // 
        txtSexo.BackColor = Color.White;
        txtSexo.BorderStyle = BorderStyle.FixedSingle;
        txtSexo.Location = new Point(644, 108);
        txtSexo.Name = "txtSexo";
        txtSexo.Size = new Size(160, 23);
        // 
        // lblFechaNacimiento
        // 
        lblFechaNacimiento.Location = new Point(16, 144);
        lblFechaNacimiento.Name = "lblFechaNacimiento";
        lblFechaNacimiento.Size = new Size(118, 23);
        lblFechaNacimiento.Text = "Fecha de nacimiento";
        lblFechaNacimiento.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // dtpFechaNacimiento
        // 
        dtpFechaNacimiento.Format = DateTimePickerFormat.Short;
        dtpFechaNacimiento.Location = new Point(140, 144);
        dtpFechaNacimiento.Name = "dtpFechaNacimiento";
        dtpFechaNacimiento.Size = new Size(160, 23);
        // 
        // lblColor
        // 
        lblColor.Location = new Point(528, 144);
        lblColor.Name = "lblColor";
        lblColor.Size = new Size(110, 23);
        lblColor.Text = "Color";
        lblColor.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // txtColor
        // 
        txtColor.BackColor = Color.White;
        txtColor.BorderStyle = BorderStyle.FixedSingle;
        txtColor.Location = new Point(644, 144);
        txtColor.Name = "txtColor";
        txtColor.Size = new Size(160, 23);
        // 
        // pnlBotones
        // 
        pnlBotones.Location = new Point(16, 216);
        pnlBotones.Name = "pnlBotones";
        pnlBotones.Size = new Size(1068, 40);
        pnlBotones.TabIndex = 1;
        pnlBotones.Controls.Add(btnVerHistorial);
        pnlBotones.Controls.Add(btnImprimirFicha);
        pnlBotones.Controls.Add(btnVolver);
        // 
        // btnVerHistorial
        // 
        btnVerHistorial.BackColor = Color.FromArgb(230, 196, 202);
        btnVerHistorial.FlatAppearance.BorderColor = Color.FromArgb(186, 162, 168);
        btnVerHistorial.FlatStyle = FlatStyle.Flat;
        btnVerHistorial.ForeColor = Color.FromArgb(58, 53, 59);
        btnVerHistorial.Location = new Point(0, 4);
        btnVerHistorial.Name = "btnVerHistorial";
        btnVerHistorial.Size = new Size(140, 32);
        btnVerHistorial.Text = "Ver historial";
        btnVerHistorial.UseVisualStyleBackColor = false;
        // 
        // btnImprimirFicha
        // 
        btnImprimirFicha.BackColor = Color.FromArgb(230, 196, 202);
        btnImprimirFicha.FlatAppearance.BorderColor = Color.FromArgb(186, 162, 168);
        btnImprimirFicha.FlatStyle = FlatStyle.Flat;
        btnImprimirFicha.ForeColor = Color.FromArgb(58, 53, 59);
        btnImprimirFicha.Location = new Point(148, 4);
        btnImprimirFicha.Name = "btnImprimirFicha";
        btnImprimirFicha.Size = new Size(140, 32);
        btnImprimirFicha.Text = "Imprimir ficha";
        btnImprimirFicha.UseVisualStyleBackColor = false;
        // 
        // btnVolver
        // 
        btnVolver.BackColor = Color.FromArgb(230, 196, 202);
        btnVolver.FlatAppearance.BorderColor = Color.FromArgb(186, 162, 168);
        btnVolver.FlatStyle = FlatStyle.Flat;
        btnVolver.ForeColor = Color.FromArgb(58, 53, 59);
        btnVolver.Location = new Point(296, 4);
        btnVolver.Name = "btnVolver";
        btnVolver.Size = new Size(140, 32);
        btnVolver.Text = "Volver al panel";
        btnVolver.UseVisualStyleBackColor = false;
        btnVolver.Click += btnVolver_Click;
        // 
        // grpHistorial
        // 
        grpHistorial.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        grpHistorial.Controls.Add(dgvHistorial);
        grpHistorial.Font = new Font("Segoe UI", 9F);
        grpHistorial.ForeColor = Color.FromArgb(58, 53, 59);
        grpHistorial.Location = new Point(16, 264);
        grpHistorial.Name = "grpHistorial";
        grpHistorial.Size = new Size(1068, 348);
        grpHistorial.TabIndex = 2;
        grpHistorial.TabStop = false;
        grpHistorial.Text = "Historial clínico";
        // 
        // dgvHistorial
        // 
        dgvHistorial.AllowUserToAddRows = false;
        dgvHistorial.AllowUserToDeleteRows = false;
        dgvHistorial.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        dgvHistorial.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        dgvHistorial.BackgroundColor = Color.White;
        dgvHistorial.BorderStyle = BorderStyle.FixedSingle;
        dgvHistorial.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
        dgvHistorial.Columns.AddRange(new DataGridViewColumn[] { colFecha, colMotivo, colDiagnostico, colPeso, colTemperatura, colTratamiento, colProximoControl });
        dgvHistorial.Location = new Point(16, 28);
        dgvHistorial.MultiSelect = false;
        dgvHistorial.Name = "dgvHistorial";
        dgvHistorial.ReadOnly = true;
        dgvHistorial.RowHeadersVisible = false;
        dgvHistorial.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        dgvHistorial.Size = new Size(1036, 304);
        // 
        // columnas
        // 
        colFecha.HeaderText = "Fecha";
        colFecha.Name = "colFecha";
        colMotivo.HeaderText = "Motivo";
        colMotivo.Name = "colMotivo";
        colDiagnostico.HeaderText = "Diagnóstico";
        colDiagnostico.Name = "colDiagnostico";
        colPeso.HeaderText = "Peso";
        colPeso.Name = "colPeso";
        colTemperatura.HeaderText = "Temperatura";
        colTemperatura.Name = "colTemperatura";
        colTratamiento.HeaderText = "Tratamiento";
        colTratamiento.Name = "colTratamiento";
        colProximoControl.HeaderText = "Próximo control";
        colProximoControl.Name = "colProximoControl";
        // 
        // statusStrip
        // 
        statusStrip.BackColor = Color.FromArgb(249, 240, 242);
        statusStrip.Items.AddRange(new ToolStripItem[] { lblStatusInfo });
        statusStrip.Location = new Point(0, 678);
        statusStrip.Name = "statusStrip";
        statusStrip.Size = new Size(1100, 22);
        statusStrip.TabIndex = 2;
        // 
        // lblStatusInfo
        // 
        lblStatusInfo.Font = new Font("Segoe UI", 8.25F);
        lblStatusInfo.ForeColor = Color.FromArgb(58, 53, 59);
        lblStatusInfo.Name = "lblStatusInfo";
        lblStatusInfo.Size = new Size(109, 17);
        lblStatusInfo.Text = "Módulo clínico listo";
        // 
        // FormFichaMedica
        // 
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        BackColor = Color.FromArgb(250, 244, 244);
        ClientSize = new Size(1100, 700);
        Controls.Add(pnlContenido);
        Controls.Add(statusStrip);
        Controls.Add(pnlHeader);
        Font = new Font("Segoe UI", 9F);
        ForeColor = Color.FromArgb(58, 53, 59);
        MinimumSize = new Size(1100, 700);
        Name = "FormFichaMedica";
        StartPosition = FormStartPosition.CenterParent;
        Text = "CLÍNICA VETERINARIA — ATENCIÓN CLÍNICA — FICHA MÉDICA";
        Load += FormFichaMedica_Load;
        pnlHeader.ResumeLayout(false);
        pnlContenido.ResumeLayout(false);
        grpDatos.ResumeLayout(false);
        grpDatos.PerformLayout();
        pnlBotones.ResumeLayout(false);
        grpHistorial.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)dgvHistorial).EndInit();
        statusStrip.ResumeLayout(false);
        statusStrip.PerformLayout();
        ResumeLayout(false);
        PerformLayout();
    }

    private Panel pnlHeader;
    private Label lblTitulo;
    private Label lblUsuarioSesion;
    private Panel pnlContenido;
    private GroupBox grpDatos;
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
    private Button btnVerHistorial;
    private Button btnImprimirFicha;
    private Button btnVolver;
    private GroupBox grpHistorial;
    private DataGridView dgvHistorial;
    private DataGridViewTextBoxColumn colFecha;
    private DataGridViewTextBoxColumn colMotivo;
    private DataGridViewTextBoxColumn colDiagnostico;
    private DataGridViewTextBoxColumn colPeso;
    private DataGridViewTextBoxColumn colTemperatura;
    private DataGridViewTextBoxColumn colTratamiento;
    private DataGridViewTextBoxColumn colProximoControl;
    private StatusStrip statusStrip;
    private ToolStripStatusLabel lblStatusInfo;
}
