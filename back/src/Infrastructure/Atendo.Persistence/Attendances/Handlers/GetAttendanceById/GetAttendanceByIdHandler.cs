using MediatR;
using Microsoft.EntityFrameworkCore;
using Atendo.Application.DTOs;
using Atendo.Application.Attendances.Queries.GetAttendanceById;
using Atendo.Application.Interfaces;

namespace Atendo.Persistence.Attendances.Handlers
{
    public class GetAttendanceByIdHandler : IRequestHandler<GetAttendanceByIdQuery, AttendanceDto?>
    {
        private readonly IAppDbContext _db;
        public GetAttendanceByIdHandler(IAppDbContext db) => _db = db;

        public async Task<AttendanceDto?> Handle(GetAttendanceByIdQuery request, CancellationToken ct)
        {
            var a = await _db.Attendances.AsNoTracking().FirstOrDefaultAsync(x => x.Id == request.Id, ct);
            return a is null ? null : new AttendanceDto { Id = a.Id, StudentId = a.StudentId, EventId = a.EventId, Status = a.Status };
        }
    }
}
