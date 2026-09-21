namespace Veterinaria.WinForms.Vistas.Veterinario;

partial class FormAltaTratamiento
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
        lblTipo = new Label();
        cboTipo = new ComboBox();
        lblDescripcion = new Label();
        txtDescripcion = new TextBox();
        lblDosis = new Label();
        txtDosis = new TextBox();
        lblPrecio = new Label();
        numPrecio = new NumericUpDown();
        pnlBotones = new Panel();
        btnGuardar = new Button();
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
        pnlEncabezado.Size = new Size(484, 45);
        pnlEncabezado.TabIndex = 0;
        // 
        // lblTitulo
        // 
        lblTitulo.Dock = DockStyle.Fill;
        lblTitulo.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
        lblTitulo.ForeColor = Color.White;
        lblTitulo.Location = new Point(16, 0);
        lblTitulo.Name = "lblTitulo";
        lblTitulo.Size = new Size(452, 45);
        lblTitulo.TabIndex = 0;
        lblTitulo.Text = "ALTA DE NUEVO TRATAMIENTO";
        lblTitulo.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // pnlContenedor
        // 
        pnlContenedor.BackColor = Color.FromArgb(250, 244, 244);
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
        pnlContenedor.Padding = new Padding(24, 20, 24, 16);
        pnlContenedor.Size = new Size(484, 255);
        pnlContenedor.TabIndex = 1;
        // 
        // lblTipo
        // 
        lblTipo.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
        lblTipo.ForeColor = Color.FromArgb(58, 53, 59);
        lblTipo.Location = new Point(24, 15);
        lblTipo.Name = "lblTipo";
        lblTipo.Size = new Size(436, 18);
        lblTipo.TabIndex = 0;
        lblTipo.Text = "Tipo de Tratamiento:";
        // 
        // cboTipo
        // 
        cboTipo.BackColor = Color.White;
        cboTipo.DropDownStyle = ComboBoxStyle.DropDownList;
        cboTipo.Font = new Font("Segoe UI", 9.5F);
        cboTipo.ForeColor = Color.FromArgb(58, 53, 59);
        cboTipo.FormattingEnabled = true;
        cboTipo.Location = new Point(24, 36);
        cboTipo.Name = "cboTipo";
        cboTipo.Size = new Size(436, 25);
        cboTipo.TabIndex = 1;
        // 
        // lblDescripcion
        // 
        lblDescripcion.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
        lblDescripcion.ForeColor = Color.FromArgb(58, 53, 59);
        lblDescripcion.Location = new Point(24, 72);
        lblDescripcion.Name = "lblDescripcion";
        lblDescripcion.Size = new Size(436, 18);
        lblDescripcion.TabIndex = 2;
        lblDescripcion.Text = "Descripción / Nombre Comercial:";
        // 
        // txtDescripcion
        // 
        txtDescripcion.BackColor = Color.White;
        txtDescripcion.BorderStyle = BorderStyle.FixedSingle;
        txtDescripcion.Font = new Font("Segoe UI", 9.5F);
        txtDescripcion.ForeColor = Color.FromArgb(58, 53, 59);
        txtDescripcion.Location = new Point(24, 93);
        txtDescripcion.MaxLength = 200;
        txtDescripcion.Name = "txtDescripcion";
        txtDescripcion.Size = new Size(436, 24);
        txtDescripcion.TabIndex = 3;
        // 
        // lblDosis
        // 
        lblDosis.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
        lblDosis.ForeColor = Color.FromArgb(58, 53, 59);
        lblDosis.Location = new Point(24, 129);
        lblDosis.Name = "lblDosis";
        lblDosis.Size = new Size(436, 18);
        lblDosis.TabIndex = 4;
        lblDosis.Text = "Dosis Recomendada / Presentación:";
        // 
        // txtDosis
        // 
        txtDosis.BackColor = Color.White;
        txtDosis.BorderStyle = BorderStyle.FixedSingle;
        txtDosis.Font = new Font("Segoe UI", 9.5F);
        txtDosis.ForeColor = Color.FromArgb(58, 53, 59);
        txtDosis.Location = new Point(24, 150);
        txtDosis.MaxLength = 150;
        txtDosis.Name = "txtDosis";
        txtDosis.Size = new Size(436, 24);
        txtDosis.TabIndex = 5;
        // 
        // lblPrecio
        // 
        lblPrecio.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
        lblPrecio.ForeColor = Color.FromArgb(58, 53, 59);
        lblPrecio.Location = new Point(24, 186);
        lblPrecio.Name = "lblPrecio";
        lblPrecio.Size = new Size(436, 18);
        lblPrecio.TabIndex = 6;
        lblPrecio.Text = "Precio Unitario ($):";
        // 
        // numPrecio
        // 
        numPrecio.BackColor = Color.White;
        numPrecio.DecimalPlaces = 2;
        numPrecio.Font = new Font("Segoe UI", 9.5F);
        numPrecio.ForeColor = Color.FromArgb(58, 53, 59);
        numPrecio.Increment = new decimal(new int[] { 100, 0, 0, 0 });
        numPrecio.Location = new Point(24, 207);
        numPrecio.Maximum = new decimal(new int[] { 10000000, 0, 0, 0 });
        numPrecio.Name = "numPrecio";
        numPrecio.Size = new Size(200, 24);
        numPrecio.TabIndex = 7;
        numPrecio.TextAlign = HorizontalAlignment.Right;
        // 
        // pnlBotones
        // 
        pnlBotones.BackColor = Color.FromArgb(249, 240, 242);
        pnlBotones.Controls.Add(btnGuardar);
        pnlBotones.Controls.Add(btnCancelar);
        pnlBotones.Dock = DockStyle.Bottom;
        pnlBotones.Location = new Point(0, 300);
        pnlBotones.Name = "pnlBotones";
        pnlBotones.Padding = new Padding(16, 10, 16, 10);
        pnlBotones.Size = new Size(484, 52);
        pnlBotones.TabIndex = 2;
        // 
        // btnGuardar
        // 
        btnGuardar.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
        btnGuardar.BackColor = Color.FromArgb(152, 196, 164);
        btnGuardar.Cursor = Cursors.Hand;
        btnGuardar.FlatAppearance.BorderSize = 0;
        btnGuardar.FlatStyle = FlatStyle.Flat;
        btnGuardar.Font = new Font("Segoe UI", 9.5F);
        btnGuardar.ForeColor = Color.Black;
        btnGuardar.Location = new Point(240, 10);
        btnGuardar.Name = "btnGuardar";
        btnGuardar.Size = new Size(110, 32);
        btnGuardar.TabIndex = 0;
        btnGuardar.Text = "Guardar";
        btnGuardar.UseVisualStyleBackColor = false;
        btnGuardar.Click += btnGuardar_Click;
        // 
        // btnCancelar
        // 
        btnCancelar.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
        btnCancelar.BackColor = Color.FromArgb(226, 217, 220);
        btnCancelar.Cursor = Cursors.Hand;
        btnCancelar.DialogResult = DialogResult.Cancel;
        btnCancelar.FlatAppearance.BorderSize = 0;
        btnCancelar.FlatStyle = FlatStyle.Flat;
        btnCancelar.Font = new Font("Segoe UI", 9.5F);
        btnCancelar.ForeColor = Color.FromArgb(58, 53, 59);
        btnCancelar.Location = new Point(360, 10);
        btnCancelar.Name = "btnCancelar";
        btnCancelar.Size = new Size(100, 32);
        btnCancelar.TabIndex = 1;
        btnCancelar.Text = "Cancelar";
        btnCancelar.UseVisualStyleBackColor = false;
        btnCancelar.Click += btnCancelar_Click;
        // 
        // FormAltaTratamiento
        // 
        AcceptButton = btnGuardar;
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        BackColor = Color.FromArgb(250, 244, 244);
        CancelButton = btnCancelar;
        ClientSize = new Size(484, 352);
        Controls.Add(pnlContenedor);
        Controls.Add(pnlBotones);
        Controls.Add(pnlEncabezado);
        Font = new Font("Segoe UI", 9F);
        ForeColor = Color.FromArgb(58, 53, 59);
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;
        MinimizeBox = false;
        Name = "FormAltaTratamiento";
        ShowInTaskbar = false;
        StartPosition = FormStartPosition.CenterParent;
        Text = "Nuevo Tratamiento — Clínica Veterinaria";
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
    private Label lblTipo;
    private ComboBox cboTipo;
    private Label lblDescripcion;
    private TextBox txtDescripcion;
    private Label lblDosis;
    private TextBox txtDosis;
    private Label lblPrecio;
    private NumericUpDown numPrecio;
    private Panel pnlBotones;
    private Button btnGuardar;
    private Button btnCancelar;
}
