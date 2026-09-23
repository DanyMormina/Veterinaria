namespace Veterinaria.WinForms.Vistas.Secretario;

partial class FormCobros
{
    private System.ComponentModel.IContainer components = null;

    protected override void Dispose(bool disposing)
    {
        if (disposing && components != null)
        {
            components.Dispose();
        }
        base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
        DataGridViewCellStyle encabezadoGrilla = new();
        DataGridViewCellStyle celdasGrilla = new();
        pnlEncabezado = new Panel();
        lblTitulo = new Label();
        lblUsuarioSesion = new Label();
        pnlContenido = new Panel();
        pnlBotones = new Panel();
        btnNuevo = new Button();
        btnCobrar = new Button();
        btnCancelar = new Button();
        grpConsulta = new GroupBox();
        lblConsultaClinica = new Label();
        cboConsultaClinica = new ComboBox();
        grpPago = new GroupBox();
        lblFecha = new Label();
        dtpFecha = new DateTimePicker();
        lblMetodoPago = new Label();
        cboMetodoPago = new ComboBox();
        lblImporte = new Label();
        pnlImporte = new Panel();
        lblSimboloPeso = new Label();
        txtImporte = new TextBox();
        lblEstado = new Label();
        cboEstado = new ComboBox();
        pnlListado = new Panel();
        dgvCobros = new DataGridView();
        colId = new DataGridViewTextBoxColumn();
        colConsulta = new DataGridViewTextBoxColumn();
        colMetodoPago = new DataGridViewTextBoxColumn();
        colFecha = new DataGridViewTextBoxColumn();
        colImporte = new DataGridViewTextBoxColumn();
        colEstado = new DataGridViewTextBoxColumn();
        barraEstado = new StatusStrip();
        lblInfoEstado = new ToolStripStatusLabel();
        pnlEncabezado.SuspendLayout();
        pnlContenido.SuspendLayout();
        pnlBotones.SuspendLayout();
        grpConsulta.SuspendLayout();
        grpPago.SuspendLayout();
        pnlListado.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)dgvCobros).BeginInit();
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
        lblTitulo.Text = "CLÍNICA VETERINARIA — COBROS";
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
        pnlContenido.Controls.Add(pnlListado);
        pnlContenido.Controls.Add(pnlBotones);
        pnlContenido.Controls.Add(grpPago);
        pnlContenido.Controls.Add(grpConsulta);
        pnlContenido.Dock = DockStyle.Fill;
        pnlContenido.Location = new Point(0, 50);
        pnlContenido.Name = "pnlContenido";
        pnlContenido.Size = new Size(1100, 628);
        pnlContenido.TabIndex = 1;
        // 
        // grpConsulta
        // 
        grpConsulta.Controls.Add(lblConsultaClinica);
        grpConsulta.Controls.Add(cboConsultaClinica);
        grpConsulta.Font = new Font("Segoe UI", 9F);
        grpConsulta.ForeColor = Color.FromArgb(58, 53, 59);
        grpConsulta.Location = new Point(16, 16);
        grpConsulta.Name = "grpConsulta";
        grpConsulta.Size = new Size(1072, 86);
        grpConsulta.TabIndex = 0;
        grpConsulta.TabStop = false;
        grpConsulta.Text = "Datos de la consulta";
        // 
        // lblConsultaClinica
        // 
        lblConsultaClinica.Font = new Font("Segoe UI", 9F);
        lblConsultaClinica.ForeColor = Color.FromArgb(58, 53, 59);
        lblConsultaClinica.Location = new Point(28, 24);
        lblConsultaClinica.Name = "lblConsultaClinica";
        lblConsultaClinica.Size = new Size(1016, 18);
        lblConsultaClinica.TabIndex = 0;
        lblConsultaClinica.Text = "Consulta clínica:";
        // 
        // cboConsultaClinica
        // 
        cboConsultaClinica.BackColor = Color.White;
        cboConsultaClinica.DropDownStyle = ComboBoxStyle.DropDownList;
        cboConsultaClinica.Font = new Font("Segoe UI", 9F);
        cboConsultaClinica.ForeColor = Color.FromArgb(58, 53, 59);
        cboConsultaClinica.FormattingEnabled = true;
        cboConsultaClinica.Location = new Point(28, 44);
        cboConsultaClinica.Name = "cboConsultaClinica";
        cboConsultaClinica.Size = new Size(1016, 23);
        cboConsultaClinica.TabIndex = 1;
        // 
        // grpPago
        // 
        grpPago.Controls.Add(lblFecha);
        grpPago.Controls.Add(dtpFecha);
        grpPago.Controls.Add(lblMetodoPago);
        grpPago.Controls.Add(cboMetodoPago);
        grpPago.Controls.Add(lblImporte);
        grpPago.Controls.Add(lblEstado);
        grpPago.Controls.Add(cboEstado);
        grpPago.Font = new Font("Segoe UI", 9F);
        grpPago.ForeColor = Color.FromArgb(58, 53, 59);
        grpPago.Controls.Add(pnlImporte);
        grpPago.Location = new Point(16, 108);
        grpPago.Name = "grpPago";
        grpPago.Size = new Size(1072, 110);
        grpPago.TabIndex = 1;
        grpPago.TabStop = false;
        grpPago.Text = "Datos del pago";
        // 
        // lblFecha
        // 
        lblFecha.Location = new Point(28, 32);
        lblFecha.Name = "lblFecha";
        lblFecha.Size = new Size(118, 23);
        lblFecha.TabIndex = 0;
        lblFecha.Text = "Fecha";
        lblFecha.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // dtpFecha
        // 
        dtpFecha.Format = DateTimePickerFormat.Short;
        dtpFecha.Location = new Point(152, 32);
        dtpFecha.Name = "dtpFecha";
        dtpFecha.Size = new Size(200, 23);
        dtpFecha.TabIndex = 1;
        // 
        // lblMetodoPago
        // 
        lblMetodoPago.Location = new Point(380, 32);
        lblMetodoPago.Name = "lblMetodoPago";
        lblMetodoPago.Size = new Size(118, 23);
        lblMetodoPago.TabIndex = 2;
        lblMetodoPago.Text = "Método de pago";
        lblMetodoPago.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // cboMetodoPago
        // 
        cboMetodoPago.BackColor = Color.White;
        cboMetodoPago.DropDownStyle = ComboBoxStyle.DropDownList;
        cboMetodoPago.FlatStyle = FlatStyle.Flat;
        cboMetodoPago.Location = new Point(504, 32);
        cboMetodoPago.Name = "cboMetodoPago";
        cboMetodoPago.Size = new Size(280, 23);
        cboMetodoPago.TabIndex = 3;
        // 
        // lblImporte
        // 
        lblImporte.Location = new Point(28, 68);
        lblImporte.Name = "lblImporte";
        lblImporte.Size = new Size(118, 23);
        lblImporte.TabIndex = 4;
        lblImporte.Text = "Importe";
        lblImporte.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // pnlImporte
        // 
        pnlImporte.BackColor = Color.White;
        pnlImporte.BorderStyle = BorderStyle.FixedSingle;
        pnlImporte.Controls.Add(lblSimboloPeso);
        pnlImporte.Controls.Add(txtImporte);
        pnlImporte.Location = new Point(152, 68);
        pnlImporte.Name = "pnlImporte";
        pnlImporte.Size = new Size(200, 23);
        pnlImporte.TabIndex = 5;
        // 
        // lblSimboloPeso
        // 
        lblSimboloPeso.AutoSize = true;
        lblSimboloPeso.Font = new Font("Segoe UI", 9F);
        lblSimboloPeso.ForeColor = Color.FromArgb(58, 53, 59);
        lblSimboloPeso.Location = new Point(6, 4);
        lblSimboloPeso.Name = "lblSimboloPeso";
        lblSimboloPeso.Size = new Size(13, 15);
        lblSimboloPeso.TabIndex = 0;
        lblSimboloPeso.Text = "$";
        // 
        // txtImporte
        // 
        txtImporte.BackColor = Color.White;
        txtImporte.BorderStyle = BorderStyle.None;
        txtImporte.Location = new Point(22, 4);
        txtImporte.Name = "txtImporte";
        txtImporte.Size = new Size(172, 16);
        txtImporte.TabIndex = 1;
        // 
        // lblEstado
        // 
        lblEstado.Location = new Point(380, 68);
        lblEstado.Name = "lblEstado";
        lblEstado.Size = new Size(118, 23);
        lblEstado.TabIndex = 6;
        lblEstado.Text = "Estado";
        lblEstado.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // cboEstado
        // 
        cboEstado.BackColor = Color.White;
        cboEstado.DropDownStyle = ComboBoxStyle.DropDownList;
        cboEstado.FlatStyle = FlatStyle.Flat;
        cboEstado.Location = new Point(504, 68);
        cboEstado.Name = "cboEstado";
        cboEstado.Size = new Size(200, 23);
        cboEstado.TabIndex = 7;
        // 
        // pnlBotones
        // 
        pnlBotones.Controls.Add(btnNuevo);
        pnlBotones.Controls.Add(btnCobrar);
        pnlBotones.Controls.Add(btnCancelar);
        pnlBotones.Location = new Point(12, 228);
        pnlBotones.Name = "pnlBotones";
        pnlBotones.Size = new Size(1076, 40);
        pnlBotones.TabIndex = 2;
        // 
        // btnNuevo
        // 
        btnNuevo.BackColor = Color.FromArgb(148, 176, 214);
        btnNuevo.FlatAppearance.BorderColor = Color.FromArgb(112, 142, 182);
        btnNuevo.FlatStyle = FlatStyle.Flat;
        btnNuevo.Font = new Font("Segoe UI", 9F);
        btnNuevo.ForeColor = Color.Black;
        btnNuevo.Location = new Point(11, 5);
        btnNuevo.Name = "btnNuevo";
        btnNuevo.Size = new Size(100, 32);
        btnNuevo.TabIndex = 0;
        btnNuevo.Text = "Nuevo";
        btnNuevo.UseVisualStyleBackColor = false;
        // 
        // btnCobrar
        // 
        btnCobrar.BackColor = Color.FromArgb(152, 196, 164);
        btnCobrar.FlatAppearance.BorderColor = Color.FromArgb(112, 158, 124);
        btnCobrar.FlatStyle = FlatStyle.Flat;
        btnCobrar.Font = new Font("Segoe UI", 9F);
        btnCobrar.ForeColor = Color.Black;
        btnCobrar.Location = new Point(119, 5);
        btnCobrar.Name = "btnCobrar";
        btnCobrar.Size = new Size(100, 32);
        btnCobrar.TabIndex = 1;
        btnCobrar.Text = "Cobrar";
        btnCobrar.UseVisualStyleBackColor = false;
        // 
        // btnCancelar
        // 
        btnCancelar.BackColor = Color.MistyRose;
        btnCancelar.FlatAppearance.BorderColor = Color.FromArgb(184, 93, 105);
        btnCancelar.FlatStyle = FlatStyle.Flat;
        btnCancelar.Font = new Font("Segoe UI", 9F);
        btnCancelar.ForeColor = Color.Black;
        btnCancelar.Location = new Point(227, 5);
        btnCancelar.Name = "btnCancelar";
        btnCancelar.Size = new Size(100, 32);
        btnCancelar.TabIndex = 2;
        btnCancelar.Text = "Cancelar";
        btnCancelar.UseVisualStyleBackColor = false;
        btnCancelar.Click += btnCancelar_Click;
        // 
        // pnlListado
        // 
        pnlListado.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        pnlListado.BackColor = Color.White;
        pnlListado.BorderStyle = BorderStyle.FixedSingle;
        pnlListado.Controls.Add(dgvCobros);
        pnlListado.Location = new Point(12, 274);
        pnlListado.Name = "pnlListado";
        pnlListado.Size = new Size(1076, 351);
        pnlListado.TabIndex = 3;
        // 
        // dgvCobros
        // 
        dgvCobros.AllowUserToAddRows = false;
        dgvCobros.AllowUserToDeleteRows = false;
        dgvCobros.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        dgvCobros.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        dgvCobros.BackgroundColor = Color.White;
        encabezadoGrilla.Alignment = DataGridViewContentAlignment.MiddleLeft;
        encabezadoGrilla.BackColor = SystemColors.Control;
        encabezadoGrilla.Font = new Font("Segoe UI", 9F);
        encabezadoGrilla.ForeColor = SystemColors.WindowText;
        encabezadoGrilla.SelectionBackColor = SystemColors.Highlight;
        encabezadoGrilla.SelectionForeColor = SystemColors.HighlightText;
        encabezadoGrilla.WrapMode = DataGridViewTriState.True;
        dgvCobros.ColumnHeadersDefaultCellStyle = encabezadoGrilla;
        dgvCobros.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
        dgvCobros.Columns.AddRange(new DataGridViewColumn[] { colId, colConsulta, colMetodoPago, colFecha, colImporte, colEstado });
        celdasGrilla.Alignment = DataGridViewContentAlignment.MiddleLeft;
        celdasGrilla.BackColor = Color.White;
        celdasGrilla.Font = new Font("Segoe UI", 9F);
        celdasGrilla.ForeColor = Color.FromArgb(58, 53, 59);
        celdasGrilla.SelectionBackColor = Color.FromArgb(232, 220, 224);
        celdasGrilla.SelectionForeColor = Color.FromArgb(58, 53, 59);
        celdasGrilla.WrapMode = DataGridViewTriState.False;
        dgvCobros.DefaultCellStyle = celdasGrilla;
        dgvCobros.EnableHeadersVisualStyles = false;
        dgvCobros.Location = new Point(10, 10);
        dgvCobros.MultiSelect = false;
        dgvCobros.Name = "dgvCobros";
        dgvCobros.ReadOnly = true;
        dgvCobros.RowHeadersVisible = false;
        dgvCobros.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        dgvCobros.Size = new Size(1054, 329);
        dgvCobros.TabIndex = 0;
        // 
        // colId
        // 
        colId.FillWeight = 10F;
        colId.HeaderText = "ID";
        colId.Name = "colId";
        colId.ReadOnly = true;
        // 
        // colConsulta
        // 
        colConsulta.FillWeight = 15F;
        colConsulta.HeaderText = "Consulta";
        colConsulta.Name = "colConsulta";
        colConsulta.ReadOnly = true;
        // 
        // colMetodoPago
        // 
        colMetodoPago.FillWeight = 25F;
        colMetodoPago.HeaderText = "Método de pago";
        colMetodoPago.Name = "colMetodoPago";
        colMetodoPago.ReadOnly = true;
        // 
        // colFecha
        // 
        colFecha.FillWeight = 15F;
        colFecha.HeaderText = "Fecha";
        colFecha.Name = "colFecha";
        colFecha.ReadOnly = true;
        // 
        // colImporte
        // 
        colImporte.FillWeight = 15F;
        colImporte.HeaderText = "Importe";
        colImporte.Name = "colImporte";
        colImporte.ReadOnly = true;
        // 
        // colEstado
        // 
        colEstado.FillWeight = 20F;
        colEstado.HeaderText = "Estado";
        colEstado.Name = "colEstado";
        colEstado.ReadOnly = true;
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
        lblInfoEstado.Size = new Size(120, 17);
        lblInfoEstado.Text = "Módulo de cobros listo";
        // 
        // FormCobros
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
        MinimumSize = new Size(800, 500);
        Name = "FormCobros";
        StartPosition = FormStartPosition.CenterScreen;
        Text = "Veterinaria - Cobros";
        Load += FormCobros_Load;
        pnlEncabezado.ResumeLayout(false);
        pnlContenido.ResumeLayout(false);
        pnlBotones.ResumeLayout(false);
        grpConsulta.ResumeLayout(false);
        grpPago.ResumeLayout(false);
        pnlImporte.ResumeLayout(false);
        pnlImporte.PerformLayout();
        pnlListado.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)dgvCobros).EndInit();
        barraEstado.ResumeLayout(false);
        barraEstado.PerformLayout();
        ResumeLayout(false);
        PerformLayout();
    }

    private Panel pnlEncabezado;
    private Label lblTitulo;
    private Label lblUsuarioSesion;
    private Panel pnlContenido;
    private GroupBox grpConsulta;
    private Label lblConsultaClinica;
    private ComboBox cboConsultaClinica;
    private GroupBox grpPago;
    private Label lblFecha;
    private DateTimePicker dtpFecha;
    private Label lblMetodoPago;
    private ComboBox cboMetodoPago;
    private Label lblImporte;
    private Panel pnlImporte;
    private Label lblSimboloPeso;
    private TextBox txtImporte;
    private Label lblEstado;
    private ComboBox cboEstado;
    private Panel pnlBotones;
    private Button btnNuevo;
    private Button btnCobrar;
    private Button btnCancelar;
    private Panel pnlListado;
    private DataGridView dgvCobros;
    private DataGridViewTextBoxColumn colId;
    private DataGridViewTextBoxColumn colConsulta;
    private DataGridViewTextBoxColumn colMetodoPago;
    private DataGridViewTextBoxColumn colFecha;
    private DataGridViewTextBoxColumn colImporte;
    private DataGridViewTextBoxColumn colEstado;
    private StatusStrip barraEstado;
    private ToolStripStatusLabel lblInfoEstado;
}
