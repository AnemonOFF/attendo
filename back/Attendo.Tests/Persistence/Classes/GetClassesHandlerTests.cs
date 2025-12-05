using Attendo.Application.Classes.Queries.GetClasses;
using Attendo.Domain.Entities;
using Attendo.Persistence;
using Attendo.Persistence.Classes.Handlers.GetClasses;
using Attendo.Tests.TestInfrastructure;
using FluentAssertions;

namespace Attendo.Tests.Persistence.Classes;

public class GetClassesHandlerTests
{
    [Fact]
    public async Task Handle_ShouldFilterByDateRangeAndGroup()
    {
        await using var context = TestDbContextFactory.CreateContext();
        var groupA = new Group
        {
            Title = "A",
            Students = [new Student { FullName = "Alice" }]
        };
        var groupB = new Group
        {
            Title = "B",
            Students = [new Student { FullName = "Bob" }]
        };

        context.Groups.AddRange(groupA, groupB);
        context.Classes.AddRange(
            new Class
            {
                Name = "Early Class",
                Start = new DateOnly(2024, 9, 1),
                End = new DateOnly(2024, 9, 30),
                Frequency = "Weekly",
                StartTime = new TimeOnly(9, 0),
                EndTime = new TimeOnly(10, 0),
                Group = groupA
            },
            new Class
            {
                Name = "Target Class",
                Start = new DateOnly(2024, 10, 15),
                End = new DateOnly(2024, 12, 15),
                Frequency = "Weekly",
                StartTime = new TimeOnly(11, 0),
                EndTime = new TimeOnly(12, 0),
                Group = groupB
            });
        await context.SaveChangesAsync();

        var handler = new GetClassesHandler(context);
        var query = new GetClassesQuery
        {
            From = new DateTime(2024, 10, 1),
            To = new DateTime(2024, 12, 31),
            Group = [groupB.Id]
        };

        var result = await handler.Handle(query, CancellationToken.None);

        result.Items.Should().HaveCount(1);
        result.Items[0].Name.Should().Be("Target Class");
        result.Items[0].Group.Id.Should().Be(groupB.Id);
        result.Items[0].Group.Students.Should().HaveCount(1);
    }
}

