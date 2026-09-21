namespace Veterinaria.WinForms.Vistas.Veterinario;

partial class FormAltaVacuna
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
        lblEspecie = new Label();
        cboEspecie = new ComboBox();
        lblNombre = new Label();
        txtNombre = new TextBox();
        lblPeriodoMeses = new Label();
        numPeriodoMeses = new NumericUpDown();
        lblPrecio = new Label();
        txtPrecioUnitario = new TextBox();
        pnlBotones = new Panel();
        btnGuardar = new Button();
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
        lblTitulo.Text = "ALTA DE NUEVA VACUNA";
        lblTitulo.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // pnlContenedor
        // 
        pnlContenedor.BackColor = Color.FromArgb(250, 244, 244);
        pnlContenedor.Controls.Add(lblEspecie);
        pnlContenedor.Controls.Add(cboEspecie);
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
        pnlContenedor.Size = new Size(484, 255);
        pnlContenedor.TabIndex = 1;
        // 
        // lblEspecie
        // 
        lblEspecie.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
        lblEspecie.ForeColor = Color.FromArgb(58, 53, 59);
        lblEspecie.Location = new Point(24, 12);
        lblEspecie.Name = "lblEspecie";
        lblEspecie.Size = new Size(436, 18);
        lblEspecie.TabIndex = 0;
        lblEspecie.Text = "Especie Destino:";
        // 
        // cboEspecie
        // 
        cboEspecie.BackColor = Color.White;
        cboEspecie.DropDownStyle = ComboBoxStyle.DropDownList;
        cboEspecie.Font = new Font("Segoe UI", 9.5F);
        cboEspecie.ForeColor = Color.FromArgb(58, 53, 59);
        cboEspecie.FormattingEnabled = true;
        cboEspecie.Location = new Point(24, 33);
        cboEspecie.Name = "cboEspecie";
        cboEspecie.Size = new Size(436, 25);
        cboEspecie.TabIndex = 1;
        // 
        // lblNombre
        // 
        lblNombre.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
        lblNombre.ForeColor = Color.FromArgb(58, 53, 59);
        lblNombre.Location = new Point(24, 68);
        lblNombre.Name = "lblNombre";
        lblNombre.Size = new Size(436, 18);
        lblNombre.TabIndex = 2;
        lblNombre.Text = "Nombre de la Vacuna:";
        // 
        // txtNombre
        // 
        txtNombre.BackColor = Color.White;
        txtNombre.BorderStyle = BorderStyle.FixedSingle;
        txtNombre.Font = new Font("Segoe UI", 9.5F);
        txtNombre.ForeColor = Color.FromArgb(58, 53, 59);
        txtNombre.Location = new Point(24, 89);
        txtNombre.MaxLength = 100;
        txtNombre.Name = "txtNombre";
        txtNombre.Size = new Size(436, 24);
        txtNombre.TabIndex = 3;
        // 
        // lblPeriodoMeses
        // 
        lblPeriodoMeses.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
        lblPeriodoMeses.ForeColor = Color.FromArgb(58, 53, 59);
        lblPeriodoMeses.Location = new Point(24, 126);
        lblPeriodoMeses.Name = "lblPeriodoMeses";
        lblPeriodoMeses.Size = new Size(210, 18);
        lblPeriodoMeses.TabIndex = 4;
        lblPeriodoMeses.Text = "Período Recomendado (meses):";
        // 
        // numPeriodoMeses
        // 
        numPeriodoMeses.BackColor = Color.White;
        numPeriodoMeses.Font = new Font("Segoe UI", 9.5F);
        numPeriodoMeses.ForeColor = Color.FromArgb(58, 53, 59);
        numPeriodoMeses.Location = new Point(24, 147);
        numPeriodoMeses.Maximum = new decimal(new int[] { 120, 0, 0, 0 });
        numPeriodoMeses.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
        numPeriodoMeses.Name = "numPeriodoMeses";
        numPeriodoMeses.Size = new Size(210, 24);
        numPeriodoMeses.TabIndex = 5;
        numPeriodoMeses.TextAlign = HorizontalAlignment.Right;
        numPeriodoMeses.Value = new decimal(new int[] { 12, 0, 0, 0 });
        // 
        // lblPrecio
        // 
        lblPrecio.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
        lblPrecio.ForeColor = Color.FromArgb(58, 53, 59);
        lblPrecio.Location = new Point(250, 126);
        lblPrecio.Name = "lblPrecio";
        lblPrecio.Size = new Size(210, 18);
        lblPrecio.TabIndex = 6;
        lblPrecio.Text = "Precio Unitario ($):";
        // 
        // txtPrecioUnitario
        // 
        txtPrecioUnitario.BackColor = Color.White;
        txtPrecioUnitario.BorderStyle = BorderStyle.FixedSingle;
        txtPrecioUnitario.Font = new Font("Segoe UI", 9.5F);
        txtPrecioUnitario.ForeColor = Color.FromArgb(58, 53, 59);
        txtPrecioUnitario.Location = new Point(250, 147);
        txtPrecioUnitario.MaxLength = 20;
        txtPrecioUnitario.Name = "txtPrecioUnitario";
        txtPrecioUnitario.Size = new Size(210, 24);
        txtPrecioUnitario.TabIndex = 7;
        txtPrecioUnitario.Text = "0,00";
        txtPrecioUnitario.TextAlign = HorizontalAlignment.Right;
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
        // FormAltaVacuna
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
        Name = "FormAltaVacuna";
        ShowInTaskbar = false;
        StartPosition = FormStartPosition.CenterParent;
        Text = "Nueva Vacuna — Catálogo Veterinario";
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
    private Label lblEspecie;
    private ComboBox cboEspecie;
    private Label lblNombre;
    private TextBox txtNombre;
    private Label lblPeriodoMeses;
    private NumericUpDown numPeriodoMeses;
    private Label lblPrecio;
    private TextBox txtPrecioUnitario;
    private Panel pnlBotones;
    private Button btnGuardar;
    private Button btnCancelar;
}
