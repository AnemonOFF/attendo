using MediatR;
using Microsoft.EntityFrameworkCore;
using Attendo.Application.Classes.Commands.UpdateClass;
using Attendo.Application.DTOs.Classes;
using Attendo.Application.Interfaces;

namespace Attendo.Persistence.Classes.Handlers.UpdateClass
{
    public class UpdateClassHandler : IRequestHandler<UpdateClassCommand, ClassDto?>
    {
        private readonly IAppDbContext _db;
        public UpdateClassHandler(IAppDbContext db) => _db = db;

        public async Task<ClassDto?> Handle(UpdateClassCommand request, CancellationToken ct)
        {
            var entity = await _db.Classes.FirstOrDefaultAsync(c => c.Id == request.Id, ct);
            if (entity is null)
                return null;

            var groupExists = await _db.Groups.AnyAsync(g => g.Id == request.GroupId, ct);
            if (!groupExists)
                throw new KeyNotFoundException($"Group {request.GroupId} not found");

            entity.Start   = request.Start;
            entity.End     = request.End;
            entity.GroupId = request.GroupId;

            await _db.SaveChangesAsync(ct);

            return new ClassDto
            {
                Id      = entity.Id,
                Start   = entity.Start,
                End     = entity.End,
                GroupId = entity.GroupId
            };
        }
    }
}
