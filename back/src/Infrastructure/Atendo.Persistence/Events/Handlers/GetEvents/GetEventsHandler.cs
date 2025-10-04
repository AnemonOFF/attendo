using MediatR;
using Microsoft.EntityFrameworkCore;
using Atendo.Application.DTOs;
using Atendo.Application.Events.Queries;
using Atendo.Application.Interfaces;

namespace Atendo.Persistence.Events.Handlers
{
    public class GetEventsHandler : IRequestHandler<GetEventsQuery, IReadOnlyList<EventDto>>
    {
        private readonly IAppDbContext _db;
        public GetEventsHandler(IAppDbContext db) => _db = db;

        public async Task<IReadOnlyList<EventDto>> Handle(GetEventsQuery request, CancellationToken ct)
        {
            var list = await _db.Events.AsNoTracking().ToListAsync(ct);
            return list.Select(e => new EventDto { Id = e.Id, Date = e.Date, Type = e.Type, GroupId = e.GroupId }).ToList();
        }
    }
}
