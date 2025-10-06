using MediatR;
using Microsoft.EntityFrameworkCore;
using Attendo.Application.DTOs;
using Attendo.Application.Classes.Queries;
using Attendo.Application.Interfaces;

namespace Attendo.Persistence.Classes.Handlers
{
    public class GetClassesHandler : IRequestHandler<GetClassesQuery, IReadOnlyList<ClassDto>>
    {
        private readonly IAppDbContext _db;
        public GetClassesHandler(IAppDbContext db) => _db = db;

        public async Task<IReadOnlyList<ClassDto>> Handle(GetClassesQuery request, CancellationToken ct)
        {
            var list = await _db.Classes.AsNoTracking().ToListAsync(ct);
            return list.Select(e => new ClassDto { Id = e.Id, Date = e.Date, Type = e.Type, GroupId = e.GroupId }).ToList();
        }
    }
}
