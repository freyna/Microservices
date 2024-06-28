using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Servicios.Api.Seguridad.Core.Entities;

namespace Servicios.Api.Seguridad.Core.Persistence
{
    public class SeguridadData
    {
        public static async Task InsertarUsuario(SeguridadContexto context, UserManager<Usuario> usuarioManager)
        {
            var exists = await usuarioManager.Users.AnyAsync();

            if (!exists)
            {
                var usuario = new Usuario
                {
                    Nombre = "Filiberto",
                    Apellido = "Reyna",
                    Direccion = "Ribera 130 Zempoala",
                    UserName = "freyna",
                    Email = "ing.Filiberto.Reyna@gmail.com"
                };

                await usuarioManager.CreateAsync(usuario,"Password123$");
            }
        }
    }
}
