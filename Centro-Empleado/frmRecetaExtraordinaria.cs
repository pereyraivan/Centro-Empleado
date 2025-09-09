using System;
using System.Drawing;
using System.Windows.Forms;

namespace Centro_Empleado
{
    public partial class frmRecetaExtraordinaria : Form
    {
        public string Motivo { get; private set; }
        public bool Aprobado { get; private set; }

        public frmRecetaExtraordinaria(string nombreAfiliado)
        {
            InitializeComponent();
            lblAfiliado.Text = string.Format("Afiliado: {0}", nombreAfiliado);
            Aprobado = false;
        }

        private void btnAprobar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtMotivo.Text))
            {
                MessageBox.Show("Debe ingresar un motivo para la receta extraordinaria.", "Motivo requerido", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            Motivo = txtMotivo.Text.Trim();
            Aprobado = true;
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            Aprobado = false;
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void txtMotivo_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Permitir solo caracteres alfanuméricos, espacios y algunos caracteres especiales
            if (!char.IsControl(e.KeyChar) && !char.IsLetterOrDigit(e.KeyChar) && 
                e.KeyChar != ' ' && e.KeyChar != '.' && e.KeyChar != ',' && e.KeyChar != '-')
            {
                e.Handled = true;
            }
        }
    }
}
