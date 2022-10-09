using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Core.Dto
{
    public class RemisionUpsertDto
    {
        public int Id { get; set; }        
        public int NumRemision { get; set; }    
        public DateTime Fecha { get; set; }
        public int EncargadoId { get; set; }  
        public int ClienteId { get; set; }   
        public  string Cantidad { get; set; }
        public string Presentacion { get; set; }
        public  int ProductoId { get; set; }    
        public string Observaciones { get; set; }    
    }
}