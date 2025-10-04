using MediatR;
using Attendo.Application.DTOs;

namespace Attendo.Application.Groups.Commands
{
    public class UpdateGroupCommand : IRequest<GroupDto>
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
    }
}
