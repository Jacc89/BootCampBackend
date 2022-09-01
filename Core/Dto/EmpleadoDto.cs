using System.ComponentModel.DataAnnotations;

namespace Core.Dto
{
    public class EmpleadoDto
    {
        public int Id { get; set; }

        [Required(ErrorMessage ="El nombres es requerida")]
        [MaxLength(100, ErrorMessage ="No sea mayor a 100")]
        public string Nombres { get; set; }

        [Required(ErrorMessage ="El Apellidos es requerida")]
        [MaxLength(100, ErrorMessage ="No sea mayor a 100")]
        public  string Apellidos { get; set; }

        [Required(ErrorMessage ="El direccion es requerida")]
        [MaxLength(150, ErrorMessage ="No sea mayor a 150")]
        public string Direccion { get; set; }

        [Required(ErrorMessage ="El telefono es requerida")]
        [MaxLength(40, ErrorMessage ="No sea mayor a 40")]
        public string Telefono { get; set; }

        [Required(ErrorMessage ="El Correo es requerida")]
        [MaxLength(100, ErrorMessage ="No sea mayor a 100")]
        public string Correo { get; set; }

        [Required(ErrorMessage ="El sueldo es requerida")]
        [MaxLength(40, ErrorMessage ="No sea mayor a 40")]
        public string Sueldo { get; set; }

        [Required(ErrorMessage ="El cargo es requerida")]
        [MaxLength(50, ErrorMessage ="No sea mayor a 50")]
        public string Cargo { get; set; }
        
    }
}