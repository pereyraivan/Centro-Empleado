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
            // Crear la base de datos si no existe
            string dbPath = "CentroEmpleado.db";
            bool nuevaBaseDatos = !File.Exists(dbPath);
            
            if (nuevaBaseDatos)
            {
                SQLiteConnection.CreateFile(dbPath);
            }
            
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
            string sql = @"SELECT COUNT(*) FROM Recetario 
                          WHERE IdAfiliado = @IdAfiliado 
                          AND strftime('%Y-%m', FechaEmision) = strftime('%Y-%m', @FechaReferencia)";

            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                using (var command = new SQLiteCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@IdAfiliado", idAfiliado);
                    command.Parameters.AddWithValue("@FechaReferencia", fechaReferencia);
                    return Convert.ToInt32(command.ExecuteScalar());
                }
            }
        }

        public DateTime? FechaProximaHabilitacion(int idAfiliado)
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
                        DateTime ultimaFecha = Convert.ToDateTime(result);
                        return new DateTime(ultimaFecha.Year, ultimaFecha.Month, 1).AddMonths(1);
                    }
                }
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

        // Método para obtener 2 números consecutivos de una vez
        public (int primero, int segundo) ObtenerDosNumerosConsecutivos()
        {
            string sql = "SELECT IFNULL(MAX(NumeroTalonario), 0) + 1 FROM Recetario";

            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                using (var command = new SQLiteCommand(sql, connection))
                {
                    int primero = Convert.ToInt32(command.ExecuteScalar());
                    int segundo = primero + 1;
                    return (primero, segundo);
                }
            }
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

        // Eliminar afiliado por ID
        public bool EliminarAfiliado(int idAfiliado)
        {
            try
            {
                // Primero verificar si tiene recetarios emitidos
                if (TieneRecetariosEmitidos(idAfiliado))
                {
                    throw new Exception("No se puede eliminar el afiliado porque tiene recetarios emitidos.");
                }

                string sql = "DELETE FROM Afiliado WHERE Id = @Id";

                using (var connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();
                    using (var command = new SQLiteCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@Id", idAfiliado);
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
    }
}
