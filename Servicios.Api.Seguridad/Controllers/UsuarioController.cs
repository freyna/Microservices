using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Servicios.Api.Seguridad.Core.Application;
using Servicios.Api.Seguridad.Core.DTO;

namespace Servicios.Api.Seguridad.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IValidator<RegisterCommand> _validator;

        public UsuarioController(IMediator mediator, IValidator<RegisterCommand> validator)
        {
            _mediator = mediator;
            _validator = validator;
        }

        [HttpPost("registrar")]
        public async Task<ActionResult<UsuarioDTO>> Registrar(RegisterCommand command)
        {
            var validationResult = await _validator.ValidateAsync(command);

            if (validationResult.IsValid)
            {
                return await _mediator.Send(command);
            }

            return BadRequest(validationResult.Errors);
        }

    }
}
