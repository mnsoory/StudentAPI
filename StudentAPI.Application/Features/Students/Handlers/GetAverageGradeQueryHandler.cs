

using MediatR;
using StudentAPI.Application.Features.Students.Queries;
using StudentAPI.Domain.Interfaces;

namespace StudentAPI.Application.Features.Students.Handlers
{
    public class GetAverageGradeQueryHandler : IRequestHandler<GetAverageGradeQuery, double?>
    {
        private readonly IStudentRepository _studentRepository;

        public GetAverageGradeQueryHandler(IStudentRepository studentRepository)
        {
            _studentRepository = studentRepository;
        }

        public async Task<double?> Handle(GetAverageGradeQuery request, CancellationToken cancellationToken)
        {
            return await _studentRepository.GetAverageGrade();
        }
    }
}
