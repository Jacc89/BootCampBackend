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
            
        }
    }
}