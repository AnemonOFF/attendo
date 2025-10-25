using MediatR;
using Microsoft.EntityFrameworkCore;
using Attendo.Application.Users.Queries.GetUsers;
using Attendo.Application.DTOs.Users;
using Attendo.Application.Interfaces;

namespace Attendo.Persistence.Users.Handlers.GetUsers
{
    public class GetUsersHandler : IRequestHandler<GetUsersQuery, UsersListResponse>
    {
        private readonly IAppDbContext _db;
        public GetUsersHandler(IAppDbContext db) => _db = db;

        public async Task<UsersListResponse> Handle(GetUsersQuery request, CancellationToken ct)
        {
            var items = await _db.Users.AsNoTracking()
                .OrderBy(u => u.Id)
                .Select(u => new UserResponse
                {
                    Id = u.Id,
                    Login = u.Login,
                    Email = u.Email,
                    Role = u.Role,
                    CreatedAt = u.CreatedAt
                })
                .ToListAsync(ct);

            return new UsersListResponse { Items = items };
        }
    }
}
