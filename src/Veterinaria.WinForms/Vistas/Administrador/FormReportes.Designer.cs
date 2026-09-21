namespace Veterinaria.WinForms.Vistas.Administrador;

partial class FormReportes
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
        dgvReporte = new DataGridView();
        pnlTotales = new Panel();
        lblTotalRegistros = new Label();
        lblTotalFacturado = new Label();
        pnlAcciones = new Panel();
        btnGenerar = new Button();
        btnImprimir = new Button();
        btnExportarPdf = new Button();
        btnVolver = new Button();
        grpFiltros = new GroupBox();
        lblTipoReporte = new Label();
        cboTipoReporte = new ComboBox();
        pnlContenedorFechas = new Panel();
        lblFechaDesde = new Label();
        dtpFechaDesde = new DateTimePicker();
        lblFechaHasta = new Label();
        dtpFechaHasta = new DateTimePicker();
        lblVeterinario = new Label();
        cboVeterinario = new ComboBox();
        lblEspecie = new Label();
        cboEspecie = new ComboBox();
        pnlEncabezado.SuspendLayout();
        pnlContenido.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)dgvReporte).BeginInit();
        pnlTotales.SuspendLayout();
        pnlAcciones.SuspendLayout();
        grpFiltros.SuspendLayout();
        pnlContenedorFechas.SuspendLayout();
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
        lblTitulo.Size = new Size(680, 48);
        lblTitulo.TabIndex = 0;
        lblTitulo.Text = "CLÍNICA VETERINARIA — ADMINISTRADOR — REPORTES";
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
        pnlContenido.Controls.Add(dgvReporte);
        pnlContenido.Controls.Add(pnlTotales);
        pnlContenido.Controls.Add(pnlAcciones);
        pnlContenido.Controls.Add(grpFiltros);
        pnlContenido.Dock = DockStyle.Fill;
        pnlContenido.Location = new Point(0, 48);
        pnlContenido.Name = "pnlContenido";
        pnlContenido.Size = new Size(1220, 672);
        pnlContenido.TabIndex = 1;
        // 
        // dgvReporte
        // 
        dgvReporte.AllowUserToAddRows = false;
        dgvReporte.AllowUserToDeleteRows = false;
        dgvReporte.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        dgvReporte.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        dgvReporte.BackgroundColor = Color.White;
        dgvReporte.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
        dgvReporte.Location = new Point(16, 190);
        dgvReporte.Name = "dgvReporte";
        dgvReporte.ReadOnly = true;
        dgvReporte.RowHeadersVisible = false;
        dgvReporte.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        dgvReporte.Size = new Size(1188, 416);
        dgvReporte.TabIndex = 0;
        // 
        // pnlTotales
        // 
        pnlTotales.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        pnlTotales.BorderStyle = BorderStyle.FixedSingle;
        pnlTotales.Controls.Add(lblTotalRegistros);
        pnlTotales.Controls.Add(lblTotalFacturado);
        pnlTotales.Location = new Point(16, 614);
        pnlTotales.Name = "pnlTotales";
        pnlTotales.Size = new Size(1188, 40);
        pnlTotales.TabIndex = 3;
        // 
        // lblTotalRegistros
        // 
        lblTotalRegistros.Font = new Font("Segoe UI", 9.75F);
        lblTotalRegistros.Location = new Point(12, 8);
        lblTotalRegistros.Name = "lblTotalRegistros";
        lblTotalRegistros.Size = new Size(400, 23);
        lblTotalRegistros.TabIndex = 0;
        lblTotalRegistros.Text = "Total de registros:";
        lblTotalRegistros.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // lblTotalFacturado
        // 
        lblTotalFacturado.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        lblTotalFacturado.Font = new Font("Segoe UI", 9.75F);
        lblTotalFacturado.Location = new Point(776, 8);
        lblTotalFacturado.Name = "lblTotalFacturado";
        lblTotalFacturado.Size = new Size(400, 23);
        lblTotalFacturado.TabIndex = 1;
        lblTotalFacturado.Text = "Total facturado:";
        lblTotalFacturado.TextAlign = ContentAlignment.MiddleRight;
        // 
        // pnlAcciones
        // 
        pnlAcciones.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
        pnlAcciones.Controls.Add(btnGenerar);
        pnlAcciones.Controls.Add(btnImprimir);
        pnlAcciones.Controls.Add(btnExportarPdf);
        pnlAcciones.Controls.Add(btnVolver);
        pnlAcciones.Location = new Point(16, 142);
        pnlAcciones.Name = "pnlAcciones";
        pnlAcciones.Size = new Size(1188, 40);
        pnlAcciones.TabIndex = 1;
        // 
        // btnGenerar
        // 
        btnGenerar.BackColor = Color.FromArgb(152, 196, 164);
        btnGenerar.Cursor = Cursors.Hand;
        btnGenerar.FlatAppearance.BorderSize = 0;
        btnGenerar.FlatStyle = FlatStyle.Flat;
        btnGenerar.Font = new Font("Segoe UI", 9F);
        btnGenerar.ForeColor = Color.Black;
        btnGenerar.Location = new Point(16, 5);
        btnGenerar.Name = "btnGenerar";
        btnGenerar.Size = new Size(120, 32);
        btnGenerar.TabIndex = 0;
        btnGenerar.Text = "Generar";
        btnGenerar.UseVisualStyleBackColor = false;
        btnGenerar.Click += btnGenerar_Click;
        // 
        // btnImprimir
        // 
        btnImprimir.BackColor = Color.Thistle;
        btnImprimir.Cursor = Cursors.Hand;
        btnImprimir.FlatAppearance.BorderSize = 0;
        btnImprimir.FlatStyle = FlatStyle.Flat;
        btnImprimir.Font = new Font("Segoe UI", 9F);
        btnImprimir.ForeColor = Color.Black;
        btnImprimir.Location = new Point(144, 5);
        btnImprimir.Name = "btnImprimir";
        btnImprimir.Size = new Size(120, 32);
        btnImprimir.TabIndex = 1;
        btnImprimir.Text = "Imprimir";
        btnImprimir.UseVisualStyleBackColor = false;
        btnImprimir.Click += btnImprimir_Click;
        // 
        // btnExportarPdf
        // 
        btnExportarPdf.BackColor = Color.FromArgb(226, 217, 220);
        btnExportarPdf.Cursor = Cursors.Hand;
        btnExportarPdf.FlatAppearance.BorderSize = 0;
        btnExportarPdf.FlatStyle = FlatStyle.Flat;
        btnExportarPdf.Font = new Font("Segoe UI", 9F);
        btnExportarPdf.ForeColor = Color.FromArgb(58, 53, 59);
        btnExportarPdf.Location = new Point(272, 5);
        btnExportarPdf.Name = "btnExportarPdf";
        btnExportarPdf.Size = new Size(120, 32);
        btnExportarPdf.TabIndex = 2;
        btnExportarPdf.Text = "Exportar PDF";
        btnExportarPdf.UseVisualStyleBackColor = false;
        btnExportarPdf.Click += btnExportarPdf_Click;
        // 
        // btnVolver
        // 
        btnVolver.BackColor = Color.FromArgb(220, 200, 204);
        btnVolver.Cursor = Cursors.Hand;
        btnVolver.FlatAppearance.BorderSize = 0;
        btnVolver.FlatStyle = FlatStyle.Flat;
        btnVolver.Font = new Font("Segoe UI", 9F);
        btnVolver.ForeColor = Color.Black;
        btnVolver.Location = new Point(400, 5);
        btnVolver.Name = "btnVolver";
        btnVolver.Size = new Size(140, 32);
        btnVolver.TabIndex = 3;
        btnVolver.Text = "Volver al panel";
        btnVolver.UseVisualStyleBackColor = false;
        btnVolver.Click += btnVolver_Click;
        // 
        // grpFiltros
        // 
        grpFiltros.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
        grpFiltros.Controls.Add(lblTipoReporte);
        grpFiltros.Controls.Add(cboTipoReporte);
        grpFiltros.Controls.Add(pnlContenedorFechas);
        grpFiltros.Controls.Add(lblVeterinario);
        grpFiltros.Controls.Add(cboVeterinario);
        grpFiltros.Controls.Add(lblEspecie);
        grpFiltros.Controls.Add(cboEspecie);
        grpFiltros.Font = new Font("Segoe UI", 9F);
        grpFiltros.ForeColor = Color.FromArgb(58, 53, 59);
        grpFiltros.Location = new Point(16, 16);
        grpFiltros.Name = "grpFiltros";
        grpFiltros.Size = new Size(1188, 118);
        grpFiltros.TabIndex = 0;
        grpFiltros.TabStop = false;
        grpFiltros.Text = "Filtros del reporte";
        // 
        // lblTipoReporte
        // 
        lblTipoReporte.Location = new Point(16, 32);
        lblTipoReporte.Name = "lblTipoReporte";
        lblTipoReporte.Size = new Size(110, 23);
        lblTipoReporte.TabIndex = 0;
        lblTipoReporte.Text = "Tipo de reporte";
        lblTipoReporte.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // cboTipoReporte
        // 
        cboTipoReporte.BackColor = Color.White;
        cboTipoReporte.DropDownStyle = ComboBoxStyle.DropDownList;
        cboTipoReporte.FormattingEnabled = true;
        cboTipoReporte.Location = new Point(132, 32);
        cboTipoReporte.Name = "cboTipoReporte";
        cboTipoReporte.Size = new Size(200, 23);
        cboTipoReporte.TabIndex = 1;
        cboTipoReporte.SelectedIndexChanged += cboTipoReporte_SelectedIndexChanged;
        // 
        // pnlContenedorFechas
        // 
        pnlContenedorFechas.BackColor = Color.FromArgb(250, 244, 244);
        pnlContenedorFechas.Controls.Add(lblFechaDesde);
        pnlContenedorFechas.Controls.Add(dtpFechaDesde);
        pnlContenedorFechas.Controls.Add(lblFechaHasta);
        pnlContenedorFechas.Controls.Add(dtpFechaHasta);
        pnlContenedorFechas.Location = new Point(348, 28);
        pnlContenedorFechas.Name = "pnlContenedorFechas";
        pnlContenedorFechas.Padding = new Padding(1);
        pnlContenedorFechas.Size = new Size(488, 31);
        pnlContenedorFechas.TabIndex = 2;
        // 
        // lblFechaDesde
        // 
        lblFechaDesde.Location = new Point(4, 4);
        lblFechaDesde.Name = "lblFechaDesde";
        lblFechaDesde.Size = new Size(86, 23);
        lblFechaDesde.TabIndex = 0;
        lblFechaDesde.Text = "Fecha desde";
        lblFechaDesde.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // dtpFechaDesde
        // 
        dtpFechaDesde.Format = DateTimePickerFormat.Short;
        dtpFechaDesde.Location = new Point(94, 4);
        dtpFechaDesde.Name = "dtpFechaDesde";
        dtpFechaDesde.Size = new Size(140, 23);
        dtpFechaDesde.TabIndex = 1;
        // 
        // lblFechaHasta
        // 
        lblFechaHasta.Location = new Point(252, 4);
        lblFechaHasta.Name = "lblFechaHasta";
        lblFechaHasta.Size = new Size(80, 23);
        lblFechaHasta.TabIndex = 2;
        lblFechaHasta.Text = "Fecha hasta";
        lblFechaHasta.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // dtpFechaHasta
        // 
        dtpFechaHasta.Format = DateTimePickerFormat.Short;
        dtpFechaHasta.Location = new Point(338, 4);
        dtpFechaHasta.Name = "dtpFechaHasta";
        dtpFechaHasta.Size = new Size(140, 23);
        dtpFechaHasta.TabIndex = 3;
        // 
        // lblVeterinario
        // 
        lblVeterinario.Location = new Point(16, 74);
        lblVeterinario.Name = "lblVeterinario";
        lblVeterinario.Size = new Size(110, 23);
        lblVeterinario.TabIndex = 3;
        lblVeterinario.Text = "Veterinario";
        lblVeterinario.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // cboVeterinario
        // 
        cboVeterinario.BackColor = Color.White;
        cboVeterinario.DropDownStyle = ComboBoxStyle.DropDownList;
        cboVeterinario.FormattingEnabled = true;
        cboVeterinario.Location = new Point(132, 74);
        cboVeterinario.Name = "cboVeterinario";
        cboVeterinario.Size = new Size(200, 23);
        cboVeterinario.TabIndex = 4;
        // 
        // lblEspecie
        // 
        lblEspecie.Location = new Point(352, 74);
        lblEspecie.Name = "lblEspecie";
        lblEspecie.Size = new Size(86, 23);
        lblEspecie.TabIndex = 5;
        lblEspecie.Text = "Especie";
        lblEspecie.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // cboEspecie
        // 
        cboEspecie.BackColor = Color.White;
        cboEspecie.DropDownStyle = ComboBoxStyle.DropDownList;
        cboEspecie.FormattingEnabled = true;
        cboEspecie.Location = new Point(444, 74);
        cboEspecie.Name = "cboEspecie";
        cboEspecie.Size = new Size(140, 23);
        cboEspecie.TabIndex = 6;
        // 
        // FormReportes
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
        Name = "FormReportes";
        StartPosition = FormStartPosition.CenterParent;
        Text = "CLÍNICA VETERINARIA — ADMINISTRADOR — REPORTES";
        Load += FormReportes_Load;
        pnlEncabezado.ResumeLayout(false);
        pnlContenido.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)dgvReporte).EndInit();
        pnlTotales.ResumeLayout(false);
        pnlAcciones.ResumeLayout(false);
        grpFiltros.ResumeLayout(false);
        pnlContenedorFechas.ResumeLayout(false);
        ResumeLayout(false);
    }

    private Panel pnlEncabezado;
    private Label lblTitulo;
    private Label lblUsuarioSesion;
    private Panel pnlContenido;
    private GroupBox grpFiltros;
    private Label lblTipoReporte;
    private ComboBox cboTipoReporte;
    private Panel pnlContenedorFechas;
    private Label lblFechaDesde;
    private DateTimePicker dtpFechaDesde;
    private Label lblFechaHasta;
    private DateTimePicker dtpFechaHasta;
    private Label lblVeterinario;
    private ComboBox cboVeterinario;
    private Label lblEspecie;
    private ComboBox cboEspecie;
    private Panel pnlAcciones;
    private Button btnGenerar;
    private Button btnImprimir;
    private Button btnExportarPdf;
    private Button btnVolver;
    private DataGridView dgvReporte;
    public DataGridView dgvResultados => dgvReporte;
    private Panel pnlTotales;
    private Label lblTotalRegistros;
    private Label lblTotalFacturado;
}
