using MediatR;
using Microsoft.EntityFrameworkCore;
using Atendo.Application.DTOs;
using Atendo.Application.Students.Queries;
using Atendo.Application.Interfaces;

namespace Atendo.Persistence.Students.Handlers
{
    public class GetStudentByIdHandler : IRequestHandler<GetStudentByIdQuery, StudentDto?>
    {
        private readonly IAppDbContext _db;
        public GetStudentByIdHandler(IAppDbContext db) => _db = db;

        public async Task<StudentDto?> Handle(GetStudentByIdQuery request, CancellationToken ct)
        {
            var s = await _db.Students.AsNoTracking().FirstOrDefaultAsync(x => x.Id == request.Id, ct);
            return s is null ? null : new StudentDto { Id = s.Id, FullName = s.FullName, GroupId = s.GroupId };
        }
    }
}
