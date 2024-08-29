using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Servicios.Api.Seguridad.Core.DTO;
using Servicios.Api.Seguridad.Core.Entities;
using Servicios.Api.Seguridad.Jwt;

namespace Servicios.Api.Seguridad.Core.Application
{
    public class UsuarioActualHandler : IRequestHandler<UsuarioActualCommand,UsuarioDTO>
    {
        private readonly UserManager<Usuario> _userManager;
        private readonly IUsuarioSesion _usuarioSesion;
        private readonly IJwtGenerator _jwtGenerator;
        private readonly IMapper _mapper;

        public UsuarioActualHandler(UserManager<Usuario> userManager, IUsuarioSesion usuarioSesion, IJwtGenerator jwtGenerator, IMapper mapper)
        {
            _userManager = userManager;
            _usuarioSesion = usuarioSesion;
            _jwtGenerator = jwtGenerator;
            _mapper = mapper;
        }

        public async Task<UsuarioDTO> Handle(UsuarioActualCommand request, CancellationToken cancellationToken)
        {

            var userName = _usuarioSesion.GetUsuarioSesion();

            var usuario = await _userManager.FindByNameAsync(userName);

            if (usuario != null)
            {
                var usuarioDTO = _mapper.Map<Usuario, UsuarioDTO>(usuario);
                usuarioDTO.Token = await _jwtGenerator.GenerateToken(usuario);
                return usuarioDTO;
            }

            throw new Exception("Usuario no válido");
        }
    }
}
