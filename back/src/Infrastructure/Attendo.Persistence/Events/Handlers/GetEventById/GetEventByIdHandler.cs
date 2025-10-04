using MediatR;
using Microsoft.EntityFrameworkCore;
using Attendo.Application.DTOs;
using Attendo.Application.Events.Queries;
using Attendo.Application.Interfaces;

namespace Attendo.Persistence.Events.Handlers
{
    public class GetEventByIdHandler : IRequestHandler<GetEventByIdQuery, EventDto?>
    {
        private readonly IAppDbContext _db;
        public GetEventByIdHandler(IAppDbContext db) => _db = db;

        public async Task<EventDto?> Handle(GetEventByIdQuery request, CancellationToken ct)
        {
            var e = await _db.Events.AsNoTracking().FirstOrDefaultAsync(x => x.Id == request.Id, ct);
            return e is null ? null : new EventDto { Id = e.Id, Date = e.Date, Type = e.Type, GroupId = e.GroupId };
        }
    }
}
