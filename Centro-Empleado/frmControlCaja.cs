using System;
using System.Windows.Forms;
using Centro_Empleado.Data;

namespace Centro_Empleado
{
    public partial class frmControlCaja : Form
    {
        private DatabaseManager dbManager;

        public frmControlCaja()
        {
            InitializeComponent();
            dbManager = new DatabaseManager();
            InicializarFormulario();
        }

        private void InicializarFormulario()
        {
            // Configurar fechas por defecto (mes actual)
            dtpFechaDesde.Value = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            dtpFechaHasta.Value = DateTime.Now;
            
            // Configurar eventos
            btnConsultar.Click += btnConsultar_Click;
            btnExportar.Click += btnExportar_Click;
            btnLimpiar.Click += btnLimpiar_Click;
            
            // Configurar grillas
            ConfigurarGrillaBonos();
            ConfigurarGrillaResumen();
            
            // Cargar datos iniciales
            CargarDatos();
        }

        private void ConfigurarGrillaBonos()
        {
            dgvBonos.AutoGenerateColumns = false;
            
            dgvBonos.Columns.Clear();
            dgvBonos.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "NumeroBono",
                HeaderText = "N° Bono",
                Width = 120
            });
            dgvBonos.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "FechaEmision",
                HeaderText = "Fecha",
                Width = 100,
                DefaultCellStyle = new DataGridViewCellStyle { Format = "dd/MM/yyyy" }
            });
            dgvBonos.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "ApellidoNombre",
                HeaderText = "Apellido y Nombre",
                Width = 200
            });
            dgvBonos.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "DNI",
                HeaderText = "DNI",
                Width = 100
            });
            dgvBonos.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "Concepto",
                HeaderText = "Concepto",
                Width = 200
            });
            dgvBonos.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "Monto",
                HeaderText = "Monto",
                Width = 100,
                DefaultCellStyle = new DataGridViewCellStyle { Format = "C2" }
            });
        }

        private void ConfigurarGrillaResumen()
        {
            dgvResumen.AutoGenerateColumns = false;
            
            dgvResumen.Columns.Clear();
            dgvResumen.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "Fecha",
                HeaderText = "Fecha",
                Width = 120,
                DefaultCellStyle = new DataGridViewCellStyle { Format = "dd/MM/yyyy" }
            });
            dgvResumen.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "CantidadBonos",
                HeaderText = "Cant. Bonos",
                Width = 100
            });
            dgvResumen.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "TotalDia",
                HeaderText = "Total del Día",
                Width = 120,
                DefaultCellStyle = new DataGridViewCellStyle { Format = "C2" }
            });
        }

        private void btnConsultar_Click(object sender, EventArgs e)
        {
            if (dtpFechaDesde.Value > dtpFechaHasta.Value)
            {
                MessageBox.Show("La fecha desde no puede ser mayor que la fecha hasta.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            CargarDatos();
        }

        private void CargarDatos()
        {
            try
            {
                Cursor = Cursors.WaitCursor;
                
                DateTime fechaDesde = dtpFechaDesde.Value.Date;
                DateTime fechaHasta = dtpFechaHasta.Value.Date;
                
                // Cargar bonos detallados
                var bonos = dbManager.ObtenerBonosPorRangoFechas(fechaDesde, fechaHasta);
                dgvBonos.DataSource = bonos;
                
                // Cargar resumen por día
                var resumen = dbManager.ObtenerResumenCajaPorDia(fechaDesde, fechaHasta);
                dgvResumen.DataSource = resumen;
                
                // Mostrar total general
                decimal totalGeneral = dbManager.ObtenerTotalCajaPorRangoFechas(fechaDesde, fechaHasta);
                lblTotalGeneral.Text = string.Format("Total General: ${0:F2}", totalGeneral);
                
                // Mostrar cantidad de bonos
                lblCantidadBonos.Text = string.Format("Cantidad de Bonos: {0}", bonos.Count);
                
                // Personalizar grillas
                PersonalizarGrillas();
            }
            catch (Exception ex)
            {
                MessageBox.Show(string.Format("Error al cargar datos: {0}", ex.Message), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }

        private void PersonalizarGrillas()
        {
            // Personalizar grilla de bonos
            if (dgvBonos.Columns.Contains("Monto"))
            {
                dgvBonos.Columns["Monto"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            }
            
            // Personalizar grilla de resumen
            if (dgvResumen.Columns.Contains("TotalDia"))
            {
                dgvResumen.Columns["TotalDia"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            }
            
            // Ocultar columna Id si existe
            if (dgvBonos.Columns.Contains("Id"))
            {
                dgvBonos.Columns["Id"].Visible = false;
            }
        }

        private void btnExportar_Click(object sender, EventArgs e)
        {
            try
            {
                SaveFileDialog saveDialog = new SaveFileDialog();
                saveDialog.Filter = "Archivo CSV (*.csv)|*.csv|Todos los archivos (*.*)|*.*";
                saveDialog.FileName = string.Format("ControlCaja_{0:yyyyMMdd}_{1:yyyyMMdd}.csv", dtpFechaDesde.Value, dtpFechaHasta.Value);
                
                if (saveDialog.ShowDialog() == DialogResult.OK)
                {
                    ExportarACSV(saveDialog.FileName);
                    MessageBox.Show("Datos exportados correctamente.", "Exportación exitosa", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(string.Format("Error al exportar: {0}", ex.Message), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ExportarACSV(string rutaArchivo)
        {
            var bonos = (System.Collections.IList)dgvBonos.DataSource;
            if (bonos == null || bonos.Count == 0) return;
            
            using (var writer = new System.IO.StreamWriter(rutaArchivo, false, System.Text.Encoding.UTF8))
            {
                // Agregar BOM para que Excel reconozca UTF-8
                writer.Write('\uFEFF');
                
                // Encabezados - usar punto y coma como separador
                writer.WriteLine("N° Bono;Fecha;Apellido y Nombre;DNI;Concepto;Monto");
                
                // Datos
                decimal totalMonto = 0;
                foreach (dynamic bono in bonos)
                {
                    writer.WriteLine(string.Format("{0};{1:dd/MM/yyyy};{2};{3};{4};{5:F2}", 
                        bono.NumeroBono, bono.FechaEmision, bono.ApellidoNombre, bono.DNI, bono.Concepto, bono.Monto));
                    totalMonto += bono.Monto;
                }
                
                // Fila de total
                writer.WriteLine(string.Format("TOTAL;;;;;{0:F2}", totalMonto));
                writer.WriteLine(string.Format("CANTIDAD DE BONOS;{0};;;;;", bonos.Count));
            }
        }

        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            dtpFechaDesde.Value = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            dtpFechaHasta.Value = DateTime.Now;
            dgvBonos.DataSource = null;
            dgvResumen.DataSource = null;
            lblTotalGeneral.Text = "Total General: $0.00";
            lblCantidadBonos.Text = "Cantidad de Bonos: 0";
        }
    }
}
