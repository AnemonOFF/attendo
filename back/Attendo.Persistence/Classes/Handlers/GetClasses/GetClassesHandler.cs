// GetClassesHandler.cs
using Attendo.Application.Classes.Queries;
using Attendo.Application.DTOs.Classes;
using Attendo.Application.DTOs.Groups;
using Attendo.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

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
                .Include(c => c.Groups)
                .OrderBy(c => c.Start)
                .Select(c => new ClassResponse
                {
                    Id = c.Id,
                    Start = c.Start,
                    End = c.End,
                    Groups = c.Groups
                        .Select(g => new GroupDto { Id = g.Id, Title = g.Title })
                        .ToList(),
                    Attendance = new List<int>()
                })
                .ToListAsync(ct);

            return new ClassesListResponse { Items = items };
        }
    }
}
