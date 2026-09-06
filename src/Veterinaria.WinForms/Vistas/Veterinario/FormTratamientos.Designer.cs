namespace Veterinaria.WinForms.Vistas.Veterinario;

partial class FormTratamientos
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
        grpDatos = new GroupBox();
        lblTipo = new Label();
        cboTipo = new ComboBox();
        lblDescripcion = new Label();
        txtDescripcion = new TextBox();
        lblDosis = new Label();
        txtDosis = new TextBox();
        lblPrecio = new Label();
        txtPrecio = new TextBox();
        lblEstado = new Label();
        cboEstado = new ComboBox();
        pnlBotonesDatos = new Panel();
        btnNuevo = new Button();
        btnGuardar = new Button();
        btnModificar = new Button();
        btnCancelar = new Button();
        grpAplicados = new GroupBox();
        lblConsulta = new Label();
        cboConsulta = new ComboBox();
        lblTratamiento = new Label();
        cboTratamiento = new ComboBox();
        lblCantidad = new Label();
        txtCantidad = new TextBox();
        lblPrecioUnitario = new Label();
        txtPrecioUnitario = new TextBox();
        lblSubtotal = new Label();
        txtSubtotal = new TextBox();
        lblIndicaciones = new Label();
        txtIndicaciones = new TextBox();
        dgvAplicados = new DataGridView();
        colConsulta = new DataGridViewTextBoxColumn();
        colTratamiento = new DataGridViewTextBoxColumn();
        colCantidad = new DataGridViewTextBoxColumn();
        colPrecioUnitario = new DataGridViewTextBoxColumn();
        colSubtotal = new DataGridViewTextBoxColumn();
        colIndicaciones = new DataGridViewTextBoxColumn();
        btnVolver = new Button();
        barraEstado = new StatusStrip();
        lblInfoEstado = new ToolStripStatusLabel();
        pnlEncabezado.SuspendLayout();
        pnlContenido.SuspendLayout();
        grpDatos.SuspendLayout();
        pnlBotonesDatos.SuspendLayout();
        grpAplicados.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)dgvAplicados).BeginInit();
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
        lblTitulo.Text = "CLÍNICA VETERINARIA — ATENCIÓN CLÍNICA — TRATAMIENTOS";
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
        pnlContenido.Controls.Add(btnVolver);
        pnlContenido.Controls.Add(grpAplicados);
        pnlContenido.Controls.Add(pnlBotonesDatos);
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
        grpDatos.Controls.Add(lblTipo);
        grpDatos.Controls.Add(cboTipo);
        grpDatos.Controls.Add(lblDescripcion);
        grpDatos.Controls.Add(txtDescripcion);
        grpDatos.Controls.Add(lblDosis);
        grpDatos.Controls.Add(txtDosis);
        grpDatos.Controls.Add(lblPrecio);
        grpDatos.Controls.Add(txtPrecio);
        grpDatos.Controls.Add(lblEstado);
        grpDatos.Controls.Add(cboEstado);
        grpDatos.Font = new Font("Segoe UI", 9F);
        grpDatos.ForeColor = Color.FromArgb(58, 53, 59);
        grpDatos.Location = new Point(16, 12);
        grpDatos.Name = "grpDatos";
        grpDatos.Size = new Size(1068, 118);
        grpDatos.TabIndex = 0;
        grpDatos.TabStop = false;
        grpDatos.Text = "Datos del tratamiento";
        // 
        // lblTipo
        // 
        lblTipo.Location = new Point(16, 32);
        lblTipo.Name = "lblTipo";
        lblTipo.Size = new Size(90, 23);
        lblTipo.Text = "Tipo";
        lblTipo.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // cboTipo
        // 
        cboTipo.BackColor = Color.White;
        cboTipo.DropDownStyle = ComboBoxStyle.DropDownList;
        cboTipo.FormattingEnabled = true;
        cboTipo.Location = new Point(112, 32);
        cboTipo.Name = "cboTipo";
        cboTipo.Size = new Size(220, 23);
        // 
        // lblDescripcion
        // 
        lblDescripcion.Location = new Point(352, 32);
        lblDescripcion.Name = "lblDescripcion";
        lblDescripcion.Size = new Size(86, 23);
        lblDescripcion.Text = "Descripción";
        lblDescripcion.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // txtDescripcion
        // 
        txtDescripcion.BackColor = Color.White;
        txtDescripcion.BorderStyle = BorderStyle.FixedSingle;
        txtDescripcion.Location = new Point(444, 32);
        txtDescripcion.Name = "txtDescripcion";
        txtDescripcion.Size = new Size(600, 23);
        // 
        // lblDosis
        // 
        lblDosis.Location = new Point(16, 72);
        lblDosis.Name = "lblDosis";
        lblDosis.Size = new Size(90, 23);
        lblDosis.Text = "Dosis";
        lblDosis.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // txtDosis
        // 
        txtDosis.BackColor = Color.White;
        txtDosis.BorderStyle = BorderStyle.FixedSingle;
        txtDosis.Location = new Point(112, 72);
        txtDosis.Name = "txtDosis";
        txtDosis.Size = new Size(220, 23);
        // 
        // lblPrecio
        // 
        lblPrecio.Location = new Point(352, 72);
        lblPrecio.Name = "lblPrecio";
        lblPrecio.Size = new Size(86, 23);
        lblPrecio.Text = "Precio";
        lblPrecio.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // txtPrecio
        // 
        txtPrecio.BackColor = Color.White;
        txtPrecio.BorderStyle = BorderStyle.FixedSingle;
        txtPrecio.Location = new Point(444, 72);
        txtPrecio.Name = "txtPrecio";
        txtPrecio.Size = new Size(160, 23);
        // 
        // lblEstado
        // 
        lblEstado.Location = new Point(628, 72);
        lblEstado.Name = "lblEstado";
        lblEstado.Size = new Size(60, 23);
        lblEstado.Text = "Estado";
        lblEstado.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // cboEstado
        // 
        cboEstado.BackColor = Color.White;
        cboEstado.DropDownStyle = ComboBoxStyle.DropDownList;
        cboEstado.FormattingEnabled = true;
        cboEstado.Location = new Point(694, 72);
        cboEstado.Name = "cboEstado";
        cboEstado.Size = new Size(350, 23);
        // 
        // pnlBotonesDatos
        // 
        pnlBotonesDatos.Controls.Add(btnNuevo);
        pnlBotonesDatos.Controls.Add(btnGuardar);
        pnlBotonesDatos.Controls.Add(btnModificar);
        pnlBotonesDatos.Controls.Add(btnCancelar);
        pnlBotonesDatos.Location = new Point(16, 138);
        pnlBotonesDatos.Name = "pnlBotonesDatos";
        pnlBotonesDatos.Size = new Size(1068, 40);
        pnlBotonesDatos.TabIndex = 1;
        // 
        // btnNuevo
        // 
        btnNuevo.BackColor = Color.FromArgb(230, 196, 202);
        btnNuevo.FlatAppearance.BorderColor = Color.FromArgb(186, 162, 168);
        btnNuevo.FlatStyle = FlatStyle.Flat;
        btnNuevo.ForeColor = Color.FromArgb(58, 53, 59);
        btnNuevo.Location = new Point(0, 4);
        btnNuevo.Name = "btnNuevo";
        btnNuevo.Size = new Size(100, 32);
        btnNuevo.Text = "Nuevo";
        btnNuevo.UseVisualStyleBackColor = false;
        // 
        // btnGuardar
        // 
        btnGuardar.BackColor = Color.FromArgb(152, 196, 164);
        btnGuardar.FlatAppearance.BorderColor = Color.FromArgb(112, 158, 124);
        btnGuardar.FlatStyle = FlatStyle.Flat;
        btnGuardar.ForeColor = Color.FromArgb(58, 53, 59);
        btnGuardar.Location = new Point(108, 4);
        btnGuardar.Name = "btnGuardar";
        btnGuardar.Size = new Size(100, 32);
        btnGuardar.Text = "Guardar";
        btnGuardar.UseVisualStyleBackColor = false;
        // 
        // btnModificar
        // 
        btnModificar.BackColor = Color.FromArgb(148, 176, 214);
        btnModificar.FlatAppearance.BorderColor = Color.FromArgb(112, 142, 182);
        btnModificar.FlatStyle = FlatStyle.Flat;
        btnModificar.ForeColor = Color.FromArgb(58, 53, 59);
        btnModificar.Location = new Point(216, 4);
        btnModificar.Name = "btnModificar";
        btnModificar.Size = new Size(100, 32);
        btnModificar.Text = "Modificar";
        btnModificar.UseVisualStyleBackColor = false;
        // 
        // btnCancelar
        // 
        btnCancelar.BackColor = Color.FromArgb(220, 150, 154);
        btnCancelar.FlatAppearance.BorderColor = Color.FromArgb(186, 118, 122);
        btnCancelar.FlatStyle = FlatStyle.Flat;
        btnCancelar.ForeColor = Color.FromArgb(58, 53, 59);
        btnCancelar.Location = new Point(324, 4);
        btnCancelar.Name = "btnCancelar";
        btnCancelar.Size = new Size(100, 32);
        btnCancelar.Text = "Cancelar";
        btnCancelar.UseVisualStyleBackColor = false;
        // 
        // grpAplicados
        // 
        grpAplicados.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
        grpAplicados.Controls.Add(dgvAplicados);
        grpAplicados.Controls.Add(lblConsulta);
        grpAplicados.Controls.Add(cboConsulta);
        grpAplicados.Controls.Add(lblTratamiento);
        grpAplicados.Controls.Add(cboTratamiento);
        grpAplicados.Controls.Add(lblCantidad);
        grpAplicados.Controls.Add(txtCantidad);
        grpAplicados.Controls.Add(lblPrecioUnitario);
        grpAplicados.Controls.Add(txtPrecioUnitario);
        grpAplicados.Controls.Add(lblSubtotal);
        grpAplicados.Controls.Add(txtSubtotal);
        grpAplicados.Controls.Add(lblIndicaciones);
        grpAplicados.Controls.Add(txtIndicaciones);
        grpAplicados.Font = new Font("Segoe UI", 9F);
        grpAplicados.ForeColor = Color.FromArgb(58, 53, 59);
        grpAplicados.Location = new Point(16, 186);
        grpAplicados.Name = "grpAplicados";
        grpAplicados.Size = new Size(1068, 388);
        grpAplicados.TabIndex = 2;
        grpAplicados.TabStop = false;
        grpAplicados.Text = "Tratamientos aplicados";
        // 
        // lblConsulta
        // 
        lblConsulta.Location = new Point(16, 32);
        lblConsulta.Name = "lblConsulta";
        lblConsulta.Size = new Size(90, 23);
        lblConsulta.Text = "Consulta";
        lblConsulta.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // cboConsulta
        // 
        cboConsulta.BackColor = Color.White;
        cboConsulta.DropDownStyle = ComboBoxStyle.DropDownList;
        cboConsulta.FormattingEnabled = true;
        cboConsulta.Location = new Point(112, 32);
        cboConsulta.Name = "cboConsulta";
        cboConsulta.Size = new Size(220, 23);
        // 
        // lblTratamiento
        // 
        lblTratamiento.Location = new Point(352, 32);
        lblTratamiento.Name = "lblTratamiento";
        lblTratamiento.Size = new Size(86, 23);
        lblTratamiento.Text = "Tratamiento";
        lblTratamiento.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // cboTratamiento
        // 
        cboTratamiento.BackColor = Color.White;
        cboTratamiento.DropDownStyle = ComboBoxStyle.DropDownList;
        cboTratamiento.FormattingEnabled = true;
        cboTratamiento.Location = new Point(444, 32);
        cboTratamiento.Name = "cboTratamiento";
        cboTratamiento.Size = new Size(240, 23);
        // 
        // lblCantidad
        // 
        lblCantidad.Location = new Point(704, 32);
        lblCantidad.Name = "lblCantidad";
        lblCantidad.Size = new Size(70, 23);
        lblCantidad.Text = "Cantidad";
        lblCantidad.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // txtCantidad
        // 
        txtCantidad.BackColor = Color.White;
        txtCantidad.BorderStyle = BorderStyle.FixedSingle;
        txtCantidad.Location = new Point(780, 32);
        txtCantidad.Name = "txtCantidad";
        txtCantidad.Size = new Size(264, 23);
        // 
        // lblPrecioUnitario
        // 
        lblPrecioUnitario.Location = new Point(16, 68);
        lblPrecioUnitario.Name = "lblPrecioUnitario";
        lblPrecioUnitario.Size = new Size(90, 23);
        lblPrecioUnitario.Text = "Precio unitario";
        lblPrecioUnitario.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // txtPrecioUnitario
        // 
        txtPrecioUnitario.BackColor = Color.White;
        txtPrecioUnitario.BorderStyle = BorderStyle.FixedSingle;
        txtPrecioUnitario.Location = new Point(112, 68);
        txtPrecioUnitario.Name = "txtPrecioUnitario";
        txtPrecioUnitario.Size = new Size(220, 23);
        // 
        // lblSubtotal
        // 
        lblSubtotal.Location = new Point(352, 68);
        lblSubtotal.Name = "lblSubtotal";
        lblSubtotal.Size = new Size(86, 23);
        lblSubtotal.Text = "Subtotal";
        lblSubtotal.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // txtSubtotal
        // 
        txtSubtotal.BackColor = Color.White;
        txtSubtotal.BorderStyle = BorderStyle.FixedSingle;
        txtSubtotal.Location = new Point(444, 68);
        txtSubtotal.Name = "txtSubtotal";
        txtSubtotal.Size = new Size(160, 23);
        // 
        // lblIndicaciones
        // 
        lblIndicaciones.Location = new Point(628, 68);
        lblIndicaciones.Name = "lblIndicaciones";
        lblIndicaciones.Size = new Size(86, 23);
        lblIndicaciones.Text = "Indicaciones";
        lblIndicaciones.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // txtIndicaciones
        // 
        txtIndicaciones.BackColor = Color.White;
        txtIndicaciones.BorderStyle = BorderStyle.FixedSingle;
        txtIndicaciones.Location = new Point(720, 68);
        txtIndicaciones.Name = "txtIndicaciones";
        txtIndicaciones.Size = new Size(324, 23);
        // 
        // dgvAplicados
        // 
        dgvAplicados.AllowUserToAddRows = false;
        dgvAplicados.AllowUserToDeleteRows = false;
        dgvAplicados.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        dgvAplicados.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        dgvAplicados.BackgroundColor = Color.White;
        dgvAplicados.BorderStyle = BorderStyle.FixedSingle;
        dgvAplicados.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
        dgvAplicados.Columns.AddRange(new DataGridViewColumn[] { colConsulta, colTratamiento, colCantidad, colPrecioUnitario, colSubtotal, colIndicaciones });
        dgvAplicados.Location = new Point(16, 108);
        dgvAplicados.MultiSelect = false;
        dgvAplicados.Name = "dgvAplicados";
        dgvAplicados.ReadOnly = true;
        dgvAplicados.RowHeadersVisible = false;
        dgvAplicados.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        dgvAplicados.Size = new Size(1036, 260);
        // 
        // columnas
        // 
        colConsulta.HeaderText = "Consulta";
        colConsulta.Name = "colConsulta";
        colTratamiento.HeaderText = "Tratamiento";
        colTratamiento.Name = "colTratamiento";
        colCantidad.HeaderText = "Cantidad";
        colCantidad.Name = "colCantidad";
        colPrecioUnitario.HeaderText = "Precio unitario";
        colPrecioUnitario.Name = "colPrecioUnitario";
        colSubtotal.HeaderText = "Subtotal";
        colSubtotal.Name = "colSubtotal";
        colIndicaciones.HeaderText = "Indicaciones";
        colIndicaciones.Name = "colIndicaciones";
        // 
        // btnVolver
        // 
        btnVolver.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
        btnVolver.BackColor = Color.FromArgb(230, 196, 202);
        btnVolver.FlatAppearance.BorderColor = Color.FromArgb(186, 162, 168);
        btnVolver.FlatStyle = FlatStyle.Flat;
        btnVolver.ForeColor = Color.FromArgb(58, 53, 59);
        btnVolver.Location = new Point(16, 582);
        btnVolver.Name = "btnVolver";
        btnVolver.Size = new Size(140, 32);
        btnVolver.Text = "Volver al panel";
        btnVolver.UseVisualStyleBackColor = false;
        btnVolver.Click += btnVolver_Click;
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
        lblInfoEstado.Size = new Size(109, 17);
        lblInfoEstado.Text = "Módulo clínico listo";
        // 
        // FormTratamientos
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
        MinimumSize = new Size(1100, 700);
        Name = "FormTratamientos";
        StartPosition = FormStartPosition.CenterParent;
        Text = "CLÍNICA VETERINARIA — ATENCIÓN CLÍNICA — TRATAMIENTOS";
        Load += FormTratamientos_Load;
        pnlEncabezado.ResumeLayout(false);
        pnlContenido.ResumeLayout(false);
        grpDatos.ResumeLayout(false);
        grpDatos.PerformLayout();
        pnlBotonesDatos.ResumeLayout(false);
        grpAplicados.ResumeLayout(false);
        grpAplicados.PerformLayout();
        ((System.ComponentModel.ISupportInitialize)dgvAplicados).EndInit();
        barraEstado.ResumeLayout(false);
        barraEstado.PerformLayout();
        ResumeLayout(false);
        PerformLayout();
    }

    private Panel pnlEncabezado;
    private Label lblTitulo;
    private Label lblUsuarioSesion;
    private Panel pnlContenido;
    private GroupBox grpDatos;
    private Label lblTipo;
    private ComboBox cboTipo;
    private Label lblDescripcion;
    private TextBox txtDescripcion;
    private Label lblDosis;
    private TextBox txtDosis;
    private Label lblPrecio;
    private TextBox txtPrecio;
    private Label lblEstado;
    private ComboBox cboEstado;
    private Panel pnlBotonesDatos;
    private Button btnNuevo;
    private Button btnGuardar;
    private Button btnModificar;
    private Button btnCancelar;
    private GroupBox grpAplicados;
    private Label lblConsulta;
    private ComboBox cboConsulta;
    private Label lblTratamiento;
    private ComboBox cboTratamiento;
    private Label lblCantidad;
    private TextBox txtCantidad;
    private Label lblPrecioUnitario;
    private TextBox txtPrecioUnitario;
    private Label lblSubtotal;
    private TextBox txtSubtotal;
    private Label lblIndicaciones;
    private TextBox txtIndicaciones;
    private DataGridView dgvAplicados;
    private DataGridViewTextBoxColumn colConsulta;
    private DataGridViewTextBoxColumn colTratamiento;
    private DataGridViewTextBoxColumn colCantidad;
    private DataGridViewTextBoxColumn colPrecioUnitario;
    private DataGridViewTextBoxColumn colSubtotal;
    private DataGridViewTextBoxColumn colIndicaciones;
    private Button btnVolver;
    private StatusStrip barraEstado;
    private ToolStripStatusLabel lblInfoEstado;
}
