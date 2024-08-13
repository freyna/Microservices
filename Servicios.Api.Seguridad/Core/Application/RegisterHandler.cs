using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Servicios.Api.Seguridad.Core.DTO;
using Servicios.Api.Seguridad.Core.Entities;
using Servicios.Api.Seguridad.Core.Persistence;
using Servicios.Api.Seguridad.Jwt;

namespace Servicios.Api.Seguridad.Core.Application
{
    public class RegisterHandler : IRequestHandler<RegisterCommand,UsuarioDTO>
    {
        private readonly SeguridadContexto _context;
        private readonly UserManager<Usuario> _userManager;
        private readonly IMapper _mapper;
        private readonly IJwtGenerator _jwtGenerator;

        public RegisterHandler(SeguridadContexto context, UserManager<Usuario> userManager, IMapper mapper, IJwtGenerator jwtGenerator)
        {
            _context = context;
            _userManager = userManager;
            _mapper = mapper;
            _jwtGenerator = jwtGenerator;
        }
        public async Task<UsuarioDTO> Handle(RegisterCommand request, CancellationToken cancellationToken)
        {
            var exists = await _context.Users.AnyAsync(x => x.Email == request.Email);

            if (exists)
            {
                throw new Exception("El email del usuario ya existe en la bd");
            }

            exists = await _context.Users.AnyAsync(x => x.UserName == request.UserName);

            if (exists)
            {
                throw new Exception("El Username ya existe en la bd");
            }

            var usuario = new Usuario
            {
                Nombre = request.Nombre,
                Apellido = request.Apellido,
                Email = request.Email,
                UserName = request.UserName,
                Direccion = request.Direccion
            };

            var result  = await _userManager.CreateAsync(usuario, request.Password);

            if (result.Succeeded)
            {
                var usuarioDTO = _mapper.Map<Usuario, UsuarioDTO>(usuario);
                usuarioDTO.Token = await _jwtGenerator.GenerateToken(usuario);
                return usuarioDTO;
            }

            throw new Exception("No se pudo registrar el usuario");
        }
    }
}
