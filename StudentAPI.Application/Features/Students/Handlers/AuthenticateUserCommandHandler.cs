

using AutoMapper;
using MediatR;
using StudentAPI.Application.DTOs;
using StudentAPI.Application.Features.Students.Commands;
using StudentAPI.Application.Interfaces;
using StudentAPI.Domain.Interfaces;

namespace StudentAPI.Application.Features.Students.Handlers
{
    public class AuthenticateUserCommandHandler : IRequestHandler<AuthenticateUserCommand, AuthenticationResponse>
    {
        private readonly IUserRepository _userRepository;
        private readonly ITokenService _tokenService;

        public AuthenticateUserCommandHandler(IUserRepository userRepository, ITokenService tokenService)
        {
            _userRepository = userRepository;
            _tokenService = tokenService;
        }
        public async Task<AuthenticationResponse> Handle(AuthenticateUserCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByUsernameAsync(request.Username);

            if (user == null || user.Password != request.Password)
                return null;

            //var userPermissions = await _userPermissionRepository.GetByUserIdAsync(user.Id);
            var token = _tokenService.GenerateToken(user);
            return new AuthenticationResponse(token);
        }
    }
}
