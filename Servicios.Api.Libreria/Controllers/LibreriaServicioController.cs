using Microsoft.AspNetCore.Mvc;
using Servicios.Api.BusinessRules.Autores;
using Servicios.Api.BusinessRules.ViewModels;

namespace Servicios.Api.Libreria.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LibreriaServicioController : ControllerBase
    {
        private readonly IAutoresBusinessRules _br;
        public LibreriaServicioController(IAutoresBusinessRules br)
        {
            _br = br;
        }

        [HttpGet("autores")]
        public async Task<ActionResult<IEnumerable<AutorVM>>> GetAutores()
        {
            var resp = await _br.Autores();

            return Ok(resp);
        }
    }
}
