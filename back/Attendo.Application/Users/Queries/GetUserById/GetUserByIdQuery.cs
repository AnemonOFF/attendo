using Attendo.Application.DTOs.Users;
using MediatR;

namespace Attendo.Application.Users.Queries.GetUserById
{
    public class GetUserByIdQuery : IRequest<UserResponse?>
    {
        public int Id { get; set; }
    }
}
