using System;
using System.Windows.Forms;
using Centro_Empleado.Data;
using Centro_Empleado.Models;

namespace Centro_Empleado
{
    public partial class frmAfiliado : Form
    {
        private DatabaseManager dbManager;

        public frmAfiliado()
        {
            InitializeComponent();
            dbManager = new DatabaseManager();
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            try
            {
                if (ValidarDatos())
                {
                    var afiliado = new Afiliado
                    {
                        ApellidoNombre = txtApellidoNombre.Text.Trim(),
                        DNI = txtDNI.Text.Trim(),
                        Empresa = txtEmpresa.Text.Trim(),
                        TieneGrupoFamiliar = chkGrupoFamiliar.Checked
                    };

                    dbManager.InsertarAfiliado(afiliado);
                    MessageBox.Show("Afiliado registrado correctamente", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LimpiarFormulario();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al guardar afiliado: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private bool ValidarDatos()
        {
            if (string.IsNullOrWhiteSpace(txtApellidoNombre.Text))
            {
                MessageBox.Show("El apellido y nombre es obligatorio", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtApellidoNombre.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtDNI.Text))
            {
                MessageBox.Show("El DNI es obligatorio", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtDNI.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtEmpresa.Text))
            {
                MessageBox.Show("La empresa es obligatoria", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtEmpresa.Focus();
                return false;
            }

            return true;
        }

        private void LimpiarFormulario()
        {
            txtApellidoNombre.Clear();
            txtDNI.Clear();
            txtEmpresa.Clear();
            chkGrupoFamiliar.Checked = false;
            txtApellidoNombre.Focus();
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
