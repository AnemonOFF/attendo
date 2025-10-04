using MediatR;
using Microsoft.EntityFrameworkCore;
using Atendo.Application.DTOs;
using Atendo.Application.Attendances.Commands.CreateAttendance;
using Atendo.Application.Interfaces;
using Atendo.Domain.Entities;

namespace Atendo.Persistence.Attendances.Handlers
{
    public class CreateAttendanceHandler : IRequestHandler<CreateAttendanceCommand, AttendanceDto>
    {
        private readonly IAppDbContext _db;
        public CreateAttendanceHandler(IAppDbContext db) => _db = db;

        public async Task<AttendanceDto> Handle(CreateAttendanceCommand request, CancellationToken ct)
        {
            var hasStudent = await _db.Students.AnyAsync(s => s.Id == request.StudentId, ct);
            var hasEvent = await _db.Events.AnyAsync(e => e.Id == request.EventId, ct);
            if (!hasStudent) throw new KeyNotFoundException($"Student {request.StudentId} not found");
            if (!hasEvent) throw new KeyNotFoundException($"Event {request.EventId} not found");

            var entity = new Attendance { StudentId = request.StudentId, EventId = request.EventId, Status = request.Status };
            _db.Attendances.Add(entity);
            await _db.SaveChangesAsync(ct);

            return new AttendanceDto { Id = entity.Id, StudentId = entity.StudentId, EventId = entity.EventId, Status = entity.Status };
        }
    }
}
