using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Atendo.Application.Groups.Commands;
using Atendo.Application.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Atendo.Persistence.Groups.Handlers
{
    public class DeleteGroupHandler : IRequestHandler<DeleteGroupCommand, bool>
    {
        private readonly IAppDbContext _db;
        public DeleteGroupHandler(IAppDbContext db) => _db = db;

        public async Task<bool> Handle(DeleteGroupCommand request, CancellationToken ct)
        {
            var entity = await _db.Groups.FirstOrDefaultAsync(x => x.Id == request.Id, ct);
            if (entity is null) return false;
            _db.Groups.Remove(entity);
            await _db.SaveChangesAsync(ct);
            return true;
        }
    }
}
