using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Core.Models;

namespace Core.Dto
{
    public class RemisionDto
    {
        public int Id { get; set; }

        [Required(ErrorMessage ="El numero de remision es requerida")]
        [MaxLength(100, ErrorMessage ="No sea mayor a 100")]
        public int NumRemision { get; set; }    

        [Required(ErrorMessage ="El fecha es requerida")]
        [MaxLength(100, ErrorMessage ="No sea mayor a 100")]    
        public DateTime Fecha { get; set; }

        [Required(ErrorMessage ="El nombres del encargado es requerida")]
        [MaxLength(100, ErrorMessage ="No sea mayor a 100")]        
        public int EncargadoId { get; set; }
        [ForeignKey("EncargadoId")]
        public Empleado Empleado { get; set; }

        [Required(ErrorMessage ="El nombres del cliente es requerida")]
        [MaxLength(150, ErrorMessage ="No sea mayor a 150")]

        public int ClienteId { get; set; }
        [ForeignKey("ClienteId")]
        public Cliente Cliente { get; set; }
        [Required(ErrorMessage ="El cantidad es requerida")]
        [MaxLength(40, ErrorMessage ="No sea mayor a 40")]

        public  string Cantidad { get; set; }

        [Required(ErrorMessage ="El presentacion es requerida")]
        [MaxLength(100, ErrorMessage ="No sea mayor a 100")]
        public string Presentacion { get; set; }

        [Required(ErrorMessage ="El nombres del producto es requerida")]
        [MaxLength(40, ErrorMessage ="No sea mayor a 40")]
        public  int ProductoId { get; set; }
        [ForeignKey("ProductoId")]
        public Producto Producto { get; set; }

        [Required(ErrorMessage ="Observaciones requeridas")]
        [MaxLength(500, ErrorMessage ="No sea mayor a 500")]
        public string Observaciones { get; set; }  
    }
}