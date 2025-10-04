using MediatR;
using Microsoft.EntityFrameworkCore;
using Attendo.Application.DTOs;
using Attendo.Application.Attendances.Queries.GetAttendances;
using Attendo.Application.Interfaces;

namespace Attendo.Persistence.Attendances.Handlers
{
    public class GetAttendancesHandler : IRequestHandler<GetAttendancesQuery, IReadOnlyList<AttendanceDto>>
    {
        private readonly IAppDbContext _db;
        public GetAttendancesHandler(IAppDbContext db) => _db = db;

        public async Task<IReadOnlyList<AttendanceDto>> Handle(GetAttendancesQuery request, CancellationToken ct)
        {
            var list = await _db.Attendances.AsNoTracking().ToListAsync(ct);
            return list.Select(a => new AttendanceDto { Id = a.Id, StudentId = a.StudentId, EventId = a.EventId, Status = a.Status }).ToList();
        }
    }
}
