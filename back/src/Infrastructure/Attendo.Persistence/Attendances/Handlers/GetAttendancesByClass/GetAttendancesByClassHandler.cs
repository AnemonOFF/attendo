using MediatR;
using Microsoft.EntityFrameworkCore;
using Attendo.Application.DTOs;
using Attendo.Application.Attendances.Queries.GetAttendancesByClass;
using Attendo.Application.Interfaces;
using Attendo.Application.DTOs.Attendances;

namespace Attendo.Persistence.Attendances.Handlers
{
    public class GetAttendancesByClassHandler : IRequestHandler<GetAttendancesByClassQuery, IReadOnlyList<AttendanceDto>>
    {
        private readonly IAppDbContext _db;
        public GetAttendancesByClassHandler(IAppDbContext db) => _db = db;

        public async Task<IReadOnlyList<AttendanceDto>> Handle(GetAttendancesByClassQuery request, CancellationToken ct)
        {
            var list = await _db.Attendances.AsNoTracking()
                .Where(a => a.ClassId == request.ClassId)
                .ToListAsync(ct);

            return list.Select(a => new AttendanceDto { Id = a.Id, StudentId = a.StudentId, ClassId = a.ClassId, Status = a.Status }).ToList();
        }
    }
}
