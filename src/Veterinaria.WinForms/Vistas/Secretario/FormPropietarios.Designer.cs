namespace Veterinaria.WinForms.Vistas.Secretario;

partial class FormPropietarios
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
        DataGridViewCellStyle dataGridViewCellStyle3 = new DataGridViewCellStyle();
        DataGridViewCellStyle dataGridViewCellStyle4 = new DataGridViewCellStyle();
        pnlEncabezado = new Panel();
        lblTitulo = new Label();
        lblUsuarioSesion = new Label();
        pnlContenido = new Panel();
        grpDatos = new GroupBox();
        lblDni = new Label();
        txtDni = new TextBox();
        lblNombre = new Label();
        txtNombre = new TextBox();
        lblApellido = new Label();
        txtApellido = new TextBox();
        lblTelefono = new Label();
        txtTelefono = new TextBox();
        lblCorreoElectronico = new Label();
        txtCorreoElectronico = new TextBox();
        lblDireccion = new Label();
        txtDireccion = new TextBox();
        pnlListado = new Panel();
        lblBuscar = new Label();
        txtBuscar = new TextBox();
        btnBuscar = new Button();
        btnActivos = new Button();
        btnInactivos = new Button();
        dgvPropietarios = new DataGridView();
        colId = new DataGridViewTextBoxColumn();
        colDni = new DataGridViewTextBoxColumn();
        colNombre = new DataGridViewTextBoxColumn();
        colApellido = new DataGridViewTextBoxColumn();
        colTelefono = new DataGridViewTextBoxColumn();
        colCorreoElectronico = new DataGridViewTextBoxColumn();
        colDireccion = new DataGridViewTextBoxColumn();
        colEstado = new DataGridViewTextBoxColumn();
        barraEstado = new StatusStrip();
        lblInfoEstado = new ToolStripStatusLabel();
        pnlBotones = new Panel();
        btnGuardar = new Button();
        btnModificar = new Button();
        btnCancelar = new Button();
        pnlEncabezado.SuspendLayout();
        pnlContenido.SuspendLayout();
        grpDatos.SuspendLayout();
        pnlListado.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)dgvPropietarios).BeginInit();
        barraEstado.SuspendLayout();
        pnlBotones.SuspendLayout();
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
        lblTitulo.Text = "CLÍNICA VETERINARIA — RECEPCIÓN — GESTIÓN DE PROPIETARIOS";
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
        pnlContenido.Controls.Add(pnlBotones);
        pnlContenido.Controls.Add(grpDatos);
        pnlContenido.Controls.Add(pnlListado);
        pnlContenido.Dock = DockStyle.Fill;
        pnlContenido.Location = new Point(0, 50);
        pnlContenido.Name = "pnlContenido";
        pnlContenido.Size = new Size(1100, 628);
        pnlContenido.TabIndex = 1;
        // 
        // grpDatos
        // 
        grpDatos.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
        grpDatos.Controls.Add(lblDni);
        grpDatos.Controls.Add(txtDni);
        grpDatos.Controls.Add(lblNombre);
        grpDatos.Controls.Add(txtNombre);
        grpDatos.Controls.Add(lblApellido);
        grpDatos.Controls.Add(txtApellido);
        grpDatos.Controls.Add(lblTelefono);
        grpDatos.Controls.Add(txtTelefono);
        grpDatos.Controls.Add(lblCorreoElectronico);
        grpDatos.Controls.Add(txtCorreoElectronico);
        grpDatos.Controls.Add(lblDireccion);
        grpDatos.Controls.Add(txtDireccion);
        grpDatos.Font = new Font("Segoe UI", 9F);
        grpDatos.ForeColor = Color.FromArgb(58, 53, 59);
        grpDatos.Location = new Point(16, 16);
        grpDatos.Name = "grpDatos";
        grpDatos.Size = new Size(1072, 177);
        grpDatos.TabIndex = 0;
        grpDatos.TabStop = false;
        grpDatos.Text = "Datos del propietario";
        // 
        // lblDni
        // 
        lblDni.Location = new Point(16, 40);
        lblDni.Name = "lblDni";
        lblDni.Size = new Size(118, 23);
        lblDni.TabIndex = 0;
        lblDni.Text = "DNI";
        lblDni.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // txtDni
        // 
        txtDni.BackColor = Color.White;
        txtDni.BorderStyle = BorderStyle.FixedSingle;
        txtDni.Location = new Point(140, 42);
        txtDni.Name = "txtDni";
        txtDni.Size = new Size(356, 23);
        txtDni.TabIndex = 1;
        // 
        // lblNombre
        // 
        lblNombre.Location = new Point(16, 84);
        lblNombre.Name = "lblNombre";
        lblNombre.Size = new Size(118, 23);
        lblNombre.TabIndex = 2;
        lblNombre.Text = "Nombre";
        lblNombre.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // txtNombre
        // 
        txtNombre.BackColor = Color.White;
        txtNombre.BorderStyle = BorderStyle.FixedSingle;
        txtNombre.Location = new Point(140, 84);
        txtNombre.Name = "txtNombre";
        txtNombre.Size = new Size(356, 23);
        txtNombre.TabIndex = 3;
        // 
        // lblApellido
        // 
        lblApellido.Location = new Point(16, 128);
        lblApellido.Name = "lblApellido";
        lblApellido.Size = new Size(118, 23);
        lblApellido.TabIndex = 4;
        lblApellido.Text = "Apellido";
        lblApellido.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // txtApellido
        // 
        txtApellido.BackColor = Color.White;
        txtApellido.BorderStyle = BorderStyle.FixedSingle;
        txtApellido.Location = new Point(140, 128);
        txtApellido.Name = "txtApellido";
        txtApellido.Size = new Size(356, 23);
        txtApellido.TabIndex = 5;
        // 
        // lblTelefono
        // 
        lblTelefono.Location = new Point(547, 40);
        lblTelefono.Name = "lblTelefono";
        lblTelefono.Size = new Size(118, 23);
        lblTelefono.TabIndex = 6;
        lblTelefono.Text = "Teléfono";
        lblTelefono.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // txtTelefono
        // 
        txtTelefono.BackColor = Color.White;
        txtTelefono.BorderStyle = BorderStyle.FixedSingle;
        txtTelefono.Location = new Point(671, 40);
        txtTelefono.Name = "txtTelefono";
        txtTelefono.Size = new Size(371, 23);
        txtTelefono.TabIndex = 7;
        // 
        // lblCorreoElectronico
        // 
        lblCorreoElectronico.Location = new Point(547, 84);
        lblCorreoElectronico.Name = "lblCorreoElectronico";
        lblCorreoElectronico.Size = new Size(118, 23);
        lblCorreoElectronico.TabIndex = 8;
        lblCorreoElectronico.Text = "Correo Electrónico";
        lblCorreoElectronico.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // txtCorreoElectronico
        // 
        txtCorreoElectronico.BackColor = Color.White;
        txtCorreoElectronico.BorderStyle = BorderStyle.FixedSingle;
        txtCorreoElectronico.Location = new Point(671, 84);
        txtCorreoElectronico.Name = "txtCorreoElectronico";
        txtCorreoElectronico.Size = new Size(371, 23);
        txtCorreoElectronico.TabIndex = 9;
        // 
        // lblDireccion
        // 
        lblDireccion.Location = new Point(547, 128);
        lblDireccion.Name = "lblDireccion";
        lblDireccion.Size = new Size(118, 23);
        lblDireccion.TabIndex = 10;
        lblDireccion.Text = "Dirección";
        lblDireccion.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // txtDireccion
        // 
        txtDireccion.BackColor = Color.White;
        txtDireccion.BorderStyle = BorderStyle.FixedSingle;
        txtDireccion.Location = new Point(671, 128);
        txtDireccion.Name = "txtDireccion";
        txtDireccion.Size = new Size(371, 23);
        txtDireccion.TabIndex = 11;
        // 
        // pnlListado
        // 
        pnlListado.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        pnlListado.BackColor = Color.White;
        pnlListado.BorderStyle = BorderStyle.FixedSingle;
        pnlListado.Controls.Add(lblBuscar);
        pnlListado.Controls.Add(txtBuscar);
        pnlListado.Controls.Add(btnBuscar);
        pnlListado.Controls.Add(btnActivos);
        pnlListado.Controls.Add(btnInactivos);
        pnlListado.Controls.Add(dgvPropietarios);
        pnlListado.Location = new Point(12, 245);
        pnlListado.Name = "pnlListado";
        pnlListado.Size = new Size(1076, 380);
        pnlListado.TabIndex = 2;
        // 
        // lblBuscar
        // 
        lblBuscar.Location = new Point(10, 12);
        lblBuscar.Name = "lblBuscar";
        lblBuscar.Size = new Size(52, 23);
        lblBuscar.TabIndex = 0;
        lblBuscar.Text = "Buscar";
        lblBuscar.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // txtBuscar
        // 
        txtBuscar.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
        txtBuscar.BackColor = Color.White;
        txtBuscar.BorderStyle = BorderStyle.FixedSingle;
        txtBuscar.Location = new Point(64, 12);
        txtBuscar.Name = "txtBuscar";
        txtBuscar.Size = new Size(678, 23);
        txtBuscar.TabIndex = 1;
        // 
        // btnBuscar
        // 
        btnBuscar.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        btnBuscar.BackColor = Color.FromArgb(226, 217, 220);
        btnBuscar.Cursor = Cursors.Hand;
        btnBuscar.FlatAppearance.BorderColor = Color.FromArgb(186, 162, 168);
        btnBuscar.FlatStyle = FlatStyle.Flat;
        btnBuscar.ForeColor = Color.FromArgb(58, 53, 59);
        btnBuscar.Location = new Point(748, 10);
        btnBuscar.Name = "btnBuscar";
        btnBuscar.Size = new Size(90, 27);
        btnBuscar.TabIndex = 2;
        btnBuscar.Text = "Buscar";
        btnBuscar.UseVisualStyleBackColor = false;
        // 
        // btnActivos
        // 
        btnActivos.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        btnActivos.BackColor = Color.FromArgb(226, 217, 220);
        btnActivos.Cursor = Cursors.Hand;
        btnActivos.FlatAppearance.BorderColor = Color.FromArgb(186, 162, 168);
        btnActivos.FlatStyle = FlatStyle.Flat;
        btnActivos.ForeColor = Color.FromArgb(58, 53, 59);
        btnActivos.Location = new Point(844, 10);
        btnActivos.Name = "btnActivos";
        btnActivos.Size = new Size(104, 27);
        btnActivos.TabIndex = 3;
        btnActivos.Text = "Activos";
        btnActivos.UseVisualStyleBackColor = false;
        // 
        // btnInactivos
        // 
        btnInactivos.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        btnInactivos.BackColor = Color.FromArgb(226, 217, 220);
        btnInactivos.Cursor = Cursors.Hand;
        btnInactivos.FlatAppearance.BorderColor = Color.FromArgb(186, 162, 168);
        btnInactivos.FlatStyle = FlatStyle.Flat;
        btnInactivos.ForeColor = Color.FromArgb(58, 53, 59);
        btnInactivos.Location = new Point(954, 10);
        btnInactivos.Name = "btnInactivos";
        btnInactivos.Size = new Size(110, 27);
        btnInactivos.TabIndex = 4;
        btnInactivos.Text = "Inactivos";
        btnInactivos.UseVisualStyleBackColor = false;
        // 
        // dgvPropietarios
        // 
        dgvPropietarios.AllowUserToAddRows = false;
        dgvPropietarios.AllowUserToDeleteRows = false;
        dgvPropietarios.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        dgvPropietarios.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        dgvPropietarios.BackgroundColor = Color.White;
        dataGridViewCellStyle3.Alignment = DataGridViewContentAlignment.MiddleLeft;
        dataGridViewCellStyle3.BackColor = SystemColors.Control;
        dataGridViewCellStyle3.Font = new Font("Segoe UI", 9F);
        dataGridViewCellStyle3.ForeColor = SystemColors.WindowText;
        dataGridViewCellStyle3.SelectionBackColor = SystemColors.Highlight;
        dataGridViewCellStyle3.SelectionForeColor = SystemColors.HighlightText;
        dataGridViewCellStyle3.WrapMode = DataGridViewTriState.True;
        dgvPropietarios.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle3;
        dgvPropietarios.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
        dgvPropietarios.Columns.AddRange(new DataGridViewColumn[] { colId, colDni, colNombre, colApellido, colTelefono, colCorreoElectronico, colDireccion, colEstado });
        dataGridViewCellStyle4.Alignment = DataGridViewContentAlignment.MiddleLeft;
        dataGridViewCellStyle4.BackColor = SystemColors.Window;
        dataGridViewCellStyle4.Font = new Font("Segoe UI", 9F);
        dataGridViewCellStyle4.ForeColor = Color.FromArgb(58, 53, 59);
        dataGridViewCellStyle4.SelectionBackColor = SystemColors.Highlight;
        dataGridViewCellStyle4.SelectionForeColor = SystemColors.HighlightText;
        dataGridViewCellStyle4.WrapMode = DataGridViewTriState.False;
        dgvPropietarios.DefaultCellStyle = dataGridViewCellStyle4;
        dgvPropietarios.Location = new Point(10, 48);
        dgvPropietarios.MultiSelect = false;
        dgvPropietarios.Name = "dgvPropietarios";
        dgvPropietarios.ReadOnly = true;
        dgvPropietarios.RowHeadersVisible = false;
        dgvPropietarios.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        dgvPropietarios.Size = new Size(1054, 320);
        dgvPropietarios.TabIndex = 5;
        // 
        // colId
        // 
        colId.HeaderText = "ID";
        colId.Name = "colId";
        colId.ReadOnly = true;
        // 
        // colDni
        // 
        colDni.HeaderText = "DNI";
        colDni.Name = "colDni";
        colDni.ReadOnly = true;
        // 
        // colNombre
        // 
        colNombre.HeaderText = "Nombre";
        colNombre.Name = "colNombre";
        colNombre.ReadOnly = true;
        // 
        // colApellido
        // 
        colApellido.HeaderText = "Apellido";
        colApellido.Name = "colApellido";
        colApellido.ReadOnly = true;
        // 
        // colTelefono
        // 
        colTelefono.HeaderText = "Teléfono";
        colTelefono.Name = "colTelefono";
        colTelefono.ReadOnly = true;
        // 
        // colCorreoElectronico
        // 
        colCorreoElectronico.HeaderText = "Correo Electrónico";
        colCorreoElectronico.Name = "colCorreoElectronico";
        colCorreoElectronico.ReadOnly = true;
        // 
        // colDireccion
        // 
        colDireccion.HeaderText = "Dirección";
        colDireccion.Name = "colDireccion";
        colDireccion.ReadOnly = true;
        // 
        // colEstado
        // 
        colEstado.HeaderText = "Estado";
        colEstado.Name = "colEstado";
        colEstado.ReadOnly = true;
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
        lblInfoEstado.Size = new Size(213, 17);
        lblInfoEstado.Text = "Módulo de gestión de propietarios listo";
        // 
        // pnlBotones
        // 
        pnlBotones.Controls.Add(btnGuardar);
        pnlBotones.Controls.Add(btnModificar);
        pnlBotones.Controls.Add(btnCancelar);
        pnlBotones.Location = new Point(12, 199);
        pnlBotones.Name = "pnlBotones";
        pnlBotones.Size = new Size(1076, 40);
        pnlBotones.TabIndex = 4;
        // 
        // btnGuardar
        // 
        btnGuardar.BackColor = Color.FromArgb(152, 196, 164);
        btnGuardar.FlatAppearance.BorderColor = Color.FromArgb(112, 158, 124);
        btnGuardar.FlatStyle = FlatStyle.Flat;
        btnGuardar.ForeColor = Color.FromArgb(58, 53, 59);
        btnGuardar.Location = new Point(7, 5);
        btnGuardar.Name = "btnGuardar";
        btnGuardar.Size = new Size(100, 32);
        btnGuardar.TabIndex = 1;
        btnGuardar.Text = "Guardar";
        btnGuardar.UseVisualStyleBackColor = false;
        // 
        // btnModificar
        // 
        btnModificar.BackColor = Color.FromArgb(148, 176, 214);
        btnModificar.FlatAppearance.BorderColor = Color.FromArgb(112, 142, 182);
        btnModificar.FlatStyle = FlatStyle.Flat;
        btnModificar.ForeColor = Color.FromArgb(58, 53, 59);
        btnModificar.Location = new Point(115, 5);
        btnModificar.Name = "btnModificar";
        btnModificar.Size = new Size(100, 32);
        btnModificar.TabIndex = 2;
        btnModificar.Text = "Modificar";
        btnModificar.UseVisualStyleBackColor = false;
        // 
        // btnCancelar
        // 
        btnCancelar.BackColor = Color.FromArgb(220, 150, 154);
        btnCancelar.FlatAppearance.BorderColor = Color.FromArgb(186, 118, 122);
        btnCancelar.FlatStyle = FlatStyle.Flat;
        btnCancelar.ForeColor = Color.FromArgb(58, 53, 59);
        btnCancelar.Location = new Point(223, 5);
        btnCancelar.Name = "btnCancelar";
        btnCancelar.Size = new Size(100, 32);
        btnCancelar.TabIndex = 3;
        btnCancelar.Text = "Cancelar";
        btnCancelar.UseVisualStyleBackColor = false;
        // 
        // FormPropietarios
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
        Name = "FormPropietarios";
        StartPosition = FormStartPosition.CenterParent;
        Text = "CLÍNICA VETERINARIA — RECEPCIÓN — GESTIÓN DE PROPIETARIOS";
        Load += FormPropietarios_Load;
        pnlEncabezado.ResumeLayout(false);
        pnlContenido.ResumeLayout(false);
        grpDatos.ResumeLayout(false);
        grpDatos.PerformLayout();
        pnlListado.ResumeLayout(false);
        pnlListado.PerformLayout();
        ((System.ComponentModel.ISupportInitialize)dgvPropietarios).EndInit();
        barraEstado.ResumeLayout(false);
        barraEstado.PerformLayout();
        pnlBotones.ResumeLayout(false);
        ResumeLayout(false);
        PerformLayout();
    }

    private Panel pnlEncabezado;
    private Label lblTitulo;
    private Label lblUsuarioSesion;
    private Panel pnlContenido;
    private GroupBox grpDatos;
    private Label lblDni;
    private TextBox txtDni;
    private Label lblNombre;
    private TextBox txtNombre;
    private Label lblApellido;
    private TextBox txtApellido;
    private Label lblTelefono;
    private TextBox txtTelefono;
    private Label lblCorreoElectronico;
    private TextBox txtCorreoElectronico;
    private Label lblDireccion;
    private TextBox txtDireccion;
    private Panel pnlListado;
    private Label lblBuscar;
    private TextBox txtBuscar;
    private Button btnBuscar;
    private Button btnActivos;
    private Button btnInactivos;
    private DataGridView dgvPropietarios;
    private DataGridViewTextBoxColumn colId;
    private DataGridViewTextBoxColumn colDni;
    private DataGridViewTextBoxColumn colNombre;
    private DataGridViewTextBoxColumn colApellido;
    private DataGridViewTextBoxColumn colTelefono;
    private DataGridViewTextBoxColumn colCorreoElectronico;
    private DataGridViewTextBoxColumn colDireccion;
    private DataGridViewTextBoxColumn colEstado;
    private StatusStrip barraEstado;
    private ToolStripStatusLabel lblInfoEstado;
    private Panel pnlBotones;
    private Button btnGuardar;
    private Button btnModificar;
    private Button btnCancelar;
}
