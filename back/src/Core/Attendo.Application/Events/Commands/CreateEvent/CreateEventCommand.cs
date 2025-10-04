using MediatR;
using Attendo.Application.DTOs;

namespace Attendo.Application.Events.Commands.CreateEvent
{
    public class CreateEventCommand : IRequest<EventDto>
    {
        public DateTime Date { get; set; }
        public string Type { get; set; } = string.Empty;
        public int GroupId { get; set; }
    }
}
