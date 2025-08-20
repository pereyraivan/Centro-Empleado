using System.Drawing;
using System.Drawing.Printing;
using System.Windows.Forms;
using Centro_Empleado.Models;


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
