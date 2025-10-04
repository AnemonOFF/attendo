using MediatR;
using Microsoft.EntityFrameworkCore;
using Attendo.Application.DTOs;
using Attendo.Application.Attendances.Queries.GetAttendancesByEvent;
using Attendo.Application.Interfaces;

namespace Attendo.Persistence.Attendances.Handlers
{
    public class GetAttendancesByEventHandler : IRequestHandler<GetAttendancesByEventQuery, IReadOnlyList<AttendanceDto>>
    {
        private readonly IAppDbContext _db;
        public GetAttendancesByEventHandler(IAppDbContext db) => _db = db;

        public async Task<IReadOnlyList<AttendanceDto>> Handle(GetAttendancesByEventQuery request, CancellationToken ct)
        {
            var list = await _db.Attendances.AsNoTracking()
                .Where(a => a.EventId == request.EventId)
                .ToListAsync(ct);

            return list.Select(a => new AttendanceDto { Id = a.Id, StudentId = a.StudentId, EventId = a.EventId, Status = a.Status }).ToList();
        }
    }
}
