using System.Collections.Generic;

namespace Attendo.Application.DTOs.Users
{
    public class UsersListResponse
    {
        public IList<UserResponse> Items { get; set; } = new List<UserResponse>();
    }
}
