

using AutoMapper;
using FluentAssertions;
using Moq;
using StudentAPI.Application.DTOs;
using StudentAPI.Application.Features.Students.Handlers;
using StudentAPI.Application.Features.Students.Queries;
using StudentAPI.Domain.Entities;
using StudentAPI.Domain.Interfaces;
using Xunit;

namespace StudentAPI.Application.Tests
{
    public class GetPassedStudentsQueryHandlerTests
    {
        private readonly Mock<IStudentRepository> _studentRepositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly GetPassedStudentsQueryHandler _sut;

        public GetPassedStudentsQueryHandlerTests()
        {
            _studentRepositoryMock = new Mock<IStudentRepository>();
            _mapperMock = new Mock<IMapper>();

            _sut = new GetPassedStudentsQueryHandler(_studentRepositoryMock.Object, _mapperMock.Object);
        }

        [Fact]
        public async Task Handle_Should_Return_Mapped_StudentDtos_When_PassedStudents_Exist()
        {
            // Scenario: Success - Students found, repository is called once, and mapper transforms data.

            // Arrange
            var request = new GetPassedStudentsQuery();
            var passedStudents = new List<Student> { new Student { Id = 1 } };
            var passedStudentDtos = new List<StudentDto> { new StudentDto { Id = 1 } };

            _studentRepositoryMock.Setup(r => r.GetPassedStudentsAsync())
                .ReturnsAsync(passedStudents);

            _mapperMock.Setup(m => m.Map<IEnumerable<StudentDto>>(passedStudents))
                .Returns(passedStudentDtos);

            // Act
            var result = await _sut.Handle(request, CancellationToken.None);

            // Assert
            result.Should().NotBeNullOrEmpty();
            result.Should().BeEquivalentTo(passedStudentDtos);
            result.Should().HaveCount(passedStudents.Count());

            _studentRepositoryMock.Verify(r => r.GetPassedStudentsAsync(), Times.Once());
            _mapperMock.Verify(m => m.Map<IEnumerable<StudentDto>>(passedStudents), Times.Once);
        }

        [Fact]
        public async Task Handle_Should_Return_EmptyList_And_Not_Call_Mapper_When_No_Students_Pass()
        {
            // Scenario: Edge Case - Repository returns null or empty, handler returns Empty list and skips mapping for optimization.

            // Arrange
            var request = new GetPassedStudentsQuery();

            _studentRepositoryMock.Setup(r => r.GetPassedStudentsAsync())
                .ReturnsAsync((IEnumerable<Student>)null);

            // Act
            var result = await _sut.Handle(request, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEmpty();

            _studentRepositoryMock.Verify(r => r.GetPassedStudentsAsync(), Times.Once());
            _mapperMock.Verify(m => m.Map<IEnumerable<StudentDto>>(It.IsAny<IEnumerable<Student>>()), Times.Never);
        }
    }
}
