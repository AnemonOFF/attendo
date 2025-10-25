using System.Threading;
using System.Threading.Tasks;
using Attendo.Application.DTOs;
using Attendo.Application.DTOs.Groups;
using Attendo.Application.Groups.Commands;
using Attendo.Application.Interfaces;
using Attendo.Domain.Entities;
using MediatR;

namespace Attendo.Persistence.Groups.Handlers
{
    public class CreateGroupHandler : IRequestHandler<CreateGroupCommand, GroupDto>
    {
        private readonly IAppDbContext _db;
        public CreateGroupHandler(IAppDbContext db) => _db = db;

        public async Task<GroupDto> Handle(CreateGroupCommand request, CancellationToken ct)
        {
            var entity = new Group { Title = request.Title };
            _db.Groups.Add(entity);
            await _db.SaveChangesAsync(ct);
            return new GroupDto { Id = entity.Id, Title = entity.Title };
        }
    }
}
