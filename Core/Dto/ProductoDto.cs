using System.ComponentModel.DataAnnotations;

namespace Core.Dto
{
    public class ProductoDto
    {
         public int Id { get; set; }  
        [Required(ErrorMessage ="El nombre es requerida")]
        [MaxLength(100, ErrorMessage ="No sea mayor a 100")  ]    
        public string Nombre { get; set; }

        [Required(ErrorMessage ="Las caracteristicas es requerida")]
        [MaxLength(150, ErrorMessage ="No sea mayor a 150")]
        public string Caracteristica { get; set; }
        
    }
}