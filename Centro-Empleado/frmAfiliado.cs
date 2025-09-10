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
            try
            {
                InitializeComponent();
                dbManager = new DatabaseManager();
                InicializarEventos();
                CargarAfiliados();
                btnEditar.Enabled = false;
                btnEliminar.Enabled = false;
                btnImprimir.Enabled = false;
                
                // El respaldo se hace solo manualmente para evitar interrupciones
            }
            catch (Exception ex)
            {
                MessageBox.Show(string.Format("Error al inicializar el formulario:\n\n{0}\n\nDetalles:\n{1}", ex.Message, ex.ToString()), 
                    "Error de Inicialización", MessageBoxButtons.OK, MessageBoxIcon.Error);
                throw; // Re-lanzar para que Program.cs lo capture
            }
        }

        private void InicializarEventos()
        {
            btnGuardar.Click += btnGuardar_Click;
            btnEditar.Click += btnEditar_Click;
            btnEliminar.Click += btnEliminar_Click;
            btnImprimir.Click += btnImprimir_Click;
            dgvAfiliados.CellDoubleClick += dgvAfiliados_CellDoubleClick;
            btnRecetaExtraordinaria.Click += btnRecetaExtraordinaria_Click;
            btnHistorialExtraordinarias.Click += btnHistorialExtraordinarias_Click;
            btnCambiarContrasena.Click += btnCambiarContrasena_Click;
            btnRespaldo.Click += btnRespaldo_Click;
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
                    (a.DNI != null && a.DNI.Contains(filtro)) ||
                    (a.NumeroAfiliado != null && a.NumeroAfiliado.Contains(filtro)));

            // Crear lista anónima con fechas de último recetario y próxima habilitación
            var lista = new List<dynamic>();
            foreach (var a in afiliados)
            {
                var recetarios = dbManager.ObtenerRecetariosPorAfiliado(a.Id);
                DateTime? ultima = recetarios.Count > 0 ? (DateTime?)recetarios[0].FechaEmision : null;
                DateTime? proxima = dbManager.FechaProximaHabilitacion(a.Id);
                
                // Verificar si tiene recetas extraordinarias este mes
                bool tieneExtraordinaria = dbManager.YaImprimioRecetaExtraordinariaEsteMes(a.Id);
                
                lista.Add(new
                {
                    a.Id,
                    a.ApellidoNombre,
                    a.DNI,
                    a.NumeroAfiliado,
                    a.Empresa,
                    TieneFamiliar = a.TieneGrupoFamiliar ? "Sí" : "",
                    FechaUltimaImpresion = ultima.HasValue ? ultima.Value.ToString("dd/MM/yyyy") : "-",
                    ProximaHabilitacion = proxima.HasValue ? proxima.Value.ToString("dd/MM/yyyy") : "-",
                    RecetaExtraordinaria = tieneExtraordinaria ? "Sí" : ""
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
            if (dgvAfiliados.Columns.Contains("RecetaExtraordinaria"))
            {
                dgvAfiliados.Columns["RecetaExtraordinaria"].HeaderText = "Rec. Extraordinaria";
            }
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            if (!ValidarDatos()) return;
            var afiliado = new Afiliado
            {
                ApellidoNombre = txtApellidoNombre.Text.Trim(),
                DNI = txtDNI.Text.Trim(),
                NumeroAfiliado = txtNumeroAfiliado.Text.Trim(),
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
                    MessageBox.Show(string.Format("Error al guardar afiliado: {0}", ex.Message), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                NumeroAfiliado = txtNumeroAfiliado.Text.Trim(),
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
                MessageBox.Show(string.Format("Error al actualizar afiliado: {0}", ex.Message), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
            string mensaje = "¿Está seguro que desea eliminar al afiliado:\n\n";
            mensaje += string.Format("Nombre: {0}\n", afiliado.ApellidoNombre);
            mensaje += string.Format("DNI: {0}\n", afiliado.DNI);
            mensaje += string.Format("Empresa: {0}\n", afiliado.Empresa);
            mensaje += string.Format("Tipo: {0}\n\n", (afiliado.TieneGrupoFamiliar ? "Grupo Familiar" : "Individual"));
            
            if (tieneRecetarios)
            {
                mensaje += string.Format("⚠️ ADVERTENCIA: Este afiliado tiene {0} recetario(s) impreso(s).\n", recetarios.Count);
                mensaje += "Al eliminarlo, se eliminarán TODOS los recetarios asociados.\n\n";
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
                             mensajeExito += string.Format(" junto con {0} recetario(s)", recetarios.Count);
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
                     string innerExceptionMessage = ex.InnerException != null ? ex.InnerException.Message : "Sin detalles adicionales";
                     MessageBox.Show(string.Format("Error al eliminar afiliado: {0}\n\nDetalles técnicos: {1}", ex.Message, innerExceptionMessage), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                    var numeroAfiliadoProp = tipo.GetProperty("NumeroAfiliado");
                    var empresaProp = tipo.GetProperty("Empresa");
                    var tieneFamiliarProp = tipo.GetProperty("TieneFamiliar");
                    if (idProp != null && nombreProp != null && dniProp != null && numeroAfiliadoProp != null && empresaProp != null && tieneFamiliarProp != null)
                    {
                        afiliadoSeleccionadoId = (int)idProp.GetValue(row);
                        var nombreValue = nombreProp.GetValue(row);
                        txtApellidoNombre.Text = nombreValue != null ? nombreValue.ToString() : "";
                        
                        var dniValue = dniProp.GetValue(row);
                        txtDNI.Text = dniValue != null ? dniValue.ToString() : "";
                        
                        var numeroAfiliadoValue = numeroAfiliadoProp.GetValue(row);
                        txtNumeroAfiliado.Text = numeroAfiliadoValue != null ? numeroAfiliadoValue.ToString() : "";
                        
                        var empresaValue = empresaProp.GetValue(row);
                        txtEmpresa.Text = empresaValue != null ? empresaValue.ToString() : "";
                        
                        // Si el valor es "Sí" entonces tiene grupo familiar
                        var familiarValue = tieneFamiliarProp.GetValue(row);
                        chkGrupoFamiliar.Checked = (familiarValue != null ? familiarValue.ToString() : "") == "Sí";
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
            if (string.IsNullOrWhiteSpace(txtNumeroAfiliado.Text))
            {
                MessageBox.Show("El número de afiliado es obligatorio", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtNumeroAfiliado.Focus();
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
            txtNumeroAfiliado.Clear();
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
                MessageBox.Show(string.Format("Error al abrir el manual: {0}", ex.Message), 
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
                        "✅ LIMPIEZA COMPLETADA\n\n" +
                        string.Format("{0}\n\n", mensaje) +
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
                    "❌ ERROR EN LA LIMPIEZA\n\n" +
                    string.Format("Detalles: {0}", ex.Message),
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

            // Obtener recetarios disponibles con el nuevo sistema (máximo 3 por mes)
            int recetariosDisponibles = dbManager.ObtenerRecetariosDisponibles(afiliado.Id);
            
            if (recetariosDisponibles <= 0)
            {
                DateTime? proxima = dbManager.FechaProximaHabilitacionRecetario(afiliado.Id);
                string fechaProx = proxima.HasValue ? proxima.Value.ToString("dd/MM/yyyy") : "-";
                
                // Verificar si ya imprimió receta extraordinaria este mes
                bool yaImprimioExtraordinaria = dbManager.YaImprimioRecetaExtraordinariaEsteMes(afiliado.Id);
                
                if (yaImprimioExtraordinaria)
                {
                    // Obtener historial de recetas extraordinarias
                    var historialExtraordinarias = dbManager.ObtenerHistorialRecetasExtraordinarias(afiliado.Id);
                    var ultimaExtraordinaria = historialExtraordinarias.FirstOrDefault();
                    
                    string mensaje = "No hay recetarios disponibles este mes.\n";
                    mensaje += string.Format("El próximo se habilitará el: {0}\n\n", fechaProx);
                    mensaje += "Ya imprimió una receta extraordinaria este mes.\n";
                    if (ultimaExtraordinaria != null)
                    {
                        mensaje += string.Format("Última receta extraordinaria: {0:dd/MM/yyyy} - Motivo: {1}\n\n", 
                            ultimaExtraordinaria.FechaImpresion, ultimaExtraordinaria.Motivo);
                    }
                    mensaje += "¿Desea ver el historial completo de recetas extraordinarias?";
                    
                    var resultado = MessageBox.Show(mensaje, "Recetarios Agotados", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                    if (resultado == DialogResult.Yes)
                    {
                        using (var frmHistorial = new frmHistorialExtraordinarias(afiliado.Id, afiliado.ApellidoNombre))
                        {
                            frmHistorial.ShowDialog();
                        }
                    }
                }
                else
                {
                    // Preguntar si desea solicitar receta extraordinaria
                    var resultado = MessageBox.Show(
                        string.Format("No hay recetarios disponibles este mes. El próximo se habilitará el: {0}\n\n¿Desea solicitar una receta extraordinaria?", fechaProx), 
                        "Recetarios Agotados", 
                        MessageBoxButtons.YesNo, 
                        MessageBoxIcon.Question);
                    
                     if (resultado == DialogResult.Yes)
                     {
                         // Pedir contraseña antes de continuar
                         using (var frmContrasena = new frmIngresarContrasena())
                         {
                             if (frmContrasena.ShowDialog() == DialogResult.OK && frmContrasena.ContrasenaCorrecta)
                             {
                                 // Mostrar formulario de receta extraordinaria
                                 using (var frmExtraordinaria = new frmRecetaExtraordinaria(afiliado.ApellidoNombre))
                                 {
                                     if (frmExtraordinaria.ShowDialog() == DialogResult.OK && frmExtraordinaria.Aprobado)
                                     {
                                         ImprimirRecetaExtraordinaria(afiliado, frmExtraordinaria.Motivo);
                                     }
                                 }
                             }
                         }
                     }
                }
                return;
            }
            
            // Mostrar formulario de selección de cantidad
            using (var frmSeleccion = new frmSeleccionRecetarios(recetariosDisponibles))
            {
                if (frmSeleccion.ShowDialog() == DialogResult.OK)
                {
                    int recetariosAImprimir = frmSeleccion.CantidadSeleccionada;
                    
                try
                {
                    // Generar todos los recetarios necesarios
                    var recetariosGenerados = new List<Models.Recetario>();
                    
                        // Obtener números consecutivos para la impresión
                        var numerosAdicionales = dbManager.ObtenerNumerosAdicionales(recetariosAImprimir);
                    
                    for (int i = 0; i < recetariosAImprimir; i++)
                    {
                        var recetario = new Models.Recetario
                        {
                                NumeroTalonario = numerosAdicionales[i],
                            IdAfiliado = afiliado.Id,
                            FechaEmision = DateTime.Now,
                            FechaVencimiento = DateTime.Now.AddMonths(1)
                        };
                        
                        dbManager.InsertarRecetario(recetario);
                            
                            // Registrar el recetario impreso en el sistema mensual
                            dbManager.RegistrarRecetarioImpreso(afiliado.Id, numerosAdicionales[i]);
                            
                        recetariosGenerados.Add(recetario);
                    }

                    // Generar HTML con todas las recetas
                    var recetarioManager = new RecetarioManager();
                        
                        // Caso especial: si se seleccionaron 3 recetarios, imprimir 2 primero y luego 1
                        if (recetariosAImprimir == 3)
                        {
                            // Primera impresión: 2 recetarios
                            var primerosDos = recetariosGenerados.Take(2).ToList();
                            recetarioManager.GenerarHTMLConRecetas(primerosDos, afiliado, true);
                            
                            // Segunda impresión: 1 recetario
                            var ultimo = recetariosGenerados.Skip(2).Take(1).ToList();
                            recetarioManager.GenerarHTMLConRecetas(ultimo, afiliado, false);
                            
                            MessageBox.Show("Se generaron 3 recetarios correctamente: 2 en la primera hoja y 1 en la segunda.", "Impresión exitosa", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else
                        {
                            // Caso normal: determinar automáticamente el formato según la cantidad
                            bool imprimirDosPorHoja = recetariosAImprimir >= 2;
                            recetarioManager.GenerarHTMLConRecetas(recetariosGenerados, afiliado, imprimirDosPorHoja);
                            MessageBox.Show(string.Format("Se generaron {0} recetarios correctamente.", recetariosAImprimir), "Impresión exitosa", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    
                    // Recargar la lista de afiliados para mostrar la información actualizada
                    CargarAfiliados();
                }
                catch (Exception ex)
                {
                        MessageBox.Show(string.Format("Error al generar los recetarios: {0}", ex.Message), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void ImprimirRecetaExtraordinaria(Models.Afiliado afiliado, string motivo)
        {
            try
            {
                // Obtener un número de recetario
                var numerosAdicionales = dbManager.ObtenerNumerosAdicionales(1);
                int numeroRecetario = numerosAdicionales[0];
                
                // Crear el recetario extraordinario
                var recetario = new Models.Recetario
                {
                    NumeroTalonario = numeroRecetario,
                    IdAfiliado = afiliado.Id,
                    FechaEmision = DateTime.Now,
                    FechaVencimiento = DateTime.Now.AddMonths(1)
                };
                
                // Insertar en la base de datos
                dbManager.InsertarRecetario(recetario);
                
                // Registrar como recetario extraordinario
                dbManager.RegistrarRecetarioExtraordinario(afiliado.Id, numeroRecetario, motivo);
                
                // Generar HTML (solo 1 recetario)
                var recetariosList = new List<Models.Recetario> { recetario };
                var recetarioManager = new RecetarioManager();
                recetarioManager.GenerarHTMLConRecetas(recetariosList, afiliado, false);
                
                MessageBox.Show(string.Format("Se generó la receta extraordinaria correctamente.\nMotivo: {0}", motivo), 
                    "Receta Extraordinaria Aprobada", MessageBoxButtons.OK, MessageBoxIcon.Information);
                
                // Recargar la lista de afiliados
                CargarAfiliados();
            }
            catch (Exception ex)
            {
                MessageBox.Show(string.Format("Error al generar la receta extraordinaria: {0}", ex.Message), 
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void txtDNI_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Solo permitir números y teclas de control (Backspace, Delete, etc.)
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true; // Cancelar el carácter
            }
        }

        private void btnRecetaExtraordinaria_Click(object sender, EventArgs e)
        {
            if (afiliadoSeleccionadoId == null)
            {
                MessageBox.Show("Debe seleccionar un afiliado primero.", "Selección requerida", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var afiliado = dbManager.ObtenerTodosLosAfiliados().Find(a => a.Id == afiliadoSeleccionadoId.Value);
            if (afiliado == null) return;

            // Verificar si ya imprimió receta extraordinaria este mes
            bool yaImprimioExtraordinaria = dbManager.YaImprimioRecetaExtraordinariaEsteMes(afiliado.Id);
            
            if (yaImprimioExtraordinaria)
            {
                MessageBox.Show("Este afiliado ya imprimió una receta extraordinaria este mes.", 
                    "Receta Extraordinaria No Disponible", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            // Pedir contraseña antes de continuar
            using (var frmContrasena = new frmIngresarContrasena())
            {
                if (frmContrasena.ShowDialog() == DialogResult.OK && frmContrasena.ContrasenaCorrecta)
                {
                    // Mostrar formulario de receta extraordinaria
                    using (var frmExtraordinaria = new frmRecetaExtraordinaria(afiliado.ApellidoNombre))
                    {
                        if (frmExtraordinaria.ShowDialog() == DialogResult.OK && frmExtraordinaria.Aprobado)
                        {
                            ImprimirRecetaExtraordinaria(afiliado, frmExtraordinaria.Motivo);
                        }
                    }
                }
            }
        }

        private void btnHistorialExtraordinarias_Click(object sender, EventArgs e)
        {
            if (afiliadoSeleccionadoId == null)
            {
                MessageBox.Show("Debe seleccionar un afiliado primero.", "Selección requerida", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var afiliado = dbManager.ObtenerTodosLosAfiliados().Find(a => a.Id == afiliadoSeleccionadoId.Value);
            if (afiliado == null) return;

            // Mostrar formulario de historial
            using (var frmHistorial = new frmHistorialExtraordinarias(afiliado.Id, afiliado.ApellidoNombre))
            {
                frmHistorial.ShowDialog();
            }
        }

        private void btnCambiarContrasena_Click(object sender, EventArgs e)
        {
            using (var frmCambiar = new frmCambiarContrasena())
            {
                frmCambiar.ShowDialog();
            }
        }

        private void btnRespaldo_Click(object sender, EventArgs e)
        {
            CrearRespaldoManual();
        }

        private void CrearRespaldoManual()
        {
            try
            {
                // Verificar que existe la base de datos
                string sourceFile = Path.Combine(Application.StartupPath, "CentroEmpleado.db");
                if (!File.Exists(sourceFile))
                {
                    MessageBox.Show("No se encontró la base de datos para respaldar.\n\n" +
                        "Asegúrese de que el sistema esté funcionando correctamente.", 
                        "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Crear carpeta de respaldos si no existe
                string backupDir = Path.Combine(Application.StartupPath, "Backups");
                if (!Directory.Exists(backupDir))
                {
                    Directory.CreateDirectory(backupDir);
                }

                // Generar nombre de archivo con timestamp
                string timestamp = DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");
                string backupFile = Path.Combine(backupDir, $"CentroEmpleado_{timestamp}.db");

                // Mostrar mensaje de confirmación
                var result = MessageBox.Show(
                    "¿Desea crear un respaldo de la base de datos?\n\n" +
                    "Esto puede tomar unos segundos dependiendo del tamaño de los datos.\n\n" +
                    "El respaldo se guardará en la carpeta 'Backups'.",
                    "Crear Respaldo",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (result != DialogResult.Yes)
                {
                    return;
                }

                // Crear el respaldo
                File.Copy(sourceFile, backupFile, true);

                // Obtener información del archivo
                FileInfo fileInfo = new FileInfo(backupFile);
                string sizeText = fileInfo.Length > 1024 * 1024 ? 
                    $"{fileInfo.Length / (1024 * 1024):F2} MB" : 
                    $"{fileInfo.Length / 1024:F2} KB";

                // Contar respaldos existentes
                string[] existingBackups = Directory.GetFiles(backupDir, "CentroEmpleado_*.db");
                int totalBackups = existingBackups.Length;

                MessageBox.Show($"✅ Respaldo creado exitosamente:\n\n" +
                    $"📁 Archivo: {Path.GetFileName(backupFile)}\n" +
                    $"📊 Tamaño: {sizeText}\n" +
                    $"📂 Ubicación: {backupDir}\n" +
                    $"🔢 Total de respaldos: {totalBackups}", 
                    "Respaldo Completado", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"❌ Error al crear el respaldo:\n\n{ex.Message}\n\n" +
                    "Verifique que tenga permisos de escritura en la carpeta de la aplicación.", 
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

    }
}
