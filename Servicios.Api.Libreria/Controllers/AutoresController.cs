using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Servicios.Api.BusinessRules.Autores;
using Servicios.Api.BusinessRules.ViewModels;
using Servicios.Api.Datos.Entities;

namespace Servicios.Api.Libreria.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AutoresController : ControllerBase
    {
        private readonly IAutoresBusinessRules _autores;
        public AutoresController(IAutoresBusinessRules autores)
        {
            _autores = autores;
        }

        [HttpGet(Name = "GetAll")]
        public async Task<IActionResult> GetAll()
        {
            var autores = await _autores.Autores();
            return Ok(autores);
        }

        [HttpGet("{Id}",Name = "GetById")]
        public async Task<IActionResult> GetById(string Id)
        {
            var autor = await _autores.AutorById(Id);
            return Ok(autor);
        }

        [HttpPost]
        public async Task<IActionResult> Post(AutorEntity autor)
        {
            await _autores.InsertAutor(autor);
            return Ok();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(string id, AutorEntity autor)
        {
            autor.Id = id;
            await _autores.UpdateAutor(autor);
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            await _autores.DeleteAutorById(id);
            return Ok();
        }
    }
}
