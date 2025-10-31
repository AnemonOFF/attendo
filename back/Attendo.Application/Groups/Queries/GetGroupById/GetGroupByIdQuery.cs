using Attendo.Application.DTOs.Groups;
using MediatR;

namespace Attendo.Application.Groups.Queries.GetGroupById;

public sealed class GetGroupByIdQuery : IRequest<GroupResponse?>
{
    public int Id { get; set; }
}
