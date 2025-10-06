using MediatR;
using Attendo.Application.DTOs;
using Attendo.Application.DTOs.Groups;

namespace Attendo.Application.Groups.Queries
{
    public class GetGroupsQuery : IRequest<IReadOnlyList<GroupDto>> {}
}
