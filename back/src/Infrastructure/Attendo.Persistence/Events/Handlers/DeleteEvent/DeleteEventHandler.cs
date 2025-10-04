using MediatR;
using Microsoft.EntityFrameworkCore;
using Attendo.Application.Events.Commands.DeleteEvent;
using Attendo.Application.Interfaces;

namespace Attendo.Persistence.Events.Handlers
{
    public class DeleteEventHandler : IRequestHandler<DeleteEventCommand, bool>
    {
        private readonly IAppDbContext _db;
        public DeleteEventHandler(IAppDbContext db) => _db = db;

        public async Task<bool> Handle(DeleteEventCommand request, CancellationToken ct)
        {
            var entity = await _db.Events.FirstOrDefaultAsync(x => x.Id == request.Id, ct);
            if (entity is null) return false;
            _db.Events.Remove(entity);
            await _db.SaveChangesAsync(ct);
            return true;
        }
    }
}
