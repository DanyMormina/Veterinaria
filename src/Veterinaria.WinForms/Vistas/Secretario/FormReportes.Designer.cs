namespace Veterinaria.WinForms.Vistas.Secretario;

partial class FormReportes
{
    private System.ComponentModel.IContainer components = null;

    protected override void Dispose(bool disposing)
    {
        if (disposing && components != null)
            components.Dispose();
        base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
        pnlEncabezado = new Panel();
        lblTitulo = new Label();
        lblUsuarioSesion = new Label();
        pnlContenido = new Panel();
        grpFiltros = new GroupBox();
        lblFechaDesde = new Label();
        dtpFechaDesde = new DateTimePicker();
        lblFechaHasta = new Label();
        dtpFechaHasta = new DateTimePicker();
        lblEstado = new Label();
        cboEstado = new ComboBox();
        btnHoy = new Button();
        btnSemana = new Button();
        btnMes = new Button();
        btnGenerar = new Button();
        btnImprimir = new Button();
        btnExportarPdf = new Button();
        btnVolver = new Button();
        dgvReporte = new DataGridView();
        colFecha = new DataGridViewTextBoxColumn();
        colConsulta = new DataGridViewTextBoxColumn();
        colMascota = new DataGridViewTextBoxColumn();
        colPropietario = new DataGridViewTextBoxColumn();
        colMetodo = new DataGridViewTextBoxColumn();
        colImporte = new DataGridViewTextBoxColumn();
        colEstado = new DataGridViewTextBoxColumn();
        lblTotal = new Label();
        pnlEncabezado.SuspendLayout();
        pnlContenido.SuspendLayout();
        grpFiltros.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)dgvReporte).BeginInit();
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
        lblTitulo.Size = new Size(560, 50);
        lblTitulo.TabIndex = 0;
        lblTitulo.Text = "CLÍNICA VETERINARIA — RECEPCIÓN — REPORTES";
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
        pnlContenido.Controls.Add(dgvReporte);
        pnlContenido.Controls.Add(lblTotal);
        pnlContenido.Controls.Add(grpFiltros);
        pnlContenido.Dock = DockStyle.Fill;
        pnlContenido.Location = new Point(0, 50);
        pnlContenido.Name = "pnlContenido";
        pnlContenido.Padding = new Padding(16);
        pnlContenido.Size = new Size(1100, 600);
        pnlContenido.TabIndex = 1;
        // 
        // grpFiltros
        // 
        grpFiltros.Controls.Add(lblFechaDesde);
        grpFiltros.Controls.Add(dtpFechaDesde);
        grpFiltros.Controls.Add(lblFechaHasta);
        grpFiltros.Controls.Add(dtpFechaHasta);
        grpFiltros.Controls.Add(lblEstado);
        grpFiltros.Controls.Add(cboEstado);
        grpFiltros.Controls.Add(btnHoy);
        grpFiltros.Controls.Add(btnSemana);
        grpFiltros.Controls.Add(btnMes);
        grpFiltros.Controls.Add(btnGenerar);
        grpFiltros.Controls.Add(btnImprimir);
        grpFiltros.Controls.Add(btnExportarPdf);
        grpFiltros.Controls.Add(btnVolver);
        grpFiltros.Dock = DockStyle.Top;
        grpFiltros.Font = new Font("Segoe UI", 9F);
        grpFiltros.ForeColor = Color.FromArgb(58, 53, 59);
        grpFiltros.Location = new Point(16, 16);
        grpFiltros.Name = "grpFiltros";
        grpFiltros.Size = new Size(1068, 100);
        grpFiltros.TabIndex = 0;
        grpFiltros.TabStop = false;
        grpFiltros.Text = "Cobros del periodo";
        // 
        // lblFechaDesde
        // 
        lblFechaDesde.Location = new Point(20, 28);
        lblFechaDesde.Name = "lblFechaDesde";
        lblFechaDesde.Size = new Size(50, 23);
        lblFechaDesde.Text = "Desde";
        lblFechaDesde.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // dtpFechaDesde
        // 
        dtpFechaDesde.Format = DateTimePickerFormat.Short;
        dtpFechaDesde.Location = new Point(74, 28);
        dtpFechaDesde.Name = "dtpFechaDesde";
        dtpFechaDesde.Size = new Size(120, 23);
        // 
        // lblFechaHasta
        // 
        lblFechaHasta.Location = new Point(210, 28);
        lblFechaHasta.Name = "lblFechaHasta";
        lblFechaHasta.Size = new Size(45, 23);
        lblFechaHasta.Text = "Hasta";
        lblFechaHasta.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // dtpFechaHasta
        // 
        dtpFechaHasta.Format = DateTimePickerFormat.Short;
        dtpFechaHasta.Location = new Point(258, 28);
        dtpFechaHasta.Name = "dtpFechaHasta";
        dtpFechaHasta.Size = new Size(120, 23);
        // 
        // lblEstado
        // 
        lblEstado.Location = new Point(400, 28);
        lblEstado.Name = "lblEstado";
        lblEstado.Size = new Size(50, 23);
        lblEstado.Text = "Estado";
        lblEstado.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // cboEstado
        // 
        cboEstado.DropDownStyle = ComboBoxStyle.DropDownList;
        cboEstado.Location = new Point(454, 28);
        cboEstado.Name = "cboEstado";
        cboEstado.Size = new Size(140, 23);
        // 
        // btnHoy
        // 
        btnHoy.BackColor = Color.FromArgb(220, 200, 204);
        btnHoy.FlatStyle = FlatStyle.Flat;
        btnHoy.Location = new Point(20, 62);
        btnHoy.Name = "btnHoy";
        btnHoy.Size = new Size(80, 28);
        btnHoy.Text = "Hoy";
        btnHoy.UseVisualStyleBackColor = false;
        // 
        // btnSemana
        // 
        btnSemana.BackColor = Color.FromArgb(220, 200, 204);
        btnSemana.FlatStyle = FlatStyle.Flat;
        btnSemana.Location = new Point(106, 62);
        btnSemana.Name = "btnSemana";
        btnSemana.Size = new Size(90, 28);
        btnSemana.Text = "Esta semana";
        btnSemana.UseVisualStyleBackColor = false;
        // 
        // btnMes
        // 
        btnMes.BackColor = Color.FromArgb(220, 200, 204);
        btnMes.FlatStyle = FlatStyle.Flat;
        btnMes.Location = new Point(202, 62);
        btnMes.Name = "btnMes";
        btnMes.Size = new Size(80, 28);
        btnMes.Text = "Este mes";
        btnMes.UseVisualStyleBackColor = false;
        // 
        // btnGenerar
        // 
        btnGenerar.BackColor = Color.FromArgb(152, 196, 164);
        btnGenerar.FlatStyle = FlatStyle.Flat;
        btnGenerar.Location = new Point(620, 28);
        btnGenerar.Name = "btnGenerar";
        btnGenerar.Size = new Size(90, 28);
        btnGenerar.Text = "Generar";
        btnGenerar.UseVisualStyleBackColor = false;
        // 
        // btnImprimir
        // 
        btnImprimir.BackColor = Color.FromArgb(200, 138, 150);
        btnImprimir.FlatStyle = FlatStyle.Flat;
        btnImprimir.ForeColor = Color.White;
        btnImprimir.Location = new Point(718, 28);
        btnImprimir.Name = "btnImprimir";
        btnImprimir.Size = new Size(90, 28);
        btnImprimir.Text = "Imprimir";
        btnImprimir.UseVisualStyleBackColor = false;
        // 
        // btnExportarPdf
        // 
        btnExportarPdf.BackColor = Color.FromArgb(184, 93, 105);
        btnExportarPdf.FlatStyle = FlatStyle.Flat;
        btnExportarPdf.ForeColor = Color.White;
        btnExportarPdf.Location = new Point(816, 28);
        btnExportarPdf.Name = "btnExportarPdf";
        btnExportarPdf.Size = new Size(110, 28);
        btnExportarPdf.Text = "Exportar PDF";
        btnExportarPdf.UseVisualStyleBackColor = false;
        // 
        // btnVolver
        // 
        btnVolver.BackColor = Color.FromArgb(220, 200, 204);
        btnVolver.FlatStyle = FlatStyle.Flat;
        btnVolver.Location = new Point(934, 28);
        btnVolver.Name = "btnVolver";
        btnVolver.Size = new Size(90, 28);
        btnVolver.Text = "Volver";
        btnVolver.UseVisualStyleBackColor = false;
        // 
        // lblTotal
        // 
        lblTotal.Dock = DockStyle.Bottom;
        lblTotal.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
        lblTotal.Location = new Point(16, 560);
        lblTotal.Name = "lblTotal";
        lblTotal.Size = new Size(1068, 24);
        lblTotal.Text = "Total: 0 registros | $ 0,00";
        lblTotal.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // dgvReporte
        // 
        dgvReporte.AllowUserToAddRows = false;
        dgvReporte.AllowUserToDeleteRows = false;
        dgvReporte.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        dgvReporte.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        dgvReporte.BackgroundColor = Color.White;
        dgvReporte.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
        dgvReporte.Columns.AddRange(new DataGridViewColumn[] { colFecha, colConsulta, colMascota, colPropietario, colMetodo, colImporte, colEstado });
        dgvReporte.Location = new Point(16, 130);
        dgvReporte.MultiSelect = false;
        dgvReporte.Name = "dgvReporte";
        dgvReporte.ReadOnly = true;
        dgvReporte.RowHeadersVisible = false;
        dgvReporte.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        dgvReporte.Size = new Size(1068, 420);
        dgvReporte.TabIndex = 1;
        // 
        // columns
        // 
        colFecha.HeaderText = "Fecha";
        colFecha.Name = "colFecha";
        colConsulta.HeaderText = "Consulta";
        colConsulta.Name = "colConsulta";
        colMascota.HeaderText = "Mascota";
        colMascota.Name = "colMascota";
        colPropietario.HeaderText = "Propietario";
        colPropietario.Name = "colPropietario";
        colMetodo.HeaderText = "Método";
        colMetodo.Name = "colMetodo";
        colImporte.HeaderText = "Importe";
        colImporte.Name = "colImporte";
        colEstado.HeaderText = "Estado";
        colEstado.Name = "colEstado";
        // 
        // FormReportes
        // 
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(1100, 650);
        Controls.Add(pnlContenido);
        Controls.Add(pnlEncabezado);
        Name = "FormReportes";
        StartPosition = FormStartPosition.CenterParent;
        Text = "CLÍNICA VETERINARIA — RECEPCIÓN — REPORTES";
        Load += FormReportes_Load;
        pnlEncabezado.ResumeLayout(false);
        pnlContenido.ResumeLayout(false);
        grpFiltros.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)dgvReporte).EndInit();
        ResumeLayout(false);
    }

    private Panel pnlEncabezado;
    private Label lblTitulo;
    private Label lblUsuarioSesion;
    private Panel pnlContenido;
    private GroupBox grpFiltros;
    private Label lblFechaDesde;
    private DateTimePicker dtpFechaDesde;
    private Label lblFechaHasta;
    private DateTimePicker dtpFechaHasta;
    private Label lblEstado;
    private ComboBox cboEstado;
    private Button btnHoy;
    private Button btnSemana;
    private Button btnMes;
    private Button btnGenerar;
    private Button btnImprimir;
    private Button btnExportarPdf;
    private Button btnVolver;
    private DataGridView dgvReporte;
    private DataGridViewTextBoxColumn colFecha;
    private DataGridViewTextBoxColumn colConsulta;
    private DataGridViewTextBoxColumn colMascota;
    private DataGridViewTextBoxColumn colPropietario;
    private DataGridViewTextBoxColumn colMetodo;
    private DataGridViewTextBoxColumn colImporte;
    private DataGridViewTextBoxColumn colEstado;
    private Label lblTotal;
}
