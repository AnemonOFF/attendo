using Attendo.Application.DTOs.Groups;
using Attendo.Application.DTOs.Students;
using Attendo.Application.Groups.Queries.GetGroups;
using Attendo.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Attendo.Persistence.Groups.Handlers.GetGroups;

public class GetGroupsHandler : IRequestHandler<GetGroupsQuery, GroupsListResponse>
{
    private readonly IAppDbContext _db;
    public GetGroupsHandler(IAppDbContext db) => _db = db;

    public async Task<GroupsListResponse> Handle(GetGroupsQuery request, CancellationToken ct)
    {
        var query = _db.Groups
            .Include(g => g.Students)
            .AsNoTracking();

        var total = await query.CountAsync(ct);

        var items = await query
            .OrderBy(g => g.Id)
            .Skip(request.Offset ?? 0)
            .Take(request.Limit ?? total)
            .Select(group => new GroupResponse
            {
                Id = group.Id,
                Title = group.Title,
                Students = group.Students
                    .Select(s => new StudentDto { Id = s.Id, FullName = s.FullName })
                    .ToList()
            })
            .ToListAsync(ct);

        return new GroupsListResponse
        {
            Total = total,
            Items = items
        };
    }
}
