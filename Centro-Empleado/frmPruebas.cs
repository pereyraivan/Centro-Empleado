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
                cboAfiliados.Items.Add(string.Format("{0} - {1} ({2})", afiliado.Id, afiliado.ApellidoNombre, (afiliado.TieneGrupoFamiliar ? "Grupo Familiar" : "Individual")));
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
            
            string info = string.Format("Recetarios del afiliado ID: {0}\n\n", afiliadoId);
            foreach (var rec in recetarios)
            {
                info += string.Format("ID: {0}, Nº: {1:D6}, Emisión: {2:dd/MM/yyyy}, Vencimiento: {3:dd/MM/yyyy}\n", rec.Id, rec.NumeroTalonario, rec.FechaEmision, rec.FechaVencimiento);
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
                opciones += string.Format("{0} - Nº {1:D6} (Emisión: {2:dd/MM/yyyy})\n", rec.Id, rec.NumeroTalonario, rec.FechaEmision);
            }

            var input = Microsoft.VisualBasic.Interaction.InputBox(opciones, "Seleccionar Recetario", "1");
            int recetarioId;
            if (int.TryParse(input, out recetarioId))
            {
                var recetario = recetarios.Find(r => r.Id == recetarioId);
                if (recetario != null)
                {
                    var fechaInput = Microsoft.VisualBasic.Interaction.InputBox(
                        string.Format("Ingrese la nueva fecha de emisión para el recetario Nº {0:D6}\n\nFormato: dd/mm/yyyy", recetario.NumeroTalonario), 
                        "Nueva Fecha", 
                        DateTime.Now.ToString("dd/MM/yyyy"));
                    
                    DateTime nuevaFecha;
                    if (DateTime.TryParse(fechaInput, out nuevaFecha))
                    {
                        try
                        {
                            dbManager.ModificarFechaEmisionRecetario(recetarioId, nuevaFecha);
                            MessageBox.Show(string.Format("Fecha modificada correctamente a: {0:dd/MM/yyyy}", nuevaFecha), "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(string.Format("Error al modificar fecha: {0}", ex.Message), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
            
            string info = "INFORMACIÓN DE HABILITACIÓN\n\n";
            info += string.Format("Afiliado: {0}\n", afiliado.ApellidoNombre);
            info += string.Format("Tipo: {0}\n", (afiliado.TieneGrupoFamiliar ? "Grupo Familiar (4 recetas)" : "Individual (2 recetas)"));
            info += string.Format("Recetarios impresos en período actual: {0}\n", recetariosImpresos);
            info += string.Format("Última impresión: {0}\n", (ultimaImpresion.HasValue ? ultimaImpresion.Value.ToString("dd/MM/yyyy") : "Nunca"));
            info += string.Format("Próxima habilitación: {0}\n", (proximaHabilitacion.HasValue ? proximaHabilitacion.Value.ToString("dd/MM/yyyy") : "Puede imprimir ahora"));
            
            if (ultimaImpresion.HasValue)
            {
                var diasTranscurridos = (DateTime.Now - ultimaImpresion.Value).Days;
                info += string.Format("Días transcurridos desde última impresión: {0}\n", diasTranscurridos);
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
                MessageBox.Show(string.Format("Error al recargar datos: {0}", ex.Message), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                MessageBox.Show(string.Format("Error al obtener información: {0}", ex.Message), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
            string mensaje = "¿Está seguro que desea eliminar al afiliado:\n\n";
            mensaje += string.Format("ID: {0}\n", afiliado.Id);
            mensaje += string.Format("Nombre: {0}\n", afiliado.ApellidoNombre);
            mensaje += string.Format("DNI: {0}\n", afiliado.DNI);
            mensaje += string.Format("Empresa: {0}\n", afiliado.Empresa);
            mensaje += string.Format("Tipo: {0}\n\n", (afiliado.TieneGrupoFamiliar ? "Grupo Familiar" : "Individual"));
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
                    MessageBox.Show(string.Format("Error al eliminar afiliado: {0}", ex.Message), 
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
