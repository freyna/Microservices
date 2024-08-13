using Servicios.Api.Seguridad.Core.Entities;

namespace Servicios.Api.Seguridad.Jwt
{
    public interface IJwtGenerator
    {
        Task<string> GenerateToken(Usuario usuario);
    }
}
