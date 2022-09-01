using System.ComponentModel.DataAnnotations;

namespace Core.Dto
{
    public class ClienteDto
    {

        public int Id { get; set; }

        [Required(ErrorMessage ="El nombre es requerida")]
        [MaxLength(100, ErrorMessage ="No sea mayor a 100")]
        public string Nombre { get; set; }

        [Required(ErrorMessage ="El Dirección es requerida")]
        [MaxLength(150, ErrorMessage ="No sea mayor a 150")]
        public string Direccion { get; set; }

        [Required(ErrorMessage ="El Telefono es requerida")]
        [MaxLength(40, ErrorMessage ="No sea mayor a 40")]
        public string Telefono { get; set; }
        
    }
}