using Attendo.Application.DTOs;
using Attendo.Application.DTOs.Groups;
using MediatR;

namespace Attendo.Application.Groups.Commands
{
    public class CreateGroupCommand : IRequest<GroupDto>
    {
        public string Title { get; set; } = string.Empty;
    }
}
