using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StudentAPI.API.Models.Auth;
using StudentAPI.Application.DTOs;
using StudentAPI.Application.Features.Students.Commands;
using System.Threading.Tasks;

namespace StudentAPI.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public UsersController(IMediator mediator, IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Route("login")]
        public async Task<ActionResult<AuthenticationResponse>> AuthenticateUser(LoginRequest request)
        {
            var command = _mapper.Map<AuthenticateUserCommand>(request);

            var authResponse = await _mediator.Send(command);

            if (authResponse == null)
                return Unauthorized(new { message = "Invalid username or password." });

            return Ok(authResponse);
        }
    }
}
