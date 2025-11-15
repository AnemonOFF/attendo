using Attendo.Application.Classes.Commands.SetAttendance;
using Attendo.Application.DTOs.Classes;
using Attendo.Domain.Entities;
using Attendo.Persistence;
using Attendo.Persistence.Classes.Handlers.SetAttendance;
using Attendo.Tests.TestInfrastructure;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;

namespace Attendo.Tests.Persistence.Classes;

public class SetClassAttendanceHandlerTests
{
    [Fact]
    public async Task Handle_ShouldPersistAttendanceForNewDates()
    {
        await using var context = TestDbContextFactory.CreateContext();
        var seed = await SeedClassAsync(context);
        var handler = new SetClassAttendanceHandler(context);

        var command = new SetClassAttendanceCommand(seed.ClassId,
        [
            new AttendanceItem
            {
                Date = new DateOnly(2024, 11, 1),
                Students = [seed.Students[0].Id, seed.Students[1].Id]
            },
            new AttendanceItem
            {
                Date = new DateOnly(2024, 11, 2),
                Students = [seed.Students[1].Id]
            }
        ]);

        var response = await handler.Handle(command, CancellationToken.None);

        response.Id.Should().Be(seed.ClassId);
        response.Group.Students.Should().HaveCount(seed.Students.Count);

        var stored = await context.ClassAttendances
            .Include(a => a.Students)
            .Where(a => a.ClassId == seed.ClassId)
            .OrderBy(a => a.Date)
            .ToListAsync();

        stored.Should().HaveCount(2);
        stored[0].Students.Select(s => s.StudentId).Should().BeEquivalentTo([seed.Students[0].Id, seed.Students[1].Id]);
        stored[1].Students.Select(s => s.StudentId).Should().Equal(new[] { seed.Students[1].Id });
    }

    [Fact]
    public async Task Handle_ShouldThrow_WhenStudentNotInGroup()
    {
        await using var context = TestDbContextFactory.CreateContext();
        var seed = await SeedClassAsync(context);
        var handler = new SetClassAttendanceHandler(context);

        var command = new SetClassAttendanceCommand(seed.ClassId,
        [
            new AttendanceItem
            {
                Date = new DateOnly(2024, 11, 3),
                Students = [seed.Students[0].Id, 999]
            }
        ]);

        await FluentActions.Awaiting(() => handler.Handle(command, CancellationToken.None))
            .Should()
            .ThrowAsync<InvalidOperationException>()
            .WithMessage("*not in class group*");
    }

    [Fact]
    public async Task Handle_ShouldReplaceExistingAttendanceEntries()
    {
        await using var context = TestDbContextFactory.CreateContext();
        var seed = await SeedClassAsync(context);

        context.ClassAttendances.Add(new ClassAttendance
        {
            ClassId = seed.ClassId,
            Date = new DateOnly(2024, 11, 5),
            Students =
            [
                new ClassAttendanceStudent { StudentId = seed.Students[2].Id }
            ]
        });
        await context.SaveChangesAsync();

        var handler = new SetClassAttendanceHandler(context);
        var command = new SetClassAttendanceCommand(seed.ClassId,
        [
            new AttendanceItem
            {
                Date = new DateOnly(2024, 11, 5),
                Students = [seed.Students[0].Id, seed.Students[0].Id, seed.Students[1].Id]
            }
        ]);

        await handler.Handle(command, CancellationToken.None);

        var stored = await context.ClassAttendances
            .Include(a => a.Students)
            .SingleAsync(a => a.ClassId == seed.ClassId && a.Date == new DateOnly(2024, 11, 5));

        stored.Students.Select(s => s.StudentId).Should().BeEquivalentTo([seed.Students[0].Id, seed.Students[1].Id]);
    }

    private static async Task<ClassSeedResult> SeedClassAsync(AppDbContext context)
    {
        var students = new List<Student>
        {
            new() { FullName = "Alice" },
            new() { FullName = "Bob" },
            new() { FullName = "Charlie" }
        };

        var group = new Group
        {
            Title = "Group A",
            Students = students
        };

        var cls = new Class
        {
            Name = "Math",
            Start = new DateOnly(2024, 10, 1),
            End = new DateOnly(2025, 3, 1),
            Frequency = "Weekly",
            StartTime = new TimeOnly(9, 0),
            EndTime = new TimeOnly(10, 0),
            Group = group
        };

        context.Groups.Add(group);
        context.Classes.Add(cls);
        await context.SaveChangesAsync();

        return new ClassSeedResult(cls.Id, students);
    }

    private sealed record ClassSeedResult(int ClassId, List<Student> Students);
}

