using System.ComponentModel.DataAnnotations;

namespace Core.Models
{
    public class Producto
    {
        [Key]
        public int Id { get; set; }        
        public string Nombre { get; set; }
        public string Caracteristica { get; set; }
        
    }
}