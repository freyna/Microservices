using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Servicios.Api.Seguridad.Core.DTO;
using Servicios.Api.Seguridad.Core.Entities;
using Servicios.Api.Seguridad.Core.Persistence;
using Servicios.Api.Seguridad.Jwt;

namespace Servicios.Api.Seguridad.Core.Application
{
    public class LoginHandler : IRequestHandler<LoginCommand,UsuarioDTO>
    {
        private readonly SeguridadContexto _context;
        private readonly UserManager<Usuario> _userManager;
        private readonly IMapper _mapper;
        private readonly IJwtGenerator _jwtGenerator;
        private readonly SignInManager<Usuario> _signInManager;

        public LoginHandler(SeguridadContexto context, UserManager<Usuario> userManager, IMapper mapper, IJwtGenerator jwtGenerator, SignInManager<Usuario> signInManager)
        {
            _context = context;
            _userManager = userManager;
            _mapper = mapper;
            _jwtGenerator = jwtGenerator;
            _signInManager = signInManager;
        }

        public async Task<UsuarioDTO> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            var usuario = await _userManager.FindByEmailAsync(request.Email);

            if (usuario == null)
            {
                throw new Exception("El usuario no existe");
            }

            var result = await _signInManager.CheckPasswordSignInAsync(usuario, request.Password, false);

            if (result.Succeeded)
            {
                var usuarioDTO = _mapper.Map<Usuario, UsuarioDTO>(usuario);
                usuarioDTO.Token = await _jwtGenerator.GenerateToken(usuario);
                return usuarioDTO;
            }

            throw new Exception("Login incorrecto");
        }
    }
}
