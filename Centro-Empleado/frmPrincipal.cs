using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Centro_Empleado.Data;

namespace Centro_Empleado
{
    public partial class frmPrincipal : Form
    {
        public frmPrincipal()
        {
            InitializeComponent();
            
            // Inicializar la base de datos al abrir la aplicación
            try
            {
                DatabaseManager db = new DatabaseManager();
                
                // Probar la conexión y verificar si las tablas existen
                if (!db.ProbarConexion())
                {
                    DialogResult result = MessageBox.Show(
                        "Las tablas de la base de datos no existen o están corruptas.\n¿Desea recrearlas?", 
                        "Base de Datos", 
                        MessageBoxButtons.YesNo, 
                        MessageBoxIcon.Question);
                    
                    if (result == DialogResult.Yes)
                    {
                        db.RecrearTablas();
                        MessageBox.Show("Base de datos recreada correctamente", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al inicializar la base de datos: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnAfiliado_Click(object sender, EventArgs e)
        {
            frmAfiliado formAfiliado = new frmAfiliado();
            formAfiliado.ShowDialog();
        }

        private void btnRecetario_Click(object sender, EventArgs e)
        {
            frmRecetario formRecetario = new frmRecetario();
            formRecetario.ShowDialog();
        }

        private void btnSalir_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void frmPrincipal_Load(object sender, EventArgs e)
        {

        }
    }
}
