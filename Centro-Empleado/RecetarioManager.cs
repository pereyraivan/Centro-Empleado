using System.Drawing;
using System.Drawing.Printing;
using System.Windows.Forms;
using Centro_Empleado.Models;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Diagnostics;
using System;
using System.Linq; // Added for .Select()

namespace Centro_Empleado
{
    public class RecetarioManager
    {
        public void ImprimirRecetario(Form parent, Recetario recetario, Afiliado afiliado)
        {
            PrintDocument printDoc = new PrintDocument();
            printDoc.PrintPage += (s, e) => DibujarRecetario(e.Graphics, recetario, afiliado);

            PrintPreviewDialog preview = new PrintPreviewDialog();
            preview.Document = printDoc;
            preview.Width = 1000;
            preview.Height = 700;
            preview.ShowDialog(parent);
        }

        // Nuevo método para generar HTML con múltiples recetas
        public void GenerarHTMLConRecetas(List<Recetario> recetarios, Afiliado afiliado, bool imprimirDosPorHoja = true)
        {
            try
            {
                string templatePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources", "recetaFinal.html");
                if (!File.Exists(templatePath))
                {
                    throw new FileNotFoundException("No se encontró la plantilla HTML de receta.");
                }

                string templateHtml = File.ReadAllText(templatePath);
                string htmlFinal = "";

                // Procesar recetarios según la configuración de impresión
                if (imprimirDosPorHoja)
                {
                    // Imprimir 2 por hoja
                    for (int i = 0; i < recetarios.Count; i += 2)
                    {
                        // Crear una copia del template para esta página
                        string htmlPagina = templateHtml;
                        
                        // Primera receta de la página
                        var recetario1 = recetarios[i];
                        htmlPagina = ReemplazarPlaceholdersPrimeraReceta(htmlPagina, recetario1, afiliado);
                        
                        // Segunda receta de la página (si existe)
                        if (i + 1 < recetarios.Count)
                        {
                            var recetario2 = recetarios[i + 1];
                            htmlPagina = ReemplazarPlaceholdersSegundaReceta(htmlPagina, recetario2, afiliado);
                        }
                        else
                        {
                            // Si no hay segunda receta, usar los mismos datos del primer recetario
                            htmlPagina = ReemplazarPlaceholdersSegundaReceta(htmlPagina, recetario1, afiliado);
                        }
                        
                        // Corregir la ruta del logo
                        htmlPagina = CorregirRutaLogo(htmlPagina);
                        
                        // Agregar comentario de depuración
                        htmlPagina = htmlPagina.Replace("</head>", "<!-- MODO: 2 RECETARIOS POR HOJA -->\n</head>");
                        
                        htmlFinal += htmlPagina;
                        
                        // Agregar salto de página si no es la última página
                        if (i + 2 < recetarios.Count)
                        {
                            htmlFinal += "<div style='page-break-before: always;'></div>";
                        }
                    }
                }
                else
                {
                    // Imprimir 1 por hoja
                    for (int i = 0; i < recetarios.Count; i++)
                    {
                        // Crear una copia del template para esta página
                        string htmlPagina = templateHtml;
                        
                        // Solo la primera receta
                        var recetario1 = recetarios[i];
                        htmlPagina = ReemplazarPlaceholdersPrimeraReceta(htmlPagina, recetario1, afiliado);
                        
                        // Limpiar el segundo formulario para que quede en blanco
                        htmlPagina = LimpiarSegundoFormulario(htmlPagina);
                        
                        // Agregar comentario de depuración
                        htmlPagina = htmlPagina.Replace("</head>", "<!-- MODO: 1 RECETARIO POR HOJA -->\n</head>");
                        
                        // Corregir la ruta del logo
                        htmlPagina = CorregirRutaLogo(htmlPagina);
                        
                        htmlFinal += htmlPagina;
                        
                        // Agregar salto de página si no es la última página
                        if (i + 1 < recetarios.Count)
                        {
                            htmlFinal += "<div style='page-break-before: always;'></div>";
                        }
                    }
                }

                // Guardar y abrir el archivo HTML
                string tempFile = Path.Combine(Path.GetTempPath(), string.Format("recetas_{0}_{1}.html", afiliado.Id, DateTime.Now.Ticks));
                File.WriteAllText(tempFile, htmlFinal);
                Process.Start(new ProcessStartInfo(tempFile) { UseShellExecute = true });
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("Error al generar el archivo HTML: {0}", ex.Message), ex);
            }
        }

