using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Authorization;
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
        private readonly IValidator<RegisterCommand> _registerValidator;
        private readonly IValidator<LoginCommand> _loginValidator;

        public UsuarioController(IMediator mediator, IValidator<RegisterCommand> validator, IValidator<LoginCommand> loginValidator)
        {
            _mediator = mediator;
            _registerValidator = validator;
            _loginValidator = loginValidator;
        }

        [HttpPost("registrar")]
        public async Task<ActionResult<UsuarioDTO>> Registrar(RegisterCommand command)
        {
            var validationResult = await _registerValidator.ValidateAsync(command);

            if (validationResult.IsValid)
            {
                return await _mediator.Send(command);
            }

            return BadRequest(validationResult.Errors);
        }

        [HttpPost("login")]
        public async Task<ActionResult<UsuarioDTO>> Login(LoginCommand command)
        {
            var validationResult = await _loginValidator.ValidateAsync(command);

            if (validationResult.IsValid)
            {
                return await _mediator.Send(command);
            }

            return BadRequest(validationResult.Errors);
        }

        [HttpGet]
        public async Task<ActionResult<UsuarioDTO>> Get()
        {
            return await _mediator.Send(new UsuarioActualCommand());
        }

    }
}
