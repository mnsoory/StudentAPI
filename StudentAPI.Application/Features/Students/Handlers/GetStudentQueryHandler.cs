

using AutoMapper;
using MediatR;
using StudentAPI.Application.DTOs;
using StudentAPI.Application.Features.Students.Queries;
using StudentAPI.Domain.Interfaces;

namespace StudentAPI.Application.Features.Students.Handlers
{
    public class GetStudentQueryHandler : IRequestHandler<GetStudentQuery, StudentDto?>
    {
        private readonly IStudentRepository _studentRepository;
        private readonly IMapper _mapper;

        public GetStudentQueryHandler(IStudentRepository studentRepository, IMapper mapper)
        {
            _studentRepository = studentRepository;
            _mapper = mapper;
        }

        public async Task<StudentDto?> Handle(GetStudentQuery request, CancellationToken cancellationToken)
        {
            var student = await _studentRepository.GetByIdAsync(request.Id);
            if (student == null)
                return null;

            return _mapper.Map<StudentDto>(student);
        }
    }
}
