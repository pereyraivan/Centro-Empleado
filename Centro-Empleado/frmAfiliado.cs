using System;
using System.Windows.Forms;
using System.Collections.Generic;
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
            var result = MessageBox.Show("¿Está seguro que desea eliminar el afiliado seleccionado?", "Confirmar eliminación", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (result == DialogResult.Yes)
            {
                if (dbManager.EliminarAfiliado(afiliadoSeleccionadoId.Value))
                {
                    MessageBox.Show("Afiliado eliminado correctamente", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LimpiarFormulario();
                    CargarAfiliados(txtBuscar.Text);
                    btnEditar.Enabled = false;
                    btnEliminar.Enabled = false;
                    btnImprimir.Enabled = false;
                    afiliadoSeleccionadoId = null;
                }
                else
                {
                    MessageBox.Show("No se pudo eliminar el afiliado. Puede que tenga recetarios emitidos.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
            
            // Para grupos familiares, imprimir 4 recetas (2 hojas) de una vez
            // Para afiliados sin grupo familiar, imprimir 2 recetas (1 hoja) de una vez
            if (afiliado.TieneGrupoFamiliar)
            {
                recetariosAImprimir = 4; // Siempre imprimir 4 recetas para grupos familiares
            }
            else
            {
                recetariosAImprimir = 2; // Siempre imprimir 2 recetas para afiliados individuales
            }

            var confirm = MessageBox.Show($"¿Desea imprimir {recetariosAImprimir} recetas del afiliado {afiliado.ApellidoNombre}?", "Confirmar impresión", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (confirm == DialogResult.Yes)
            {
                try
                {
                    // Generar todos los recetarios necesarios
                    var recetariosGenerados = new List<Models.Recetario>();
                    
                    // Obtener todos los números necesarios de una vez
                    List<int> numerosTalonario;
                    if (recetariosAImprimir <= 2)
                    {
                        var numeros = dbManager.ObtenerDosNumerosConsecutivos();
                        numerosTalonario = new List<int> { numeros.primero, numeros.segundo };
                    }
                    else
                    {
                        // Para 4 recetarios, obtener 4 números consecutivos
                        numerosTalonario = dbManager.ObtenerNumerosAdicionales(4);
                    }
                    
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
