using System;

namespace Centro_Empleado.Models
{
    public class Bono
    {
        public int Id { get; set; }
        public int IdAfiliado { get; set; }
        public string NumeroBono { get; set; }
        public DateTime FechaEmision { get; set; }
        public decimal Monto { get; set; }
        public string Concepto { get; set; }
        public string Observaciones { get; set; }
        public DateTime FechaCreacion { get; set; }
        
        // Propiedad de navegación
        public Afiliado Afiliado { get; set; }
    }
}
