

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
    public class GetStudentsQueryHandlerTests
    {
        private readonly Mock<IStudentRepository> _studentRepositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly GetStudentsQueryHandler _sut;

        public GetStudentsQueryHandlerTests()
        {
            _studentRepositoryMock = new Mock<IStudentRepository>();
            _mapperMock = new Mock<IMapper>();

            _sut = new GetStudentsQueryHandler(_studentRepositoryMock.Object, _mapperMock.Object);
        }

        [Fact]
        public async Task Handle_Should_Return_Mapped_StudentDtos_When_Students_Exist()
        {
            // Scenario: Success - Repository contains students, they are mapped and returned correctly

            // Arrange
            var request = new GetStudentsQuery();
            var students = new List<Student> { new Student { Id = 1 } };
            var studentDtos = new List<StudentDto> { new StudentDto { Id = 1 } };

            _studentRepositoryMock.Setup(r => r.GetAllAsync())
                .ReturnsAsync(students);

            _mapperMock.Setup(m => m.Map<IEnumerable<StudentDto>>(students))
                .Returns(studentDtos);

            // Act
            var result = await _sut.Handle(request, CancellationToken.None);

            // Assert
            result.Should().NotBeNullOrEmpty();
            result.Should().BeEquivalentTo(studentDtos);
            result.Should().HaveCount(students.Count());

            _studentRepositoryMock.Verify(r => r.GetAllAsync(), Times.Once());
            _mapperMock.Verify(m => m.Map<IEnumerable<StudentDto>>(students), Times.Once);
        }

        [Fact]
        public async Task Handle_Should_Return_EmptyList_And_Not_Call_Mapper_When_No_Students_Exist()
        {
            // Scenario: Edge Case - Repository returns null or empty, handler returns empty list without mapping

            // Arrange
            var request = new GetStudentsQuery();

            _studentRepositoryMock.Setup(r => r.GetAllAsync())
                .ReturnsAsync((IEnumerable<Student>) null);

            // Act
            var result = await _sut.Handle(request, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEmpty();

            _studentRepositoryMock.Verify(r => r.GetAllAsync(), Times.Once());
            _mapperMock.Verify(m => m.Map<IEnumerable<StudentDto>>(It.IsAny<IEnumerable<Student>>()), Times.Never);
        }
    }
}
