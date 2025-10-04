using MediatR;
using Microsoft.EntityFrameworkCore;
using Atendo.Application.DTOs;
using Atendo.Application.Students.Queries;
using Atendo.Application.Interfaces;

namespace Atendo.Persistence.Students.Handlers
{
    public class GetStudentsHandler : IRequestHandler<GetStudentsQuery, IReadOnlyList<StudentDto>>
    {
        private readonly IAppDbContext _db;
        public GetStudentsHandler(IAppDbContext db) => _db = db;

        public async Task<IReadOnlyList<StudentDto>> Handle(GetStudentsQuery request, CancellationToken ct)
        {
            var list = await _db.Students.AsNoTracking().ToListAsync(ct);
            return list.Select(s => new StudentDto { Id = s.Id, FullName = s.FullName, GroupId = s.GroupId }).ToList();
        }
    }
}
