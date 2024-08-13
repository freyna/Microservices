using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Servicios.Api.Seguridad.Core.Entities;

namespace Servicios.Api.Seguridad.Jwt
{
    public class JwtGenerator : IJwtGenerator
    {
        public async Task<string> GenerateToken(Usuario usuario)
        {
            var claims = new List<Claim>
            {
                new Claim("UserName", usuario.UserName),
                new Claim("Name", usuario.Nombre),
                new Claim("Apellido", usuario.Apellido),
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("I|ka5vbOv8!GAds"));

            var credential = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var tokenDescription = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = credential
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescription);

            return tokenHandler.WriteToken(token);
        }
    }
}
