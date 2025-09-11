using System;
using System.Windows.Forms;
using System.IO;

namespace Centro_Empleado
{
    public partial class frmCambiarContrasena : Form
    {
        private string archivoConfiguracion = Path.Combine(Application.StartupPath, "config.txt");

        public frmCambiarContrasena()
        {
            InitializeComponent();
            CargarContrasenaActual();
        }

        private void CargarContrasenaActual()
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
                           
                            return;
                        }
                    }
                }
                
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar la configuración: " + ex.Message, "Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
              
            }
        }

        private void btnCambiar_Click(object sender, EventArgs e)
        {
            if (!ValidarDatos()) return;

            try
            {
                // Verificar contraseña actual
                string contrasenaActual = ObtenerContrasenaActual();
                if (txtContrasenaActual.Text.Trim() != contrasenaActual)
                {
                    MessageBox.Show("La contraseña actual es incorrecta.", "Error de Validación", 
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtContrasenaActual.Focus();
                    return;
                }

                // Verificar que las nuevas contraseñas coincidan
                if (txtNuevaContrasena.Text.Trim() != txtConfirmarContrasena.Text.Trim())
                {
                    MessageBox.Show("Las nuevas contraseñas no coinciden.", "Error de Validación", 
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtConfirmarContrasena.Focus();
                    return;
                }

                // Guardar nueva contraseña
                GuardarNuevaContrasena(txtNuevaContrasena.Text.Trim());

                MessageBox.Show("Contraseña cambiada exitosamente.", "Éxito", 
                    MessageBoxButtons.OK, MessageBoxIcon.Information);

                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cambiar la contraseña: " + ex.Message, "Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private bool ValidarDatos()
        {
            if (string.IsNullOrWhiteSpace(txtContrasenaActual.Text))
            {
                MessageBox.Show("Debe ingresar la contraseña actual.", "Validación", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtContrasenaActual.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtNuevaContrasena.Text))
            {
                MessageBox.Show("Debe ingresar la nueva contraseña.", "Validación", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtNuevaContrasena.Focus();
                return false;
            }

            if (txtNuevaContrasena.Text.Trim().Length < 4)
            {
                MessageBox.Show("La nueva contraseña debe tener al menos 4 caracteres.", "Validación", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtNuevaContrasena.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtConfirmarContrasena.Text))
            {
                MessageBox.Show("Debe confirmar la nueva contraseña.", "Validación", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtConfirmarContrasena.Focus();
                return false;
            }

            return true;
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

        private void GuardarNuevaContrasena(string nuevaContrasena)
        {
            try
            {
                string[] lineas;
                bool encontrado = false;

                if (File.Exists(archivoConfiguracion))
                {
                    lineas = File.ReadAllLines(archivoConfiguracion);
                    for (int i = 0; i < lineas.Length; i++)
                    {
                        if (lineas[i].StartsWith("CONTRASENA="))
                        {
                            lineas[i] = "CONTRASENA=" + nuevaContrasena;
                            encontrado = true;
                            break;
                        }
                    }
                }
                else
                {
                    lineas = new string[1];
                }

                if (!encontrado)
                {
                    Array.Resize(ref lineas, lineas.Length + 1);
                    lineas[lineas.Length - 1] = "CONTRASENA=" + nuevaContrasena;
                }

                File.WriteAllLines(archivoConfiguracion, lineas);
            }
            catch (Exception ex)
            {
                throw new Exception("No se pudo guardar la nueva contraseña: " + ex.Message);
            }
        }

        private void txtContrasenaActual_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                txtNuevaContrasena.Focus();
            }
        }

        private void txtNuevaContrasena_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                txtConfirmarContrasena.Focus();
            }
        }

        private void txtConfirmarContrasena_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                btnCambiar_Click(sender, e);
            }
        }
    }
}
