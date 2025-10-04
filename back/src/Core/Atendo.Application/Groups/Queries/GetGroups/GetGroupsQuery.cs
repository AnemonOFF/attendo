using MediatR;
using Atendo.Application.DTOs;

namespace Atendo.Application.Groups.Queries
{
    public class GetGroupsQuery : IRequest<IReadOnlyList<GroupDto>> {}
}
