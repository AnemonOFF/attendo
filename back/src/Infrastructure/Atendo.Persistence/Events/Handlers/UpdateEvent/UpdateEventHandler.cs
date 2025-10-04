using MediatR;
using Microsoft.EntityFrameworkCore;
using Atendo.Application.DTOs;
using Atendo.Application.Events.Commands.UpdateEvent;
using Atendo.Application.Interfaces;

namespace Atendo.Persistence.Events.Handlers
{
    public class UpdateEventHandler : IRequestHandler<UpdateEventCommand, EventDto>
    {
        private readonly IAppDbContext _db;
        public UpdateEventHandler(IAppDbContext db) => _db = db;

        public async Task<EventDto> Handle(UpdateEventCommand request, CancellationToken ct)
        {
            var entity = await _db.Events.FirstOrDefaultAsync(x => x.Id == request.Id, ct)
                         ?? throw new KeyNotFoundException($"Event {request.Id} not found");

            entity.Date = request.Date;
            entity.Type = request.Type;
            entity.GroupId = request.GroupId;
            await _db.SaveChangesAsync(ct);

            return new EventDto { Id = entity.Id, Date = entity.Date, Type = entity.Type, GroupId = entity.GroupId };
        }
    }
}
