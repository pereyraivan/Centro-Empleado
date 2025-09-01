using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SQLite;
using System.IO;
using Centro_Empleado.Models;

namespace Centro_Empleado.Data
{
    public class DatabaseManager
    {
        private string connectionString;

        public DatabaseManager()
        {
            try
            {
                connectionString = ConfigurationManager.ConnectionStrings["CentroEmpleadoDB"].ConnectionString;
                InitializeDatabase();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al inicializar la base de datos: {ex.Message}");
            }
        }

        // Método para probar la conexión
        public bool ProbarConexion()
        {
            try
            {
                using (var connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();
                    
                    // Verificar si la tabla Afiliado existe
                    string sql = "SELECT name FROM sqlite_master WHERE type='table' AND name='Afiliado';";
                    using (var command = new SQLiteCommand(sql, connection))
                    {
                        var result = command.ExecuteScalar();
                        return result != null;
                    }
                }
            }
            catch
            {
                return false;
            }
        }

        // Forzar recreación de tablas
        public void RecrearTablas()
        {
            try
            {
                using (var connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();
                    
                    // Eliminar tablas si existen
                    ExecuteNonQuery("DROP TABLE IF EXISTS Recetario", connection);
                    ExecuteNonQuery("DROP TABLE IF EXISTS Familiar", connection);
                    ExecuteNonQuery("DROP TABLE IF EXISTS Afiliado", connection);
                    
                    // Recrear tablas
                    string createAfiliadoTable = @"
                        CREATE TABLE Afiliado (
                            Id INTEGER PRIMARY KEY AUTOINCREMENT,
                            ApellidoNombre TEXT NOT NULL,
                            DNI TEXT NOT NULL UNIQUE,
                            Empresa TEXT NOT NULL,
                            TieneGrupoFamiliar INTEGER NOT NULL
                        )";

                    string createFamiliarTable = @"
                        CREATE TABLE Familiar (
                            Id INTEGER PRIMARY KEY AUTOINCREMENT,
                            IdAfiliadoTitular INTEGER NOT NULL,
                            Nombre TEXT NOT NULL,
                            DNI TEXT NOT NULL,
                            FOREIGN KEY (IdAfiliadoTitular) REFERENCES Afiliado(Id)
                        )";

                    string createRecetarioTable = @"
                        CREATE TABLE Recetario (
                            Id INTEGER PRIMARY KEY AUTOINCREMENT,
                            NumeroTalonario INTEGER NOT NULL UNIQUE,
                            IdAfiliado INTEGER NOT NULL,
                            FechaEmision DATE NOT NULL,
                            FechaVencimiento DATE NOT NULL,
                            FOREIGN KEY (IdAfiliado) REFERENCES Afiliado(Id)
                        )";

                    ExecuteNonQuery(createAfiliadoTable, connection);
                    ExecuteNonQuery(createFamiliarTable, connection);
                    ExecuteNonQuery(createRecetarioTable, connection);
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al recrear las tablas: {ex.Message}");
            }
        }

        private void InitializeDatabase()
        {
            // FORZAR USO DE UNA ÚNICA BASE DE DATOS EN LA CARPETA DEL PROYECTO
            string dbPath = Path.Combine(Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location), "CentroEmpleado.db");
            
            // Crear la base de datos si no existe
            bool nuevaBaseDatos = !File.Exists(dbPath);
            
            if (nuevaBaseDatos)
            {
                SQLiteConnection.CreateFile(dbPath);
            }
            
            // Actualizar la cadena de conexión para usar la ruta absoluta
            connectionString = $"Data Source={dbPath};Version=3;";
            
            // Siempre intentar crear las tablas (si ya existen, no hace nada por el CREATE TABLE IF NOT EXISTS)
            CreateTables();
        }

