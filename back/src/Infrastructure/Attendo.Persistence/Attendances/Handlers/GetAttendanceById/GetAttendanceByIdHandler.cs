using MediatR;
using Microsoft.EntityFrameworkCore;
using Attendo.Application.DTOs;
using Attendo.Application.Attendances.Queries.GetAttendanceById;
using Attendo.Application.Interfaces;

namespace Attendo.Persistence.Attendances.Handlers
{
    public class GetAttendanceByIdHandler : IRequestHandler<GetAttendanceByIdQuery, AttendanceDto?>
    {
        private readonly IAppDbContext _db;
        public GetAttendanceByIdHandler(IAppDbContext db) => _db = db;

        public async Task<AttendanceDto?> Handle(GetAttendanceByIdQuery request, CancellationToken ct)
        {
            var a = await _db.Attendances.AsNoTracking().FirstOrDefaultAsync(x => x.Id == request.Id, ct);
            return a is null ? null : new AttendanceDto { Id = a.Id, StudentId = a.StudentId, ClassId = a.EventId, Status = a.Status };
        }
    }
}
