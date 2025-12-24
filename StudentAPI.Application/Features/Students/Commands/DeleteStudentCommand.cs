

using MediatR;

namespace StudentAPI.Application.Features.Students.Commands
{
    public class DeleteStudentCommand : IRequest<bool>
    {
        public int Id { get; set; }
    }
}
