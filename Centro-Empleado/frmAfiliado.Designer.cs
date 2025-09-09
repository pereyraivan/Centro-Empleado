namespace Centro_Empleado
{
    partial class frmAfiliado
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            this.lblApellidoNombre = new System.Windows.Forms.Label();
            this.txtApellidoNombre = new System.Windows.Forms.TextBox();
            this.lblDNI = new System.Windows.Forms.Label();
            this.txtDNI = new System.Windows.Forms.TextBox();
            this.lblNumeroAfiliado = new System.Windows.Forms.Label();
            this.txtNumeroAfiliado = new System.Windows.Forms.TextBox();
            this.lblEmpresa = new System.Windows.Forms.Label();
            this.txtEmpresa = new System.Windows.Forms.TextBox();
            this.chkGrupoFamiliar = new System.Windows.Forms.CheckBox();
            this.btnGuardar = new System.Windows.Forms.Button();
            this.txtBuscar = new System.Windows.Forms.TextBox();
            this.lblBuscar = new System.Windows.Forms.Label();
            this.dgvAfiliados = new System.Windows.Forms.DataGridView();
            this.btnImprimir = new System.Windows.Forms.Button();
            this.btnEditar = new System.Windows.Forms.Button();
            this.btnEliminar = new System.Windows.Forms.Button();
            this.paneltitulo = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.btnOperaciones = new System.Windows.Forms.Button();
            this.panelOperaciones = new System.Windows.Forms.Panel();
            this.btnHistorialExtraordinarias = new System.Windows.Forms.Button();
            this.btnRecetaExtraordinaria = new System.Windows.Forms.Button();
            this.btnManual = new System.Windows.Forms.Button();
            this.btnImprimirCupon = new System.Windows.Forms.Button();
            this.btnVerCaja = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgvAfiliados)).BeginInit();
            this.paneltitulo.SuspendLayout();
            this.panelOperaciones.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblApellidoNombre
            // 
            this.lblApellidoNombre.AutoSize = true;
            this.lblApellidoNombre.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblApellidoNombre.Location = new System.Drawing.Point(45, 173);
            this.lblApellidoNombre.Name = "lblApellidoNombre";
            this.lblApellidoNombre.Size = new System.Drawing.Size(177, 25);
            this.lblApellidoNombre.TabIndex = 0;
            this.lblApellidoNombre.Text = "Apellido y Nombre:";
            // 
            // txtApellidoNombre
            // 
            this.txtApellidoNombre.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtApellidoNombre.Location = new System.Drawing.Point(44, 202);
            this.txtApellidoNombre.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.txtApellidoNombre.Name = "txtApellidoNombre";
            this.txtApellidoNombre.Size = new System.Drawing.Size(471, 30);
            this.txtApellidoNombre.TabIndex = 1;
            // 
            // lblDNI
            // 
            this.lblDNI.AutoSize = true;
            this.lblDNI.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.8F);
            this.lblDNI.Location = new System.Drawing.Point(45, 258);
            this.lblDNI.Name = "lblDNI";
            this.lblDNI.Size = new System.Drawing.Size(45, 22);
            this.lblDNI.TabIndex = 2;
            this.lblDNI.Text = "DNI:";
            // 
            // txtDNI
            // 
            this.txtDNI.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtDNI.Location = new System.Drawing.Point(44, 283);
            this.txtDNI.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.txtDNI.Name = "txtDNI";
            this.txtDNI.Size = new System.Drawing.Size(249, 30);
            this.txtDNI.TabIndex = 3;
            this.txtDNI.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtDNI_KeyPress);
            // 
            // lblNumeroAfiliado
            // 
            this.lblNumeroAfiliado.AutoSize = true;
            this.lblNumeroAfiliado.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblNumeroAfiliado.Location = new System.Drawing.Point(44, 335);
            this.lblNumeroAfiliado.Name = "lblNumeroAfiliado";
            this.lblNumeroAfiliado.Size = new System.Drawing.Size(156, 25);
            this.lblNumeroAfiliado.TabIndex = 5;
            this.lblNumeroAfiliado.Text = "Número Afiliado:";
            // 
            // txtNumeroAfiliado
            // 
            this.txtNumeroAfiliado.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtNumeroAfiliado.Location = new System.Drawing.Point(44, 364);
            this.txtNumeroAfiliado.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.txtNumeroAfiliado.Name = "txtNumeroAfiliado";
            this.txtNumeroAfiliado.Size = new System.Drawing.Size(249, 30);
            this.txtNumeroAfiliado.TabIndex = 4;
            // 
            // lblEmpresa
            // 
            this.lblEmpresa.AutoSize = true;
            this.lblEmpresa.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblEmpresa.Location = new System.Drawing.Point(44, 417);
            this.lblEmpresa.Name = "lblEmpresa";
            this.lblEmpresa.Size = new System.Drawing.Size(96, 25);
            this.lblEmpresa.TabIndex = 4;
            this.lblEmpresa.Text = "Empresa:";
            // 
            // txtEmpresa
            // 
            this.txtEmpresa.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtEmpresa.Location = new System.Drawing.Point(44, 445);
            this.txtEmpresa.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.txtEmpresa.Name = "txtEmpresa";
            this.txtEmpresa.Size = new System.Drawing.Size(471, 30);
            this.txtEmpresa.TabIndex = 5;
            // 
            // chkGrupoFamiliar
            // 
            this.chkGrupoFamiliar.AutoSize = true;
            this.chkGrupoFamiliar.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkGrupoFamiliar.Location = new System.Drawing.Point(44, 503);
            this.chkGrupoFamiliar.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.chkGrupoFamiliar.Name = "chkGrupoFamiliar";
            this.chkGrupoFamiliar.Size = new System.Drawing.Size(216, 29);
            this.chkGrupoFamiliar.TabIndex = 6;
            this.chkGrupoFamiliar.Text = "Tiene Grupo Familiar";
            this.chkGrupoFamiliar.UseVisualStyleBackColor = true;
            // 
            // btnGuardar
            // 
            this.btnGuardar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnGuardar.BackColor = System.Drawing.Color.White;
            this.btnGuardar.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnGuardar.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.btnGuardar.Location = new System.Drawing.Point(44, 662);
            this.btnGuardar.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnGuardar.Name = "btnGuardar";
            this.btnGuardar.Size = new System.Drawing.Size(205, 46);
            this.btnGuardar.TabIndex = 7;
            this.btnGuardar.Text = "Guardar";
            this.btnGuardar.UseVisualStyleBackColor = false;
            // 
            // txtBuscar
            // 
            this.txtBuscar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtBuscar.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtBuscar.Location = new System.Drawing.Point(677, 90);
            this.txtBuscar.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.txtBuscar.Name = "txtBuscar";
            this.txtBuscar.Size = new System.Drawing.Size(521, 30);
            this.txtBuscar.TabIndex = 9;
            // 
            // lblBuscar
            // 
            this.lblBuscar.AutoSize = true;
            this.lblBuscar.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblBuscar.Location = new System.Drawing.Point(572, 92);
            this.lblBuscar.Name = "lblBuscar";
            this.lblBuscar.Size = new System.Drawing.Size(86, 25);
            this.lblBuscar.TabIndex = 8;
            this.lblBuscar.Text = "Buscar:";
            // 
            // dgvAfiliados
            // 
            this.dgvAfiliados.AllowUserToAddRows = false;
            this.dgvAfiliados.AllowUserToDeleteRows = false;
            this.dgvAfiliados.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvAfiliados.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvAfiliados.BackgroundColor = System.Drawing.Color.White;
            this.dgvAfiliados.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.GradientActiveCaption;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvAfiliados.DefaultCellStyle = dataGridViewCellStyle1;
            this.dgvAfiliados.GridColor = System.Drawing.Color.LightSteelBlue;
            this.dgvAfiliados.Location = new System.Drawing.Point(559, 147);
            this.dgvAfiliados.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.dgvAfiliados.MultiSelect = false;
            this.dgvAfiliados.Name = "dgvAfiliados";
            this.dgvAfiliados.ReadOnly = true;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvAfiliados.RowHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.dgvAfiliados.RowHeadersVisible = false;
            this.dgvAfiliados.RowHeadersWidth = 51;
            this.dgvAfiliados.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvAfiliados.Size = new System.Drawing.Size(877, 498);
            this.dgvAfiliados.TabIndex = 10;
            // 
            // btnImprimir
            // 
            this.btnImprimir.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnImprimir.BackColor = System.Drawing.Color.Azure;
            this.btnImprimir.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnImprimir.Location = new System.Drawing.Point(1234, 81);
            this.btnImprimir.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnImprimir.Name = "btnImprimir";
            this.btnImprimir.Size = new System.Drawing.Size(202, 46);
            this.btnImprimir.TabIndex = 11;
            this.btnImprimir.Text = "Imprimir";
            this.btnImprimir.UseVisualStyleBackColor = false;
            // 
            // btnEditar
            // 
            this.btnEditar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnEditar.BackColor = System.Drawing.Color.White;
            this.btnEditar.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnEditar.Location = new System.Drawing.Point(310, 662);
            this.btnEditar.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnEditar.Name = "btnEditar";
            this.btnEditar.Size = new System.Drawing.Size(205, 46);
            this.btnEditar.TabIndex = 12;
            this.btnEditar.Text = "Editar";
            this.btnEditar.UseVisualStyleBackColor = false;
            // 
            // btnEliminar
            // 
            this.btnEliminar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnEliminar.BackColor = System.Drawing.Color.LightCoral;
            this.btnEliminar.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnEliminar.Location = new System.Drawing.Point(1215, 662);
            this.btnEliminar.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnEliminar.Name = "btnEliminar";
            this.btnEliminar.Size = new System.Drawing.Size(221, 46);
            this.btnEliminar.TabIndex = 13;
            this.btnEliminar.Text = "Eliminar";
            this.btnEliminar.UseVisualStyleBackColor = false;
            // 
            // paneltitulo
            // 
            this.paneltitulo.BackColor = System.Drawing.Color.LightSteelBlue;
            this.paneltitulo.Controls.Add(this.label1);
            this.paneltitulo.Controls.Add(this.btnOperaciones);
            this.paneltitulo.Dock = System.Windows.Forms.DockStyle.Top;
            this.paneltitulo.Location = new System.Drawing.Point(0, 0);
            this.paneltitulo.Name = "paneltitulo";
            this.paneltitulo.Size = new System.Drawing.Size(1491, 41);
            this.paneltitulo.TabIndex = 14;
            // 
            // label1
            // 
            this.label1.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(383, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(730, 25);
            this.label1.TabIndex = 9;
            this.label1.Text = "CENTRO EMPLEADOS DE COMERCIO DE CONCEPCIÓN - AGUILARES";
            // 
            // btnOperaciones
            // 
            this.btnOperaciones.BackColor = System.Drawing.Color.LightSteelBlue;
            this.btnOperaciones.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnOperaciones.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnOperaciones.Location = new System.Drawing.Point(0, -1);
            this.btnOperaciones.Name = "btnOperaciones";
            this.btnOperaciones.Size = new System.Drawing.Size(200, 42);
            this.btnOperaciones.TabIndex = 15;
            this.btnOperaciones.Text = "▶ Operaciones";
            this.btnOperaciones.UseVisualStyleBackColor = false;
            // 
            // panelOperaciones
            // 
            this.panelOperaciones.BackColor = System.Drawing.Color.WhiteSmoke;
            this.panelOperaciones.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelOperaciones.Controls.Add(this.btnHistorialExtraordinarias);
            this.panelOperaciones.Controls.Add(this.btnRecetaExtraordinaria);
            this.panelOperaciones.Controls.Add(this.btnManual);
            this.panelOperaciones.Controls.Add(this.btnImprimirCupon);
            this.panelOperaciones.Controls.Add(this.btnVerCaja);
            this.panelOperaciones.Location = new System.Drawing.Point(4, 43);
            this.panelOperaciones.Name = "panelOperaciones";
            this.panelOperaciones.Size = new System.Drawing.Size(218, 169);
            this.panelOperaciones.TabIndex = 16;
            this.panelOperaciones.Visible = false;
            // 
            // btnHistorialExtraordinarias
            // 
            this.btnHistorialExtraordinarias.BackColor = System.Drawing.Color.MediumTurquoise;
            this.btnHistorialExtraordinarias.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnHistorialExtraordinarias.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnHistorialExtraordinarias.Location = new System.Drawing.Point(5, 129);
            this.btnHistorialExtraordinarias.Name = "btnHistorialExtraordinarias";
            this.btnHistorialExtraordinarias.Size = new System.Drawing.Size(208, 30);
            this.btnHistorialExtraordinarias.TabIndex = 4;
            this.btnHistorialExtraordinarias.Text = "📋 Historial Extraordinarias";
            this.btnHistorialExtraordinarias.UseVisualStyleBackColor = false;
            // 
            // btnRecetaExtraordinaria
            // 
            this.btnRecetaExtraordinaria.BackColor = System.Drawing.Color.Orange;
            this.btnRecetaExtraordinaria.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRecetaExtraordinaria.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnRecetaExtraordinaria.Location = new System.Drawing.Point(5, 98);
            this.btnRecetaExtraordinaria.Name = "btnRecetaExtraordinaria";
            this.btnRecetaExtraordinaria.Size = new System.Drawing.Size(208, 30);
            this.btnRecetaExtraordinaria.TabIndex = 3;
            this.btnRecetaExtraordinaria.Text = "⚠️ Receta Extraordinaria";
            this.btnRecetaExtraordinaria.UseVisualStyleBackColor = false;
            // 
            // btnManual
            // 
            this.btnManual.BackColor = System.Drawing.Color.LightYellow;
            this.btnManual.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnManual.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnManual.Location = new System.Drawing.Point(5, 67);
            this.btnManual.Name = "btnManual";
            this.btnManual.Size = new System.Drawing.Size(208, 30);
            this.btnManual.TabIndex = 2;
            this.btnManual.Text = "📖 Manual";
            this.btnManual.UseVisualStyleBackColor = false;
            // 
            // btnImprimirCupon
            // 
            this.btnImprimirCupon.BackColor = System.Drawing.Color.LightBlue;
            this.btnImprimirCupon.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnImprimirCupon.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnImprimirCupon.Location = new System.Drawing.Point(5, 36);
            this.btnImprimirCupon.Name = "btnImprimirCupon";
            this.btnImprimirCupon.Size = new System.Drawing.Size(208, 30);
            this.btnImprimirCupon.TabIndex = 1;
            this.btnImprimirCupon.Text = "🏥 Imprimir Bono";
            this.btnImprimirCupon.UseVisualStyleBackColor = false;
            // 
            // btnVerCaja
            // 
            this.btnVerCaja.BackColor = System.Drawing.Color.LightGreen;
            this.btnVerCaja.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnVerCaja.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnVerCaja.Location = new System.Drawing.Point(5, 5);
            this.btnVerCaja.Name = "btnVerCaja";
            this.btnVerCaja.Size = new System.Drawing.Size(208, 30);
            this.btnVerCaja.TabIndex = 0;
            this.btnVerCaja.Text = "💰 Ver Caja";
            this.btnVerCaja.UseVisualStyleBackColor = false;
            // 
            // frmAfiliado
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.ClientSize = new System.Drawing.Size(1491, 739);
            this.Controls.Add(this.paneltitulo);
            this.Controls.Add(this.panelOperaciones);
            this.Controls.Add(this.btnEliminar);
            this.Controls.Add(this.btnEditar);
            this.Controls.Add(this.btnImprimir);
            this.Controls.Add(this.dgvAfiliados);
            this.Controls.Add(this.txtBuscar);
            this.Controls.Add(this.lblBuscar);
            this.Controls.Add(this.btnGuardar);
            this.Controls.Add(this.chkGrupoFamiliar);
            this.Controls.Add(this.txtEmpresa);
            this.Controls.Add(this.lblEmpresa);
            this.Controls.Add(this.txtNumeroAfiliado);
            this.Controls.Add(this.lblNumeroAfiliado);
            this.Controls.Add(this.txtDNI);
            this.Controls.Add(this.lblDNI);
            this.Controls.Add(this.txtApellidoNombre);
            this.Controls.Add(this.lblApellidoNombre);
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.MinimumSize = new System.Drawing.Size(800, 600);
            this.Name = "frmAfiliado";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Gestión de Afiliados y Recetarios";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            ((System.ComponentModel.ISupportInitialize)(this.dgvAfiliados)).EndInit();
            this.paneltitulo.ResumeLayout(false);
            this.paneltitulo.PerformLayout();
            this.panelOperaciones.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
    private System.Windows.Forms.Label lblApellidoNombre;
    private System.Windows.Forms.TextBox txtApellidoNombre;
    private System.Windows.Forms.Label lblDNI;
    private System.Windows.Forms.TextBox txtDNI;
    private System.Windows.Forms.Label lblNumeroAfiliado;
    private System.Windows.Forms.TextBox txtNumeroAfiliado;
    private System.Windows.Forms.Label lblEmpresa;
    private System.Windows.Forms.TextBox txtEmpresa;
    private System.Windows.Forms.CheckBox chkGrupoFamiliar;
    private System.Windows.Forms.Button btnGuardar;
    private System.Windows.Forms.Label lblBuscar;
    private System.Windows.Forms.TextBox txtBuscar;
    private System.Windows.Forms.DataGridView dgvAfiliados;
    private System.Windows.Forms.Button btnImprimir;
    private System.Windows.Forms.Button btnEditar;
    private System.Windows.Forms.Button btnEliminar;
        private System.Windows.Forms.Panel paneltitulo;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnOperaciones;
        private System.Windows.Forms.Panel panelOperaciones;
        private System.Windows.Forms.Button btnVerCaja;
        private System.Windows.Forms.Button btnImprimirCupon;
        private System.Windows.Forms.Button btnManual;
        private System.Windows.Forms.Button btnRecetaExtraordinaria;
        private System.Windows.Forms.Button btnHistorialExtraordinarias;

    }
}
