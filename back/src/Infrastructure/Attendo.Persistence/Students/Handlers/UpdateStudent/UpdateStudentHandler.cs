using MediatR;
using Microsoft.EntityFrameworkCore;
using Attendo.Application.DTOs.Students;
using Attendo.Application.Students.Commands;
using Attendo.Application.Interfaces;

namespace Attendo.Persistence.Students.Handlers.UpdateStudent
{
    public class UpdateStudentHandler : IRequestHandler<UpdateStudentCommand, StudentDto>
    {
        private readonly IAppDbContext _db;
        public UpdateStudentHandler(IAppDbContext db) => _db = db;

        public async Task<StudentDto> Handle(UpdateStudentCommand request, CancellationToken ct)
        {
            var entity = await _db.Students.FirstOrDefaultAsync(s => s.Id == request.Id, ct);
            if (entity is null)
                throw new KeyNotFoundException($"Student {request.Id} not found.");

            var groupExists = await _db.Groups.AnyAsync(g => g.Id == request.GroupId, ct);
            if (!groupExists)
                throw new ArgumentException($"Group {request.GroupId} not found.", nameof(request.GroupId));

            entity.FullName = request.FullName;
            entity.GroupId  = request.GroupId;

            await _db.SaveChangesAsync(ct);

            return new StudentDto
            {
                Id       = entity.Id,
                FullName = entity.FullName,
                GroupId  = entity.GroupId
            };
        }
    }
}
