using AutoMapper;
using ComponentesComputadoras.Application.Dtos.Cliente;
using ComponentesComputadoras.Application.Dtos.Proveedor;
using ComponentesComputadoras.Application.Dtos.SocioNegocio;
using ComponentesComputadoras.Entities;

namespace ComponentesComputadoras.WebApi.Mapping
{


    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            // Request → Entidad
            CreateMap<SocioNegocioRequestDto, SocioNegocio>();

            // Entidad → Response
            CreateMap<SocioNegocio, SocioNegocioResponseDto>()
                .ForMember(dest => dest.Cliente, opt => opt.MapFrom(src => src.Cliente))
                .ForMember(dest => dest.Proveedor, opt => opt.MapFrom(src => src.Proveedor));

            // Relaciones
            CreateMap<Cliente, ClienteResponseDto>();
            CreateMap<Proveedor, ProveedorResponseDto>();
        }
    }

}

    


