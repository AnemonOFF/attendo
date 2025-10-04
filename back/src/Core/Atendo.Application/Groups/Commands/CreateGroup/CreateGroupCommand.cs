using MediatR;
using Atendo.Application.DTOs;

namespace Atendo.Application.Groups.Commands
{
    public class CreateGroupCommand : IRequest<GroupDto>
    {
        public string Title { get; set; } = string.Empty;
    }
}
