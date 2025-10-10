namespace Attendo.Application.DTOs.Users
{
    public class UpdateUserRequest
    {
        public string Email { get; set; } = string.Empty;
        public string Role { get; set; } = "User";
        public string? NewPassword { get; set; }
    }
}
