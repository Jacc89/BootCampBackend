using System.ComponentModel.DataAnnotations;

namespace Core.Models
{
    public class Empleado
    {
        [Key]
        public int Id { get; set; }
        public string Nombres { get; set; }
        public  string Apellidos { get; set; }
        public string Direccion { get; set; }
        public string Telefono { get; set; }
        public string Correo { get; set; }
        public string Sueldo { get; set; }
        public string Cargo { get; set; }

    }
}