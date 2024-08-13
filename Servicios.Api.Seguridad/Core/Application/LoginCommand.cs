using MediatR;
using Servicios.Api.Seguridad.Core.DTO;

namespace Servicios.Api.Seguridad.Core.Application
{
    public class LoginCommand : IRequest<UsuarioDTO>
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
