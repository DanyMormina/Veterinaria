namespace Veterinaria.WinForms.Vistas.Veterinario;

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
        btnGenerar = new Button();
        btnImprimir = new Button();
        btnExportarPdf = new Button();
        btnVolver = new Button();
        dgvReporte = new DataGridView();
        colFecha = new DataGridViewTextBoxColumn();
        colHora = new DataGridViewTextBoxColumn();
        colMascota = new DataGridViewTextBoxColumn();
        colPropietario = new DataGridViewTextBoxColumn();
        colMotivo = new DataGridViewTextBoxColumn();
        colDiagnostico = new DataGridViewTextBoxColumn();
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
        // 
        // lblTitulo
        // 
        lblTitulo.Dock = DockStyle.Left;
        lblTitulo.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
        lblTitulo.ForeColor = Color.White;
        lblTitulo.Location = new Point(16, 0);
        lblTitulo.Name = "lblTitulo";
        lblTitulo.Size = new Size(560, 50);
        lblTitulo.Text = "CLÍNICA VETERINARIA — VETERINARIO — REPORTES";
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
        lblUsuarioSesion.Text = "Médico: Veterinario";
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
        // 
        // grpFiltros
        // 
        grpFiltros.Controls.Add(lblFechaDesde);
        grpFiltros.Controls.Add(dtpFechaDesde);
        grpFiltros.Controls.Add(lblFechaHasta);
        grpFiltros.Controls.Add(dtpFechaHasta);
        grpFiltros.Controls.Add(btnGenerar);
        grpFiltros.Controls.Add(btnImprimir);
        grpFiltros.Controls.Add(btnExportarPdf);
        grpFiltros.Controls.Add(btnVolver);
        grpFiltros.Dock = DockStyle.Top;
        grpFiltros.Font = new Font("Segoe UI", 9F);
        grpFiltros.ForeColor = Color.FromArgb(58, 53, 59);
        grpFiltros.Location = new Point(16, 16);
        grpFiltros.Name = "grpFiltros";
        grpFiltros.Size = new Size(1068, 70);
        grpFiltros.TabStop = false;
        grpFiltros.Text = "Mis consultas del periodo";
        // 
        // labels / pickers / buttons
        // 
        lblFechaDesde.Location = new Point(20, 28);
        lblFechaDesde.Size = new Size(50, 23);
        lblFechaDesde.Text = "Desde";
        lblFechaDesde.TextAlign = ContentAlignment.MiddleLeft;
        dtpFechaDesde.Format = DateTimePickerFormat.Short;
        dtpFechaDesde.Location = new Point(74, 28);
        dtpFechaDesde.Size = new Size(120, 23);
        lblFechaHasta.Location = new Point(210, 28);
        lblFechaHasta.Size = new Size(45, 23);
        lblFechaHasta.Text = "Hasta";
        lblFechaHasta.TextAlign = ContentAlignment.MiddleLeft;
        dtpFechaHasta.Format = DateTimePickerFormat.Short;
        dtpFechaHasta.Location = new Point(258, 28);
        dtpFechaHasta.Size = new Size(120, 23);
        btnGenerar.BackColor = Color.FromArgb(152, 196, 164);
        btnGenerar.FlatStyle = FlatStyle.Flat;
        btnGenerar.Location = new Point(420, 26);
        btnGenerar.Size = new Size(100, 28);
        btnGenerar.Text = "Generar";
        btnGenerar.UseVisualStyleBackColor = false;
        btnImprimir.BackColor = Color.FromArgb(200, 138, 150);
        btnImprimir.FlatStyle = FlatStyle.Flat;
        btnImprimir.ForeColor = Color.White;
        btnImprimir.Location = new Point(530, 26);
        btnImprimir.Name = "btnImprimir";
        btnImprimir.Size = new Size(100, 28);
        btnImprimir.Text = "Imprimir";
        btnImprimir.UseVisualStyleBackColor = false;
        btnExportarPdf.BackColor = Color.FromArgb(184, 93, 105);
        btnExportarPdf.FlatStyle = FlatStyle.Flat;
        btnExportarPdf.ForeColor = Color.White;
        btnExportarPdf.Location = new Point(640, 26);
        btnExportarPdf.Name = "btnExportarPdf";
        btnExportarPdf.Size = new Size(110, 28);
        btnExportarPdf.Text = "Exportar PDF";
        btnExportarPdf.UseVisualStyleBackColor = false;
        btnVolver.BackColor = Color.FromArgb(220, 200, 204);
        btnVolver.FlatStyle = FlatStyle.Flat;
        btnVolver.Location = new Point(760, 26);
        btnVolver.Size = new Size(100, 28);
        btnVolver.Text = "Volver";
        btnVolver.UseVisualStyleBackColor = false;
        // 
        // lblTotal
        // 
        lblTotal.Dock = DockStyle.Bottom;
        lblTotal.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
        lblTotal.Location = new Point(16, 560);
        lblTotal.Size = new Size(1068, 24);
        lblTotal.Text = "Total de consultas: 0";
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
        dgvReporte.Columns.AddRange(new DataGridViewColumn[] { colFecha, colHora, colMascota, colPropietario, colMotivo, colDiagnostico });
        dgvReporte.Location = new Point(16, 100);
        dgvReporte.MultiSelect = false;
        dgvReporte.Name = "dgvReporte";
        dgvReporte.ReadOnly = true;
        dgvReporte.RowHeadersVisible = false;
        dgvReporte.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        dgvReporte.Size = new Size(1068, 450);
        colFecha.HeaderText = "Fecha";
        colHora.HeaderText = "Hora";
        colMascota.HeaderText = "Mascota";
        colPropietario.HeaderText = "Propietario";
        colMotivo.HeaderText = "Motivo";
        colDiagnostico.HeaderText = "Diagnóstico";
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
        Text = "CLÍNICA VETERINARIA — VETERINARIO — REPORTES";
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
    private Button btnGenerar;
    private Button btnImprimir;
    private Button btnExportarPdf;
    private Button btnVolver;
    private DataGridView dgvReporte;
    private DataGridViewTextBoxColumn colFecha;
    private DataGridViewTextBoxColumn colHora;
    private DataGridViewTextBoxColumn colMascota;
    private DataGridViewTextBoxColumn colPropietario;
    private DataGridViewTextBoxColumn colMotivo;
    private DataGridViewTextBoxColumn colDiagnostico;
    private Label lblTotal;
}
