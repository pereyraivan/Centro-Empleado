using System;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using Centro_Empleado.Data;
using Centro_Empleado.Models;
using System.Linq; // Added for .Select()

namespace Centro_Empleado
{
    public partial class frmAfiliado : Form
    {
        private DatabaseManager dbManager;
        private int? afiliadoSeleccionadoId = null;
        private bool editando = false;

        public frmAfiliado()
        {
            InitializeComponent();
            dbManager = new DatabaseManager();
            InicializarEventos();
            CargarAfiliados();
            btnEditar.Enabled = false;
            btnEliminar.Enabled = false;
            btnImprimir.Enabled = false;
        }

        private void InicializarEventos()
        {
            btnGuardar.Click += btnGuardar_Click;
            btnEditar.Click += btnEditar_Click;
            btnEliminar.Click += btnEliminar_Click;
            btnImprimir.Click += btnImprimir_Click;
            dgvAfiliados.CellDoubleClick += dgvAfiliados_CellDoubleClick;
            dgvAfiliados.SelectionChanged += dgvAfiliados_SelectionChanged;
            txtBuscar.TextChanged += txtBuscar_TextChanged;
            dgvAfiliados.DataBindingComplete += dgvAfiliados_DataBindingComplete;
            
            // Agregar eventos para el panel de operaciones
            btnOperaciones.Click += BtnOperaciones_Click;
            btnVerCaja.Click += BtnVerCaja_Click;
            btnImprimirCupon.Click += BtnImprimirCupon_Click;
            btnManual.Click += BtnManual_Click;
            
            // Agregar evento para abrir herramientas de pruebas (Ctrl+P)
            this.KeyPreview = true;
            this.KeyDown += FrmAfiliado_KeyDown;
            
            // Agregar evento para abrir bonos (Ctrl+B)
            this.KeyDown += FrmAfiliado_KeyDown;
        }

