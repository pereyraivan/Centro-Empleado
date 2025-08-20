using System;

namespace Centro_Empleado.Models
{
    public class Afiliado
    {
        public int Id { get; set; }
        public string ApellidoNombre { get; set; }
        public string DNI { get; set; }
        public string Empresa { get; set; }
        public bool TieneGrupoFamiliar { get; set; }
    }
}
