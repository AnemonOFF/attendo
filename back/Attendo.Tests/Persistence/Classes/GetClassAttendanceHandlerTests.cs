using Attendo.Application.Classes.Queries.GetClassAttendance;
using Attendo.Domain.Entities;
using Attendo.Persistence;
using Attendo.Persistence.Classes.Handlers.GetAttendance;
using Attendo.Tests.TestInfrastructure;
using FluentAssertions;

namespace Attendo.Tests.Persistence.Classes;

public class GetClassAttendanceHandlerTests
{
    [Fact]
    public async Task Handle_ShouldThrow_WhenClassIsMissing()
    {
        await using var context = TestDbContextFactory.CreateContext();
        var handler = new GetClassAttendanceHandler(context);

        await FluentActions.Awaiting(() => handler.Handle(new GetClassAttendanceQuery { ClassId = 1 }, CancellationToken.None))
            .Should()
            .ThrowAsync<KeyNotFoundException>();
    }

    [Fact]
    public async Task Handle_ShouldReturnOrderedAttendance()
    {
        await using var context = TestDbContextFactory.CreateContext();
        var group = new Group { Title = "Group" };
        var cls = new Class
        {
            Name = "Economics",
            Start = new DateOnly(2024, 10, 1),
            End = new DateOnly(2024, 12, 1),
            Frequency = "Weekly",
            StartTime = new TimeOnly(11, 0),
            EndTime = new TimeOnly(12, 0),
            Group = group
        };

        var student = new Student { FullName = "Alice" };
        group.Students.Add(student);

        context.Groups.Add(group);
        context.Classes.Add(cls);
        await context.SaveChangesAsync();

        context.ClassAttendances.AddRange(
            new ClassAttendance
            {
                ClassId = cls.Id,
                Date = new DateOnly(2024, 11, 10),
                Students = [new ClassAttendanceStudent { StudentId = student.Id }]
            },
            new ClassAttendance
            {
                ClassId = cls.Id,
                Date = new DateOnly(2024, 11, 5),
                Students = [new ClassAttendanceStudent { StudentId = student.Id }]
            });
        await context.SaveChangesAsync();

        var handler = new GetClassAttendanceHandler(context);

        var result = await handler.Handle(new GetClassAttendanceQuery { ClassId = cls.Id }, CancellationToken.None);

        result.Attendance.Should().HaveCount(2);
        result.Attendance.Select(a => a.Date).Should().BeInAscendingOrder();
    }
}

