using MediatR;
using Attendo.Application.DTOs.Classes;

namespace Attendo.Application.Classes.Commands.UpdateClass
{
    public class UpdateClassCommand : IRequest<ClassDto?>
    {
        public int Id { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public int GroupId { get; set; }
    }
}
