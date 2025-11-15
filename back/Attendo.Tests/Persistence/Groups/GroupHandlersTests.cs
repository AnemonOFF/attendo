using Attendo.Application.Groups.Commands;
using Attendo.Application.Groups.Commands.UpdateGroup;
using Attendo.Application.Groups.Queries.GetGroupById;
using Attendo.Application.Groups.Queries.GetGroups;
using Attendo.Domain.Entities;
using Attendo.Persistence.Groups.Handlers;
using Attendo.Persistence.Groups.Handlers.GetGroupById;
using Attendo.Persistence.Groups.Handlers.GetGroups;
using Attendo.Persistence.Groups.Handlers.UpdateGroup;
using Attendo.Tests.TestInfrastructure;
using FluentAssertions;

namespace Attendo.Tests.Persistence.Groups;

public class CreateGroupHandlerTests
{
    [Fact]
    public async Task ShouldCreateGroup()
    {
        await using var context = TestDbContextFactory.CreateContext();
        var handler = new CreateGroupHandler(context);

        var response = await handler.Handle(new CreateGroupCommand { Title = "New Group" }, CancellationToken.None);

        response.Id.Should().BeGreaterThan(0);
        response.Title.Should().Be("New Group");

        context.Groups.Should().ContainSingle(g => g.Title == "New Group");
    }
}

public class DeleteGroupHandlerTests
{
    [Fact]
    public async Task ShouldReturnFalse_WhenGroupMissing()
    {
        await using var context = TestDbContextFactory.CreateContext();
        var handler = new DeleteGroupHandler(context);

        var result = await handler.Handle(new DeleteGroupCommand { Id = 1 }, CancellationToken.None);

        result.Should().BeFalse();
    }

    [Fact]
    public async Task ShouldDeleteGroup()
    {
        await using var context = TestDbContextFactory.CreateContext();
        context.Groups.Add(new Group { Title = "Group" });
        await context.SaveChangesAsync();

        var handler = new DeleteGroupHandler(context);

        var result = await handler.Handle(new DeleteGroupCommand { Id = 1 }, CancellationToken.None);

        result.Should().BeTrue();
        context.Groups.Should().BeEmpty();
    }
}

public class GetGroupHandlerTests
{
    [Fact]
    public async Task GetGroupById_ShouldReturnNull_WhenMissing()
    {
        await using var context = TestDbContextFactory.CreateContext();
        var handler = new GetGroupByIdHandler(context);

        var result = await handler.Handle(new GetGroupByIdQuery { Id = 5 }, CancellationToken.None);

        result.Should().BeNull();
    }

    [Fact]
    public async Task GetGroups_ShouldApplyPaginationAndIncludeStudents()
    {
        await using var context = TestDbContextFactory.CreateContext();
        context.Groups.AddRange(
            new Group
            {
                Title = "First",
                Students = [new Student { FullName = "Alice" }]
            },
            new Group
            {
                Title = "Second",
                Students = [new Student { FullName = "Bob" }, new Student { FullName = "Charlie" }]
            },
            new Group
            {
                Title = "Third"
            });
        await context.SaveChangesAsync();

        var handler = new GetGroupsHandler(context);

        var result = await handler.Handle(new GetGroupsQuery { Offset = 1, Limit = 1 }, CancellationToken.None);

        result.Total.Should().Be(3);
        result.Items.Should().ContainSingle();
        result.Items[0].Title.Should().Be("Second");
        result.Items[0].Students.Should().HaveCount(2);
    }
}

public class UpdateGroupHandlerTests
{
    [Fact]
    public async Task ShouldUpdateTitleAndStudents()
    {
        await using var context = TestDbContextFactory.CreateContext();
        var students = new[]
        {
            new Student { FullName = "Alice" },
            new Student { FullName = "Bob" },
            new Student { FullName = "Charlie" }
        };

        context.Students.AddRange(students);

        var group = new Group
        {
            Title = "Original",
            Students = [students[0]]
        };
        context.Groups.Add(group);
        await context.SaveChangesAsync();

        var handler = new UpdateGroupHandler(context);
        var command = new UpdateGroupCommand
        {
            Id = group.Id,
            Title = "Updated",
            Students = [students[1].Id, students[2].Id]
        };

        var response = await handler.Handle(command, CancellationToken.None);

        response.Title.Should().Be("Updated");
        response.Students.Select(s => s.Id).Should().BeEquivalentTo(new[] { students[1].Id, students[2].Id });
    }

    [Fact]
    public async Task ShouldThrow_WhenStudentMissing()
    {
        await using var context = TestDbContextFactory.CreateContext();
        var group = new Group { Title = "Group" };
        context.Groups.Add(group);
        await context.SaveChangesAsync();

        var handler = new UpdateGroupHandler(context);

        await FluentActions.Awaiting(() => handler.Handle(
                new UpdateGroupCommand { Id = group.Id, Students = [999] },
                CancellationToken.None))
            .Should()
            .ThrowAsync<KeyNotFoundException>()
            .WithMessage("*999*");
    }
}

