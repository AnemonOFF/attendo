using MediatR;
using Attendo.Application.DTOs;
using Attendo.Application.DTOs.Groups;

namespace Attendo.Application.Groups.Commands
{
    public class CreateGroupCommand : IRequest<GroupDto>
    {
        public string Title { get; set; } = string.Empty; 
    }
}
