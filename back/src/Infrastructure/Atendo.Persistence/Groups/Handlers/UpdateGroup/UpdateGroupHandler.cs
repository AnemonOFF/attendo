using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Atendo.Application.DTOs;
using Atendo.Application.Groups.Commands;
using Atendo.Application.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Atendo.Persistence.Groups.Handlers
{
    public class UpdateGroupHandler : IRequestHandler<UpdateGroupCommand, GroupDto>
    {
        private readonly IAppDbContext _db;
        public UpdateGroupHandler(IAppDbContext db) => _db = db;

        public async Task<GroupDto> Handle(UpdateGroupCommand request, CancellationToken ct)
        {
            var entity = await _db.Groups.FirstOrDefaultAsync(x => x.Id == request.Id, ct)
                         ?? throw new KeyNotFoundException($"Group {request.Id} not found");
            entity.Title = request.Title;
            await _db.SaveChangesAsync(ct);
            return new GroupDto { Id = entity.Id, Title = entity.Title };
        }
    }
}
