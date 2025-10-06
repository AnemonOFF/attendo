using MediatR;
using Microsoft.EntityFrameworkCore;
using Attendo.Application.Classes.Queries;
using Attendo.Application.DTOs.Classes;
using Attendo.Application.Interfaces;

namespace Attendo.Persistence.Classes.Handlers.GetClassById
{
    public class GetClassByIdHandler : IRequestHandler<GetClassByIdQuery, ClassDto?>
    {
        private readonly IAppDbContext _db;
        public GetClassByIdHandler(IAppDbContext db) => _db = db;

        public async Task<ClassDto?> Handle(GetClassByIdQuery request, CancellationToken ct)
        {
            var entity = await _db.Classes
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.Id == request.Id, ct);

            if (entity is null) return null;

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
