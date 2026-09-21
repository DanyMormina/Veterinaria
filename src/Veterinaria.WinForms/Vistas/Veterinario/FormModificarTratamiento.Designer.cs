namespace Veterinaria.WinForms.Vistas.Veterinario;

partial class FormModificarTratamiento
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
        cboSeleccionarTratamiento = new ComboBox();
        lblTipo = new Label();
        cboTipo = new ComboBox();
        lblDescripcion = new Label();
        txtDescripcion = new TextBox();
        lblDosis = new Label();
        txtDosis = new TextBox();
        lblPrecio = new Label();
        numPrecio = new NumericUpDown();
        pnlBotones = new Panel();
        btnModificar = new Button();
        btnBaja = new Button();
        btnCancelar = new Button();
        pnlEncabezado.SuspendLayout();
        pnlContenedor.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)numPrecio).BeginInit();
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
        pnlEncabezado.Size = new Size(514, 45);
        pnlEncabezado.TabIndex = 0;
        // 
        // lblTitulo
        // 
        lblTitulo.Dock = DockStyle.Fill;
        lblTitulo.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
        lblTitulo.ForeColor = Color.White;
        lblTitulo.Location = new Point(16, 0);
        lblTitulo.Name = "lblTitulo";
        lblTitulo.Size = new Size(482, 45);
        lblTitulo.TabIndex = 0;
        lblTitulo.Text = "MODIFICAR / DAR DE BAJA TRATAMIENTO";
        lblTitulo.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // pnlContenedor
        // 
        pnlContenedor.BackColor = Color.FromArgb(250, 244, 244);
        pnlContenedor.Controls.Add(lblSeleccionar);
        pnlContenedor.Controls.Add(cboSeleccionarTratamiento);
        pnlContenedor.Controls.Add(lblTipo);
        pnlContenedor.Controls.Add(cboTipo);
        pnlContenedor.Controls.Add(lblDescripcion);
        pnlContenedor.Controls.Add(txtDescripcion);
        pnlContenedor.Controls.Add(lblDosis);
        pnlContenedor.Controls.Add(txtDosis);
        pnlContenedor.Controls.Add(lblPrecio);
        pnlContenedor.Controls.Add(numPrecio);
        pnlContenedor.Dock = DockStyle.Fill;
        pnlContenedor.Location = new Point(0, 45);
        pnlContenedor.Name = "pnlContenedor";
        pnlContenedor.Padding = new Padding(24, 16, 24, 16);
        pnlContenedor.Size = new Size(514, 313);
        pnlContenedor.TabIndex = 1;
        // 
        // lblSeleccionar
        // 
        lblSeleccionar.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
        lblSeleccionar.ForeColor = Color.FromArgb(58, 53, 59);
        lblSeleccionar.Location = new Point(24, 12);
        lblSeleccionar.Name = "lblSeleccionar";
        lblSeleccionar.Size = new Size(466, 18);
        lblSeleccionar.TabIndex = 0;
        lblSeleccionar.Text = "Seleccionar Tratamiento del Catálogo:";
        // 
        // cboSeleccionarTratamiento
        // 
        cboSeleccionarTratamiento.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
        cboSeleccionarTratamiento.AutoCompleteSource = AutoCompleteSource.ListItems;
        cboSeleccionarTratamiento.BackColor = Color.White;
        cboSeleccionarTratamiento.Font = new Font("Segoe UI", 9.5F);
        cboSeleccionarTratamiento.ForeColor = Color.FromArgb(58, 53, 59);
        cboSeleccionarTratamiento.FormattingEnabled = true;
        cboSeleccionarTratamiento.Location = new Point(24, 33);
        cboSeleccionarTratamiento.Name = "cboSeleccionarTratamiento";
        cboSeleccionarTratamiento.Size = new Size(466, 25);
        cboSeleccionarTratamiento.TabIndex = 1;
        cboSeleccionarTratamiento.SelectedIndexChanged += cboSeleccionarTratamiento_SelectedIndexChanged;
        // 
        // lblTipo
        // 
        lblTipo.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
        lblTipo.ForeColor = Color.FromArgb(58, 53, 59);
        lblTipo.Location = new Point(24, 70);
        lblTipo.Name = "lblTipo";
        lblTipo.Size = new Size(466, 18);
        lblTipo.TabIndex = 2;
        lblTipo.Text = "Tipo de Tratamiento:";
        // 
        // cboTipo
        // 
        cboTipo.BackColor = Color.White;
        cboTipo.DropDownStyle = ComboBoxStyle.DropDownList;
        cboTipo.Font = new Font("Segoe UI", 9.5F);
        cboTipo.ForeColor = Color.FromArgb(58, 53, 59);
        cboTipo.FormattingEnabled = true;
        cboTipo.Location = new Point(24, 91);
        cboTipo.Name = "cboTipo";
        cboTipo.Size = new Size(466, 25);
        cboTipo.TabIndex = 3;
        // 
        // lblDescripcion
        // 
        lblDescripcion.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
        lblDescripcion.ForeColor = Color.FromArgb(58, 53, 59);
        lblDescripcion.Location = new Point(24, 128);
        lblDescripcion.Name = "lblDescripcion";
        lblDescripcion.Size = new Size(466, 18);
        lblDescripcion.TabIndex = 4;
        lblDescripcion.Text = "Descripción / Nombre Comercial:";
        // 
        // txtDescripcion
        // 
        txtDescripcion.BackColor = Color.White;
        txtDescripcion.BorderStyle = BorderStyle.FixedSingle;
        txtDescripcion.Font = new Font("Segoe UI", 9.5F);
        txtDescripcion.ForeColor = Color.FromArgb(58, 53, 59);
        txtDescripcion.Location = new Point(24, 149);
        txtDescripcion.MaxLength = 200;
        txtDescripcion.Name = "txtDescripcion";
        txtDescripcion.Size = new Size(466, 24);
        txtDescripcion.TabIndex = 5;
        // 
        // lblDosis
        // 
        lblDosis.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
        lblDosis.ForeColor = Color.FromArgb(58, 53, 59);
        lblDosis.Location = new Point(24, 185);
        lblDosis.Name = "lblDosis";
        lblDosis.Size = new Size(466, 18);
        lblDosis.TabIndex = 6;
        lblDosis.Text = "Dosis Recomendada / Presentación:";
        // 
        // txtDosis
        // 
        txtDosis.BackColor = Color.White;
        txtDosis.BorderStyle = BorderStyle.FixedSingle;
        txtDosis.Font = new Font("Segoe UI", 9.5F);
        txtDosis.ForeColor = Color.FromArgb(58, 53, 59);
        txtDosis.Location = new Point(24, 206);
        txtDosis.MaxLength = 150;
        txtDosis.Name = "txtDosis";
        txtDosis.Size = new Size(466, 24);
        txtDosis.TabIndex = 7;
        // 
        // lblPrecio
        // 
        lblPrecio.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
        lblPrecio.ForeColor = Color.FromArgb(58, 53, 59);
        lblPrecio.Location = new Point(24, 242);
        lblPrecio.Name = "lblPrecio";
        lblPrecio.Size = new Size(466, 18);
        lblPrecio.TabIndex = 8;
        lblPrecio.Text = "Precio Unitario ($):";
        // 
        // numPrecio
        // 
        numPrecio.BackColor = Color.White;
        numPrecio.DecimalPlaces = 2;
        numPrecio.Font = new Font("Segoe UI", 9.5F);
        numPrecio.ForeColor = Color.FromArgb(58, 53, 59);
        numPrecio.Increment = new decimal(new int[] { 100, 0, 0, 0 });
        numPrecio.Location = new Point(24, 263);
        numPrecio.Maximum = new decimal(new int[] { 10000000, 0, 0, 0 });
        numPrecio.Name = "numPrecio";
        numPrecio.Size = new Size(200, 24);
        numPrecio.TabIndex = 9;
        numPrecio.TextAlign = HorizontalAlignment.Right;
        // 
        // pnlBotones
        // 
        pnlBotones.BackColor = Color.FromArgb(249, 240, 242);
        pnlBotones.Controls.Add(btnModificar);
        pnlBotones.Controls.Add(btnBaja);
        pnlBotones.Controls.Add(btnCancelar);
        pnlBotones.Dock = DockStyle.Bottom;
        pnlBotones.Location = new Point(0, 358);
        pnlBotones.Name = "pnlBotones";
        pnlBotones.Padding = new Padding(16, 10, 16, 10);
        pnlBotones.Size = new Size(514, 52);
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
        btnModificar.Location = new Point(144, 10);
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
        btnBaja.Location = new Point(262, 10);
        btnBaja.Name = "btnBaja";
        btnBaja.Size = new Size(116, 32);
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
        btnCancelar.Location = new Point(386, 10);
        btnCancelar.Name = "btnCancelar";
        btnCancelar.Size = new Size(100, 32);
        btnCancelar.TabIndex = 2;
        btnCancelar.Text = "Cancelar";
        btnCancelar.UseVisualStyleBackColor = false;
        btnCancelar.Click += btnCancelar_Click;
        // 
        // FormModificarTratamiento
        // 
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        BackColor = Color.FromArgb(250, 244, 244);
        CancelButton = btnCancelar;
        ClientSize = new Size(514, 410);
        Controls.Add(pnlContenedor);
        Controls.Add(pnlBotones);
        Controls.Add(pnlEncabezado);
        Font = new Font("Segoe UI", 9F);
        ForeColor = Color.FromArgb(58, 53, 59);
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;
        MinimizeBox = false;
        Name = "FormModificarTratamiento";
        ShowInTaskbar = false;
        StartPosition = FormStartPosition.CenterParent;
        Text = "Modificar Tratamiento — Clínica Veterinaria";
        Load += FormModificarTratamiento_Load;
        pnlEncabezado.ResumeLayout(false);
        pnlContenedor.ResumeLayout(false);
        pnlContenedor.PerformLayout();
        ((System.ComponentModel.ISupportInitialize)numPrecio).EndInit();
        pnlBotones.ResumeLayout(false);
        ResumeLayout(false);
    }

    private Panel pnlEncabezado;
    private Label lblTitulo;
    private Panel pnlContenedor;
    private Label lblSeleccionar;
    private ComboBox cboSeleccionarTratamiento;
    private Label lblTipo;
    private ComboBox cboTipo;
    private Label lblDescripcion;
    private TextBox txtDescripcion;
    private Label lblDosis;
    private TextBox txtDosis;
    private Label lblPrecio;
    private NumericUpDown numPrecio;
    private Panel pnlBotones;
    private Button btnModificar;
    private Button btnBaja;
    private Button btnCancelar;
}
