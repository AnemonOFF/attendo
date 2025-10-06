using MediatR;
using Microsoft.EntityFrameworkCore;
using Attendo.Application.Classes.Commands.CreateClass;
using Attendo.Application.DTOs.Classes;
using Attendo.Application.Interfaces;
using Attendo.Domain.Entities;

namespace Attendo.Persistence.Classes.Handlers
{
    public class CreateClassHandler : IRequestHandler<CreateClassCommand, ClassDto>
    {
        private readonly IAppDbContext _db;
        public CreateClassHandler(IAppDbContext db) => _db = db;

        public async Task<ClassDto> Handle(CreateClassCommand request, CancellationToken ct)
        {
            if (request.End.HasValue && request.End.Value < request.Start)
                throw new ArgumentException("End date must be greater than or equal to Start date.", nameof(request.End));

            var groupExists = await _db.Groups.AnyAsync(g => g.Id == request.GroupId, ct);
            if (!groupExists)
                throw new KeyNotFoundException($"Group {request.GroupId} not found");

            var entity = new Class
            {
                Start   = request.Start,
                End     = request.End,
                GroupId = request.GroupId
            };

            _db.Classes.Add(entity);
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
