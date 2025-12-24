

using MediatR;
using StudentAPI.Domain.Entities;

namespace StudentAPI.Application.Features.Students.Commands
{
    public class CreateStudentCommand : IRequest<Student>
    {
        public string Name { get; set; }
        public int Age { get; set; }
        public double Grade { get; set; }
    }
}
