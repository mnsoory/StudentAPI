

using FluentAssertions;
using Moq;
using StudentAPI.Application.Features.Students.Commands;
using StudentAPI.Application.Features.Students.Handlers;
using StudentAPI.Domain.Entities;
using StudentAPI.Domain.Interfaces;
using Xunit;

namespace StudentAPI.Application.Tests
{
    public class DeleteStudentCommandHandlerTests
    {

        private readonly Mock<IStudentRepository> _studentRepositoryMock;
        private readonly DeleteStudentCommandHandler _sut;

        public DeleteStudentCommandHandlerTests()
        {
            _studentRepositoryMock = new Mock<IStudentRepository>();

            _sut = new DeleteStudentCommandHandler(_studentRepositoryMock.Object);
        }

        [Fact]
        public async Task Handle_Should_Return_True_And_Delete_When_Student_Exists()
        {
            // Scenario: Success - Student is found, then deleted, and result is true.

            // Arrange
            var command = new DeleteStudentCommand { Id = 777 };
            var studentToDelete = new Student { Id = command.Id };

            _studentRepositoryMock.Setup(r => r.GetByIdAsync(command.Id))
                .ReturnsAsync(studentToDelete);

            _studentRepositoryMock.Setup(r => r.DeleteAsync(studentToDelete))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _sut.Handle(command, CancellationToken.None);

            // Assert
            result.Should().BeTrue();

            _studentRepositoryMock.Verify(r => r.GetByIdAsync(command.Id), Times.Once);
            _studentRepositoryMock.Verify(r => r.DeleteAsync(studentToDelete), Times.Once);
        }

        [Fact]
        public async Task Handle_Should_Return_False_And_Never_Delete_When_Student_DoesNotExist()
        {
            // Scenario: Failure - Student not found, no deletion attempt should be made.

            // Arrange
            var command = new DeleteStudentCommand { Id = 777 };

            _studentRepositoryMock.Setup(r => r.GetByIdAsync(command.Id))
                .ReturnsAsync((Student) null);

            // Act
            var result = await _sut.Handle(command, CancellationToken.None);

            // Assert
            result.Should().BeFalse();

            _studentRepositoryMock.Verify(r => r.GetByIdAsync(command.Id), Times.Once);
            _studentRepositoryMock.Verify(r => r.DeleteAsync(It.IsAny<Student>()), Times.Never);
        }
    }
}