using MediatR;
using Microsoft.EntityFrameworkCore;
using Attendo.Application.DTOs;
using Attendo.Application.Classes.Commands.UpdateClass;
using Attendo.Application.Interfaces;

namespace Attendo.Persistence.Classes.Handlers
{
    public class UpdateClassHandler : IRequestHandler<UpdateClassCommand, ClassDto>
    {
        private readonly IAppDbContext _db;
        public UpdateClassHandler(IAppDbContext db) => _db = db;

        public async Task<ClassDto> Handle(UpdateClassCommand request, CancellationToken ct)
        {
            var entity = await _db.Classes.FirstOrDefaultAsync(x => x.Id == request.Id, ct)
                         ?? throw new KeyNotFoundException($"Event {request.Id} not found");

            entity.Date = request.Date;
            entity.Type = request.Type;
            entity.GroupId = request.GroupId;
            await _db.SaveChangesAsync(ct);

            return new ClassDto { Id = entity.Id, Date = entity.Date, Type = entity.Type, GroupId = entity.GroupId };
        }
    }
}
