using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Centro_Empleado
{
    public partial class frmHistorialExtraordinarias : Form
    {
        private Data.DatabaseManager dbManager;
        private int idAfiliado;
        private string nombreAfiliado;

        public frmHistorialExtraordinarias(int idAfiliado, string nombreAfiliado)
        {
            InitializeComponent();
            this.idAfiliado = idAfiliado;
            this.nombreAfiliado = nombreAfiliado;
            this.dbManager = new Data.DatabaseManager();
            
            lblAfiliado.Text = string.Format("Afiliado: {0}", nombreAfiliado);
            CargarHistorial();
        }

        private void CargarHistorial()
        {
            try
            {
                var historial = dbManager.ObtenerHistorialRecetasExtraordinarias(idAfiliado);
                
                if (historial.Count == 0)
                {
                    dgvHistorial.DataSource = null;
                    lblMensaje.Text = "No hay recetas extraordinarias registradas para este afiliado.";
                    lblMensaje.Visible = true;
                    return;
                }

                lblMensaje.Visible = false;
                
                // Crear lista para la grilla
                var lista = new List<dynamic>();
                foreach (var item in historial)
                {
                    lista.Add(new
                    {
                        FechaImpresion = item.FechaImpresion.ToString("dd/MM/yyyy HH:mm"),
                        NumeroRecetario = item.NumeroRecetario.ToString("D6"),
                        Motivo = item.Motivo
                    });
                }
                
                dgvHistorial.DataSource = lista;
                
                // Configurar encabezados
                if (dgvHistorial.Columns.Contains("FechaImpresion"))
                    dgvHistorial.Columns["FechaImpresion"].HeaderText = "Fecha y Hora";
                if (dgvHistorial.Columns.Contains("NumeroRecetario"))
                    dgvHistorial.Columns["NumeroRecetario"].HeaderText = "N° Recetario";
                if (dgvHistorial.Columns.Contains("Motivo"))
                    dgvHistorial.Columns["Motivo"].HeaderText = "Motivo";
                
                // Ajustar ancho de columnas
                dgvHistorial.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                if (dgvHistorial.Columns.Contains("Motivo"))
                    dgvHistorial.Columns["Motivo"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            }
            catch (Exception ex)
            {
                MessageBox.Show(string.Format("Error al cargar el historial: {0}", ex.Message), 
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnCerrar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnActualizar_Click(object sender, EventArgs e)
        {
            CargarHistorial();
        }
    }
}
