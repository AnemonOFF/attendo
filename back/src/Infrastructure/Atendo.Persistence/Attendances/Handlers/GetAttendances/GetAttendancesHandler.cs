using MediatR;
using Microsoft.EntityFrameworkCore;
using Atendo.Application.DTOs;
using Atendo.Application.Attendances.Queries.GetAttendances;
using Atendo.Application.Interfaces;

namespace Atendo.Persistence.Attendances.Handlers
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
