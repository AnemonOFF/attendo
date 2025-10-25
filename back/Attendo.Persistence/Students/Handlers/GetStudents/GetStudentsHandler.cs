using Attendo.Application.DTOs;
using Attendo.Application.DTOs.Students;
using Attendo.Application.Interfaces;
using Attendo.Application.Students.Queries;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Attendo.Persistence.Students.Handlers
{
    public class GetStudentsHandler : IRequestHandler<GetStudentsQuery, IReadOnlyList<StudentDto>>
    {
        private readonly IAppDbContext _db;
        public GetStudentsHandler(IAppDbContext db) => _db = db;

        public async Task<IReadOnlyList<StudentDto>> Handle(GetStudentsQuery request, CancellationToken ct)
        {
            var list = await _db.Students.AsNoTracking().ToListAsync(ct);
            return list.Select(s => new StudentDto { Id = s.Id, FullName = s.FullName }).ToList();
        }
    }
}
