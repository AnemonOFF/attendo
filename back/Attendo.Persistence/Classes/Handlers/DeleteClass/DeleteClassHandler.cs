using Attendo.Application.Classes.Commands.DeleteClass;
using Attendo.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Attendo.Persistence.Classes.Handlers
{
    public class DeleteClassHandler : IRequestHandler<DeleteClassCommand, bool>
    {
        private readonly IAppDbContext _db;
        public DeleteClassHandler(IAppDbContext db) => _db = db;

        public async Task<bool> Handle(DeleteClassCommand request, CancellationToken ct)
        {
            var entity = await _db.Classes.FirstOrDefaultAsync(x => x.Id == request.Id, ct);
            if (entity is null)
            {
                return false;
            }

            _db.Classes.Remove(entity);
            await _db.SaveChangesAsync(ct);
            return true;
        }
    }
}
