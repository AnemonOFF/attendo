using Attendo.Application.Classes.Queries.GetClasses;
using Attendo.Application.DTOs.Classes;
using Attendo.Application.DTOs.Groups;
using Attendo.Application.DTOs.Students;
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
            var query = _db.Classes
                .AsNoTracking()
                .Include(c => c.Group)
                    .ThenInclude(g => g.Students)
                .AsQueryable();

            if (request.From.HasValue)
            {
                var fromDate = DateOnly.FromDateTime(request.From.Value);
                query = query.Where(c => c.Start >= fromDate);
            }

            if (request.To.HasValue)
            {
                var toDate = DateOnly.FromDateTime(request.To.Value);
                query = query.Where(c => c.Start <= toDate);
            }

            if (request.Group is { Count: > 0 })
            {
                query = query.Where(c => request.Group!.Contains(c.GroupId));
            }

            var items = await query
                .OrderBy(c => c.Start).ThenBy(c => c.StartTime)
                .Select(c => new ClassResponse
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
                        Students = c.Group.Students
                            .Select(s => new StudentDto
                            {
                                Id = s.Id,
                                FullName = s.FullName
                            })
                            .ToList()
                    }
                })
                .ToListAsync(ct);

            return new ClassesListResponse { Items = items };
        }
    }
}
