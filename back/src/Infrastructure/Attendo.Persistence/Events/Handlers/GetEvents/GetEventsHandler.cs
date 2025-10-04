using MediatR;
using Microsoft.EntityFrameworkCore;
using Attendo.Application.DTOs;
using Attendo.Application.Events.Queries;
using Attendo.Application.Interfaces;

namespace Attendo.Persistence.Events.Handlers
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