        private void FrmAfiliado_KeyDown(object sender, KeyEventArgs e)
        {
            // Abrir herramientas de pruebas con Ctrl+P
            if (e.Control && e.KeyCode == Keys.P)
            {
                var frmPruebas = new frmPruebas();
                frmPruebas.ShowDialog();
            }
            
            // Abrir bonos de cobro con Ctrl+B
            if (e.Control && e.KeyCode == Keys.B)
            {
                var frmBono = new frmBono();
                frmBono.ShowDialog();
            }
            
            // Abrir control de caja con Ctrl+C
            if (e.Control && e.KeyCode == Keys.C)
            {
                var frmControlCaja = new frmControlCaja();
                frmControlCaja.ShowDialog();
            }
            
            // Recargar datos con F5
            if (e.KeyCode == Keys.F5)
            {
                CargarAfiliados(txtBuscar.Text);
                MessageBox.Show("Datos recargados desde la base de datos", "Recarga completada", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            
            // COMBINACIÓN SECRETA: Limpiar base de datos con Ctrl+Shift+L
            if (e.Control && e.Shift && e.KeyCode == Keys.L)
            {
                LimpiarBaseDatosSecreta();
            }
        }

        private void CargarAfiliados(string filtro = "")
        {
            var afiliados = string.IsNullOrWhiteSpace(filtro)
                ? dbManager.ObtenerTodosLosAfiliados()
                : dbManager.ObtenerTodosLosAfiliados().FindAll(a =>
                    (a.ApellidoNombre != null && a.ApellidoNombre.ToLower().Contains(filtro.ToLower())) ||
                    (a.DNI != null && a.DNI.Contains(filtro)));

            // Crear lista anónima con fechas de último recetario y próxima habilitación
            var lista = new List<dynamic>();
            foreach (var a in afiliados)
            {
                var recetarios = dbManager.ObtenerRecetariosPorAfiliado(a.Id);
                DateTime? ultima = recetarios.Count > 0 ? (DateTime?)recetarios[0].FechaEmision : null;
                DateTime? proxima = dbManager.FechaProximaHabilitacion(a.Id);
                
                lista.Add(new
                {
                    a.Id,
                    a.ApellidoNombre,
                    a.DNI,
                    a.Empresa,
                    TieneFamiliar = a.TieneGrupoFamiliar ? "Sí" : "",
                    FechaUltimaImpresion = ultima.HasValue ? ultima.Value.ToString("dd/MM/yyyy") : "-",
                    ProximaHabilitacion = proxima.HasValue ? proxima.Value.ToString("dd/MM/yyyy") : "-"
                });
            }
            dgvAfiliados.DataSource = null;
            dgvAfiliados.DataSource = lista;
            dgvAfiliados.ClearSelection();
            PersonalizarEncabezadoGrilla();

            // Cambiar encabezados de columnas
            if (dgvAfiliados.Columns.Contains("ApellidoNombre"))
                dgvAfiliados.Columns["ApellidoNombre"].HeaderText = "Apellido y Nombre";
            if (dgvAfiliados.Columns.Contains("FechaUltimaImpresion"))
                dgvAfiliados.Columns["FechaUltimaImpresion"].HeaderText = "Última impresión";
            if (dgvAfiliados.Columns.Contains("ProximaHabilitacion"))
                dgvAfiliados.Columns["ProximaHabilitacion"].HeaderText = "Próxima habilitación (30 días)";
            if (dgvAfiliados.Columns.Contains("TieneFamiliar"))
            {
                dgvAfiliados.Columns["TieneFamiliar"].HeaderText = "Tiene familiar";
            }
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            if (!ValidarDatos()) return;
            var afiliado = new Afiliado
            {
                ApellidoNombre = txtApellidoNombre.Text.Trim(),
                DNI = txtDNI.Text.Trim(),
                Empresa = txtEmpresa.Text.Trim(),
                TieneGrupoFamiliar = chkGrupoFamiliar.Checked
            };
                try
                {
                    dbManager.InsertarAfiliado(afiliado);
                    MessageBox.Show("Afiliado registrado correctamente", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LimpiarFormulario();
                    CargarAfiliados(txtBuscar.Text);
                    PersonalizarEncabezadoGrilla();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error al guardar afiliado: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
        }

        private void btnEditar_Click(object sender, EventArgs e)
        {
            if (afiliadoSeleccionadoId == null) return;
            if (!ValidarDatos()) return;
            var afiliado = new Afiliado
            {
                Id = afiliadoSeleccionadoId.Value,
                ApellidoNombre = txtApellidoNombre.Text.Trim(),
                DNI = txtDNI.Text.Trim(),
                Empresa = txtEmpresa.Text.Trim(),
                TieneGrupoFamiliar = chkGrupoFamiliar.Checked
            };
            try
            {
                dbManager.ActualizarAfiliado(afiliado);
                MessageBox.Show("Afiliado actualizado correctamente", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LimpiarFormulario();
                CargarAfiliados(txtBuscar.Text);
                afiliadoSeleccionadoId = null;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al actualizar afiliado: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if (afiliadoSeleccionadoId == null) return;
            
            // Obtener información del afiliado para mostrar en la confirmación
            var afiliado = dbManager.ObtenerTodosLosAfiliados().Find(a => a.Id == afiliadoSeleccionadoId.Value);
            if (afiliado == null) return;
            
            // Verificar si tiene recetarios
            var recetarios = dbManager.ObtenerRecetariosPorAfiliado(afiliadoSeleccionadoId.Value);
            bool tieneRecetarios = recetarios.Count > 0;
            
            // Crear mensaje de confirmación
            string mensaje = $"¿Está seguro que desea eliminar al afiliado:\n\n";
            mensaje += $"Nombre: {afiliado.ApellidoNombre}\n";
            mensaje += $"DNI: {afiliado.DNI}\n";
            mensaje += $"Empresa: {afiliado.Empresa}\n";
            mensaje += $"Tipo: {(afiliado.TieneGrupoFamiliar ? "Grupo Familiar" : "Individual")}\n\n";
            
            if (tieneRecetarios)
            {
                mensaje += $"⚠️ ADVERTENCIA: Este afiliado tiene {recetarios.Count} recetario(s) impreso(s).\n";
                mensaje += $"Al eliminarlo, se eliminarán TODOS los recetarios asociados.\n\n";
            }
            
            mensaje += "Esta acción NO se puede deshacer. ¿Desea continuar?";
            
            var result = MessageBox.Show(mensaje, "Confirmar eliminación", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (result == DialogResult.Yes)
            {
                                 try
                 {
                     if (dbManager.EliminarAfiliado(afiliadoSeleccionadoId.Value))
                     {
                         string mensajeExito = "Afiliado eliminado correctamente";
                         if (tieneRecetarios)
                         {
                             mensajeExito += $" junto con {recetarios.Count} recetario(s)";
                         }
                         mensajeExito += ".";
                         
                         MessageBox.Show(mensajeExito, "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                         LimpiarFormulario();
                         CargarAfiliados(txtBuscar.Text);
                         btnEditar.Enabled = false;
                         btnEliminar.Enabled = false;
                         btnImprimir.Enabled = false;
                         afiliadoSeleccionadoId = null;
                     }
                     else
                     {
                         MessageBox.Show("No se pudo eliminar el afiliado.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                     }
                 }
                 catch (Exception ex)
                 {
                     MessageBox.Show($"Error al eliminar afiliado: {ex.Message}\n\nDetalles técnicos: {ex.InnerException?.Message ?? "Sin detalles adicionales"}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                 }
            }
        }

        private void dgvAfiliados_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                var row = dgvAfiliados.Rows[e.RowIndex].DataBoundItem;
                if (row != null)
                {
                    var tipo = row.GetType();
                    var idProp = tipo.GetProperty("Id");
                    var nombreProp = tipo.GetProperty("ApellidoNombre");
                    var dniProp = tipo.GetProperty("DNI");
                    var empresaProp = tipo.GetProperty("Empresa");
                    var tieneFamiliarProp = tipo.GetProperty("TieneFamiliar");
                    if (idProp != null && nombreProp != null && dniProp != null && empresaProp != null && tieneFamiliarProp != null)
                    {
                        afiliadoSeleccionadoId = (int)idProp.GetValue(row);
                        txtApellidoNombre.Text = nombreProp.GetValue(row)?.ToString() ?? "";
                        txtDNI.Text = dniProp.GetValue(row)?.ToString() ?? "";
                        txtEmpresa.Text = empresaProp.GetValue(row)?.ToString() ?? "";
                        // Si el valor es "Sí" entonces tiene grupo familiar
                        chkGrupoFamiliar.Checked = (tieneFamiliarProp.GetValue(row)?.ToString() ?? "") == "Sí";
                        btnEditar.Enabled = true;
                        btnGuardar.Enabled = false;
                        editando = true;
                    }
                }
            }
        }

        private void dgvAfiliados_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvAfiliados.SelectedRows.Count > 0)
            {
                var row = dgvAfiliados.SelectedRows[0].DataBoundItem;
                if (row != null)
                {
                    // Usar reflexión para obtener el Id de la fila dinámica
                    var idProp = row.GetType().GetProperty("Id");
                    if (idProp != null)
                    {
                        afiliadoSeleccionadoId = (int)idProp.GetValue(row);
                        btnEliminar.Enabled = true;
                        btnImprimir.Enabled = true;
                        return;
                    }
                }
            }
            afiliadoSeleccionadoId = null;
            btnEliminar.Enabled = false;
            btnImprimir.Enabled = false;
        }

        private void txtBuscar_TextChanged(object sender, EventArgs e)
        {
            CargarAfiliados(txtBuscar.Text);
        }

        private bool ValidarDatos()
        {
            if (string.IsNullOrWhiteSpace(txtApellidoNombre.Text))
            {
                MessageBox.Show("El apellido y nombre es obligatorio", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtApellidoNombre.Focus();
                return false;
            }
            if (string.IsNullOrWhiteSpace(txtDNI.Text))
            {
                MessageBox.Show("El DNI es obligatorio", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtDNI.Focus();
                return false;
            }
            if (string.IsNullOrWhiteSpace(txtEmpresa.Text))
            {
                MessageBox.Show("La empresa es obligatoria", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtEmpresa.Focus();
                return false;
            }
            return true;
        }

        private void LimpiarFormulario()
        {
            txtApellidoNombre.Clear();
            txtDNI.Clear();
            txtEmpresa.Clear();
            chkGrupoFamiliar.Checked = false;
            btnGuardar.Enabled = true;
            btnEditar.Enabled = false;
            btnEliminar.Enabled = false;
            btnImprimir.Enabled = false;
            editando = false;
            afiliadoSeleccionadoId = null;
            txtApellidoNombre.Focus();
        }

        private void dgvAfiliados_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            if (dgvAfiliados.Columns.Contains("Id"))
                dgvAfiliados.Columns["Id"].Visible = false;
        }

        private void PersonalizarEncabezadoGrilla()
        {
            var header = dgvAfiliados.ColumnHeadersDefaultCellStyle;
            header.BackColor = System.Drawing.Color.White;
            header.Font = new System.Drawing.Font(dgvAfiliados.Font, System.Drawing.FontStyle.Bold);
            header.Font = new System.Drawing.Font(header.Font.FontFamily, header.Font.Size + 1, System.Drawing.FontStyle.Bold);
            header.SelectionBackColor = header.BackColor; // Quitar azul de selección en encabezado
            header.SelectionForeColor = header.ForeColor;
            dgvAfiliados.EnableHeadersVisualStyles = false;
            // Aumentar tamaño de letra de las filas
            dgvAfiliados.DefaultCellStyle.Font = new System.Drawing.Font(dgvAfiliados.Font.FontFamily, dgvAfiliados.Font.Size + 1);
        }

        // ========================================
        // MÉTODOS PARA EL PANEL DE OPERACIONES
        // ========================================

        private void BtnOperaciones_Click(object sender, EventArgs e)
        {
            // Alternar la visibilidad del panel de operaciones
            panelOperaciones.Visible = !panelOperaciones.Visible;
            
            // Cambiar el texto del botón según el estado
            if (panelOperaciones.Visible)
            {
                btnOperaciones.Text = "▼ Operaciones";
            }
            else
            {
                btnOperaciones.Text = "▶ Operaciones";
            }
        }

        private void BtnVerCaja_Click(object sender, EventArgs e)
        {
            // Ocultar el panel de operaciones
            panelOperaciones.Visible = false;
            btnOperaciones.Text = "▶ Operaciones";
            
            // Abrir el control de caja
            var frmControlCaja = new frmControlCaja();
            frmControlCaja.ShowDialog();
        }

        private void BtnImprimirCupon_Click(object sender, EventArgs e)
        {
            // Ocultar el panel de operaciones
            panelOperaciones.Visible = false;
            btnOperaciones.Text = "▶ Operaciones";
            
            // Abrir el formulario de bonos
            var frmBono = new frmBono();
            frmBono.ShowDialog();
        }

        private void BtnManual_Click(object sender, EventArgs e)
        {
            try
            {
                // Ocultar el panel de operaciones
                panelOperaciones.Visible = false;
                btnOperaciones.Text = "▶ Operaciones";
                
                // Ruta del manual HTML
                string manualPath = Path.Combine(Application.StartupPath, "Manual_Usuario.html");
                
                if (!File.Exists(manualPath))
                {
                    MessageBox.Show("No se encontró el archivo del manual de usuario.\n\n" +
                        "El manual se abrirá en el navegador para que pueda imprimirlo como PDF.", 
                        "Manual no encontrado", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Abrir el manual en el navegador predeterminado
                Process.Start(new ProcessStartInfo(manualPath) { UseShellExecute = true });
                
                MessageBox.Show("El manual se ha abierto en el navegador.\n\n" +
                    "Para descargar como PDF:\n" +
                    "1. Presione Ctrl+P en el navegador\n" +
                    "2. Seleccione 'Guardar como PDF'\n" +
                    "3. Elija la ubicación y guarde", 
                    "Manual abierto", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al abrir el manual: {ex.Message}", 
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ========================================
        // FUNCIÓN SECRETA PARA LIMPIAR BASE DE DATOS
        // ========================================
        
        private void LimpiarBaseDatosSecreta()
        {
            try
            {
                // Confirmar la acción con mensaje más técnico
                var result = MessageBox.Show(
                    "🔧 FUNCIÓN DE DESARROLLO\n\n" +
                    "Esta función eliminará TODOS los datos de la base de datos:\n" +
                    "• Afiliados\n" +
                    "• Recetarios\n" +
                    "• Bonos\n" +
                    "• Familiares\n\n" +
                    "Se mantendrá la estructura de las tablas.\n" +
                    "Esta acción es IRREVERSIBLE.\n\n" +
                    "¿Desea continuar con la limpieza?",
                    "Función de Desarrollo - Limpiar Base de Datos",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning);

                if (result == DialogResult.Yes)
                {
                    // Ejecutar limpieza
                    string mensaje = dbManager.LimpiarBaseDatos();
                    
                    MessageBox.Show(
                        $"✅ LIMPIEZA COMPLETADA\n\n" +
                        $"{mensaje}\n\n" +
                        "La base de datos está lista para crear el instalador.",
                        "Limpieza de Base de Datos",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                    
                    // Recargar la lista de afiliados
                    CargarAfiliados();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"❌ ERROR EN LA LIMPIEZA\n\n" +
                    $"Detalles: {ex.Message}",
                    "Error de Limpieza",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }
        private void btnImprimir_Click(object sender, EventArgs e)
        {
            if (afiliadoSeleccionadoId == null) return;
            var afiliado = dbManager.ObtenerTodosLosAfiliados().Find(a => a.Id == afiliadoSeleccionadoId.Value);
            if (afiliado == null) return;

            // Control de recetarios mensuales
            int maxRecetarios = afiliado.TieneGrupoFamiliar ? 4 : 2;
            int recetariosEsteMes = dbManager.ContarRecetariosMensuales(afiliado.Id, DateTime.Now);
            
            if (recetariosEsteMes >= maxRecetarios)
            {
                DateTime? proxima = dbManager.FechaProximaHabilitacion(afiliado.Id);
                string fechaProx = proxima.HasValue ? proxima.Value.ToString("dd/MM/yyyy") : "-";
                MessageBox.Show($"Ya imprimió las recetas correspondientes al período. Podrá imprimir nuevamente a partir del: {fechaProx}", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            // Calcular cuántos recetarios imprimir
            int recetariosAImprimir = maxRecetarios - recetariosEsteMes;
            
            // Solo permitir imprimir si hay recetas disponibles
            if (recetariosAImprimir <= 0)
            {
                DateTime? proxima = dbManager.FechaProximaHabilitacion(afiliado.Id);
                string fechaProx = proxima.HasValue ? proxima.Value.ToString("dd/MM/yyyy") : "-";
                MessageBox.Show($"Ya imprimió las recetas correspondientes al período. Podrá imprimir nuevamente a partir del: {fechaProx}", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            
            // Siempre imprimir 2 recetas por impresión (1 hoja)
            // Para grupos familiares, quedan 2 pendientes para otra impresión
            recetariosAImprimir = 2; // Siempre imprimir 2 recetas por impresión

            string mensajeConfirmacion = $"¿Desea imprimir {recetariosAImprimir} recetas del afiliado {afiliado.ApellidoNombre}?";
            
            if (afiliado.TieneGrupoFamiliar && recetariosEsteMes == 0)
            {
                mensajeConfirmacion += "\n\nNota: Al ser grupo familiar, quedará 1 impresión adicional disponible (2 recetas más).";
            }
            
            var confirm = MessageBox.Show(mensajeConfirmacion, "Confirmar impresión", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (confirm == DialogResult.Yes)
            {
                try
                {
                    // Generar todos los recetarios necesarios
                    var recetariosGenerados = new List<Models.Recetario>();
                    
                    // Obtener 2 números consecutivos para la impresión
                    var numeros = dbManager.ObtenerDosNumerosConsecutivos();
                    List<int> numerosTalonario = new List<int> { numeros.primero, numeros.segundo };
                    
                    for (int i = 0; i < recetariosAImprimir; i++)
                    {
                        var recetario = new Models.Recetario
                        {
                            NumeroTalonario = numerosTalonario[i],
                            IdAfiliado = afiliado.Id,
                            FechaEmision = DateTime.Now,
                            FechaVencimiento = DateTime.Now.AddMonths(1)
                        };
                        
                        dbManager.InsertarRecetario(recetario);
                        recetariosGenerados.Add(recetario);
                    }

                    // Generar HTML con todas las recetas
                    var recetarioManager = new RecetarioManager();
                    recetarioManager.GenerarHTMLConRecetas(recetariosGenerados, afiliado);
                    
                    MessageBox.Show($"Se generaron {recetariosAImprimir} recetarios correctamente.", "Impresión exitosa", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    
                    // Recargar la lista de afiliados para mostrar la información actualizada
                    CargarAfiliados();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error al generar los recetarios: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}
