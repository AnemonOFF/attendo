using MediatR;
using Attendo.Application.DTOs.Groups;

namespace Attendo.Application.Groups.Commands
{
    public class UpdateGroupCommand : IRequest<GroupDto>
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public List<int>? Students { get; set; }
    }
}
