using AutoMapper;
using Core.Dto;
using Core.Models;

namespace API.Helpers
{
    public class MappingProfile: Profile
    {
        public MappingProfile()
        {
            CreateMap<Cliente, ClienteDto>().ReverseMap();
            CreateMap<Producto, ProductoDto>().ReverseMap();
            CreateMap<Empleado, EmpleadoDto>().ReverseMap();
            CreateMap<Remision, RemisionDto>().ReverseMap();
            

              // mapeo de nombre de encargado en la remision
            CreateMap<Remision, RemisionReadDto>()
                       .ForMember(r => r.EncargadoNom, m => m.MapFrom(e=>e.Empleado.Nombres ));

              // mapeo de nombre de cliente en el Remision
            CreateMap<Remision, RemisionReadDto>()
                       .ForMember(r => r.ClienteNom, m => m.MapFrom(c => c.Cliente.Nombre));

              // mapeo de nombre de producto en el Remision
            CreateMap<Remision, RemisionReadDto>()
                       .ForMember(r=> r.ProductoNom, m => m.MapFrom(p => p.Producto.Nombre));           
        }
    }
}