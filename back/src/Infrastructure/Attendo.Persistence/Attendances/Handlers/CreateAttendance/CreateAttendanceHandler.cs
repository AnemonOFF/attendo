using MediatR;
using Microsoft.EntityFrameworkCore;
using Attendo.Application.Attendances.Commands.CreateAttendance;
using Attendo.Application.DTOs.Attendances;
using Attendo.Application.Interfaces;
using Attendo.Domain.Entities;

namespace Attendo.Persistence.Attendances.Handlers
{
    public class CreateAttendanceHandler : IRequestHandler<CreateAttendanceCommand, AttendanceDto>
    {
        private readonly IAppDbContext _db;
        public CreateAttendanceHandler(IAppDbContext db) => _db = db;

        public async Task<AttendanceDto> Handle(CreateAttendanceCommand request, CancellationToken ct)
        {
            var hasStudent = await _db.Students.AnyAsync(s => s.Id == request.StudentId, ct);
            var hasClass   = await _db.Classes.AnyAsync(c => c.Id == request.ClassId, ct);

            if (!hasStudent) throw new KeyNotFoundException($"Student {request.StudentId} not found");
            if (!hasClass)   throw new KeyNotFoundException($"Class {request.ClassId} not found");

            var entity = new Attendance
            {
                StudentId = request.StudentId,
                ClassId   = request.ClassId,
                Status    = request.Status
            };

            _db.Attendances.Add(entity);
            await _db.SaveChangesAsync(ct);

            return new AttendanceDto
            {
                StudentId = entity.StudentId,
                ClassId   = entity.ClassId,
                Status    = entity.Status
            };
        }
    }
}
