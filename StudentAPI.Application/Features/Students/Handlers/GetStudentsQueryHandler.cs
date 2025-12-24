using AutoMapper;
using MediatR;
using StudentAPI.Application.DTOs;
using StudentAPI.Application.Features.Students.Queries;
using StudentAPI.Domain.Interfaces;

namespace StudentAPI.Application.Features.Students.Handlers
{
    public class GetStudentsQueryHandler : IRequestHandler<GetStudentsQuery, IEnumerable<StudentDto>>
    {
        private readonly IStudentRepository _studentRepository;
        private readonly IMapper _mapper;

        public GetStudentsQueryHandler(IStudentRepository studentRepository, IMapper mapper)
        {
            _studentRepository = studentRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<StudentDto>> Handle(GetStudentsQuery request, 
            CancellationToken cancellationToken)
        {
            var students = await _studentRepository.GetAllAsync();

            if(students == null || !students.Any())
            {
                return Enumerable.Empty<StudentDto>();
            }

            return _mapper.Map<IEnumerable<StudentDto>>(students);
        }
    }
}
