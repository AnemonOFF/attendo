using MediatR;
using Attendo.Application.DTOs;

namespace Attendo.Application.Groups.Commands
{
    public class CreateGroupCommand : IRequest<GroupDto>
    {
        public string Title { get; set; } = string.Empty;
    }
}
