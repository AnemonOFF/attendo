using Attendo.Application.Classes.Commands.SetAttendance;
using Attendo.Application.DTOs.Classes;
using Attendo.Application.DTOs.Groups;
using Attendo.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Attendo.Persistence.Classes.Handlers.SetAttendance
{
    public class SetClassAttendanceHandler : IRequestHandler<SetClassAttendanceCommand, ClassResponse>
    {
        private readonly IAppDbContext _db;
        public SetClassAttendanceHandler(IAppDbContext db) => _db = db;

        public async Task<ClassResponse> Handle(SetClassAttendanceCommand request, CancellationToken ct)
        {
            var entity = await _db.Classes
                .Include(c => c.Groups)
                .Include(c => c.Attendance)
                .FirstOrDefaultAsync(c => c.Id == request.ClassId, ct);

            if (entity is null)
            {
                throw new KeyNotFoundException($"Class {request.ClassId} not found");
            }

            var ids = request.StudentIds?.Distinct().ToList() ?? new List<int>();
            var students = ids.Count == 0
                ? new List<Domain.Entities.Student>()
                : await _db.Students.Where(s => ids.Contains(s.Id)).ToListAsync(ct);

            if (students.Count != ids.Count)
            {
                var found = students.Select(s => s.Id).ToHashSet();
                var missing = ids.Where(id => !found.Contains(id));
                throw new KeyNotFoundException($"Students not found: {string.Join(", ", missing)}");
            }

            entity.Attendance.Clear();
            foreach (var s in students)
            {
                entity.Attendance.Add(s);
            }

            await _db.SaveChangesAsync(ct);

            return new ClassResponse
            {
                Id = entity.Id,
                Start = entity.Start,
                End = entity.End,
                Groups = entity.Groups
                    .Select(g => new GroupDto { Id = g.Id, Title = g.Title })
                    .ToList(),
                Attendance = entity.Attendance.Select(s => s.Id).ToList()
            };
        }
    }
}
