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
        DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
        DataGridViewCellStyle dataGridViewCellStyle2 = new DataGridViewCellStyle();
        DataGridViewCellStyle dataGridViewCellStyle3 = new DataGridViewCellStyle();
        pnlEncabezado = new Panel();
        lblTitulo = new Label();
        lblUsuarioSesion = new Label();
        pnlContenido = new Panel();
        pnlBotones = new Panel();
        btnLimpiar = new Button();
        btnVolver = new Button();
        btnAplicar = new Button();
        grpTratamientosAplicados = new GroupBox();
        dgvTratamientosAplicados = new DataGridView();
        colTratamiento = new DataGridViewTextBoxColumn();
        colTipo = new DataGridViewTextBoxColumn();
        colCantidad = new DataGridViewTextBoxColumn();
        colPrecioUnitario = new DataGridViewTextBoxColumn();
        colSubtotal = new DataGridViewTextBoxColumn();
        colIndicaciones = new DataGridViewTextBoxColumn();
        grpAplicarTratamiento = new GroupBox();
        lblMascota = new Label();
        cboMascotas = new ComboBox();
        lblInfoMascota = new Label();
        lblConsulta = new Label();
        cboConsultas = new ComboBox();
        lblTratamiento = new Label();
        cboTratamientos = new ComboBox();
        btnAltaTratamiento = new Button();
        btnModificarTratamiento = new Button();
        lblCantidad = new Label();
        numCantidad = new NumericUpDown();
        lblIndicaciones = new Label();
        txtIndicaciones = new TextBox();
        lblSubtotal = new Label();
        txtSubtotal = new TextBox();
        lblTotal = new Label();
        barraEstado = new StatusStrip();
        lblInfoEstado = new ToolStripStatusLabel();
        pnlEncabezado.SuspendLayout();
        pnlContenido.SuspendLayout();
        pnlBotones.SuspendLayout();
        grpTratamientosAplicados.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)dgvTratamientosAplicados).BeginInit();
        grpAplicarTratamiento.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)numCantidad).BeginInit();
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
        lblUsuarioSesion.Text = "Dr./Dra. Veterinario | Atención Clínica";
        lblUsuarioSesion.TextAlign = ContentAlignment.MiddleRight;
        // 
        // pnlContenido
        // 
        pnlContenido.BackColor = Color.FromArgb(250, 244, 244);
        pnlContenido.Controls.Add(pnlBotones);
        pnlContenido.Controls.Add(grpTratamientosAplicados);
        pnlContenido.Controls.Add(grpAplicarTratamiento);
        pnlContenido.Dock = DockStyle.Fill;
        pnlContenido.Location = new Point(0, 50);
        pnlContenido.Name = "pnlContenido";
        pnlContenido.Size = new Size(1100, 628);
        pnlContenido.TabIndex = 1;
        // 
        // pnlBotones
        // 
        pnlBotones.Controls.Add(btnLimpiar);
        pnlBotones.Controls.Add(btnVolver);
        pnlBotones.Controls.Add(btnAplicar);
        pnlBotones.Location = new Point(16, 207);
        pnlBotones.Name = "pnlBotones";
        pnlBotones.Size = new Size(1068, 40);
        pnlBotones.TabIndex = 18;
        // 
        // btnLimpiar
        // 
        btnLimpiar.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        btnLimpiar.BackColor = Color.FromArgb(226, 217, 220);
        btnLimpiar.Cursor = Cursors.Hand;
        btnLimpiar.FlatAppearance.BorderSize = 0;
        btnLimpiar.FlatStyle = FlatStyle.Flat;
        btnLimpiar.Font = new Font("Segoe UI", 9F);
        btnLimpiar.ForeColor = Color.FromArgb(58, 53, 59);
        btnLimpiar.Location = new Point(159, 3);
        btnLimpiar.Name = "btnLimpiar";
        btnLimpiar.Size = new Size(120, 32);
        btnLimpiar.TabIndex = 16;
        btnLimpiar.Text = "Limpiar";
        btnLimpiar.UseVisualStyleBackColor = false;
        btnLimpiar.Click += btnLimpiar_Click;
        // 
        // btnVolver
        // 
        btnVolver.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
        btnVolver.BackColor = Color.FromArgb(220, 200, 204);
        btnVolver.Cursor = Cursors.Hand;
        btnVolver.FlatAppearance.BorderSize = 0;
        btnVolver.FlatStyle = FlatStyle.Flat;
        btnVolver.Font = new Font("Segoe UI", 9F);
        btnVolver.ForeColor = Color.Black;
        btnVolver.Location = new Point(297, 3);
        btnVolver.Name = "btnVolver";
        btnVolver.Size = new Size(140, 32);
        btnVolver.TabIndex = 2;
        btnVolver.Text = "Volver al panel";
        btnVolver.UseVisualStyleBackColor = false;
        btnVolver.Click += btnVolver_Click;
        // 
        // btnAplicar
        // 
        btnAplicar.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        btnAplicar.BackColor = Color.FromArgb(200, 138, 150);
        btnAplicar.Cursor = Cursors.Hand;
        btnAplicar.FlatAppearance.BorderSize = 0;
        btnAplicar.FlatStyle = FlatStyle.Flat;
        btnAplicar.Font = new Font("Segoe UI", 9F);
        btnAplicar.ForeColor = Color.White;
        btnAplicar.Location = new Point(20, 3);
        btnAplicar.Name = "btnAplicar";
        btnAplicar.Size = new Size(120, 32);
        btnAplicar.TabIndex = 17;
        btnAplicar.Text = "Aplicar";
        btnAplicar.UseVisualStyleBackColor = false;
        btnAplicar.Click += btnAplicar_Click;
        // 
        // grpTratamientosAplicados
        // 
        grpTratamientosAplicados.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        grpTratamientosAplicados.BackColor = Color.FromArgb(250, 244, 244);
        grpTratamientosAplicados.Controls.Add(dgvTratamientosAplicados);
        grpTratamientosAplicados.Font = new Font("Segoe UI", 9.5F);
        grpTratamientosAplicados.ForeColor = Color.FromArgb(58, 53, 59);
        grpTratamientosAplicados.Location = new Point(16, 253);
        grpTratamientosAplicados.Name = "grpTratamientosAplicados";
        grpTratamientosAplicados.Size = new Size(1068, 353);
        grpTratamientosAplicados.TabIndex = 1;
        grpTratamientosAplicados.TabStop = false;
        grpTratamientosAplicados.Text = "Tratamientos Aplicados a la Consulta Activa";
        // 
        // dgvTratamientosAplicados
        // 
        dgvTratamientosAplicados.AllowUserToAddRows = false;
        dgvTratamientosAplicados.AllowUserToDeleteRows = false;
        dataGridViewCellStyle1.BackColor = Color.FromArgb(250, 244, 244);
        dataGridViewCellStyle1.Font = new Font("Segoe UI", 9F);
        dataGridViewCellStyle1.ForeColor = Color.FromArgb(58, 53, 59);
        dataGridViewCellStyle1.SelectionBackColor = Color.FromArgb(226, 217, 220);
        dataGridViewCellStyle1.SelectionForeColor = Color.FromArgb(58, 53, 59);
        dgvTratamientosAplicados.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
        dgvTratamientosAplicados.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        dgvTratamientosAplicados.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        dgvTratamientosAplicados.BackgroundColor = Color.White;
        dataGridViewCellStyle2.BackColor = Color.White;
        dataGridViewCellStyle2.Font = new Font("Segoe UI", 9F);
        dataGridViewCellStyle2.ForeColor = Color.FromArgb(58, 53, 59);
        dataGridViewCellStyle2.SelectionBackColor = Color.White;
        dataGridViewCellStyle2.SelectionForeColor = Color.FromArgb(58, 53, 59);
        dataGridViewCellStyle2.WrapMode = DataGridViewTriState.True;
        dgvTratamientosAplicados.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
        dgvTratamientosAplicados.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
        dgvTratamientosAplicados.Columns.AddRange(new DataGridViewColumn[] { colTratamiento, colTipo, colCantidad, colPrecioUnitario, colSubtotal, colIndicaciones });
        dataGridViewCellStyle3.Alignment = DataGridViewContentAlignment.MiddleLeft;
        dataGridViewCellStyle3.BackColor = Color.White;
        dataGridViewCellStyle3.Font = new Font("Segoe UI", 9F);
        dataGridViewCellStyle3.ForeColor = Color.FromArgb(58, 53, 59);
        dataGridViewCellStyle3.SelectionBackColor = Color.FromArgb(226, 217, 220);
        dataGridViewCellStyle3.SelectionForeColor = Color.FromArgb(58, 53, 59);
        dataGridViewCellStyle3.WrapMode = DataGridViewTriState.False;
        dgvTratamientosAplicados.DefaultCellStyle = dataGridViewCellStyle3;
        dgvTratamientosAplicados.EnableHeadersVisualStyles = false;
        dgvTratamientosAplicados.GridColor = Color.FromArgb(226, 217, 220);
        dgvTratamientosAplicados.Location = new Point(16, 28);
        dgvTratamientosAplicados.MultiSelect = false;
        dgvTratamientosAplicados.Name = "dgvTratamientosAplicados";
        dgvTratamientosAplicados.ReadOnly = true;
        dgvTratamientosAplicados.RowHeadersVisible = false;
        dgvTratamientosAplicados.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        dgvTratamientosAplicados.Size = new Size(1036, 308);
        dgvTratamientosAplicados.TabIndex = 0;
        // 
        // colTratamiento
        // 
        colTratamiento.HeaderText = "Tratamiento";
        colTratamiento.MinimumWidth = 200;
        colTratamiento.Name = "colTratamiento";
        colTratamiento.ReadOnly = true;
        // 
        // colTipo
        // 
        colTipo.FillWeight = 60F;
        colTipo.HeaderText = "Tipo";
        colTipo.MinimumWidth = 110;
        colTipo.Name = "colTipo";
        colTipo.ReadOnly = true;
        // 
        // colCantidad
        // 
        colCantidad.FillWeight = 40F;
        colCantidad.HeaderText = "Cantidad";
        colCantidad.MinimumWidth = 70;
        colCantidad.Name = "colCantidad";
        colCantidad.ReadOnly = true;
        // 
        // colPrecioUnitario
        // 
        colPrecioUnitario.FillWeight = 50F;
        colPrecioUnitario.HeaderText = "Precio Unit.";
        colPrecioUnitario.MinimumWidth = 90;
        colPrecioUnitario.Name = "colPrecioUnitario";
        colPrecioUnitario.ReadOnly = true;
        // 
        // colSubtotal
        // 
        colSubtotal.FillWeight = 50F;
        colSubtotal.HeaderText = "Subtotal";
        colSubtotal.MinimumWidth = 90;
        colSubtotal.Name = "colSubtotal";
        colSubtotal.ReadOnly = true;
        // 
        // colIndicaciones
        // 
        colIndicaciones.FillWeight = 90F;
        colIndicaciones.HeaderText = "Indicaciones Clínicas";
        colIndicaciones.MinimumWidth = 150;
        colIndicaciones.Name = "colIndicaciones";
        colIndicaciones.ReadOnly = true;
        // 
        // grpAplicarTratamiento
        // 
        grpAplicarTratamiento.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
        grpAplicarTratamiento.BackColor = Color.FromArgb(250, 244, 244);
        grpAplicarTratamiento.Controls.Add(lblMascota);
        grpAplicarTratamiento.Controls.Add(cboMascotas);
        grpAplicarTratamiento.Controls.Add(lblInfoMascota);
        grpAplicarTratamiento.Controls.Add(lblConsulta);
        grpAplicarTratamiento.Controls.Add(cboConsultas);
        grpAplicarTratamiento.Controls.Add(lblTratamiento);
        grpAplicarTratamiento.Controls.Add(cboTratamientos);
        grpAplicarTratamiento.Controls.Add(btnAltaTratamiento);
        grpAplicarTratamiento.Controls.Add(btnModificarTratamiento);
        grpAplicarTratamiento.Controls.Add(lblCantidad);
        grpAplicarTratamiento.Controls.Add(numCantidad);
        grpAplicarTratamiento.Controls.Add(lblIndicaciones);
        grpAplicarTratamiento.Controls.Add(txtIndicaciones);
        grpAplicarTratamiento.Controls.Add(lblSubtotal);
        grpAplicarTratamiento.Controls.Add(txtSubtotal);
        grpAplicarTratamiento.Controls.Add(lblTotal);
        grpAplicarTratamiento.Font = new Font("Segoe UI", 9.5F);
        grpAplicarTratamiento.ForeColor = Color.FromArgb(58, 53, 59);
        grpAplicarTratamiento.Location = new Point(16, 10);
        grpAplicarTratamiento.Name = "grpAplicarTratamiento";
        grpAplicarTratamiento.Size = new Size(1068, 191);
        grpAplicarTratamiento.TabIndex = 0;
        grpAplicarTratamiento.TabStop = false;
        grpAplicarTratamiento.Text = "Aplicar Tratamiento";
        // 
        // lblMascota
        // 
        lblMascota.Font = new Font("Segoe UI", 9F);
        lblMascota.ForeColor = Color.FromArgb(58, 53, 59);
        lblMascota.Location = new Point(20, 24);
        lblMascota.Name = "lblMascota";
        lblMascota.Size = new Size(500, 18);
        lblMascota.TabIndex = 0;
        lblMascota.Text = "Paciente / Mascota:";
        // 
        // cboMascotas
        // 
        cboMascotas.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
        cboMascotas.AutoCompleteSource = AutoCompleteSource.ListItems;
        cboMascotas.BackColor = Color.White;
        cboMascotas.Font = new Font("Segoe UI", 9F);
        cboMascotas.ForeColor = Color.FromArgb(58, 53, 59);
        cboMascotas.FormattingEnabled = true;
        cboMascotas.Location = new Point(20, 44);
        cboMascotas.Name = "cboMascotas";
        cboMascotas.Size = new Size(500, 23);
        cboMascotas.TabIndex = 1;
        cboMascotas.SelectedIndexChanged += cboMascotas_SelectedIndexChanged;
        // 
        // lblInfoMascota
        // 
        lblInfoMascota.AutoEllipsis = true;
        lblInfoMascota.BackColor = Color.FromArgb(244, 236, 238);
        lblInfoMascota.BorderStyle = BorderStyle.FixedSingle;
        lblInfoMascota.Font = new Font("Segoe UI", 8.5F);
        lblInfoMascota.ForeColor = Color.FromArgb(58, 53, 59);
        lblInfoMascota.Location = new Point(20, 72);
        lblInfoMascota.Name = "lblInfoMascota";
        lblInfoMascota.Padding = new Padding(6, 0, 6, 0);
        lblInfoMascota.Size = new Size(500, 24);
        lblInfoMascota.TabIndex = 2;
        lblInfoMascota.Text = "Especie: — | Raza: — | Propietario: —";
        lblInfoMascota.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // lblConsulta
        // 
        lblConsulta.Font = new Font("Segoe UI", 9F);
        lblConsulta.ForeColor = Color.FromArgb(58, 53, 59);
        lblConsulta.Location = new Point(20, 102);
        lblConsulta.Name = "lblConsulta";
        lblConsulta.Size = new Size(500, 18);
        lblConsulta.TabIndex = 3;
        lblConsulta.Text = "Consulta Clínica:";
        // 
        // cboConsultas
        // 
        cboConsultas.BackColor = Color.White;
        cboConsultas.DropDownStyle = ComboBoxStyle.DropDownList;
        cboConsultas.Font = new Font("Segoe UI", 9F);
        cboConsultas.ForeColor = Color.FromArgb(58, 53, 59);
        cboConsultas.FormattingEnabled = true;
        cboConsultas.Location = new Point(20, 122);
        cboConsultas.Name = "cboConsultas";
        cboConsultas.Size = new Size(500, 23);
        cboConsultas.TabIndex = 4;
        cboConsultas.SelectedIndexChanged += cboConsultas_SelectedIndexChanged;
        // 
        // lblTratamiento
        // 
        lblTratamiento.Font = new Font("Segoe UI", 9F);
        lblTratamiento.ForeColor = Color.FromArgb(58, 53, 59);
        lblTratamiento.Location = new Point(544, 24);
        lblTratamiento.Name = "lblTratamiento";
        lblTratamiento.Size = new Size(500, 18);
        lblTratamiento.TabIndex = 5;
        lblTratamiento.Text = "Tratamiento (Catálogo activo):";
        // 
        // cboTratamientos
        // 
        cboTratamientos.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
        cboTratamientos.AutoCompleteSource = AutoCompleteSource.ListItems;
        cboTratamientos.BackColor = Color.White;
        cboTratamientos.Font = new Font("Segoe UI", 9F);
        cboTratamientos.ForeColor = Color.FromArgb(58, 53, 59);
        cboTratamientos.FormattingEnabled = true;
        cboTratamientos.Location = new Point(544, 44);
        cboTratamientos.Name = "cboTratamientos";
        cboTratamientos.Size = new Size(500, 23);
        cboTratamientos.TabIndex = 6;
        cboTratamientos.SelectedIndexChanged += cboTratamientos_SelectedIndexChanged;
        // 
        // btnAltaTratamiento
        // 
        btnAltaTratamiento.BackColor = Color.FromArgb(200, 138, 150);
        btnAltaTratamiento.Cursor = Cursors.Hand;
        btnAltaTratamiento.FlatAppearance.BorderSize = 0;
        btnAltaTratamiento.FlatStyle = FlatStyle.Flat;
        btnAltaTratamiento.Font = new Font("Segoe UI", 9F);
        btnAltaTratamiento.ForeColor = Color.White;
        btnAltaTratamiento.Location = new Point(544, 73);
        btnAltaTratamiento.Name = "btnAltaTratamiento";
        btnAltaTratamiento.Size = new Size(120, 28);
        btnAltaTratamiento.TabIndex = 7;
        btnAltaTratamiento.Text = "Alta Trat.";
        btnAltaTratamiento.UseVisualStyleBackColor = false;
        btnAltaTratamiento.Click += btnAltaTratamiento_Click;
        // 
        // btnModificarTratamiento
        // 
        btnModificarTratamiento.BackColor = Color.FromArgb(226, 217, 220);
        btnModificarTratamiento.Cursor = Cursors.Hand;
        btnModificarTratamiento.FlatAppearance.BorderSize = 0;
        btnModificarTratamiento.FlatStyle = FlatStyle.Flat;
        btnModificarTratamiento.Font = new Font("Segoe UI", 9F);
        btnModificarTratamiento.ForeColor = Color.Black;
        btnModificarTratamiento.Location = new Point(672, 73);
        btnModificarTratamiento.Name = "btnModificarTratamiento";
        btnModificarTratamiento.Size = new Size(120, 28);
        btnModificarTratamiento.TabIndex = 8;
        btnModificarTratamiento.Text = "Mod. Trat.";
        btnModificarTratamiento.UseVisualStyleBackColor = false;
        btnModificarTratamiento.Click += btnModificarTratamiento_Click;
        // 
        // lblCantidad
        // 
        lblCantidad.Font = new Font("Segoe UI", 9F);
        lblCantidad.ForeColor = Color.FromArgb(58, 53, 59);
        lblCantidad.Location = new Point(20, 155);
        lblCantidad.Name = "lblCantidad";
        lblCantidad.Size = new Size(62, 23);
        lblCantidad.TabIndex = 9;
        lblCantidad.Text = "Cantidad:";
        lblCantidad.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // numCantidad
        // 
        numCantidad.BackColor = Color.White;
        numCantidad.Font = new Font("Segoe UI", 9.5F);
        numCantidad.ForeColor = Color.FromArgb(58, 53, 59);
        numCantidad.Location = new Point(86, 155);
        numCantidad.Maximum = new decimal(new int[] { 9999, 0, 0, 0 });
        numCantidad.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
        numCantidad.Name = "numCantidad";
        numCantidad.Size = new Size(65, 24);
        numCantidad.TabIndex = 10;
        numCantidad.TextAlign = HorizontalAlignment.Right;
        numCantidad.Value = new decimal(new int[] { 1, 0, 0, 0 });
        numCantidad.ValueChanged += numCantidad_ValueChanged;
        // 
        // lblIndicaciones
        // 
        lblIndicaciones.Font = new Font("Segoe UI", 9F);
        lblIndicaciones.ForeColor = Color.FromArgb(58, 53, 59);
        lblIndicaciones.Location = new Point(544, 122);
        lblIndicaciones.Name = "lblIndicaciones";
        lblIndicaciones.Size = new Size(168, 23);
        lblIndicaciones.TabIndex = 11;
        lblIndicaciones.Text = "Indicaciones / Dosis clínica:";
        lblIndicaciones.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // txtIndicaciones
        // 
        txtIndicaciones.BackColor = Color.White;
        txtIndicaciones.BorderStyle = BorderStyle.FixedSingle;
        txtIndicaciones.Font = new Font("Segoe UI", 9.5F);
        txtIndicaciones.ForeColor = Color.FromArgb(58, 53, 59);
        txtIndicaciones.Location = new Point(718, 121);
        txtIndicaciones.MaxLength = 300;
        txtIndicaciones.Name = "txtIndicaciones";
        txtIndicaciones.Size = new Size(326, 24);
        txtIndicaciones.TabIndex = 12;
        // 
        // lblSubtotal
        // 
        lblSubtotal.Font = new Font("Segoe UI", 9F);
        lblSubtotal.ForeColor = Color.FromArgb(58, 53, 59);
        lblSubtotal.Location = new Point(198, 155);
        lblSubtotal.Name = "lblSubtotal";
        lblSubtotal.Size = new Size(58, 23);
        lblSubtotal.TabIndex = 13;
        lblSubtotal.Text = "Subtotal:";
        lblSubtotal.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // txtSubtotal
        // 
        txtSubtotal.BackColor = Color.FromArgb(249, 240, 242);
        txtSubtotal.BorderStyle = BorderStyle.FixedSingle;
        txtSubtotal.Font = new Font("Segoe UI", 9.5F);
        txtSubtotal.ForeColor = Color.FromArgb(58, 53, 59);
        txtSubtotal.Location = new Point(260, 155);
        txtSubtotal.Name = "txtSubtotal";
        txtSubtotal.ReadOnly = true;
        txtSubtotal.Size = new Size(118, 24);
        txtSubtotal.TabIndex = 14;
        txtSubtotal.Text = "$ 0,00";
        txtSubtotal.TextAlign = HorizontalAlignment.Right;
        // 
        // lblTotal
        // 
        lblTotal.Font = new Font("Segoe UI", 11F);
        lblTotal.ForeColor = Color.FromArgb(58, 53, 59);
        lblTotal.Location = new Point(942, 158);
        lblTotal.Name = "lblTotal";
        lblTotal.Size = new Size(120, 26);
        lblTotal.TabIndex = 15;
        lblTotal.Text = "Total: $ 0,00";
        lblTotal.TextAlign = ContentAlignment.MiddleLeft;
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
        pnlBotones.ResumeLayout(false);
        grpTratamientosAplicados.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)dgvTratamientosAplicados).EndInit();
        grpAplicarTratamiento.ResumeLayout(false);
        grpAplicarTratamiento.PerformLayout();
        ((System.ComponentModel.ISupportInitialize)numCantidad).EndInit();
        barraEstado.ResumeLayout(false);
        barraEstado.PerformLayout();
        ResumeLayout(false);
        PerformLayout();
    }

    private Panel pnlEncabezado;
    private Label lblTitulo;
    private Label lblUsuarioSesion;
    private Panel pnlContenido;
    private GroupBox grpAplicarTratamiento;
    private Label lblMascota;
    private ComboBox cboMascotas;
    private Label lblInfoMascota;
    private Label lblConsulta;
    private ComboBox cboConsultas;
    private Label lblTratamiento;
    private ComboBox cboTratamientos;
    private Button btnAltaTratamiento;
    private Button btnModificarTratamiento;
    private Label lblCantidad;
    private NumericUpDown numCantidad;
    private Label lblIndicaciones;
    private TextBox txtIndicaciones;
    private TextBox txtSubtotal;
    private Label lblTotal;
    private Button btnLimpiar;
    private Button btnAplicar;
    private GroupBox grpTratamientosAplicados;
    private DataGridView dgvTratamientosAplicados;
    private DataGridViewTextBoxColumn colTratamiento;
    private DataGridViewTextBoxColumn colTipo;
    private DataGridViewTextBoxColumn colCantidad;
    private DataGridViewTextBoxColumn colPrecioUnitario;
    private DataGridViewTextBoxColumn colSubtotal;
    private DataGridViewTextBoxColumn colIndicaciones;
    private Button btnVolver;
    private StatusStrip barraEstado;
    private ToolStripStatusLabel lblInfoEstado;
    private Label lblSubtotal;
    private Panel pnlBotones;
}
