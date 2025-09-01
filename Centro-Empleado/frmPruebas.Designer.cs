namespace Centro_Empleado
{
    partial class frmPruebas
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
            this.label1 = new System.Windows.Forms.Label();
            this.cboAfiliados = new System.Windows.Forms.ComboBox();
            this.btnVerRecetarios = new System.Windows.Forms.Button();
            this.btnModificarFecha = new System.Windows.Forms.Button();
            this.btnVerificarHabilitacion = new System.Windows.Forms.Button();
            this.btnRecargarDatos = new System.Windows.Forms.Button();
            this.btnInfoBaseDatos = new System.Windows.Forms.Button();
            this.btnEliminarAfiliado = new System.Windows.Forms.Button();
            this.btnCerrar = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(200, 16);
            this.label1.TabIndex = 0;
            this.label1.Text = "HERRAMIENTAS DE PRUEBAS";
            // 
            // cboAfiliados
            // 
            this.cboAfiliados.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboAfiliados.FormattingEnabled = true;
            this.cboAfiliados.Location = new System.Drawing.Point(15, 40);
            this.cboAfiliados.Name = "cboAfiliados";
            this.cboAfiliados.Size = new System.Drawing.Size(400, 21);
            this.cboAfiliados.TabIndex = 1;
            // 
            // btnVerRecetarios
            // 
            this.btnVerRecetarios.Location = new System.Drawing.Point(15, 25);
            this.btnVerRecetarios.Name = "btnVerRecetarios";
            this.btnVerRecetarios.Size = new System.Drawing.Size(150, 30);
            this.btnVerRecetarios.TabIndex = 2;
            this.btnVerRecetarios.Text = "Ver Recetarios";
            this.btnVerRecetarios.UseVisualStyleBackColor = true;
            this.btnVerRecetarios.Click += new System.EventHandler(this.btnVerRecetarios_Click);
            // 
            // btnModificarFecha
            // 
            this.btnModificarFecha.Location = new System.Drawing.Point(175, 25);
            this.btnModificarFecha.Name = "btnModificarFecha";
            this.btnModificarFecha.Size = new System.Drawing.Size(150, 30);
            this.btnModificarFecha.TabIndex = 3;
            this.btnModificarFecha.Text = "Modificar Fecha";
            this.btnModificarFecha.UseVisualStyleBackColor = true;
            this.btnModificarFecha.Click += new System.EventHandler(this.btnModificarFecha_Click);
            // 
            // btnVerificarHabilitacion
            // 
            this.btnVerificarHabilitacion.Location = new System.Drawing.Point(335, 25);
            this.btnVerificarHabilitacion.Name = "btnVerificarHabilitacion";
            this.btnVerificarHabilitacion.Size = new System.Drawing.Size(150, 30);
            this.btnVerificarHabilitacion.TabIndex = 4;
            this.btnVerificarHabilitacion.Text = "Verificar Habilitación";
            this.btnVerificarHabilitacion.UseVisualStyleBackColor = true;
            this.btnVerificarHabilitacion.Click += new System.EventHandler(this.btnVerificarHabilitacion_Click);
            // 
            // btnRecargarDatos
            // 
            this.btnRecargarDatos.Location = new System.Drawing.Point(15, 65);
            this.btnRecargarDatos.Name = "btnRecargarDatos";
            this.btnRecargarDatos.Size = new System.Drawing.Size(150, 30);
            this.btnRecargarDatos.TabIndex = 5;
            this.btnRecargarDatos.Text = "Recargar Datos";
            this.btnRecargarDatos.UseVisualStyleBackColor = true;
            this.btnRecargarDatos.Click += new System.EventHandler(this.btnRecargarDatos_Click);
            // 
            // btnInfoBaseDatos
            // 
            this.btnInfoBaseDatos.Location = new System.Drawing.Point(175, 65);
            this.btnInfoBaseDatos.Name = "btnInfoBaseDatos";
            this.btnInfoBaseDatos.Size = new System.Drawing.Size(150, 30);
            this.btnInfoBaseDatos.TabIndex = 6;
            this.btnInfoBaseDatos.Text = "Info Base Datos";
            this.btnInfoBaseDatos.UseVisualStyleBackColor = true;
            this.btnInfoBaseDatos.Click += new System.EventHandler(this.btnInfoBaseDatos_Click);
            // 
            // btnEliminarAfiliado
            // 
            this.btnEliminarAfiliado.Location = new System.Drawing.Point(335, 65);
            this.btnEliminarAfiliado.Name = "btnEliminarAfiliado";
            this.btnEliminarAfiliado.Size = new System.Drawing.Size(150, 30);
            this.btnEliminarAfiliado.TabIndex = 7;
            this.btnEliminarAfiliado.Text = "Eliminar Afiliado";
            this.btnEliminarAfiliado.UseVisualStyleBackColor = true;
            this.btnEliminarAfiliado.Click += new System.EventHandler(this.btnEliminarAfiliado_Click);
            // 
            // btnCerrar
            // 
            this.btnCerrar.Location = new System.Drawing.Point(175, 200);
            this.btnCerrar.Name = "btnCerrar";
            this.btnCerrar.Size = new System.Drawing.Size(100, 30);
            this.btnCerrar.TabIndex = 5;
            this.btnCerrar.Text = "Cerrar";
            this.btnCerrar.UseVisualStyleBackColor = true;
            this.btnCerrar.Click += new System.EventHandler(this.btnCerrar_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btnVerRecetarios);
            this.groupBox1.Controls.Add(this.btnModificarFecha);
            this.groupBox1.Controls.Add(this.btnVerificarHabilitacion);
            this.groupBox1.Controls.Add(this.btnRecargarDatos);
            this.groupBox1.Controls.Add(this.btnInfoBaseDatos);
            this.groupBox1.Controls.Add(this.btnEliminarAfiliado);
            this.groupBox1.Location = new System.Drawing.Point(15, 70);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(500, 150);
            this.groupBox1.TabIndex = 6;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Acciones";
            // 
            // frmPruebas
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(530, 240);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.btnCerrar);
            this.Controls.Add(this.cboAfiliados);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.Name = "frmPruebas";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Herramientas de Pruebas - Centro Empleado";
            this.groupBox1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cboAfiliados;
        private System.Windows.Forms.Button btnVerRecetarios;
        private System.Windows.Forms.Button btnModificarFecha;
        private System.Windows.Forms.Button btnVerificarHabilitacion;
        private System.Windows.Forms.Button btnRecargarDatos;
        private System.Windows.Forms.Button btnInfoBaseDatos;
        private System.Windows.Forms.Button btnEliminarAfiliado;
        private System.Windows.Forms.Button btnCerrar;
        private System.Windows.Forms.GroupBox groupBox1;
    }
}