        private string ReemplazarPlaceholdersPrimeraReceta(string html, Recetario recetario, Afiliado afiliado)
        {
            // Reemplazar placeholders de la primera receta (sin sufijo)
            html = html.Replace("{{NUMERO_TALONARIO}}", recetario.NumeroTalonario.ToString("D6"));
            html = html.Replace("{{DNI}}", WebUtility.HtmlEncode(afiliado.DNI ?? ""));
            html = html.Replace("{{FECHA_EMISION}}", recetario.FechaEmision.ToString("dd/MM/yyyy"));
            html = html.Replace("{{FECHA_VENCIMIENTO}}", recetario.FechaVencimiento.ToString("dd/MM/yyyy"));
            html = html.Replace("{{APELLIDO_NOMBRE}}", WebUtility.HtmlEncode(afiliado.ApellidoNombre ?? ""));
            html = html.Replace("{{EMPRESA}}", WebUtility.HtmlEncode(afiliado.Empresa ?? ""));
            
            return html;
        }

        private string ReemplazarPlaceholdersSegundaReceta(string html, Recetario recetario, Afiliado afiliado)
        {
            // Reemplazar placeholders de la segunda receta (con sufijo SEGUNDO)
            html = html.Replace("{{NUMERO_TALONARIO_SEGUNDO}}", recetario.NumeroTalonario.ToString("D6"));
            html = html.Replace("{{DNI_SEGUNDO}}", WebUtility.HtmlEncode(afiliado.DNI ?? ""));
            html = html.Replace("{{FECHA_EMISION_SEGUNDO}}", recetario.FechaEmision.ToString("dd/MM/yyyy"));
            html = html.Replace("{{FECHA_VENCIMIENTO_SEGUNDO}}", recetario.FechaVencimiento.ToString("dd/MM/yyyy"));
            html = html.Replace("{{APELLIDO_NOMBRE_SEGUNDO}}", WebUtility.HtmlEncode(afiliado.ApellidoNombre ?? ""));
            html = html.Replace("{{EMPRESA_SEGUNDO}}", WebUtility.HtmlEncode(afiliado.Empresa ?? ""));
            
            return html;
        }

        private string LimpiarSegundoFormulario(string html)
        {
            // Limpiar los placeholders del segundo formulario si no hay segunda receta
            string[] placeholders = {
                "{{NUMERO_TALONARIO_SEGUNDO}}", "{{DNI_SEGUNDO}}", "{{FECHA_EMISION_SEGUNDO}}", 
                "{{FECHA_VENCIMIENTO_SEGUNDO}}", "{{APELLIDO_NOMBRE_SEGUNDO}}", "{{EMPRESA_SEGUNDO}}"
            };
            
            foreach (string placeholder in placeholders)
            {
                html = html.Replace(placeholder, "");
            }
            
            // Ocultar completamente el segundo formulario
            html = html.Replace("<div class=\"form-container\" id=\"segundo-formulario\">", 
                               "<div class=\"form-container\" id=\"segundo-formulario\" style=\"display: none !important;\">");
            
            return html;
        }


