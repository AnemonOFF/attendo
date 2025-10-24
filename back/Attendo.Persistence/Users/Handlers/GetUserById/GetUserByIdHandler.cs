using MediatR;
using Microsoft.EntityFrameworkCore;
using Attendo.Application.Users.Queries.GetUserById;
using Attendo.Application.DTOs.Users;
using Attendo.Application.Interfaces;

namespace Attendo.Persistence.Users.Handlers.GetUserById
{
    public class GetUserByIdHandler : IRequestHandler<GetUserByIdQuery, UserResponse?>
    {
        private readonly IAppDbContext _db;
        public GetUserByIdHandler(IAppDbContext db) => _db = db;

        public async Task<UserResponse?> Handle(GetUserByIdQuery request, CancellationToken ct)
        {
            var entity = await _db.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Id == request.Id, ct);
            if (entity is null) return null;

            return new UserResponse
            {
                Id = entity.Id,
                Login = entity.Login,
                Email = entity.Email,
                Role = entity.Role,
                CreatedAt = entity.CreatedAt
            };
        }
    }
}
