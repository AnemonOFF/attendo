using MediatR;
using Attendo.Application.DTOs;

namespace Attendo.Application.Classes.Commands.CreateClass
{
    public class CreateClassCommand : IRequest<ClassDto>
    {
        public DateTime Date { get; set; }
        public string Type { get; set; } = string.Empty;
        public int GroupId { get; set; }
    }
}
