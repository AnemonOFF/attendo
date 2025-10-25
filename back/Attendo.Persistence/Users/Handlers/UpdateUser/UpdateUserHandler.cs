using MediatR;
using Microsoft.EntityFrameworkCore;
using Attendo.Application.Users.Commands.UpdateUser;
using Attendo.Application.DTOs.Users;
using Attendo.Application.Interfaces;
using System.Security.Cryptography;
using System.Text;

namespace Attendo.Persistence.Users.Handlers.UpdateUser
{
    public class UpdateUserHandler : IRequestHandler<UpdateUserCommand, UserResponse?>
    {
        private readonly IAppDbContext _db;
        public UpdateUserHandler(IAppDbContext db) => _db = db;

        public async Task<UserResponse?> Handle(UpdateUserCommand request, CancellationToken ct)
        {
            var entity = await _db.Users.FirstOrDefaultAsync(u => u.Id == request.Id, ct);
            if (entity is null) return null;

            entity.Email = request.Email;
            entity.Role = request.Role;

            if (!string.IsNullOrWhiteSpace(request.NewPassword))
            {
                entity.PasswordHash = HashPassword(request.NewPassword);
            }

            await _db.SaveChangesAsync(ct);

            return new UserResponse
            {
                Id = entity.Id,
                Login = entity.Login,
                Email = entity.Email,
                Role = entity.Role,
                CreatedAt = entity.CreatedAt
            };
        }

        private static string HashPassword(string password)
        {
            using var sha = SHA256.Create();
            var bytes = sha.ComputeHash(Encoding.UTF8.GetBytes(password));
            return Convert.ToHexString(bytes);
        }
    }
}
