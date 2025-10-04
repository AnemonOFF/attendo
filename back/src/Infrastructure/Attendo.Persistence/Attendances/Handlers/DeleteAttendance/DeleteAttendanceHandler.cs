using MediatR;
using Microsoft.EntityFrameworkCore;
using Attendo.Application.Attendances.Commands.DeleteAttendance;
using Attendo.Application.Interfaces;

namespace Attendo.Persistence.Attendances.Handlers
{
    public class DeleteAttendanceHandler : IRequestHandler<DeleteAttendanceCommand, bool>
    {
        private readonly IAppDbContext _db;
        public DeleteAttendanceHandler(IAppDbContext db) => _db = db;

        public async Task<bool> Handle(DeleteAttendanceCommand request, CancellationToken ct)
        {
            var entity = await _db.Attendances.FirstOrDefaultAsync(x => x.Id == request.Id, ct);
            if (entity is null) return false;
            _db.Attendances.Remove(entity);
            await _db.SaveChangesAsync(ct);
            return true;
        }
    }
}
