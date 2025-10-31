using Attendo.Application.DTOs.Groups;
using MediatR;

namespace Attendo.Application.Groups.Commands.UpdateGroup;

public sealed class UpdateGroupCommand : IRequest<GroupResponse>
{
    public int Id { get; set; }
    public string? Title { get; set; }
    public List<int>? Students { get; set; }
}
