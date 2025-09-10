using System;
using System.Windows.Forms;
using System.IO;

namespace Centro_Empleado
{
    public partial class frmIngresarContrasena : Form
    {
        public bool ContrasenaCorrecta { get; private set; }
        private string archivoConfiguracion = Path.Combine(Application.StartupPath, "config.txt");

        public frmIngresarContrasena()
        {
            InitializeComponent();
            ContrasenaCorrecta = false;
        }

        private void btnAceptar_Click(object sender, EventArgs e)
        {
            string contrasenaIngresada = txtContrasena.Text.Trim();
            
            // Obtener contraseña desde archivo de configuración
            string contrasenaCorrecta = ObtenerContrasenaActual();
            
            if (contrasenaIngresada == contrasenaCorrecta)
            {
                ContrasenaCorrecta = true;
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            else
            {
                MessageBox.Show("Contraseña incorrecta. Intente nuevamente.", "Error de Autenticación", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtContrasena.Clear();
                txtContrasena.Focus();
            }
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            ContrasenaCorrecta = false;
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private string ObtenerContrasenaActual()
        {
            try
            {
                if (File.Exists(archivoConfiguracion))
                {
                    string[] lineas = File.ReadAllLines(archivoConfiguracion);
                    foreach (string linea in lineas)
                    {
                        if (linea.StartsWith("CONTRASENA="))
                        {
                            return linea.Substring(11);
                        }
                    }
                }
                return "admin123"; // Contraseña por defecto
            }
            catch
            {
                return "admin123";
            }
        }

        private void txtContrasena_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Permitir Enter para confirmar
            if (e.KeyChar == (char)Keys.Enter)
            {
                btnAceptar_Click(sender, e);
            }
            // Permitir Escape para cancelar
            else if (e.KeyChar == (char)Keys.Escape)
            {
                btnCancelar_Click(sender, e);
            }
        }
    }
}
