using Attendo.Application.Users.Commands.DeleteUser;
using Attendo.Application.Users.Commands.UpdateUser;
using Attendo.Application.Users.Queries.GetUserById;
using Attendo.Application.Users.Queries.GetUsers;
using Attendo.Domain.Entities;
using Attendo.Persistence.Users.Handlers.DeleteUser;
using Attendo.Persistence.Users.Handlers.GetUserById;
using Attendo.Persistence.Users.Handlers.GetUsers;
using Attendo.Persistence.Users.Handlers.UpdateUser;
using Attendo.Tests.TestInfrastructure;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;

namespace Attendo.Tests.Persistence.Users;

public class UpdateUserHandlerTests
{
    [Fact]
    public async Task ShouldUpdateEmailAndPassword()
    {
        await using var context = TestDbContextFactory.CreateContext();
        var user = new User
        {
            Login = "user",
            Email = "old@mail.com",
            PasswordHash = "hash"
        };
        context.Users.Add(user);
        await context.SaveChangesAsync();

        var handler = new UpdateUserHandler(context);
        var command = new UpdateUserCommand
        {
            Id = user.Id,
            Email = "new@mail.com",
            NewPassword = "Sup3rSecret!"
        };

        var response = await handler.Handle(command, CancellationToken.None);

        response.Should().NotBeNull();
        response!.Email.Should().Be("new@mail.com");

        var stored = await context.Users.SingleAsync();
        stored.Email.Should().Be("new@mail.com");
        stored.PasswordHash.Should().NotBe("hash");
    }

    [Fact]
    public async Task ShouldReturnNull_WhenUserMissing()
    {
        await using var context = TestDbContextFactory.CreateContext();
        var handler = new UpdateUserHandler(context);

        var result = await handler.Handle(
            new UpdateUserCommand { Id = 42, Email = "ghost@mail.com" },
            CancellationToken.None);

        result.Should().BeNull();
    }
}

public class DeleteUserHandlerTests
{
    [Fact]
    public async Task ShouldReturnFalse_WhenUserNotFound()
    {
        await using var context = TestDbContextFactory.CreateContext();
        var handler = new DeleteUserHandler(context);

        var result = await handler.Handle(new DeleteUserCommand { Id = 1 }, CancellationToken.None);

        result.Should().BeFalse();
    }

    [Fact]
    public async Task ShouldRemoveUser_WhenExists()
    {
        await using var context = TestDbContextFactory.CreateContext();
        context.Users.Add(new User
        {
            Login = "user",
            Email = "user@mail.com",
            PasswordHash = "hash"
        });
        await context.SaveChangesAsync();

        var handler = new DeleteUserHandler(context);

        var result = await handler.Handle(new DeleteUserCommand { Id = 1 }, CancellationToken.None);

        result.Should().BeTrue();
        (await context.Users.AnyAsync()).Should().BeFalse();
    }
}

public class UserQueryHandlersTests
{
    [Fact]
    public async Task GetUserById_ShouldReturnDto_WhenFound()
    {
        await using var context = TestDbContextFactory.CreateContext();
        context.Users.Add(new User
        {
            Login = "user",
            Email = "user@mail.com",
            PasswordHash = "hash"
        });
        await context.SaveChangesAsync();

        var handler = new GetUserByIdHandler(context);

        var result = await handler.Handle(new GetUserByIdQuery { Id = 1 }, CancellationToken.None);

        result.Should().NotBeNull();
        result!.Login.Should().Be("user");
    }

    [Fact]
    public async Task GetUserById_ShouldReturnNull_WhenMissing()
    {
        await using var context = TestDbContextFactory.CreateContext();
        var handler = new GetUserByIdHandler(context);

        var result = await handler.Handle(new GetUserByIdQuery { Id = 99 }, CancellationToken.None);

        result.Should().BeNull();
    }

    [Fact]
    public async Task GetUsers_ShouldReturnOrderedList()
    {
        await using var context = TestDbContextFactory.CreateContext();
        context.Users.AddRange(
            new User { Id = 10, Login = "charlie", Email = "c@mail.com", PasswordHash = "x" },
            new User { Id = 5, Login = "alice", Email = "a@mail.com", PasswordHash = "x" },
            new User { Id = 7, Login = "bob", Email = "b@mail.com", PasswordHash = "x" });
        await context.SaveChangesAsync();

        var handler = new GetUsersHandler(context);

        var result = await handler.Handle(new GetUsersQuery(), CancellationToken.None);

        result.Items.Select(i => i.Id).Should().Equal(new[] { 5, 7, 10 });
    }
}

