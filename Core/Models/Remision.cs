using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Core.Models
{

    public class Remision
    {
        [Key]
        public int Id { get; set; }
        public int NumRemision { get; set; }        
        public DateTime Fecha { get; set; }
        
        public int EncargadoId { get; set; }
        [ForeignKey("EncargadoId")]
        public Empleado Empleado { get; set; }

        public int ClienteId { get; set; }
        [ForeignKey("ClienteId")]
        public Cliente Cliente { get; set; }

        public  string Cantidad { get; set; }
        public string Presentacion { get; set; }
        public  int ProductoId { get; set; }
        [ForeignKey("ProductoId")]
        public Producto Producto { get; set; }

        public string Observaciones { get; set; }    
     
    }
}