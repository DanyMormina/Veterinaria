namespace Veterinaria.WinForms.Views.Admin
{
    partial class FormUsuarios
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
            lblUsuarioSesion = new Label();
            lblTitulo = new Label();
            pnlHeader = new Panel();
            panel1 = new Panel();
            panel2 = new Panel();
            panel3 = new Panel();
            TBBuscar = new TextBox();
            pnlHeader.SuspendLayout();
            panel2.SuspendLayout();
            SuspendLayout();
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
            // lblTitulo
            // 
            lblTitulo.Dock = DockStyle.Left;
            lblTitulo.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            lblTitulo.ForeColor = Color.White;
            lblTitulo.Location = new Point(16, 0);
            lblTitulo.Name = "lblTitulo";
            lblTitulo.Size = new Size(539, 50);
            lblTitulo.TabIndex = 0;
            lblTitulo.Text = "CLÍNICA VETERINARIA — ADMINISTRADOR — GESTIÓN DE USUARIOS";
            lblTitulo.TextAlign = ContentAlignment.MiddleLeft;
            lblTitulo.Click += lblTitulo_Click;
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
            pnlHeader.TabIndex = 1;
            // 
            // panel1
            // 
            panel1.BackColor = SystemColors.Control;
            panel1.BorderStyle = BorderStyle.FixedSingle;
            panel1.Font = new Font("Microsoft Sans Serif", 8.25F);
            panel1.Location = new Point(26, 79);
            panel1.Name = "panel1";
            panel1.Size = new Size(405, 571);
            panel1.TabIndex = 2;
            panel1.Paint += panel1_Paint;
            // 
            // panel2
            // 
            panel2.BackColor = SystemColors.Control;
            panel2.BorderStyle = BorderStyle.FixedSingle;
            panel2.Controls.Add(TBBuscar);
            panel2.Controls.Add(panel3);
            panel2.Font = new Font("Microsoft Sans Serif", 8.25F);
            panel2.Location = new Point(451, 79);
            panel2.Name = "panel2";
            panel2.Size = new Size(618, 571);
            panel2.TabIndex = 3;
            // 
            // panel3
            // 
            panel3.BackColor = SystemColors.Control;
            panel3.BorderStyle = BorderStyle.FixedSingle;
            panel3.Location = new Point(35, 82);
            panel3.Name = "panel3";
            panel3.Size = new Size(553, 467);
            panel3.TabIndex = 0;
            // 
            // TBBuscar
            // 
            TBBuscar.BackColor = SystemColors.Control;
            TBBuscar.BorderStyle = BorderStyle.None;
            TBBuscar.Font = new Font("Bahnschrift Condensed", 14.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            TBBuscar.Location = new Point(35, 25);
            TBBuscar.Name = "TBBuscar";
            TBBuscar.Size = new Size(55, 23);
            TBBuscar.TabIndex = 1;
            TBBuscar.Text = "Buscar";
            // 
            // FormUsuarios
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1100, 700);
            Controls.Add(panel2);
            Controls.Add(panel1);
            Controls.Add(pnlHeader);
            Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            Name = "FormUsuarios";
            Text = "CLÍNICA VETERINARIA — ADMINISTRADOR — GESTIÓN DE USUARIOS";
            Load += FormUsuarios_Load;
            pnlHeader.ResumeLayout(false);
            panel2.ResumeLayout(false);
            panel2.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private Panel pnlHeader;
        private Label lblTitulo;
        private Label lblUsuarioSesion;
        private Panel panel1;
        private Panel panel2;
        private TextBox TBBuscar;
        private Panel panel3;
    }
}