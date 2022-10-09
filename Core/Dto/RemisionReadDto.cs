using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Core.Dto
{
    public class RemisionReadDto
    {
        
        public int Id { get; set; }        
        public int NumRemision { get; set; }    
        public DateTime Fecha { get; set; }
        public string EncargadoNom { get; set; }   //nombre de encargado
        public string ClienteNom { get; set; }   //nombre de cliente
        public  string Cantidad { get; set; }
        public string Presentacion { get; set; }
        public  string ProductoNom { get; set; }    //nombre de producto
        public string Observaciones { get; set; } 
    }
}