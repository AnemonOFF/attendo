using Attendo.Application.DTOs.Groups;
using MediatR;

namespace Attendo.Application.Groups.Queries.GetGroups;

public class GetGroupsQuery : IRequest<GroupsListResponse>
{
    public int? Offset { get; set; }
    public int? Limit { get; set; }
}