        private void CreateTables()
        {
            try
            {
                using (var connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();

                    string createAfiliadoTable = @"
                        CREATE TABLE IF NOT EXISTS Afiliado (
                            Id INTEGER PRIMARY KEY AUTOINCREMENT,
                            ApellidoNombre TEXT NOT NULL,
                            DNI TEXT NOT NULL UNIQUE,
                            Empresa TEXT NOT NULL,
                            TieneGrupoFamiliar INTEGER NOT NULL
                        )";

                    string createFamiliarTable = @"
                        CREATE TABLE IF NOT EXISTS Familiar (
                            Id INTEGER PRIMARY KEY AUTOINCREMENT,
                            IdAfiliadoTitular INTEGER NOT NULL,
                            Nombre TEXT NOT NULL,
                            DNI TEXT NOT NULL,
                            FOREIGN KEY (IdAfiliadoTitular) REFERENCES Afiliado(Id)
                        )";

                    string createRecetarioTable = @"
                        CREATE TABLE IF NOT EXISTS Recetario (
                            Id INTEGER PRIMARY KEY AUTOINCREMENT,
                            NumeroTalonario INTEGER NOT NULL UNIQUE,
                            IdAfiliado INTEGER NOT NULL,
                            FechaEmision DATE NOT NULL,
                            FechaVencimiento DATE NOT NULL,
                            FOREIGN KEY (IdAfiliado) REFERENCES Afiliado(Id)
                        )";

                    ExecuteNonQuery(createAfiliadoTable, connection);
                    ExecuteNonQuery(createFamiliarTable, connection);
                    ExecuteNonQuery(createRecetarioTable, connection);
                    
                    // Crear tabla de bonos
                    CreateBonoTable();
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al crear las tablas de la base de datos: {ex.Message}");
            }
        }

        private void ExecuteNonQuery(string sql, SQLiteConnection connection)
        {
            try
            {
                using (var command = new SQLiteCommand(sql, connection))
                {
                    command.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al ejecutar SQL: {sql}. Error: {ex.Message}");
            }
        }

        // Métodos para Afiliado
        public void InsertarAfiliado(Afiliado afiliado)
        {
            string sql = @"INSERT INTO Afiliado (ApellidoNombre, DNI, Empresa, TieneGrupoFamiliar) 
                          VALUES (@ApellidoNombre, @DNI, @Empresa, @TieneGrupoFamiliar)";

            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                using (var command = new SQLiteCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@ApellidoNombre", afiliado.ApellidoNombre);
                    command.Parameters.AddWithValue("@DNI", afiliado.DNI);
                    command.Parameters.AddWithValue("@Empresa", afiliado.Empresa);
                    command.Parameters.AddWithValue("@TieneGrupoFamiliar", afiliado.TieneGrupoFamiliar ? 1 : 0);
                    command.ExecuteNonQuery();
                }
            }
        }

        public Afiliado BuscarAfiliadoPorDNI(string dni)
        {
            string sql = "SELECT * FROM Afiliado WHERE DNI = @DNI";

            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                using (var command = new SQLiteCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@DNI", dni);
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new Afiliado
                            {
                                Id = Convert.ToInt32(reader["Id"]),
                                ApellidoNombre = reader["ApellidoNombre"].ToString(),
                                DNI = reader["DNI"].ToString(),
                                Empresa = reader["Empresa"].ToString(),
                                TieneGrupoFamiliar = Convert.ToBoolean(reader["TieneGrupoFamiliar"])
                            };
                        }
                    }
                }
            }
            return null;
        }

        // Métodos para Recetario
        public void InsertarRecetario(Recetario recetario)
        {
            string sql = @"INSERT INTO Recetario (NumeroTalonario, IdAfiliado, FechaEmision, FechaVencimiento) 
                          VALUES (@NumeroTalonario, @IdAfiliado, @FechaEmision, @FechaVencimiento)";

            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                using (var command = new SQLiteCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@NumeroTalonario", recetario.NumeroTalonario);
                    command.Parameters.AddWithValue("@IdAfiliado", recetario.IdAfiliado);
                    command.Parameters.AddWithValue("@FechaEmision", recetario.FechaEmision);
                    command.Parameters.AddWithValue("@FechaVencimiento", recetario.FechaVencimiento);
                    command.ExecuteNonQuery();
                }
            }
        }

        public int ContarRecetariosMensuales(int idAfiliado, DateTime fechaReferencia)
        {
            // Obtener la fecha de la última impresión
            var ultimaImpresion = ObtenerUltimaFechaImpresion(idAfiliado);
            
            // Si no hay impresiones previas, permitir imprimir
            if (!ultimaImpresion.HasValue)
                return 0;
            
            // Verificar si han pasado exactamente 30 días desde la última impresión
            var diasTranscurridos = (fechaReferencia - ultimaImpresion.Value).Days;
            
            // Si han pasado menos de 30 días, contar todos los recetarios del afiliado
            if (diasTranscurridos < 30)
            {
                string sql = @"SELECT COUNT(*) FROM Recetario 
                              WHERE IdAfiliado = @IdAfiliado";

                using (var connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();
                    using (var command = new SQLiteCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@IdAfiliado", idAfiliado);
                        return Convert.ToInt32(command.ExecuteScalar());
                    }
                }
            }
            
            // Si han pasado 30 días o más, permitir nueva impresión
            return 0;
        }

        // Método auxiliar para obtener la fecha de la última impresión
        public DateTime? ObtenerUltimaFechaImpresion(int idAfiliado)
        {
            string sql = @"SELECT MAX(FechaEmision) FROM Recetario WHERE IdAfiliado = @IdAfiliado";

            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                using (var command = new SQLiteCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@IdAfiliado", idAfiliado);
                    var result = command.ExecuteScalar();
                    if (result != DBNull.Value && result != null)
                    {
                        return Convert.ToDateTime(result);
                    }
                }
            }
            return null;
        }

        public DateTime? FechaProximaHabilitacion(int idAfiliado)
        {
            // Obtener el afiliado para verificar si tiene grupo familiar
            var afiliado = ObtenerTodosLosAfiliados().Find(a => a.Id == idAfiliado);
            if (afiliado == null) return null;
            
            // Obtener la cantidad de recetarios ya impresos
            int recetariosImpresos = ContarRecetariosMensuales(idAfiliado, DateTime.Now);
            int maxRecetarios = afiliado.TieneGrupoFamiliar ? 4 : 2;
            
            // Si aún quedan recetarios disponibles, no mostrar fecha de próxima habilitación
            if (recetariosImpresos < maxRecetarios)
            {
                return null; // Aún puede imprimir más recetarios
            }
            
            // Solo calcular próxima habilitación si ya completó todos sus recetarios
            var ultimaImpresion = ObtenerUltimaFechaImpresion(idAfiliado);
            
            if (ultimaImpresion.HasValue)
            {
                // Calcular la próxima habilitación: 30 días desde la última impresión
                return ultimaImpresion.Value.AddDays(30);
            }
            
            return null;
        }

        public int ObtenerProximoNumeroTalonario()
        {
            string sql = "SELECT IFNULL(MAX(NumeroTalonario), 0) + 1 FROM Recetario";

            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                using (var command = new SQLiteCommand(sql, connection))
                {
                    return Convert.ToInt32(command.ExecuteScalar());
                }
            }
        }

        // Método para obtener 2 números consecutivos de una vez de manera segura
        public (int primero, int segundo) ObtenerDosNumerosConsecutivos()
        {
            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        // Obtener el máximo número actual
                        string sqlMax = "SELECT IFNULL(MAX(NumeroTalonario), 0) FROM Recetario";
                        int maxActual;
                        
                        using (var command = new SQLiteCommand(sqlMax, connection, transaction))
                        {
                            maxActual = Convert.ToInt32(command.ExecuteScalar());
                        }
                        
                        // Calcular los dos números consecutivos
                        int primero = maxActual + 1;
                        int segundo = maxActual + 2;
                        
                        transaction.Commit();
                        return (primero, segundo);
                    }
                    catch
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }

        // Método para obtener números adicionales de talonario de manera segura
        public List<int> ObtenerNumerosAdicionales(int cantidad)
        {
            var numeros = new List<int>();
            
            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        // Obtener el máximo número actual
                        string sqlMax = "SELECT IFNULL(MAX(NumeroTalonario), 0) FROM Recetario";
                        int maxActual;
                        
                        using (var command = new SQLiteCommand(sqlMax, connection, transaction))
                        {
                            maxActual = Convert.ToInt32(command.ExecuteScalar());
                        }
                        
                        // Generar números consecutivos empezando desde el siguiente al máximo
                        for (int i = 1; i <= cantidad; i++)
                        {
                            numeros.Add(maxActual + i);
                        }
                        
                        transaction.Commit();
                    }
                    catch
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
            
            return numeros;
        }

        // MÉTODOS SELECT (CONSULTAS)
        
        // Obtener todos los afiliados
        public List<Afiliado> ObtenerTodosLosAfiliados()
        {
            List<Afiliado> afiliados = new List<Afiliado>();
            string sql = "SELECT * FROM Afiliado ORDER BY ApellidoNombre";

            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                using (var command = new SQLiteCommand(sql, connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            afiliados.Add(new Afiliado
                            {
                                Id = Convert.ToInt32(reader["Id"]),
                                ApellidoNombre = reader["ApellidoNombre"].ToString(),
                                DNI = reader["DNI"].ToString(),
                                Empresa = reader["Empresa"].ToString(),
                                TieneGrupoFamiliar = Convert.ToBoolean(reader["TieneGrupoFamiliar"])
                            });
                        }
                    }
                }
            }
            return afiliados;
        }

        // Obtener afiliados por empresa
        public List<Afiliado> ObtenerAfiliadosPorEmpresa(string empresa)
        {
            List<Afiliado> afiliados = new List<Afiliado>();
            string sql = "SELECT * FROM Afiliado WHERE Empresa LIKE @Empresa ORDER BY ApellidoNombre";

            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                using (var command = new SQLiteCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@Empresa", "%" + empresa + "%");
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            afiliados.Add(new Afiliado
                            {
                                Id = Convert.ToInt32(reader["Id"]),
                                ApellidoNombre = reader["ApellidoNombre"].ToString(),
                                DNI = reader["DNI"].ToString(),
                                Empresa = reader["Empresa"].ToString(),
                                TieneGrupoFamiliar = Convert.ToBoolean(reader["TieneGrupoFamiliar"])
                            });
                        }
                    }
                }
            }
            return afiliados;
        }

        // Obtener todos los recetarios de un afiliado
        public List<Recetario> ObtenerRecetariosPorAfiliado(int idAfiliado)
        {
            List<Recetario> recetarios = new List<Recetario>();
            string sql = "SELECT * FROM Recetario WHERE IdAfiliado = @IdAfiliado ORDER BY FechaEmision DESC";

            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                using (var command = new SQLiteCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@IdAfiliado", idAfiliado);
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            recetarios.Add(new Recetario
                            {
                                Id = Convert.ToInt32(reader["Id"]),
                                NumeroTalonario = Convert.ToInt32(reader["NumeroTalonario"]),
                                IdAfiliado = Convert.ToInt32(reader["IdAfiliado"]),
                                FechaEmision = Convert.ToDateTime(reader["FechaEmision"]),
                                FechaVencimiento = Convert.ToDateTime(reader["FechaVencimiento"])
                            });
                        }
                    }
                }
            }
            return recetarios;
        }

        // Obtener todos los recetarios emitidos en un período
        public List<Recetario> ObtenerRecetariosPorFecha(DateTime fechaDesde, DateTime fechaHasta)
        {
            List<Recetario> recetarios = new List<Recetario>();
            string sql = @"SELECT r.*, a.ApellidoNombre, a.DNI, a.Empresa 
                          FROM Recetario r 
                          INNER JOIN Afiliado a ON r.IdAfiliado = a.Id 
                          WHERE r.FechaEmision BETWEEN @FechaDesde AND @FechaHasta 
                          ORDER BY r.FechaEmision DESC";

            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                using (var command = new SQLiteCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@FechaDesde", fechaDesde.Date);
                    command.Parameters.AddWithValue("@FechaHasta", fechaHasta.Date.AddHours(23).AddMinutes(59));
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            recetarios.Add(new Recetario
                            {
                                Id = Convert.ToInt32(reader["Id"]),
                                NumeroTalonario = Convert.ToInt32(reader["NumeroTalonario"]),
                                IdAfiliado = Convert.ToInt32(reader["IdAfiliado"]),
                                FechaEmision = Convert.ToDateTime(reader["FechaEmision"]),
                                FechaVencimiento = Convert.ToDateTime(reader["FechaVencimiento"])
                            });
                        }
                    }
                }
            }
            return recetarios;
        }

        // MÉTODOS DELETE (ELIMINAR)

        // Eliminar afiliado por ID (incluyendo todos sus recetarios)
        public bool EliminarAfiliado(int idAfiliado)
        {
            try
            {
                using (var connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();
                    using (var transaction = connection.BeginTransaction())
                    {
                        try
                        {
                            // Primero eliminar todos los recetarios del afiliado
                            string sqlEliminarRecetarios = "DELETE FROM Recetario WHERE IdAfiliado = @IdAfiliado";
                            using (var command = new SQLiteCommand(sqlEliminarRecetarios, connection, transaction))
                            {
                                command.Parameters.AddWithValue("@IdAfiliado", idAfiliado);
                                int recetariosEliminados = command.ExecuteNonQuery();
                            }

                            // Luego eliminar el afiliado
                            string sqlEliminarAfiliado = "DELETE FROM Afiliado WHERE Id = @Id";
                            using (var command = new SQLiteCommand(sqlEliminarAfiliado, connection, transaction))
                            {
                                command.Parameters.AddWithValue("@Id", idAfiliado);
                                int filasAfectadas = command.ExecuteNonQuery();
                                
                                if (filasAfectadas > 0)
                                {
                                    transaction.Commit();
                                    return true;
                                }
                                else
                                {
                                    transaction.Rollback();
                                    return false;
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            transaction.Rollback();
                            throw new Exception($"Error durante la transacción: {ex.Message}", ex);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al eliminar afiliado ID {idAfiliado}: {ex.Message}", ex);
            }
        }

        // Eliminar recetario por ID
        public bool EliminarRecetario(int idRecetario)
        {
            try
            {
                string sql = "DELETE FROM Recetario WHERE Id = @Id";

                using (var connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();
                    using (var command = new SQLiteCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@Id", idRecetario);
                        int filasAfectadas = command.ExecuteNonQuery();
                        return filasAfectadas > 0;
                    }
                }
            }
            catch
            {
                return false;
            }
        }

        // Eliminar todos los recetarios de un afiliado
        public bool EliminarRecetariosDeAfiliado(int idAfiliado)
        {
            try
            {
                string sql = "DELETE FROM Recetario WHERE IdAfiliado = @IdAfiliado";

                using (var connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();
                    using (var command = new SQLiteCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@IdAfiliado", idAfiliado);
                        int filasAfectadas = command.ExecuteNonQuery();
                        return filasAfectadas >= 0; // Puede ser 0 si no tenía recetarios
                    }
                }
            }
            catch
            {
                return false;
            }
        }

        // MÉTODOS AUXILIARES

        // Verificar si un afiliado tiene recetarios emitidos
        private bool TieneRecetariosEmitidos(int idAfiliado)
        {
            string sql = "SELECT COUNT(*) FROM Recetario WHERE IdAfiliado = @IdAfiliado";

            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                using (var command = new SQLiteCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@IdAfiliado", idAfiliado);
                    int cantidad = Convert.ToInt32(command.ExecuteScalar());
                    return cantidad > 0;
                }
            }
        }

        // MÉTODO PARA PRUEBAS - Modificar fecha de emisión de recetarios
        public void ModificarFechaEmisionRecetario(int idRecetario, DateTime nuevaFecha)
        {
            string sql = @"UPDATE Recetario 
                          SET FechaEmision = @NuevaFecha 
                          WHERE Id = @IdRecetario";

            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                using (var command = new SQLiteCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@IdRecetario", idRecetario);
                    command.Parameters.AddWithValue("@NuevaFecha", nuevaFecha);
                    command.ExecuteNonQuery();
                }
            }
        }

        // MÉTODO PARA OBTENER INFORMACIÓN DE LA BASE DE DATOS
        public string ObtenerInfoBaseDatos()
        {
            try
            {
                // Extraer la ruta de la base de datos de la cadena de conexión
                string dbPath = connectionString.Replace("Data Source=", "").Replace(";Version=3;", "");
                
                // Verificar si existe la base de datos
                bool existeDB = File.Exists(dbPath);
                
                // Obtener información de la conexión
                string info = $"INFORMACIÓN DE BASE DE DATOS:\n\n";
                info += $"Ruta de la base de datos: {dbPath}\n";
                info += $"¿Existe el archivo?: {existeDB}\n";
                info += $"Connection String: {connectionString}\n\n";
                
                // Si existe la base de datos, obtener información adicional
                if (existeDB)
                {
                    var fileInfo = new FileInfo(dbPath);
                    info += $"Tamaño del archivo: {fileInfo.Length} bytes\n";
                    info += $"Última modificación: {fileInfo.LastWriteTime}\n";
                    
                    // Probar conexión y contar registros
                    using (var connection = new SQLiteConnection(connectionString))
                    {
                        connection.Open();
                        using (var command = new SQLiteCommand("SELECT COUNT(*) FROM Afiliado", connection))
                        {
                            int countAfiliados = Convert.ToInt32(command.ExecuteScalar());
                            info += $"Total de afiliados en DB: {countAfiliados}\n";
                        }
                        using (var command = new SQLiteCommand("SELECT COUNT(*) FROM Recetario", connection))
                        {
                            int countRecetarios = Convert.ToInt32(command.ExecuteScalar());
                            info += $"Total de recetarios en DB: {countRecetarios}\n";
                        }
                    }
                }
                else
                {
                    info += "⚠️ ADVERTENCIA: La base de datos no existe en la ruta especificada.\n";
                    info += "Esto puede causar problemas de sincronización.\n";
                }
                
                return info;
            }
            catch (Exception ex)
            {
                return $"Error al obtener información de la base de datos: {ex.Message}";
            }
        }

        // MÉTODO PARA LIMPIAR CACHE Y FORZAR RECARGA
        public void LimpiarCache()
        {
            // Forzar recarga de conexión
            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                // Ejecutar una consulta simple para verificar conexión
                using (var command = new SQLiteCommand("SELECT COUNT(*) FROM Afiliado", connection))
                {
                    command.ExecuteScalar();
                }
            }
        }



        // MÉTODO PARA PRUEBAS - Obtener información de recetarios para debugging
        public List<dynamic> ObtenerInfoRecetariosParaPruebas(int idAfiliado)
        {
            var info = new List<dynamic>();
            string sql = @"SELECT Id, NumeroTalonario, FechaEmision, FechaVencimiento 
                          FROM Recetario 
                          WHERE IdAfiliado = @IdAfiliado 
                          ORDER BY FechaEmision DESC";

            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                using (var command = new SQLiteCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@IdAfiliado", idAfiliado);
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            info.Add(new
                            {
                                Id = Convert.ToInt32(reader["Id"]),
                                NumeroTalonario = Convert.ToInt32(reader["NumeroTalonario"]),
                                FechaEmision = Convert.ToDateTime(reader["FechaEmision"]),
                                FechaVencimiento = Convert.ToDateTime(reader["FechaVencimiento"])
                            });
                        }
                    }
                }
            }
            return info;
        }

        // ========================================
        // MÉTODOS PARA BONOS DE COBRO MÉDICO
        // ========================================
        
        // Crear tabla de bonos si no existe
        private void CreateBonoTable()
        {
            try
            {
                using (var connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();
                    
                    string createBonoTable = @"
                        CREATE TABLE IF NOT EXISTS Bono (
                            Id INTEGER PRIMARY KEY AUTOINCREMENT,
                            IdAfiliado INTEGER NOT NULL,
                            NumeroBono TEXT NOT NULL UNIQUE,
                            FechaEmision DATE NOT NULL,
                            Monto DECIMAL(10,2) NOT NULL,
                            Concepto TEXT,
                            Observaciones TEXT,
                            FechaCreacion DATETIME DEFAULT CURRENT_TIMESTAMP,
                            FOREIGN KEY (IdAfiliado) REFERENCES Afiliado(Id)
                        )";
                    
                    ExecuteNonQuery(createBonoTable, connection);
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al crear tabla Bono: {ex.Message}");
            }
        }
        
        // Insertar nuevo bono
        public void InsertarBono(Bono bono)
        {
            // Verificar que el número de bono no exista ya
            if (NumeroBonoExiste(bono.NumeroBono))
            {
                throw new Exception($"El número de bono {bono.NumeroBono} ya existe en la base de datos.");
            }
            
            string sql = @"INSERT INTO Bono (IdAfiliado, NumeroBono, FechaEmision, Monto, Concepto, Observaciones, FechaCreacion) 
                          VALUES (@IdAfiliado, @NumeroBono, @FechaEmision, @Monto, @Concepto, @Observaciones, @FechaCreacion)";
            
            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                using (var command = new SQLiteCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@IdAfiliado", bono.IdAfiliado);
                    command.Parameters.AddWithValue("@NumeroBono", bono.NumeroBono);
                    command.Parameters.AddWithValue("@FechaEmision", bono.FechaEmision);
                    command.Parameters.AddWithValue("@Monto", bono.Monto);
                    command.Parameters.AddWithValue("@Concepto", bono.Concepto ?? "");
                    command.Parameters.AddWithValue("@Observaciones", bono.Observaciones ?? "");
                    command.Parameters.AddWithValue("@FechaCreacion", DateTime.Now);
                    command.ExecuteNonQuery();
                }
            }
        }
        
        // Verificar si un número de bono ya existe
        private bool NumeroBonoExiste(string numeroBono)
        {
            string sql = "SELECT COUNT(*) FROM Bono WHERE NumeroBono = @NumeroBono";
            
            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                using (var command = new SQLiteCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@NumeroBono", numeroBono);
                    int count = Convert.ToInt32(command.ExecuteScalar());
                    return count > 0;
                }
            }
        }
        
        // Obtener próximo número de bono
        public string ObtenerProximoNumeroBono()
        {
            string sql = "SELECT MAX(CAST(SUBSTR(NumeroBono, 5) AS INTEGER)) FROM Bono WHERE NumeroBono LIKE 'BON-%'";
            
            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                using (var command = new SQLiteCommand(sql, connection))
                {
                    try
                    {
                        var result = command.ExecuteScalar();
                        if (result == null || result == DBNull.Value)
                        {
                            return "BON-000001";
                        }
                        
                        int ultimoNumero = Convert.ToInt32(result);
                        int siguienteNumero = ultimoNumero + 1;
                        return $"BON-{siguienteNumero:D6}";
                    }
                    catch
                    {
                        // Si hay algún error, usar timestamp para garantizar unicidad
                        long timestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
                        return $"BON-{timestamp:D6}";
                    }
                }
            }
        }
        
        // Buscar afiliado por DNI para bono
        public Afiliado BuscarAfiliadoPorDNIParaBono(string dni)
        {
            string sql = "SELECT * FROM Afiliado WHERE DNI = @DNI";
            
            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                using (var command = new SQLiteCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@DNI", dni);
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new Afiliado
                            {
                                Id = Convert.ToInt32(reader["Id"]),
                                ApellidoNombre = reader["ApellidoNombre"].ToString(),
                                DNI = reader["DNI"].ToString(),
                                Empresa = reader["Empresa"].ToString(),
                                TieneGrupoFamiliar = Convert.ToBoolean(reader["TieneGrupoFamiliar"])
                            };
                        }
                    }
                }
            }
            return null;
        }
        
        // Obtener bonos por rango de fechas
        public List<dynamic> ObtenerBonosPorRangoFechas(DateTime fechaDesde, DateTime fechaHasta)
        {
            var bonos = new List<dynamic>();
            string sql = @"SELECT b.*, a.ApellidoNombre, a.DNI, a.Empresa 
                          FROM Bono b 
                          INNER JOIN Afiliado a ON b.IdAfiliado = a.Id 
                          WHERE b.FechaEmision BETWEEN @FechaDesde AND @FechaHasta 
                          ORDER BY b.FechaEmision DESC, b.NumeroBono";
            
            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                using (var command = new SQLiteCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@FechaDesde", fechaDesde.Date);
                    command.Parameters.AddWithValue("@FechaHasta", fechaHasta.Date.AddHours(23).AddMinutes(59));
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            bonos.Add(new
                            {
                                Id = Convert.ToInt32(reader["Id"]),
                                NumeroBono = reader["NumeroBono"].ToString(),
                                FechaEmision = Convert.ToDateTime(reader["FechaEmision"]),
                                Monto = Convert.ToDecimal(reader["Monto"]),
                                Concepto = reader["Concepto"].ToString(),
                                ApellidoNombre = reader["ApellidoNombre"].ToString(),
                                DNI = reader["DNI"].ToString(),
                                Empresa = reader["Empresa"].ToString()
                            });
                        }
                    }
                }
            }
            return bonos;
        }
        
        // Obtener total de caja por rango de fechas
        public decimal ObtenerTotalCajaPorRangoFechas(DateTime fechaDesde, DateTime fechaHasta)
        {
            string sql = "SELECT COALESCE(SUM(Monto), 0) FROM Bono WHERE FechaEmision BETWEEN @FechaDesde AND @FechaHasta";
            
            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                using (var command = new SQLiteCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@FechaDesde", fechaDesde.Date);
                    command.Parameters.AddWithValue("@FechaHasta", fechaHasta.Date.AddHours(23).AddMinutes(59));
                    var result = command.ExecuteScalar();
                    return Convert.ToDecimal(result);
                }
            }
        }
        
        // Obtener resumen de caja por día
        public List<dynamic> ObtenerResumenCajaPorDia(DateTime fechaDesde, DateTime fechaHasta)
        {
            var resumen = new List<dynamic>();
            string sql = @"SELECT 
                            DATE(b.FechaEmision) as Fecha,
                            COUNT(*) as CantidadBonos,
                            SUM(b.Monto) as TotalDia
                          FROM Bono b 
                          WHERE b.FechaEmision BETWEEN @FechaDesde AND @FechaHasta 
                          GROUP BY DATE(b.FechaEmision)
                          ORDER BY Fecha DESC";
            
            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                using (var command = new SQLiteCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@FechaDesde", fechaDesde.Date);
                    command.Parameters.AddWithValue("@FechaHasta", fechaHasta.Date.AddHours(23).AddMinutes(59));
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            resumen.Add(new
                            {
                                Fecha = Convert.ToDateTime(reader["Fecha"]),
                                CantidadBonos = Convert.ToInt32(reader["CantidadBonos"]),
                                TotalDia = Convert.ToDecimal(reader["TotalDia"])
                            });
                        }
                    }
                }
            }
            return resumen;
        }

        // Actualizar datos de un afiliado
        public bool ActualizarAfiliado(Afiliado afiliado)
        {
            try
            {
                string sql = @"UPDATE Afiliado 
                              SET ApellidoNombre = @ApellidoNombre, 
                                  DNI = @DNI, 
                                  Empresa = @Empresa, 
                                  TieneGrupoFamiliar = @TieneGrupoFamiliar 
                              WHERE Id = @Id";

                using (var connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();
                    using (var command = new SQLiteCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@Id", afiliado.Id);
                        command.Parameters.AddWithValue("@ApellidoNombre", afiliado.ApellidoNombre);
                        command.Parameters.AddWithValue("@DNI", afiliado.DNI);
                        command.Parameters.AddWithValue("@Empresa", afiliado.Empresa);
                        command.Parameters.AddWithValue("@TieneGrupoFamiliar", afiliado.TieneGrupoFamiliar ? 1 : 0);
                        int filasAfectadas = command.ExecuteNonQuery();
                        return filasAfectadas > 0;
                    }
                }
            }
            catch
            {
                return false;
            }
        }

        // ========================================
        // MÉTODO PARA LIMPIAR BASE DE DATOS
        // ========================================
        
        // Limpiar todos los datos de la base de datos (para instalador)
        public string LimpiarBaseDatos()
        {
            try
            {
                using (var connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();
                    using (var transaction = connection.BeginTransaction())
                    {
                        try
                        {
                            // Desactivar verificación de claves foráneas
                            using (var command = new SQLiteCommand("PRAGMA foreign_keys = OFF", connection, transaction))
                            {
                                command.ExecuteNonQuery();
                            }

                            // Limpiar tabla de Bonos
                            using (var command = new SQLiteCommand("DELETE FROM Bono", connection, transaction))
                            {
                                int bonosEliminados = command.ExecuteNonQuery();
                            }

                            // Limpiar tabla de Recetarios
                            using (var command = new SQLiteCommand("DELETE FROM Recetario", connection, transaction))
                            {
                                int recetariosEliminados = command.ExecuteNonQuery();
                            }

                            // Limpiar tabla de Familiares
                            using (var command = new SQLiteCommand("DELETE FROM Familiar", connection, transaction))
                            {
                                int familiaresEliminados = command.ExecuteNonQuery();
                            }

                            // Limpiar tabla de Afiliados
                            using (var command = new SQLiteCommand("DELETE FROM Afiliado", connection, transaction))
                            {
                                int afiliadosEliminados = command.ExecuteNonQuery();
                            }

                            // Reiniciar contadores de auto-incremento
                            using (var command = new SQLiteCommand("DELETE FROM sqlite_sequence WHERE name IN ('Afiliado', 'Familiar', 'Recetario', 'Bono')", connection, transaction))
                            {
                                command.ExecuteNonQuery();
                            }

                            // Reactivar verificación de claves foráneas
                            using (var command = new SQLiteCommand("PRAGMA foreign_keys = ON", connection, transaction))
                            {
                                command.ExecuteNonQuery();
                            }

                            transaction.Commit();
                            return "Base de datos limpiada exitosamente. Todos los datos han sido eliminados.";
                        }
                        catch (Exception ex)
                        {
                            transaction.Rollback();
                            throw new Exception($"Error durante la limpieza: {ex.Message}", ex);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al limpiar la base de datos: {ex.Message}", ex);
            }
        }
    }
}
