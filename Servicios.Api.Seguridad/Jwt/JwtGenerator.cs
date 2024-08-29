using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
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
                new Claim("username", usuario.UserName),
                new Claim("name", usuario.Nombre),
                new Claim("apellido", usuario.Apellido),
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("cf83e1357eefb8bdf1542850d66d8007d620e4050b5715dc83f4a921d36ce9ce47d0d13c5d85f2b0ff8318d2877eec2f63b931bd47417a81a538327af927da3e"));

            var credential = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var tokenDescription = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims,IdentityConstants.ApplicationScheme),
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = credential
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescription);

            return tokenHandler.WriteToken(token);
        }
    }
}
