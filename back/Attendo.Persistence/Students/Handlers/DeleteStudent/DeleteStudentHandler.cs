using MediatR;
using Microsoft.EntityFrameworkCore;
using Attendo.Application.Students.Commands;
using Attendo.Application.Interfaces;

namespace Attendo.Persistence.Students.Handlers
{
    public class DeleteStudentHandler : IRequestHandler<DeleteStudentCommand, bool>
    {
        private readonly IAppDbContext _db;
        public DeleteStudentHandler(IAppDbContext db) => _db = db;

        public async Task<bool> Handle(DeleteStudentCommand request, CancellationToken ct)
        {
            var entity = await _db.Students.FirstOrDefaultAsync(s => s.Id == request.Id, ct);
            if (entity is null) return false;

            _db.Students.Remove(entity);
            await _db.SaveChangesAsync(ct);
            return true;
        }
    }
}
