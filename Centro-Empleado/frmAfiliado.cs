using System;
using System.Windows.Forms;
using System.Collections.Generic;
using Centro_Empleado.Data;
using Centro_Empleado.Models;

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
                DateTime? proxima = null;
                if (ultima.HasValue)
                {
                    proxima = ultima.Value.AddMonths(1);
                }
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
                dgvAfiliados.Columns["ProximaHabilitacion"].HeaderText = "Próxima impresión";
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
                MessageBox.Show($"Ya imprimió las recetas correspondientes al mes. Podrá imprimir nuevamente a partir del: {fechaProx}", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var confirm = MessageBox.Show($"¿Desea imprimir las recetas del afiliado {afiliado.ApellidoNombre}?", "Confirmar impresión", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (confirm == DialogResult.Yes)
            {
                // Lógica de impresión: generar los recetarios y registrar en la base
                int aImprimir = maxRecetarios - recetariosEsteMes;
                if (aImprimir > 2) aImprimir = 2; // Solo se imprimen 2 por vez
                var numeros = dbManager.ObtenerDosNumerosConsecutivos();
                var recetariosGenerados = new List<Models.Recetario>();
                for (int i = 0; i < aImprimir; i++)
                {
                    int nro = (i == 0) ? numeros.primero : numeros.segundo;
                    var recetario = new Models.Recetario
                    {
                        NumeroTalonario = nro,
                        IdAfiliado = afiliado.Id,
                        FechaEmision = DateTime.Now,
                        FechaVencimiento = DateTime.Now.AddMonths(1)
                    };
                    dbManager.InsertarRecetario(recetario);
                    recetariosGenerados.Add(recetario);
                }
                MessageBox.Show($"Recetarios generados correctamente. Se imprimieron {aImprimir} recetarios.", "Impresión", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // Generar HTML personalizado y abrirlo para imprimir
                try
                {
                    string templatePath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "recetaFinal.html");
                    if (!System.IO.File.Exists(templatePath))
                    {
                        MessageBox.Show("No se encontró la plantilla HTML de receta.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    foreach (var recetario in recetariosGenerados)
                    {
                        string html = System.IO.File.ReadAllText(templatePath);
                        // Reemplazo seguro de todos los placeholders
                        html = html.Replace("{{APELLIDO_NOMBRE}}", System.Net.WebUtility.HtmlEncode(afiliado.ApellidoNombre ?? ""));
                        html = html.Replace("{{DNI}}", System.Net.WebUtility.HtmlEncode(afiliado.DNI ?? ""));
                        html = html.Replace("{{EMPRESA}}", System.Net.WebUtility.HtmlEncode(afiliado.Empresa ?? ""));
                        html = html.Replace("{{FECHA_EMISION}}", recetario.FechaEmision.ToString("dd/MM/yyyy"));
                        html = html.Replace("{{FECHA_VENCIMIENTO}}", recetario.FechaVencimiento.ToString("dd/MM/yyyy"));
                        html = html.Replace("{{NUMERO_TALONARIO}}", recetario.NumeroTalonario.ToString());
                        string tempFile = System.IO.Path.Combine(System.IO.Path.GetTempPath(), $"receta_{afiliado.Id}_{recetario.NumeroTalonario}_{DateTime.Now.Ticks}.html");
                        System.IO.File.WriteAllText(tempFile, html);
                        System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo(tempFile) { UseShellExecute = true });
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error al generar o abrir el archivo HTML: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}
