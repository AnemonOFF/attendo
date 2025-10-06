namespace Attendo.Application.DTOs.Users
{
    public class CreateUserRequest
    {
        public string Login { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Role { get; set; } = "User";
        public string Password { get; set; } = string.Empty; 
    }
}
