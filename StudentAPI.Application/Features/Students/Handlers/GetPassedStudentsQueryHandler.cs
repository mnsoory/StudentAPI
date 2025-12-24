

using AutoMapper;
using MediatR;
using StudentAPI.Application.DTOs;
using StudentAPI.Application.Features.Students.Queries;
using StudentAPI.Domain.Interfaces;

namespace StudentAPI.Application.Features.Students.Handlers
{
    public class GetPassedStudentsQueryHandler 
        : IRequestHandler<GetPassedStudentsQuery, IEnumerable<StudentDto>>
    {
        private readonly IStudentRepository _studentRepository;
        private readonly IMapper _mapper;

        public GetPassedStudentsQueryHandler(IStudentRepository studentRepository, IMapper mapper)
        {
            _studentRepository = studentRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<StudentDto>> Handle(GetPassedStudentsQuery request,
            CancellationToken cancellationToken)
        {
            var passedStudents = await _studentRepository.GetPassedStudentsAsync();

            if(passedStudents == null || !passedStudents.Any())
            {
                return Enumerable.Empty<StudentDto>();
            }

            return _mapper.Map<IEnumerable<StudentDto>>(passedStudents);
        }
    }
}
