using Attendo.Application.Classes.Queries.GetClassAttendance;
using Attendo.Application.DTOs.Classes;
using Attendo.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Attendo.Persistence.Classes.Handlers.GetAttendance;

public sealed class GetClassAttendanceHandler : IRequestHandler<GetClassAttendanceQuery, ClassAttendanceResponse>
{
    private readonly IAppDbContext _db;
    public GetClassAttendanceHandler(IAppDbContext db) => _db = db;

    public async Task<ClassAttendanceResponse> Handle(GetClassAttendanceQuery request, CancellationToken ct)
    {
        var exists = await _db.Classes.AnyAsync(c => c.Id == request.ClassId, ct);
        if (!exists)
        {
            throw new KeyNotFoundException($"Class {request.ClassId} not found");
        }

        var list = await _db.ClassAttendances
            .AsNoTracking()
            .Include(a => a.Students)
            .Where(a => a.ClassId == request.ClassId)
            .OrderBy(a => a.Date)
            .Select(a => new AttendanceItem
            {
                Date = a.Date,
                Students = a.Students.Select(s => s.StudentId).ToList()
            })
            .ToListAsync(ct);

        return new ClassAttendanceResponse { Attendance = list };
    }
}
