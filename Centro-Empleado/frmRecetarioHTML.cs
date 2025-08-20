using System;
using System.IO;
using System.Windows.Forms;

namespace Centro_Empleado
{
    public partial class frmRecetarioHTML : Form
    {
        private WebBrowser webBrowser;
        private string templatePath;

        public frmRecetarioHTML()
        {
            InitializeComponent();
            SetupWebBrowser();
        }

        private void SetupWebBrowser()
        {
            webBrowser = new WebBrowser();
            webBrowser.Dock = DockStyle.Fill;
            this.Controls.Add(webBrowser);
            
            templatePath = Path.Combine(Application.StartupPath, "template_recetario.html");
            if (!File.Exists(templatePath))
            {
                templatePath = Path.Combine(Directory.GetCurrentDirectory(), "template_recetario.html");
            }
        }

        public void CargarRecetario(Afiliado afiliado, Recetario recetario)
        {
            try
            {
                string htmlContent = File.ReadAllText(templatePath);
                
                // Reemplazar placeholders con datos reales
                htmlContent = htmlContent.Replace("{NUMERO_RECETARIO}", recetario.NumeroTalonario.ToString("D6"));
                htmlContent = htmlContent.Replace("{SOCIO_NUMERO}", afiliado.NumeroAfiliado.ToString());
                htmlContent = htmlContent.Replace("{FECHA_EMISION}", recetario.FechaEmision.ToString("dd/MM/yyyy"));
                htmlContent = htmlContent.Replace("{FECHA_VENCIMIENTO}", recetario.FechaVencimiento.ToString("dd/MM/yyyy"));
                htmlContent = htmlContent.Replace("{NOMBRE_TITULAR}", afiliado.ApellidoNombre);
                htmlContent = htmlContent.Replace("{LUGAR_TRABAJO}", afiliado.Empresa);
                htmlContent = htmlContent.Replace("{NOMBRE_PACIENTE}", ""); // Puedes agregar lógica para familiares
                htmlContent = htmlContent.Replace("{AÑO}", DateTime.Now.Year.ToString());
                
                webBrowser.DocumentText = htmlContent;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar el template: {ex.Message}", "Error", 
                              MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void ImprimirRecetario()
        {
            webBrowser.ShowPrintDialog();
        }

        public void ExportarAPDF()
        {
            // Aquí puedes agregar lógica para exportar a PDF
            // Usando librerías como wkhtmltopdf o similar
            SaveFileDialog saveDialog = new SaveFileDialog();
            saveDialog.Filter = "PDF files (*.pdf)|*.pdf";
            saveDialog.DefaultExt = "pdf";
            
            if (saveDialog.ShowDialog() == DialogResult.OK)
            {
                // Implementar exportación a PDF
                MessageBox.Show("Funcionalidad de exportar a PDF se puede implementar con librerías adicionales", 
                              "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
    }
}
