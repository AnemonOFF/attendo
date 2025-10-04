using MediatR;
using Microsoft.EntityFrameworkCore;
using Attendo.Application.DTOs;
using Attendo.Application.Attendances.Commands.UpdateAttendance;
using Attendo.Application.Interfaces;

namespace Attendo.Persistence.Attendances.Handlers
{
    public class UpdateAttendanceHandler : IRequestHandler<UpdateAttendanceCommand, AttendanceDto>
    {
        private readonly IAppDbContext _db;
        public UpdateAttendanceHandler(IAppDbContext db) => _db = db;

        public async Task<AttendanceDto> Handle(UpdateAttendanceCommand request, CancellationToken ct)
        {
            var entity = await _db.Attendances.FirstOrDefaultAsync(x => x.Id == request.Id, ct)
                         ?? throw new KeyNotFoundException($"Attendance {request.Id} not found");

            entity.Status = request.Status;
            await _db.SaveChangesAsync(ct);

            return new AttendanceDto { Id = entity.Id, StudentId = entity.StudentId, EventId = entity.EventId, Status = entity.Status };
        }
    }
}
