using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Atendo.Application.DTOs;
using Atendo.Application.Groups.Queries;
using Atendo.Application.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Atendo.Persistence.Groups.Handlers
{
    public class GetGroupsHandler : IRequestHandler<GetGroupsQuery, IReadOnlyList<GroupDto>>
    {
        private readonly IAppDbContext _db;
        public GetGroupsHandler(IAppDbContext db) => _db = db;

        public async Task<IReadOnlyList<GroupDto>> Handle(GetGroupsQuery request, CancellationToken ct)
        {
            var list = await _db.Groups.AsNoTracking().ToListAsync(ct);
            return list.Select(x => new GroupDto { Id = x.Id, Title = x.Title }).ToList();
        }
    }
}
