using System;

namespace Centro_Empleado.Models
{
    public class Recetario
    {
        public int Id { get; set; }
        public int NumeroTalonario { get; set; }
        public int IdAfiliado { get; set; }
        public DateTime FechaEmision { get; set; }
        public DateTime FechaVencimiento { get; set; }
    }
}
