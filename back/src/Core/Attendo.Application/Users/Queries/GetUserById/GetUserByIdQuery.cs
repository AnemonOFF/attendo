using MediatR;
using Attendo.Application.DTOs.Users;

namespace Attendo.Application.Users.Queries.GetUserById
{
    public class GetUserByIdQuery : IRequest<UserResponse?>
    {
        public int Id { get; set; }
    }
}
