

using AutoMapper;
using MediatR;
using StudentAPI.Application.Features.Students.Commands;
using StudentAPI.Domain.Entities;
using StudentAPI.Domain.Interfaces;

namespace StudentAPI.Application.Features.Students.Handlers
{
    public class UpdateStudentCommandHandler : IRequestHandler<UpdateStudentCommand, bool>
    {
        private readonly IStudentRepository _studentRepository;
        private readonly IMapper _mapper;

        public UpdateStudentCommandHandler(IStudentRepository studentRepository, IMapper mapper)
        {
            _studentRepository = studentRepository;
            _mapper = mapper;
        }

        public async Task<bool> Handle(UpdateStudentCommand command, CancellationToken cancellationToken)
        {
            var studentToUpdate = await _studentRepository.GetByIdAsync(command.Id);

            if(studentToUpdate == null)
            {
                return false;
            }

            _mapper.Map(command, studentToUpdate);
            await _studentRepository.UpdateAsync(studentToUpdate);

            return true;

        }
    }
}
