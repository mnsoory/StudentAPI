

using FluentValidation;
using StudentAPI.Application.Features.Students.Commands;

namespace StudentAPI.Application.Validation
{
    public class CreateStudentCommandValidator : AbstractValidator<CreateStudentCommand>
    {
        public CreateStudentCommandValidator()
        {
            RuleFor(p => p.Name)
                .NotEmpty().WithMessage("Name is required.")
                .Length(2, 50).WithMessage("Name must be between 2 and 50 characters.");

            RuleFor(p => p.Grade)
                .InclusiveBetween(0, 100).WithMessage("Grade must be between 0 and 100.");

            RuleFor(p => p.Age)
                .InclusiveBetween(5, 50).WithMessage("Age must be between 5 and 50.");
        }

    }
}
