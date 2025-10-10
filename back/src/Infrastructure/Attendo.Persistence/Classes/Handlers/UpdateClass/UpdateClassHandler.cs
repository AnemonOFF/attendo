using MediatR;
using Microsoft.EntityFrameworkCore;
using Attendo.Application.Classes.Commands.UpdateClass;
using Attendo.Application.DTOs.Classes;
using Attendo.Application.DTOs.Groups;
using Attendo.Application.Interfaces;

namespace Attendo.Persistence.Classes.Handlers.UpdateClass
{
    public class UpdateClassHandler : IRequestHandler<UpdateClassCommand, ClassDto?>
    {
        private readonly IAppDbContext _db;
        public UpdateClassHandler(IAppDbContext db) => _db = db;

        public async Task<ClassDto?> Handle(UpdateClassCommand request, CancellationToken ct)
        {
            var entity = await _db.Classes
                .Include(c => c.Groups)
                .FirstOrDefaultAsync(c => c.Id == request.Id, ct);

            if (entity is null) return null;

            entity.Start = request.Start;
            entity.End = request.End;

            var requestedGroupIds = request.Groups?.Select(g => g.Id).Distinct().ToList() ?? new List<int>();
            var groups = requestedGroupIds.Count == 0
                ? new List<Domain.Entities.Group>()
                : await _db.Groups.Where(g => requestedGroupIds.Contains(g.Id)).ToListAsync(ct);

            if (groups.Count != requestedGroupIds.Count)
            {
                var found = groups.Select(g => g.Id).ToHashSet();
                var missing = requestedGroupIds.Where(id => !found.Contains(id));
                throw new KeyNotFoundException($"Groups not found: {string.Join(", ", missing)}");
            }

            entity.Groups.Clear();
            foreach (var g in groups) entity.Groups.Add(g);

            await _db.SaveChangesAsync(ct);

            return new ClassDto
            {
                Id = entity.Id,
                Start = entity.Start,
                End = entity.End,
                Groups = entity.Groups.Select(g => new GroupDto { Id = g.Id, Title = g.Title }).ToList()
            };
        }
    }
}
