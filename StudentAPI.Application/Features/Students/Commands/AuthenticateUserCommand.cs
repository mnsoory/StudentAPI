

using MediatR;
using StudentAPI.Application.DTOs;

namespace StudentAPI.Application.Features.Students.Commands
{
    public record AuthenticateUserCommand(string Username, string Password) : IRequest<AuthenticationResponse>;
}
