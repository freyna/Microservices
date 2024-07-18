using MediatR;
using Servicios.Api.Seguridad.Core.DTO;

namespace Servicios.Api.Seguridad.Core.Application
{
    public class RegisterCommand : IRequest<UsuarioDTO>
    {
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Direccion { get; set; }
    }
}
