using Attendo.Application.Classes.Commands.DeleteClass;
using Attendo.Domain.Entities;
using Attendo.Persistence;
using Attendo.Persistence.Classes.Handlers;
using Attendo.Tests.TestInfrastructure;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;

namespace Attendo.Tests.Persistence.Classes;

public class DeleteClassHandlerTests
{
    [Fact]
    public async Task Handle_ShouldReturnFalse_WhenClassDoesNotExist()
    {
        await using var context = TestDbContextFactory.CreateContext();
        var handler = new DeleteClassHandler(context);

        var result = await handler.Handle(new DeleteClassCommand { Id = 123 }, CancellationToken.None);

        result.Should().BeFalse();
    }

    [Fact]
    public async Task Handle_ShouldRemoveClass_WhenFound()
    {
        await using var context = TestDbContextFactory.CreateContext();
        var group = new Group { Title = "Group" };
        var cls = new Class
        {
            Name = "Math",
            Start = new DateOnly(2024, 9, 1),
            End = new DateOnly(2024, 10, 1),
            Frequency = "Weekly",
            StartTime = new TimeOnly(9, 0),
            EndTime = new TimeOnly(10, 0),
            Group = group
        };

        context.Groups.Add(group);
        context.Classes.Add(cls);
        await context.SaveChangesAsync();

        var handler = new DeleteClassHandler(context);

        var result = await handler.Handle(new DeleteClassCommand { Id = cls.Id }, CancellationToken.None);

        result.Should().BeTrue();
        (await context.Classes.AnyAsync()).Should().BeFalse();
    }
}

