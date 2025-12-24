

using AutoMapper;
using MediatR;
using StudentAPI.Application.Features.Students.Commands;
using StudentAPI.Domain.Entities;
using StudentAPI.Domain.Interfaces;

namespace StudentAPI.Application.Features.Students.Handlers
{
    public class CreateStudentCommandHandler : IRequestHandler<CreateStudentCommand, Student>
    {
        private readonly IStudentRepository _studentRepository;
        private readonly IMapper _mapper;
        public CreateStudentCommandHandler(IStudentRepository studentRepository, IMapper mapper)
        {
            _studentRepository = studentRepository;
            _mapper = mapper;
        }

        public async Task<Student> Handle(CreateStudentCommand command, CancellationToken cancellationToken)
        {
            var student = _mapper.Map<CreateStudentCommand, Student>(command);
            await _studentRepository.AddAsync(student);

            return student;
        }
    }
}
