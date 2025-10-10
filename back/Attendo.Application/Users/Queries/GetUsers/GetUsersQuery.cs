using MediatR;
using Attendo.Application.DTOs.Users;

namespace Attendo.Application.Users.Queries.GetUsers
{
    public class GetUsersQuery : IRequest<UsersListResponse>
    {
    }
}
