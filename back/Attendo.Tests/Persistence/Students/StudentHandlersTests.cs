using Attendo.Application.Students.Commands;
using Attendo.Application.Students.Queries;
using Attendo.Domain.Entities;
using Attendo.Persistence.Students.Handlers;
using Attendo.Persistence.Students.Handlers.UpdateStudent;
using Attendo.Tests.TestInfrastructure;
using FluentAssertions;

namespace Attendo.Tests.Persistence.Students;

public class CreateStudentHandlerTests
{
    [Fact]
    public async Task ShouldCreateStudent()
    {
        await using var context = TestDbContextFactory.CreateContext();
        var handler = new CreateStudentHandler(context);

        var response = await handler.Handle(new CreateStudentCommand { FullName = "Alice" }, CancellationToken.None);

        response.FullName.Should().Be("Alice");
        context.Students.Should().ContainSingle(s => s.FullName == "Alice");
    }
}

public class UpdateStudentHandlerTests
{
    [Fact]
    public async Task ShouldUpdateFullName()
    {
        await using var context = TestDbContextFactory.CreateContext();
        var student = new Student { FullName = "Old" };
        context.Students.Add(student);
        await context.SaveChangesAsync();

        var handler = new UpdateStudentHandler(context);
        var response = await handler.Handle(new UpdateStudentCommand { Id = student.Id, FullName = "New" }, CancellationToken.None);

        response.Should().NotBeNull();
        response!.FullName.Should().Be("New");
        context.Students.Single().FullName.Should().Be("New");
    }

    [Fact]
    public async Task ShouldReturnNull_WhenStudentMissing()
    {
        await using var context = TestDbContextFactory.CreateContext();
        var handler = new UpdateStudentHandler(context);

        var result = await handler.Handle(new UpdateStudentCommand { Id = 5, FullName = "Ghost" }, CancellationToken.None);

        result.Should().BeNull();
    }
}

public class DeleteStudentHandlerTests
{
    [Fact]
    public async Task ShouldReturnFalse_WhenMissing()
    {
        await using var context = TestDbContextFactory.CreateContext();
        var handler = new DeleteStudentHandler(context);

        var result = await handler.Handle(new DeleteStudentCommand { Id = 1 }, CancellationToken.None);

        result.Should().BeFalse();
    }

    [Fact]
    public async Task ShouldDeleteStudent()
    {
        await using var context = TestDbContextFactory.CreateContext();
        context.Students.Add(new Student { FullName = "Alice" });
        await context.SaveChangesAsync();

        var handler = new DeleteStudentHandler(context);

        var result = await handler.Handle(new DeleteStudentCommand { Id = 1 }, CancellationToken.None);

        result.Should().BeTrue();
        context.Students.Should().BeEmpty();
    }
}

public class StudentQueryHandlerTests
{
    [Fact]
    public async Task GetStudentById_ShouldReturnDto()
    {
        await using var context = TestDbContextFactory.CreateContext();
        context.Students.Add(new Student { FullName = "Alice" });
        await context.SaveChangesAsync();

        var handler = new GetStudentByIdHandler(context);
        var result = await handler.Handle(new GetStudentByIdQuery { Id = 1 }, CancellationToken.None);

        result.Should().NotBeNull();
        result!.FullName.Should().Be("Alice");
    }

    [Fact]
    public async Task GetStudentById_ShouldReturnNull_WhenMissing()
    {
        await using var context = TestDbContextFactory.CreateContext();
        var handler = new GetStudentByIdHandler(context);

        var result = await handler.Handle(new GetStudentByIdQuery { Id = 42 }, CancellationToken.None);

        result.Should().BeNull();
    }

    [Fact]
    public async Task GetStudents_ShouldReturnAll()
    {
        await using var context = TestDbContextFactory.CreateContext();
        context.Students.AddRange(
            new Student { FullName = "Alice" },
            new Student { FullName = "Bob" });
        await context.SaveChangesAsync();

        var handler = new GetStudentsHandler(context);

        var result = await handler.Handle(new GetStudentsQuery(), CancellationToken.None);

        result.Should().HaveCount(2);
        result.Select(s => s.FullName).Should().Contain(new[] { "Alice", "Bob" });
    }
}

