namespace Veterinaria.WinForms.Vistas.Administrador;

partial class FormBackup
{
    /// <summary>
    /// Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary>
    /// Clean up any resources being used.
    /// </summary>
    /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
    protected override void Dispose(bool disposing)
    {
        if (disposing && (components != null))
        {
            components.Dispose();
        }
        base.Dispose(disposing);
    }

    #region Windows Form Designer generated code

    /// <summary>
    /// Required method for Designer support - do not modify
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
        DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
        DataGridViewCellStyle dataGridViewCellStyle2 = new DataGridViewCellStyle();
        DataGridViewCellStyle dataGridViewCellStyle3 = new DataGridViewCellStyle();
        pnlEncabezado = new Panel();
        lblTitulo = new Label();
        lblUsuarioSesion = new Label();
        pnlContenido = new Panel();
        grpConfiguracion = new GroupBox();
        lblNombreArchivo = new Label();
        txtNombreArchivo = new TextBox();
        lblRutaInfo = new Label();
        btnGenerarBackup = new Button();
        dgvBackups = new DataGridView();
        colNombreArchivo = new DataGridViewTextBoxColumn();
        colFechaHora = new DataGridViewTextBoxColumn();
        colUsuario = new DataGridViewTextBoxColumn();
        pnlInferior = new Panel();
        btnVolver = new Button();
        btnRestaurarCopia = new Button();
        btnExaminarRestaurar = new Button();
        lblTotalBackups = new Label();
        pnlEncabezado.SuspendLayout();

