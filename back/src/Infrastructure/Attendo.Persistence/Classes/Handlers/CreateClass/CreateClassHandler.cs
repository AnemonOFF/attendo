using MediatR;
using Microsoft.EntityFrameworkCore;
using Attendo.Application.DTOs;
using Attendo.Application.Classes.Commands.CreateClass;
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
            var groupExists = await _db.Groups.AnyAsync(g => g.Id == request.GroupId, ct);
            if (!groupExists) throw new KeyNotFoundException($"Group {request.GroupId} not found");

            var entity = new Class { Date = request.Date, Type = request.Type, GroupId = request.GroupId };
            _db.Classes.Add(entity);
            await _db.SaveChangesAsync(ct);

            return new ClassDto { Id = entity.Id, Date = entity.Date, Type = entity.Type, GroupId = entity.GroupId };
        }
    }
}
