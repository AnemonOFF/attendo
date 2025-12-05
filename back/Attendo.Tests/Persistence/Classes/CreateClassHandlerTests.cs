using Attendo.Application.Classes.Commands.CreateClass;
using Attendo.Domain.Entities;
using Attendo.Persistence;
using Attendo.Persistence.Classes.Handlers.CreateClass;
using Attendo.Tests.TestInfrastructure;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;

namespace Attendo.Tests.Persistence.Classes;

public class CreateClassHandlerTests
{
    [Fact]
    public async Task Handle_ShouldCreateClass_WhenGroupExists()
    {
        await using var context = TestDbContextFactory.CreateContext();
        var group = new Group
        {
            Title = "Group",
            Students =
            [
                new Student { FullName = "Alice" },
                new Student { FullName = "Bob" }
            ]
        };
        context.Groups.Add(group);
        await context.SaveChangesAsync();

        var handler = new CreateClassHandler(context);
        var command = new CreateClassCommand(
            "Biology",
            new DateOnly(2024, 11, 1),
            new DateOnly(2025, 2, 1),
            "Weekly",
            new TimeOnly(9, 0),
            new TimeOnly(10, 0),
            group.Id);

        var response = await handler.Handle(command, CancellationToken.None);

        response.Name.Should().Be("Biology");
        response.Group.Id.Should().Be(group.Id);
        response.Group.Students.Should().HaveCount(2);

        var stored = await context.Classes.AsNoTracking().SingleAsync();
        stored.GroupId.Should().Be(group.Id);
    }

    [Fact]
    public async Task Handle_ShouldThrow_WhenGroupNotFound()
    {
        await using var context = TestDbContextFactory.CreateContext();
        var handler = new CreateClassHandler(context);

        var command = new CreateClassCommand(
            "Physics",
            new DateOnly(2024, 11, 1),
            new DateOnly(2025, 2, 1),
            "Weekly",
            new TimeOnly(9, 0),
            new TimeOnly(10, 0),
            999);

        await FluentActions.Awaiting(() => handler.Handle(command, CancellationToken.None))
            .Should()
            .ThrowAsync<KeyNotFoundException>()
            .WithMessage("*999*");
    }
}

