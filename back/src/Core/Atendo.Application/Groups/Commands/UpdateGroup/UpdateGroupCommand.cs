using MediatR;
using Atendo.Application.DTOs;

namespace Atendo.Application.Groups.Commands
{
    public class UpdateGroupCommand : IRequest<GroupDto>
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
    }
}
