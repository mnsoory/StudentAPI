

using AutoMapper;
using FluentAssertions;
using Moq;
using StudentAPI.Application.Features.Students.Commands;
using StudentAPI.Application.Features.Students.Handlers;
using StudentAPI.Domain.Entities;
using StudentAPI.Domain.Interfaces;
using Xunit;

namespace StudentAPI.Application.Tests
{
    public class CreateStudentCommandHandlerTests
    {
        private readonly Mock<IStudentRepository> _studentRepositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly CreateStudentCommandHandler _sut;

        public CreateStudentCommandHandlerTests()
        {
            _studentRepositoryMock = new Mock<IStudentRepository>();
            _mapperMock = new Mock<IMapper>();

            _sut = new CreateStudentCommandHandler(_studentRepositoryMock.Object, _mapperMock.Object);
        }

        [Fact]
        public async Task Handle_Should_Map_Save_And_Return_Student_When_Command_Is_Valid()
        {
            // Scenario: Comprehensive success path - Command to Entity mapping, DB persistence, and returning the result.

            // Arrange
            var command = new CreateStudentCommand { Name = "Student Name", Age = 16, Grade = 87 };
            var mappedStudent = new Student { Name = "Student Name", Age = 16, Grade = 87 };

            _mapperMock.Setup(m => m.Map<CreateStudentCommand, Student>(command))
                .Returns(mappedStudent);

            _studentRepositoryMock.Setup(r => r.AddAsync(mappedStudent))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _sut.Handle(command, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(mappedStudent);

            _studentRepositoryMock.Verify(r => r.AddAsync(mappedStudent), Times.Once());
            _mapperMock.Verify(m => m.Map<CreateStudentCommand, Student>(command), Times.Once());
        }
    }
}
