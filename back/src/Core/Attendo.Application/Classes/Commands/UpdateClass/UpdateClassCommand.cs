using MediatR;
using Attendo.Application.DTOs;

namespace Attendo.Application.Classes.Commands.UpdateClass
{
    public class UpdateClassCommand : IRequest<ClassDto>
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public string Type { get; set; } = string.Empty;
        public int GroupId { get; set; }
    }
}
