using System.Threading;
using System.Threading.Tasks;
using Attendo.Application.Groups.Commands;
using Attendo.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Attendo.Persistence.Groups.Handlers
{
    public class DeleteGroupHandler : IRequestHandler<DeleteGroupCommand, bool>
    {
        private readonly IAppDbContext _db;
        public DeleteGroupHandler(IAppDbContext db) => _db = db;

        public async Task<bool> Handle(DeleteGroupCommand request, CancellationToken ct)
        {
            var entity = await _db.Groups.FirstOrDefaultAsync(x => x.Id == request.Id, ct);
            if (entity is null)
            {
                return false;
            }

            _db.Groups.Remove(entity);
            await _db.SaveChangesAsync(ct);
            return true;
        }
    }
}
