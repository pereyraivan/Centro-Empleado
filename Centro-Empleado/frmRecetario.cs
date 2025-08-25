using System;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.Windows.Forms;
using Centro_Empleado.Data;
using Centro_Empleado.Models;
using System.IO;
using System.Diagnostics;

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
                // int recetariosEmitidos = dbManager.ContarRecetariosMensuales(afiliadoSeleccionado.Id, DateTime.Now);
                // int limite = afiliadoSeleccionado.TieneGrupoFamiliar ? 4 : 2;
                // if (recetariosEmitidos >= limite - 1)
                // {
                //     MessageBox.Show("No puede imprimir 2 recetarios. Verificar límite mensual.", "Límite excedido", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                //     return;
                // }

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
                ImprimirRecetarioHTML(afiliadoSeleccionado, recetario);

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
            // Ajustar proporciones para A4 y usar fuentes itálicas
            // Reducir el tamaño general
            ancho = 650; // antes 750
            alto = 300;  // antes 400

            // Fuentes SOLO itálicas
            Font fontTituloItalic = new Font("Arial", 10, FontStyle.Italic);
            Font fontNormalItalic = new Font("Arial", 7, FontStyle.Italic);
            Font fontPequenoItalic = new Font("Arial", 6, FontStyle.Italic);
            Font fontNumeroItalic = new Font("Arial", 12, FontStyle.Italic);

            // Borde principal del recetario
            g.DrawRectangle(new Pen(Color.Black, 1.2f), x, y, ancho, alto);

            // HEADER - Primera fila con logo y título
            int yHeader = y + 8;
            int logoSize = 36;
            // Logo
            try
            {
                string logoPath = System.IO.Path.Combine(Application.StartupPath, "Resources", "logo_cec.png");
                if (!System.IO.File.Exists(logoPath))
                    logoPath = System.IO.Path.Combine(System.IO.Directory.GetCurrentDirectory(), "Resources", "logo_cec.png");
                if (System.IO.File.Exists(logoPath))
                {
                    using (var logo = Image.FromFile(logoPath))
                        g.DrawImage(logo, x + 8, yHeader, logoSize, logoSize);
                }
                else
                {
                    g.DrawEllipse(new Pen(Color.Black, 1.5f), x + 8, yHeader, logoSize, logoSize);
                    g.DrawString("CEC", new Font("Arial", 9, FontStyle.Bold | FontStyle.Italic), brush, x + 15, yHeader + 13);
                }
            }
            catch { g.DrawEllipse(pen, x + 8, yHeader, logoSize, logoSize); }

            // Título y número (en la misma línea, ocupando todo el ancho)
            g.DrawString("CENTRO DE EMPLEADOS DE", fontTituloItalic, brush, x + 55, yHeader + 2);
            g.DrawString("COMERCIO DE CONCEPCIÓN", fontTituloItalic, brush, x + 55, yHeader + 20);
            g.DrawString("Nº", fontTituloItalic, brush, x + 350, yHeader + 2);
            g.DrawString(recetario.NumeroTalonario.ToString("D6"), fontNumeroItalic, brush, x + 380, yHeader - 2);

            // Datos socio (alineados a la izquierda y con línea para completar)
            int datosSocioY = yHeader + 2;
            int datosSocioX = x + 55;
            int espacioY = 15;
            int lineaLargo = 70;
            int espacioEntreCampos = 120;
            // SOCIO Nº
            g.DrawString("SOCIO Nº", fontNormalItalic, brush, datosSocioX, datosSocioY);
            g.DrawLine(pen, datosSocioX + 55, datosSocioY + 10, datosSocioX + 55 + lineaLargo, datosSocioY + 10);
            // FECHA DE EMISIÓN
            g.DrawString("FECHA DE EMISIÓN", fontNormalItalic, brush, datosSocioX + espacioEntreCampos, datosSocioY);
            g.DrawLine(pen, datosSocioX + espacioEntreCampos + 95, datosSocioY + 10, datosSocioX + espacioEntreCampos + 95 + lineaLargo, datosSocioY + 10);
            // VÁLIDO HASTA
            g.DrawString("VÁLIDO HASTA", fontNormalItalic, brush, datosSocioX + espacioEntreCampos * 2, datosSocioY);
            g.DrawLine(pen, datosSocioX + espacioEntreCampos * 2 + 75, datosSocioY + 10, datosSocioX + espacioEntreCampos * 2 + 75 + lineaLargo, datosSocioY + 10);

            // Cuadros principales y textos (ajustados a menor tamaño)
            int yDatos = yHeader + 45;
            int altoFila = 18;
            int anchoCol1 = 180;
            int anchoCol2 = 120;
            int anchoCol3 = 80;
            int anchoCol4 = 80;
            int anchoCol5 = 80;

            // Apellido y Nombre Titular
            g.DrawRectangle(pen, x + 8, yDatos, anchoCol1, altoFila);
            g.DrawString("Apellido y Nombre Titular", fontNormalItalic, brush, x + 12, yDatos + 3);
            // Lugar de trabajo
            g.DrawRectangle(pen, x + 8 + anchoCol1, yDatos, anchoCol2, altoFila);
            g.DrawString("Lugar de trabajo", fontNormalItalic, brush, x + 12 + anchoCol1, yDatos + 3);
            // Importe Total
            g.DrawRectangle(pen, x + 8 + anchoCol1 + anchoCol2, yDatos, anchoCol3, altoFila);
            g.DrawString("Importe Total", fontNormalItalic, brush, x + 12 + anchoCol1 + anchoCol2, yDatos + 3);

            // Apellido y Nombre Paciente
            g.DrawRectangle(pen, x + 8, yDatos + altoFila, anchoCol1, altoFila);
            g.DrawString("Apellido y Nombre Paciente", fontNormalItalic, brush, x + 12, yDatos + altoFila + 3);
            // Edad
            g.DrawRectangle(pen, x + 8 + anchoCol1, yDatos + altoFila, 40, altoFila);
            g.DrawString("Edad", fontNormalItalic, brush, x + 12 + anchoCol1, yDatos + altoFila + 3);
            // Sexo
            g.DrawRectangle(pen, x + 8 + anchoCol1 + 40, yDatos + altoFila, 40, altoFila);
            g.DrawString("Sexo", fontNormalItalic, brush, x + 12 + anchoCol1 + 40, yDatos + altoFila + 3);
            // Cantidad Recetada
            g.DrawRectangle(pen, x + 8 + anchoCol1 + 80, yDatos + altoFila, anchoCol4, altoFila);
            g.DrawString("CANTIDAD RECETADA", fontNormalItalic, brush, x + 12 + anchoCol1 + 80, yDatos + altoFila + 3);
            // Precio Unitario
            g.DrawRectangle(pen, x + 8 + anchoCol1 + 80 + anchoCol4, yDatos + altoFila, anchoCol5, altoFila);
            g.DrawString("PRECIO UNITARIO", fontNormalItalic, brush, x + 12 + anchoCol1 + 80 + anchoCol4, yDatos + altoFila + 3);
            
            // (1) Rp.
            g.DrawRectangle(pen, x + 8, yDatos + altoFila * 2, ancho - 16, altoFila);
            g.DrawString("(1) Rp.", fontNormalItalic, brush, x + 12, yDatos + altoFila * 2 + 3);
            // (2) Rp.
            g.DrawRectangle(pen, x + 8, yDatos + altoFila * 3, ancho - 16, altoFila);
            g.DrawString("(2) Rp.", fontNormalItalic, brush, x + 12, yDatos + altoFila * 3 + 3);
            
            // Diagnóstico (1)
            g.DrawRectangle(pen, x + 8, yDatos + altoFila * 4, (ancho - 16) / 2, altoFila);
            g.DrawString("Diagnóstico (1)", fontNormalItalic, brush, x + 12, yDatos + altoFila * 4 + 3);
            // Diagnóstico (2)
            g.DrawRectangle(pen, x + 8 + (ancho - 16) / 2, yDatos + altoFila * 4, (ancho - 16) / 2, altoFila);
            g.DrawString("Diagnóstico (2)", fontNormalItalic, brush, x + 12 + (ancho - 16) / 2, yDatos + altoFila * 4 + 3);

            // Firmas
            int yFirmas = yDatos + altoFila * 5;
            int anchoFirma = (ancho - 16) / 3;
            g.DrawRectangle(pen, x + 8, yFirmas, anchoFirma, altoFila);
            g.DrawString("Conformidad", fontPequenoItalic, brush, x + 12, yFirmas + 3);
            g.DrawRectangle(pen, x + 8 + anchoFirma, yFirmas, anchoFirma, altoFila);
            g.DrawString("Fecha sello y Firma del Profesional", fontPequenoItalic, brush, x + 12 + anchoFirma, yFirmas + 3);
            g.DrawRectangle(pen, x + 8 + anchoFirma * 2, yFirmas, anchoFirma, altoFila);
            g.DrawString("Fecha Doc N° - Firma del Afiliado", fontPequenoItalic, brush, x + 12 + anchoFirma * 2, yFirmas + 3);

            // Pie de página
            string piePagina = $"Concepción - Serie A 78.000 - 79000 JULIO / {DateTime.Now.Year}";
            g.DrawString(piePagina, fontPequenoItalic, brush, x + ancho - 250, y + alto - 10);
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

        private string ReplaceFirst(string text, string search, string replace)
        {
            int pos = text.IndexOf(search);
            if (pos < 0) return text;
            return text.Substring(0, pos) + replace + text.Substring(pos + search.Length);
        }

        private void ImprimirRecetarioHTML(Afiliado afiliado, Recetario recetario)
        {
            // Ruta de la plantilla HTML
            string plantillaPath = Path.Combine(Application.StartupPath, "Resources", "recetaFinal.html");
            if (!File.Exists(plantillaPath))
            {
                MessageBox.Show("No se encontró la plantilla HTML de receta.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            string html = File.ReadAllText(plantillaPath);

            // Reemplazar los campos necesarios
            html = ReplaceFirst(html, "<input type=\"text\">", $"<span>{afiliado.ApellidoNombre}</span>"); // Apellido y Nombre Titular
            html = ReplaceFirst(html, "<input type=\"text\">", $"<span>{afiliado.Empresa}</span>"); // Lugar de trabajo
            html = html.Replace("Nº 080097", $"Nº {recetario.NumeroTalonario:D6}"); // Número de receta
            html = ReplaceFirst(html, "<input type=\"text\" class=\"field-input\">", $"<span>{afiliado.DNI}</span>"); // Socio Nº
            // Dejar FECHA DE EMISIÓN y VÁLIDO HASTA vacíos (no reemplazar)
            // No completar nada en (1) Rp. ni (2) Rp. (no reemplazar esos inputs)
            // Cambiar texto 'Abonado por el Afiliado' a 'Abona afiliado'
            html = html.Replace("Abonado por el Afiliado", "Abona afiliado");

            // Agregar script para imprimir automáticamente
            int headClose = html.IndexOf("</head>");
            if (headClose > 0)
            {
                html = html.Insert(headClose, "<script>window.onload=function(){window.print();}</script>");
            }

            // Guardar archivo temporal
            string tempFile = Path.Combine(Path.GetTempPath(), $"receta_{recetario.NumeroTalonario:D6}.html");
            File.WriteAllText(tempFile, html);

            // Abrir en navegador predeterminado
            Process.Start(new ProcessStartInfo(tempFile) { UseShellExecute = true });
        }

    }
}
