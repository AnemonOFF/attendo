using MediatR;
using Microsoft.EntityFrameworkCore;
using Attendo.Application.Classes.Queries;
using Attendo.Application.DTOs.Classes;
using Attendo.Application.Interfaces;

namespace Attendo.Persistence.Classes.Handlers.GetClasses
{
    public class GetClassesHandler : IRequestHandler<GetClassesQuery, ClassesListResponse>
    {
        private readonly IAppDbContext _db;
        public GetClassesHandler(IAppDbContext db) => _db = db;

        public async Task<ClassesListResponse> Handle(GetClassesQuery request, CancellationToken ct)
        {
            var items = await _db.Classes
                .AsNoTracking()
                .OrderBy(c => c.Start)
                .Select(c => new ClassResponse
                {
                    Id = c.Id,
                    Start = c.Start,
                    End = c.End,
                    GroupId = c.GroupId
                })
                .ToListAsync(ct);

            return new ClassesListResponse { Items = items };
        }
    }
}
