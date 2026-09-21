namespace Veterinaria.WinForms.Vistas.Veterinario;

partial class FormModificarVacuna
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
        pnlContenedor = new Panel();
        lblSeleccionar = new Label();
        cboSeleccionarVacuna = new ComboBox();
        lblNombre = new Label();
        txtNombre = new TextBox();
        lblPeriodoMeses = new Label();
        numPeriodoMeses = new NumericUpDown();
        lblPrecio = new Label();
        txtPrecioUnitario = new TextBox();
        pnlBotones = new Panel();
        btnModificar = new Button();
        btnBaja = new Button();
        btnCancelar = new Button();
        pnlEncabezado.SuspendLayout();
        pnlContenedor.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)numPeriodoMeses).BeginInit();
        pnlBotones.SuspendLayout();
        SuspendLayout();
        // 
        // pnlEncabezado
        // 
        pnlEncabezado.BackColor = Color.FromArgb(200, 138, 150);
        pnlEncabezado.Controls.Add(lblTitulo);
        pnlEncabezado.Dock = DockStyle.Top;
        pnlEncabezado.Location = new Point(0, 0);
        pnlEncabezado.Name = "pnlEncabezado";
        pnlEncabezado.Padding = new Padding(16, 0, 16, 0);
        pnlEncabezado.Size = new Size(520, 45);
        pnlEncabezado.TabIndex = 0;
        // 
        // lblTitulo
        // 
        lblTitulo.Dock = DockStyle.Fill;
        lblTitulo.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
        lblTitulo.ForeColor = Color.White;
        lblTitulo.Location = new Point(16, 0);
        lblTitulo.Name = "lblTitulo";
        lblTitulo.Size = new Size(488, 45);
        lblTitulo.TabIndex = 0;
        lblTitulo.Text = "MODIFICACIÓN Y BAJA DE VACUNA";
        lblTitulo.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // pnlContenedor
        // 
        pnlContenedor.BackColor = Color.FromArgb(250, 244, 244);
        pnlContenedor.Controls.Add(lblSeleccionar);
        pnlContenedor.Controls.Add(cboSeleccionarVacuna);
        pnlContenedor.Controls.Add(lblNombre);
        pnlContenedor.Controls.Add(txtNombre);
        pnlContenedor.Controls.Add(lblPeriodoMeses);
        pnlContenedor.Controls.Add(numPeriodoMeses);
        pnlContenedor.Controls.Add(lblPrecio);
        pnlContenedor.Controls.Add(txtPrecioUnitario);
        pnlContenedor.Dock = DockStyle.Fill;
        pnlContenedor.Location = new Point(0, 45);
        pnlContenedor.Name = "pnlContenedor";
        pnlContenedor.Padding = new Padding(24, 16, 24, 16);
        pnlContenedor.Size = new Size(520, 260);
        pnlContenedor.TabIndex = 1;
        // 
        // lblSeleccionar
        // 
        lblSeleccionar.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
        lblSeleccionar.ForeColor = Color.FromArgb(58, 53, 59);
        lblSeleccionar.Location = new Point(24, 12);
        lblSeleccionar.Name = "lblSeleccionar";
        lblSeleccionar.Size = new Size(472, 18);
        lblSeleccionar.TabIndex = 0;
        lblSeleccionar.Text = "Seleccionar Vacuna del Catálogo:";
        // 
        // cboSeleccionarVacuna
        // 
        cboSeleccionarVacuna.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
        cboSeleccionarVacuna.AutoCompleteSource = AutoCompleteSource.ListItems;
        cboSeleccionarVacuna.BackColor = Color.White;
        cboSeleccionarVacuna.Font = new Font("Segoe UI", 9.5F);
        cboSeleccionarVacuna.ForeColor = Color.FromArgb(58, 53, 59);
        cboSeleccionarVacuna.FormattingEnabled = true;
        cboSeleccionarVacuna.Location = new Point(24, 33);
        cboSeleccionarVacuna.Name = "cboSeleccionarVacuna";
        cboSeleccionarVacuna.Size = new Size(472, 25);
        cboSeleccionarVacuna.TabIndex = 1;
        // 
        // lblNombre
        // 
        lblNombre.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
        lblNombre.ForeColor = Color.FromArgb(58, 53, 59);
        lblNombre.Location = new Point(24, 72);
        lblNombre.Name = "lblNombre";
        lblNombre.Size = new Size(472, 18);
        lblNombre.TabIndex = 2;
        lblNombre.Text = "Nombre de la Vacuna:";
        // 
        // txtNombre
        // 
        txtNombre.BackColor = Color.White;
        txtNombre.BorderStyle = BorderStyle.FixedSingle;
        txtNombre.Font = new Font("Segoe UI", 9.5F);
        txtNombre.ForeColor = Color.FromArgb(58, 53, 59);
        txtNombre.Location = new Point(24, 93);
        txtNombre.MaxLength = 100;
        txtNombre.Name = "txtNombre";
        txtNombre.Size = new Size(472, 24);
        txtNombre.TabIndex = 3;
        // 
        // lblPeriodoMeses
        // 
        lblPeriodoMeses.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
        lblPeriodoMeses.ForeColor = Color.FromArgb(58, 53, 59);
        lblPeriodoMeses.Location = new Point(24, 130);
        lblPeriodoMeses.Name = "lblPeriodoMeses";
        lblPeriodoMeses.Size = new Size(228, 18);
        lblPeriodoMeses.TabIndex = 4;
        lblPeriodoMeses.Text = "Período Recomendado (meses):";
        // 
        // numPeriodoMeses
        // 
        numPeriodoMeses.BackColor = Color.White;
        numPeriodoMeses.Font = new Font("Segoe UI", 9.5F);
        numPeriodoMeses.ForeColor = Color.FromArgb(58, 53, 59);
        numPeriodoMeses.Location = new Point(24, 151);
        numPeriodoMeses.Maximum = new decimal(new int[] { 120, 0, 0, 0 });
        numPeriodoMeses.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
        numPeriodoMeses.Name = "numPeriodoMeses";
        numPeriodoMeses.Size = new Size(228, 24);
        numPeriodoMeses.TabIndex = 5;
        numPeriodoMeses.TextAlign = HorizontalAlignment.Right;
        numPeriodoMeses.Value = new decimal(new int[] { 12, 0, 0, 0 });
        // 
        // lblPrecio
        // 
        lblPrecio.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
        lblPrecio.ForeColor = Color.FromArgb(58, 53, 59);
        lblPrecio.Location = new Point(268, 130);
        lblPrecio.Name = "lblPrecio";
        lblPrecio.Size = new Size(228, 18);
        lblPrecio.TabIndex = 6;
        lblPrecio.Text = "Precio Unitario ($):";
        // 
        // txtPrecioUnitario
        // 
        txtPrecioUnitario.BackColor = Color.White;
        txtPrecioUnitario.BorderStyle = BorderStyle.FixedSingle;
        txtPrecioUnitario.Font = new Font("Segoe UI", 9.5F);
        txtPrecioUnitario.ForeColor = Color.FromArgb(58, 53, 59);
        txtPrecioUnitario.Location = new Point(268, 151);
        txtPrecioUnitario.MaxLength = 20;
        txtPrecioUnitario.Name = "txtPrecioUnitario";
        txtPrecioUnitario.Size = new Size(228, 24);
        txtPrecioUnitario.TabIndex = 7;
        txtPrecioUnitario.Text = "0,00";
        txtPrecioUnitario.TextAlign = HorizontalAlignment.Right;
        // 
        // pnlBotones
        // 
        pnlBotones.BackColor = Color.FromArgb(249, 240, 242);
        pnlBotones.Controls.Add(btnModificar);
        pnlBotones.Controls.Add(btnBaja);
        pnlBotones.Controls.Add(btnCancelar);
        pnlBotones.Dock = DockStyle.Bottom;
        pnlBotones.Location = new Point(0, 305);
        pnlBotones.Name = "pnlBotones";
        pnlBotones.Padding = new Padding(16, 10, 16, 10);
        pnlBotones.Size = new Size(520, 52);
        pnlBotones.TabIndex = 2;
        // 
        // btnModificar
        // 
        btnModificar.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
        btnModificar.BackColor = Color.FromArgb(148, 176, 214);
        btnModificar.Cursor = Cursors.Hand;
        btnModificar.FlatAppearance.BorderSize = 0;
        btnModificar.FlatStyle = FlatStyle.Flat;
        btnModificar.Font = new Font("Segoe UI", 9F);
        btnModificar.ForeColor = Color.Black;
        btnModificar.Location = new Point(160, 10);
        btnModificar.Name = "btnModificar";
        btnModificar.Size = new Size(110, 32);
        btnModificar.TabIndex = 0;
        btnModificar.Text = "Modificar";
        btnModificar.UseVisualStyleBackColor = false;
        btnModificar.Click += btnModificar_Click;
        // 
        // btnBaja
        // 
        btnBaja.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
        btnBaja.BackColor = Color.FromArgb(230, 160, 165);
        btnBaja.Cursor = Cursors.Hand;
        btnBaja.FlatAppearance.BorderSize = 0;
        btnBaja.FlatStyle = FlatStyle.Flat;
        btnBaja.Font = new Font("Segoe UI", 9F);
        btnBaja.ForeColor = Color.Black;
        btnBaja.Location = new Point(280, 10);
        btnBaja.Name = "btnBaja";
        btnBaja.Size = new Size(115, 32);
        btnBaja.TabIndex = 1;
        btnBaja.Text = "Dar de Baja";
        btnBaja.UseVisualStyleBackColor = false;
        btnBaja.Click += btnBaja_Click;
        // 
        // btnCancelar
        // 
        btnCancelar.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
        btnCancelar.BackColor = Color.FromArgb(226, 217, 220);
        btnCancelar.Cursor = Cursors.Hand;
        btnCancelar.DialogResult = DialogResult.Cancel;
        btnCancelar.FlatAppearance.BorderSize = 0;
        btnCancelar.FlatStyle = FlatStyle.Flat;
        btnCancelar.Font = new Font("Segoe UI", 9F);
        btnCancelar.ForeColor = Color.FromArgb(58, 53, 59);
        btnCancelar.Location = new Point(405, 10);
        btnCancelar.Name = "btnCancelar";
        btnCancelar.Size = new Size(100, 32);
        btnCancelar.TabIndex = 2;
        btnCancelar.Text = "Cancelar";
        btnCancelar.UseVisualStyleBackColor = false;
        btnCancelar.Click += btnCancelar_Click;
        // 
        // FormModificarVacuna
        // 
        AcceptButton = btnModificar;
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        BackColor = Color.FromArgb(250, 244, 244);
        CancelButton = btnCancelar;
        ClientSize = new Size(520, 357);
        Controls.Add(pnlContenedor);
        Controls.Add(pnlBotones);
        Controls.Add(pnlEncabezado);
        Font = new Font("Segoe UI", 9F);
        ForeColor = Color.FromArgb(58, 53, 59);
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;
        MinimizeBox = false;
        Name = "FormModificarVacuna";
        ShowInTaskbar = false;
        StartPosition = FormStartPosition.CenterParent;
        Text = "Modificar Vacuna — Catálogo Veterinario";
        pnlEncabezado.ResumeLayout(false);
        pnlContenedor.ResumeLayout(false);
        pnlContenedor.PerformLayout();
        ((System.ComponentModel.ISupportInitialize)numPeriodoMeses).EndInit();
        pnlBotones.ResumeLayout(false);
        ResumeLayout(false);
    }

    private Panel pnlEncabezado;
    private Label lblTitulo;
    private Panel pnlContenedor;
    private Label lblSeleccionar;
    private ComboBox cboSeleccionarVacuna;
    private Label lblNombre;
    private TextBox txtNombre;
    private Label lblPeriodoMeses;
    private NumericUpDown numPeriodoMeses;
    private Label lblPrecio;
    private TextBox txtPrecioUnitario;
    private Panel pnlBotones;
    private Button btnModificar;
    private Button btnBaja;
    private Button btnCancelar;
}
