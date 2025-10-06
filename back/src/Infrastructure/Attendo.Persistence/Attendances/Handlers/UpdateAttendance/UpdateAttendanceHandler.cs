using MediatR;
using Microsoft.EntityFrameworkCore;
using Attendo.Application.Attendances.Commands.UpdateAttendance;
using Attendo.Application.DTOs.Attendances;
using Attendo.Application.Interfaces;

namespace Attendo.Persistence.Attendances.Handlers.UpdateAttendance
{
    public class UpdateAttendanceHandler : IRequestHandler<UpdateAttendanceCommand, AttendanceDto>
    {
        private readonly IAppDbContext _db;
        public UpdateAttendanceHandler(IAppDbContext db) => _db = db;

        public async Task<AttendanceDto> Handle(UpdateAttendanceCommand request, CancellationToken ct)
        {
            var entity = await _db.Attendances.FirstOrDefaultAsync(a => a.Id == request.Id, ct);
            if (entity is null)
                throw new KeyNotFoundException($"Attendance {request.Id} not found.");

            if (request.StudentId.HasValue && request.StudentId.Value != entity.StudentId)
            {
                var hasStudent = await _db.Students.AnyAsync(s => s.Id == request.StudentId.Value, ct);
                if (!hasStudent)
                    throw new ArgumentException($"Student {request.StudentId.Value} not found.", nameof(request.StudentId));

                entity.StudentId = request.StudentId.Value;
            }

            if (request.ClassId.HasValue && request.ClassId.Value != entity.ClassId)
            {
                var hasClass = await _db.Classes.AnyAsync(c => c.Id == request.ClassId.Value, ct);
                if (!hasClass)
                    throw new ArgumentException($"Class {request.ClassId.Value} not found.", nameof(request.ClassId));

                entity.ClassId = request.ClassId.Value;
            }

            entity.Status = request.Status;

            await _db.SaveChangesAsync(ct);

            return new AttendanceDto
            {
                Id        = entity.Id,
                StudentId = entity.StudentId,
                ClassId   = entity.ClassId,
                Status    = entity.Status
            };
        }
    }
}
