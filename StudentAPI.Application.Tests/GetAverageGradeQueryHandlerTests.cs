

using FluentAssertions;
using Moq;
using StudentAPI.Application.Features.Students.Handlers;
using StudentAPI.Application.Features.Students.Queries;
using StudentAPI.Domain.Interfaces;
using Xunit;

namespace StudentAPI.Application.Tests
{
    public class GetAverageGradeQueryHandlerTests
    {
        private readonly Mock<IStudentRepository> _studentRepositoryMock;
        private readonly GetAverageGradeQueryHandler _sut;

        public GetAverageGradeQueryHandlerTests()
        {
            _studentRepositoryMock = new Mock<IStudentRepository>();

            _sut = new GetAverageGradeQueryHandler(_studentRepositoryMock.Object);
        }

        [Fact]
        public async Task Handle_Should_Return_AverageGrade_From_Repository()
        {
            // Scenario: Success - Repository returns a value, handler delegates and returns it correctly.

            // Arrange
            double expectedAverageGrade = 66.5;
            var request = new GetAverageGradeQuery();

            _studentRepositoryMock.Setup(r => r.GetAverageGrade())
                .ReturnsAsync(expectedAverageGrade);

            // Act
            var result = await _sut.Handle(request, CancellationToken.None);

            // Assert
            result.Should().Be(expectedAverageGrade);

            _studentRepositoryMock.Verify(r => r.GetAverageGrade(), Times.Once());
        }

        [Fact]
        public async Task Handle_Should_Return_Null_When_Repository_Returns_Null()
        {
            // Scenario: Edge Case - DB is empty, repository returns null, handler should pass it through.
            
            // Arrange
            var request = new GetAverageGradeQuery();

            _studentRepositoryMock.Setup(r => r.GetAverageGrade())
                .ReturnsAsync((double?)null);

            // Act
            var result = await _sut.Handle(request, CancellationToken.None);

            // Assert
            result.Should().BeNull();

            _studentRepositoryMock.Verify(r => r.GetAverageGrade(), Times.Once());
        }
    }
}
