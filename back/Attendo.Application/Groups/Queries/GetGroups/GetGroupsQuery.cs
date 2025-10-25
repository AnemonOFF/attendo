using Attendo.Application.DTOs;
using Attendo.Application.DTOs.Groups;
using MediatR;

namespace Attendo.Application.Groups.Queries
{
    public class GetGroupsQuery : IRequest<IReadOnlyList<GroupDto>>
    {
        public int? Offset { get; set; }
        public int? Limit { get; set; }
    }
}
