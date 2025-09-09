using System;
using System.Drawing;
using System.Windows.Forms;

namespace Centro_Empleado
{
    public partial class frmSeleccionRecetarios : Form
    {
        public int CantidadSeleccionada { get; private set; }
        public bool ImprimirDosPorHoja { get; private set; }

        public frmSeleccionRecetarios(int recetariosDisponibles)
        {
            InitializeComponent();
            CantidadSeleccionada = 0;
            ImprimirDosPorHoja = false;
            
            // Actualizar el texto del label de disponibles
            lblDisponibles.Text = string.Format("Recetarios disponibles este mes: {0}", recetariosDisponibles);
            
            // Si no hay recetarios disponibles, deshabilitar el botón
            if (recetariosDisponibles <= 0)
            {
                btnAceptar.Enabled = false;
                lblDisponibles.Text = "No hay recetarios disponibles este mes";
                lblDisponibles.ForeColor = Color.Red;
                nudCantidad.Enabled = false;
                nudCantidad.Value = 0;
                nudCantidad.Minimum = 0;
                nudCantidad.Maximum = 0;
            }
            else
            {
                // Configurar el máximo del NumericUpDown solo si hay recetarios disponibles
                nudCantidad.Maximum = Math.Min(3, recetariosDisponibles);
                nudCantidad.Minimum = 1;
                nudCantidad.Value = 1;
                nudCantidad.Enabled = true;
            }
        }


        private Label lblTitulo;
        private Label lblInfo;
        private Label lblDisponibles;
        private Label lblCantidad;
        private NumericUpDown nudCantidad;
        private Button btnAceptar;
        private Button btnCancelar;

        private void nudCantidad_ValueChanged(object sender, EventArgs e)
        {
            // Solo procesar si el control está habilitado
            if (!nudCantidad.Enabled) return;
            
            int cantidad = (int)nudCantidad.Value;
            
            // Actualizar el estado del checkbox según la cantidad
            chkDosPorHoja.Enabled = cantidad >= 2;
            if (cantidad < 2)
            {
                chkDosPorHoja.Checked = false;
            }
        }

        private void chkDosPorHoja_CheckedChanged(object sender, EventArgs e)
        {
            // Si está marcado, asegurar que la cantidad sea par
            if (chkDosPorHoja.Checked && nudCantidad.Value % 2 != 0)
            {
                nudCantidad.Value = nudCantidad.Value + 1;
            }
        }

        private void btnAceptar_Click(object sender, EventArgs e)
        {
            CantidadSeleccionada = (int)nudCantidad.Value;
            ImprimirDosPorHoja = chkDosPorHoja.Checked;
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}
