using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;
using Centro_Empleado.Data;
using Centro_Empleado.Models;

namespace Centro_Empleado
{
    public partial class frmBono : Form
    {
        private DatabaseManager dbManager;
        private Afiliado afiliadoSeleccionado = null;

        public frmBono()
        {
            InitializeComponent();
            dbManager = new DatabaseManager();
            InicializarFormulario();
            this.Text = "Generar Orden de Consulta - Bono de Cobro";
        }

        private void InicializarFormulario()
        {
            // Configurar fecha actual
            dtpFecha.Value = DateTime.Now;
            
            // Configurar eventos
            btnBuscar.Click += btnBuscar_Click;
            btnImprimirBono.Click += btnImprimirBono_Click;
            btnLimpiar.Click += btnLimpiar_Click;
            
            // Configurar validación de monto
            txtMonto.KeyPress += TxtMonto_KeyPress;
            
            // Deshabilitar botón de imprimir hasta que se seleccione un afiliado
            btnImprimirBono.Enabled = false;
        }

        private void TxtMonto_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Solo permitir números, punto decimal y teclas de control
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && e.KeyChar != '.')
            {
                e.Handled = true;
            }
            
            // Solo permitir un punto decimal
            if (e.KeyChar == '.' && (sender as TextBox).Text.IndexOf('.') > -1)
            {
                e.Handled = true;
            }
        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtDNI.Text))
            {
                MessageBox.Show("Por favor ingrese un DNI para buscar", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtDNI.Focus();
                return;
            }

            try
            {
                afiliadoSeleccionado = dbManager.BuscarAfiliadoPorDNIParaBono(txtDNI.Text.Trim());
                
                if (afiliadoSeleccionado != null)
                {
                    // Mostrar información del afiliado
                    txtApellidoNombre.Text = afiliadoSeleccionado.ApellidoNombre;
                    txtEmpresa.Text = afiliadoSeleccionado.Empresa;
                    txtDNI.Text = afiliadoSeleccionado.DNI;
                    
                    // Habilitar campos y botón de imprimir
                    txtMonto.Enabled = true;
                    txtConcepto.Enabled = true;
                    txtObservaciones.Enabled = true;
                    btnImprimirBono.Enabled = true;
                    
                    // Enfocar en el monto
                    txtMonto.Focus();
                   
                }
                else
                {
                    MessageBox.Show("No se encontró ningún afiliado con el DNI ingresado.", 
                        "Afiliado no encontrado", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    LimpiarFormulario();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(string.Format("Error al buscar afiliado: {0}", ex.Message), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnImprimirBono_Click(object sender, EventArgs e)
        {
            if (afiliadoSeleccionado == null)
            {
                MessageBox.Show("Debe buscar un afiliado antes de imprimir el bono.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(txtMonto.Text))
            {
                MessageBox.Show("Debe ingresar un monto para el bono.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtMonto.Focus();
                return;
            }

            decimal monto;
            if (!decimal.TryParse(txtMonto.Text, out monto) || monto <= 0)
            {
                MessageBox.Show("El monto debe ser un número válido mayor a 0.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtMonto.Focus();
                return;
            }

            try
            {
                // Crear el bono
                var bono = new Bono
                {
                    IdAfiliado = afiliadoSeleccionado.Id,
                    NumeroBono = dbManager.ObtenerProximoNumeroBono(),
                    FechaEmision = dtpFecha.Value.Date,
                    Monto = monto,
                    Concepto = txtConcepto.Text.Trim(),
                    Observaciones = txtObservaciones.Text.Trim(),
                    FechaCreacion = DateTime.Now
                };

                // Guardar en la base de datos
                dbManager.InsertarBono(bono);

                // Generar e imprimir el bono
                GenerarEImprimirBono(bono, afiliadoSeleccionado);

                MessageBox.Show(string.Format("Bono {0} generado correctamente por ${1:F2}", bono.NumeroBono, monto), 
                    "Bono generado", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // Limpiar formulario para el siguiente bono
                LimpiarFormulario();
            }
            catch (Exception ex)
            {
                MessageBox.Show(string.Format("Error al generar el bono: {0}", ex.Message), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void GenerarEImprimirBono(Bono bono, Afiliado afiliado)
        {
            try
            {
                // Leer el template HTML (misma ruta que recetaFinal.html)
                string templatePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "bonoFinal.html");
                if (!File.Exists(templatePath))
                {
                    MessageBox.Show("No se encontró el archivo bonoFinal.html", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                string htmlTemplate = File.ReadAllText(templatePath, System.Text.Encoding.UTF8);
                
                // Reemplazar los placeholders con los datos reales
                string html = htmlTemplate
                    .Replace("{NUMERO_BONO}", bono.NumeroBono)
                    .Replace("{APELLIDO_NOMBRE}", afiliado.ApellidoNombre)
                    .Replace("{DNI}", afiliado.DNI)
                    .Replace("{CONCEPTO}", bono.Concepto ?? "")
                    .Replace("{MONTO}", bono.Monto.ToString("F2"))
                    .Replace("{FECHA}", bono.FechaEmision.ToString("dd/MM/yyyy"));
                
                // Corregir la ruta del logo (mismo método que RecetarioManager)
                html = CorregirRutaLogo(html);
                
                // Guardar y abrir el archivo HTML (mismo método que RecetarioManager)
                string tempFile = Path.Combine(Path.GetTempPath(), string.Format("bono_{0}_{1}.html", bono.NumeroBono, DateTime.Now.Ticks));
                File.WriteAllText(tempFile, html, System.Text.Encoding.UTF8);
                
                // Abrir con el navegador predeterminado (mismo método que RecetarioManager)
                Process.Start(new ProcessStartInfo(tempFile) { UseShellExecute = true });
            }
            catch (Exception ex)
            {
                MessageBox.Show(string.Format("Error al generar el bono: {0}", ex.Message), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private string CorregirRutaLogo(string html)
        {
            // Buscar el logo en diferentes ubicaciones posibles (mismo método que RecetarioManager)
            string[] posiblesRutas = {
                "logo_cec1.png",
                "logo_cec.png",
                "Resources/logo_cec1.png",
                "Resources/logo_cec.png"
            };

            string logoPath = "";
            string rutaCompleta = "";
            
            foreach (string ruta in posiblesRutas)
            {
                rutaCompleta = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, ruta);
                if (File.Exists(rutaCompleta))
                {
                    logoPath = ruta;
                    break;
                }
            }

            // Si no se encuentra el logo, usar un placeholder
            if (string.IsNullOrEmpty(logoPath))
            {
                // Reemplazar la imagen del logo con texto "CEC" estilizado
                html = html.Replace("<img src=\"logo_cec1.png\" alt=\"Logo\" style=\"width: 35px; height: 35px; object-fit: contain;\">", 
                                   "<div style=\"width: 35px; height: 35px; border: 1px solid #000; display: flex; align-items: center; justify-content: center; font-weight: bold; font-size: 12px; background-color: #f0f0f0;\">CEC</div>");
            }
            else
            {
                // Copiar el logo al directorio temporal para que sea accesible desde el HTML
                string tempDir = Path.GetTempPath();
                string tempLogoPath = Path.Combine(tempDir, Path.GetFileName(logoPath));
                
                try
                {
                    if (!File.Exists(tempLogoPath))
                    {
                        File.Copy(rutaCompleta, tempLogoPath, true);
                    }
                    
                    // Usar la ruta temporal del logo
                    html = html.Replace("logo_cec1.png", Path.GetFileName(tempLogoPath));
                }
                catch
                {
                    // Si no se puede copiar, usar el placeholder
                    html = html.Replace("<img src=\"logo_cec1.png\" alt=\"Logo\" style=\"width: 35px; height: 35px; object-fit: contain;\">", 
                                       "<div style=\"width: 35px; height: 35px; border: 1px solid #000; display: flex; align-items: center; justify-content: center; font-weight: bold; font-size: 12px; background-color: #f0f0f0;\">CEC</div>");
                }
            }

            return html;
        }



        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            LimpiarFormulario();
        }

        private void LimpiarFormulario()
        {
            txtDNI.Clear();
            txtApellidoNombre.Clear();
            txtEmpresa.Clear();
            txtMonto.Clear();
            txtConcepto.Clear();
            txtObservaciones.Clear();
            dtpFecha.Value = DateTime.Now;
            
            txtMonto.Enabled = false;
            txtConcepto.Enabled = false;
            txtObservaciones.Enabled = false;
            btnImprimirBono.Enabled = false;
            
            afiliadoSeleccionado = null;
            txtDNI.Focus();
        }

        private void btnImprimirBono_Click_1(object sender, EventArgs e)
        {

        }
    }
}
