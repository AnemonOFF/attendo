using MediatR;
using Attendo.Application.DTOs.Classes;

namespace Attendo.Application.Classes.Commands.CreateClass
{
    public class CreateClassCommand : IRequest<ClassDto>
    {
        public DateTimeOffset Start { get; set; }
        public DateTimeOffset? End { get; set; }
        public int GroupId { get; set; }
    }
}
