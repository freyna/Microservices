using AutoMapper;
using Servicios.Api.Seguridad.Core.Entities;

namespace Servicios.Api.Seguridad.Core.DTO
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Usuario, UsuarioDTO>();
        }
    }
}
