using System.Security.Claims;
using Attendo.Application.DTOs.Auth;
using Attendo.Domain.Entities;
using Attendo.Infrastructure.Auth;
using Attendo.Infrastructure.Security;
using Attendo.Infrastructure.Users;
using Attendo.WebAPI.Controllers;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace Attendo.Tests.WebApi;

public class AuthControllerTests
{
    private readonly Mock<IUserRepository> _users = new();
    private readonly Mock<IPasswordHasher> _hasher = new();
    private readonly Mock<IJwtTokenGenerator> _jwt = new();

    private AuthController CreateController()
    {
        return new AuthController(_users.Object, _hasher.Object, _jwt.Object);
    }

    [Fact]
    public async Task Register_ReturnsConflict_WhenLoginTaken()
    {
        _users.Setup(u => u.ExistsLoginAsync("taken", It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        var controller = CreateController();

        var result = await controller.Register(new RegisterRequest
        {
            Login = "taken",
            Email = "user@mail.com",
            Password = "secret"
        }, CancellationToken.None);

        result.Should().BeOfType<ConflictObjectResult>();
        _users.Verify(u => u.ExistsEmailAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task Register_CreatesUser_WhenUnique()
    {
        _users.Setup(u => u.ExistsLoginAsync(It.IsAny<string>(), It.IsAny<CancellationToken>())).ReturnsAsync(false);
        _users.Setup(u => u.ExistsEmailAsync(It.IsAny<string>(), It.IsAny<CancellationToken>())).ReturnsAsync(false);
        _hasher.Setup(h => h.Hash("secret")).Returns("hashed");

        var controller = CreateController();
        var result = await controller.Register(new RegisterRequest
        {
            Login = "demo",
            Email = "demo@mail.com",
            Password = "secret"
        }, CancellationToken.None);

        var ok = result.Should().BeOfType<OkObjectResult>().Subject;
        _users.Verify(u => u.AddAsync(It.Is<User>(user =>
                user.Login == "demo" &&
                user.Email == "demo@mail.com" &&
                user.PasswordHash == "hashed"),
            It.IsAny<CancellationToken>()), Times.Once);
        ok.Value.Should().BeEquivalentTo(new { Id = 0, Login = "demo", Email = "demo@mail.com" }, options => options.ExcludingMissingMembers());
    }

    [Fact]
    public async Task Login_ReturnsUnauthorized_WhenUserMissing()
    {
        _users.Setup(u => u.FindByLoginOrEmailAsync("ghost", It.IsAny<CancellationToken>()))
            .ReturnsAsync((User?)null);

        var controller = CreateController();
        var result = await controller.Login(new LoginRequest { Login = "ghost", Password = "secret" }, CancellationToken.None);

        result.Result.Should().BeOfType<UnauthorizedObjectResult>();
    }

    [Fact]
    public async Task Login_ReturnsUnauthorized_WhenPasswordInvalid()
    {
        var user = new User { Login = "demo", Email = "demo@mail.com", PasswordHash = "hash" };
        _users.Setup(u => u.FindByLoginOrEmailAsync("demo", It.IsAny<CancellationToken>()))
            .ReturnsAsync(user);
        _hasher.Setup(h => h.Verify("bad", "hash")).Returns(false);

        var controller = CreateController();
        var result = await controller.Login(new LoginRequest { Login = "demo", Password = "bad" }, CancellationToken.None);

        result.Result.Should().BeOfType<UnauthorizedObjectResult>();
    }

    [Fact]
    public async Task Login_ReturnsToken_WhenCredentialsValid()
    {
        var user = new User { Id = 7, Login = "demo", Email = "demo@mail.com", PasswordHash = "hash" };
        _users.Setup(u => u.FindByLoginOrEmailAsync("demo", It.IsAny<CancellationToken>()))
            .ReturnsAsync(user);
        _hasher.Setup(h => h.Verify("good", "hash")).Returns(true);
        var expires = DateTime.UtcNow.AddMinutes(30);
        _jwt.Setup(j => j.CreateToken(user)).Returns(("token", expires));

        var controller = CreateController();
        var result = await controller.Login(new LoginRequest { Login = "demo", Password = "good" }, CancellationToken.None);

        var ok = result.Result.Should().BeOfType<OkObjectResult>().Subject;
        ok.Value.Should().BeEquivalentTo(new AuthResponse { AccessToken = "token", ExpiresAt = expires });
    }

    [Fact]
    public async Task Me_ReturnsUnauthorized_WhenClaimMissing()
    {
        var controller = CreateController();
        controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext
            {
                User = new ClaimsPrincipal(new ClaimsIdentity())
            }
        };

        var result = await controller.Me(CancellationToken.None);

        result.Result.Should().BeOfType<UnauthorizedResult>();
    }

    [Fact]
    public async Task Me_ReturnsUser_WhenRepositoryMatches()
    {
        var controller = CreateController();
        var httpContext = new DefaultHttpContext
        {
            User = new ClaimsPrincipal(new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.NameIdentifier, "5")
            }))
        };
        controller.ControllerContext = new ControllerContext { HttpContext = httpContext };

        var user = new User { Id = 5, Login = "demo", Email = "demo@mail.com" };
        _users.Setup(u => u.FindByIdAsync(5, It.IsAny<CancellationToken>())).ReturnsAsync(user);

        var result = await controller.Me(CancellationToken.None);

        var ok = result.Result.Should().BeOfType<OkObjectResult>().Subject;
        ok.Value.Should().BeEquivalentTo(new AuthUserResponse
        {
            Id = 5,
            Login = "demo",
            Email = "demo@mail.com"
        });
    }
}

