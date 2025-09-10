using System;
using System.Drawing;
using System.Windows.Forms;

namespace Centro_Empleado
{
    public partial class frmSeleccionRecetarios : Form
    {
        public int CantidadSeleccionada { get; private set; }

        public frmSeleccionRecetarios(int recetariosDisponibles)
        {
            InitializeComponent();
            CantidadSeleccionada = 0;
            
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
            
            // No se requiere lógica adicional ya que el sistema maneja automáticamente
            // la impresión de 3 recetarios por afiliado
        }

        private void btnAceptar_Click(object sender, EventArgs e)
        {
            CantidadSeleccionada = (int)nudCantidad.Value;
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}
