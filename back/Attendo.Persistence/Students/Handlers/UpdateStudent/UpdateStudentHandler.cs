using Attendo.Application.DTOs.Students;
using Attendo.Application.Interfaces;
using Attendo.Application.Students.Commands;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Attendo.Persistence.Students.Handlers.UpdateStudent
{
    public class UpdateStudentHandler : IRequestHandler<UpdateStudentCommand, StudentDto?>
    {
        private readonly IAppDbContext _db;
        public UpdateStudentHandler(IAppDbContext db) => _db = db;

        public async Task<StudentDto?> Handle(UpdateStudentCommand request, CancellationToken ct)
        {
            var entity = await _db.Students.FirstOrDefaultAsync(s => s.Id == request.Id, ct);
            if (entity is null)
            {
                return null;
            }

            entity.FullName = request.FullName;

            await _db.SaveChangesAsync(ct);

            return new StudentDto
            {
                Id = entity.Id,
                FullName = entity.FullName
            };
        }
    }
}
