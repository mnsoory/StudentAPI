

using MediatR;

namespace StudentAPI.Application.Features.Students.Commands
{
    public class UpdateStudentCommand : IRequest<bool>
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public int Age { get; set; }
        public double Grade { get; set; }
    }
}
