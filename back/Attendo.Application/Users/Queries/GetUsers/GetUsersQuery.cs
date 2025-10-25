using Attendo.Application.DTOs.Users;
using MediatR;

namespace Attendo.Application.Users.Queries.GetUsers
{
    public class GetUsersQuery : IRequest<UsersListResponse>
    {
    }
}
