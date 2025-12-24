

using FluentValidation;
using StudentAPI.Application.Features.Students.Commands;

namespace StudentAPI.Application.Validation
{
    public class UpdateStudentCommandValidator : AbstractValidator<UpdateStudentCommand>
    {
        public UpdateStudentCommandValidator()
        {
            RuleFor(p => p.Id)
                .NotEmpty().WithMessage("ID is required.")
                .GreaterThan(0).WithMessage("Id must be greater than 0");

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
