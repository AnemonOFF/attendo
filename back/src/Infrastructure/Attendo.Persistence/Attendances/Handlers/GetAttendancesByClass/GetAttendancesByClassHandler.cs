using MediatR;
using Microsoft.EntityFrameworkCore;
using Attendo.Application.DTOs;
using Attendo.Application.Attendances.Queries.GetAttendancesByClass;
using Attendo.Application.Interfaces;

namespace Attendo.Persistence.Attendances.Handlers
{
    public class GetAttendancesByClassHandler : IRequestHandler<GetAttendancesByClassQuery, IReadOnlyList<AttendanceDto>>
    {
        private readonly IAppDbContext _db;
        public GetAttendancesByClassHandler(IAppDbContext db) => _db = db;

        public async Task<IReadOnlyList<AttendanceDto>> Handle(GetAttendancesByClassQuery request, CancellationToken ct)
        {
            var list = await _db.Attendances.AsNoTracking()
                .Where(a => a.EventId == request.ClassId)
                .ToListAsync(ct);

            return list.Select(a => new AttendanceDto { Id = a.Id, StudentId = a.StudentId, ClassId = a.EventId, Status = a.Status }).ToList();
        }
    }
}
