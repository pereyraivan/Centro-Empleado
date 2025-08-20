using System;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.Windows.Forms;
using Centro_Empleado.Data;
using Centro_Empleado.Models;

namespace Centro_Empleado
{
    public partial class frmRecetario : Form
    {
        private DatabaseManager dbManager;
        private Afiliado afiliadoSeleccionado;
        private Recetario recetarioActual;
        private Recetario recetarioActual2;

        private RecetarioManager recetarioManager;

        public frmRecetario()
        {
            InitializeComponent();
            dbManager = new DatabaseManager();
            recetarioManager = new RecetarioManager();
        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txtDNI.Text))
                {
                    MessageBox.Show("Ingrese el DNI para buscar", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                afiliadoSeleccionado = dbManager.BuscarAfiliadoPorDNI(txtDNI.Text.Trim());

                if (afiliadoSeleccionado == null)
                {
                    MessageBox.Show("No se encontró un afiliado con ese DNI", "Búsqueda", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LimpiarDatos();
                    return;
                }

                MostrarDatosAfiliado();
                VerificarLimiteRecetarios();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al buscar afiliado: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void MostrarDatosAfiliado()
        {
            lblNombre.Text = afiliadoSeleccionado.ApellidoNombre;
            lblEmpresa.Text = afiliadoSeleccionado.Empresa;
            lblGrupoFamiliar.Text = afiliadoSeleccionado.TieneGrupoFamiliar ? "Sí" : "No";
        }

        private void VerificarLimiteRecetarios()
        {
            int recetariosEmitidos = dbManager.ContarRecetariosMensuales(afiliadoSeleccionado.Id, DateTime.Now);
            int limite = afiliadoSeleccionado.TieneGrupoFamiliar ? 4 : 2;

            lblRecetariosEmitidos.Text = $"{recetariosEmitidos} de {limite}";

            // Verificar si puede imprimir 2 recetarios más
            if (recetariosEmitidos >= limite - 1) // -1 porque imprime 2 a la vez
            {
                btnImprimir.Enabled = false;
                DateTime? proximaFecha = dbManager.FechaProximaHabilitacion(afiliadoSeleccionado.Id);
                if (proximaFecha.HasValue)
                {
                    lblMensaje.Text = $"Ya imprimió los recetarios mensuales correspondientes.\nPodrá imprimir nuevamente a partir del {proximaFecha.Value:dd/MM/yyyy}";
                    lblMensaje.ForeColor = Color.Red;
                }
            }
            else
            {
                btnImprimir.Enabled = true;
                int recetariosRestantes = limite - recetariosEmitidos;
                if (recetariosRestantes == 1)
                {
                    lblMensaje.Text = "Solo puede imprimir 1 recetario más este mes";
                    lblMensaje.ForeColor = Color.Orange;
                    btnImprimir.Enabled = false; // No permitir imprimir 2 si solo queda 1
                }
                else
                {
                    lblMensaje.Text = $"Puede imprimir 2 recetarios (quedarán {recetariosRestantes - 2})";
                    lblMensaje.ForeColor = Color.Green;
                }
            }
        }

        private void btnImprimir_Click(object sender, EventArgs e)
        {
            try
            {
                if (afiliadoSeleccionado == null)
                {
                    MessageBox.Show("Debe buscar un afiliado primero", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Verificar que puede imprimir 2 recetarios
                int recetariosEmitidos = dbManager.ContarRecetariosMensuales(afiliadoSeleccionado.Id, DateTime.Now);
                int limite = afiliadoSeleccionado.TieneGrupoFamiliar ? 4 : 2;
                if (recetariosEmitidos >= limite - 1)
                {
                    MessageBox.Show("No puede imprimir 2 recetarios. Verificar límite mensual.", "Límite excedido", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Obtener el próximo número de recetario
                int numero = dbManager.ObtenerProximoNumeroTalonario();
                var recetario = new Recetario
                {
                    NumeroTalonario = numero,
                    IdAfiliado = afiliadoSeleccionado.Id,
                    FechaEmision = DateTime.Now,
                    FechaVencimiento = DateTime.Now.AddMonths(1)
                };

                // Imprimir vista previa y luego imprimir
                recetarioManager.ImprimirRecetario(this, recetario, afiliadoSeleccionado);

                // Guardar en base de datos después de imprimir
                dbManager.InsertarRecetario(recetario);
                MessageBox.Show($"Recetario N° {numero:D6} impreso correctamente", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                VerificarLimiteRecetarios();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al imprimir recetario: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void PrintDoc_PrintPage(object sender, PrintPageEventArgs e)
        {
            Graphics g = e.Graphics;
            Font fontTitulo = new Font("Arial", 11, FontStyle.Bold);
            Font fontNormal = new Font("Arial", 9);
            Font fontPequeno = new Font("Arial", 7);
            Font fontNumero = new Font("Arial", 16, FontStyle.Bold);
            Brush brush = Brushes.Black;
            Pen pen = new Pen(Color.Black, 1);

            // Dimensiones de página A4 (aprox 800x1100 puntos)
            int anchoHoja = 750;
            int altoRecetario = 400;
            int margenIzq = 30;
            int margenSup = 50;

            // Verificar que ambos recetarios existen
            if (recetarioActual == null || recetarioActual2 == null)
            {
                MessageBox.Show("Error: Los recetarios no están inicializados correctamente", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Imprimir 2 recetarios por hoja con números diferentes
            // Primer recetario (arriba)
            DibujarRecetario(g, fontTitulo, fontNormal, fontPequeno, fontNumero, brush, pen, 
                           margenIzq, margenSup, anchoHoja, altoRecetario, recetarioActual);
            
            // Segundo recetario (abajo)  
            DibujarRecetario(g, fontTitulo, fontNormal, fontPequeno, fontNumero, brush, pen, 
                           margenIzq, margenSup + altoRecetario + 50, anchoHoja, altoRecetario, recetarioActual2);
        }

        private void DibujarRecetario(Graphics g, Font fontTitulo, Font fontNormal, Font fontPequeno, 
                                    Font fontNumero, Brush brush, Pen pen, 
                                    int x, int y, int ancho, int alto, Recetario recetario)
        {
            // Borde principal del recetario
            g.DrawRectangle(pen, x, y, ancho, alto);

            // HEADER - Primera fila con logo y título
            int yHeader = y + 10;
            
            // Logo mejorado
            try
            {
                string logoPath = System.IO.Path.Combine(Application.StartupPath, "Resources", "logo_cec.png");
                if (!System.IO.File.Exists(logoPath))
                {
                    logoPath = System.IO.Path.Combine(System.IO.Directory.GetCurrentDirectory(), "Resources", "logo_cec.png");
                }
                
                if (System.IO.File.Exists(logoPath))
                {
                    using (var logo = Image.FromFile(logoPath))
                    {
                        g.DrawImage(logo, x + 10, yHeader, 60, 60);
                    }
                }
                else
                {
                    // Logo del CEC según imagen real
                    g.DrawEllipse(new Pen(Color.Black, 2), x + 10, yHeader, 60, 60);
                    g.DrawEllipse(new Pen(Color.Black, 1), x + 15, yHeader + 5, 50, 50);
                    g.DrawString("CEC", new Font("Arial", 14, FontStyle.Bold), brush, x + 28, yHeader + 25);
                    g.DrawString("CENTRO DE", new Font("Arial", 6), brush, x + 18, yHeader + 45);
                    g.DrawString("EMPLEADOS", new Font("Arial", 6), brush, x + 18, yHeader + 52);
                }
            }
            catch
            {
                g.DrawEllipse(pen, x + 10, yHeader, 60, 60);
                g.DrawString("CEC", new Font("Arial", 14, FontStyle.Bold), brush, x + 30, yHeader + 25);
            }
            
            // Título principal
            g.DrawString("CENTRO DE EMPLEADOS DE", new Font("Arial", 14, FontStyle.Bold), brush, x + 80, yHeader + 10);
            g.DrawString("COMERCIO DE CONCEPCION", new Font("Arial", 14, FontStyle.Bold), brush, x + 80, yHeader + 30);

            // Número de recetario (lado derecho del header)
            string numeroRecetario = $"N° {recetario.NumeroTalonario:D6}";
            g.DrawString(numeroRecetario, new Font("Arial", 16, FontStyle.Bold), brush, x + 450, yHeader + 20);

            // TABLA SUPERIOR DERECHA: SOCIO N° | FECHA DE EMISION | VALIDO HASTA
            int yTablaHeader = yHeader + 55;
            int xTablaHeader = x + 450;
            
            // Headers
            g.DrawRectangle(pen, xTablaHeader, yTablaHeader, 80, 20);
            g.DrawString("SOCIO N°", fontPequeno, brush, xTablaHeader + 15, yTablaHeader + 5);
            
            g.DrawRectangle(pen, xTablaHeader + 80, yTablaHeader, 120, 20);
            g.DrawString("FECHA DE EMISION", fontPequeno, brush, xTablaHeader + 85, yTablaHeader + 5);
            
            g.DrawRectangle(pen, xTablaHeader + 200, yTablaHeader, 100, 20);
            g.DrawString("VALIDO HASTA", fontPequeno, brush, xTablaHeader + 210, yTablaHeader + 5);
            
            // Campos de datos
            g.DrawRectangle(pen, xTablaHeader, yTablaHeader + 20, 80, 25);
            g.DrawRectangle(pen, xTablaHeader + 80, yTablaHeader + 20, 120, 25);
            g.DrawString(recetario.FechaEmision.ToString("dd/MM/yyyy"), fontPequeno, brush, xTablaHeader + 100, yTablaHeader + 30);
            g.DrawRectangle(pen, xTablaHeader + 200, yTablaHeader + 20, 100, 25);
            g.DrawString(recetario.FechaVencimiento.ToString("dd/MM/yyyy"), fontPequeno, brush, xTablaHeader + 220, yTablaHeader + 30);

            // APELLIDO Y NOMBRE TITULAR
            int yTitular = yTablaHeader + 55;
            g.DrawRectangle(pen, x + 10, yTitular, 430, 30);
            g.DrawString("Apellido y Nombre Titular", fontPequeno, brush, x + 15, yTitular + 5);
            g.DrawString(afiliadoSeleccionado.ApellidoNombre, fontNormal, brush, x + 15, yTitular + 18);

            // LUGAR DE TRABAJO | IMPORTE TOTAL (lado derecho del titular)
            g.DrawRectangle(pen, xTablaHeader, yTitular, 200, 30);
            g.DrawString("Lugar de trabajo", fontPequeno, brush, xTablaHeader + 5, yTitular + 5);
            g.DrawString(afiliadoSeleccionado.Empresa, fontPequeno, brush, xTablaHeader + 5, yTitular + 18);
            
            g.DrawRectangle(pen, xTablaHeader + 200, yTitular, 100, 30);
            g.DrawString("Importe", fontPequeno, brush, xTablaHeader + 225, yTitular + 8);
            g.DrawString("Total", fontPequeno, brush, xTablaHeader + 230, yTitular + 20);

            // APELLIDO Y NOMBRE PACIENTE
            int yPaciente = yTitular + 40;
            g.DrawRectangle(pen, x + 10, yPaciente, 430, 35);
            g.DrawString("Apellido y Nombre Paciente", fontPequeno, brush, x + 15, yPaciente + 5);

            // EDAD | SEXO (pequeños cuadros junto al paciente)
            g.DrawRectangle(pen, x + 450, yPaciente, 40, 35);
            g.DrawString("Edad", fontPequeno, brush, x + 460, yPaciente + 15);
            
            g.DrawRectangle(pen, x + 490, yPaciente, 40, 35);
            g.DrawString("Sexo", fontPequeno, brush, x + 500, yPaciente + 15);

            // TABLA DE MEDICAMENTOS (parte derecha)
            int yMedicamentos = yPaciente + 40;
            int xMedicamentos = x + 450;
            
            // Headers de medicamentos
            g.DrawRectangle(pen, xMedicamentos, yMedicamentos, 60, 25);
            g.DrawString("CANTIDAD", fontPequeno, brush, xMedicamentos + 5, yMedicamentos + 5);
            g.DrawString("RECETADA", fontPequeno, brush, xMedicamentos + 5, yMedicamentos + 15);
            
            g.DrawRectangle(pen, xMedicamentos + 60, yMedicamentos, 80, 25);
            g.DrawString("N° EN LETRAS", fontPequeno, brush, xMedicamentos + 75, yMedicamentos + 10);
            
            g.DrawRectangle(pen, xMedicamentos + 140, yMedicamentos, 70, 25);
            g.DrawString("PRECIO", fontPequeno, brush, xMedicamentos + 155, yMedicamentos + 5);
            g.DrawString("UNITARIO", fontPequeno, brush, xMedicamentos + 155, yMedicamentos + 15);
            
            g.DrawRectangle(pen, xMedicamentos + 210, yMedicamentos, 30, 25);
            g.DrawString("%", fontPequeno, brush, xMedicamentos + 220, yMedicamentos + 10);
            
            g.DrawRectangle(pen, xMedicamentos + 240, yMedicamentos, 60, 25);
            g.DrawString("IMPORTE", fontPequeno, brush, xMedicamentos + 255, yMedicamentos + 10);
            
            // Columna OSECAC
            g.DrawRectangle(pen, xMedicamentos + 300, yMedicamentos, 70, 25);
            g.DrawString("OSECAC", fontPequeno, brush, xMedicamentos + 320, yMedicamentos + 10);
            
            // Filas de datos de medicamentos (2 filas según imagen)
            for (int i = 0; i < 2; i++)
            {
                int yFila = yMedicamentos + 25 + (i * 25);
                g.DrawRectangle(pen, xMedicamentos, yFila, 60, 25);
                g.DrawRectangle(pen, xMedicamentos + 60, yFila, 80, 25);
                g.DrawRectangle(pen, xMedicamentos + 140, yFila, 70, 25);
                g.DrawRectangle(pen, xMedicamentos + 210, yFila, 30, 25);
                g.DrawRectangle(pen, xMedicamentos + 240, yFila, 60, 25);
            }
            
            // Subcampos de OSECAC
            g.DrawRectangle(pen, xMedicamentos + 300, yMedicamentos + 25, 70, 25);
            g.DrawString("A/c", fontPequeno, brush, xMedicamentos + 310, yMedicamentos + 32);
            g.DrawString("C.E.C", fontPequeno, brush, xMedicamentos + 330, yMedicamentos + 32);
            
            g.DrawRectangle(pen, xMedicamentos + 300, yMedicamentos + 50, 70, 25);
            g.DrawString("A/c", fontPequeno, brush, xMedicamentos + 310, yMedicamentos + 57);
            g.DrawString("Farmacia", fontPequeno, brush, xMedicamentos + 325, yMedicamentos + 57);
            
            // Abonado por el Afiliado (cuadro grande)
            g.DrawRectangle(pen, xMedicamentos + 300, yMedicamentos + 75, 70, 60);
            g.DrawString("Abonado", fontPequeno, brush, xMedicamentos + 315, yMedicamentos + 90);
            g.DrawString("por el", fontPequeno, brush, xMedicamentos + 320, yMedicamentos + 105);
            g.DrawString("Afiliado", fontPequeno, brush, xMedicamentos + 315, yMedicamentos + 120);

            // RECETAS (lado izquierdo)
            int yRecetas = yPaciente + 40;
            
            // (1) Rp.
            g.DrawRectangle(pen, x + 10, yRecetas, 430, 30);
            g.DrawString("(1) Rp.", fontNormal, brush, x + 15, yRecetas + 10);

            // (2) Rp.
            g.DrawRectangle(pen, x + 10, yRecetas + 30, 430, 30);
            g.DrawString("(2) Rp.", fontNormal, brush, x + 15, yRecetas + 40);

            // DIAGNOSTICOS
            int yDiagnostico = yRecetas + 70;
            
            // Diagnóstico (1)
            g.DrawRectangle(pen, x + 10, yDiagnostico, 220, 60);
            g.DrawString("Diagnóstico (1)", fontNormal, brush, x + 15, yDiagnostico + 10);

            // Diagnóstico (2)
            g.DrawRectangle(pen, x + 240, yDiagnostico, 200, 30);
            g.DrawString("Diagnóstico (2)", fontNormal, brush, x + 245, yDiagnostico + 10);

            // FIRMAS (parte inferior)
            int yFirmas = yDiagnostico + 70;
            
            // Conformidad (cuadro con sello circular)
            g.DrawRectangle(pen, x + 10, yFirmas, 100, 60);
            g.DrawString("Conformidad", fontPequeno, brush, x + 30, yFirmas + 30);
            
            // Dibujar círculo del sello
            g.DrawEllipse(pen, x + 25, yFirmas + 10, 70, 40);

            // Fecha sello y Firma del Profesional
            g.DrawRectangle(pen, x + 110, yFirmas, 170, 30);
            g.DrawString("Fecha sello y Firma del Profesional", fontPequeno, brush, x + 115, yFirmas + 10);

            // Fecha Doc N° - Firma del Afiliado
            g.DrawRectangle(pen, x + 280, yFirmas, 160, 30);
            g.DrawString("Fecha Doc N° - Firma del Afiliado", fontPequeno, brush, x + 285, yFirmas + 10);

            // Segunda fila de firmas
            g.DrawRectangle(pen, x + 110, yFirmas + 30, 330, 30);

            // PIE DE PÁGINA
            string piePagina = $"Concepción - Serie A 78.000 - 79000 JULIO / {DateTime.Now.Year}";
            g.DrawString(piePagina, fontPequeno, brush, x + 200, y + alto + 5);
        }

        private void LimpiarDatos()
        {
            lblNombre.Text = "-";
            lblEmpresa.Text = "-";
            lblGrupoFamiliar.Text = "-";
            lblRecetariosEmitidos.Text = "-";
            lblMensaje.Text = "";
            btnImprimir.Enabled = false;
            afiliadoSeleccionado = null;
        }

        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            txtDNI.Clear();
            LimpiarDatos();
            txtDNI.Focus();
        }

    }
}
