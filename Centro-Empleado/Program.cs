using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Centro_Empleado
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            try
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new frmAfiliado());
            }
            catch (Exception ex)
            {
                MessageBox.Show(string.Format("Error al iniciar la aplicación:\n\n{0}\n\nDetalles:\n{1}", ex.Message, ex.ToString()), 
                    "Error de Inicio", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
