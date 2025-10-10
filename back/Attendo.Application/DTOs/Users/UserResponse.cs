namespace Attendo.Application.DTOs.Users
{
    public class UserResponse
    {
        public int Id { get; set; }
        public string Login { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Role { get; set; } = "User";
        public DateTimeOffset CreatedAt { get; set; }
    }
}
