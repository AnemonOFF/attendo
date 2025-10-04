using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Attendo.Application.DTOs;
using Attendo.Application.Groups.Queries;
using Attendo.Application.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Attendo.Persistence.Groups.Handlers
{
    public class GetGroupByIdHandler : IRequestHandler<GetGroupByIdQuery, GroupDto?>
    {
        private readonly IAppDbContext _db;
        public GetGroupByIdHandler(IAppDbContext db) => _db = db;

        public async Task<GroupDto?> Handle(GetGroupByIdQuery request, CancellationToken ct)
        {
            var g = await _db.Groups.AsNoTracking().FirstOrDefaultAsync(x => x.Id == request.Id, ct);
            return g is null ? null : new GroupDto { Id = g.Id, Title = g.Title };
        }
    }
}
