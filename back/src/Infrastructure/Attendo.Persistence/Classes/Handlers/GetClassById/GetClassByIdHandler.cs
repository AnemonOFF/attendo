using MediatR;
using Microsoft.EntityFrameworkCore;
using Attendo.Application.DTOs;
using Attendo.Application.Classes.Queries;
using Attendo.Application.Interfaces;

namespace Attendo.Persistence.Classes.Handlers
{
    public class GetClassByIdHandler : IRequestHandler<GetClassByIdQuery, ClassDto?>
    {
        private readonly IAppDbContext _db;
        public GetClassByIdHandler(IAppDbContext db) => _db = db;

        public async Task<ClassDto?> Handle(GetClassByIdQuery request, CancellationToken ct)
        {
            var e = await _db.Classes.AsNoTracking().FirstOrDefaultAsync(x => x.Id == request.Id, ct);
            return e is null ? null : new ClassDto { Id = e.Id, Date = e.Date, Type = e.Type, GroupId = e.GroupId };
        }
    }
}
