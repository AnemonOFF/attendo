using Attendo.Application.Classes.Commands.CreateClass;
using Attendo.Application.DTOs.Classes;
using Attendo.Application.DTOs.Groups;
using Attendo.Application.Interfaces;
using Attendo.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Attendo.Persistence.Classes.Handlers
{
    public class CreateClassHandler : IRequestHandler<CreateClassCommand, ClassDto>
    {
        private readonly IAppDbContext _db;
        public CreateClassHandler(IAppDbContext db) => _db = db;

        public async Task<ClassDto> Handle(CreateClassCommand request, CancellationToken ct)
        {
            if (request.End.HasValue && request.End.Value < request.Start)
            {
                throw new ArgumentException("End date must be greater than or equal to Start date.", nameof(request.End));
            }

            var requestedGroupIds = request.Groups?.Select(g => g.Id).Distinct().ToList() ?? new List<int>();
            var groups = requestedGroupIds.Count == 0
                ? new List<Group>()
                : await _db.Groups.Where(g => requestedGroupIds.Contains(g.Id)).ToListAsync(ct);

            if (groups.Count != requestedGroupIds.Count)
            {
                var found = groups.Select(g => g.Id).ToHashSet();
                var missing = requestedGroupIds.Where(id => !found.Contains(id));
                throw new KeyNotFoundException($"Groups not found: {string.Join(", ", missing)}");
            }

            var entity = new Class
            {
                Start = request.Start,
                End = request.End,
                Groups = groups
            };

            _db.Classes.Add(entity);
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
