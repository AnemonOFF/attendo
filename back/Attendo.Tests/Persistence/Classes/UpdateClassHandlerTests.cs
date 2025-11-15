using Attendo.Application.Classes.Commands.UpdateClass;
using Attendo.Domain.Entities;
using Attendo.Persistence;
using Attendo.Persistence.Classes.Handlers.UpdateClass;
using Attendo.Tests.TestInfrastructure;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;

namespace Attendo.Tests.Persistence.Classes;

public class UpdateClassHandlerTests
{
    [Fact]
    public async Task Handle_ShouldUpdateAllProvidedFields()
    {
        await using var context = TestDbContextFactory.CreateContext();
        var originalGroup = new Group { Title = "Original" };
        var newGroup = new Group { Title = "Target" };
        var cls = new Class
        {
            Name = "History",
            Start = new DateOnly(2024, 9, 1),
            End = new DateOnly(2024, 12, 31),
            Frequency = "Weekly",
            StartTime = new TimeOnly(8, 0),
            EndTime = new TimeOnly(9, 0),
            Group = originalGroup
        };

        context.Groups.AddRange(originalGroup, newGroup);
        context.Classes.Add(cls);
        await context.SaveChangesAsync();

        var handler = new UpdateClassHandler(context);
        var command = new UpdateClassCommand(
            cls.Id,
            "Advanced History",
            new DateOnly(2024, 10, 1),
            new DateOnly(2025, 1, 1),
            "Bi-Weekly",
            new TimeOnly(10, 0),
            new TimeOnly(12, 0),
            newGroup.Id);

        var response = await handler.Handle(command, CancellationToken.None);

        response.Name.Should().Be("Advanced History");
        response.Start.Should().Be(new DateOnly(2024, 10, 1));
        response.End.Should().Be(new DateOnly(2025, 1, 1));
        response.Frequency.Should().Be("Bi-Weekly");
        response.StartTime.Should().Be(new TimeOnly(10, 0));
        response.EndTime.Should().Be(new TimeOnly(12, 0));
        response.Group.Id.Should().Be(newGroup.Id);

        var stored = await context.Classes.AsNoTracking().SingleAsync(c => c.Id == cls.Id);
        stored.GroupId.Should().Be(newGroup.Id);
    }

    [Fact]
    public async Task Handle_ShouldThrow_WhenGroupIdDoesNotExist()
    {
        await using var context = TestDbContextFactory.CreateContext();
        var group = new Group { Title = "Original" };
        var cls = new Class
        {
            Name = "History",
            Start = new DateOnly(2024, 9, 1),
            End = new DateOnly(2024, 12, 31),
            Frequency = "Weekly",
            StartTime = new TimeOnly(8, 0),
            EndTime = new TimeOnly(9, 0),
            Group = group
        };

        context.Groups.Add(group);
        context.Classes.Add(cls);
        await context.SaveChangesAsync();

        var handler = new UpdateClassHandler(context);
        var command = new UpdateClassCommand(cls.Id, null, null, null, null, null, null, 999);

        await FluentActions.Awaiting(() => handler.Handle(command, CancellationToken.None))
            .Should()
            .ThrowAsync<KeyNotFoundException>()
            .WithMessage("*999*");
    }
}

