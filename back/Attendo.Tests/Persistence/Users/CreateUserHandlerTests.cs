using Attendo.Application.Users.Commands.CreateUser;
using Attendo.Persistence.Users.Handlers.CreateUser;
using Attendo.Tests.TestInfrastructure;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;

namespace Attendo.Tests.Persistence.Users;

public class CreateUserHandlerTests
{
    [Fact]
    public async Task Handle_ShouldCreateUser_WhenCredentialsAreUnique()
    {
        await using var context = TestDbContextFactory.CreateContext();
        var handler = new CreateUserHandler(context);
        var command = new CreateUserCommand
        {
            Login = "new-user",
            Email = "new@attendo.dev",
            Password = "PlainText!"
        };

        var response = await handler.Handle(command, CancellationToken.None);

        response.Login.Should().Be(command.Login);
        response.Email.Should().Be(command.Email);

        var stored = await context.Users.SingleAsync();
        stored.PasswordHash.Should().NotBeNullOrWhiteSpace();
        stored.PasswordHash.Should().NotBe(command.Password);
        stored.Login.Should().Be(command.Login);
    }

    [Fact]
    public async Task Handle_ShouldThrow_WhenLoginOrEmailExists()
    {
        await using var context = TestDbContextFactory.CreateContext();
        context.Users.Add(new Attendo.Domain.Entities.User
        {
            Login = "existing",
            Email = "existing@attendo.dev",
            PasswordHash = "hash"
        });
        await context.SaveChangesAsync();

        var handler = new CreateUserHandler(context);
        var command = new CreateUserCommand
        {
            Login = "existing",
            Email = "new@attendo.dev",
            Password = "secret"
        };

        await FluentActions.Awaiting(() => handler.Handle(command, CancellationToken.None))
            .Should()
            .ThrowAsync<InvalidOperationException>()
            .WithMessage("*already exists*");
    }
}

