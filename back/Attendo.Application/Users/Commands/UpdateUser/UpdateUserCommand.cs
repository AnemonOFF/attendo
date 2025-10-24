using MediatR;
using Attendo.Application.DTOs.Users;

namespace Attendo.Application.Users.Commands.UpdateUser
{
    public class UpdateUserCommand : IRequest<UserResponse?>
    {
        public int Id { get; set; }
        public string Email { get; set; } = string.Empty;
        public string Role { get; set; } = "User";
        public string? NewPassword { get; set; }
    }
}
