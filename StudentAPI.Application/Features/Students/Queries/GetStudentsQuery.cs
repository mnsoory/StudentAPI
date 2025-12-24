using MediatR;
using StudentAPI.Application.DTOs;

namespace StudentAPI.Application.Features.Students.Queries
{
    public class GetStudentsQuery : IRequest<IEnumerable<StudentDto>>
    {

    }
}
