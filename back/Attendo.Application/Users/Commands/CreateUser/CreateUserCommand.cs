using MediatR;
using Attendo.Application.DTOs.Users;

namespace Attendo.Application.Users.Commands.CreateUser
{
    public class CreateUserCommand : IRequest<UserResponse>
    {
        public string Login { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Role { get; set; } = "User";
        public string Password { get; set; } = string.Empty;
    }
}
