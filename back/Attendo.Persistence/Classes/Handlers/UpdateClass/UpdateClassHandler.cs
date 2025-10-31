using Attendo.Application.Classes.Commands.UpdateClass;
using Attendo.Application.DTOs.Classes;
using Attendo.Application.DTOs.Groups;
using Attendo.Application.DTOs.Students;
using Attendo.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Attendo.Persistence.Classes.Handlers.UpdateClass;

public sealed class UpdateClassHandler : IRequestHandler<UpdateClassCommand, ClassResponse>
{
    private readonly IAppDbContext _db;
    public UpdateClassHandler(IAppDbContext db) => _db = db;

    public async Task<ClassResponse> Handle(UpdateClassCommand request, CancellationToken ct)
    {
        var entity = await _db.Classes
            .FirstOrDefaultAsync(c => c.Id == request.Id, ct)
            ?? throw new KeyNotFoundException($"Class {request.Id} not found");

        if (request.Name is not null)
        {
            entity.Name = request.Name;
        }

        if (request.Start is not null)
        {
            entity.Start = request.Start.Value;
        }

        if (request.End is not null)
        {
            entity.End = request.End.Value;
        }

        if (request.Frequency is not null)
        {
            entity.Frequency = request.Frequency;
        }

        if (request.StartTime is not null)
        {
            entity.StartTime = request.StartTime.Value;
        }

        if (request.EndTime is not null)
        {
            entity.EndTime = request.EndTime.Value;
        }

        if (request.GroupId is not null)
        {
            var exists = await _db.Groups.AnyAsync(g => g.Id == request.GroupId.Value, ct);
            if (!exists)
            {
                throw new KeyNotFoundException($"Group {request.GroupId.Value} not found");
            }

            entity.GroupId = request.GroupId.Value;
        }

        await _db.SaveChangesAsync(ct);

        var fresh = await _db.Classes
            .AsNoTracking()
            .Include(c => c.Group)
                .ThenInclude(g => g.Students)
            .FirstAsync(c => c.Id == entity.Id, ct);

        return Map(fresh);
    }

    private static ClassResponse Map(Domain.Entities.Class c) =>
        new ClassResponse
        {
            Id = c.Id,
            Name = c.Name,
            Start = c.Start,
            End = c.End,
            Frequency = c.Frequency,
            StartTime = c.StartTime,
            EndTime = c.EndTime,
            Group = new GroupResponse
            {
                Id = c.Group.Id,
                Title = c.Group.Title,
                Students = [.. c.Group.Students.Select(s => new StudentDto { Id = s.Id, FullName = s.FullName })]
            }
        };
}