        pnlContenido.SuspendLayout();
        grpConfiguracion.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)dgvBackups).BeginInit();
        pnlInferior.SuspendLayout();
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
        lblTitulo.Size = new Size(720, 48);
        lblTitulo.TabIndex = 0;
        lblTitulo.Text = "CLÍNICA VETERINARIA — ADMINISTRADOR — GESTIÓN DE BACKUPS";
        lblTitulo.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // lblUsuarioSesion
        // 
        lblUsuarioSesion.Dock = DockStyle.Right;
        lblUsuarioSesion.Font = new Font("Segoe UI", 9.75F);
        lblUsuarioSesion.ForeColor = Color.FromArgb(250, 244, 244);
        lblUsuarioSesion.Location = new Point(784, 0);
        lblUsuarioSesion.Name = "lblUsuarioSesion";
        lblUsuarioSesion.Size = new Size(420, 48);
        lblUsuarioSesion.TabIndex = 1;
        lblUsuarioSesion.Text = "Usuario: Admin";
        lblUsuarioSesion.TextAlign = ContentAlignment.MiddleRight;
        // 
        // pnlContenido
        // 
        pnlContenido.BackColor = Color.FromArgb(250, 244, 244);
        pnlContenido.Controls.Add(grpConfiguracion);
        pnlContenido.Controls.Add(dgvBackups);
        pnlContenido.Controls.Add(pnlInferior);
        pnlContenido.Dock = DockStyle.Fill;
        pnlContenido.Location = new Point(0, 48);
        pnlContenido.Name = "pnlContenido";
        pnlContenido.Padding = new Padding(16);
        pnlContenido.Size = new Size(1220, 672);
        pnlContenido.TabIndex = 1;
        // 
        // grpConfiguracion
        // 
        grpConfiguracion.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
        grpConfiguracion.BackColor = Color.White;
        grpConfiguracion.Controls.Add(lblNombreArchivo);
        grpConfiguracion.Controls.Add(txtNombreArchivo);
        grpConfiguracion.Controls.Add(lblRutaInfo);
        grpConfiguracion.Controls.Add(btnGenerarBackup);
        grpConfiguracion.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
        grpConfiguracion.ForeColor = Color.FromArgb(58, 53, 59);
        grpConfiguracion.Location = new Point(16, 14);
        grpConfiguracion.Name = "grpConfiguracion";
        grpConfiguracion.Size = new Size(1188, 116);
        grpConfiguracion.TabIndex = 0;
        grpConfiguracion.TabStop = false;
        grpConfiguracion.Text = "Configuración y Generación de Copia de Seguridad";
        // 
        // lblNombreArchivo
        // 
        lblNombreArchivo.Font = new Font("Segoe UI", 9F);
        lblNombreArchivo.ForeColor = Color.FromArgb(58, 53, 59);
        lblNombreArchivo.Location = new Point(20, 32);
        lblNombreArchivo.Name = "lblNombreArchivo";
        lblNombreArchivo.Size = new Size(130, 26);
        lblNombreArchivo.TabIndex = 0;
        lblNombreArchivo.Text = "Nombre del archivo:";
        lblNombreArchivo.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // txtNombreArchivo
        // 
        txtNombreArchivo.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
        txtNombreArchivo.BackColor = Color.White;
        txtNombreArchivo.BorderStyle = BorderStyle.FixedSingle;
        txtNombreArchivo.Font = new Font("Segoe UI", 9.5F);
        txtNombreArchivo.ForeColor = Color.FromArgb(58, 53, 59);
        txtNombreArchivo.Location = new Point(155, 33);
        txtNombreArchivo.Name = "txtNombreArchivo";
        txtNombreArchivo.Size = new Size(815, 24);
        txtNombreArchivo.TabIndex = 1;
        // 
        // lblRutaInfo
        // 
        lblRutaInfo.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
        lblRutaInfo.Font = new Font("Segoe UI", 9F, FontStyle.Italic);
        lblRutaInfo.ForeColor = Color.FromArgb(100, 95, 101);
        lblRutaInfo.Location = new Point(20, 72);
        lblRutaInfo.Name = "lblRutaInfo";
        lblRutaInfo.Size = new Size(950, 26);
        lblRutaInfo.TabIndex = 2;
        lblRutaInfo.Text = "Ruta de guardado: C:\\Backup_Veterinaria\\";
        lblRutaInfo.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // btnGenerarBackup
        // 
        btnGenerarBackup.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        btnGenerarBackup.BackColor = Color.FromArgb(200, 138, 150);
        btnGenerarBackup.Cursor = Cursors.Hand;
        btnGenerarBackup.FlatAppearance.BorderSize = 0;
        btnGenerarBackup.FlatStyle = FlatStyle.Flat;
        btnGenerarBackup.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
        btnGenerarBackup.ForeColor = Color.White;
        btnGenerarBackup.Location = new Point(988, 30);
        btnGenerarBackup.Name = "btnGenerarBackup";
        btnGenerarBackup.Size = new Size(184, 68);
        btnGenerarBackup.TabIndex = 3;
        btnGenerarBackup.Text = "Generar Backup";
        btnGenerarBackup.UseVisualStyleBackColor = false;
        // 
        // dgvBackups
        // 
        dgvBackups.AllowUserToAddRows = false;
        dgvBackups.AllowUserToDeleteRows = false;
        dgvBackups.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        dgvBackups.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        dgvBackups.BackgroundColor = Color.White;
        dataGridViewCellStyle1.Alignment = DataGridViewContentAlignment.MiddleLeft;
        dataGridViewCellStyle1.BackColor = Color.White;
        dataGridViewCellStyle1.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
        dataGridViewCellStyle1.ForeColor = Color.Black;
        dataGridViewCellStyle1.SelectionBackColor = SystemColors.Highlight;
        dataGridViewCellStyle1.SelectionForeColor = SystemColors.HighlightText;
        dataGridViewCellStyle1.WrapMode = DataGridViewTriState.True;
        dgvBackups.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
        dgvBackups.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
        dgvBackups.Columns.AddRange(new DataGridViewColumn[] { colNombreArchivo, colFechaHora, colUsuario });
        dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleLeft;
        dataGridViewCellStyle2.BackColor = Color.White;
        dataGridViewCellStyle2.Font = new Font("Segoe UI", 9F);
        dataGridViewCellStyle2.ForeColor = Color.FromArgb(58, 53, 59);
        dataGridViewCellStyle2.SelectionBackColor = Color.FromArgb(226, 217, 220);
        dataGridViewCellStyle2.SelectionForeColor = Color.FromArgb(58, 53, 59);
        dataGridViewCellStyle2.WrapMode = DataGridViewTriState.False;
        dgvBackups.DefaultCellStyle = dataGridViewCellStyle2;
        dgvBackups.EnableHeadersVisualStyles = false;
        dgvBackups.Location = new Point(16, 142);
        dgvBackups.MultiSelect = false;
        dgvBackups.Name = "dgvBackups";
        dgvBackups.ReadOnly = true;
        dataGridViewCellStyle3.Alignment = DataGridViewContentAlignment.MiddleLeft;
        dataGridViewCellStyle3.BackColor = Color.White;
        dataGridViewCellStyle3.Font = new Font("Segoe UI", 9F);
        dataGridViewCellStyle3.ForeColor = Color.FromArgb(58, 53, 59);
        dataGridViewCellStyle3.SelectionBackColor = Color.FromArgb(226, 217, 220);
        dataGridViewCellStyle3.SelectionForeColor = Color.FromArgb(58, 53, 59);
        dataGridViewCellStyle3.WrapMode = DataGridViewTriState.True;
        dgvBackups.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
        dgvBackups.RowHeadersVisible = false;
        dgvBackups.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        dgvBackups.Size = new Size(1188, 468);
        dgvBackups.TabIndex = 1;
        // 
        // colNombreArchivo
        // 
        colNombreArchivo.DataPropertyName = "NombreArchivo";
        colNombreArchivo.FillWeight = 45F;
        colNombreArchivo.HeaderText = "Nombre del archivo";
        colNombreArchivo.Name = "colNombreArchivo";
        colNombreArchivo.ReadOnly = true;
        // 
        // colFechaHora
        // 
        colFechaHora.DataPropertyName = "FechaHoraFormateada";
        colFechaHora.FillWeight = 25F;
        colFechaHora.HeaderText = "Fecha y hora";
        colFechaHora.Name = "colFechaHora";
        colFechaHora.ReadOnly = true;
        // 
        // colUsuario
        // 
        colUsuario.DataPropertyName = "UsuarioResponsable";
        colUsuario.FillWeight = 30F;
        colUsuario.HeaderText = "Usuario que realizó el backup";
        colUsuario.Name = "colUsuario";
        colUsuario.ReadOnly = true;
        // 
        // pnlInferior
        // 
        pnlInferior.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        pnlInferior.Controls.Add(btnVolver);
        pnlInferior.Controls.Add(btnRestaurarCopia);
        pnlInferior.Controls.Add(btnExaminarRestaurar);
        pnlInferior.Controls.Add(lblTotalBackups);
        pnlInferior.Location = new Point(16, 620);
        pnlInferior.Name = "pnlInferior";
        pnlInferior.Size = new Size(1188, 38);
        pnlInferior.TabIndex = 2;
        // 
        // btnVolver
        // 
        btnVolver.BackColor = Color.FromArgb(226, 217, 220);
        btnVolver.Cursor = Cursors.Hand;
        btnVolver.FlatAppearance.BorderSize = 0;
        btnVolver.FlatStyle = FlatStyle.Flat;
        btnVolver.Font = new Font("Segoe UI", 9F);
        btnVolver.ForeColor = Color.FromArgb(58, 53, 59);
        btnVolver.Location = new Point(0, 3);
        btnVolver.Name = "btnVolver";
        btnVolver.Size = new Size(140, 32);
        btnVolver.TabIndex = 0;
        btnVolver.Text = "Volver al panel";
        btnVolver.UseVisualStyleBackColor = false;
        // 
        // btnRestaurarCopia
        // 
        btnRestaurarCopia.BackColor = Color.FromArgb(200, 138, 150);
        btnRestaurarCopia.Cursor = Cursors.Hand;
        btnRestaurarCopia.FlatAppearance.BorderSize = 0;
        btnRestaurarCopia.FlatStyle = FlatStyle.Flat;
        btnRestaurarCopia.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
        btnRestaurarCopia.ForeColor = Color.White;
        btnRestaurarCopia.Location = new Point(150, 3);
        btnRestaurarCopia.Name = "btnRestaurarCopia";
        btnRestaurarCopia.Size = new Size(170, 32);
        btnRestaurarCopia.TabIndex = 1;
        btnRestaurarCopia.Text = "Restaurar Copia";
        btnRestaurarCopia.UseVisualStyleBackColor = false;
        // 
        // btnExaminarRestaurar
        // 
        btnExaminarRestaurar.BackColor = Color.FromArgb(226, 217, 220);
        btnExaminarRestaurar.Cursor = Cursors.Hand;
        btnExaminarRestaurar.FlatAppearance.BorderSize = 0;
        btnExaminarRestaurar.FlatStyle = FlatStyle.Flat;
        btnExaminarRestaurar.Font = new Font("Segoe UI", 9F);
        btnExaminarRestaurar.ForeColor = Color.FromArgb(58, 53, 59);
        btnExaminarRestaurar.Location = new Point(330, 3);
        btnExaminarRestaurar.Name = "btnExaminarRestaurar";
        btnExaminarRestaurar.Size = new Size(180, 32);
        btnExaminarRestaurar.TabIndex = 2;
        btnExaminarRestaurar.Text = "Examinar archivo .bak...";
        btnExaminarRestaurar.UseVisualStyleBackColor = false;
        // 
        // lblTotalBackups
        // 
        lblTotalBackups.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        lblTotalBackups.Font = new Font("Segoe UI", 9F, FontStyle.Italic);
        lblTotalBackups.ForeColor = Color.FromArgb(58, 53, 59);
        lblTotalBackups.Location = new Point(788, 3);
        lblTotalBackups.Name = "lblTotalBackups";
        lblTotalBackups.Size = new Size(400, 32);
        lblTotalBackups.TabIndex = 3;
        lblTotalBackups.Text = "Total de copias registradas: 0";

        lblTotalBackups.TextAlign = ContentAlignment.MiddleRight;
        // 
        // FormBackup
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
        Name = "FormBackup";
        StartPosition = FormStartPosition.CenterParent;
        Text = "CLÍNICA VETERINARIA — ADMINISTRADOR — GESTIÓN DE BACKUPS";
        pnlEncabezado.ResumeLayout(false);
        pnlContenido.ResumeLayout(false);
        grpConfiguracion.ResumeLayout(false);
        grpConfiguracion.PerformLayout();
        ((System.ComponentModel.ISupportInitialize)dgvBackups).EndInit();
        pnlInferior.ResumeLayout(false);
        ResumeLayout(false);
    }

    #endregion

    private Panel pnlEncabezado;
    private Label lblTitulo;
    private Label lblUsuarioSesion;
    private Panel pnlContenido;
    private GroupBox grpConfiguracion;
    private Label lblNombreArchivo;
    private TextBox txtNombreArchivo;
    private Label lblRutaInfo;
    private Button btnGenerarBackup;
    private DataGridView dgvBackups;
    private DataGridViewTextBoxColumn colNombreArchivo;
    private DataGridViewTextBoxColumn colFechaHora;
    private DataGridViewTextBoxColumn colUsuario;
    private Panel pnlInferior;
    private Button btnVolver;
    private Button btnRestaurarCopia;
    private Button btnExaminarRestaurar;
    private Label lblTotalBackups;
}