using MediatR;
using Microsoft.EntityFrameworkCore;
using Attendo.Application.Users.Commands.DeleteUser;
using Attendo.Application.Interfaces;

namespace Attendo.Persistence.Users.Handlers.DeleteUser
{
    public class DeleteUserHandler : IRequestHandler<DeleteUserCommand, bool>
    {
        private readonly IAppDbContext _db;
        public DeleteUserHandler(IAppDbContext db) => _db = db;

        public async Task<bool> Handle(DeleteUserCommand request, CancellationToken ct)
        {
            var entity = await _db.Users.FirstOrDefaultAsync(u => u.Id == request.Id, ct);
            if (entity is null) return false;

            _db.Users.Remove(entity);
            await _db.SaveChangesAsync(ct);
            return true;
        }
    }
}
