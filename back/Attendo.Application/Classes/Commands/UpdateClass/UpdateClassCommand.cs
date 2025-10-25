using Attendo.Application.DTOs.Classes;
using Attendo.Application.DTOs.Groups;
using MediatR;

namespace Attendo.Application.Classes.Commands.UpdateClass
{
    public class UpdateClassCommand : IRequest<ClassDto?>
    {
        public int Id { get; set; }
        public DateTimeOffset Start { get; set; }
        public DateTimeOffset? End { get; set; }

        public List<GroupDto> Groups { get; set; } = new();
    }
}
