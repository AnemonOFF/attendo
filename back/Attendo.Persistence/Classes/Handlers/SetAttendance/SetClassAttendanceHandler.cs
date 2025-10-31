using Attendo.Application.Classes.Commands.SetAttendance;
using Attendo.Application.DTOs.Classes;
using Attendo.Application.DTOs.Groups;
using Attendo.Application.DTOs.Students;
using Attendo.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Attendo.Persistence.Classes.Handlers.SetAttendance;

public sealed class SetClassAttendanceHandler : IRequestHandler<SetClassAttendanceCommand, ClassResponse>
{
    private readonly IAppDbContext _db;
    public SetClassAttendanceHandler(IAppDbContext db) => _db = db;

    public async Task<ClassResponse> Handle(SetClassAttendanceCommand request, CancellationToken ct)
    {
        var cls = await _db.Classes
            .Include(c => c.Group).ThenInclude(g => g.Students)
            .FirstOrDefaultAsync(c => c.Id == request.ClassId, ct)
            ?? throw new KeyNotFoundException($"Class {request.ClassId} not found");

        var allowed = cls.Group.Students.Select(s => s.Id).ToHashSet();
        foreach (var item in request.Attendance)
        {
            if (!item.Students.All(id => allowed.Contains(id)))
            {
                throw new InvalidOperationException("Attendance includes student not in class group.");
            }
        }

        var dates = request.Attendance.Select(a => a.Date).Distinct().ToList();

        var existing = await _db.ClassAttendances
            .Include(a => a.Students)
            .Where(a => a.ClassId == cls.Id && dates.Contains(a.Date))
            .ToListAsync(ct);

        foreach (var item in request.Attendance)
        {
            var entry = existing.FirstOrDefault(x => x.Date == item.Date);
            if (entry is null)
            {
                entry = new Domain.Entities.ClassAttendance
                {
                    ClassId = cls.Id,
                    Date = item.Date
                };
                _db.ClassAttendances.Add(entry);
            }
            else
            {
                entry.Students.Clear();
            }

            foreach (var sid in item.Students.Distinct())
            {
                entry.Students.Add(new Domain.Entities.ClassAttendanceStudent
                {
                    ClassAttendance = entry,
                    StudentId = sid
                });
            }
        }

        await _db.SaveChangesAsync(ct);

        return new ClassResponse
        {
            Id = cls.Id,
            Name = cls.Name,
            Start = cls.Start,
            End = cls.End,
            Frequency = cls.Frequency,
            StartTime = cls.StartTime,
            EndTime = cls.EndTime,
            Group = new GroupResponse
            {
                Id = cls.Group.Id,
                Title = cls.Group.Title,
                Students = [.. cls.Group.Students.Select(s => new StudentDto
                {
                    Id = s.Id,
                    FullName = s.FullName
                })]
            }
        };
    }
}
