
using FluentAssertions;
using Moq;
using StudentAPI.Application.Features.Students.Commands;
using StudentAPI.Application.Features.Students.Handlers;
using StudentAPI.Application.Interfaces;
using StudentAPI.Domain.Entities;
using StudentAPI.Domain.Interfaces;
using Xunit;


namespace StudentAPI.Application.Tests
{
    public class AuthenticateUserCommandHandlerTests
    {
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly Mock<ITokenService> _tokenServiceMock;
        private readonly AuthenticateUserCommandHandler _sut;
        public AuthenticateUserCommandHandlerTests()
        {
            _userRepositoryMock = new Mock<IUserRepository>();
            _tokenServiceMock = new Mock<ITokenService>();

            _sut = new AuthenticateUserCommandHandler(
                _userRepositoryMock.Object,
                _tokenServiceMock.Object 
                );
        }

        [Fact]
        public async Task Handle_Should_Return_AuthenticationResponse_When_Credentials_Are_Valid()
        {
            // Scenario: Success - Username exists and password matches

            // Arrange
            var command = new AuthenticateUserCommand("testUser", "testPassword123");
            var user = new User { Username = command.Username, Password = command.Password };
            var expectedToken = "mock_jwt_token";

            _userRepositoryMock.Setup(r => r.GetByUsernameAsync(user.Username))
                .ReturnsAsync(user);

            _tokenServiceMock.Setup(t => t.GenerateToken(user))
                .Returns(expectedToken);

            //Act
            var result = await _sut.Handle(command, CancellationToken.None);

            //Assert
            result.Should().NotBeNull();
            result.Token.Should().Be(expectedToken);

            _userRepositoryMock.Verify(r => r.GetByUsernameAsync(command.Username), Times.Once());
            _tokenServiceMock.Verify(r => r.GenerateToken(user), Times.Once);
        }

        [Fact]
        public async Task Handle_Should_Return_Null_When_User_DoesNotExist()
        {
            // Scenario: Failure - Username not found in database

            // Arrange
            var command = new AuthenticateUserCommand("testUser", "testPassword123");

            _userRepositoryMock.Setup(r => r.GetByUsernameAsync(command.Username))
                .ReturnsAsync((User)null);
            //Act
            var result = await _sut.Handle(command, CancellationToken.None);

            //Assert
            result.Should().BeNull();
            _userRepositoryMock.Verify(r => r.GetByUsernameAsync(command.Username), Times.Once);
            _tokenServiceMock.Verify(r => r.GenerateToken(It.IsAny<User>()), Times.Never);
        }

        [Fact]
        public async Task Handle_Should_Return_Null_When_Password_Is_Incorrect()
        {
            // Scenario: Failure - User exists but password provided is wrong

            // Arrange
            var command = new AuthenticateUserCommand("testUser", "wrongPassword");
            var existingUser = new User { Username = command.Username, Password = "coreectPassword" };

            _userRepositoryMock.Setup(r => r.GetByUsernameAsync(command.Username))
                .ReturnsAsync(existingUser);

            //Act
            var result = await _sut.Handle(command, CancellationToken.None);

            //Assert
            result.Should().BeNull();
            _userRepositoryMock.Verify(r => r.GetByUsernameAsync(command.Username), Times.Once);
            _tokenServiceMock.Verify(t => t.GenerateToken(It.IsAny<User>()), Times.Never);
        }

        [Fact]
        public async Task Handle_Should_Not_Call_GenerateToken_When_Credentials_Are_Invalid()
        {
            // Scenario: Security check - Verify token service is never touched if login fails

            // Arrange
            var command = new AuthenticateUserCommand("correctUsername", "wrongPassword");
            var existingUser = new User { Username = command.Username, Password = "somePassword" };

            _userRepositoryMock.Setup(r => r.GetByUsernameAsync(command.Username))
                .ReturnsAsync(existingUser);

            //Act
            var result = await _sut.Handle(command, CancellationToken.None);

            //Assert
            _tokenServiceMock.Verify(t => t.GenerateToken(It.IsAny<User>()), Times.Never);
        }

        [Fact]
        public async Task Handle_Should_Call_GetByUsernameAsync_Exactly_Once()
        {
            // Scenario: Performance/Logic check - Ensure repository is called only once

            // Arrange
            var command = new AuthenticateUserCommand("anyUsername", "anyPassword");

            //Act
            var result = await _sut.Handle(command, CancellationToken.None);

            //Assert
            _userRepositoryMock.Verify(r => r.GetByUsernameAsync(command.Username), Times.Once);
        }

        [Fact]
        public async Task Handle_Should_Return_Correct_Token_Value_In_Response()
        {
            // Scenario: Integrity - Ensure the token string from TokenService is the one returned in AuthResponse

            // Arrange
            var command = new AuthenticateUserCommand("testUser", "testPassword123");
            var user = new User { Username = command.Username, Password = command.Password };
            var expectedToken = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.dummy.token";

            _userRepositoryMock.Setup(r => r.GetByUsernameAsync(user.Username))
                .ReturnsAsync(user);

            _tokenServiceMock.Setup(t => t.GenerateToken(user))
                .Returns(expectedToken);

            //Act
            var result = await _sut.Handle(command, CancellationToken.None);

            //Assert
            result.Token.Should().Be(expectedToken);
            result.Token.Should().NotBeNullOrEmpty();
        }
    }
}

