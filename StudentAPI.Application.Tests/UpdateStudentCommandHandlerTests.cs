

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
    public class UpdateStudentCommandHandlerTests
    {
        private readonly Mock<IStudentRepository> _studentRepositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly UpdateStudentCommandHandler _sut;

        public UpdateStudentCommandHandlerTests()
        {
            _studentRepositoryMock = new Mock<IStudentRepository>();
            _mapperMock = new Mock<IMapper>();

            _sut = new UpdateStudentCommandHandler(_studentRepositoryMock.Object, _mapperMock.Object);
        }

        [Fact]
        public async Task Handle_Should_Return_True_And_Update_Student_When_Exists()
        {
            // Scenario: Success Path - Student is fetched, mapped with new values, and saved.

            // Arrange
            var command = new UpdateStudentCommand
            {
                Id = 99,
                Name = "New Name"
            };

            var studentToUpdate = new Student
            {
                Id = 99,
                Name = "Old Name"
            };

            _studentRepositoryMock.Setup(r => r.GetByIdAsync(command.Id))
                .ReturnsAsync(studentToUpdate);

            _studentRepositoryMock.Setup(r => r.UpdateAsync(studentToUpdate))
                .Returns(Task.CompletedTask);

            _mapperMock.Setup(m => m.Map(command, studentToUpdate))
                .Returns(studentToUpdate);

            // Act
            var result = await _sut.Handle(command, CancellationToken.None);

            // Assert
            result.Should().BeTrue();

            _studentRepositoryMock.Verify(r => r.GetByIdAsync(command.Id), Times.Once());
            _studentRepositoryMock.Verify(r => r.UpdateAsync(studentToUpdate), Times.Once());
            _mapperMock.Verify(m => m.Map(command, studentToUpdate), Times.Once());
        }

        [Fact]
        public async Task Handle_Should_Return_False_And_Abort_When_Student_DoesNotExist()
        {
            // Scenario: Failure Path - Student not found, no mapping or update should occur.

            // Arrange
            var command = new UpdateStudentCommand { Id = 999 };

            _studentRepositoryMock.Setup(r => r.GetByIdAsync(command.Id))
                .ReturnsAsync((Student) null);

            // Act
            var result = await _sut.Handle(command, CancellationToken.None);

            // Assert
            result.Should().BeFalse();

            _studentRepositoryMock.Verify(r => r.GetByIdAsync(command.Id), Times.Once());
            _studentRepositoryMock.Verify(r => r.UpdateAsync(It.IsAny<Student>()), Times.Never());
            _mapperMock.Verify(m => m.Map(command, It.IsAny<Student>()), Times.Never());
        }
    }
}