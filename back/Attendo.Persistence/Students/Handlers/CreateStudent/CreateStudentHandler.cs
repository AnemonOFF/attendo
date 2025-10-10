using MediatR;
using Attendo.Application.DTOs.Students;
using Attendo.Application.Students.Commands;
using Attendo.Application.Interfaces;
using Attendo.Domain.Entities;

namespace Attendo.Persistence.Students.Handlers
{
    public class CreateStudentHandler : IRequestHandler<CreateStudentCommand, StudentDto>
    {
        private readonly IAppDbContext _db;

        public CreateStudentHandler(IAppDbContext db) => _db = db;

        public async Task<StudentDto> Handle(CreateStudentCommand request, CancellationToken ct)
        {
            var entity = new Student
            {
                FullName = request.FullName
            };

            _db.Students.Add(entity);
            await _db.SaveChangesAsync(ct);

            return new StudentDto
            {
                Id = entity.Id,
                FullName = entity.FullName
            };
        }
    }
}
