using MediatR;
using Servicios.Api.Seguridad.Core.DTO;

namespace Servicios.Api.Seguridad.Core.Application
{
    public class UsuarioActualCommand : IRequest<UsuarioDTO>
    {
    }
}
