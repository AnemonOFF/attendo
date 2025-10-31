using Attendo.Application.Classes.Queries.GetClassById;
using Attendo.Application.DTOs.Classes;
using Attendo.Application.DTOs.Groups;
using Attendo.Application.DTOs.Students;
using Attendo.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Attendo.Persistence.Classes.Handlers.GetClassById
{
    public sealed class GetClassByIdHandler : IRequestHandler<GetClassByIdQuery, ClassResponse?>
    {
        private readonly IAppDbContext _db;
        public GetClassByIdHandler(IAppDbContext db) => _db = db;

        public async Task<ClassResponse?> Handle(GetClassByIdQuery request, CancellationToken ct)
        {
            var entity = await _db.Classes
                .AsNoTracking()
                .Include(c => c.Group)
                    .ThenInclude(g => g.Students)
                .FirstOrDefaultAsync(c => c.Id == request.Id, ct);

            if (entity is null)
            {
                return null;
            }

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
                    Id = entity.Group.Id,
                    Title = entity.Group.Title,
                    Students = [.. entity.Group.Students.Select(s => new StudentDto { Id = s.Id, FullName = s.FullName })]
                }
            };
        }
    }
}
