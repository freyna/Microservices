using System.Security.Claims;

namespace Servicios.Api.Seguridad.Jwt
{
    public class UsuarioSesion : IUsuarioSesion
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UsuarioSesion(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public string GetUsuarioSesion()
        {
            return _httpContextAccessor.HttpContext?.User?.Claims?.FirstOrDefault(x => x.Type == "UserName")?.Value;
        }

    }
}
