using Attendo.Application.DTOs.Groups;
using Attendo.Application.DTOs.Students;
using Attendo.Application.Groups.Queries.GetGroupById;
using Attendo.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Attendo.Persistence.Groups.Handlers.GetGroupById
{
    public class GetGroupByIdHandler : IRequestHandler<GetGroupByIdQuery, GroupResponse?>
    {
        private readonly IAppDbContext _db;
        public GetGroupByIdHandler(IAppDbContext db) => _db = db;

        public async Task<GroupResponse?> Handle(GetGroupByIdQuery request, CancellationToken ct)
        {
            var entity = await _db.Groups
                .AsNoTracking()
                .Include(g => g.Students)
                .FirstOrDefaultAsync(g => g.Id == request.Id, ct);

            if (entity is null)
            {
                return null;
            }

            return new GroupResponse
            {
                Id = entity.Id,
                Title = entity.Title,
                Students = [.. entity.Students
                    .Select(s => new StudentDto
                    {
                        Id = s.Id,
                        FullName = s.FullName
                    })]
            };
        }
    }
}
