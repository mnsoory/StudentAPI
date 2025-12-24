

using MediatR;
using StudentAPI.Application.DTOs;

namespace StudentAPI.Application.Features.Students.Queries
{
    public class GetStudentQuery : IRequest<StudentDto?>
    {
        public int Id { get; set; }
    }
}
