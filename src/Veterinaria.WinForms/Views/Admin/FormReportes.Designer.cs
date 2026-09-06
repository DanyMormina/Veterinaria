namespace Veterinaria.WinForms.Views.Admin;

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
        pnlHeader = new Panel();
        lblTitulo = new Label();
        lblUsuarioSesion = new Label();
        pnlContenido = new Panel();
        grpFiltros = new GroupBox();
        lblTipoReporte = new Label();
        cboTipoReporte = new ComboBox();
        lblFechaDesde = new Label();
        dtpFechaDesde = new DateTimePicker();
        lblFechaHasta = new Label();
        dtpFechaHasta = new DateTimePicker();
        lblVeterinario = new Label();
        cboVeterinario = new ComboBox();
        lblEspecie = new Label();
        cboEspecie = new ComboBox();
        pnlAcciones = new Panel();
        btnGenerar = new Button();
        btnImprimir = new Button();
        btnExportarPdf = new Button();
        btnVolver = new Button();
        dgvReporte = new DataGridView();
        pnlTotales = new Panel();
        lblTotalRegistros = new Label();
        lblTotalFacturado = new Label();
        pnlHeader.SuspendLayout();
        pnlContenido.SuspendLayout();
        grpFiltros.SuspendLayout();
        pnlAcciones.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)dgvReporte).BeginInit();
        pnlTotales.SuspendLayout();
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
        lblTitulo.Size = new Size(620, 50);
        lblTitulo.TabIndex = 0;
        lblTitulo.Text = "CLÍNICA VETERINARIA — ADMINISTRADOR — REPORTES";
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
        pnlContenido.Location = new Point(0, 50);
        pnlContenido.Name = "pnlContenido";
        pnlContenido.Size = new Size(1100, 650);
        pnlContenido.TabIndex = 1;
        // 
        // grpFiltros
        // 
        grpFiltros.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
        grpFiltros.Controls.Add(lblTipoReporte);
        grpFiltros.Controls.Add(cboTipoReporte);
        grpFiltros.Controls.Add(lblFechaDesde);
        grpFiltros.Controls.Add(dtpFechaDesde);
        grpFiltros.Controls.Add(lblFechaHasta);
        grpFiltros.Controls.Add(dtpFechaHasta);
        grpFiltros.Controls.Add(lblVeterinario);
        grpFiltros.Controls.Add(cboVeterinario);
        grpFiltros.Controls.Add(lblEspecie);
        grpFiltros.Controls.Add(cboEspecie);
        grpFiltros.Font = new Font("Segoe UI", 9F);
        grpFiltros.ForeColor = Color.FromArgb(58, 53, 59);
        grpFiltros.Location = new Point(16, 16);
        grpFiltros.Name = "grpFiltros";
        grpFiltros.Size = new Size(1068, 118);
        grpFiltros.TabIndex = 0;
        grpFiltros.TabStop = false;
        grpFiltros.Text = "Filtros del reporte";
        // 
        // lblTipoReporte
        // 
        lblTipoReporte.Location = new Point(16, 32);
        lblTipoReporte.Name = "lblTipoReporte";
        lblTipoReporte.Size = new Size(110, 23);
        lblTipoReporte.Text = "Tipo de reporte";
        lblTipoReporte.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // cboTipoReporte
        // 
        cboTipoReporte.BackColor = Color.White;
        cboTipoReporte.DropDownStyle = ComboBoxStyle.DropDownList;
        cboTipoReporte.FormattingEnabled = true;
        cboTipoReporte.Items.AddRange(new object[] { "Consultas", "Pagos", "Mascotas", "Propietarios" });
        cboTipoReporte.Location = new Point(132, 32);
        cboTipoReporte.Name = "cboTipoReporte";
        cboTipoReporte.Size = new Size(200, 23);
        // 
        // lblFechaDesde
        // 
        lblFechaDesde.Location = new Point(352, 32);
        lblFechaDesde.Name = "lblFechaDesde";
        lblFechaDesde.Size = new Size(86, 23);
        lblFechaDesde.Text = "Fecha desde";
        lblFechaDesde.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // dtpFechaDesde
        // 
        dtpFechaDesde.Format = DateTimePickerFormat.Short;
        dtpFechaDesde.Location = new Point(444, 32);
        dtpFechaDesde.Name = "dtpFechaDesde";
        dtpFechaDesde.Size = new Size(140, 23);
        // 
        // lblFechaHasta
        // 
        lblFechaHasta.Location = new Point(604, 32);
        lblFechaHasta.Name = "lblFechaHasta";
        lblFechaHasta.Size = new Size(80, 23);
        lblFechaHasta.Text = "Fecha hasta";
        lblFechaHasta.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // dtpFechaHasta
        // 
        dtpFechaHasta.Format = DateTimePickerFormat.Short;
        dtpFechaHasta.Location = new Point(690, 32);
        dtpFechaHasta.Name = "dtpFechaHasta";
        dtpFechaHasta.Size = new Size(140, 23);
        // 
        // lblVeterinario
        // 
        lblVeterinario.Location = new Point(16, 74);
        lblVeterinario.Name = "lblVeterinario";
        lblVeterinario.Size = new Size(110, 23);
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
        // 
        // lblEspecie
        // 
        lblEspecie.Location = new Point(352, 74);
        lblEspecie.Name = "lblEspecie";
        lblEspecie.Size = new Size(86, 23);
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
        pnlAcciones.Size = new Size(1068, 40);
        pnlAcciones.TabIndex = 1;
        // 
        // btnGenerar
        // 
        btnGenerar.BackColor = Color.FromArgb(220, 200, 204);
        btnGenerar.FlatAppearance.BorderColor = Color.FromArgb(186, 162, 168);
        btnGenerar.FlatStyle = FlatStyle.Flat;
        btnGenerar.ForeColor = Color.FromArgb(58, 53, 59);
        btnGenerar.Location = new Point(0, 4);
        btnGenerar.Name = "btnGenerar";
        btnGenerar.Size = new Size(120, 32);
        btnGenerar.Text = "Generar";
        btnGenerar.UseVisualStyleBackColor = false;
        // 
        // btnImprimir
        // 
        btnImprimir.BackColor = Color.FromArgb(220, 200, 204);
        btnImprimir.FlatAppearance.BorderColor = Color.FromArgb(186, 162, 168);
        btnImprimir.FlatStyle = FlatStyle.Flat;
        btnImprimir.ForeColor = Color.FromArgb(58, 53, 59);
        btnImprimir.Location = new Point(128, 4);
        btnImprimir.Name = "btnImprimir";
        btnImprimir.Size = new Size(120, 32);
        btnImprimir.Text = "Imprimir";
        btnImprimir.UseVisualStyleBackColor = false;
        // 
        // btnExportarPdf
        // 
        btnExportarPdf.BackColor = Color.FromArgb(220, 200, 204);
        btnExportarPdf.FlatAppearance.BorderColor = Color.FromArgb(186, 162, 168);
        btnExportarPdf.FlatStyle = FlatStyle.Flat;
        btnExportarPdf.ForeColor = Color.FromArgb(58, 53, 59);
        btnExportarPdf.Location = new Point(256, 4);
        btnExportarPdf.Name = "btnExportarPdf";
        btnExportarPdf.Size = new Size(120, 32);
        btnExportarPdf.Text = "Exportar PDF";
        btnExportarPdf.UseVisualStyleBackColor = false;
        // 
        // btnVolver
        // 
        btnVolver.BackColor = Color.FromArgb(220, 200, 204);
        btnVolver.FlatAppearance.BorderColor = Color.FromArgb(186, 162, 168);
        btnVolver.FlatStyle = FlatStyle.Flat;
        btnVolver.ForeColor = Color.FromArgb(58, 53, 59);
        btnVolver.Location = new Point(384, 4);
        btnVolver.Name = "btnVolver";
        btnVolver.Size = new Size(140, 32);
        btnVolver.Text = "Volver al panel";
        btnVolver.UseVisualStyleBackColor = false;
        btnVolver.Click += btnVolver_Click;
        // 
        // dgvReporte
        // 
        dgvReporte.AllowUserToAddRows = false;
        dgvReporte.AllowUserToDeleteRows = false;
        dgvReporte.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        dgvReporte.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        dgvReporte.BackgroundColor = Color.White;
        dgvReporte.BorderStyle = BorderStyle.FixedSingle;
        dgvReporte.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
        dgvReporte.Location = new Point(16, 190);
        dgvReporte.Name = "dgvReporte";
        dgvReporte.ReadOnly = true;
        dgvReporte.RowHeadersVisible = false;
        dgvReporte.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        dgvReporte.Size = new Size(1068, 396);
        // 
        // pnlTotales
        // 
        pnlTotales.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        pnlTotales.BorderStyle = BorderStyle.FixedSingle;
        pnlTotales.Controls.Add(lblTotalRegistros);
        pnlTotales.Controls.Add(lblTotalFacturado);
        pnlTotales.Location = new Point(16, 594);
        pnlTotales.Name = "pnlTotales";
        pnlTotales.Size = new Size(1068, 40);
        pnlTotales.TabIndex = 3;
        // 
        // lblTotalRegistros
        // 
        lblTotalRegistros.Font = new Font("Segoe UI", 9.75F);
        lblTotalRegistros.Location = new Point(12, 8);
        lblTotalRegistros.Name = "lblTotalRegistros";
        lblTotalRegistros.Size = new Size(400, 23);
        lblTotalRegistros.Text = "Total de registros:";
        lblTotalRegistros.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // lblTotalFacturado
        // 
        lblTotalFacturado.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        lblTotalFacturado.Font = new Font("Segoe UI", 9.75F);
        lblTotalFacturado.Location = new Point(656, 8);
        lblTotalFacturado.Name = "lblTotalFacturado";
        lblTotalFacturado.Size = new Size(400, 23);
        lblTotalFacturado.Text = "Total facturado:";
        lblTotalFacturado.TextAlign = ContentAlignment.MiddleRight;
        // 
        // FormReportes
        // 
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        BackColor = Color.FromArgb(250, 244, 244);
        ClientSize = new Size(1100, 700);
        Controls.Add(pnlContenido);
        Controls.Add(pnlHeader);
        Font = new Font("Segoe UI", 9F);
        ForeColor = Color.FromArgb(58, 53, 59);
        MinimumSize = new Size(1100, 700);
        Name = "FormReportes";
        StartPosition = FormStartPosition.CenterParent;
        Text = "CLÍNICA VETERINARIA — ADMINISTRADOR — REPORTES";
        Load += FormReportes_Load;
        pnlHeader.ResumeLayout(false);
        pnlContenido.ResumeLayout(false);
        grpFiltros.ResumeLayout(false);
        grpFiltros.PerformLayout();
        pnlAcciones.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)dgvReporte).EndInit();
        pnlTotales.ResumeLayout(false);
        ResumeLayout(false);
    }

    private Panel pnlHeader;
    private Label lblTitulo;
    private Label lblUsuarioSesion;
    private Panel pnlContenido;
    private GroupBox grpFiltros;
    private Label lblTipoReporte;
    private ComboBox cboTipoReporte;
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
    private Panel pnlTotales;
    private Label lblTotalRegistros;
    private Label lblTotalFacturado;
}
