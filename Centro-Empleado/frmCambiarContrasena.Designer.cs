namespace Centro_Empleado
{
    partial class frmCambiarContrasena
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
            this.lblTitulo = new System.Windows.Forms.Label();
            this.lblContrasenaActual = new System.Windows.Forms.Label();
            this.lblContrasenaActualInput = new System.Windows.Forms.Label();
            this.txtContrasenaActual = new System.Windows.Forms.TextBox();
            this.lblNuevaContrasena = new System.Windows.Forms.Label();
            this.txtNuevaContrasena = new System.Windows.Forms.TextBox();
            this.lblConfirmarContrasena = new System.Windows.Forms.Label();
            this.txtConfirmarContrasena = new System.Windows.Forms.TextBox();
            this.btnCambiar = new System.Windows.Forms.Button();
            this.btnCancelar = new System.Windows.Forms.Button();
            this.lblInfo = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lblTitulo
            // 
            this.lblTitulo.AutoSize = true;
            this.lblTitulo.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTitulo.Location = new System.Drawing.Point(12, 9);
            this.lblTitulo.Name = "lblTitulo";
            this.lblTitulo.Size = new System.Drawing.Size(200, 20);
            this.lblTitulo.TabIndex = 0;
            this.lblTitulo.Text = "🔐 Cambiar Contraseña";
            // 
            // lblContrasenaActual
            // 
            this.lblContrasenaActual.AutoSize = true;
            this.lblContrasenaActual.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblContrasenaActual.ForeColor = System.Drawing.Color.Blue;
            this.lblContrasenaActual.Location = new System.Drawing.Point(12, 35);
            this.lblContrasenaActual.Name = "lblContrasenaActual";
            this.lblContrasenaActual.Size = new System.Drawing.Size(150, 15);
            this.lblContrasenaActual.TabIndex = 1;
            this.lblContrasenaActual.Text = "Contraseña actual: admin123";
            // 
            // lblContrasenaActualInput
            // 
            this.lblContrasenaActualInput.AutoSize = true;
            this.lblContrasenaActualInput.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblContrasenaActualInput.Location = new System.Drawing.Point(12, 65);
            this.lblContrasenaActualInput.Name = "lblContrasenaActualInput";
            this.lblContrasenaActualInput.Size = new System.Drawing.Size(120, 17);
            this.lblContrasenaActualInput.TabIndex = 2;
            this.lblContrasenaActualInput.Text = "Contraseña actual:";
            // 
            // txtContrasenaActual
            // 
            this.txtContrasenaActual.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtContrasenaActual.Location = new System.Drawing.Point(15, 85);
            this.txtContrasenaActual.Name = "txtContrasenaActual";
            this.txtContrasenaActual.PasswordChar = '*';
            this.txtContrasenaActual.Size = new System.Drawing.Size(250, 23);
            this.txtContrasenaActual.TabIndex = 3;
            this.txtContrasenaActual.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtContrasenaActual_KeyPress);
            // 
            // lblNuevaContrasena
            // 
            this.lblNuevaContrasena.AutoSize = true;
            this.lblNuevaContrasena.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblNuevaContrasena.Location = new System.Drawing.Point(12, 120);
            this.lblNuevaContrasena.Name = "lblNuevaContrasena";
            this.lblNuevaContrasena.Size = new System.Drawing.Size(110, 17);
            this.lblNuevaContrasena.TabIndex = 4;
            this.lblNuevaContrasena.Text = "Nueva contraseña:";
            // 
            // txtNuevaContrasena
            // 
            this.txtNuevaContrasena.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtNuevaContrasena.Location = new System.Drawing.Point(15, 140);
            this.txtNuevaContrasena.Name = "txtNuevaContrasena";
            this.txtNuevaContrasena.PasswordChar = '*';
            this.txtNuevaContrasena.Size = new System.Drawing.Size(250, 23);
            this.txtNuevaContrasena.TabIndex = 5;
            this.txtNuevaContrasena.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtNuevaContrasena_KeyPress);
            // 
            // lblConfirmarContrasena
            // 
            this.lblConfirmarContrasena.AutoSize = true;
            this.lblConfirmarContrasena.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblConfirmarContrasena.Location = new System.Drawing.Point(12, 175);
            this.lblConfirmarContrasena.Name = "lblConfirmarContrasena";
            this.lblConfirmarContrasena.Size = new System.Drawing.Size(140, 17);
            this.lblConfirmarContrasena.TabIndex = 6;
            this.lblConfirmarContrasena.Text = "Confirmar contraseña:";
            // 
            // txtConfirmarContrasena
            // 
            this.txtConfirmarContrasena.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtConfirmarContrasena.Location = new System.Drawing.Point(15, 195);
            this.txtConfirmarContrasena.Name = "txtConfirmarContrasena";
            this.txtConfirmarContrasena.PasswordChar = '*';
            this.txtConfirmarContrasena.Size = new System.Drawing.Size(250, 23);
            this.txtConfirmarContrasena.TabIndex = 7;
            this.txtConfirmarContrasena.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtConfirmarContrasena_KeyPress);
            // 
            // btnCambiar
            // 
            this.btnCambiar.BackColor = System.Drawing.Color.LightGreen;
            this.btnCambiar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCambiar.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCambiar.Location = new System.Drawing.Point(15, 240);
            this.btnCambiar.Name = "btnCambiar";
            this.btnCambiar.Size = new System.Drawing.Size(120, 35);
            this.btnCambiar.TabIndex = 8;
            this.btnCambiar.Text = "✓ Cambiar";
            this.btnCambiar.UseVisualStyleBackColor = false;
            this.btnCambiar.Click += new System.EventHandler(this.btnCambiar_Click);
            // 
            // btnCancelar
            // 
            this.btnCancelar.BackColor = System.Drawing.Color.LightCoral;
            this.btnCancelar.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancelar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCancelar.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCancelar.Location = new System.Drawing.Point(145, 240);
            this.btnCancelar.Name = "btnCancelar";
            this.btnCancelar.Size = new System.Drawing.Size(120, 35);
            this.btnCancelar.TabIndex = 9;
            this.btnCancelar.Text = "✗ Cancelar";
            this.btnCancelar.UseVisualStyleBackColor = false;
            this.btnCancelar.Click += new System.EventHandler(this.btnCancelar_Click);
            // 
            // lblInfo
            // 
            this.lblInfo.AutoSize = true;
            this.lblInfo.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblInfo.ForeColor = System.Drawing.Color.Gray;
            this.lblInfo.Location = new System.Drawing.Point(12, 285);
            this.lblInfo.Name = "lblInfo";
            this.lblInfo.Size = new System.Drawing.Size(250, 13);
            this.lblInfo.TabIndex = 10;
            this.lblInfo.Text = "La contraseña se guarda en el archivo config.txt";
            // 
            // frmCambiarContrasena
            // 
            this.AcceptButton = this.btnCambiar;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancelar;
            this.ClientSize = new System.Drawing.Size(280, 310);
            this.Controls.Add(this.lblInfo);
            this.Controls.Add(this.btnCancelar);
            this.Controls.Add(this.btnCambiar);
            this.Controls.Add(this.txtConfirmarContrasena);
            this.Controls.Add(this.lblConfirmarContrasena);
            this.Controls.Add(this.txtNuevaContrasena);
            this.Controls.Add(this.lblNuevaContrasena);
            this.Controls.Add(this.txtContrasenaActual);
            this.Controls.Add(this.lblContrasenaActualInput);
            this.Controls.Add(this.lblContrasenaActual);
            this.Controls.Add(this.lblTitulo);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmCambiarContrasena";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Cambiar Contraseña del Sistema";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblTitulo;
        private System.Windows.Forms.Label lblContrasenaActual;
        private System.Windows.Forms.Label lblContrasenaActualInput;
        private System.Windows.Forms.TextBox txtContrasenaActual;
        private System.Windows.Forms.Label lblNuevaContrasena;
        private System.Windows.Forms.TextBox txtNuevaContrasena;
        private System.Windows.Forms.Label lblConfirmarContrasena;
        private System.Windows.Forms.TextBox txtConfirmarContrasena;
        private System.Windows.Forms.Button btnCambiar;
        private System.Windows.Forms.Button btnCancelar;
        private System.Windows.Forms.Label lblInfo;
    }
}
