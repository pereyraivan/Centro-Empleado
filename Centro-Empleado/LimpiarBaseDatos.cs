using System;
using System.Data.SQLite;
using System.IO;

namespace Centro_Empleado
{
    public class LimpiarBaseDatos
    {
        private static string connectionString = "Data Source=CentroEmpleado.db;Version=3;";

        public static void Main(string[] args)
        {
            Console.WriteLine("========================================");
            Console.WriteLine("   LIMPIEZA DE BASE DE DATOS");
            Console.WriteLine("   Centro de Empleados de Comercio");
            Console.WriteLine("========================================");
            Console.WriteLine();

            try
            {
                // Verificar si existe la base de datos
                string dbPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "CentroEmpleado.db");
                if (!File.Exists(dbPath))
                {
                    Console.WriteLine("ERROR: No se encontró la base de datos CentroEmpleado.db");
                    Console.WriteLine("Asegúrate de que la aplicación se haya ejecutado al menos una vez.");
                    Console.ReadKey();
                    return;
                }

                Console.WriteLine($"Base de datos encontrada: {dbPath}");
                Console.WriteLine();

                // Crear copia de seguridad
                string backupPath = Path.Combine(
                    AppDomain.CurrentDomain.BaseDirectory, 
                    $"CentroEmpleado_backup_{DateTime.Now:yyyyMMdd_HHmmss}.db"
                );
                
                Console.WriteLine("Creando copia de seguridad...");
                File.Copy(dbPath, backupPath);
                Console.WriteLine($"Copia de seguridad creada: {Path.GetFileName(backupPath)}");
                Console.WriteLine();

                // Limpiar la base de datos
                Console.WriteLine("Ejecutando limpieza de datos...");
                LimpiarDatos();

                Console.WriteLine();
                Console.WriteLine("========================================");
                Console.WriteLine("    LIMPIEZA COMPLETADA EXITOSAMENTE");
                Console.WriteLine("========================================");
                Console.WriteLine();
                Console.WriteLine("La base de datos ha sido limpiada.");
                Console.WriteLine("Todos los datos han sido eliminados.");
                Console.WriteLine("La estructura de las tablas se mantiene.");
                Console.WriteLine();
                Console.WriteLine("Ahora puedes crear el instalador.");
                Console.WriteLine();
            }
            catch (Exception ex)
            {
                Console.WriteLine();
                Console.WriteLine("========================================");
                Console.WriteLine("    ERROR EN LA LIMPIEZA");
                Console.WriteLine("========================================");
                Console.WriteLine();
                Console.WriteLine($"Error: {ex.Message}");
                Console.WriteLine();
            }

            Console.WriteLine("Presiona cualquier tecla para continuar...");
            Console.ReadKey();
        }

        private static void LimpiarDatos()
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
                            Console.WriteLine($"Bonos eliminados: {bonosEliminados}");
                        }

                        // Limpiar tabla de Recetarios
                        using (var command = new SQLiteCommand("DELETE FROM Recetario", connection, transaction))
                        {
                            int recetariosEliminados = command.ExecuteNonQuery();
                            Console.WriteLine($"Recetarios eliminados: {recetariosEliminados}");
                        }

                        // Limpiar tabla de Familiares
                        using (var command = new SQLiteCommand("DELETE FROM Familiar", connection, transaction))
                        {
                            int familiaresEliminados = command.ExecuteNonQuery();
                            Console.WriteLine($"Familiares eliminados: {familiaresEliminados}");
                        }

                        // Limpiar tabla de Afiliados
                        using (var command = new SQLiteCommand("DELETE FROM Afiliado", connection, transaction))
                        {
                            int afiliadosEliminados = command.ExecuteNonQuery();
                            Console.WriteLine($"Afiliados eliminados: {afiliadosEliminados}");
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
                        Console.WriteLine("Transacción completada exitosamente.");
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw new Exception($"Error durante la limpieza: {ex.Message}", ex);
                    }
                }
            }
        }
    }
}
