using Attendo.Application.DTOs.Classes;
using Attendo.Application.DTOs.Groups;
using MediatR;

namespace Attendo.Application.Classes.Commands.CreateClass
{
    public class CreateClassCommand : IRequest<ClassDto>
    {
        public DateTimeOffset Start { get; set; }
        public DateTimeOffset? End { get; set; }
        public List<GroupDto> Groups { get; set; } = new();

    }
}
