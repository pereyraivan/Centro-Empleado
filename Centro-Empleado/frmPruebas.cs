using System;
using System.IO;
using System.Windows.Forms;
using Centro_Empleado.Data;
using Centro_Empleado.Models;

namespace Centro_Empleado
{
    public partial class frmPruebas : Form
    {
        private DatabaseManager dbManager;

        public frmPruebas()
        {
            InitializeComponent();
            dbManager = new DatabaseManager();
            CargarAfiliados();
        }

        private void CargarAfiliados()
        {
            var afiliados = dbManager.ObtenerTodosLosAfiliados();
            cboAfiliados.Items.Clear();
            foreach (var afiliado in afiliados)
            {
                cboAfiliados.Items.Add($"{afiliado.Id} - {afiliado.ApellidoNombre} ({(afiliado.TieneGrupoFamiliar ? "Grupo Familiar" : "Individual")})");
            }
        }

        private void btnVerRecetarios_Click(object sender, EventArgs e)
        {
            if (cboAfiliados.SelectedIndex == -1)
            {
                MessageBox.Show("Seleccione un afiliado", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var afiliadoId = ObtenerIdAfiliado();
            var recetarios = dbManager.ObtenerInfoRecetariosParaPruebas(afiliadoId);
            
            string info = $"Recetarios del afiliado ID: {afiliadoId}\n\n";
            foreach (var rec in recetarios)
            {
                info += $"ID: {rec.Id}, Nº: {rec.NumeroTalonario:D6}, Emisión: {rec.FechaEmision:dd/MM/yyyy}, Vencimiento: {rec.FechaVencimiento:dd/MM/yyyy}\n";
            }
            
            if (recetarios.Count == 0)
                info += "No tiene recetarios registrados.";
            
            MessageBox.Show(info, "Información de Recetarios", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnModificarFecha_Click(object sender, EventArgs e)
        {
            if (cboAfiliados.SelectedIndex == -1)
            {
                MessageBox.Show("Seleccione un afiliado", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var afiliadoId = ObtenerIdAfiliado();
            var recetarios = dbManager.ObtenerInfoRecetariosParaPruebas(afiliadoId);
            
            if (recetarios.Count == 0)
            {
                MessageBox.Show("Este afiliado no tiene recetarios para modificar", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Mostrar lista de recetarios para seleccionar
            string opciones = "Seleccione el recetario a modificar:\n\n";
            foreach (var rec in recetarios)
            {
                opciones += $"{rec.Id} - Nº {rec.NumeroTalonario:D6} (Emisión: {rec.FechaEmision:dd/MM/yyyy})\n";
            }

            var input = Microsoft.VisualBasic.Interaction.InputBox(opciones, "Seleccionar Recetario", "1");
            if (int.TryParse(input, out int recetarioId))
            {
                var recetario = recetarios.Find(r => r.Id == recetarioId);
                if (recetario != null)
                {
                    var fechaInput = Microsoft.VisualBasic.Interaction.InputBox(
                        $"Ingrese la nueva fecha de emisión para el recetario Nº {recetario.NumeroTalonario:D6}\n\nFormato: dd/mm/yyyy", 
                        "Nueva Fecha", 
                        DateTime.Now.ToString("dd/MM/yyyy"));
                    
                    if (DateTime.TryParse(fechaInput, out DateTime nuevaFecha))
                    {
                        try
                        {
                            dbManager.ModificarFechaEmisionRecetario(recetarioId, nuevaFecha);
                            MessageBox.Show($"Fecha modificada correctamente a: {nuevaFecha:dd/MM/yyyy}", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show($"Error al modificar fecha: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    else
                    {
                        MessageBox.Show("Formato de fecha inválido", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    MessageBox.Show("ID de recetario no válido", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnVerificarHabilitacion_Click(object sender, EventArgs e)
        {
            if (cboAfiliados.SelectedIndex == -1)
            {
                MessageBox.Show("Seleccione un afiliado", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var afiliadoId = ObtenerIdAfiliado();
            var afiliado = dbManager.ObtenerTodosLosAfiliados().Find(a => a.Id == afiliadoId);
            
            int recetariosImpresos = dbManager.ContarRecetariosMensuales(afiliadoId, DateTime.Now);
            DateTime? proximaHabilitacion = dbManager.FechaProximaHabilitacion(afiliadoId);
            DateTime? ultimaImpresion = dbManager.ObtenerUltimaFechaImpresion(afiliadoId);
            
            string info = $"INFORMACIÓN DE HABILITACIÓN\n\n";
            info += $"Afiliado: {afiliado.ApellidoNombre}\n";
            info += $"Tipo: {(afiliado.TieneGrupoFamiliar ? "Grupo Familiar (4 recetas)" : "Individual (2 recetas)")}\n";
            info += $"Recetarios impresos en período actual: {recetariosImpresos}\n";
            info += $"Última impresión: {(ultimaImpresion.HasValue ? ultimaImpresion.Value.ToString("dd/MM/yyyy") : "Nunca")}\n";
            info += $"Próxima habilitación: {(proximaHabilitacion.HasValue ? proximaHabilitacion.Value.ToString("dd/MM/yyyy") : "Puede imprimir ahora")}\n";
            
            if (ultimaImpresion.HasValue)
            {
                var diasTranscurridos = (DateTime.Now - ultimaImpresion.Value).Days;
                info += $"Días transcurridos desde última impresión: {diasTranscurridos}\n";
            }
            
            MessageBox.Show(info, "Estado de Habilitación", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private int ObtenerIdAfiliado()
        {
            var item = cboAfiliados.SelectedItem.ToString();
            return int.Parse(item.Split('-')[0].Trim());
        }

        private void btnRecargarDatos_Click(object sender, EventArgs e)
        {
            try
            {
   
                CargarAfiliados();
                MessageBox.Show("Datos recargados desde la base de datos", "Recarga completada", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al recargar datos: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnInfoBaseDatos_Click(object sender, EventArgs e)
        {
            try
            {
                string info = dbManager.ObtenerInfoBaseDatos();
                MessageBox.Show(info, "Información de Base de Datos", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al obtener información: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnEliminarAfiliado_Click(object sender, EventArgs e)
        {
            if (cboAfiliados.SelectedIndex == -1)
            {
                MessageBox.Show("Seleccione un afiliado", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var afiliadoId = ObtenerIdAfiliado();
            var afiliado = dbManager.ObtenerTodosLosAfiliados().Find(a => a.Id == afiliadoId);
            
            if (afiliado == null)
            {
                MessageBox.Show("No se pudo obtener información del afiliado", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Confirmar eliminación
            string mensaje = $"¿Está seguro que desea eliminar al afiliado:\n\n";
            mensaje += $"ID: {afiliado.Id}\n";
            mensaje += $"Nombre: {afiliado.ApellidoNombre}\n";
            mensaje += $"DNI: {afiliado.DNI}\n";
            mensaje += $"Empresa: {afiliado.Empresa}\n";
            mensaje += $"Tipo: {(afiliado.TieneGrupoFamiliar ? "Grupo Familiar" : "Individual")}\n\n";
            mensaje += "⚠️ ADVERTENCIA: Esta acción eliminará TODOS los recetarios asociados y NO se puede deshacer.";

            var confirm = MessageBox.Show(mensaje, "Confirmar Eliminación", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            
            if (confirm == DialogResult.Yes)
            {
                try
                {
                    bool eliminado = dbManager.EliminarAfiliado(afiliadoId);
                    
                    if (eliminado)
                    {
                        MessageBox.Show("Afiliado eliminado correctamente junto con todos sus recetarios.", 
                            "Eliminación Exitosa", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        
                        // Recargar la lista de afiliados
                        CargarAfiliados();
                    }
                    else
                    {
                        MessageBox.Show("No se pudo eliminar el afiliado.", 
                            "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error al eliminar afiliado: {ex.Message}", 
                        "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }



        private void btnCerrar_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
