using MediatR;
using Microsoft.EntityFrameworkCore;
using Attendo.Application.DTOs;
using Attendo.Application.Events.Commands.CreateEvent;
using Attendo.Application.Interfaces;
using Attendo.Domain.Entities;

namespace Attendo.Persistence.Events.Handlers
{
    public class CreateEventHandler : IRequestHandler<CreateEventCommand, EventDto>
    {
        private readonly IAppDbContext _db;
        public CreateEventHandler(IAppDbContext db) => _db = db;

        public async Task<EventDto> Handle(CreateEventCommand request, CancellationToken ct)
        {
            var groupExists = await _db.Groups.AnyAsync(g => g.Id == request.GroupId, ct);
            if (!groupExists) throw new KeyNotFoundException($"Group {request.GroupId} not found");

            var entity = new Event { Date = request.Date, Type = request.Type, GroupId = request.GroupId };
            _db.Events.Add(entity);
            await _db.SaveChangesAsync(ct);

            return new EventDto { Id = entity.Id, Date = entity.Date, Type = entity.Type, GroupId = entity.GroupId };
        }
    }
}
