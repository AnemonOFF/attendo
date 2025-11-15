using Attendo.Domain.Entities;
using Attendo.Infrastructure.Users;
using Attendo.Tests.TestInfrastructure;
using FluentAssertions;

namespace Attendo.Tests.Infrastructure.Users;

public class UserRepositoryTests
{
    [Fact]
    public async Task FindByLoginOrEmailAsync_ShouldMatchOnLoginOrEmail()
    {
        await using var context = TestDbContextFactory.CreateContext();
        context.Users.AddRange(
            new User { Login = "alpha", Email = "alpha@mail.com", PasswordHash = "hash" },
            new User { Login = "beta", Email = "beta@mail.com", PasswordHash = "hash" });
        await context.SaveChangesAsync();

        var repo = new UserRepository(context);

        var byLogin = await repo.FindByLoginOrEmailAsync("alpha", CancellationToken.None);
        var byEmail = await repo.FindByLoginOrEmailAsync("beta@mail.com", CancellationToken.None);

        byLogin.Should().NotBeNull();
        byLogin!.Email.Should().Be("alpha@mail.com");
        byEmail.Should().NotBeNull();
        byEmail!.Login.Should().Be("beta");
    }

    [Fact]
    public async Task ExistsChecks_ShouldReflectState()
    {
        await using var context = TestDbContextFactory.CreateContext();
        context.Users.Add(new User { Login = "alpha", Email = "alpha@mail.com", PasswordHash = "hash" });
        await context.SaveChangesAsync();

        var repo = new UserRepository(context);

        (await repo.ExistsLoginAsync("alpha", CancellationToken.None)).Should().BeTrue();
        (await repo.ExistsEmailAsync("alpha@mail.com", CancellationToken.None)).Should().BeTrue();
        (await repo.ExistsLoginAsync("beta", CancellationToken.None)).Should().BeFalse();
    }

    [Fact]
    public async Task AddAsync_ShouldPersistUser()
    {
        await using var context = TestDbContextFactory.CreateContext();
        var repo = new UserRepository(context);

        var user = new User { Login = "gamma", Email = "gamma@mail.com", PasswordHash = "hash" };
        var created = await repo.AddAsync(user, CancellationToken.None);

        created.Id.Should().BeGreaterThan(0);
        (await repo.FindByIdAsync(created.Id, CancellationToken.None))!.Login.Should().Be("gamma");
    }
}

