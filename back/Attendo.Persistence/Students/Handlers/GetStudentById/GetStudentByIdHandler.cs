using Attendo.Application.DTOs;
using Attendo.Application.DTOs.Students;
using Attendo.Application.Interfaces;
using Attendo.Application.Students.Queries;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Attendo.Persistence.Students.Handlers
{
    public class GetStudentByIdHandler : IRequestHandler<GetStudentByIdQuery, StudentDto?>
    {
        private readonly IAppDbContext _db;
        public GetStudentByIdHandler(IAppDbContext db) => _db = db;

        public async Task<StudentDto?> Handle(GetStudentByIdQuery request, CancellationToken ct)
        {
            var s = await _db.Students.AsNoTracking().FirstOrDefaultAsync(x => x.Id == request.Id, ct);
            return s is null ? null : new StudentDto { Id = s.Id, FullName = s.FullName };
        }
    }
}
