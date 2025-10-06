using MediatR;
using Microsoft.EntityFrameworkCore;
using Attendo.Application.DTOs;
using Attendo.Application.Attendances.Queries.GetAttendanceById;
using Attendo.Application.Interfaces;
using Attendo.Application.DTOs.Attendances;

namespace Attendo.Persistence.Attendances.Handlers
{
    public class GetAttendanceByIdHandler : IRequestHandler<GetAttendanceByIdQuery, AttendanceDto?>
    {
        private readonly IAppDbContext _db;
        public GetAttendanceByIdHandler(IAppDbContext db) => _db = db;

        public async Task<AttendanceDto?> Handle(GetAttendanceByIdQuery request, CancellationToken ct)
        {
            var a = await _db.Attendances.AsNoTracking().FirstOrDefaultAsync(x => x.Id == request.Id, ct);
            return a is null ? null : new AttendanceDto { Id = a.Id, StudentId = a.StudentId, Status = a.Status };
        }
    }
}
