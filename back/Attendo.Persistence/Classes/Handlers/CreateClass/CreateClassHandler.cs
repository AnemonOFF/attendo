using Attendo.Application.Classes.Commands.CreateClass;
using Attendo.Application.DTOs.Classes;
using Attendo.Application.DTOs.Groups;
using Attendo.Application.DTOs.Students;
using Attendo.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Attendo.Persistence.Classes.Handlers.CreateClass;

public sealed class CreateClassHandler : IRequestHandler<CreateClassCommand, ClassResponse>
{
    private readonly IAppDbContext _db;

    public CreateClassHandler(IAppDbContext db) => _db = db;

    public async Task<ClassResponse> Handle(CreateClassCommand request, CancellationToken ct)
    {
        var group = await _db.Groups
            .Include(g => g.Students)
            .AsNoTracking()
            .FirstOrDefaultAsync(g => g.Id == request.GroupId, ct) ?? throw new KeyNotFoundException($"Group {request.GroupId} not found");
        var entity = new Domain.Entities.Class
        {
            Name = request.Name,
            Start = request.Start,
            End = request.End,
            Frequency = request.Frequency,
            StartTime = request.StartTime,
            EndTime = request.EndTime,
            GroupId = request.GroupId
        };

        _db.Classes.Add(entity);
        await _db.SaveChangesAsync(ct);

        return new ClassResponse
        {
            Id = entity.Id,
            Name = entity.Name,
            Start = entity.Start,
            End = entity.End,
            Frequency = entity.Frequency,
            StartTime = entity.StartTime,
            EndTime = entity.EndTime,
            Group = new GroupResponse
            {
                Id = group.Id,
                Title = group.Title,
                Students = [.. group.Students
                    .Select(s => new StudentDto
                    {
                        Id = s.Id,
                        FullName = s.FullName
                    })]
            }
        };
    }
}