        private string CorregirRutaLogo(string html)
        {
            // Buscar el logo en diferentes ubicaciones posibles
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
                html = html.Replace("<img src=\"logo_cec1.png\" alt=\"Logo\" style=\"width: 40px; height: 40px; object-fit: contain;\">", 
                                   "<div style=\"width: 40px; height: 40px; border: 1px solid #000; display: flex; align-items: center; justify-content: center; font-weight: bold; font-size: 12px; background-color: #f0f0f0;\">CEC</div>");
                
                // También reemplazar en el segundo formulario si existe
                html = html.Replace("<img src=\"logo_cec1.png\" alt=\"Logo\" style=\"width: 40px; height: 40px; object-fit: contain;\">", 
                                   "<div style=\"width: 40px; height: 40px; border: 1px solid #000; display: flex; align-items: center; justify-content: center; font-weight: bold; font-size: 12px; background-color: #f0f0f0;\">CEC</div>");
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
                    html = html.Replace("<img src=\"logo_cec1.png\" alt=\"Logo\" style=\"width: 40px; height: 40px; object-fit: contain;\">", 
                                       "<div style=\"width: 40px; height: 40px; border: 1px solid #000; display: flex; align-items: center; justify-content: center; font-weight: bold; font-size: 12px; background-color: #f0f0f0;\">CEC</div>");
                }
            }

            return html;
        }

        private void DibujarRecetario(Graphics g, Recetario recetario, Afiliado afiliado)
        {
            // Medidas base
            int x = 20, y = 20, ancho = 900, alto = 350;
            Pen pen = new Pen(Color.Black, 1);
            Brush brush = Brushes.Black;
            Font fontTitulo = new Font("Arial", 14, FontStyle.Bold);
            Font fontSubtitulo = new Font("Arial", 12, FontStyle.Bold);
            Font fontNormal = new Font("Arial", 10);
            Font fontPequeno = new Font("Arial", 8);
            Font fontNumero = new Font("Arial", 16, FontStyle.Bold);

            // Borde principal
            g.DrawRectangle(pen, x, y, ancho, alto);

            // Logo y título
            g.DrawEllipse(pen, x + 10, y + 10, 50, 50);
            g.DrawString("CEC", new Font("Arial", 14, FontStyle.Bold), brush, x + 20, y + 25);
            g.DrawString("CENTRO DE EMPLEADOS DE", fontTitulo, brush, x + 70, y + 15);
            g.DrawString("COMERCIO DE CONCEPCIÓN", fontTitulo, brush, x + 70, y + 40);

            // Número de recetario
            g.DrawString("Nº", fontSubtitulo, brush, x + 400, y + 15);
            g.DrawString(recetario.NumeroTalonario.ToString("D6"), fontNumero, brush, x + 440, y + 10);

            // Cuadro información socio con líneas internas
            int cuadroX = x + 600;
            g.DrawRectangle(pen, cuadroX, y + 10, 280, 60);
            g.DrawLine(pen, cuadroX + 80, y + 10, cuadroX + 80, y + 70);
            g.DrawLine(pen, cuadroX + 190, y + 10, cuadroX + 190, y + 70);
            g.DrawString("SOCIO Nº", fontPequeno, brush, cuadroX + 5, y + 15);
            g.DrawString("FECHA DE EMISION", fontPequeno, brush, cuadroX + 90, y + 15);
            g.DrawString("VALIDO HASTA", fontPequeno, brush, cuadroX + 200, y + 15);

            // Datos principales
            int lineaY = y + 80;
            g.DrawString("Apellido y Nombre Titular", fontNormal, brush, x + 10, lineaY);
            g.DrawString(afiliado.ApellidoNombre, fontNormal, brush, x + 180, lineaY);
            g.DrawString("Lugar de trabajo", fontNormal, brush, x + 600, lineaY);
            g.DrawString(afiliado.Empresa ?? "", fontNormal, brush, x + 720, lineaY);

            // Tabla principal
            int tablaY = y + 105;
            int tablaAlto = 140;
            int[] anchosColumnas = { 280, 50, 50, 90, 90, 40, 80, 220 }; // Suma = 900
            int acumuladoAncho = x;

            // Borde de tabla
            g.DrawRectangle(pen, x, tablaY, ancho, tablaAlto);

            // Líneas verticales
            foreach (int anchoCol in anchosColumnas)
            {
                acumuladoAncho += anchoCol;
                g.DrawLine(pen, acumuladoAncho, tablaY, acumuladoAncho, tablaY + tablaAlto);
            }

            // Líneas horizontales
            g.DrawLine(pen, x, tablaY + 35, x + ancho, tablaY + 35);  // Debajo cabecera
            g.DrawLine(pen, x, tablaY + 70, x + ancho, tablaY + 70);  // Entre medicamentos
            g.DrawLine(pen, x, tablaY + 105, x + ancho, tablaY + 105); // Encima diagnósticos
            g.DrawLine(pen, x, tablaY + 140, x + ancho, tablaY + 140); // Borde inferior

            // Cabecera de tabla
            g.DrawString("Apellido y Nombre Paciente", fontNormal, brush, x + 5, tablaY + 5);
            g.DrawString("Edad", fontNormal, brush, x + 285, tablaY + 5);
            g.DrawString("Sexo", fontNormal, brush, x + 335, tablaY + 5);
            g.DrawString("CANTIDAD RECETADA", fontNormal, brush, x + 385, tablaY + 5);
            g.DrawString("PRECIO UNITARIO", fontNormal, brush, x + 475, tablaY + 5);
            g.DrawString("%", fontNormal, brush, x + 565, tablaY + 5);
            g.DrawString("IMPORTE", fontNormal, brush, x + 605, tablaY + 5);
            g.DrawString("OSECAC", fontNormal, brush, x + 685, tablaY + 5);

            // Subcabecera OSECAC
            g.DrawLine(pen, x + 685, tablaY + 20, x + 685 + 220, tablaY + 20);
            g.DrawString("AC C.E.C.", fontPequeno, brush, x + 685 + 5, tablaY + 22);
            g.DrawString("AC Farmacia", fontPequeno, brush, x + 685 + 110, tablaY + 22);

            // Contenido
            g.DrawString("(1) Rp.", fontNormal, brush, x + 5, tablaY + 40);
            g.DrawString("(2) Rp.", fontNormal, brush, x + 5, tablaY + 75);
            g.DrawString("Diagnóstico (1)", fontNormal, brush, x + 5, tablaY + 110);
            g.DrawString("Diagnóstico (2)", fontNormal, brush, x + 300, tablaY + 110);

            // Total y pie
            g.DrawString("Importe Total", fontNormal, brush, x + 600, y + 80);
            g.DrawString("Abonado por el Afiliado", fontNormal, brush, x + 685, tablaY + 125);

            // Pie de página
            g.DrawString("Conformidad", fontNormal, brush, x + 10, y + 320);
            g.DrawString("Fecha sello y Firma del Profesional", fontNormal, brush, x + 200, y + 320);
            g.DrawString("Fecha Doc N° - Firma del Afiliado", fontNormal, brush, x + 500, y + 320);
            g.DrawString("Concepción - Serie A 78.000 - 79000 JULIO / 2024", fontPequeno, brush, x + 500, y + 350);
        }
    }
}
