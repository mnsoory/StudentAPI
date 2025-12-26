

using AutoMapper;
using MediatR;
using StudentAPI.Application.Features.Students.Commands;
using StudentAPI.Domain.Interfaces;

namespace StudentAPI.Application.Features.Students.Handlers
{
    public class DeleteStudentCommandHandler : IRequestHandler<DeleteStudentCommand, bool>
    {
        private readonly IStudentRepository _studentRepository;

        public DeleteStudentCommandHandler(IStudentRepository studentRepository)
        {
            _studentRepository = studentRepository;
        }

        public async Task<bool> Handle(DeleteStudentCommand command, CancellationToken cancellationToken)
        {
            var student = await _studentRepository.GetByIdAsync(command.Id);

            if (student == null)
                return false;

            await _studentRepository.DeleteAsync(student);
            return true;
        }
    }
}
