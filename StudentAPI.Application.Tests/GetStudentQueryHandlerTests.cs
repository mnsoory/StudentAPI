

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
    public class GetStudentQueryHandlerTests
    {
        private readonly Mock<IStudentRepository> _studentRepositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly GetStudentQueryHandler _sut;

        public GetStudentQueryHandlerTests()
        {
            _studentRepositoryMock = new Mock<IStudentRepository>();
            _mapperMock = new Mock<IMapper>();

            _sut = new GetStudentQueryHandler(_studentRepositoryMock.Object, _mapperMock.Object); 
        }


        [Fact]
        public async Task Handle_Should_Return_MappedStudent_When_Exists()
        {
            // Arrange 
            int studentId = 5;
            var request = new GetStudentQuery { Id = studentId };
            var student = new Student { Id = studentId, Name = "Test Student" };
            var studentDto = new StudentDto { Id = studentId, Name = "Test Student" };

            _studentRepositoryMock.Setup(r => r.GetByIdAsync(studentId))
                .ReturnsAsync(student);

            _mapperMock.Setup(m => m.Map<StudentDto>(student))
                .Returns(studentDto);

            // Act 
            var result = await _sut.Handle(request, CancellationToken.None);

            // Assert 
            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(student);

            // Verify Behavior
            _studentRepositoryMock.Verify(r => r.GetByIdAsync(studentId), Times.Once);
            _mapperMock.Verify(m => m.Map<StudentDto>(student), Times.Once);
        }

        [Fact]
        public async Task Handle_Should_Return_Null_And_Not_Map_When_NotFound()
        {
            // Arrange 
            var request = new GetStudentQuery { Id = 999 };

            _studentRepositoryMock.Setup(r => r.GetByIdAsync(request.Id))
                .ReturnsAsync((Student)null);

            // Act 
            var result = await _sut.Handle(request, CancellationToken.None);

            // Assert 
            result.Should().BeNull();

            // Verify Behavior
            _studentRepositoryMock.Verify(r => r.GetByIdAsync(request.Id), Times.Once);
            _mapperMock.Verify(m => m.Map<StudentDto>(It.IsAny<Student>()), Times.Never);
        }
    }
}
