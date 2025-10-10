using MediatR;
using Microsoft.EntityFrameworkCore;
using Attendo.Application.Users.Commands.CreateUser;
using Attendo.Application.DTOs.Users;
using Attendo.Application.Interfaces;
using Attendo.Domain.Entities;
using System.Security.Cryptography;
using System.Text;

namespace Attendo.Persistence.Users.Handlers.CreateUser
{
    public class CreateUserHandler : IRequestHandler<CreateUserCommand, UserResponse>
    {
        private readonly IAppDbContext _db;
        public CreateUserHandler(IAppDbContext db) => _db = db;

        public async Task<UserResponse> Handle(CreateUserCommand request, CancellationToken ct)
        {
            var exists = await _db.Users.AnyAsync(u => u.Login == request.Login || u.Email == request.Email, ct);
            if (exists) throw new InvalidOperationException("User with the same login or email already exists.");

            var entity = new User
            {
                Login = request.Login,
                Email = request.Email,
                Role = request.Role,
                PasswordHash = HashPassword(request.Password),
                CreatedAt = DateTimeOffset.UtcNow
            };

            _db.Users.Add(entity);
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
