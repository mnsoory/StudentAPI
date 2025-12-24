

using FluentValidation;
using StudentAPI.Application.Features.Students.Commands;

namespace StudentAPI.Application.Validation
{
    public class AuthenticateUserCommandValidator : AbstractValidator<AuthenticateUserCommand>
    {
        public AuthenticateUserCommandValidator()
        {
            RuleFor(a => a.Username)
                .NotEmpty().WithMessage("Name is required.")
                .Length(4, 20).WithMessage("Username must be between 4 and 20 characters.");

            RuleFor(a => a.Password)
                .NotEmpty().WithMessage("Password is required.")
                .Length(4, 20).WithMessage("Password must be between 4 and 20 characters.");
        }
    }
}
