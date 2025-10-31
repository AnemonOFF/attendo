using Attendo.Application.Groups.Commands.UpdateGroup;
using Attendo.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using GDTO = Attendo.Application.DTOs.Groups;
using SDTO = Attendo.Application.DTOs.Students;

namespace Attendo.Persistence.Groups.Handlers.UpdateGroup;

public sealed class UpdateGroupHandler : IRequestHandler<UpdateGroupCommand, GDTO.GroupResponse>
{
    private readonly IAppDbContext _db;
    public UpdateGroupHandler(IAppDbContext db) => _db = db;

    public async Task<GDTO.GroupResponse> Handle(UpdateGroupCommand request, CancellationToken ct)
    {
        var group = await _db.Groups
            .Include(g => g.Students)
            .FirstOrDefaultAsync(g => g.Id == request.Id, ct)
            ?? throw new KeyNotFoundException($"Group {request.Id} not found");

        if (request.Title is not null)
        {
            group.Title = request.Title;
        }

        if (request.Students is not null)
        {
            var ids = request.Students.Distinct().ToList();
            var students = ids.Count == 0
                ? new List<Domain.Entities.Student>()
                : await _db.Students.Where(s => ids.Contains(s.Id)).ToListAsync(ct);

            if (students.Count != ids.Count)
            {
                var found = students.Select(s => s.Id).ToHashSet();
                var missing = ids.Where(id => !found.Contains(id));
                throw new KeyNotFoundException($"Students not found: {string.Join(", ", missing)}");
            }

            group.Students.Clear();
            foreach (var s in students)
            {
                group.Students.Add(s);
            }
        }

        await _db.SaveChangesAsync(ct);

        return new GDTO.GroupResponse
        {
            Id = group.Id,
            Title = group.Title,
            Students = [.. group.Students.Select(s => new SDTO.StudentDto { Id = s.Id, FullName = s.FullName })]
        };
    }
}
