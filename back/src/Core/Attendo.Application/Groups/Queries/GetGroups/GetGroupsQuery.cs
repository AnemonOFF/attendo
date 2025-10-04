using MediatR;
using Attendo.Application.DTOs;

namespace Attendo.Application.Groups.Queries
{
    public class GetGroupsQuery : IRequest<IReadOnlyList<GroupDto>> {}
}
